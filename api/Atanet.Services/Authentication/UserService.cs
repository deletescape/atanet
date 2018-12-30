namespace Atanet.Services.Authentication
{
    using System.Threading.Tasks;
    using Atanet.Services.UoW;
    using System.Linq;
    using Microsoft.AspNetCore.Http;
    using Atanet.Model.Data;
    using Atanet.Services.Exceptions;
    using Atanet.Services.Common;
    using Microsoft.EntityFrameworkCore;
    using Atanet.Model.Validation;
    using Atanet.Services.ApiResult;
    using Atanet.Model.Dto;
    using AutoMapper;
    using Atanet.Services.Scoring;
    using System;

    public class UserService : IUserService
    {
        public const int LowScoreUser = -100;

        private readonly IHttpContextAccessor httpContextAccessor;

        private readonly IQueryService queryService;

        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        private readonly IPictureService pictureService;

        private readonly IApiResultService apiResultService;

        private readonly IScoreService scoreService;

        private readonly IMapper mapper;

        public UserService(
            IHttpContextAccessor httpContextAccessor,
            IQueryService queryService,
            IUnitOfWorkFactory unitOfWorkFactory,
            IPictureService pictureService,
            IApiResultService apiResultService,
            IScoreService scoreService,
            IMapper mapper)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.queryService = queryService;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.pictureService = pictureService;
            this.apiResultService = apiResultService;
            this.scoreService = scoreService;
            this.mapper = mapper;
        }

        public bool IsLowScoreUser(long userId)
        {
            return this.scoreService.CalculateUserScore(userId) < UserService.LowScoreUser;
        }

        public void UpdateProfilePicture(long userId)
        {
            using (var unitOfWork = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var userRepository = unitOfWork.CreateEntityRepository<User>();
                var user = userRepository.FindById(userId);
                user.Picture = this.pictureService.GetAtanetPicture(unitOfWork);
                userRepository.Update(user);
                unitOfWork.Save();
            }
        }

        public void DeleteUser(long userId)
        {
            var currentUserId = this.GetCurrentUserId();
            if (!this.scoreService.Can(AtanetAction.DeleteLowScoreUser, currentUserId))
            {
                var minScore = this.scoreService.GetMinScore(AtanetAction.DeleteLowScoreUser);
                throw new ApiException(this.apiResultService.BadRequestResult($"You can only delete low score users, if you have a score greater than {minScore}"));
            }

            if (currentUserId == userId)
            {
                // Easter egg: update profile picture if user tries to delete himself
                UpdateProfilePicture(currentUserId);
                return;
            }

            if (!this.IsLowScoreUser(userId))
            {
                throw new ApiException(this.apiResultService.BadRequestResult($"User is not a low score user ({LowScoreUser})"));
            }

            using (var unitOfWork = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var userRepository = unitOfWork.CreateEntityRepository<User>();
                userRepository.Delete(x => x.Id == userId);
                unitOfWork.Save();
            }
        }

        public ShowUserDto GetUserInfo(long userId)
        {
            var user = this.queryService.Query<User>().Include(x => x.Picture).FirstOrDefault(x => x.Id == userId);
            if (user == null)
            {
                throw new ApiException(this.apiResultService.NotFoundResult(AtanetEntityName.User, userId));
            }

            var currentUserId = this.GetCurrentUserId();
            if (currentUserId != userId && !this.scoreService.Can(AtanetAction.ViewUserProfile, currentUserId))
            {
                var minScore = this.scoreService.GetMinScore(AtanetAction.ViewUserProfile);
                throw new ApiException(this.apiResultService.BadRequestResult($"User must have a score greater than {minScore} in order to view user profiles"));
            }

            if (currentUserId == userId && !this.scoreService.Can(AtanetAction.ViewOwnUserProfile, currentUserId))
            {
                var minScore = this.scoreService.GetMinScore(AtanetAction.ViewOwnUserProfile);
                throw new ApiException(this.apiResultService.BadRequestResult($"You cannot view your own profile unless you have a score greater than {minScore}"));
            }

            var userDto = this.mapper.Map<ShowUserDto>(user);
            userDto.Capabilities = this.scoreService.GetUserCapabilities(userId).ToArray();
            userDto.Score = this.scoreService.CalculateUserScore(userId);
            userDto.Picture = new PictureDto
            {
                Base64Data = Convert.ToBase64String(user.Picture.Data),
                ContentType = user.Picture.ContentType
            };

            return userDto;
        }

        public File GetUserProfilePicture(long userId)
        {
            var userPicture = this.queryService.Query<User>().Include(x => x.Picture)
                .Where(x => x.Id == userId)
                .Select(x => x.Picture)
                .FirstOrDefault();
            if (userPicture == null)
            {
                throw new ApiException(this.apiResultService.NotFoundResult(AtanetEntityName.File, userId));
            }

            return userPicture;
        }

        public long GetCurrentUserId()
        {
            var subject = GetRequiredClaim("sub");
            var userId = this.queryService.Query<User>().Where(x => x.Subject == subject).Select(x => x.Id).FirstOrDefault();
            if (userId == default(long))
            {
                return this.CreateUser();
            }

            return userId;
        }

        private long CreateUser()
        {
            using (var unitOfWork = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var user = new User();
                user.Email = GetRequiredClaim("email");
                user.Subject = GetRequiredClaim("sub");
                user.Picture = this.pictureService.GetAtanetPicture(unitOfWork);
                var userRepository = unitOfWork.CreateEntityRepository<User>();
                userRepository.Create(user);
                unitOfWork.Save();
                return user.Id;
            }
        }

        private string GetRequiredClaim(string type)
        {
            var claim = this.httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == type);
            if (claim == null)
            {
                throw new ApiException(this.apiResultService.UnauthorizedResult());
            }

            return claim.Value;
        }
    }
}
