using Tgs.ServiceUtility;

namespace Tgs.GameInterface.GlobalTechnology.Core.ApplicationServices;

public class FundOutGetService(IGameApiClient apiClient) : IGetService<FundOutQuery, FundResult>
{
	public async ValueTask<FundResult> GetAsync(FundOutQuery query, CancellationToken cancellationToken = default)
	{
		var resp = await apiClient.PaymentPlayerDepositAsync(
			apiBaseUrl: query.ApiUrl,
			brandId: query.BrandId,
			secretKey: query.SecretKey,
			playerId: query.PlayerCode,
			currency: query.Currency,
			amount: query.Amount,
			extTransId: query.OrderId,
			cancellationToken)
			.ConfigureAwait(false);

		return new FundResult(
			OrderId: resp.ExtTransId,
			ExternalOrderId: resp.TransId,
			Balance: resp.Balance,
			Resp: resp.Resp);
	}
}
