namespace Atanet.Services.BusinessRule
{
    using Atanet.Model.Interfaces;
    using Atanet.Services.ApiResult;
    using Atanet.Services.Authentication;
    using Atanet.Services.BusinessRules;
    using Atanet.Services.Exceptions;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class UserBusinessRule : BusinessRuleBase<IUserCreatedEntity>
    {
        private readonly IApiResultService apiResultService;

        public UserBusinessRule(IApiResultService apiResultService)
        {
            this.apiResultService = apiResultService;
        }

        public override void PreSave(IList<IUserCreatedEntity> added, IList<IUserCreatedEntity> updated, IList<IUserCreatedEntity> removed)
        {
            foreach (var item in added.Concat(updated))
            {
                if (item.UserId == default(long))
                {
                    throw new ApiException(this.apiResultService.UnauthorizedResult());
                }
            }
        }
    }
}
