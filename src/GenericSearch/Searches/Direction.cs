﻿#pragma warning disable 1591
using System.ComponentModel.DataAnnotations;

namespace GenericSearch.Searches
{
    public enum Direction
    {
        [Display(Name = "Asc")]
        Ascending,

        [Display(Name = "Desc")]
        Descending
    }
}