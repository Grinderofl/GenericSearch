﻿using GenericSearch.Configuration;

namespace GenericSearch.ModelBinders.Activation
{
    public interface IRequestPropertyActivator
    {
        void Activate(ListConfiguration configuration, object model);
    }
}