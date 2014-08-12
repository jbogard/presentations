using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleAfter
{
    using StructureMap;

    class Program
    {
        static void Main(string[] args)
        {
            var container = new Container(cfg =>
            {
                cfg.Scan(scanner =>
                {
                    scanner.AddAllTypesOf<IDiscountApplier>();
                    scanner.AssemblyContainingType<Program>();
                    scanner.WithDefaultConventions();
                });
                cfg.For<IInvoiceApprover>()
                    .DecorateAllWith<LoggingApprover>();
            });
            var approver = container.GetInstance<IInvoiceApprover>();
            var invoice = new Invoice
            {
                Total = 2000m,
                Customer = "Joe",
                State = "KC"
            };

            approver.Approve(invoice);
        }
    }

    public interface IInvoiceApprover
    {
        void Approve(Invoice invoice);
    }

    public class LoggingApprover : IInvoiceApprover
    {
        private readonly IInvoiceApprover _inner;

        public LoggingApprover(IInvoiceApprover inner)
        {
            _inner = inner;
        }


        public void Approve(Invoice invoice)
        {
            Console.WriteLine("Starting approval for " + invoice.Customer);
            _inner.Approve(invoice);
            Console.WriteLine("Finished approval for " + invoice.Customer);
        }
    }

    public class InvoiceApprover : IInvoiceApprover
    {
        private readonly IEnumerable<IDiscountApplier> _discounters;
        private readonly IEmailSender _emailSender;

        public InvoiceApprover(IEnumerable<IDiscountApplier> discounters,
            IEmailSender emailSender)
        {
            _discounters = discounters;
            _emailSender = emailSender;
        }

        public void Approve(Invoice invoice)
        {
            decimal discount = _discounters
                .Aggregate(0m, (previous, discounter) 
                    => discounter.Calculate(invoice) + previous);

            invoice.ApprovedAmount = invoice.Total - discount;

            _emailSender.Send("Approved " + invoice.ApprovedAmount + " for " + invoice.Customer);
        }
    }

    public interface IDiscountApplier
    {
        decimal Calculate(Invoice invoice);
    }

    public class LargeTotalDiscountApplier : IDiscountApplier
    {
        public decimal Calculate(Invoice invoice)
        {
            if (invoice.Total > 1000m)
            {
                return 50m;
            }
            return 0m;
        }
    }

    public class JoeDiscountApplier : IDiscountApplier
    {
        public decimal Calculate(Invoice invoice)
        {
            if (invoice.Customer == "Joe")
            {
                return 10m;
            }
            return 0m;
        }
    }

    public class KCDiscountApplier : IDiscountApplier
    {
        public decimal Calculate(Invoice invoice)
        {
            if (invoice.State == "KC")
            {
                return invoice.Total * 0.1m;
            }
            return 0m;
        }
    }

    public interface IEmailSender
    {
        void Send(string message);
    }

    public class EmailSender : IEmailSender
    {
        public void Send(string message)
        {
            Console.WriteLine(message);
            // Send email
        }
    }

    public class Invoice
    {
        public string Customer { get; set; }
        public decimal ApprovedAmount { get; set; }
        public decimal Total { get; set; }
        public string State { get; set; }
    }
}
