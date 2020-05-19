using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Grinderofl.GenericSearch.UnitTests
{
    internal class TestModelBindingContext : DefaultModelBindingContext
    {
        public override Type ModelType => typeof(TestRequest);
    }
}