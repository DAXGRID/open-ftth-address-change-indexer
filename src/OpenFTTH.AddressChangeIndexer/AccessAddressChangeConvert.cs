namespace OpenFTTH.AddressChangeIndexer;

internal static class AccessAddressChangeConvert
{
    public static AddressChange MunicipalCodeChanged(
        Guid unitAddressId,
        Guid eventId,
        DateTime externalUpdated,
        string? municipalCodeBefore,
        string? municipalCodeAfter)
    {
        return new AddressChange(
                unitAddressId: unitAddressId,
                eventId: eventId,
                changeType: AddressChangeType.MunicipalCodeChanged,
                externalUpdated: externalUpdated,
                before: municipalCodeBefore,
                after: municipalCodeAfter);
    }

    // AccessAddressMunicipalCodeChanged

    // AccessAddressStatusChanged

    // AccessAddressRoadCodeChanged

    // AccessAddressHouseNumberChanged

    // AccessAddressSupplementaryTownNameChanged

    // AccessAddressPlotIdChanged

    // AccessAddressRoadIdChanged

    // AccessAddressCoordinateChanged
}
