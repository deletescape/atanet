namespace Atanet.Validation.Dto.Account
{
    using FluentValidation;
    using Atanet.Model.Dto;
    using Atanet.Model.Validation;
    using Atanet.Validation.Common;
    using System.Linq;

    public class RegisterDtoValidator : AbstractAtanetValidator<RegisterDto>
    {
        protected override void Initalize()
        {
            this.AddRuleForNotNullOrEmpty(x => x.Email, AtanetEntityName.Account, PropertyName.Account.Email);
            this.AddRuleForNotNullOrEmpty(x => x.FirstName, AtanetEntityName.Account, PropertyName.Account.FirstName);
            this.AddRuleForNotNullOrEmpty(x => x.LastName, AtanetEntityName.Account, PropertyName.Account.LastName);
            this.AddRuleForNotNullOrEmpty(x => x.Password, AtanetEntityName.Account, PropertyName.Account.Password);
            this.RuleFor(x => x.Email).EmailAddress()
                .WithErrorCode(ErrorCode.Parse(ErrorCodeType.PropertyInvalidData, AtanetEntityName.Account, PropertyName.Account.Email).Code)
                .WithMessage("Please provide a valid mail address");
            this.AddRuleForMinLength(x => x.Password, 10, AtanetEntityName.Account, PropertyName.Account.Password);
            this.RuleFor(x => x.Password)
                .Must(x => x != null && x.Any(char.IsUpper) && x.Any(char.IsLower) && x.Any(char.IsDigit))
                .WithErrorCode(ErrorCode.Parse(ErrorCodeType.OutOfRange, AtanetEntityName.Account, PropertyName.Account.Password).Code)
                .WithMessage("Password must contain uppercase, lowercase and digit characters");
        }
    }
}
