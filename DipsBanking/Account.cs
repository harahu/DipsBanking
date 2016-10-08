using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankNS
{
    public class Account
    {
        private int serialNum;

        private Person owner;

        private Money balance;

        // Constructor
        public Account(int serialNum, Person owner, Money balance)
        {
            this.serialNum = serialNum;
            this.owner = owner;
            this.balance = balance;
        }

        // Deposit Money to Account
        public void Deposit(Money amount)
        {
            balance = new Money(balance.GetAmount() + amount.GetAmount());
        }

        // Withdraw Money from Account
        public void Withdraw(Money amount)
        {
            balance = new Money(balance.GetAmount() - amount.GetAmount());
        }

        // Confirms whether or not balance allows a certain amount to be withdrawn
        public bool CanWithdraw(Money amout)
        {
            return (amout.GetAmount() <= balance.GetAmount());
        }

        // Returns serial number of account
        public int GetSerialNum()
        {
            return serialNum;
        }

        // returns owner of account
        public Person GetOwner()
        {
            return owner;
        }

        // returns balance of account
        public Money GetBalance()
        {
            return balance;
        }
    }
}
