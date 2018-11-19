namespace Atanet.Validation.Dto.Comment
{
    using Atanet.Model.Dto;
    using Atanet.Validation.Common;

    public class CreateCommentDtoValidator : AbstractAtanetValidator<CreateCommentDto>
    {
        protected override void Initalize()
        {
            this.AddRuleForPostText(x => x.Text, 10, 100);
            this.AddRuleForLocation();
        }
    }
}
