
namespace MultiIndex
{
    public class TestDataProvider
    {
        private const string AddressSuffix = " S Elm St";

        public List<CelebInfo> GetCelebTestData()
        {
            var houseNumber = 0;

            var testData = new List<CelebInfo>();

            var lines = File.ReadAllLines("TestData.txt");
            foreach(string line in lines)
            {
                ++ houseNumber;
                var lineSpan = line.AsSpan();
                var commaPosition = lineSpan.IndexOf(',');

                testData.Add(new CelebInfo()
                {
                    FirstName = new string(lineSpan.Slice(0, commaPosition)),
                    LastName = new string(lineSpan.Slice(commaPosition+1)),
                    Address = houseNumber + AddressSuffix,
                });
            }

            return testData;
        }
    }
}