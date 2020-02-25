using System.ComponentModel;
using Grinderofl.GenericSearch.Searches;

namespace Grinderofl.GenericSearch.UnitTests.BooleanSearchTests
{
    public class TestRequest
    {
        public BooleanSearch MatchAlways { get; set; }

        public TrueBooleanSearch MatchWhenTrue { get; set; }
        [DefaultValue(true)]
        public TrueBooleanSearch MatchWhenTrueDefaultTrue { get; set; }
        
        public OptionalBooleanSearch MatchWhenNotNull { get; set; }
        [DefaultValue(true)]
        public OptionalBooleanSearch MatchWhenNotNullDefaultTrue { get; set; }
        [DefaultValue(false)]
        public OptionalBooleanSearch MatchWhenNotNullDefaultFalse { get; set; }
    }
}