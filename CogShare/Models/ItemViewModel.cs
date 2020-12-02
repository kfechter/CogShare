using CogShare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CogShare.Models
{
    public class ItemViewModel
    {
        public bool ErrorState { get; set; }

        public string StatusMessage { get; set; }

        public List<Item> Items { get; set; }

        public ItemViewModel()
        {
            ErrorState = false;
            StatusMessage = string.Empty;
            Items = new List<Item>();
        }

        public ItemViewModel(List<Item> items)
        {
            Items = items;
            ErrorState = false;
            StatusMessage = string.Empty;
        }

        public ItemViewModel(bool errorState, string statusMessage, List<Item> items)
        {
            ErrorState = errorState;
            StatusMessage = statusMessage;
            Items = items;
        }
    }
}
