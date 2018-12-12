namespace Atanet.Validation.Common
{
    using FluentValidation;
    using Atanet.Model.Data;
    using Atanet.Model.Dto;
    using Atanet.Model.Interfaces;
    using Atanet.Model.Validation;
    using System;
    using System.Linq.Expressions;
    using Microsoft.AspNetCore.Http;

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

        public static void AddRuleForImageFile<T>(
            this AbstractAtanetValidator<T> validator,
            Expression<Func<T, IFormFile>> property)
        {
            validator.RuleFor(property)
                .Must(x => x != null)
                .WithErrorCode(ErrorCode.Parse(
                    ErrorCodeType.PropertyDataNullOrEmpty,
                    AtanetEntityName.File,
                    PropertyName.File.Data).Code)
                .WithMessage("No file provided");
            validator.RuleFor(property)
                .Must(x => x.ContentType.StartsWith("image/"))
                .WithErrorCode(ErrorCode.Parse(
                    ErrorCodeType.PropertyInvalidData,
                    AtanetEntityName.File,
                    PropertyName.File.ContentType).Code)
                .WithMessage("Must provide an image");
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
    }
}
