using System.Collections.Generic;

namespace GenericSearch.Sample.Data.Entities
{
    public class Region : Entity<int>
    {
        public virtual string RegionDescription { get; set; }
        public virtual List<Territory> Territories { get; set; } = new List<Territory>();
    }
}