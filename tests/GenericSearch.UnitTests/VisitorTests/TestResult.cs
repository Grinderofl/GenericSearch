using System.ComponentModel;
using GenericSearch.Searches;

namespace GenericSearch.UnitTests.VisitorTests
{
    internal class TestResult
    {
        public TextSearch One { get; set; }

        public TextSearch Two { get; set; }

        public TextSearch Three { get; set; }

        [DefaultValue(3)]
        public SingleIntegerOptionSearch Four { get; set; }

        public SingleIntegerOptionSearch Five { get; set; }

        public int Page { get; set; }

        [DefaultValue(20)]
        public int Rows { get; set; }

        public string Ordx { get; set; }
    }
}