namespace Atanet.Validation.Common
{
    using FluentValidation;
    using Atanet.Model.Data;
    using Atanet.Model.Dto;
    using Atanet.Model.Interfaces;
    using Atanet.Model.Validation;
    using System;
    using System.Linq.Expressions;

    public static class CommonValidators
    {
        public static void AddRuleForCommentAmount<T>(
            this AbstractAtanetValidator<T> validator) where T : PagedPostDto
        {
            validator.RuleFor(x => x.CommentNumber)
                .Must(x => x >= 0 && x <= 100)
                .WithErrorCode(ErrorCode.Parse(
                    ErrorCodeType.PropertyInvalidData,
                    AtanetEntityName.Post,
                    PropertyName.Filter.CommentAmount).Code)
                .WithMessage("Cannot fetch less than 0 or more than 100 comments on a post.");
        }

        public static void AddRuleForPostText<T>(
            this AbstractAtanetValidator<T> validator,
            Expression<Func<T, string>> textSelector,
            int min,
            int max)
        {
            validator.RuleFor(textSelector)
                .Must(x => x != null && !string.IsNullOrEmpty(x.Trim()))
                .WithErrorCode(ErrorCode.Parse(
                    ErrorCodeType.PropertyDataNullOrEmpty,
                    AtanetEntityName.Post,
                    PropertyName.Post.Text).Code)
                .WithMessage("Text cannot be empty");
            validator.RuleFor(textSelector)
                .Must(x => x != null && x.Trim().Length >= min)
                .WithErrorCode(ErrorCode.Parse(
                    ErrorCodeType.PropertyInvalidData,
                    AtanetEntityName.Post,
                    PropertyName.Post.Text).Code)
                .WithMessage("Text too short");
            validator.RuleFor(textSelector)
                .Must(x => x != null && x.Trim().Length <= max)
                .WithErrorCode(ErrorCode.Parse(
                    ErrorCodeType.OutOfRange,
                    AtanetEntityName.Post,
                    PropertyName.Post.Text).Code)
                .WithMessage("Text too long");
        }

        public static void AddRuleForQueryText<T>(
            this AbstractAtanetValidator<T> validator,
            Expression<Func<T, string>> property)
        {
            validator.RuleFor(property)
                .Must(x => !string.IsNullOrWhiteSpace(x))
                .WithErrorCode(ErrorCode.Parse(
                    ErrorCodeType.PropertyDataNullOrEmpty,
                    AtanetEntityName.Post,
                    PropertyName.Post.Query).Code)
                .WithMessage("Cannot filter for empty string");
            validator.RuleFor(property)
                .Must(x => !string.IsNullOrWhiteSpace(x) && x.Length >= 2)
                .WithErrorCode(ErrorCode.Parse(
                    ErrorCodeType.TooShort,
                    AtanetEntityName.Post,
                    PropertyName.Post.Query).Code)
                .WithMessage("Please provide a query longer than a characters");
        }

        public static void AddRuleForDate<T>(
            this AbstractAtanetValidator<T> validator,
            Expression<Func<T, DateTime?>> dateTimeSelector,
            DateTime min,
            DateTime max,
            PropertyName propertyName)
        {
            validator.RuleFor(dateTimeSelector)
                .Must(x => x.HasValue && x.Value >= min && x.Value <= max)
                .WithErrorCode(ErrorCode.Parse(
                    ErrorCodeType.PropertyInvalidData,
                    AtanetEntityName.Post,
                    propertyName).Code)
                .WithMessage("Please provide a valid date between 2016 and 3 years in the future from now");
        }

        public static void AddRuleForPaging<T>(
            this AbstractAtanetValidator<T> validator) where T : IPaged
        {
            validator.RuleFor(x => x.PageNumber)
                .Must(x => x >= 0)
                .WithErrorCode(ErrorCode.Parse(
                    ErrorCodeType.PropertyInvalidData,
                    AtanetEntityName.Post,
                    PropertyName.Post.Id).Code)
                .WithMessage("Page number must be non-negative");
            validator.RuleFor(x => x.PageSize)
                .Must(x => x >= 5 && x <= 100)
                .WithErrorCode(ErrorCode.Parse(
                    ErrorCodeType.OutOfRange,
                    AtanetEntityName.Post,
                    PropertyName.Post.Id).Code)
                .WithMessage("Page size must be between 5 and 100 items");
        }

        public static void AddRuleForMinLength<T>(
            this AbstractAtanetValidator<T> validator,
            Expression<Func<T, string>> property,
            int minimumLength,
            AtanetEntityName entity,
            PropertyName name)
        {
            validator.RuleFor(property)
                .MinimumLength(minimumLength)
                .WithErrorCode(ErrorCode.Parse(
                    ErrorCodeType.TooShort,
                    entity,
                    name).Code)
                .WithMessage($"{name.Name} must be at least {minimumLength} characters long");
        }

        public static void AddRuleForNotNullOrEmpty<T>(
            this AbstractAtanetValidator<T> validator,
            Expression<Func<T, string>> property,
            AtanetEntityName entity,
            PropertyName name)
        {
            validator.RuleFor(property)
                .Must(x => !string.IsNullOrEmpty(x))
                .WithErrorCode(ErrorCode.Parse(
                    ErrorCodeType.PropertyDataNullOrEmpty,
                    entity,
                    name).Code)
                .WithMessage($"{name.Name} cannot be null or empty");
        }
    }
}
