using System;
using System.Net;
using System.Reflection;
using NUnit.Framework;
using www.BankBals.Controllers;

namespace www.BankBals.Tests {

    [TestFixture]
    public class AccountTest {

        //public const string SERVER_ADDRESS = "http://192.168.1.12";
        public const string SERVER_ADDRESS = "http://bankiru.net.192-185-6-162.secure2.win.hostgator.com";
        //public const string SERVER_ADDRESS = "http://localhost:10627";

        [Test]
        public void ResultCodes0() {
            Tools.TestResultCode(typeof(HomeController));
        }

        [Test]
        public void ResultCodes1() {
            Tools.TestResultCode(typeof(BankController));
        }

        [Test]
        public void ResultCodes2() {
            Tools.TestResultCode(typeof(ViewController));
        }

        [Test]
        public void ResultCodes3() {
            Tools.TestResultCode(typeof(ComparisonController));
        }

        [Test]
        public void ResultCodes4() {
            Tools.TestResultCode(typeof(APIController));
        }

        [Test]
        public void ResultCodes5() {
            Tools.TestResultCode(typeof(ServiceController));
        }

        [Test]
        public void ResultCodes6() {
            //Tools.TestResultCode(typeof(AccountController));
        }

        [Test]
        public void ResultCodes7() {
            //Tools.TestResultCode(typeof(AdminController));
        }

        /*
        [Test]
        public void TransferFunds() {
            Account source = new Account();
            source.Deposit(200m);

            Account destination = new Account();
            destination.Deposit(150m);

            source.TransferFunds(destination, 100m);

            Assert.AreEqual(250m, destination.Balance);
            Assert.AreEqual(100m, source.Balance);
        }

        [Test]
        [ExpectedException(typeof(InsufficientFundsException))]
        public void TransferWithInsufficientFunds() {
            Account source = new Account();
            source.Deposit(200m);

            Account destination = new Account();
            destination.Deposit(150m);

            source.TransferFunds(destination, 300m);
        }

        [Test]
        public void TransferWithInsufficientFundsAtomicity() {
            Account source = new Account();
            source.Deposit(200m);

            Account destination = new Account();
            destination.Deposit(150m);

            try {
                source.TransferFunds(destination, 300m);
            } catch (InsufficientFundsException expected) {
            }

            Assert.AreEqual(200m, source.Balance);
            Assert.AreEqual(150m, destination.Balance);
        }*/
    }

    /*
     cheatsheet
     
     */
    public class InsufficientFundsException : ApplicationException { }

    public class Account {
        private decimal balance;
        private decimal minimumBalance = 10m;

        public decimal MinimumBalance {
            get { return minimumBalance; }
        }

        public void Deposit(decimal amount) {
            balance += amount;
        }

        public void Withdraw(decimal amount) {
            balance -= amount;
        }

        public void TransferFunds(Account destination, decimal amount) {
            if (balance - amount < minimumBalance)
                throw new InsufficientFundsException();

            destination.Deposit(amount);

            Withdraw(amount);
        }

        public decimal Balance {
            get { return balance; }
        }
    }

}