using McpSeriesGenerator.App.Domain.Entities;
using McpSeriesGenerator.App.Domain.Exceptions;

namespace McpSeriesGenerator.Unit.Domain
{
    public class VehicleTest
    {

        [Fact(DisplayName = "Given serial number without check digit When calculating with ascii code Then it should return the check code in hexadecimal completing the full serial number")]
        public void Given_SerialNumberWithoutCheckDigit_When_CalculatingWithAsciiCode_Then_ItShouldReturnTheCheckCodeInHexadecimalCompletingTheFullSerialNumber()
        {
            var itemsWithoutCheckDigit = SerialNumberWithoutCheckDigitTestData.Generate();
            var vehicles = new List<Vehicle>();    
            foreach (var item in itemsWithoutCheckDigit)
            {
                var vehicle = Vehicle.Create(item);
                vehicles.Add(vehicle);
            }

            Assert.NotEmpty(vehicles);
            Assert.Equal(itemsWithoutCheckDigit.Count(), vehicles.Count);
            Assert.True(vehicles.All(x => x.VehicleSerialNumber.GetValueWithCheckDigit().Contains('-')));
        }

        [Fact(DisplayName = "Given serial number with check digit When validating check digit Then it should return true")]
        public void Given_SerialNumberWithCheckDigit_When_ValidatingCheckDigit_Then_ItShouldReturnTrue()
        {
            var trueItems = SerialNumberToValidTestData.GenerateWithTrueValues();
            var vehiclesWithCheckDigitValid = new List<Vehicle>();    
            foreach (var item in trueItems)
            {
                var vehicle = Vehicle.Create(item);
                vehiclesWithCheckDigitValid.Add(vehicle);
            }
            Assert.True(vehiclesWithCheckDigitValid.All(x => x.VehicleSerialNumber.ValidateCheckDigit()));
            var falseItems = SerialNumberToValidTestData.GenerateWithFalseValues();
            var vehiclesWithCheckDigitInvalid = new List<Vehicle>();
            foreach (var item in falseItems)
            {
                var vehicle = Vehicle.Create(item);
                vehiclesWithCheckDigitInvalid.Add(vehicle);
            }
            Assert.True(vehiclesWithCheckDigitInvalid.All(x => !x.VehicleSerialNumber.ValidateCheckDigit()));
        }

        [Fact(DisplayName = "Given invalid serial number When creating vehicle Then it should throw SerialNumberInvalidException")]
        public void Given_InvalidSerialNumber_When_CreatingVehicle_Then_ItShouldThrowSerialNumberInvalidException()
        {
            var invalidValue = "123";
            Assert.Throws<SerialNumberInvalidException>(() => Vehicle.Create(invalidValue));
        }

        [Fact(DisplayName = "Given empty serial number When setting year of manufacture Then it should throw VehicleInvalidException")]
        public void Given_EmptySerialNumber_When_SettingYearOfManufacture_Then_ItShouldThrowVehicleInvalidException()
        {
            var vehicle = new Vehicle("1313MEXXXA7989-4");
            // Simulate invalid state
            typeof(Vehicle)
                .GetProperty("VehicleSerialNumber")!
                .SetValue(vehicle, null);
            Assert.Throws<VehicleInvalidException>(() => vehicle.SetYearOfManufacture());
        }
    }
}
