namespace Atanet.Model.ApiResponse.HTTP404
{
    using Atanet.Model.Validation;

    public interface INotFoundApiResult : IApiResult
    {
        AtanetEntityName AccessedEntityType { get; set; }

        long AccessedEntityId { get; set; }
    }
}
