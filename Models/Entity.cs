using System;
using System.ComponentModel.DataAnnotations;

namespace Agenda.Models
{
    public abstract class Entity
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? LastUpdatedDate { get; set; }

    }
}