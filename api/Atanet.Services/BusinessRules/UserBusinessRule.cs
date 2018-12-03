namespace Atanet.Services.BusinessRule
{
    using Atanet.Model.Interfaces;
    using Atanet.Services.Authentication;
    using Atanet.Services.BusinessRules;
    using System;
    using System.Collections.Generic;

    public class UserBusinessRule : BusinessRuleBase<IUserCreatedEntity>
    {
        private readonly IUserService userService;

        public UserBusinessRule(IUserService userService)
        {
            this.userService = userService;
        }        

        public override void PreSave(IList<IUserCreatedEntity> added, IList<IUserCreatedEntity> updated, IList<IUserCreatedEntity> removed)
        {
            var currentUserId = this.userService.GetCurrentUserId();
            foreach (var addedItem in added)
            {
                addedItem.UserId = currentUserId;
            }
        }
    }
}
