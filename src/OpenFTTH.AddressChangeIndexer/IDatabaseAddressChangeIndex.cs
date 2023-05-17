using System.Collections.Generic;

namespace OpenFTTH.AddressChangeIndexer;

internal interface IDatabaseAddressChangeIndex
{
    public long HighestSequenceNumber();
    public void InitSchema();
    public void BulkInsert(ICollection<AddressChange> addressChanges);
}
