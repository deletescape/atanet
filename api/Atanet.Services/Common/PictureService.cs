namespace Atanet.Services.Common
{
    using System;
    using System.Net;
    using Atanet.Model.Data;
    using Atanet.Services.Files;
    using Atanet.Services.UoW;
    using ImageMagick;

    public class PictureService : IPictureService
    {
        private readonly IFileCreationService fileCreationService;

        private const string PictureSource = "https://atanet90.github.io/expression-pack/img/{0}.jpg";
        private const int MaxImages = 88241;
        private const int ImageSize = 128;

        private readonly Random random = new Random();

        public PictureService(IFileCreationService fileCreationService)
        {
            this.fileCreationService = fileCreationService;
        }

        public File GetAtanetPicture(IUnitOfWork unitOfWork)
        {
            using (var client = new WebClient())
            {
                var url = this.GetPictureUrl();
                var data = client.DownloadData(url);
                var fileName = System.IO.Path.GetFileName(url);

                using (var image = new MagickImage(data))
                {
                    image.Format = MagickFormat.Jpeg;

                    image.AdaptiveResize(ImageSize, ImageSize);
                    data = image.ToByteArray();
                    return this.fileCreationService.CreateImageFile(unitOfWork, data, fileName);
                }
            }
        }

        public string GetPictureUrl()
        {
            var pictureId = this.random.Next(0, MaxImages);
            var url = string.Format(PictureSource, pictureId.ToString());
            return url;
        }
    }
}
