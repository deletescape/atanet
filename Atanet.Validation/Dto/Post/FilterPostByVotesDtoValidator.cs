namespace Atanet.Validation.Dto.Post
{
    using Atanet.Model.Dto;
    using Atanet.Validation.Common;

    public class FilterPostByVotesDtoValidator : AbstractAtanetValidator<PagedDto>
    {
        protected override void Initalize()
        {
            this.AddRuleForPaging();
        }
    }
}
