namespace Atanet.Validation.Data.Post
{
    using Atanet.Model.Data;
    using Atanet.Validation.Common;

    public class PostValidator : AbstractAtanetValidator<Post>
    {
        protected override void Initalize()
        {
            this.AddRuleForPostText(x => x.Text, 10, 1000);
        }
    }
}
