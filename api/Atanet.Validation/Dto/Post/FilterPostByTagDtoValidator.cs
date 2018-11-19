namespace Atanet.Validation.Dto.Post
{
    using Atanet.Model.Dto;
    using Atanet.Model.Validation;
    using Atanet.Validation.Common;

    public class FilterPostByTagDtoValidator : AbstractAtanetValidator<FilterPostByTagDto>
    {
        public FilterPostByTagDtoValidator()
        {
        }

        protected override void Initalize()
        {
            this.AddRuleForNotNullOrEmpty(x => x.Tags, AtanetEntityName.Post, PropertyName.Post.Topic);
            this.AddRuleForPaging();
        }
    }
}
