using System.Runtime.CompilerServices;
using Google.Protobuf.WellKnownTypes;
using Tgs.GameInterface.GlobalTechnology.Protos;

namespace Tgs.GameInterface.GlobalTechnology.Client;

internal sealed class GameInterfaceGlobalTechnologyClient(
	GameInterfaceGlobalTechnology.GameInterfaceGlobalTechnologyClient client)
	: IGameInterfaceGlobalTechnologyClient
{
	public async ValueTask<string> CheckingServiceAsync(string machineName, CancellationToken cancellationToken = default)
	{
		var request = new CheckingServiceRequest
		{
			MachineName = machineName,
		};

		var reply = await client.CheckingServiceAsync(
				request: request,
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return reply.MachineName;
	}

	public async ValueTask<FundReply> FundInAsync(Uri apiUrl, int brandId, string secretKey, string playerCode, string currency, string orderId, decimal amount, CancellationToken cancellationToken = default)
	{
		var request = new FundRequest
		{
			ApiUrl = apiUrl.ToString(),
			BrandId = brandId,
			SecretKey = secretKey,
			PlayerCode = playerCode,
			Currency = currency,
			OrderId = orderId,
			Amount = amount,
		};

		return await client.FundInAsync(
			request: request,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask<FundReply> FundOutAsync(Uri apiUrl, int brandId, string secretKey, string playerCode, string currency, string orderId, decimal amount, CancellationToken cancellationToken = default)
	{
		var request = new FundRequest
		{
			ApiUrl = apiUrl.ToString(),
			BrandId = brandId,
			SecretKey = secretKey,
			PlayerCode = playerCode,
			Currency = currency,
			OrderId = orderId,
			Amount = amount,
		};

		return await client.FundOutAsync(
			request: request,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask<bool> FundStatusAsync(Uri apiUrl, int brandId, string secretKey, string orderId, CancellationToken cancellationToken = default)
	{
		var request = new FundStatusRequest
		{
			ApiUrl = apiUrl.ToString(),
			BrandId = brandId,
			SecretKey = secretKey,
			OrderId = orderId
		};

		var reply = await client.FundStatusAsync(
			request: request,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return reply.Status;
	}

	public async IAsyncEnumerable<GameRecordInfo> GameRecordAsync(Uri dataApiUrl, int brandId, string secretKey, DateTimeOffset startTime, DateTimeOffset endTime, int page = 1, int pageSize = 5000, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var request = new GameRecordRequest
		{
			DataApiUrl = dataApiUrl.ToString(),
			BrandId = brandId,
			SecretKey = secretKey,
			StartTime = startTime.ToTimestamp(),
			EndTime = endTime.ToTimestamp(),
			Page = page,
			PageSize = pageSize
		};

		var reply = await client.GameRecordAsync(
			request: request,
		   cancellationToken: cancellationToken)
		   .ConfigureAwait(false);

		foreach (var info in reply.Items)
			yield return info;
	}

	public async ValueTask<GameUrlReply> GameUrlAsync(Uri apiUrl, Uri launcherApiUrl, int brandId, string secretKey, string playerCode, string currency, string language, bool isMobile, string gameCode, CancellationToken cancellationToken = default)
	{
		var request = new GameUrlRequest
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
		};

		return await client.GameUrlAsync(
			request: request,
		   cancellationToken: cancellationToken)
		   .ConfigureAwait(false);
	}

	public async ValueTask<GameUrlReply> GameUrlPlaytechByLiveAsync(Uri apiUrl, Uri launcherApiUrl, int brandId, string secretKey, string playerCode, string currency, string language, bool isMobile, CancellationToken cancellationToken = default)
	{
		var request = new GameUrlPlaytechByLiveRequest
		{
			ApiUrl = apiUrl.ToString(),
			LauncherApiUrl = launcherApiUrl.ToString(),
			BrandId = brandId,
			SecretKey = secretKey,
			PlayerCode = playerCode,
			Currency = currency,
			Language = language,
			IsMobile = isMobile
		};

		return await client.GameUrlPlaytechByLiveAsync(
			request: request,
		   cancellationToken: cancellationToken)
		   .ConfigureAwait(false);
	}

	public async ValueTask<PlayerBalanceReply> PlayerBalanceAsync(Uri apiUrl, int brandId, string secretKey, string playerCode, CancellationToken cancellationToken = default)
	{
		var request = new PlayerBalanceRequest
		{
			ApiUrl = apiUrl.ToString(),
			BrandId = brandId,
			SecretKey = secretKey,
			PlayerCode = playerCode
		};

		return await client.PlayerBalanceAsync(
			request: request,
		   cancellationToken: cancellationToken)
		   .ConfigureAwait(false);
	}

	public async ValueTask PlayerCreateAsync(Uri apiUrl, int brandId, string secretKey, string playerCode, string currency, string language, CancellationToken cancellationToken = default)
	{
		var request = new PlayerCreateRequest
		{
			ApiUrl = apiUrl.ToString(),
			BrandId = brandId,
			SecretKey = secretKey,
			PlayerCode = playerCode,
			Currency = currency,
			Language = language
		};

		_ = await client.PlayerCreateAsync(
			request: request,
		   cancellationToken: cancellationToken)
		   .ConfigureAwait(false);
	}

	public async ValueTask PlayerLogoutAsync(Uri apiUrl, int brandId, string secretKey, string playerCode, CancellationToken cancellationToken = default)
	{
		var request = new PlayerLogoutRequest
		{
			ApiUrl = apiUrl.ToString(),
			BrandId = brandId,
			SecretKey = secretKey,
			PlayerCode = playerCode
		};

		_ = await client.PlayerLogoutAsync(
			request: request,
		   cancellationToken: cancellationToken)
		   .ConfigureAwait(false);
	}
}
