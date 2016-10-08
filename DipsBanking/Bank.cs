using System;
using System.Collections.Generic;

namespace BankNS
{
    /// <summary> 
    /// Bank system demonstration
    /// </summary> 
    public class Bank
    {

        private int accNum;

        private Dictionary<Person, List<Account>> customerDict;

        public const string CannotAffordNewAccountMessage = "Person cannot afford new account";
        public const string CannotAffordDepositMessage = "Person cannot afford the deposit";
        public const string BalanceTooLowMessage = "Not enough money in account";

        // Constructor
        public Bank()
        {
            accNum = 0;
            customerDict = new Dictionary<Person, List<Account>>();
        }

        // Creates a bank account for a customer with enough money
        public Account CreateAccount(Person customer, Money initialDeposit)
        {
            if(customer.CanPay(initialDeposit))
            {
                accNum++;
                // Take money from customer
                customer.Pay(initialDeposit);
                // Create account
                Account newAccount = new Account(accNum, customer, initialDeposit);
                // Save reference to account
                AddToDict(newAccount);
                return newAccount;
            }
            else
            {
                throw new Exception(CannotAffordNewAccountMessage);
            }
        }

        // Returns an array of accounts for customer.
        // Returns an empty array if person is not a customer.
        public Account[] GetAccountsForCustomer(Person customer)
        {
            if (customerDict.ContainsKey(customer))
            {
                return customerDict[customer].ToArray();
            }
            else
            {
                return new Account[0];
            }
        }

        // Deposists money into account if owner can afford it
        public void Deposit(Account to, Money amount)
        {
            Person owner = to.GetOwner();
            if (owner.CanPay(amount))
            {
                owner.Pay(amount);
                to.Deposit(amount);
            }
            else
            {
                throw new Exception(CannotAffordDepositMessage);
            }
        }

        // Withdraws money from account to customer, if balance allows it
        public void Withdraw(Account from, Money amount)
        {
            if (from.CanWithdraw(amount))
            {
                from.Withdraw(amount);
                from.GetOwner().Receive(amount);
            }
            else
            {
                throw new Exception(BalanceTooLowMessage);
            }
        }

        // Transfers money between two accounts, if balance allows it
        public void Transfer(Account from, Account to, Money amount)
        {
            if (from.CanWithdraw(amount))
            {
                from.Withdraw(amount);
                to.Deposit(amount);
            }
            else
            {
                throw new Exception(BalanceTooLowMessage);
            }
        }

        // Private helper function for updating bank records
        private void AddToDict(Account account)
        {
            Person owner = account.GetOwner();
            if (!customerDict.ContainsKey(owner))
            {
                customerDict.Add(owner, new List<Account>());
            }
            customerDict[owner].Add(account);
        }
    }
}
