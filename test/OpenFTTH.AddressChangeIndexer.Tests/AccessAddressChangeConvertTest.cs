namespace OpenFTTH.AddressChangeIndexer.Tests;

public sealed class AccessAddressChangeConvertTest
{
    [Fact]
    public void Access_address_municipal_code_changed_to_address_change()
    {
        var unitAddressId = Guid.Parse("55113f86-b304-4ee8-945e-086a398f34ef");
        var eventId = Guid.Parse("6665e1a1-0de2-4038-a0d0-3e155cc0d7ef");
        var externalUpdated = DateTime.UtcNow;
        var municipalCodeBefore = "AD12";
        var municipalCodeAfter = "DF12";

        var expected = new AddressChange(
                unitAddressId: unitAddressId,
                eventId: eventId,
                changeType: AddressChangeType.MunicipalCodeChanged,
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
}
