using System;
using System.Collections.Generic;
using After.Services;

namespace After.Model
{
	public class Member : Entity
	{
		private readonly IList<Offer> _assignedOffers
			= new List<Offer>();

		private string _firstName;

		public Member(string firstName, string lastName, string email)
		{
			FirstName = firstName;
			LastName = lastName;
			Email = email;
		}

		public string FirstName
		{
			get { return _firstName; }
			set
			{
				if (value == null)
					throw new ArgumentNullException("value", "First name is required");

				_firstName = value;
			}
		}

		public string LastName { get; set; }

		public string FullName
		{
			get { return FirstName + " " + LastName; }
		}

		public string Email { get; set; }
		public int NumberOfActiveOffers { get; private set; }

		public IEnumerable<Offer> AssignedOffers
		{
			get { return _assignedOffers; }
		}

		public Offer AssignOffer(OfferType offerType, IOfferValueCalculator valueCalculator)
		{
			DateTime dateExpiring = offerType.CalculateExpirationDate();
			int value = valueCalculator.CalculateValue(this, offerType);

			var offer = new Offer(this, offerType, dateExpiring, value);

			_assignedOffers.Add(offer);

			NumberOfActiveOffers++;

			return offer;
		}
	}
}