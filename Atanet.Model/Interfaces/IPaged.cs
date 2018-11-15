namespace Atanet.Model.Interfaces
{
    public interface IPaged
    {
        int PageNumber { get; set; }

        int PageSize { get; set; }
    }
}
