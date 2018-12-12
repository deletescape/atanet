namespace Atanet.Validation.Dto.Newsletter
{
    using Atanet.Model.Dto;
    using Atanet.Model.Validation;
    using Atanet.Validation.Common;
    using FluentValidation;

    public class CreateReactionDtoValidator : AbstractAtanetValidator<CreateReactionDto>
    {
        protected override void Initalize()
        {
            this.RuleFor(x => x.ReactionState)
                .Must(x => x != null)
                .WithErrorCode(ErrorCode.Parse(
                    ErrorCodeType.PropertyDataNullOrEmpty,
                    AtanetEntityName.Reaction,
                    PropertyName.Reaction.ReactionState).Code)
                .WithMessage("Reaction state must be provided");
        }
    }
}
