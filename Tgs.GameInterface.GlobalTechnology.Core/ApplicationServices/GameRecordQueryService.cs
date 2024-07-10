using System.Runtime.CompilerServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.GlobalTechnology.Core.ApplicationServices;

internal class GameRecordQueryService(
	IGameApiClient apiClient) : IQueryService<GameRecordQuery, GameRecordResult>
{
	public async IAsyncEnumerable<GameRecordResult> QueryAsync(GameRecordQuery query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var resps = apiClient.HistoryGameAsync(
			apiBaseUrl: query.ApiUrl,
			brandId: query.BrandId,
			secretKey: query.SecretKey,
			startTime: query.StartTime,
			endTime: query.EndTime,
			page: query.Page,
			size: query.Size,
			cancellationToken: cancellationToken);

		await foreach (var resp in resps)
		{
			yield return new GameRecordResult(
				PlayerCode: resp.PlayerCode,
				ExternalBetId: resp.ExternalBetId,
				ExternalGameId: resp.ExternalGameId,
				ExternalCreatedOn: resp.ExternalCreatedOn,
				ExternalClosedOn: resp.ExternalClosedOn,
				IsFreeGame: resp.RoundType.Equals("freegame", StringComparison.OrdinalIgnoreCase),
				IsBonusGame: resp.RoundType.Equals("bonusgame", StringComparison.OrdinalIgnoreCase),
				Bet: resp.Bet,
				Settle: resp.Settle,
				WinLoss: resp.Settle - resp.Bet - resp.Cancels,
				JackpotContribution: resp.JackpotContribution,
				JackpotWin: resp.JackpotWin,
				AdditionalData: resp.AdditionalData);
		}
	}
}
