using System;

namespace ExampleBefore
{
    using System.Collections.Generic;
    using System.Linq;
    using StructureMap;

    class Program
    {
        static void Main(string[] args)
        {
            var container = new Container(cfg =>
            {
                cfg.Scan(scanner =>
                {
                    scanner.AddAllTypesOf<IDiscounter>();

                    scanner.AssemblyContainingType<Program>();
                    scanner.WithDefaultConventions();
                });

                cfg.For<IInvoiceApprover>().DecorateAllWith<LoggingInvoiceApprover>();
                cfg.ForSingletonOf<EmailSender>().Use<EmailSender>();
            });

            var approver = container.GetInstance<IInvoiceApprover>();
            var invoice = new Invoice
            {
                Total = 2000m,
                Customer = "Joe",
                State = "KS"
            };

            approver.Approve(invoice);
        }
    }AS

    public interface IInvoiceApprover
    {
        void Approve(Invoice invoice);
    }

    public class LoggingInvoiceApprover : IInvoiceApprover
    {
        private readonly IInvoiceApprover _inner;

        public LoggingInvoiceApprover(IInvoiceApprover inner)
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

    public interface IDiscounter
    {
        decimal Apply(Invoice invoice);
    }

    public class KansasDiscountRule : IDiscounter
    {
        public decimal Apply(Invoice invoice)
        {
            if (invoice.State == "KS")
            {
                return invoice.Total * 0.1m;
            }
            return 0m;
        }
    }

    public class JoeDiscountRule : IDiscounter
    {
        public decimal Apply(Invoice invoice)
        {
            if (invoice.Customer == "Joe")
            {
                return 10m;
            }
            return 0m;
        }
    }

    public class LargeOrderDiscountRule : IDiscounter
    {
        public decimal Apply(Invoice invoice)
        {
            if (invoice.Total > 1000m)
            {
                return 50m;
            }
            return 0m;
        }
    }

    public class InvoiceApprover : IInvoiceApprover
    {
        private readonly IEnumerable<IDiscounter> _discounters;
        private readonly EmailSender _emailSender;

        public InvoiceApprover(IEnumerable<IDiscounter> discounters, EmailSender emailSender)
        {
            _discounters = discounters;
            _emailSender = emailSender;
        }

        public void Approve(Invoice invoice)
        {
            decimal discount = _discounters
                .Aggregate(0m, (current, discounter) 
                    => current + discounter.Apply(invoice));

            invoice.ApprovedAmount = invoice.Total - discount;

            _emailSender.Send("Approved " + invoice.ApprovedAmount + " for " + invoice.Customer);
        }
    }

    public class EmailSender
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
