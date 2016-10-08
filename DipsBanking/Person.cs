using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankNS
{
    public class Person
    {
        private Money cash;

        // Constructor
        public Person(Money cash)
        {
            this.cash = cash;
        }

        // Confirms whether or not Person can afford a certain payment
        public bool CanPay(Money amout)
        {
            return (amout.GetAmount() <= cash.GetAmount());
        }

        // Makes Person pay a certain amount of Money
        public void Pay(Money amount)
        {
            cash = new Money(cash.GetAmount() - amount.GetAmount());
        }

        // Gives Person a certain amount of money
        public void Receive(Money amount)
        {
            cash = new Money(cash.GetAmount() + amount.GetAmount());
        }
    }
}
