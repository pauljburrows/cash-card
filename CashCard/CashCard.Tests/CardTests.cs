using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CashCard.Tests
{
    [TestFixture]
    public class CardTests
    {
        private const string PinNumber = "1234";

        [Test]
        public void CreateCard_InvalidPinNumber_Throws()
        {
            Assert.Throws<ArgumentException>(() => new Card(null));
            Assert.Throws<ArgumentException>(() => new Card("123"));
            Assert.Throws<ArgumentException>(() => new Card("12345"));
            Assert.Throws<ArgumentException>(() => new Card("-123"));
        }

        [Test]
        public void Balance_Initial_Zero()
        {
            var card = new Card(PinNumber);
            Assert.AreEqual(0m, card.Balance);
        }

        [Test]
        public void TopUp_IncorrectPin_Throws()
        {
            var card = new Card(PinNumber);
            Assert.Throws<ArgumentException>(() => card.TopUp("9999", 10m));            
        }

        [Test]
        public void TopUp_InvalidAmount_Throws()
        {
            var card = new Card(PinNumber);
            Assert.Throws<ArgumentException>(() => card.TopUp(PinNumber, -1m));
            Assert.Throws<ArgumentException>(() => card.TopUp(PinNumber, 0m));
        }

        [Test]
        public void TopUp_UpdatesBalance()
        {
            var card = new Card(PinNumber);
            card.TopUp(PinNumber, 10m);
            card.TopUp(PinNumber, 20m);
            Assert.AreEqual(30m, card.Balance);
        }

        [Test]
        public void Withdraw_IncorrectPin_Throws()
        {
            var card = new Card(PinNumber);
            Assert.Throws<ArgumentException>(() => card.Withdraw("9999", 10m));
        }

        [Test]
        public void Withdraw_InvalidAmount_Throws()
        {
            var card = new Card(PinNumber);
            Assert.Throws<ArgumentException>(() => card.Withdraw(PinNumber, 0m));
            Assert.Throws<ArgumentException>(() => card.Withdraw(PinNumber, -10m));
        }

        [Test]
        public void Withdraw_InsufficientFinds_CashNotDispensed()
        {
            var card = new Card(PinNumber);
            var cashDispensed = card.Withdraw(PinNumber, 10m);

            Assert.False(cashDispensed);
            Assert.AreEqual(0m, card.Balance);
        }

        [Test]
        public void Withdraw_CashDispensed()
        {
            var card = new Card(PinNumber);
            card.TopUp(PinNumber, 50m);
            var cashDispensed = card.Withdraw(PinNumber, 10m);

            Assert.True(cashDispensed);
            Assert.AreEqual(40m, card.Balance);
        }

        [Test]
        public void Withdraw_AllFunds_CashDispensed()
        {
            var card = new Card(PinNumber);
            card.TopUp(PinNumber, 50m);
            var cashDispensed = card.Withdraw(PinNumber, 50m);

            Assert.True(cashDispensed);
            Assert.AreEqual(0m, card.Balance);
        }
    }
}