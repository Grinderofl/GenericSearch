﻿using System;
using System.Reflection;
using GenericSearch.Internal.Configuration;
using GenericSearch.Searches;
using Microsoft.AspNetCore.Routing;

namespace GenericSearch.ActionFilters.Visitors
{
    /// <summary>
    /// Provides a visitor for top-level model properties
    /// </summary>
    public class ModelPropertyVisitor : RouteValuePropertyVisitor
    {
        private readonly IListConfiguration configuration;
        private readonly object model;
        private readonly RouteValueDictionary routeValueDictionary;

        /// <summary>
        /// Initializes a new instance of <see cref="ModelPropertyVisitor"/>
        /// </summary>
        /// <param name="configuration">Filter Configuration</param>
        /// <param name="model">Value of the model property</param>
        /// <param name="routeValueDictionary">RouteValueDictionary to populate</param>
        public ModelPropertyVisitor(IListConfiguration configuration, 
                                    object model,
                                    RouteValueDictionary routeValueDictionary) : base(routeValueDictionary)
        {
            this.configuration = configuration;
            this.model = model;
            this.routeValueDictionary = routeValueDictionary;
        }

        /// <summary>
        /// Visits the provided property
        /// </summary>
        /// <param name="propertyInfo">Property to visit</param>
        public override void Visit(PropertyInfo propertyInfo)
        {
            if (ShouldSkipProperty(propertyInfo))
            {
                return;
            }

            var propertyValue = propertyInfo.GetValue(model);
            if (IsDefaultPropertyValue(propertyInfo, propertyValue))
            {
                return;
            }

            if (propertyValue is ISearch search)
            {
                var properties = search.GetType().GetProperties();
                var visitor = new SearchPropertyVisitor(search, propertyInfo, routeValueDictionary);
                foreach (var property in properties)
                {
                    visitor.Visit(property);
                }

                return;
            }
            
            PopulateRouteValues(propertyInfo.Name, propertyInfo, propertyValue);
        }

        /// <inheritdoc />
        protected override bool IsDefaultPropertyValue(PropertyInfo propertyInfo, object value)
        {
            if (base.IsDefaultPropertyValue(propertyInfo, value))
            {
                return true;
            }
            
            return false;
        }
    }
}