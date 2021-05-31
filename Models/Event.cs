using AgendaEventos.Models;
using System;

namespace Agenda.Models
{
    public class Event : Entity
    {
        public string Title { get; set; }
        public User Speaker { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public int DurationInMinutes { get; set; }
        public DateTime EnrollmentDeadlineDate { get; set; }
        public string UrlSegment { get; set; }
        public string BannerUrl { get; set; }
        public bool Active { get; set; }
        public User Organizer { get; set; }
        public Category Category { get; set; }
    }
}