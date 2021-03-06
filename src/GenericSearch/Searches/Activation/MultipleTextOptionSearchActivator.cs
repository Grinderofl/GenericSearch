﻿using System;
using System.Reflection;

namespace GenericSearch.Searches.Activation
{
    public class MultipleTextOptionSearchActivator : SearchActivator<MultipleTextOptionSearch>
    {
        public override ISearch Activate(params string[] entityPaths) => new MultipleTextOptionSearch(entityPaths);
    }
}