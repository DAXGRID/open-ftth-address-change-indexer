using Npgsql;
using NpgsqlTypes;
using System.Collections.Generic;

namespace OpenFTTH.AddressChangeIndexer;

internal class PostgresDatabaseAddressChangeIndex : IDatabaseAddressChangeIndex
{
    private readonly Setting _setting;

    public PostgresDatabaseAddressChangeIndex(Setting setting)
    {
        _setting = setting;
    }

    /// <summary>
    /// Creates the schema for address changes if it does not already exists.
    /// </summary>
    public void InitSchema()
    {
        const string INIT_SCHEMA_SQL = @"
CREATE TABLE IF NOT EXISTS location.address_changes(
  unit_address_id uuid NOT NULL,
  event_id uuid NOT NULL,
  sequence_number bigint NOT NULL,
  change_type varchar(255) NOT NULL,
  external_updated timestamptz NULL,
  before varchar(4096) NULL,
  after varchar(4096) NULL,
  PRIMARY KEY(unit_address_id, event_id))";

        using var connection = new NpgsqlConnection(_setting.GeoDatabaseConnectionString);
        using var cmd = new NpgsqlCommand(INIT_SCHEMA_SQL, connection);
        connection.Open();
        cmd.ExecuteNonQuery();
    }

    public void BulkInsert(ICollection<AddressChange> addressChanges)
    {
        const string ADDRESS_CHANGES_COPY_SQL = @"
COPY location.address_changes(
  unit_address_id,
  event_id,
  sequence_number,
  change_type,
  external_updated,
  before,
  after) FROM STDIN (FORMAT BINARY)";

        using var connection = new NpgsqlConnection(_setting.GeoDatabaseConnectionString);
        connection.Open();

        using (var writer = connection.BeginBinaryImport(ADDRESS_CHANGES_COPY_SQL))
        {
            foreach (var addressChange in addressChanges)
            {
                writer.WriteRow(
                    addressChange.UnitAddressId,
                    addressChange.EventId,
                    addressChange.SequenceNumber,
                    addressChange.ChangeType.ToString(),
                    addressChange.ExternalUpdated is not null ? addressChange.ExternalUpdated : DBNull.Value,
                    addressChange.Before is not null ? addressChange.Before : DBNull.Value,
                    addressChange.After is not null ? addressChange.After : DBNull.Value);
            }

            writer.Complete();
        }
    }
}
