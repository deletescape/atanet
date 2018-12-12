namespace Atanet.Services.Common
{
    using Atanet.Model.Data;
    using Atanet.Services.UoW;

    public interface IPictureService
    {
        File GetAtanetPicture(IUnitOfWork unitOfWork);

        string GetPictureUrl();
    }
}
