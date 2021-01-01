using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CogShare.Domain.Entities
{
    public class Request
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime Created { get; set; }

        public ApplicationUser Requestor { get; set; }

        public Item RequestedItem { get; set; }

        public ApplicationUser Requestee { get; set; }

        public string RequestMessage { get; set; }
    }
}
