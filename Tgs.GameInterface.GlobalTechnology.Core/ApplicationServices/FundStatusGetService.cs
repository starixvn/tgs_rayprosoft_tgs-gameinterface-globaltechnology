using Tgs.ServiceUtility;

namespace Tgs.GameInterface.GlobalTechnology.Core.ApplicationServices;

public class FundStatusGetService(IGameApiClient apiClient) : IGetService<FundStatusQuery, bool>
{
	public ValueTask<bool> GetAsync(FundStatusQuery query, CancellationToken cancellationToken = default)
	=> apiClient.PaymentPlayerCheckTransAsync(
		apiBaseUrl: query.ApiUrl,
		brandId: query.BrandId,
		secretKey: query.SecretKey,
		extTransId: query.OrderId,
		cancellationToken);
}
