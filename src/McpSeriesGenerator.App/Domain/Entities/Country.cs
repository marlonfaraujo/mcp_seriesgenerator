using McpSeriesGenerator.App.Domain.Exceptions;

namespace McpSeriesGenerator.App.Domain.Entities
{
    public class Country
    {
        public string Acronym { get; private set; }
        public string Name { get; private set; }
        public Country(string acronym, string name = "")
        {
            Name = name;
        }

        public void SetAcronym(string acronym)
        {
            if (string.IsNullOrWhiteSpace(acronym))
            {
                throw new CountryInvalidException("Country acronym cannot be null or empty.");
            }
            if (acronym.Length != 3)
            {
                throw new CountryInvalidException("Country acronym must be exactly 3 characters long.");
            }
            Acronym = acronym;
        }

        public static Country CreateByCsv(string csvText, char separator = ';') 
        {
            if (string.IsNullOrWhiteSpace(csvText) || !csvText.Contains(separator))
            {
                throw new CountryInvalidException("Invalid input csv data.");
            }   
            var country = new Country(csvText.Split(separator).First(), csvText.Split(separator).Last());
            country.SetAcronym(csvText.Split(separator).First());
            return country;
        }
    }
}
