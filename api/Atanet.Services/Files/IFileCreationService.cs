namespace Atanet.Services.Files
{
    using Microsoft.AspNetCore.Http;
    using Atanet.Model.Dto;
    using Atanet.Services.UoW;
    using Atanet.Model.Data;

    public interface IFileCreationService
    {
        File CreateImageFile(IUnitOfWork unitOfWork, IFormFile formFile);

        File CreateImageFile(IUnitOfWork unitOfWork, byte[] data, string fileName);
    }
}
