namespace Atanet.Model.Dto
{
    using Atanet.Model.Interfaces;
    using Microsoft.AspNetCore.Http;

    public class CreatePostDto
    {
        public string Text { get; set; }

        public IFormFile Picture { get; set; }
    }
}
