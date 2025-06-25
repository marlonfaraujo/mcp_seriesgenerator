namespace McpSeriesGenerator.Unit
{
    public class SerialNumberToValidTestData
    {
        public static IEnumerable<string> GenerateWithTrueValues()
        {
            return new string[] {
                "1313MEXXXA7989-4",
                "0606MEXXXA3820-4",
                "0708BRAXXC4014-0",
                "0606BRAXXA6466-8",
                "0909BRAXXC3262-7",
                "1414ARGXXA5834-9",
                "0202ARGXXC2614-E",
                "1717ARGXXA9193-1",
                "1415PAKXXM0980-5",
                "1213ASMXXC8348-2"
            };
        }


        public static IEnumerable<string> GenerateWithFalseValues()
        {
            return new string[] {
                "1313MEXXXA7989-1",
                "0606MEXXXA3820-1",
                "0708BRAXXC4014-3",
                "0606BRAXXA6466-1",
                "0909BRAXXC3262-8",
                "1414ARGXXA5834-1",
                "0202ARGXXC2614-1",
                "1717ARGXXA9193-C",
                "1415PAKXXM0980-1",
                "1213ASMXXC8348-1"
            };
        }
    }
}
