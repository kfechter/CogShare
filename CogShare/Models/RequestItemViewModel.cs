using CogShare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CogShare.Models
{
    public class RequestItemViewModel
    {
        public bool ErrorState { get; set; }

        public string StatusMessage { get; set; }

        public List<Item> RequestableItems { get; set; }

        public string Requestee { get; set; }

        public RequestItemViewModel()
        {
            RequestableItems = new List<Item>();
            ErrorState = false;
            StatusMessage = string.Empty;
            Requestee = string.Empty;
        }

        public RequestItemViewModel(List<Item> requestableItems, string requestee)
        {
            RequestableItems = requestableItems;
            ErrorState = false;
            StatusMessage = string.Empty;
            Requestee = requestee;
        }

        public RequestItemViewModel(bool errorState, string statusMessage, List<Item> requestableItems, string requestee)
        {
            ErrorState = errorState;
            StatusMessage = statusMessage;
            RequestableItems = requestableItems;
            Requestee = requestee;
        }
    }
}
