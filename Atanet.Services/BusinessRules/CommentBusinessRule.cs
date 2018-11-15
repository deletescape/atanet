namespace Atanet.Services.BusinessRules
{
    using Microsoft.EntityFrameworkCore;
    using Atanet.Model.ApiResponse.HTTP400;
    using Atanet.Model.Data;
    using Atanet.Model.Validation;
    using Atanet.Services.Exceptions;
    using Atanet.Services.UoW;
    using System.Collections.Generic;
    using System.Linq;

    public class CommentBusinessRule : BusinessRuleBase<Comment>
    {
        private readonly IQueryService queryService;

        public CommentBusinessRule(IQueryService queryService) =>
            this.queryService = queryService;

        public override void PreSave(IList<Comment> added, IList<Comment> updated, IList<Comment> removed)
        {
            foreach (var item in added)
            {
                if (!queryService.Query<Post>().AsNoTracking().Any(x => x.Id == item.PostId))
                {
                    throw new ApiException(x => x.BadRequestResult(
                        (ErrorCode.Parse(ErrorCodeType.InvalidReferenceId, AtanetEntityName.Comment, PropertyName.Comment.PostId, AtanetEntityName.Post),
                        new ErrorDefinition(item.PostId, "The given post does not exist", PropertyName.Comment.PostId))));
                }
            }
        }
    }
}
