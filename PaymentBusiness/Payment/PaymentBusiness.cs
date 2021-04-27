using System;
using System.Threading.Tasks;
using AutoMapper;
using PaymentBusiness.DI;
using PaymentCommon.Interfaces;
using PaymentCommon.Models;
using PaymentCommon.Models.Response;
using PaymentCommon.Resources;
using PaymentEntities.Interfaces;

namespace PaymentBusiness.Payment
{
    /// <summary>
    /// Payment business.
    /// </summary>
    public class PaymentBusiness : IPaymentBusiness
    {
        /// <summary>
        /// Log manager ref.
        /// </summary>
        private readonly ILoggerManager _logger;

        /// <summary>
        /// Unit of work repo.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Auto mapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Payment gateway.
        /// </summary>
        private IPaymentGateway _paymentGateway;

        /// <summary>
        /// Payment gateway factory.
        /// </summary>
        private readonly Register.ServiceResolver _serviceAccessor;

        public PaymentBusiness(ILoggerManager logger, IUnitOfWork unitOfWork, IMapper mapper, Register.ServiceResolver serviceAccessor)
        {
            this._logger = logger;
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._serviceAccessor = serviceAccessor;
        }

        /// <summary>
        /// Process payment.
        /// </summary>
        /// <param name="paymentRequestModel"></param>
        /// <returns></returns>
        public async Task<PaymentStatus> ProcessPaymentAsync(PaymentRequestModel paymentRequestModel)
        {
            var result = PaymentStatus.Failed;
            var paymentEntity = new PaymentEntities.Entities.Payment();
            try
            {
                paymentEntity = this._mapper.Map<PaymentEntities.Entities.Payment>(paymentRequestModel);

                // First save with Pending status
                paymentEntity.PaymentStatusId = (int) PaymentStatus.Pending;
                await this._unitOfWork.PaymentsRepository.AddAsync(paymentEntity);
                result = PaymentStatus.Pending;
                _unitOfWork.Complete();  // call to get Id

                // call gateway and get the gateway response status (Processed/ Fail) 
                var status = await this.CallGatewayAsync(paymentRequestModel);
                result = status;

                // update payment status in DB
                paymentEntity.PaymentStatusId = (int) result;
                await this._unitOfWork.PaymentsRepository.UpdateAsync(paymentEntity);

                return result;
            }
            catch (Exception e)
            {
                this._logger.LogError(e);
                if (paymentEntity?.Id > 0)
                {
                    paymentEntity.PaymentStatusId = (int) result;
                    await this._unitOfWork.PaymentsRepository.UpdateAsync(paymentEntity);
                }

                return result;
            }
            finally
            {
                _unitOfWork.Complete();
            }
        }

        /// <summary>
        /// Call intended payment gateway.
        /// </summary>
        /// <param name="paymentRequestModel"></param>
        /// <returns></returns>
        private async Task<PaymentStatus> CallGatewayAsync(PaymentRequestModel paymentRequestModel)
        {
            var amount = paymentRequestModel.Amount;
            if (amount <= 20)
            {
                this._paymentGateway = this._serviceAccessor(Constants.CHEAP);
                return await this.CallGatewayAsync(paymentRequestModel, 0, Constants.CHEAP);
            }
            else if (amount > 20 && amount <= 500)  // In requirement some ranges are missing like range between 20 and 21 , here in the condition assuming some ranges.
            {
                this._paymentGateway = this._serviceAccessor(Constants.EXPENSIVE);
                if (await this._paymentGateway.IsAvailableAsync())
                {
                    return await this.CallGatewayAsync(paymentRequestModel, 0, Constants.EXPENSIVE);
                }
                else
                {
                    this._paymentGateway = this._serviceAccessor(Constants.CHEAP);
                    return await this.CallGatewayAsync(paymentRequestModel, 0, Constants.CHEAP);
                }
            }
            else //if (amount > 500)
            {
                this._paymentGateway = this._serviceAccessor(Constants.PREMIUM);
                return await this.CallGatewayAsync(paymentRequestModel, 3, Constants.PREMIUM);
            }
        }

        /// <summary>
        /// Payment gateway retry method.
        /// </summary>
        /// <param name="paymentRequestModel"></param>
        /// <param name="retry"></param>
        /// <param name="retryGatewayName"></param>
        /// <param name="isRetried"></param>
        /// <returns></returns>
        private async Task<PaymentStatus> CallGatewayAsync(PaymentRequestModel paymentRequestModel, int retry, string retryGatewayName, bool isRetried = false)
        {
            try
            {
                return await this._paymentGateway.ProcessPaymentAsync(paymentRequestModel);
            }
            catch (Exception e)
            {
                this._logger.LogError(e);
                if (retry > 0)
                {
                    if (!isRetried)  // Only get the another gateway first while first retry
                    {
                        this._paymentGateway = this._serviceAccessor(retryGatewayName);
                    }

                    return await this.CallGatewayAsync(paymentRequestModel, retry - 1, retryGatewayName, true);
                }
                else
                {
                    return PaymentStatus.Failed;
                }
            }
        }
    }
}
