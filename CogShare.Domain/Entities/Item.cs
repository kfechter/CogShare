using System;
using System.Collections.Generic;
using System.Text;

namespace CogShare.Domain.Entities
{
    public class Item
    {
        public int Id { get; set; }

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
