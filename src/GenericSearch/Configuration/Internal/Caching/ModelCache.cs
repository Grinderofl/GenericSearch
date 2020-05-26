using System;

namespace GenericSearch.Configuration.Internal.Caching
{
    /// <summary>
    /// Provides a per-request cache for the request/parameter model to simplify searching, ordering, paging,
    /// and transfering properties from request/parameter model to result/viewmodel.
    /// </summary>
    public class ModelCache
    {
        /// <summary>
        /// Specifies the type of the model. 
        /// </summary>
        public Type ModelType { get; private set; }

        /// <summary>
        /// Specifies the request/parameter model
        /// </summary>
        public object Model { get; private set; }


        /// <summary>
        /// Caches the request/parameter
        /// </summary>
        /// <param name="model"></param>
        public void CacheModel(object model)
        {
            Model = model;
            ModelType = model.GetType();
        }
    }
}