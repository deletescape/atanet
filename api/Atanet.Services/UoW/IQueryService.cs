namespace Atanet.Services.UoW
{
    using Atanet.Model.Interfaces;
    using System.Linq;

    public interface IQueryService
    {
        IQueryable<T> Query<T>() where T : class, IIdentifiable;
    }
}
