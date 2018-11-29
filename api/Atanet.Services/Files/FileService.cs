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

    public class FileService : IFileService
    {
        private readonly IQueryService queryService;

        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public FileService(IQueryService queryService, IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.queryService = queryService;
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public long CreateFile(IFormFile formFile)
        {
            if (formFile == null)
            {
                throw new ApiException(x => x.BadRequestResult(
                    (ErrorCode.Parse(ErrorCodeType.PropertyDataNullOrEmpty, AtanetEntityName.File, PropertyName.File.Id),
                    new ErrorDefinition(null, "Invalid file", PropertyName.File.Id))));
            }

            using (var unitOfWork = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var fileRepository = unitOfWork.CreateEntityRepository<File>();
                var file = new File
                {
                    ContentType = formFile.ContentType,
                    FileName = formFile.FileName
                };

                using (var stream = formFile.OpenReadStream())
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        var buffer = new byte[16 * 1024];
                        var read = 0;
                        while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            memoryStream.Write(buffer, 0, read);
                        }

                        file.Data = memoryStream.ToArray();
                    }
                }

                fileRepository.Create(file);
                unitOfWork.Save();
                return file.Id;
            }
        }

        public FileDto GetFile(long id)
        {
            var file = this.Get(id);
            return file == null ? null : Mapper.Map<FileDto>(file);
        }

        public FileInfoDto GetFileInfoForPost(long postId)
        {
            var post = this.queryService.Query<Post>().FirstOrDefault(x => x.Id == postId);
            if (post == null)
            {
                throw new ApiException(x => x.BadRequestResult(
                    (ErrorCode.Parse(ErrorCodeType.InvalidReferenceId, AtanetEntityName.Post, PropertyName.Post.Id),
                    new ErrorDefinition(postId, "The post was not found", PropertyName.Post.Id))));
            }

            var file = this.queryService.Query<File>().FirstOrDefault(x => x.Id == post.PictureId);
            return Mapper.Map<FileInfoDto>(file);
        }

        public FileInfoDto GetFileInfo(long id)
        {
            var file = this.Get(id);
            return file == null ? null : Mapper.Map<FileInfoDto>(file);
        }

        private File Get(long fileId)
        {
            var file = this.queryService.Query<File>().FirstOrDefault(x => x.Id == fileId);
            if (file == null)
            {
                throw new ApiException(x => x.NotFoundResult(AtanetEntityName.File, fileId));
            }

            return file;
        }

        private void FillLinkedFile(string link, out string contentType, out string fileName)
        {
            if (!Uri.TryCreate(link, UriKind.Absolute, out Uri uri))
            {
                throw new ApiException(x => x.BadRequestResult(
                    (ErrorCode.Parse(ErrorCodeType.PropertyInvalidData, AtanetEntityName.File, PropertyName.File.Link),
                    new ErrorDefinition(link, "Please enter a valid link", PropertyName.File.Link))));
            }

            try
            {
                var webRequest = WebRequest.Create(uri);
                webRequest.Method = "HEAD";
                var response = webRequest.GetResponse();
                contentType = response.Headers.Get("Content-Type");
                fileName = Path.GetFileName(uri.LocalPath);
            }
            catch (Exception)
            {
                throw new ApiException(x => x.BadRequestResult(
                    (ErrorCode.Parse(ErrorCodeType.PropertyInvalidData, AtanetEntityName.File, PropertyName.File.Link),
                    new ErrorDefinition(link, "No file found at the given URL", PropertyName.File.Link))));
            }
        }
    }
}
