namespace Atanet.Services.UoW
{
    using Atanet.Services.Repository;
    using System;

    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> CreateEntityRepository<T>();

        void Save();
    }
}
