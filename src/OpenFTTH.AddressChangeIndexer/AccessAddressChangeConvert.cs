using OpenFTTH.Core.Address;

namespace OpenFTTH.AddressChangeIndexer;

internal static class AccessAddressChangeConvert
{
    public static AddressChange MunicipalCodeChanged(
        Guid unitAddressId,
        Guid eventId,
        DateTime? externalUpdated,
        string? municipalCodeBefore,
        string? municipalCodeAfter)
    {
        return new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.AccessAddressMunicipalCodeChanged,
            externalUpdated: externalUpdated,
            before: municipalCodeBefore,
            after: municipalCodeAfter);
    }

    public static AddressChange StatusChanged(
        Guid unitAddressId,
        Guid eventId,
        DateTime? externalUpdated,
        AccessAddressStatus statusBefore,
        AccessAddressStatus statusAfter)
    {
        return new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.AccessAddressStatusChanged,
            externalUpdated: externalUpdated,
            before: statusBefore.ToString(),
            after: statusAfter.ToString());
    }

    // AccessAddressRoadCodeChanged

    // AccessAddressHouseNumberChanged

    // AccessAddressSupplementaryTownNameChanged

    // AccessAddressPlotIdChanged

    // AccessAddressRoadIdChanged

    // AccessAddressCoordinateChanged
}
