namespace Atanet.Services.UoW
{
    public interface IConnectionStringBuilder
    {
        string ConstructConnectionStringFromEnvironment();
    }
}
