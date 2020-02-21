namespace Grinderofl.GenericSearch.Sample.Data.Entities
{
    public class EmployeeTerritory : IEntity
    {
        public virtual int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        public virtual string TerritoryId { get; set; }
        public virtual Territory Territory { get; set; }
    }
}