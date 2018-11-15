namespace Atanet.Validation.Dto.Post
{
    using Atanet.Model.Dto;
    using Atanet.Validation.Common;

    public class FilterPostByLocationDtoValidator : AbstractAtanetValidator<FilterPostByLocationDto>
    {
        protected override void Initalize()
        {
            this.AddRuleForLocation();
            this.AddRuleForPaging();
        }
    }
}
