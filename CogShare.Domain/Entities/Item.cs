using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CogShare.Domain.Entities
{
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime Created { get; set; }

        public string DisplayName { get; set; }

        public bool Consumable { get; set; }

        public long QuantityOnHand { get; set; }

        public ApplicationUser Owner { get; set; }

        public bool CanBorrow { get; set; }

        public bool OnLoan { get; set; }

        public DateTime? BorrowedDate { get; set; }

        public ApplicationUser Borrower { get; set; }
    }
}
