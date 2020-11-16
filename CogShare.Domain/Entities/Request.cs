using System;
using System.Collections.Generic;
using System.Text;

namespace CogShare.Domain.Entities
{
    public class Request
    {
        public int Id { get; set; }

        public ApplicationUser Requestor { get; set; }

        public Item RequestedItem { get; set; }

        public ApplicationUser Requestee { get; set; }

        public string RequestMessage { get; set; }
    }
}
