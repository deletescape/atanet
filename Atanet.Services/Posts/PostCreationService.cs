namespace Atanet.Services.Posts
{
    using Atanet.Model.ApiResponse.HTTP400;
    using Atanet.Model.Data;
    using Atanet.Model.Dto;
    using Atanet.Model.Extensions;
    using Atanet.Model.Settings;
    using Atanet.Model.Validation;
    using Atanet.Services.Exceptions;
    using Atanet.Services.Location;
    using Atanet.Services.Tagging;
    using Atanet.Services.UoW;
    using System.Linq;

    public class PostCreationService : IPostCreationService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        private readonly IPostTaggingService postTaggingService;

        private readonly ILocationNameService locationNameService;

        private readonly AtanetSettings settings;

        public PostCreationService(
            IUnitOfWorkFactory unitOfWorkFactory,
            IPostTaggingService postTaggingService,
            ILocationNameService locationNameService,
            AtanetSettings settings)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.postTaggingService = postTaggingService;
            this.locationNameService = locationNameService;
            this.settings = settings;
        }

        public long CreatePost(CreatePostDto createPostDto)
        {
            using (var unitOfWork = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                if (createPostDto.FileId.HasValue)
                {
                    var fileRepository = unitOfWork.CreateEntityRepository<File>();
                    var file = fileRepository.Query().FirstOrDefault(x => x.Id == createPostDto.FileId.Value);
                    if (file == null)
                    {
                        throw new ApiException(x => x.BadRequestResult(
                            (ErrorCode.Parse(ErrorCodeType.InvalidReferenceId, AtanetEntityName.Post, PropertyName.Post.FileId, AtanetEntityName.File),
                            new ErrorDefinition(createPostDto.FileId, "The file was not found", PropertyName.Post.FileId))));
                    }
                }

                var repository = unitOfWork.CreateEntityRepository<Post>();
                var post = createPostDto.MapTo<Post>();
                post.Topic = this.PredictTopic(post.Text);
                var namedLocationId = this.locationNameService.NameLocation(post);
                post.LocationNameId = namedLocationId;
                repository.Create(post);
                unitOfWork.Save();
                return post.Id;
            }
        }

        private string PredictTopic(string text) =>
            this.postTaggingService.PredictTag(text);
    }
}
