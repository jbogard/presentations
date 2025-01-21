using System;
using MediatR;

namespace Before.Features.Offers.AssignOffer
{
    public class AssignOfferRequest : IRequest
    {
        public Guid MemberId { get; set; }
        public Guid OfferTypeId { get; set; }
    }
}