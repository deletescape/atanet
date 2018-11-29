namespace Atanet.Services.Files
{
    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using Atanet.Model.ApiResponse.HTTP400;
    using Atanet.Model.Data;
    using Atanet.Model.Dto;
    using Atanet.Model.Validation;
    using Atanet.Services.Exceptions;
    using Atanet.Services.UoW;
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using File = Model.Data.File;
    using ImageMagick;

    public class FileCreationService : IFileCreationService
    {
        private readonly IQueryService queryService;

        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public FileCreationService(IQueryService queryService, IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.queryService = queryService;
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public File CreateImageFile(IUnitOfWork unitOfWork, IFormFile formFile)
        {
            if (formFile == null)
            {
                throw new ApiException(x => x.BadRequestResult(
                    (ErrorCode.Parse(ErrorCodeType.PropertyDataNullOrEmpty, AtanetEntityName.File, PropertyName.File.Id),
                    new ErrorDefinition(null, "Invalid file", PropertyName.File.Id))));
            }

            var bytes = default(byte[]);
            using (var stream = formFile.OpenReadStream())
            {
                using (var image = new MagickImage(stream))
                {
                    var imageOptimizer = new ImageOptimizer();
                    bytes = image.ToByteArray(MagickFormat.WebP);
                }

                using (var webPMemoryStream = new MemoryStream(bytes))
                {
                    var compressor = new ImageOptimizer();
                    var compressed = compressor.Compress(webPMemoryStream);
                    if (!compressed)
                    {
                        throw new ApiException(x => x.BadRequestResult(
                            (ErrorCode.Parse(ErrorCodeType.PropertyInvalidData, AtanetEntityName.File, PropertyName.File.Id),
                            new ErrorDefinition(null, "File could not be compressed", PropertyName.File.Id))));
                    }

                    bytes = webPMemoryStream.ToArray();
                }
            }

            var fileRepository = unitOfWork.CreateEntityRepository<File>();
            var file = new File
            {
                ContentType = formFile.ContentType,
                FileName = formFile.FileName
            };

            file.Data = bytes;
            fileRepository.Create(file);
            return file;
        }
    }
}
