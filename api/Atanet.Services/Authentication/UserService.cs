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

    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        private readonly IQueryService queryService;

        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        private readonly IPictureService pictureService;

        private readonly IApiResultService apiResultService;

        private readonly IMapper mapper;

        public UserService(
            IHttpContextAccessor httpContextAccessor,
            IQueryService queryService,
            IUnitOfWorkFactory unitOfWorkFactory,
            IPictureService pictureService,
            IApiResultService apiResultService,
            IMapper mapper)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.queryService = queryService;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.pictureService = pictureService;
            this.apiResultService = apiResultService;
            this.mapper = mapper;
        }

        public ShowUserDto GetUserInfo(long userId)
        {
            var user = this.queryService.Query<User>().FirstOrDefault(x => x.Id == userId);
            if (user == null)
            {
                throw new ApiException(this.apiResultService.NotFoundResult(AtanetEntityName.User, userId));
            }

            return this.mapper.Map<ShowUserDto>(user);
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
