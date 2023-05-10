using System.Collections.Generic;

namespace OpenFTTH.AddressChangeIndexer;

internal interface IDatabaseAddressChangeIndex
{
    public void InitSchema();
    public void BulkInsert(ICollection<AddressChange> addressChanges);
}
