using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CogShare.Domain.Entities
{
    public class Friendship
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public ApplicationUser User1 { get; set; }

        public ApplicationUser User2 { get; set; }

        public string User1Id { get; set; }

        public string User2Id { get; set; }

        public bool Accepted { get; set; }
        public string Message { get; set; }
    }
}
