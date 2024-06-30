
using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;

namespace MultiIndex
{
    [MemoryDiagnoser]
    public class MultiIndexDictionaryBenchmark
    {
        private const string Separator = "|";

        private List<CelebInfo> testData;

        //public MultiIndexDictionaryBenchmark(TestDataProvider testDataProvider)
        public MultiIndexDictionaryBenchmark()
        {
            // test data is initialized in Setup()
            testData = null!;

            //Console.WriteLine("Test started");
        }

        [GlobalSetup]
        public void Setup()
        {
            TestDataProvider testDataProvider = new TestDataProvider();
            testData = testDataProvider.GetCelebTestData();
        }

        [Benchmark]
        public void AddWithConcatenatedHashCodes()
        {
            var myDictionary = new Dictionary<long, CelebInfo>();

            foreach (var celeb in testData)
            {
                myDictionary.Add(((long)celeb.FirstName.GetHashCode() << 32) | (uint)celeb.LastName.GetHashCode(), celeb);
            }
        }

        [Benchmark]
        public void AddWithUnionedHashCodes()
        {
            var myDictionary = new Dictionary<long, CelebInfo>();
            TwoKeyUnion keyBuilder;
            keyBuilder.fullKey = 0; // to avoid compiler error

            foreach (var celeb in testData)
            {
                keyBuilder.key0 = celeb.FirstName.GetHashCode();
                keyBuilder.key1 = celeb.LastName.GetHashCode();
                myDictionary.Add(keyBuilder.fullKey, celeb);
            }
        }

        [Benchmark]
        public void AddWithStringInterpolatedKey()
        {
            var myDictionary = new Dictionary<string, CelebInfo>();

            foreach(var celeb in testData)
            {
                myDictionary.Add($"{celeb.FirstName}{Separator}{celeb.LastName}", celeb);
            }
        }

        [Benchmark]
        public void AddWithStringConcatenatedKey()
        {
            var myDictionary = new Dictionary<string, CelebInfo>();

            foreach(var celeb in testData)
            {
                myDictionary.Add(celeb.FirstName + Separator + celeb.LastName, celeb);
            }
        }

        [Benchmark]
        public void AddWithStringBuilderMadeKey()
        {
            var myDictionary = new Dictionary<string, CelebInfo>();
            var compositKeyBuilder = new StringBuilder();

            foreach(var celeb in testData)
            {
                compositKeyBuilder.Append(celeb.FirstName);
                compositKeyBuilder.Append(Separator);
                compositKeyBuilder.Append(celeb.LastName);
                myDictionary.Add(compositKeyBuilder.ToString(), celeb);
                compositKeyBuilder.Clear();
            }
        }

        [Benchmark]
        public void AddWithStringJoinedKey()
        {
            var myDictionary = new Dictionary<string, CelebInfo>();

            foreach(var celeb in testData)
            {
                myDictionary.Add(String.Join(Separator, celeb.FirstName, celeb.LastName), celeb);
            }
        }

        [Benchmark]
        public void AddToNestedDictionaries()
        {
            Dictionary<string, Dictionary<string, CelebInfo>> outerDictionary = new Dictionary<string, Dictionary<string, CelebInfo>>();

            foreach(var celeb in testData)
            {
                if (!outerDictionary.TryGetValue(celeb.FirstName, out Dictionary<string, CelebInfo>? innerDictionary))
                {
                    innerDictionary = new Dictionary<string, CelebInfo>();
                    outerDictionary.Add(celeb.FirstName, innerDictionary);
                }
                if (!innerDictionary.TryGetValue(celeb.LastName, out var _))
                {
                    innerDictionary.Add(celeb.LastName, celeb);
                }
            }
        }

        [Benchmark]
        public void AddWithStringArrayKey()
        {
            var myDictionary = new Dictionary<string[], CelebInfo>();

            foreach(var celeb in testData)
            {
                string[] twoKeys = [celeb.FirstName, celeb.LastName];
                myDictionary.Add(twoKeys, celeb);
            }
        }


    }
}