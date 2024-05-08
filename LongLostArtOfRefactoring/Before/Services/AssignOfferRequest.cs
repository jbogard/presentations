using System;
using MediatR;

namespace Before.Services
{
    public class AssignOfferRequest : IRequest
    {
        public Guid MemberId { get; set; }
        public Guid OfferTypeId { get; set; }
    }
}