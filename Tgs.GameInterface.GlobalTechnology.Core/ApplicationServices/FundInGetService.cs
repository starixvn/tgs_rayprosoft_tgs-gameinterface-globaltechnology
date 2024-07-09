using Tgs.ServiceUtility;

namespace Tgs.GameInterface.GlobalTechnology.Core.ApplicationServices;

public class FundInGetService(IGameApiClient apiClient) : IGetService<FundInQuery, FundResult>
{
	public async ValueTask<FundResult> GetAsync(FundInQuery query, CancellationToken cancellationToken = default)
	{
		var resp = await apiClient.PaymentPlayerWithdrawalAsync(
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
