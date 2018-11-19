namespace Atanet.Validation.Dto.Newsletter
{
    using Atanet.Model.Dto;
    using Atanet.Validation.Common;

    public class PostDtoValidator : AbstractAtanetValidator<PostDto>
    {
        protected override void Initalize()
        {
            this.AddRuleForPostText(x => x.Text, 10, 1000);
        }
    }
}
