using Tgs.ServiceUtility;

namespace Tgs.GameInterface.GlobalTechnology.Core.ApplicationServices;

internal class GameRecordDetailGetService(
	IGameApiClient apiClient) : IGetService<GameRecordDetailQuery, GameRecordDetailResult>
{
	public async ValueTask<GameRecordDetailResult> GetAsync(GameRecordDetailQuery query, CancellationToken cancellationToken = default)
	{
		var resp = await apiClient.HistoryRoundDetailAsync(
			apiBaseUrl: query.ApiUrl,
			brandId: query.BrandId,
			secretKey: query.SecretKey,
			providerCode: query.ExternalProviderCode,
			roundId: query.ExternalBetId,
			endTime: query.ExternalEndTime,
			language: query.Language,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return new GameRecordDetailResult(
			IsSupport: resp.IsSupport,
			DetailUrl: resp.DetailUrl);
	}
}
