using System.Collections.Generic;

namespace Before.Model
{
	public class Member : Entity
	{
		public Member()
		{
			AssignedOffers = new List<Offer>();
		}

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public ICollection<Offer> AssignedOffers { get; set; }
		public int NumberOfActiveOffers { get; set; }
	}
}