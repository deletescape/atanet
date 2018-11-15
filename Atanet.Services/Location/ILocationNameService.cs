namespace Atanet.Services.Location
{
    using Atanet.Model.Interfaces;

    public interface ILocationNameService
    {
        long NameLocation(ILocatable locatable);
    }
}
