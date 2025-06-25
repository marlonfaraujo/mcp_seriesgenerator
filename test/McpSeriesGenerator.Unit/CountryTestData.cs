namespace McpSeriesGenerator.Unit
{
    public class CountryTestData
    {
        public static IEnumerable<string> Generate()
        {
            return new[] {
                "ARG;Argentina",
                "ARM;Armenia",
                "ABW;Aruba",
                "AUS;Australia",
                "AUT;Austria",
                "BRA;Brazil"
            };
        }
    }
}
