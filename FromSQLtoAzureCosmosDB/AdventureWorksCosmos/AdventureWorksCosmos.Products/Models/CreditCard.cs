using System;
using System.Collections.Generic;

namespace AdventureWorksCosmos.Products.Models
{
    public partial class CreditCard
    {
        public CreditCard()
        {
            PersonCreditCard = new HashSet<PersonCreditCard>();
            SalesOrderHeader = new HashSet<SalesOrderHeader>();
        }

        public int CreditCardId { get; set; }
        public string CardType { get; set; }
        public string CardNumber { get; set; }
        public byte ExpMonth { get; set; }
        public short ExpYear { get; set; }
        public DateTime ModifiedDate { get; set; }

        public ICollection<PersonCreditCard> PersonCreditCard { get; set; }
        public ICollection<SalesOrderHeader> SalesOrderHeader { get; set; }
    }
}
