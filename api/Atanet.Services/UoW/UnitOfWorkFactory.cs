namespace Atanet.Services.UoW
{
    using Atanet.Services.BusinessRules.Registry.Interfaces;

    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IBusinessRuleRegistry businessRuleRegistry;

        private readonly IConnectionStringBuilder connectionStringBuilder;

        public UnitOfWorkFactory(IConnectionStringBuilder connectionStringBuilder, IBusinessRuleRegistry businessRuleRegistry)
        {
            this.connectionStringBuilder = connectionStringBuilder;
            this.businessRuleRegistry = businessRuleRegistry;
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            var unitOfWork = new UnitOfWork(this.connectionStringBuilder, this.businessRuleRegistry);
            return unitOfWork;
        }
    }
}
