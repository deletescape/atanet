namespace Atanet.Validation.Dto.Post
{
    using Atanet.Model.Dto;
    using Atanet.Validation.Common;

    public class FilterPostByQueryDtoValidator : AbstractAtanetValidator<FilterPostByQueryDto>
    {
        public FilterPostByQueryDtoValidator()
        {
        }

        protected override void Initalize()
        {
            this.AddRuleForPaging();
            this.AddRuleForQueryText(x => x.Query);
        }
    }
}
