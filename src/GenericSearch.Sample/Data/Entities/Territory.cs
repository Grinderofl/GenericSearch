using System.Collections.Generic;

namespace GenericSearch.Sample.Data.Entities
{
    public class Territory : Entity<string>
    {
        public virtual string TerritoryDescription { get; set; }

        public virtual int RegionId { get; set; }
        public virtual Region Region { get; set; }

        public virtual List<EmployeeTerritory> EmployeeTerritories { get; set; } = new List<EmployeeTerritory>();
    }
}