using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BankNS;

namespace BankTests
{
    [TestClass]
    public class BankTests
    {
        [TestMethod]
        public void CreateAccount_WithEnoughCash_CreatesNewAccountWithInitialDeposit()
        {
            // arrange
            Bank bank = new Bank();
            Money cash = new Money(300);
            Money init = new Money(200);
            Person person = new Person(cash);

            // act
            Account account = bank.CreateAccount(person, init);

            // assert
            Assert.AreEqual(200, account.GetBalance().GetAmount());
        }

        [TestMethod]
        public void CreateAccount_WithEnoughCash_CreatesNewAccountWithCorrectOwner()
        {
            // arrange
            Bank bank = new Bank();
            Money cash = new Money(300);
            Money init = new Money(200);
            Person person = new Person(cash);
            Person otherPerson = new Person(cash);

            // act
            Account account = bank.CreateAccount(person, init);

            // assert
            Assert.AreEqual(person, account.GetOwner());
            Assert.AreNotEqual(otherPerson, account.GetOwner());
        }

        [TestMethod]
        public void CreateAccount_WithEnoughCash_CreatesNewAccountWithCorrectSerialNumber()
        {
            // arrange
            Bank bank = new Bank();
            Money cash = new Money(600);
            Money init = new Money(200);
            Person person = new Person(cash);
            Person otherPerson = new Person(cash);

            // act
            Account account1 = bank.CreateAccount(person, init);
            Account account2 = bank.CreateAccount(otherPerson, init);
            Account account3 = bank.CreateAccount(person, init);

            // assert
            Assert.AreEqual(1, account1.GetSerialNum());
            Assert.AreEqual(2, account2.GetSerialNum());
            Assert.AreEqual(3, account3.GetSerialNum());
        }

        [TestMethod]
        public void CreateAccount_WithoutEnoughCash_ThrowsExeption()
        {
            // arrange
            Bank bank = new Bank();
            Money cash = new Money(100);
            Money init = new Money(200);
            Person person = new Person(cash);

            // act
            try
            {
                bank.CreateAccount(person, init);
            }
            catch (Exception e)
            {
                // assert
                StringAssert.Contains(e.Message, Bank.CannotAffordNewAccountMessage);
                return;
            }
            Assert.Fail("No exception was thrown.");
        }

        [TestMethod]
        public void GetAccountsForCustomer_WithCustomer_ReturnsNonemptyArray()
        {
            // arrange
            Bank bank = new Bank();
            Money cash = new Money(400);
            Money init = new Money(200);
            Person person = new Person(cash);
            Account account0 = bank.CreateAccount(person, init);
            Account account1 = bank.CreateAccount(person, init);

            // act
            Account[] personAccounts = bank.GetAccountsForCustomer(person);

            // assert
            Assert.AreEqual(account0, personAccounts[0]);
            Assert.AreEqual(account1, personAccounts[1]);
            Assert.AreEqual(2, personAccounts.Length);
        }

        [TestMethod]
        public void GetAccountsForCustomer_WithNoncustomer_ReturnsEmptyArray()
        {
            // arrange
            Bank bank = new Bank();
            Money cash = new Money(400);
            Person person = new Person(cash);


            // act
            Account[] personAccounts = bank.GetAccountsForCustomer(person);

            // assert
            Assert.AreEqual(0, personAccounts.Length);
        }

        [TestMethod]
        public void Deposit_WithEnoughCash_UpdatesBalance()
        {
            // arrange
            Bank bank = new Bank();
            Money cash = new Money(400);
            Money init = new Money(200);
            Money deposit = new Money(100);
            Person person = new Person(cash);
            Account account = bank.CreateAccount(person, init);

            // act
            bank.Deposit(account, deposit);

            // assert
            Assert.AreEqual(300, account.GetBalance().GetAmount());
        }

        [TestMethod]
        public void Deposit_WithEnoughCash_TakesFromCash()
        {
            // arrange
            Bank bank = new Bank();
            Money cash = new Money(400);
            Money init = new Money(200);
            Money deposit = new Money(100);
            Person person = new Person(cash);
            Account account = bank.CreateAccount(person, init);

            // act
            bank.Deposit(account, deposit);

            // assert
            Assert.IsTrue(person.CanPay(new Money(100)) && !person.CanPay(new Money(101)));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Deposit_WithoutEnoughCash_ThrowsExeption()
        {
            // arrange
            Bank bank = new Bank();
            Money cash = new Money(250);
            Money init = new Money(200);
            Money deposit = new Money(100);
            Person person = new Person(cash);
            Account account = bank.CreateAccount(person, init);

            // act
            bank.Deposit(account, deposit);

            // assert: expects exeption
        }

        [TestMethod]
        public void Deposit_WithoutEnoughCash_DoesNotUpdateBalance()
        {
            // arrange
            Bank bank = new Bank();
            Money cash = new Money(250);
            Money init = new Money(200);
            Money deposit = new Money(100);
            Person person = new Person(cash);
            Account account = bank.CreateAccount(person, init);

            // act
            try
            {
                bank.Deposit(account, deposit);
            }
            catch (Exception)
            {

            }

            // assert
            Assert.AreEqual(200, account.GetBalance().GetAmount());
        }

        [TestMethod]
        public void Withdraw_WithEnoughBalance_UpdatesBalance()
        {
            // arrange
            Bank bank = new Bank();
            Money cash = new Money(400);
            Money init = new Money(200);
            Money amount = new Money(100);
            Person person = new Person(cash);
            Account account = bank.CreateAccount(person, init);

            // act
            bank.Withdraw(account, amount);

            // assert
            Assert.AreEqual(100, account.GetBalance().GetAmount());
        }

        [TestMethod]
        public void Withdraw_WithEnoughBalance_UpdatesCash()
        {
            // arrange
            Bank bank = new Bank();
            Money cash = new Money(400);
            Money init = new Money(200);
            Money amount = new Money(100);
            Person person = new Person(cash);
            Account account = bank.CreateAccount(person, init);

            // act
            bank.Withdraw(account, amount);

            // assert
            Assert.IsTrue(person.CanPay(new Money(300)) && !person.CanPay(new Money(301)));
        }

        [TestMethod]
        public void Withdraw_WithoutEnoughBalance_DoesNotUpdateBalance()
        {
            // arrange
            Bank bank = new Bank();
            Money cash = new Money(400);
            Money init = new Money(200);
            Money amount = new Money(300);
            Person person = new Person(cash);
            Account account = bank.CreateAccount(person, init);

            // act
            try
            {
                bank.Withdraw(account, amount);
            }
            catch (Exception)
            {

            }

            // assert
            Assert.AreEqual(200, account.GetBalance().GetAmount());
        }

        [TestMethod]
        public void Withdraw_WithoutEnoughBalance_DoesNotUpdateCash()
        {
            // arrange
            Bank bank = new Bank();
            Money cash = new Money(400);
            Money init = new Money(200);
            Money amount = new Money(300);
            Person person = new Person(cash);
            Account account = bank.CreateAccount(person, init);

            // act
            try
            {
                bank.Withdraw(account, amount);
            }
            catch (Exception)
            {

            }

            // assert
            Assert.IsTrue(person.CanPay(new Money(200)) && !person.CanPay(new Money(201)));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Withdraw_WithoutEnoughBalance_TrowsExeption()
        {
            // arrange
            Bank bank = new Bank();
            Money cash = new Money(400);
            Money init = new Money(200);
            Money amount = new Money(300);
            Person person = new Person(cash);
            Account account = bank.CreateAccount(person, init);

            // act
            bank.Withdraw(account, amount);

            // assert: expects exception
        }

        [TestMethod]
        public void Transfer_WithEnoughBalance_UpdatesFromBalance()
        {
            // arrange
            Bank bank = new Bank();
            Money cash = new Money(400);
            Money init = new Money(200);
            Money amount = new Money(100);
            Person person = new Person(cash);
            Account account1 = bank.CreateAccount(person, init);
            Account account2 = bank.CreateAccount(person, init);

            // act
            bank.Transfer(account1, account2, amount);

            // assert
            Assert.AreEqual(100, account1.GetBalance().GetAmount());
        }

        [TestMethod]
        public void Transfer_WithEnoughBalance_UpdatesToBalance()
        {
            // arrange
            Bank bank = new Bank();
            Money cash = new Money(400);
            Money init = new Money(200);
            Money amount = new Money(100);
            Person person = new Person(cash);
            Account account1 = bank.CreateAccount(person, init);
            Account account2 = bank.CreateAccount(person, init);

            // act
            bank.Transfer(account1, account2, amount);

            // assert
            Assert.AreEqual(300, account2.GetBalance().GetAmount());
        }

        [TestMethod]
        public void Transfer_WithoutEnoughBalance_DoesNotUpdateFromBalance()
        {
            // arrange
            Bank bank = new Bank();
            Money cash = new Money(400);
            Money init = new Money(200);
            Money amount = new Money(300);
            Person person = new Person(cash);
            Account account1 = bank.CreateAccount(person, init);
            Account account2 = bank.CreateAccount(person, init);

            // act
            try
            {
                bank.Transfer(account1, account2, amount);
            }
            catch (Exception)
            {

            }

            // assert
            Assert.AreEqual(200, account1.GetBalance().GetAmount());
        }

        [TestMethod]
        public void Transfer_WithoutEnoughBalance_DoesNotUpdateToBalance()
        {
            // arrange
            Bank bank = new Bank();
            Money cash = new Money(400);
            Money init = new Money(200);
            Money amount = new Money(300);
            Person person = new Person(cash);
            Account account1 = bank.CreateAccount(person, init);
            Account account2 = bank.CreateAccount(person, init);

            // act
            try
            {
                bank.Transfer(account1, account2, amount);
            }
            catch (Exception)
            {

            }

            // assert
            Assert.AreEqual(200, account2.GetBalance().GetAmount());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Transfer_WithoutEnoughBalance_TrowsExeption()
        {
            // arrange
            Bank bank = new Bank();
            Money cash = new Money(400);
            Money init = new Money(200);
            Money amount = new Money(300);
            Person person = new Person(cash);
            Account account1 = bank.CreateAccount(person, init);
            Account account2 = bank.CreateAccount(person, init);

            // act
            bank.Transfer(account1, account2, amount);


            // assert: expects exception
        }
    }
}
