using McpSeriesGenerator.App.Domain.Exceptions;

namespace McpSeriesGenerator.App.Domain.Entities
{
    public class Country
    {
        public string Acronym { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;

        public Country(string name)
        {
            Name = name;
        }

        public Country(string acronym, string name = "")
        {
            SetAcronym(acronym);
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
            return Create(csvText.Split(separator).First(), csvText.Split(separator).Last());
        }

        public static Country Create(string acronym, string name)
        {
            var country = new Country(name);
            country.SetAcronym(acronym);
            return country;
        }
    }
}
