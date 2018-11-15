namespace Atanet.Services.Posts
{
    using Atanet.Model.Dto;

    public interface IPostCreationService
    {
        long CreatePost(CreatePostDto createPostDto);
    }
}
