using McpSeriesGenerator.App.Domain.Entities;
using McpSeriesGenerator.App.Domain.Exceptions;

namespace McpSeriesGenerator.Unit.Domain
{
    public class CountryTest
    {
        [Fact(DisplayName = "Given valid CSV When creating country Then it should create country successfully")]
        public void Given_ValidCsv_When_CreatingCountry_Then_ItShouldCreateCountrySuccessfully()
        {
            var csv = "BRA;Brazil";
            var country = Country.CreateByCsv(csv);
            Assert.Equal("BRA", country.Acronym);
            Assert.Equal("Brazil", country.Name);
        }

        [Fact(DisplayName = "Given invalid CSV When creating country Then it should throw CountryInvalidException")]
        public void Given_InvalidCsv_When_CreatingCountry_Then_ItShouldThrowCountryInvalidException()
        {
            var invalidCsv = "InvalidData";
            Assert.Throws<CountryInvalidException>(() => Country.CreateByCsv(invalidCsv));
        }

        [Fact(DisplayName = "Given empty CSV When creating country Then it should throw CountryInvalidException")]
        public void Given_EmptyCsv_When_CreatingCountry_Then_ItShouldThrowCountryInvalidException()
        {
            var emptyCsv = "";
            Assert.Throws<CountryInvalidException>(() => Country.CreateByCsv(emptyCsv));
        }
    }
}
