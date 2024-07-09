using Tgs.ServiceUtility;

namespace Tgs.GameInterface.GlobalTechnology.Core.ApplicationServices;

public class PlayerBalanceGetService(IGameApiClient apiClient) : IGetService<PlayerBalanceQuery, PlayerBalanceResult>
{
	public async ValueTask<PlayerBalanceResult> GetAsync(PlayerBalanceQuery query, CancellationToken cancellationToken = default)
	{
		var resp = await apiClient.PlayerBalanceAsync(
			apiBaseUrl: query.ApiUrl,
			brandId: query.BrandId,
			secretKey: query.SecretKey,
			playerId: query.PlayerCode,
			cancellationToken)
			.ConfigureAwait(false);

		return new PlayerBalanceResult(
			IsPlayerExist: resp.IsPlayerExist,
			PlayerCode: resp.PlayerId,
			Currency: resp.Currency,
			Balance: resp.Balance,
			BonusBalance: resp.BonusBalance,
			BrandId: resp.BrandId);
	}
}
