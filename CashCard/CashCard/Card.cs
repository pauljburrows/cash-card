namespace CashCard
{
    public class Card
    {
        private readonly string _pin;
        private readonly object _transactionLock = new object();

        public decimal Balance { get; private set; }

        public Card(string pin)
        {
            EnsureValidPin(pin);
            _pin = pin;
        }        

        public void TopUp(string pin, decimal amount)
        {
            EnsureCorrectPin(pin);
            EnsureAmountGreaterThanZero(amount);

            lock (_transactionLock)
            {
                Balance += amount;
            }
        }

        public void Withdraw(string pin, decimal amount)
        {
            EnsureCorrectPin(pin);
            EnsureAmountGreaterThanZero(amount);

            lock (_transactionLock)
            {
                if (Balance < amount)
                {
                    throw new InsufficientFundsException();
                }

                Balance -= amount;
            }            
        }     

        private static void EnsureValidPin(string pin)
        {
            int pinNumber;

            if (pin == null || pin.Length != 4 || !int.TryParse(pin, out pinNumber) || pinNumber < 0)
            {
                throw new InvalidPinException();
            }
        }

        private void EnsureCorrectPin(string pin)
        {
            if (pin != _pin) throw new IncorrectPinException();
        }

        private static void EnsureAmountGreaterThanZero(decimal amount)
        {
            if (amount <= 0) throw new InvalidAmountException();
        }
    }
}