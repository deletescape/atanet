namespace Atanet.Services.Posts
{
    using Atanet.Model.ApiResponse.HTTP400;
    using Atanet.Model.Data;
    using Atanet.Model.Dto;
    using Atanet.Model.Extensions;
    using Atanet.Model.Settings;
    using Atanet.Model.Validation;
    using Atanet.Services.Exceptions;
    using Atanet.Services.UoW;
    using Atanet.Services.Files;
    using System.Linq;

    public class PostCreationService : IPostCreationService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        private readonly AtanetSettings settings;

        private readonly IFileCreationService fileCreationService;

        public PostCreationService(
            IUnitOfWorkFactory unitOfWorkFactory,
            AtanetSettings settings,
            IFileCreationService fileCreationService)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.settings = settings;
            this.fileCreationService = fileCreationService;
        }

        public long CreatePost(CreatePostDto createPostDto)
        {
            using (var unitOfWork = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var repository = unitOfWork.CreateEntityRepository<Post>();
                var post = createPostDto.MapTo<Post>();
                var picture = this.fileCreationService.CreateImageFile(unitOfWork, createPostDto.Picture);
                post.Picture = picture;
                repository.Create(post);
                unitOfWork.Save();
                return post.Id;
            }
        }
    }
}
