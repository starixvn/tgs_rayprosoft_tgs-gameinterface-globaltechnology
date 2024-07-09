using System.Web;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.GlobalTechnology.Core.ApplicationServices;

internal class GameUrlGetService(
	IGameApiClient apiClient) : IGetService<GameUrlQuery, GameUrlResult>
{
	public async ValueTask<GameUrlResult> GetAsync(GameUrlQuery query, CancellationToken cancellationToken = default)
	{
		var resp = await apiClient.PlayerGetTokenAsync(
			apiBaseUrl: query.ApiUrl,
			brandId: query.BrandId,
			secretKey: query.SecretKey,
			playerId: query.PlayerCode,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		if (!resp.IsPlayerExist)
		{
			return new GameUrlResult(
				resp.IsPlayerExist,
				string.Empty);
		}

		var uri = new Uri(query.GameLauncherUrl, "launcher");
		var builder = new UriBuilder(uri);
		var queryString = HttpUtility.ParseQueryString(uri.Query);
		queryString["gameCode"] = query.GameCode;
		queryString["token"] = resp.Token;
		queryString["platform"] = query.IsMobile ? PlatformType.Mobile : PlatformType.Web;
		queryString["language"] = query.Language;
		queryString["playerId"] = query.PlayerCode;
		queryString["brandId"] = query.BrandId.ToString();
		queryString["mode"] = 1.ToString();
		queryString["currency"] = query.Currency.ToString();
		builder.Query = queryString.ToString();

		return new GameUrlResult(
			true,
			builder.ToString());
	}
}
