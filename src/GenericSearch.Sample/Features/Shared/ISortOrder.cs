using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericSearch.Searches;

namespace GenericSearch.Sample.Features.Shared
{
    public interface ISortOrder
    {
        Direction Ordd { get; set; }
        string Ordx { get; set; }
    }
}
