namespace Atanet.Validation.Dto.Post
{
    using Atanet.Model.Dto;
    using Atanet.Model.Validation;
    using Atanet.Validation.Common;
    using System;

    public class FilterPostByDateDtoValidator : AbstractAtanetValidator<FilterPostByDateDto>
    {
        protected override void Initalize()
        {
            this.AddRuleForPaging();
            this.AddRuleForDate(x => x.From, new DateTime(2016, 01, 01), DateTime.Now.AddYears(3), PropertyName.Post.From);
            this.AddRuleForDate(x => x.To, new DateTime(2016, 01, 01), DateTime.Now.AddYears(3), PropertyName.Post.To);
        }
    }
}
