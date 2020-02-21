#pragma warning disable 1591
using System.ComponentModel.DataAnnotations;

namespace Grinderofl.GenericSearch
{
    public enum Direction
    {
        [Display(Name = "Asc")]
        Ascending,

        [Display(Name = "Desc")]
        Descending
    }
}