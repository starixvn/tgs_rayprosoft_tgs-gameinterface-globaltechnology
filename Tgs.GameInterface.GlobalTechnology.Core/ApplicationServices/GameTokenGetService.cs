using Tgs.ServiceUtility;

namespace Tgs.GameInterface.GlobalTechnology.Core.ApplicationServices;

public class GameTokenGetService(IGameApiClient apiClient) : IGetService<GameTokenQuery, GameTokenResult>
{
	public async ValueTask<GameTokenResult> GetAsync(GameTokenQuery query, CancellationToken cancellationToken = default)
	{
		var resp = await apiClient.PlayerGetTokenAsync(
			apiBaseUrl: query.ApiUrl,
			brandId: query.BrandId,
			secretKey: query.SecretKey,
			playerId: query.PlayerCode,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return new GameTokenResult(
			resp.IsPlayerExist,
			resp.Token);
	}
}
