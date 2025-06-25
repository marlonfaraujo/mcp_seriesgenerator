using McpSeriesGenerator.App.Domain.Exceptions;
using McpSeriesGenerator.App.Domain.ValueObjects;

namespace McpSeriesGenerator.Unit.Domain
{
    public class SerialNumberTest
    {
        [Fact(DisplayName = "Given short serial number When creating Then it should throw SerialNumberInvalidException")]
        public void Given_ShortSerialNumber_When_Creating_Then_ItShouldThrowSerialNumberInvalidException()
        {
            var shortValue = "123";
            Assert.Throws<SerialNumberInvalidException>(() => SerialNumber.Create(shortValue));
        }

        [Fact(DisplayName = "Given null serial number When creating Then it should throw SerialNumberInvalidException")]
        public void Given_NullSerialNumber_When_Creating_Then_ItShouldThrowSerialNumberInvalidException()
        {
            string? nullValue = null;
            Assert.Throws<SerialNumberInvalidException>(() => SerialNumber.Create(nullValue));
        }

        [Fact(DisplayName = "Given serial number with check digit When validating check digit Then it should return true or false")]
        public void Given_SerialNumberWithCheckDigit_When_ValidatingCheckDigit_Then_ItShouldReturnTrueOrFalse()
        {
            var valid = "1313MEXXXA7989-4";
            var invalid = "1313MEXXXA7989-1";
            var serialValid = SerialNumber.Create(valid);
            var serialInvalid = SerialNumber.Create(invalid);
            Assert.True(serialValid.ValidateCheckDigit());
            Assert.False(serialInvalid.ValidateCheckDigit());
        }
    }
}
