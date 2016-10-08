using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankNS
{
    public class Money
    {
        private decimal amount;

        // Constructor
        public Money(decimal amount)
        {
            this.amount = amount;
        }

        // Returns size of money
        public decimal GetAmount()
        {
            return amount;
        }
    }
}
