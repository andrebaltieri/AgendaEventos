using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agenda.Models
{
    public abstract class Entity
    {
        [Column(TypeName = nameof(DateTime))]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; }

        public int Id { get; set; }

        [Column(TypeName = nameof(DateTime))]
        public DateTime? LastUpdatedDate { get; set; }
    }
}