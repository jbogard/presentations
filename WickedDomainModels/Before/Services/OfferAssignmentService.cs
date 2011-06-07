using System;
using Before.Model;

namespace Before.Services
{
	public interface IMemberRepository
	{
		Member GetById(Guid id);
	}

	public interface IOfferTypeRepository
	{
		OfferType GetById(Guid id);
	}

	public interface IOfferRepository
	{
		void Save(Offer offer);
	}

	public interface IOfferValueCalculator
	{
		int CalculateValue(Member member, OfferType offerType);
	}

	public class OfferAssignmentService
	{
		private readonly IMemberRepository _memberRepository;
		private readonly IOfferTypeRepository _offerTypeRepository;
		private readonly IOfferValueCalculator _offerValueCalculator;
		private readonly IOfferRepository _offerRepository;

		public OfferAssignmentService(
			IMemberRepository memberRepository,
			IOfferTypeRepository offerTypeRepository,
			IOfferValueCalculator offerValueCalculator,
			IOfferRepository offerRepository
			)
		{
			_memberRepository = memberRepository;
			_offerTypeRepository = offerTypeRepository;
			_offerValueCalculator = offerValueCalculator;
			_offerRepository = offerRepository;
		}

		public void AssignOffer(Guid memberId, Guid offerTypeId)
		{
			var member = _memberRepository.GetById(memberId);
			var offerType = _offerTypeRepository.GetById(offerTypeId);

			var value = _offerValueCalculator
				.CalculateValue(member, offerType);

			DateTime dateExpiring;

			switch (offerType.ExpirationType)
			{
				case ExpirationType.Assignment:
					dateExpiring = DateTime.Now.AddDays(offerType.DaysValid);
					break;
				case ExpirationType.Fixed:
					if (offerType.BeginDate != null)
						dateExpiring =
							offerType.BeginDate.Value.AddDays(offerType.DaysValid);
					else
						throw new InvalidOperationException();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			var offer = new Offer
			{
				MemberAssigned = member,
				Type = offerType,
				Value = value,
				DateExpiring = dateExpiring
			};
			member.AssignedOffers.Add(offer);
			member.NumberOfActiveOffers++;

			_offerRepository.Save(offer);
		}
	}
}