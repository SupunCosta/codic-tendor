namespace UPrinceV4.Shared.Data;

public interface IMultitenantDbContext
{
    int TenantId { get; }
}