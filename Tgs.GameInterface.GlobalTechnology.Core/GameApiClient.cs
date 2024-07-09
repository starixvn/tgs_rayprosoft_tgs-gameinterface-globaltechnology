using System.Diagnostics;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Web;
using Microsoft.Extensions.Logging;
using Tgs.GameInterface.GlobalTechnology.Core.Extensions;
using Tgs.GameInterface.GlobalTechnology.Core.Models;

namespace Tgs.GameInterface.GlobalTechnology.Core;

public class GameApiClient(
	ILogger<GameApiClient> logger,
	HttpClient httpClient,
	IParameterEncryptor encryptor)
	: IGameApiClient
{
	private readonly DateTimeConverter _dateTimeConverter = new();

	public async ValueTask PlayerCreateAsync(Uri apiBaseUrl, int brandId, string secretKey, string playerId, string currency, string language, CancellationToken cancellationToken = default)
	{
		var parameters = new JsonObject
		{
			["requestId"] = Guid.NewGuid().ToString("N"),
			["brandId"] = brandId,
			["playerId"] = playerId,
			["currency"] = currency,
			["language"] = language,
		};

		var jsonNode = await PostAsync(
			apiBaseUrl: apiBaseUrl,
			relativeUrl: "/player/create",
			secretKey: secretKey,
			parameters: parameters,
			logPrefix: nameof(PlayerCreateAsync),
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		var error = jsonNode["error"]!.GetValue<string>()!;
		if (error != "0" && !error.Equals("B_01", StringComparison.OrdinalIgnoreCase))
			throw new ErrorCodeException(error, jsonNode["message"]!.GetValue<string>()!);
	}

	public async ValueTask<PlayerGetTokenResp> PlayerGetTokenAsync(Uri apiBaseUrl, int brandId, string secretKey, string playerId, CancellationToken cancellationToken = default)
	{
		var parameters = new JsonObject
		{
			["requestId"] = Guid.NewGuid().ToString("N"),
			["brandId"] = brandId,
			["playerId"] = playerId,
		};

		var jsonNode = await PostAsync(
			apiBaseUrl: apiBaseUrl,
			relativeUrl: "/player/getToken",
			secretKey: secretKey,
			parameters: parameters,
			logPrefix: nameof(PlayerGetTokenAsync),
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		var error = jsonNode["error"]!.GetValue<string>()!;

		return error != "0"
			? !error.Equals("P_04", StringComparison.OrdinalIgnoreCase)
				? throw new ErrorCodeException(error, jsonNode["message"]!.GetValue<string>()!)
				: new PlayerGetTokenResp(
					IsPlayerExist: false,
					Token: string.Empty)
			: new PlayerGetTokenResp(
				IsPlayerExist: true,
				jsonNode["token"]!.GetValue<string>());
	}

	public async ValueTask<PlayerBalanceResp> PlayerBalanceAsync(Uri apiBaseUrl, int brandId, string secretKey, string playerId, CancellationToken cancellationToken = default)
	{
		var parameters = new JsonObject
		{
			["requestId"] = Guid.NewGuid().ToString("N"),
			["brandId"] = brandId,
			["playerId"] = playerId,
		};

		var jsonNode = await PostAsync(
			apiBaseUrl: apiBaseUrl,
			relativeUrl: "/player/balance",
			secretKey: secretKey,
			parameters: parameters,
			logPrefix: nameof(PlayerBalanceAsync),
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		var error = jsonNode["error"]!.GetValue<string>()!;
		return error == "P_04"
			? new PlayerBalanceResp(
				IsPlayerExist: false,
				PlayerId: string.Empty,
				Currency: string.Empty,
				Balance: 0m,
				BonusBalance: 0m,
				BrandId: 0)
			: error != "0"
			? throw new ErrorCodeException(error, jsonNode["message"]!.GetValue<string>()!)
			: new PlayerBalanceResp(
			IsPlayerExist: true,
			PlayerId: jsonNode["playerId"]!.GetValue<string>()!,
			Currency: jsonNode["currency"]!.GetValue<string>()!,
			Balance: jsonNode["balance"]!.GetValue<decimal>()!,
			BonusBalance: jsonNode["bonusBalance"]!.GetValue<decimal>()!,
			BrandId: jsonNode["brandId"]!.GetValue<int>()!);
		;
	}

	public async IAsyncEnumerable<PlayerBalanceAllResp> PlayerBalanceAllAsync(Uri apiBaseUrl, int brandId, string secretKey, int page = 1, int size = 5000, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var parameters = new JsonObject
		{
			["requestId"] = Guid.NewGuid().ToString("N"),
			["brandId"] = brandId,
			["page"] = page,
			["size"] = size
		};

		var jsonNode = await PostAsync(
			apiBaseUrl: apiBaseUrl,
			relativeUrl: "/player/balance/all",
			secretKey: secretKey,
			parameters: parameters,
			logPrefix: nameof(PlayerBalanceAllAsync),
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		var error = jsonNode["error"]!.GetValue<string>()!;
		if (error != "0")
			throw new ErrorCodeException(error, jsonNode["message"]!.GetValue<string>()!);

		foreach (var recordNode in jsonNode["records"]!.AsArray())
		{
			if (recordNode is JsonObject record)
				yield return new PlayerBalanceAllResp(
					PlayerId: record["playerId"]!.GetValue<string>()!,
					Currency: record["currency"]!.GetValue<string>()!,
					Balance: record["balance"]!.GetValue<decimal>()!,
					BonusBalance: record["bonusBalance"]!.GetValue<decimal>()!,
					BrandId: record["brandId"]!.GetValue<int>()!);
		}
	}

	public async IAsyncEnumerable<BrandBalanceResp> PaymentBrandBalanceAsync(Uri apiBaseUrl, int brandId, string secretKey, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var parameters = new JsonObject
		{
			["requestId"] = Guid.NewGuid().ToString("N"),
			["brandId"] = brandId,
		};

		var jsonNode = await PostAsync(
			apiBaseUrl: apiBaseUrl,
			relativeUrl: "/payment/brand/balance",
			secretKey: secretKey,
			parameters: parameters,
			logPrefix: nameof(PaymentBrandBalanceAsync),
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		var error = jsonNode["error"]!.GetValue<string>()!;
		if (error != "0")
			throw new ErrorCodeException(error, jsonNode["message"]!.GetValue<string>()!);

		foreach (var recordNode in jsonNode["data"]!.AsArray())
		{
			if (recordNode is JsonObject record)
				yield return new BrandBalanceResp(
					BrandId: record["brandId"]!.GetValue<int>()!,
					Currency: record["currency"]!.GetValue<string>()!,
					Balance: record["balance"]!.GetValue<decimal>()!,
					LastTransId: record["lastTransId"]!.GetValue<string>()!,
					UpdateAt: _dateTimeConverter.StringConvertToDatetime(record["updatedAt"]!.GetValue<string>()!));
		}
	}

	public async ValueTask<PaymentTransactionResp> PaymentPlayerDepositAsync(Uri apiBaseUrl, int brandId, string secretKey, string playerId, string currency, decimal amount, string extTransId, CancellationToken cancellationToken = default)
	{
		var requestId = Guid.NewGuid().ToString("N");
		var parameters = new JsonObject
		{
			["requestId"] = requestId,
			["brandId"] = brandId,
			["playerId"] = playerId,
			["currency"] = currency,
			["amount"] = amount,
			["extTransId"] = extTransId,
		};

		var jsonNode = await PostAsync(
			apiBaseUrl: apiBaseUrl,
			relativeUrl: "/payment/player/deposit",
			secretKey: secretKey,
			parameters: parameters,
			logPrefix: nameof(PaymentPlayerDepositAsync),
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		var error = jsonNode["error"]!.GetValue<string>()!;
		return error != "0"
			? throw new ErrorCodeException(error, jsonNode["message"]!.GetValue<string>()!)
			: jsonNode["status"]!.GetValue<string>() != "approved"
			? throw new Exception($"The status of this transaction number is not approved.({extTransId})")
			: new PaymentTransactionResp(
			TransId: jsonNode["transId"]!.GetValue<string>(),
			ExtTransId: jsonNode["extTransId"]!.GetValue<string>(),
			Balance: jsonNode["balance"]!.GetValue<decimal>(),
			Resp: jsonNode.ToJsonString());
	}

	public async ValueTask<PaymentTransactionResp> PaymentPlayerWithdrawalAsync(Uri apiBaseUrl, int brandId, string secretKey, string playerId, string currency, decimal amount, string extTransId, CancellationToken cancellationToken = default)
	{
		var requestId = Guid.NewGuid().ToString("N");
		var parameters = new JsonObject
		{
			["requestId"] = requestId,
			["brandId"] = brandId,
			["playerId"] = playerId,
			["currency"] = currency,
			["amount"] = amount,
			["extTransId"] = extTransId,
		};

		var jsonNode = await PostAsync(
			apiBaseUrl: apiBaseUrl,
			relativeUrl: "/payment/player/withdrawal",
			secretKey: secretKey,
			parameters: parameters,
			logPrefix: nameof(PaymentPlayerWithdrawalAsync),
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		var error = jsonNode["error"]!.GetValue<string>()!;
		return error != "0"
			? throw new ErrorCodeException(error, jsonNode["message"]!.GetValue<string>()!)
			: jsonNode["status"]!.GetValue<string>() != "approved"
			? throw new Exception($"The status of this transaction number is not approved.({extTransId})")
			: new PaymentTransactionResp(
			TransId: jsonNode["transId"]!.GetValue<string>(),
			ExtTransId: jsonNode["extTransId"]!.GetValue<string>(),
			Balance: jsonNode["balance"]!.GetValue<decimal>(),
			Resp: jsonNode.ToJsonString());
	}

	public async ValueTask<bool> PaymentPlayerCheckTransAsync(Uri apiBaseUrl, int brandId, string secretKey, string extTransId, CancellationToken cancellationToken = default)
	{
		var requestId = Guid.NewGuid().ToString("N");
		var parameters = new JsonObject
		{
			["requestId"] = requestId,
			["brandId"] = brandId,
			["extTransId"] = extTransId,
		};

		var jsonNode = await PostAsync(
			apiBaseUrl: apiBaseUrl,
			relativeUrl: "/payment/player/checkTrans",
			secretKey: secretKey,
			parameters: parameters,
			logPrefix: nameof(PaymentPlayerCheckTransAsync),
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		var error = jsonNode["error"]!.GetValue<string>()!;

		return !error.Equals("0", StringComparison.OrdinalIgnoreCase)
			? !error.Equals("T_03", StringComparison.OrdinalIgnoreCase)
				? throw new ErrorCodeException(error, jsonNode["message"]!.GetValue<string>()!)
				: false
			: jsonNode["status"]!.GetValue<string>().Equals("approved", StringComparison.OrdinalIgnoreCase)
			|| (!jsonNode["status"]!.GetValue<string>().Equals("declined", StringComparison.OrdinalIgnoreCase)
					? throw new Exception("此交易代號狀態(pending),請聯繫遊戲商客服")
					: false);
	}

	public async IAsyncEnumerable<HistoryGameResp> HistoryGameAsync(Uri apiBaseUrl, int brandId, string secretKey, DateTimeOffset startTime, DateTimeOffset endTime, int page = 1, int size = 5000, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var parameters = new JsonObject
		{
			["requestId"] = Guid.NewGuid().ToString("N"),
			["brandId"] = brandId,
			["startTime"] = _dateTimeConverter.DatetimeConvertToString(startTime),
			["endTime"] = _dateTimeConverter.DatetimeConvertToString(endTime),
			["showAll"] = 1,
			["page"] = page,
			["size"] = size
		};

		var jsonNode = await PostAsync(
			apiBaseUrl: apiBaseUrl,
			relativeUrl: "/history/game",
			secretKey: secretKey,
			parameters: parameters,
			logPrefix: nameof(HistoryGameAsync),
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		var error = jsonNode["error"]!.GetValue<string>()!;
		if (error != "0")
			throw new ErrorCodeException(error, jsonNode["message"]!.GetValue<string>()!);

		foreach (var recordNode in jsonNode["records"]!.AsArray())
		{
			if (recordNode is JsonObject record)
				yield return new HistoryGameResp(
					PlayerCode: recordNode["playerId"]!.GetValue<string>()!,
					ExternalRoundId: recordNode["roundId"]!.GetValue<string>()!,
					ExternalBetId: recordNode["sessionId"]!.GetValue<string>()!,
					ExternalGameId: recordNode["gameCode"]!.GetValue<string>()!,
					ExternalCreatedOn: _dateTimeConverter.StringConvertToDatetime(recordNode["startTime"]!.GetValue<string>()!),
					ExternalClosedOn: _dateTimeConverter.StringConvertToDatetime(recordNode["endTime"]!.GetValue<string>()!),
					RoundType: recordNode["roundType"]!.GetValue<string>()!,
					Bet: recordNode["bets"]!.GetValue<decimal>()!,
					Cancels: recordNode["cancels"]!.GetValue<decimal>()!,
					Settle: recordNode["wins"]!.GetValue<decimal>()!,
					JackpotContribution: recordNode["jpc"]?.GetValue<decimal>() ?? 0.0m,
					JackpotWin: recordNode["jpw"]?.GetValue<decimal>() ?? 0.0m,
					AdditionalData: recordNode.ToJsonString());
		}
	}

	public async ValueTask PlayerkickOutAsync(Uri apiBaseUrl, int brandId, string secretKey, string playerId, CancellationToken cancellationToken = default)

	{
		var parameters = new JsonObject
		{
			["requestId"] = Guid.NewGuid().ToString("N"),
			["brandId"] = brandId,
			["playerId"] = playerId
		};

		var jsonNode = await PostAsync(
			apiBaseUrl: apiBaseUrl,
			relativeUrl: "/player/kickOut",
			secretKey: secretKey,
			parameters: parameters,
			logPrefix: nameof(PlayerCreateAsync),
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		var error = jsonNode["error"]!.GetValue<string>()!;
		if (error != "0" && !error.Equals("P_04", StringComparison.OrdinalIgnoreCase))
			throw new ErrorCodeException(error, jsonNode["message"]!.GetValue<string>()!);
	}

	private async ValueTask<JsonNode> PostAsync(Uri apiBaseUrl, string relativeUrl, string secretKey, JsonObject parameters, string logPrefix, CancellationToken cancellationToken = default)
	{
		logPrefix = $"{nameof(GameApiClient).ToLower()}.{logPrefix}";

		var builder = new UriBuilder(apiBaseUrl)
		{
			Path = relativeUrl,
		};

		var hash = encryptor.Encrypt(parameters, secretKey);
		if (string.IsNullOrWhiteSpace(hash))
			throw new Exception("parameter hash is empty");
		var query = HttpUtility.ParseQueryString(builder.Query);
		query["hash"] = hash;
		builder.Query = query.ToString();

		var apiUrl = builder.Uri;
		var requestContent = JsonContent.Create(parameters);
		var request = new HttpRequestMessage(HttpMethod.Post, apiUrl)
		{
			Content = requestContent,
		};

		var sw = Stopwatch.StartNew();

		var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

		sw.Stop();

		var responseContent = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

		var apiLogEvent = new
		{
			Path = request?.RequestUri?.AbsoluteUri.NoLongerThan(512) ?? string.Empty,
			Method = request?.Method.ToString().NoLongerThan(8) ?? string.Empty,
			Prefix = logPrefix,
			ServerName = Environment.MachineName.NoLongerThan(128),
			RequestHeaders = request?.Headers + request?.Content?.Headers?.ToString(),
			RequestParameters = parameters.ToJsonString(),
			RequestContent = (await requestContent.ReadAsStringAsync(cancellationToken).ConfigureAwait(false)).NoLongerThan(4096),
			ResponseHeaders = response.Headers.ToString(),
			ResponseContent = responseContent.NoLongerThan(4096),
			ResponseCode = (short)response.StatusCode,
			ExecutionTimeMs = (int)sw.ElapsedMilliseconds,
			RecordedOn = DateTimeOffset.UtcNow
		};
		logger.LogInformation("{apiLogEvent}", JsonSerializer.Serialize(apiLogEvent));

		// API 有提及所有要求皆以 200 回應, 錯誤資訊額外攜帶在 回應內文中
		// https://globaltllc.atlassian.net/wiki/spaces/GAG1/pages/1243546539#%E5%BF%85%E9%80%89%E5%93%8D%E5%BA%94%E5%8F%82%E6%95%B0
		response.EnsureSuccessStatusCode();

		return JsonNode.Parse(responseContent)!;
	}
}
