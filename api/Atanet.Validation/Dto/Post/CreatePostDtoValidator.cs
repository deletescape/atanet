namespace Atanet.Validation.Dto.Newsletter
{
    using Atanet.Model.Dto;
    using Atanet.Validation.Common;

    public class CreatePostDtoValidator : AbstractAtanetValidator<CreatePostDto>
    {
        protected override void Initalize()
        {
            this.AddRuleForPostText(x => x.Text, 10, 1000);
            this.AddRuleForLocation();
        }
    }
}
