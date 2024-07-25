using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrudApp.Models
{
    public class Thing
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }



    }
}
