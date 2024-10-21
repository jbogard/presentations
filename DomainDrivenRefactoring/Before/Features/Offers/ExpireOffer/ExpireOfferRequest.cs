using System;
using MediatR;

namespace Before.Features.Offers.ExpireOffer
{
    public class ExpireOfferRequest : IRequest
    {
        public Guid OfferId { get; set; }
        public Guid MemberId { get; set; }
    } 
}