﻿namespace McpSeriesGenerator.Unit
{
    public class SerialNumberWithoutCheckDigitTestData
    {
        public static IEnumerable<string> Generate()
        {
            return new string[] {
                "1313MEXXXA7747",
                "0102MEXXXC7090",
                "0101BRAXXC7905",
                "0809BRAXXC0563",
                "2020BRAXXM7671",
                "0203ARGXXM2982",
                "1415ARGXXA2794",
                "1819ARGXXM9860",
                "2020PHLXXC9800",
                "0607MWIXXA9994"
            };
        }
    }
}
