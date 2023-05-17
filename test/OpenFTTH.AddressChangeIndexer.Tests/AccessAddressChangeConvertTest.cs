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
        var sequenceNumber = 50L;
        var eventTimestamp = DateTime.UtcNow;

        var expected = new AddressChange(
                unitAddressId: unitAddressId,
                eventId: eventId,
                changeType: AddressChangeType.AccessAddressMunicipalCodeChanged,
                externalUpdated: externalUpdated,
                sequenceNumber: sequenceNumber,
                eventTimestamp: eventTimestamp,
                before: municipalCodeBefore,
                after: municipalCodeAfter);

        var result = AccessAddressChangeConvert.MunicipalCodeChanged(
            unitAddressId: unitAddressId,
            eventId: eventId,
            externalUpdated: externalUpdated,
            sequenceNumber: sequenceNumber,
            eventTimestamp: eventTimestamp,
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
        var sequenceNumber = 50L;
        var eventTimestamp = DateTime.UtcNow;

        var expected = new AddressChange(
                unitAddressId: unitAddressId,
                eventId: eventId,
                changeType: AddressChangeType.AccessAddressStatusChanged,
                externalUpdated: externalUpdated,
                sequenceNumber: sequenceNumber,
                eventTimestamp: eventTimestamp,
                before: "Pending",
                after: "Active");

        var result = AccessAddressChangeConvert.StatusChanged(
            unitAddressId: unitAddressId,
            eventId: eventId,
            externalUpdated: externalUpdated,
            sequenceNumber: sequenceNumber,
            eventTimestamp: eventTimestamp,
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
        var sequenceNumber = 50L;
        var eventTimestamp = DateTime.UtcNow;

        var expected = new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.AccessAddressRoadCodeChanged,
            externalUpdated: externalUpdated,
            sequenceNumber: sequenceNumber,
            eventTimestamp: eventTimestamp,
            before: roadCodeBefore,
            after: roadCodeAfter);

        var result = AccessAddressChangeConvert.RoadCodeChanged(
            unitAddressId: unitAddressId,
            eventId: eventId,
            externalUpdated: externalUpdated,
            sequenceNumber: sequenceNumber,
            eventTimestamp: eventTimestamp,
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
        var sequenceNumber = 50L;
        var eventTimestamp = DateTime.UtcNow;

        var expected = new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.AccessAddressHouseNumberChanged,
            externalUpdated: externalUpdated,
            sequenceNumber: sequenceNumber,
            eventTimestamp: eventTimestamp,
            before: houseNumberBefore,
            after: houseNumberAfter);

        var result = AccessAddressChangeConvert.HouseNumberChanged(
            unitAddressId: unitAddressId,
            eventId: eventId,
            externalUpdated: externalUpdated,
            sequenceNumber: sequenceNumber,
            eventTimestamp: eventTimestamp,
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
        var sequenceNumber = 50L;
        var eventTimestamp = DateTime.UtcNow;

        var expected = new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.AccessAddressSupplementaryTownNameChanged,
            externalUpdated: externalUpdated,
            sequenceNumber: sequenceNumber,
            eventTimestamp: eventTimestamp,
            before: supplementaryTownNameBefore,
            after: supplementaryTownNameAfter);

        var result = AccessAddressChangeConvert.SupplementaryTownNameChanged(
            unitAddressId: unitAddressId,
            eventId: eventId,
            externalUpdated: externalUpdated,
            sequenceNumber: sequenceNumber,
            eventTimestamp: eventTimestamp,
            supplementaryTownNameBefore: supplementaryTownNameBefore,
            supplementaryTownNameAfter: supplementaryTownNameAfter);

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
        var sequenceNumber = 50L;
        var eventTimestamp = DateTime.UtcNow;

        var expected = new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.AccessAddressRoadIdChanged,
            externalUpdated: externalUpdated,
            sequenceNumber: sequenceNumber,
            eventTimestamp: eventTimestamp,
            before: roadIdBefore.ToString(),
            after: roadIdAfter.ToString());

        var result = AccessAddressChangeConvert.RoadIdChanged(
            unitAddressId: unitAddressId,
            eventId: eventId,
            externalUpdated: externalUpdated,
            sequenceNumber: sequenceNumber,
            eventTimestamp: eventTimestamp,
            roadIdBefore: roadIdBefore,
            roadIdAfter: roadIdAfter);

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
        var sequenceNumber = 50L;
        var eventTimestamp = DateTime.UtcNow;

        var expected = new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.AccessAddressPlotIdChanged,
            externalUpdated: externalUpdated,
            sequenceNumber: sequenceNumber,
            eventTimestamp: eventTimestamp,
            before: plotIdBefore,
            after: plotIdAfter);

        var result = AccessAddressChangeConvert.PlotIdChanged(
            unitAddressId: unitAddressId,
            eventId: eventId,
            externalUpdated: externalUpdated,
            sequenceNumber: sequenceNumber,
            eventTimestamp: eventTimestamp,
            plotIdBefore: plotIdBefore,
            plotIdAfter: plotIdAfter);

        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Road__name_changed_to_address_change()
    {
        var unitAddressId = Guid.Parse("55113f86-b304-4ee8-945e-086a398f34ef");
        var eventId = Guid.Parse("6665e1a1-0de2-4038-a0d0-3e155cc0d7ef");
        var externalUpdated = DateTime.UtcNow;
        var roadNameBefore = "Vejlevej 10";
        var roadNameAfter = "Fredericiavej 20";
        var sequenceNumber = 50L;
        var eventTimestamp = DateTime.UtcNow;

        var expected = new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.AccessAddressRoadNameChanged,
            externalUpdated: externalUpdated,
            sequenceNumber: sequenceNumber,
            eventTimestamp: eventTimestamp,
            before: roadNameBefore,
            after: roadNameAfter);

        var result = AccessAddressChangeConvert.RoadNameChanged(
            unitAddressId: unitAddressId,
            eventId: eventId,
            externalUpdated: externalUpdated,
            sequenceNumber: sequenceNumber,
            eventTimestamp: eventTimestamp,
            roadNameBefore: roadNameBefore,
            roadNameAfter: roadNameAfter);

        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Road_id_changed_new_road_name_to_address_change()
    {
        var unitAddressId = Guid.Parse("55113f86-b304-4ee8-945e-086a398f34ef");
        var eventId = Guid.Parse("6665e1a1-0de2-4038-a0d0-3e155cc0d7ef");
        var externalUpdated = DateTime.UtcNow;
        var roadNameBefore = "Vejlevej 10";
        var roadNameAfter = "Fredericiavej 20";
        var sequenceNumber = 50L;
        var eventTimestamp = DateTime.UtcNow;

        var expected = new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.AccessAddressRoadIdChangedNewRoadName,
            externalUpdated: externalUpdated,
            sequenceNumber: sequenceNumber,
            eventTimestamp: eventTimestamp,
            before: roadNameBefore,
            after: roadNameAfter);

        var result = AccessAddressChangeConvert.RoadIdChangedNewRoadName(
            unitAddressId: unitAddressId,
            eventId: eventId,
            externalUpdated: externalUpdated,
            sequenceNumber: sequenceNumber,
            eventTimestamp: eventTimestamp,
            roadNameBefore: roadNameBefore,
            roadNameAfter: roadNameAfter);

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
        var sequenceNumber = 50L;
        var eventTimestamp = DateTime.UtcNow;

        var expected = new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.AccessAddressCoordinateChanged,
            externalUpdated: externalUpdated,
            sequenceNumber: sequenceNumber,
            eventTimestamp: eventTimestamp,
            before: "53.205,10.203",
            after: "63.206,20.204");

        var result = AccessAddressChangeConvert.CoordinateChanged(
            unitAddressId: unitAddressId,
            eventId: eventId,
            externalUpdated: externalUpdated,
            sequenceNumber: sequenceNumber,
            eventTimestamp: eventTimestamp,
            eastCoordinateBefore: eastCoordinateBefore,
            northCoordinateBefore: northCoordinateBefore,
            eastCoordinateAfter: eastCoordinateAfter,
            northCoordinateAfter: northCoordinateAfter);

        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Deleted_to_address_change()
    {
        var unitAddressId = Guid.Parse("55113f86-b304-4ee8-945e-086a398f34ef");
        var eventId = Guid.Parse("6665e1a1-0de2-4038-a0d0-3e155cc0d7ef");
        var externalUpdated = DateTime.UtcNow;
        var sequenceNumber = 50L;
        var eventTimestamp = DateTime.UtcNow;

        var expected = new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.AccessAddressDeleted,
            externalUpdated: externalUpdated,
            sequenceNumber: sequenceNumber,
            eventTimestamp: eventTimestamp);

        var result = AccessAddressChangeConvert.Deleted(
            unitAddressId: unitAddressId,
            eventId: eventId,
            externalUpdated: externalUpdated,
            sequenceNumber: sequenceNumber,
            eventTimestamp: eventTimestamp);

        result.Should().BeEquivalentTo(expected);
    }
}
