using System;
using MediatR;

namespace Before.Services
{
    public class ExpireOfferRequest : IRequest
    {
        public Guid OfferId { get; set; }
        public Guid MemberId { get; set; }
    } 
}