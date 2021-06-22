using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agenda.Models
{
    [Table(nameof(Category))]
    public class Category : Entity
    {
        [MinLength(3)]
        [MaxLength(80)]
        public string Title { get; set; }

        [MinLength(3)]
        [MaxLength(500)]
        public string Description { get; set; }
        public bool Active { get; set; }
    }
}