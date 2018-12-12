namespace Atanet.Services.Posts
{
    using Atanet.Model.ApiResponse.HTTP400;
    using Atanet.Model.Data;
    using Atanet.Model.Dto;
    using Atanet.Model.Settings;
    using Atanet.Model.Validation;
    using Atanet.Services.Exceptions;
    using Atanet.Services.UoW;
    using Atanet.Services.Files;
    using System.Linq;
    using Microsoft.AspNetCore.Http;
    using Atanet.Services.Authentication;
    using Atanet.Services.Scoring;
    using Atanet.Services.ApiResult;
    using AutoMapper;

    public class PostCreationService : IPostCreationService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        private readonly IFileCreationService fileCreationService;

        private readonly IUserService userService;

        private readonly IScoreService scoreService;

        private readonly IApiResultService apiResultService;

        private readonly IMapper mapper;

        public PostCreationService(
            IUnitOfWorkFactory unitOfWorkFactory,
            IFileCreationService fileCreationService,
            IUserService userService,
            IScoreService scoreService,
            IApiResultService apiResultService,
            IMapper mapper)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.fileCreationService = fileCreationService;
            this.userService = userService;
            this.scoreService = scoreService;
            this.apiResultService = apiResultService;
            this.mapper = mapper;
        }

        public long CreatePost(CreatePostDto createPostDto)
        {
            var currentUserId = this.userService.GetCurrentUserId();
            if (!this.scoreService.Can(AtanetAction.CreatePost, currentUserId))
            {
                var minScore = this.scoreService.GetMinScore(AtanetAction.CreatePost);
                throw new ApiException(this.apiResultService.BadRequestResult($"User must have a score greater than {minScore} to create posts."));
            }

            using (var unitOfWork = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var repository = unitOfWork.CreateEntityRepository<Post>();
                var post = this.mapper.Map<Post>(createPostDto);
                var picture = this.fileCreationService.CreateImageFile(unitOfWork, createPostDto.Picture);
                post.Picture = picture;
                post.UserId = currentUserId; 
                // post.Sentiment = TODO: sentiment analysis
                repository.Create(post);
                unitOfWork.Save();
                return post.Id;
            }
        }
    }
}
