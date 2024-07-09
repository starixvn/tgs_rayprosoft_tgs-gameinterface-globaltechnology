using System.Runtime.CompilerServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.GlobalTechnology.Core.ApplicationServices;

public class FundBrandBalanceQueryService(IGameApiClient apiClient) : IQueryService<FundBrandBalanceQuery, FundBrandBalanceResult>
{
	public async IAsyncEnumerable<FundBrandBalanceResult> QueryAsync(FundBrandBalanceQuery query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var resps = apiClient.PaymentBrandBalanceAsync(
			apiBaseUrl: query.ApiUrl,
			brandId: query.BrandId,
			secretKey: query.SecretKey,
			cancellationToken);

		await foreach (var resp in resps)
		{
			yield return new FundBrandBalanceResult(
				BrandId: resp.BrandId,
				Currency: resp.Currency,
				Balance: resp.Balance,
				LastTransId: resp.LastTransId,
				UpdateAt: resp.UpdateAt);
		}
	}
}
