namespace Atanet.Services.Authentication
{
    using System.Threading.Tasks;
    using Atanet.Model.Data;
    using Atanet.Model.Dto;

    public interface IUserService
    {
        ShowUserDto GetUserInfo(long userId);

        long GetCurrentUserId();

        File GetUserProfilePicture(long userId);

        void DeleteUser(long userId);
    }
}
