namespace Atanet.Model.ApiResponse.HTTP403
{
    using Atanet.Model.Validation;

    public interface IForbiddenApiResult : IApiResult
    {
        long UserId { get; set; }

        AtanetEntityName AccessedEntityType { get; set; }

        long AccessedEntityId { get; set; }
    }
}
