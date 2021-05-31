namespace Agenda.Models
{
    public class Category : Entity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
    }
}