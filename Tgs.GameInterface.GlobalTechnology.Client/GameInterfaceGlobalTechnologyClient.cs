using System.Runtime.CompilerServices;
using Google.Protobuf.WellKnownTypes;
using Tgs.GameInterface.GlobalTechnology.Protos;

namespace Tgs.GameInterface.GlobalTechnology.Client;

internal sealed class GameInterfaceGlobalTechnologyClient(
	GameInterfaceGlobalTechnology.GameInterfaceGlobalTechnologyClient client)
	: IGameInterfaceGlobalTechnologyClient
{
	public async ValueTask<string> CheckingServiceAsync(string machineName, CancellationToken cancellationToken = default)
		=> (await client.CheckingServiceAsync(
			request: new CheckingServiceRequest
			{
				MachineName = machineName,
			},
			cancellationToken: cancellationToken)
		.ConfigureAwait(false)).MachineName;

	public async ValueTask<FundReply> FundInAsync(Uri apiUrl, int brandId, string secretKey, string playerCode, string currency, string orderId, decimal amount, CancellationToken cancellationToken = default)
		=> await client.FundInAsync(
			request: new FundRequest
			{
				ApiUrl = apiUrl.ToString(),
				BrandId = brandId,
				SecretKey = secretKey,
				PlayerCode = playerCode,
				Currency = currency,
				OrderId = orderId,
				Amount = amount,
			},
			cancellationToken: cancellationToken)
		.ConfigureAwait(false);

	public async ValueTask<FundReply> FundOutAsync(Uri apiUrl, int brandId, string secretKey, string playerCode, string currency, string orderId, decimal amount, CancellationToken cancellationToken = default)
		=> await client.FundOutAsync(
			request: new FundRequest
			{
				ApiUrl = apiUrl.ToString(),
				BrandId = brandId,
				SecretKey = secretKey,
				PlayerCode = playerCode,
				Currency = currency,
				OrderId = orderId,
				Amount = amount,
			},
			cancellationToken: cancellationToken)
		.ConfigureAwait(false);

	public async ValueTask<bool> FundStatusAsync(Uri apiUrl, int brandId, string secretKey, string orderId, CancellationToken cancellationToken = default)
		=> (await client.FundStatusAsync(
			request: new FundStatusRequest
			{
				ApiUrl = apiUrl.ToString(),
				BrandId = brandId,
				SecretKey = secretKey,
				OrderId = orderId
			},
			cancellationToken: cancellationToken)
		.ConfigureAwait(false)).Status;

	public async IAsyncEnumerable<GameRecordInfo> GameRecordAsync(Uri dataApiUrl, int brandId, string secretKey, DateTimeOffset startTime, DateTimeOffset endTime, int page = 1, int pageSize = 5000, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var reply = await client.GameRecordAsync(
			request: new GameRecordRequest
			{
				DataApiUrl = dataApiUrl.ToString(),
				BrandId = brandId,
				SecretKey = secretKey,
				StartTime = startTime.ToTimestamp(),
				EndTime = endTime.ToTimestamp(),
				Page = page,
				PageSize = pageSize
			},
		   cancellationToken: cancellationToken)
		   .ConfigureAwait(false);

		foreach (var info in reply.Items)
			yield return info;
	}

	public async ValueTask<GameRecordDetailReply> GameRecordDetailAsync(Uri dataApiUrl, int brandId, string secretKey, string externalProviderCode, string externalBetId, DateTimeOffset externalEndTime, string language = "CN", CancellationToken cancellationToken = default)
		=> await client.GameRecordDetailAsync(
			request: new GameRecordDetailRequest
			{
				DataApiUrl = dataApiUrl.ToString(),
				BrandId = brandId,
				SecretKey = secretKey,
				ExternalProviderCode = externalProviderCode,
				ExternalBetId = externalBetId,
				ExternalEndTime = externalEndTime.ToTimestamp(),
				Language = language,
			},
		   cancellationToken: cancellationToken)
		.ConfigureAwait(false);

	public async ValueTask<GameUrlReply> GameUrlAsync(Uri apiUrl, Uri launcherApiUrl, int brandId, string secretKey, string playerCode, string currency, string language, bool isMobile, string gameCode, CancellationToken cancellationToken = default)
		=> await client.GameUrlAsync(
			request: new GameUrlRequest
			{
				ApiUrl = apiUrl.ToString(),
				LauncherApiUrl = launcherApiUrl.ToString(),
				BrandId = brandId,
				SecretKey = secretKey,
				PlayerCode = playerCode,
				Currency = currency,
				Language = language,
				IsMobile = isMobile,
				GameCode = gameCode,
			},
		   cancellationToken: cancellationToken)
		.ConfigureAwait(false);

	public async ValueTask<GameUrlReply> GameUrlPlaytechByLiveAsync(Uri apiUrl, Uri launcherApiUrl, int brandId, string secretKey, string playerCode, string currency, string language, bool isMobile, CancellationToken cancellationToken = default)
		=> await client.GameUrlPlaytechByLiveAsync(
			request: new GameUrlPlaytechByLiveRequest
			{
				ApiUrl = apiUrl.ToString(),
				LauncherApiUrl = launcherApiUrl.ToString(),
				BrandId = brandId,
				SecretKey = secretKey,
				PlayerCode = playerCode,
				Currency = currency,
				Language = language,
				IsMobile = isMobile
			},
		   cancellationToken: cancellationToken)
		.ConfigureAwait(false);

	public async ValueTask<PlayerBalanceReply> PlayerBalanceAsync(Uri apiUrl, int brandId, string secretKey, string playerCode, CancellationToken cancellationToken = default)
		=> await client.PlayerBalanceAsync(
			request: new PlayerBalanceRequest
			{
				ApiUrl = apiUrl.ToString(),
				BrandId = brandId,
				SecretKey = secretKey,
				PlayerCode = playerCode
			},
		   cancellationToken: cancellationToken)
		.ConfigureAwait(false);

	public async ValueTask PlayerCreateAsync(Uri apiUrl, int brandId, string secretKey, string playerCode, string currency, string language, CancellationToken cancellationToken = default)
		=> _ = await client.PlayerCreateAsync(
			request: new PlayerCreateRequest
			{
				ApiUrl = apiUrl.ToString(),
				BrandId = brandId,
				SecretKey = secretKey,
				PlayerCode = playerCode,
				Currency = currency,
				Language = language
			},
			cancellationToken: cancellationToken)
		.ConfigureAwait(false);

	public async ValueTask PlayerLogoutAsync(Uri apiUrl, int brandId, string secretKey, string playerCode, CancellationToken cancellationToken = default)
		=> _ = await client.PlayerLogoutAsync(
			request: new PlayerLogoutRequest
			{
				ApiUrl = apiUrl.ToString(),
				BrandId = brandId,
				SecretKey = secretKey,
				PlayerCode = playerCode
			},
			cancellationToken: cancellationToken)
		.ConfigureAwait(false);
}
