namespace Atanet.Validation.Dto.Post
{
    using Atanet.Model.Dto;
    using Atanet.Validation.Common;

    public class PagedPostDtoValidator : AbstractAtanetValidator<PagedPostDto>
    {
        protected override void Initalize()
        {
            this.AddRuleForPaging();
            this.AddRuleForCommentAmount();
        }
    }
}
