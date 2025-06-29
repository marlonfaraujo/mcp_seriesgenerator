using System.Text;
using McpSeriesGenerator.App.Domain.Exceptions;

namespace McpSeriesGenerator.App.Domain.ValueObjects
{
    public class SerialNumber
    {
        private string CheckDigit { get; set; } = string.Empty;
        private string Value { get; set; } = string.Empty;

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
            ValidateNullOrLength(value);

            if (!value.Contains(separator))
            {
                return new SerialNumber(value);
            }
            var serialNumber = new SerialNumber(value, value.Split(separator).LastOrDefault() ?? string.Empty);
            return serialNumber;
        }

        private static void ValidateNullOrLength(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length < 14)
            {
                throw new SerialNumberInvalidException("Serial number must be at least 14 characters long.");
            }
        }

        public void SetValue(string value, char separator = '-')
        {
            ValidateNullOrLength(value);
            if (!value.Contains(separator))
            {
                Value = value;
                return;
            }
            Value = value.Split(separator).FirstOrDefault() ?? string.Empty;
        }

        public void SetCheckDigit()
        {
            ValidateNullOrLength(Value);
            CheckDigit = CalculateCheckDigitForHexadecimal();
        }

        public bool ValidateCheckDigit()
        {
            ValidateNullOrLength(Value);
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
