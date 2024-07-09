using Tgs.GameInterface.GlobalTechnology.Protos;

namespace Tgs.GameInterface.GlobalTechnology.Client;

public interface IGameInterfaceGlobalTechnologyClient
{
	ValueTask<string> CheckingServiceAsync(string machineName, CancellationToken cancellationToken = default);

	ValueTask<FundReply> FundInAsync(Uri apiUrl, int brandId, string secretKey, string playerCode, string currency, string orderId, decimal amount, CancellationToken cancellationToken = default);

	ValueTask<FundReply> FundOutAsync(Uri apiUrl, int brandId, string secretKey, string playerCode, string currency, string orderId, decimal amount, CancellationToken cancellationToken = default);

	ValueTask<bool> FundStatusAsync(Uri apiUrl, int brandId, string secretKey, string orderId, CancellationToken cancellationToken = default);

	IAsyncEnumerable<GameRecordInfo> GameRecordAsync(Uri dataApiUrl, int brandId, string secretKey, DateTimeOffset startTime, DateTimeOffset endTime, int page = 1, int pageSize = 5000, CancellationToken cancellationToken = default);

	ValueTask<GameUrlReply> GameUrlAsync(Uri apiUrl, Uri launcherApiUrl, int brandId, string secretKey, string playerCode, string currency, string language, bool isMobile, string gameCode, CancellationToken cancellationToken = default);

	ValueTask<GameUrlReply> GameUrlPlaytechByLiveAsync(Uri apiUrl, Uri launcherApiUrl, int brandId, string secretKey, string playerCode, string currency, string language, bool isMobile, CancellationToken cancellationToken = default);

	ValueTask<PlayerBalanceReply> PlayerBalanceAsync(Uri apiUrl, int brandId, string secretKey, string playerCode, CancellationToken cancellationToken = default);

	ValueTask PlayerCreateAsync(Uri apiUrl, int brandId, string secretKey, string playerCode, string currency, string language, CancellationToken cancellationToken = default);

	ValueTask PlayerLogoutAsync(Uri apiUrl, int brandId, string secretKey, string playerCode, CancellationToken cancellationToken = default);
}
