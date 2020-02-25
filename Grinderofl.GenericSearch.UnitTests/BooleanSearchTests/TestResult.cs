using System.Collections.Generic;
using Grinderofl.GenericSearch.Searches;

namespace Grinderofl.GenericSearch.UnitTests.BooleanSearchTests
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