namespace Atanet.Services.Assembly
{
    using System.Reflection;

    public interface IAssemblyContainer
    {
        Assembly[] GetAssemblies();
    }
}
