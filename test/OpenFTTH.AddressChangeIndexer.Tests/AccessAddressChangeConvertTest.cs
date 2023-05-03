using OpenFTTH.Core.Address;

namespace OpenFTTH.AddressChangeIndexer.Tests;

public sealed class AccessAddressChangeConvertTest
{
    [Fact]
    public void Municipal_code_changed_to_address_change()
    {
        var unitAddressId = Guid.Parse("55113f86-b304-4ee8-945e-086a398f34ef");
        var eventId = Guid.Parse("6665e1a1-0de2-4038-a0d0-3e155cc0d7ef");
        var externalUpdated = DateTime.UtcNow;
        var municipalCodeBefore = "AD12";
        var municipalCodeAfter = "DF12";

        var expected = new AddressChange(
                unitAddressId: unitAddressId,
                eventId: eventId,
                changeType: AddressChangeType.AccessAddressMunicipalCodeChanged,
                externalUpdated: externalUpdated,
                before: municipalCodeBefore,
                after: municipalCodeAfter);

        var result = AccessAddressChangeConvert.MunicipalCodeChanged(
            unitAddressId: unitAddressId,
            eventId: eventId,
            externalUpdated: externalUpdated,
            municipalCodeBefore: municipalCodeBefore,
            municipalCodeAfter: municipalCodeAfter);

        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Status_changed_to_address_change()
    {
        var unitAddressId = Guid.Parse("55113f86-b304-4ee8-945e-086a398f34ef");
        var eventId = Guid.Parse("6665e1a1-0de2-4038-a0d0-3e155cc0d7ef");
        var externalUpdated = DateTime.UtcNow;
        var statusBefore = AccessAddressStatus.Pending;
        var statusAfter = AccessAddressStatus.Active;

        var expected = new AddressChange(
                unitAddressId: unitAddressId,
                eventId: eventId,
                changeType: AddressChangeType.AccessAddressStatusChanged,
                externalUpdated: externalUpdated,
                before: "Pending",
                after: "Active");

        var result = AccessAddressChangeConvert.StatusChanged(
            unitAddressId: unitAddressId,
            eventId: eventId,
            externalUpdated: externalUpdated,
            statusBefore: statusBefore,
            statusAfter: statusAfter);

        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Road_code_changed_to_address_change()
    {
        var unitAddressId = Guid.Parse("55113f86-b304-4ee8-945e-086a398f34ef");
        var eventId = Guid.Parse("6665e1a1-0de2-4038-a0d0-3e155cc0d7ef");
        var externalUpdated = DateTime.UtcNow;
        var roadCodeBefore = "0101";
        var roadCodeAfter = "0102";

        var expected = new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.AccessAddressRoadCodeChanged,
            externalUpdated: externalUpdated,
            before: roadCodeBefore,
            after: roadCodeAfter);

        var result = AccessAddressChangeConvert.RoadCodeChanged(
            unitAddressId: unitAddressId,
            eventId: eventId,
            externalUpdated: externalUpdated,
            roadCodeBefore: roadCodeBefore,
            roadCodeAfter: roadCodeAfter);

        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void House_number_changed_to_address_change()
    {
        var unitAddressId = Guid.Parse("55113f86-b304-4ee8-945e-086a398f34ef");
        var eventId = Guid.Parse("6665e1a1-0de2-4038-a0d0-3e155cc0d7ef");
        var externalUpdated = DateTime.UtcNow;
        var houseNumberBefore = "10B";
        var houseNumberAfter = "11F";

        var expected = new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.AccessAddressHouseNumberChanged,
            externalUpdated: externalUpdated,
            before: houseNumberBefore,
            after: houseNumberAfter);

        var result = AccessAddressChangeConvert.HouseNumberChanged(
            unitAddressId: unitAddressId,
            eventId: eventId,
            externalUpdated: externalUpdated,
            houseNumberBefore: houseNumberBefore,
            houseNumberAfter: houseNumberAfter);

        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Supplementary_town_name_changed_to_address_change()
    {
        var unitAddressId = Guid.Parse("55113f86-b304-4ee8-945e-086a398f34ef");
        var eventId = Guid.Parse("6665e1a1-0de2-4038-a0d0-3e155cc0d7ef");
        var externalUpdated = DateTime.UtcNow;
        var supplementaryTownNameBefore = "Old town name";
        var supplementaryTownNameAfter = "New town name";

        var expected = new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.AccessAddressSupplementaryTownNameChanged,
            externalUpdated: externalUpdated,
            before: supplementaryTownNameBefore,
            after: supplementaryTownNameAfter);

        var result = AccessAddressChangeConvert.SupplementaryTownNameChanged(
            unitAddressId: unitAddressId,
            eventId: eventId,
            externalUpdated: externalUpdated,
            supplementaryTownNameBefore: supplementaryTownNameBefore,
            supplementaryTownNameAfter: supplementaryTownNameAfter);

        result.Should().BeEquivalentTo(expected);
    }
}
