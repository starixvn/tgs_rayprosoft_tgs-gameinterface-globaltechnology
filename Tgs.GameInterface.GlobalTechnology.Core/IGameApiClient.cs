using System.ComponentModel;
using Tgs.GameInterface.GlobalTechnology.Core.Models;

namespace Tgs.GameInterface.GlobalTechnology.Core;

public interface IGameApiClient
{
	ValueTask PlayerCreateAsync(Uri apiBaseUrl, int brandId, string secretKey, string playerId, string currency, string language, CancellationToken cancellationToken = default);

	ValueTask PlayerkickOutAsync(Uri apiBaseUrl, int brandId, string secretKey, string playerId, CancellationToken cancellationToken = default);

	ValueTask<PlayerGetTokenResp> PlayerGetTokenAsync(Uri apiBaseUrl, int brandId, string secretKey, string playerId, CancellationToken cancellationToken = default);

	ValueTask<PlayerBalanceResp> PlayerBalanceAsync(Uri apiBaseUrl, int brandId, string secretKey, string playerId, CancellationToken cancellationToken = default);

	IAsyncEnumerable<PlayerBalanceAllResp> PlayerBalanceAllAsync(Uri apiBaseUrl, int brandId, string secretKey, int page = 1, int size = 5000, CancellationToken cancellationToken = default);

	IAsyncEnumerable<BrandBalanceResp> PaymentBrandBalanceAsync(Uri apiBaseUrl, int brandId, string secretKey, CancellationToken cancellationToken = default);

	ValueTask<PaymentTransactionResp> PaymentPlayerDepositAsync(Uri apiBaseUrl, int brandId, string secretKey, string playerId, string currency, decimal amount, string extTransId, CancellationToken cancellationToken = default);

	ValueTask<PaymentTransactionResp> PaymentPlayerWithdrawalAsync(Uri apiBaseUrl, int brandId, string secretKey, string playerId, string currency, decimal amount, string extTransId, CancellationToken cancellationToken = default);

	ValueTask<bool> PaymentPlayerCheckTransAsync(Uri apiBaseUrl, int brandId, string secretKey, string extTransId, CancellationToken cancellationToken = default);

	IAsyncEnumerable<HistoryGameResp> HistoryGameAsync(Uri apiBaseUrl, int brandId, string secretKey, DateTimeOffset startTime, DateTimeOffset endTime, int page = 1, int size = 5000, CancellationToken cancellationToken = default);
}
