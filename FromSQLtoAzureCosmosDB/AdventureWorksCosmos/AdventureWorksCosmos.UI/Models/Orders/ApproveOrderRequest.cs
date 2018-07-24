using System;
using MediatR;

namespace AdventureWorksCosmos.UI.Models.Orders
{
    public class ApproveOrderRequest : IRequest
    {
        public Guid Id { get; set; }
    }
}