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
    using Atanet.Services.ApiResult;

    public class FileCreationService : IFileCreationService
    {
        private readonly IQueryService queryService;

        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        private readonly IApiResultService apiResultService;

        private const string WebPContentType = "image/webp";

        public FileCreationService(IQueryService queryService, IUnitOfWorkFactory unitOfWorkFactory, IApiResultService apiResultService)
        {
            this.queryService = queryService;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.apiResultService = apiResultService;
        }

        public File CreateImageFile(IUnitOfWork unitOfWork, byte[] data, string fileName)
        {
            if (data == null || data.Length == 0)
            {
                this.ThrowInvalidFile();
            }

            using (var memoryStream = new MemoryStream(data))
            {
                var bytes = this.CompressAndFormat(memoryStream);
                return this.CreateFile(unitOfWork, fileName, bytes);
            }
        }

        public File CreateImageFile(IUnitOfWork unitOfWork, IFormFile formFile)
        {
            if (formFile == null)
            {
                this.ThrowInvalidFile();
            }

            using (var stream = formFile.OpenReadStream())
            {
                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    var bytes = this.CompressAndFormat(memoryStream);
                    return this.CreateFile(unitOfWork, formFile.FileName, bytes);
                }
            }
        }

        private void ThrowInvalidFile()
        {
            throw new ApiException(this.apiResultService.BadRequestResult(
                (ErrorCode.Parse(ErrorCodeType.PropertyDataNullOrEmpty, AtanetEntityName.File, PropertyName.File.Id),
                new ErrorDefinition(null, "Invalid or empty file", PropertyName.File.Id))));
        }

        private File CreateFile(IUnitOfWork unitOfWork, string fileName, byte[] data)
        {
            var fileRepository = unitOfWork.CreateEntityRepository<File>();
            var file = new File
            {
                ContentType = WebPContentType,
                FileName = fileName
            };

            file.Data = data;
            fileRepository.Create(file);
            return file;
        }

        private byte[] CompressAndFormat(MemoryStream stream)
        {
            try
            {
                var compressor = new ImageOptimizer();
                stream.Position = 0;
                var compressed = compressor.Compress(stream);
                if (!compressed)
                {
                    throw new ApiException(this.apiResultService.BadRequestResult(
                        (ErrorCode.Parse(ErrorCodeType.PropertyInvalidData, AtanetEntityName.File, PropertyName.File.Id),
                        new ErrorDefinition(null, "File could not be compressed", PropertyName.File.Id))));
                }

                var compressedBytes = stream.ToArray();
                using (var image = new MagickImage(compressedBytes))
                {
                    var imageOptimizer = new ImageOptimizer();
                    return image.ToByteArray(MagickFormat.WebP);
                }
            }
            catch (MagickCorruptImageErrorException)
            {
                throw new ApiException(this.apiResultService.BadRequestResult("Invalid file provided"));
            }
        }
    }
}
