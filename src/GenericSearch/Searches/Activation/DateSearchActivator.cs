using System;
using System.Reflection;

namespace GenericSearch.Searches.Activation
{
    public class DateSearchActivator : SearchActivator<DateSearch>
    {
        public override ISearch Activate(string entityPath) => new DateSearch(entityPath);
    }
}