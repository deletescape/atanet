namespace Atanet.Validation.Dto.File
{
    using FluentValidation;
    using Atanet.Model.Dto;
    using Atanet.Model.Validation;
    using Atanet.Validation.Common;
    using System;

    public class CreateLinkedFileDtoValidator : AbstractAtanetValidator<CreateLinkedFileDto>
    {
        protected override void Initalize()
        {
            this.RuleFor(x => x.Link)
                .Must(x => !string.IsNullOrEmpty(x) && Uri.TryCreate(x, UriKind.Absolute, out _))
                .WithErrorCode(ErrorCode.Parse(
                    ErrorCodeType.PropertyInvalidData,
                    AtanetEntityName.File,
                    PropertyName.File.Link).Code)
                .WithMessage("Please enter a valid URI");
        }
    }
}
