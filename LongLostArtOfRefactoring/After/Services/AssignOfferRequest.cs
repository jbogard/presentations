using System;

namespace After.Services;

public class AssignOfferRequest
{
    public Guid MemberId { get; set; }
    public Guid OfferTypeId { get; set; }
}