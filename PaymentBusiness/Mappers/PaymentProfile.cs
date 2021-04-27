using AutoMapper;
using PaymentCommon.Models;

namespace PaymentBusiness.Mappers
{
    /// <summary>
    /// AutoMapper profile.
    /// </summary>
	public class PaymentProfile : Profile
    {
        /// <summary>
        /// Dto to model mapping and vice versa.
        /// </summary>
        public PaymentProfile()
        {
            CreateMap<PaymentEntities.Entities.Payment, PaymentRequestModel>().ReverseMap();
        }
    }
}
