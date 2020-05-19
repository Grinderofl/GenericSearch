using System.ComponentModel;
using Grinderofl.GenericSearch.Searches;

namespace Grinderofl.GenericSearch.UnitTests.VisitorTests
{
    internal class TestRequest
    {
        public TextSearch One { get; set; } = new TextSearch("One");

        public TextSearch Two { get; set; } = new TextSearch("Two");

        public TextSearch Three { get; set; } = new TextSearch("Three");

        [DefaultValue(3)]
        public SingleIntegerOptionSearch Four { get; set; } = new SingleIntegerOptionSearch("Four"){Is = 3};

        public SingleIntegerOptionSearch Five { get; set; } = new SingleIntegerOptionSearch("Four"){Is = 3};

        [DefaultValue(1)]
        public int Page { get; set; } = 3;

        [DefaultValue(20)]
        public int Rows { get; set; } = 20;

        [DefaultValue("Four")]
        public string Ordx { get; set; } = "Four";
    }
}