using OpenFTTH.Core.Address;

namespace OpenFTTH.AddressChangeIndexer.Tests;

public sealed class UnitAddressChangeConvertTest
{
    [Fact]
    public void Unit_address_created_to_address_change()
    {
        var unitAddressId = Guid.Parse("55113f86-b304-4ee8-945e-086a398f34ef");
        var eventId = Guid.Parse("6665e1a1-0de2-4038-a0d0-3e155cc0d7ef");
        var externalUpdated = DateTime.UtcNow;

        var expected = new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.AddressCreated,
            externalUpdated: externalUpdated,
            before: null,
            after: null);

        var result = UnitAddressChangeConvert.Created(
            unitAddressId: unitAddressId,
            eventId: eventId,
            externalUpdated: externalUpdated);

        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Unit_address_access_address_id_changed_to_address_change()
    {
        var unitAddressId = Guid.Parse("55113f86-b304-4ee8-945e-086a398f34ef");
        var eventId = Guid.Parse("6665e1a1-0de2-4038-a0d0-3e155cc0d7ef");
        var externalUpdated = DateTime.UtcNow;
        var accessAddressIdBefore = Guid.Parse("01aed8c7-28b5-4e77-ac6b-80dd84df921f");
        var accessAddressIdAfter = Guid.Parse("6a1b141c-c2d9-4eb9-bd6f-8c73171a92ab");

        var expected = new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.AccessAddressIdChanged,
            externalUpdated: externalUpdated,
            before: accessAddressIdBefore.ToString(),
            after: accessAddressIdAfter.ToString());

        var result = UnitAddressChangeConvert.AccessAddressIdChanged(
            unitAddressId: unitAddressId,
            eventId: eventId,
            externalUpdated: externalUpdated,
            accessAddressIdBefore: accessAddressIdBefore,
            accessAddressIdAfter: accessAddressIdAfter);

        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Unit_address_status_changed_to_address_change()
    {
        var unitAddressId = Guid.Parse("55113f86-b304-4ee8-945e-086a398f34ef");
        var eventId = Guid.Parse("6665e1a1-0de2-4038-a0d0-3e155cc0d7ef");
        var externalUpdated = DateTime.UtcNow;
        var statusBefore = UnitAddressStatus.Pending;
        var statusAfter = UnitAddressStatus.Active;

        var expected = new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.StatusChanged,
            externalUpdated: externalUpdated,
            before: Enum.GetName(typeof(UnitAddressStatus), statusBefore),
            after: Enum.GetName(typeof(UnitAddressStatus), statusAfter));

        var result = UnitAddressChangeConvert.StatusChanged(
            unitAddressId: unitAddressId,
            eventId: eventId,
            externalUpdated: externalUpdated,
            statusBefore: statusBefore,
            statusAfter: statusAfter);

        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Unit_address_floor_name_changed_to_address_change()
    {
        var unitAddressId = Guid.Parse("55113f86-b304-4ee8-945e-086a398f34ef");
        var eventId = Guid.Parse("6665e1a1-0de2-4038-a0d0-3e155cc0d7ef");
        var externalUpdated = DateTime.UtcNow;
        var floorNameBefore = "2b";
        var floorNameAfter = "3b";

        var expected = new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.FloorNameChanged,
            externalUpdated: externalUpdated,
            before: floorNameBefore,
            after: floorNameAfter);

        var result = UnitAddressChangeConvert.FloorNameChanged(
            unitAddressId: unitAddressId,
            eventId: eventId,
            externalUpdated: externalUpdated,
            floorNameBefore: floorNameBefore,
            floorNameAfter: floorNameAfter);

        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Unit_address_suite_name_changed_to_address_change()
    {
        var unitAddressId = Guid.Parse("55113f86-b304-4ee8-945e-086a398f34ef");
        var eventId = Guid.Parse("6665e1a1-0de2-4038-a0d0-3e155cc0d7ef");
        var externalUpdated = DateTime.UtcNow;
        string? suiteNameBefore = null;
        var suiteNameAfter = "mf";

        var expected = new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.SuiteNameChanged,
            externalUpdated: externalUpdated,
            before: suiteNameBefore,
            after: suiteNameAfter);

        var result = UnitAddressChangeConvert.SuiteNameChanged(
            unitAddressId: unitAddressId,
            eventId: eventId,
            externalUpdated: externalUpdated,
            suiteNameBefore: suiteNameBefore,
            suiteNameAfter: suiteNameAfter);

        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Unit_address_deleted_to_address_change()
    {
        var unitAddressId = Guid.Parse("55113f86-b304-4ee8-945e-086a398f34ef");
        var eventId = Guid.Parse("6665e1a1-0de2-4038-a0d0-3e155cc0d7ef");
        var externalUpdated = DateTime.UtcNow;

        var expected = new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.AddressDeleted,
            externalUpdated: externalUpdated,
            before: null,
            after: null);

        var result = UnitAddressChangeConvert.Deleted(
            unitAddressId: unitAddressId,
            eventId: eventId,
            externalUpdated: externalUpdated);

        result.Should().BeEquivalentTo(expected);
    }
}
