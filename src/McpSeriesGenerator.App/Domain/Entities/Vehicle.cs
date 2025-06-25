using McpSeriesGenerator.App.Domain.Exceptions;
using McpSeriesGenerator.App.Domain.ValueObjects;

namespace McpSeriesGenerator.App.Domain.Entities
{
    public class Vehicle
    {
        public SerialNumber VehicleSerialNumber { get; private set; }
        public int YearOfManufacture { get; private set; }
        public int ModelYear { get; private set; }
        public string VehicleType { get; private set; }
        public string AcronymCountryOfManufacture { get; private set; }

        public Vehicle(string serialNumber)
        {
            VehicleSerialNumber = SerialNumber.Create(serialNumber);
        }

        public static Vehicle Create(string serialNumber) 
        {
            var vehicle = new Vehicle(serialNumber);
            vehicle.SetYearOfManufacture();
            vehicle.SetModelYear();
            vehicle.SetVehicleType();
            vehicle.SetCountryOfManufacture();
            return vehicle;
        }

        public void SetYearOfManufacture()
        {
            ValidateSerialNumberNullOrEmpty();
            if (int.TryParse(VehicleSerialNumber.GetValueWithoutCheckDigit().Substring(0, 2), out int year) == false)
            {
                throw new VehicleInvalidException("Year of manufacture is invalid in the vehicle serial number.");
            }
            YearOfManufacture = year;
        }

        private void ValidateSerialNumberNullOrEmpty()
        {
            if (VehicleSerialNumber == null ||
                string.IsNullOrWhiteSpace(VehicleSerialNumber.GetValueWithoutCheckDigit()))
            {
                throw new VehicleInvalidException("Vehicle serial number is invalid or empty.");
            }
        }

        public void SetModelYear()
        {
            ValidateSerialNumberNullOrEmpty();
            if (int.TryParse(VehicleSerialNumber.GetValueWithoutCheckDigit().Substring(2, 2), out int modelYear) == false)
            {
                throw new VehicleInvalidException("Model year is invalid in the vehicle serial number.");
            }
            ModelYear = modelYear;
        }

        public void SetVehicleType()
        {
            ValidateSerialNumberNullOrEmpty();
            VehicleType = VehicleSerialNumber.GetValueWithoutCheckDigit().Substring(9, 1);
        }

        public void SetCountryOfManufacture()
        {
            ValidateSerialNumberNullOrEmpty();
            AcronymCountryOfManufacture = VehicleSerialNumber.GetValueWithoutCheckDigit().Substring(4, 3);
        }

    }
}
