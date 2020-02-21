using System.ComponentModel.DataAnnotations;

namespace Grinderofl.GenericSearch.Sample.Data.Entities
{
    public abstract class Entity<TKey> : IEntity
    {
        [Key]
        [Required]
        public virtual TKey Id { get; set; }
    }
}