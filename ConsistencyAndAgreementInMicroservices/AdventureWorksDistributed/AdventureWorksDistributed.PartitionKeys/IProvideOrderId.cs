namespace AdventureWorksDistributed.PartitionKeys;

public interface IProvideOrderId
{
    Guid OrderId { get; }
}