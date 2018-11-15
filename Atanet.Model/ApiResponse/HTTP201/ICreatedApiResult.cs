namespace Atanet.Model.ApiResponse.HTTP201
{
    using Atanet.Model.Validation;

    public interface ICreatedApiResult : IApiResult
    {
        AtanetEntityName Entity { get; set; }

        long CreatedId { get; set; }
    }
}
