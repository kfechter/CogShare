namespace CogShare.Models
{
    public class CreateItemViewModel
    {
        public bool ErrorState { get; set; }

        public string StatusMessage { get; set; }

        public string DisplayName { get; set; }

        public bool Consumable { get; set; }

        public long QuantityOnHand { get; set; }

        public bool CanBorrow { get; set; }

        public CreateItemViewModel()
        {
            DisplayName = string.Empty;
            Consumable = false;
            QuantityOnHand = 0L;
            CanBorrow = false;
            StatusMessage = string.Empty;
            ErrorState = false;
        }

        public CreateItemViewModel(string displayName, bool consumable, long quantityOnHand, bool canBorrow)
        {
            DisplayName = displayName;
            Consumable = consumable;
            QuantityOnHand = quantityOnHand;
            CanBorrow = canBorrow;
            StatusMessage = string.Empty;
            ErrorState = false;
        }

        public CreateItemViewModel(string displayName, bool consumable, long quantityOnHand, bool canBorrow, string statusMessage, bool errorState)
        {
            DisplayName = displayName;
            Consumable = consumable;
            QuantityOnHand = quantityOnHand;
            CanBorrow = canBorrow;
            StatusMessage = statusMessage;
            ErrorState = errorState;
        }

        public CreateItemViewModel(string statusMessage, bool errorState)
        {
            DisplayName = string.Empty;
            Consumable = false;
            QuantityOnHand = 0L;
            CanBorrow = false;
            StatusMessage = statusMessage;
            ErrorState = errorState;
        }
    }
}
