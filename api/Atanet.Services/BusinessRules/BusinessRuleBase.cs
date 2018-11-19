namespace Atanet.Services.BusinessRules
{
    using Atanet.Services.BusinessRules.Interfaces;
    using Atanet.Services.UoW;
    using System.Collections.Generic;
    using System.Linq;

    public class BusinessRuleBase<TEntity> : IBusinessRuleBase
    {
        public virtual void PostSave(IUnitOfWork unitOfWork)
        {
        }

        public virtual void PreSave(IList<TEntity> added, IList<TEntity> updated, IList<TEntity> removed)
        {
        }

        public void PreSave(IList<object> added, IList<object> updated, IList<object> removed) =>
            this.PreSave(added.Cast<TEntity>().ToList(), updated.Cast<TEntity>().ToList(), removed.Cast<TEntity>().ToList());
    }
}
