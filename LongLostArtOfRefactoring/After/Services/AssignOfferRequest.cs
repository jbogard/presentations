using System;
using MediatR;

namespace After.Services
{
    public class AssignOfferRequest : IRequest
    {
        public Guid MemberId { get; set; }
        public Guid OfferTypeId { get; set; }
    }
}