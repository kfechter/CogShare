using CogShare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CogShare.Models
{
    public class RequestViewModel
    {
        public bool ErrorState { get; set; }

        public string StatusMessage { get; set; }

        public List<Request> Requests { get; set; }

        public string OwnerId { get; set; }

        public List<Request> SentRequests
        {
            get
            {
                return Requests.Where(x => x.Requestor.Id == OwnerId).ToList();
            }
        }

        public List<Request> ReceivedRequests
        {
            get
            {
                return Requests.Where(x => x.Requestee.Id == OwnerId).ToList();
            }
        }

        public RequestViewModel()
        {
            OwnerId = string.Empty;
            Requests = new List<Request>();
            ErrorState = false;
            StatusMessage = string.Empty;
        }

        public RequestViewModel(string ownerId, List<Request> requests)
        {
            OwnerId = ownerId;
            Requests = requests;
            ErrorState = false;
            StatusMessage = string.Empty;
        }

        public RequestViewModel(string ownerId, List<Request> requests, bool errorState, string statusMessage)
        {
            OwnerId = ownerId;
            Requests = requests;
            ErrorState = errorState;
            StatusMessage = statusMessage;
        }
    }
}
