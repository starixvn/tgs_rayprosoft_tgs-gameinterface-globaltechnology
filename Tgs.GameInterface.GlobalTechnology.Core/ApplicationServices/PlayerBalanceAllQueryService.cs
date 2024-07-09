using System.Runtime.CompilerServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.GlobalTechnology.Core.ApplicationServices;

public class PlayerBalanceAllQueryService(IGameApiClient apiClient) : IQueryService<PlayerBalanceAllQuery, PlayerBalanceAllResult>
{
	public async IAsyncEnumerable<PlayerBalanceAllResult> QueryAsync(PlayerBalanceAllQuery query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var resps = apiClient.PlayerBalanceAllAsync(
			apiBaseUrl: query.ApiUrl,
			brandId: query.BrandId,
			secretKey: query.SecretKey,
			page: query.Page,
			size: query.Size,
			cancellationToken);

		await foreach (var resp in resps)
		{
			yield return new PlayerBalanceAllResult(
				PlayerCode: resp.PlayerId,
				Currency: resp.Currency,
				Balance: resp.Balance,
				BonusBalance: resp.BonusBalance,
				BrandId: resp.BrandId);
		}
	}
}
