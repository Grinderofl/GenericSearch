using System.Collections.Generic;
using GenericSearch.Searches;

namespace GenericSearch.UnitTests
{
    public class TestResult
    {
        public TestResult(IEnumerable<TestEntity> items)
        {
            Items = items;
        }

        public IEnumerable<TestEntity> Items { get; }

        public BooleanSearch MatchAlways { get; set; }

        public TrueBooleanSearch MatchWhenTrue { get; set; }
        public TrueBooleanSearch MatchWhenTrueDefaultTrue { get; set; }
        
        public OptionalBooleanSearch MatchWhenNotNull { get; set; }
        public OptionalBooleanSearch MatchWhenNotNullDefaultTrue { get; set; }
        public OptionalBooleanSearch MatchWhenNotNullDefaultFalse { get; set; }
    }
}