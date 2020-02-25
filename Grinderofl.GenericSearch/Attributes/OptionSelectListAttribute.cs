using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Grinderofl.GenericSearch.Attributes
{
    /// <summary>
    ///     OptionSelectListAttribute is an attribute to set a specific index key for retrieving
    ///     a <see cref="IReadOnlyList{T}"/> of <see cref="SelectListItem"/> from the <see cref="ViewDataDictionary"/>
    ///     when using 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class OptionSelectListAttribute : Attribute
    {
        /// <summary>
        /// Initialises a new instance of <see cref="OptionSelectListAttribute"/>.
        /// </summary>
        public OptionSelectListAttribute()
        {
        }

        /// <summary>
        /// Initialises a new instance of <see cref="OptionSelectListAttribute"/> with its <see cref="ViewDataKey"/> attribute.
        /// </summary>
        /// <param name="viewDataKey"></param>
        public OptionSelectListAttribute(string viewDataKey)
        {
            ViewDataKey = viewDataKey;
        }

        /// <summary>
        /// Name of the index key in <see cref="ViewDataDictionary"/> to access the <see cref="IReadOnlyList{T}"/> of <see cref="SelectListItem"/>.
        /// </summary>
        public string ViewDataKey { get; set; }
    }
}
