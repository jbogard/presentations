using System;
using After.Model;

namespace After.Services
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
			// Retreive
			var member = _memberRepository.GetById(memberId);
			var offerType = _offerTypeRepository.GetById(offerTypeId);

			// Delegate to business objects
			var offer = member.AssignOffer(offerType, _offerValueCalculator);

			// Save
			_offerRepository.Save(offer);
		}
	}
}