using System.Text;
using McpSeriesGenerator.App.Domain.Exceptions;

namespace McpSeriesGenerator.App.Domain.ValueObjects
{
    public class SerialNumber
    {
        private string CheckDigit { get; set; }
        private string Value { get; set; }

        public SerialNumber(string value) 
        {
            SetValue(value);
            SetCheckDigit();
        }

        public SerialNumber(string value, string checkDigit)
        {
            SetValue(value);
            CheckDigit = checkDigit;
        }

        public static SerialNumber Create(string value, char separator = '-')
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length < 14)
            {
                throw new SerialNumberInvalidException("Serial number must be at least 14 characters long.");
            }

            if (!value.Contains(separator))
            {
                return new SerialNumber(value);
            }
            var serialNumber = new SerialNumber(value, value.Split(separator).LastOrDefault());
            return serialNumber;
        }

        public void SetValue(string value, char separator = '-')
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length < 14)
            {
                throw new SerialNumberInvalidException("Serial number must be at least 14 characters long.");
            }
            if (!value.Contains(separator))
            {
                Value = value;
                return;
            }
            Value = value.Split(separator).FirstOrDefault();
        }

        public void SetCheckDigit()
        {
            if (string.IsNullOrWhiteSpace(Value) || Value.Length < 14)
            {
                throw new SerialNumberInvalidException("Serial number must be at least 14 characters long.");
            }
            CheckDigit = CalculateCheckDigitForHexadecimal();
        }

        public bool ValidateCheckDigit()
        {
            if (string.IsNullOrWhiteSpace(Value) || Value.Length < 14)
            {
                throw new SerialNumberInvalidException("Serial number must contain a check digit.");
            }
            string expectedCheckDigit = CalculateCheckDigitForHexadecimal();
            return expectedCheckDigit.Trim().ToUpper().Equals(CheckDigit.Trim().ToUpper());
        }

        private string CalculateCheckDigitForHexadecimal()
        {
            if (string.IsNullOrWhiteSpace(Value))
            {
                throw new SerialNumberInvalidException("Invalid input serial number.");
            }
            byte[] asciiItems = Encoding.ASCII.GetBytes(Value);
            int total = asciiItems.Sum(x => x);
            int rest = total % 16;
            return rest.ToString("X");
        }

        public string GetValueWithoutCheckDigit()
        {
            return Value;
        }

        public string GetValueWithCheckDigit(char separator = '-')
        {
            return $"{Value}{separator}{CheckDigit}";
        }
    }
}
