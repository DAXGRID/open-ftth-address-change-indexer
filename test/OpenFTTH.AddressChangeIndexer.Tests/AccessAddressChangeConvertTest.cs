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

    [Fact]
    public void Plot_id_changed_to_address_change()
    {
        var unitAddressId = Guid.Parse("55113f86-b304-4ee8-945e-086a398f34ef");
        var eventId = Guid.Parse("6665e1a1-0de2-4038-a0d0-3e155cc0d7ef");
        var externalUpdated = DateTime.UtcNow;
        var plotIdBefore = "1234";
        var plotIdAfter = "5678";

        var expected = new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.AccessAddressPlotIdChanged,
            externalUpdated: externalUpdated,
            before: plotIdBefore,
            after: plotIdAfter);

        var result = AccessAddressChangeConvert.PlotIdChanged(
            unitAddressId: unitAddressId,
            eventId: eventId,
            externalUpdated: externalUpdated,
            plotIdBefore: plotIdBefore,
            plotIdAfter: plotIdAfter);

        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Road_id_changed_to_address_change()
    {
        var unitAddressId = Guid.Parse("55113f86-b304-4ee8-945e-086a398f34ef");
        var eventId = Guid.Parse("6665e1a1-0de2-4038-a0d0-3e155cc0d7ef");
        var externalUpdated = DateTime.UtcNow;
        var roadIdBefore = Guid.Parse("efcfe55a-e2c3-4bd2-ae08-44327787da54");
        var roadIdAfter = Guid.Parse("fce4562c-1b64-4bbb-a02f-7e8e966a5ea0");

        var expected = new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.AccessAddressRoadIdChanged,
            externalUpdated: externalUpdated,
            before: roadIdBefore.ToString(),
            after: roadIdAfter.ToString());

        var result = AccessAddressChangeConvert.RoadIdChanged(
            unitAddressId: unitAddressId,
            eventId: eventId,
            externalUpdated: externalUpdated,
            roadIdBefore: roadIdBefore,
            roadIdAfter: roadIdAfter);

        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Coordinate_changed_to_address_change()
    {
        var unitAddressId = Guid.Parse("55113f86-b304-4ee8-945e-086a398f34ef");
        var eventId = Guid.Parse("6665e1a1-0de2-4038-a0d0-3e155cc0d7ef");
        var externalUpdated = DateTime.UtcNow;
        var eastCoordinateBefore = 53.205;
        var northCoordinateBefore = 10.203;
        var eastCoordinateAfter = 63.206;
        var northCoordinateAfter = 20.204;

        var expected = new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.AccessAddressCoordinateChanged,
            externalUpdated: externalUpdated,
            before: "53.205,10.203",
            after: "63.206,20.204");

        var result = AccessAddressChangeConvert.CoordinateChanged(
            unitAddressId: unitAddressId,
            eventId: eventId,
            externalUpdated: externalUpdated,
            eastCoordinateBefore: eastCoordinateBefore,
            northCoordinateBefore: northCoordinateBefore,
            eastCoordinateAfter: eastCoordinateAfter,
            northCoordinateAfter: northCoordinateAfter);

        result.Should().BeEquivalentTo(expected);
    }
}
