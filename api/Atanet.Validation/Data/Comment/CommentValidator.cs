namespace Atanet.Validation.Data.Comment
{
    using Atanet.Model.Data;
    using Atanet.Validation.Common;

    public class CommentValidator : AbstractAtanetValidator<Comment>
    {
        protected override void Initalize()
        {
            this.AddRuleForPostText(x => x.Text, 10, 100);
        }
    }
}
