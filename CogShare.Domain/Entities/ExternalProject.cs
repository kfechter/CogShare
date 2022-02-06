using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CogShare.Domain.Entities
{
    public class ExternalProject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? Tag { get; set; }

        public string? Type { get; set; }

        public string? Name { get; set; }

        public string? URL { get; set; }

        public string? Description { get; set; }

        public int? Votes { get; set; }
    }
}
