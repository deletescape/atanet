namespace Atanet.Model.Dto
{
    using Atanet.Model.Interfaces;

    public class CreatePostDto
    {
        public string Text { get; set; }

        public long PictureId { get; set; }
    }
}
