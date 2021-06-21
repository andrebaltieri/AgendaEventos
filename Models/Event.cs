using System.ComponentModel.DataAnnotations;
using AgendaEventos.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agenda.Models
{
  [Table(nameof(Event))]
  public class Event : Entity
  {
    [Required(ErrorMessage = "Este campo é obrigatório")]
    [MaxLength(60, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres")]
    [MinLength(3, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Este campo é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "Tipo de usuario inválido")]
    public int SpeakerId { get; set; }
    public User Speaker { get; set; }

    [Required(ErrorMessage = "Este campo é obrigatório")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Este campo é obrigatório")]
    [Column(TypeName = nameof(DateTime))]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "Este campo é obrigatório")]
    [Range(1,60, ErrorMessage = "Este campo deve conter entre 1 e 60 minutos")]
    public int DurationInMinutes { get; set; }
    
    [Required(ErrorMessage = "Este campo é obrigatório")]
    [Column(TypeName = nameof(DateTime))]
    public DateTime EnrollmentDeadlineDate { get; set; }
    
    [Required(ErrorMessage = "Este campo é obrigatório")]
    [MaxLength(1024, ErrorMessage = "Este campo deve conter no máximo 1024 caracteres")]
    public string UrlSegment { get; set; }
    
    [Required(ErrorMessage = "Este campo é obrigatório")]
    [MaxLength(2000, ErrorMessage = "Este campo deve conter no máximo 2000 caracteres")]
    public string BannerUrl { get; set; }
    
    [Required(ErrorMessage = "Este campo é obrigatório")]
    public bool Active { get; set; }
    
    [Required(ErrorMessage = "Este campo é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "Tipo de usuario inválido")]
    public int OrganizerId { get; set; }
    public User Organizer { get; set; }
    
    [Required(ErrorMessage = "Este campo é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "Categoria inválida")]
    public int CategoryId { get; set; }
    public Category Category { get; set; }
  }
}