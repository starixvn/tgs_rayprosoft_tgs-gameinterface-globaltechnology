using System.Net;

using System.Net.Http.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.GlobalTechnology.Core.UnitTest;

public class GameApiClient_HistoryGameTest
{
	[Fact]
	public async Task Slots_settle_is_0()
	{
		var apiUrl = new Uri("http://gm-stage-data.gmgiantgold.com/");
		var brandId = 908;
		var secretKey = "SMT1ZCVRI3uqtOXNV93q";
		var startTime = new DateTimeOffset(2024, 07, 08, 15, 00, 00, TimeSpan.FromHours(+08));
		var endTime = new DateTimeOffset(2024, 07, 08, 15, 15, 00, TimeSpan.FromHours(+08));
		var page = 1;
		var size = 5000;
		var fakerHash = Guid.NewGuid().ToString("N");
		var handler = new MockHttpMessageHandler();
		handler.Expect(HttpMethod.Post, new Uri(apiUrl, "/history/game").ToString())
			.WithExactQueryString(new Dictionary<string, string>
			{
				["hash"] = fakerHash,
			})
			.WithJsonContent<JsonObject>(content =>
				content["brandId"]!.GetValue<int>() == brandId &&
				content["startTime"]!.GetValue<string>() == "2024-07-08 07:00:00Z" &&
				content["endTime"]!.GetValue<string>() == "2024-07-08 07:15:00Z" &&
				content["showAll"]!.GetValue<int>() == 1 &&
				content["page"]!.GetValue<int>() == 1 &&
				content["size"]!.GetValue<int>() == 5000)
			.Respond(HttpStatusCode.OK, JsonContent.Create(JsonNode.Parse("""
			{
				"requestId": "e345736762f5465d91d39a19aebf66d8",
				"error": "0",
				"message": "SUCCESS",
				"total": 1,
				"current": 1,
				"size": 1,
				"pages": 1,
				"records": [
					{
						"roundId": "gpas_3_CT76ykoNwUhtp7E5X3",
						"playerId": "4ucmqplxb029",
						"brandId": 908,
						"providerCode": "pt",
						"sessionId": "4dae8cebcfaf4e0191e437e1c295deca",
						"roundType": "normal",
						"roundStatus": "end",
						"gameCode": "gpas_dostormspp_pop",
						"currency": "CNY",
						"platform": "mobile",
						"bets": 0.25,
						"wins": 0,
						"cancels": 0,
						"transIn": 0,
						"transOut": 0,
						"balanceEnd": 99.75,
						"bonusBalanceEnd": 0,
						"rake": 0,
						"startTime": "2024-07-08 07:06:01.000",
						"endTime": "2024-07-08 07:06:02.608",
						"jpc": 0.0075,
						"jpw": 0,
						"jpDetails": {
							"type": "json",
							"value": "{\"type\":\"json\",\"value\":\"[{\\\\\"id\\\\\":\\\\\"ptjp_114230_114240_114250_306\\\\\",\\\\\"contribution\\\\\":0.007475}]\",\"null\":true}",
							"null": true
						},
						"detailUrl": "https://extstg3-cashier01.ptstaging.eu/getgamehistoryurl.php?casino=ptstaging3.33&username=1CLUB__PEG_4ucmqplxb029&ngscode=21467861&remotesessionenddate=2024-07-08+07%3A06%3A01&token=38bd8cb9ccad811308e96a8b35f1eb5908a62e44da2d269c0d392d89a22e493f&language=EN&showFullHistoryPerBet=false",
						"gameType": "slots"
					}
				]
			}
			""")));
		var httpClient = handler.ToHttpClient();
		var fakerEncryptor = Substitute.For<IParameterEncryptor>();
		fakerEncryptor.Encrypt(
			values: Arg.Any<JsonObject>(),
			secretKey: Arg.Is(secretKey))
			.Returns(fakerHash);
		var fakerLogger = NullLogger<GameApiClient>.Instance;
		var apiClient = new GameApiClient(fakerLogger, httpClient, fakerEncryptor);

		var actual = await apiClient.HistoryGameAsync(
			apiBaseUrl: apiUrl,
			brandId: brandId,
			secretKey: secretKey,
			startTime: startTime,
			endTime: endTime,
			page: page,
			size: size,
			cancellationToken: default)
			.ToArrayAsync();

		Assert.NotNull(actual);
		var historyGame = actual.FirstOrDefault();
		Assert.NotNull(historyGame);
		Assert.Equal("4ucmqplxb029", historyGame.PlayerCode);
		Assert.Equal("gpas_3_CT76ykoNwUhtp7E5X3", historyGame.ExternalRoundId);
		Assert.Equal("4dae8cebcfaf4e0191e437e1c295deca", historyGame.ExternalBetId);
		Assert.Equal("gpas_dostormspp_pop", historyGame.ExternalGameId);
		Assert.Equal(new DateTimeOffset(2024, 07, 08, 07, 06, 01, 000, TimeSpan.Zero), historyGame.ExternalCreatedOn);
		Assert.Equal(new DateTimeOffset(2024, 07, 08, 07, 06, 02, 608, TimeSpan.Zero), historyGame.ExternalClosedOn);
		Assert.Equal("normal", historyGame.RoundType);
		Assert.Equal(0.25m, historyGame.Bet);
		Assert.Equal(0.0m, historyGame.Cancels);
		Assert.Equal(0.0m, historyGame.Settle);
		Assert.Equal(0.0075m, historyGame.JackpotContribution);
		Assert.Equal(0.0m, historyGame.JackpotWin);
		Assert.NotEmpty(historyGame.AdditionalData);
	}

	[Fact]
	public async Task Slots_settle_is_not_0()
	{
		var apiUrl = new Uri("http://gm-stage-data.gmgiantgold.com/");
		var brandId = 908;
		var secretKey = "SMT1ZCVRI3uqtOXNV93q";
		var startTime = new DateTimeOffset(2024, 07, 08, 15, 00, 00, TimeSpan.FromHours(+08));
		var endTime = new DateTimeOffset(2024, 07, 08, 15, 15, 00, TimeSpan.FromHours(+08));
		var page = 1;
		var size = 5000;
		var fakerHash = Guid.NewGuid().ToString("N");
		var handler = new MockHttpMessageHandler();
		handler.Expect(HttpMethod.Post, new Uri(apiUrl, "/history/game").ToString())
			.WithExactQueryString(new Dictionary<string, string>
			{
				["hash"] = fakerHash,
			})
			.WithJsonContent<JsonObject>(content =>
				content["brandId"]!.GetValue<int>() == brandId &&
				content["startTime"]!.GetValue<string>() == "2024-07-08 07:00:00Z" &&
				content["endTime"]!.GetValue<string>() == "2024-07-08 07:15:00Z" &&
				content["showAll"]!.GetValue<int>() == 1 &&
				content["page"]!.GetValue<int>() == 1 &&
				content["size"]!.GetValue<int>() == 5000)
			.Respond(HttpStatusCode.OK, JsonContent.Create(JsonNode.Parse("""
			{
				"requestId": "38889d505a0a43c7b025647c90b79546",
				"error": "0",
				"message": "SUCCESS",
				"total": 1,
				"current": 1,
				"size": 1,
				"pages": 1,
				"records": [
					{
						"roundId": "gpas_9LdEl9Rtbt_zDNC0oX8N",
						"playerId": "4ucmqplxb029",
						"brandId": 908,
						"providerCode": "pt",
						"sessionId": "4dae8cebcfaf4e0191e437e1c295deca",
						"roundType": "normal",
						"roundStatus": "end",
						"gameCode": "gpas_dostormspp_pop",
						"currency": "CNY",
						"platform": "mobile",
						"bets": 0.25,
						"wins": 0.05,
						"cancels": 0,
						"transIn": 0,
						"transOut": 0,
						"balanceEnd": 99.55,
						"bonusBalanceEnd": 0,
						"rake": 0,
						"startTime": "2024-07-08 07:06:05.000",
						"endTime": "2024-07-08 07:06:06.000",
						"jpc": 0.0075,
						"jpw": 0,
						"jpDetails": {
							"type": "json",
							"value": "{\"type\":\"json\",\"value\":\"[{\\\\\"id\\\\\":\\\\\"ptjp_114230_114240_114250_306\\\\\",\\\\\"contribution\\\\\":0.007475}]\",\"null\":true}",
							"null": true
						},
						"detailUrl": "https://extstg3-cashier01.ptstaging.eu/getgamehistoryurl.php?casino=ptstaging3.33&username=1CLUB__PEG_4ucmqplxb029&ngscode=21467862&remotesessionenddate=2024-07-08+07%3A06%3A05&token=8c159f04c940ad08b6147fa3166e5c4dce6f1994ed3b7043a8b75e91ceaeba27&language=EN&showFullHistoryPerBet=false",
						"gameType": "slots"
					}
				]
			}
			""")));
		var httpClient = handler.ToHttpClient();
		var fakerEncryptor = Substitute.For<IParameterEncryptor>();
		fakerEncryptor.Encrypt(
			values: Arg.Any<JsonObject>(),
			secretKey: Arg.Is(secretKey))
			.Returns(fakerHash);
		var fakerLogger = NullLogger<GameApiClient>.Instance;
		var apiClient = new GameApiClient(fakerLogger, httpClient, fakerEncryptor);

		var actual = await apiClient.HistoryGameAsync(
			apiBaseUrl: apiUrl,
			brandId: brandId,
			secretKey: secretKey,
			startTime: startTime,
			endTime: endTime,
			page: page,
			size: size,
			cancellationToken: default)
			.ToArrayAsync();

		Assert.NotNull(actual);
		var historyGame = actual.FirstOrDefault();
		Assert.NotNull(historyGame);
		Assert.Equal("4ucmqplxb029", historyGame.PlayerCode);
		Assert.Equal("gpas_9LdEl9Rtbt_zDNC0oX8N", historyGame.ExternalRoundId);
		Assert.Equal("4dae8cebcfaf4e0191e437e1c295deca", historyGame.ExternalBetId);
		Assert.Equal("gpas_dostormspp_pop", historyGame.ExternalGameId);
		Assert.Equal(new DateTimeOffset(2024, 07, 08, 07, 06, 05, 000, TimeSpan.Zero), historyGame.ExternalCreatedOn);
		Assert.Equal(new DateTimeOffset(2024, 07, 08, 07, 06, 06, 000, TimeSpan.Zero), historyGame.ExternalClosedOn);
		Assert.Equal("normal", historyGame.RoundType);
		Assert.Equal(0.25m, historyGame.Bet);
		Assert.Equal(0.0m, historyGame.Cancels);
		Assert.Equal(0.05m, historyGame.Settle);
		Assert.Equal(0.0075m, historyGame.JackpotContribution);
		Assert.Equal(0.0m, historyGame.JackpotWin);
		Assert.NotEmpty(historyGame.AdditionalData);
	}

	[Fact]
	public async Task Live_settle_is_20()
	{
		var apiUrl = new Uri("http://gm-stage-data.gmgiantgold.com/");
		var brandId = 908;
		var secretKey = "SMT1ZCVRI3uqtOXNV93q";
		var startTime = new DateTimeOffset(2024, 07, 09, 10, 45, 00, TimeSpan.FromHours(+08));
		var endTime = new DateTimeOffset(2024, 07, 09, 11, 00, 00, TimeSpan.FromHours(+08));
		var page = 1;
		var size = 5000;
		var fakerHash = Guid.NewGuid().ToString("N");
		var handler = new MockHttpMessageHandler();
		handler.Expect(HttpMethod.Post, new Uri(apiUrl, "/history/game").ToString())
			.WithExactQueryString(new Dictionary<string, string>
			{
				["hash"] = fakerHash,
			})
			.WithJsonContent<JsonObject>(content =>
				content["brandId"]!.GetValue<int>() == brandId &&
				content["startTime"]!.GetValue<string>() == "2024-07-09 02:45:00Z" &&
				content["endTime"]!.GetValue<string>() == "2024-07-09 03:00:00Z" &&
				content["showAll"]!.GetValue<int>() == 1 &&
				content["page"]!.GetValue<int>() == 1 &&
				content["size"]!.GetValue<int>() == 5000)
			.Respond(HttpStatusCode.OK, JsonContent.Create(JsonNode.Parse("""
			{
				"requestId": "e345736762f5465d91d39a19aebf66d8",
				"error": "0",
				"message": "SUCCESS",
				"total": 1,
				"current": 1,
				"size": 1,
				"pages": 1,
				"records": [
					{
						"roundId": "live_87615669175",
						"playerId": "4ucmqplxb029",
						"brandId": 908,
						"providerCode": "pt",
						"sessionId": "25e1afd4d45c46ffb8dbbbd75fc5da7b",
						"roundType": "normal",
						"roundStatus": "end",
						"gameCode": "ubal",
						"currency": "CNY",
						"platform": "mobile",
						"bets": 10,
						"wins": 20,
						"cancels": 0,
						"transIn": 0,
						"transOut": 0,
						"balanceEnd": 99.55,
						"bonusBalanceEnd": 0,
						"rake": 0,
						"startTime": "2024-07-09 02:50:39.000",
						"endTime": "2024-07-09 02:50:52.000",
						"jpc": 0,
						"jpw": 0,
						"additionalData": {
							"type": "json",
							"value": "{\"launchAlias\":\"7bal_loungebaccarat\",\"tableId\":\"1609\"}",
							"null": true
						},
						"detailUrl": "https://extstg3-cashier01.ptstaging.eu/getgamehistoryurl.php?casino=ptstaging3.33&username=1CLUB__PEG_4ucmqplxb029&ngscode=21484360&remotesessionenddate=2024-07-09+02%3A50%3A52&token=7ae5b3bc0129689f29ce281dfb6608865e0c6c9a4e25155f1f29946d67448ab4&language=EN&showFullHistoryPerBet=false",
						"gameType": "live"
					}
				]
			}
			""")));
		var httpClient = handler.ToHttpClient();
		var fakerEncryptor = Substitute.For<IParameterEncryptor>();
		fakerEncryptor.Encrypt(
			values: Arg.Any<JsonObject>(),
			secretKey: Arg.Is(secretKey))
			.Returns(fakerHash);
		var fakerLogger = NullLogger<GameApiClient>.Instance;
		var apiClient = new GameApiClient(fakerLogger, httpClient, fakerEncryptor);

		var actual = await apiClient.HistoryGameAsync(
			apiBaseUrl: apiUrl,
			brandId: brandId,
			secretKey: secretKey,
			startTime: startTime,
			endTime: endTime,
			page: page,
			size: size,
			cancellationToken: default)
			.ToArrayAsync();

		Assert.NotNull(actual);
		var historyGame = actual.FirstOrDefault();
		Assert.NotNull(historyGame);
		Assert.Equal("4ucmqplxb029", historyGame.PlayerCode);
		Assert.Equal("live_87615669175", historyGame.ExternalRoundId);
		Assert.Equal("25e1afd4d45c46ffb8dbbbd75fc5da7b", historyGame.ExternalBetId);
		Assert.Equal("ubal", historyGame.ExternalGameId);
		Assert.Equal(new DateTimeOffset(2024, 07, 09, 02, 50, 39, 000, TimeSpan.Zero), historyGame.ExternalCreatedOn);
		Assert.Equal(new DateTimeOffset(2024, 07, 09, 02, 50, 52, 000, TimeSpan.Zero), historyGame.ExternalClosedOn);
		Assert.Equal("normal", historyGame.RoundType);
		Assert.Equal(10.0m, historyGame.Bet);
		Assert.Equal(0.0m, historyGame.Cancels);
		Assert.Equal(20.0m, historyGame.Settle);
		Assert.Equal(0.0m, historyGame.JackpotContribution);
		Assert.Equal(0.0m, historyGame.JackpotWin);
		Assert.NotEmpty(historyGame.AdditionalData);
	}

	[Fact]
	public async Task Live_settle_is_0()
	{
		var apiUrl = new Uri("http://gm-stage-data.gmgiantgold.com/");
		var brandId = 908;
		var secretKey = "SMT1ZCVRI3uqtOXNV93q";
		var startTime = new DateTimeOffset(2024, 07, 09, 10, 45, 00, TimeSpan.FromHours(+08));
		var endTime = new DateTimeOffset(2024, 07, 09, 11, 00, 00, TimeSpan.FromHours(+08));
		var page = 1;
		var size = 5000;
		var fakerHash = Guid.NewGuid().ToString("N");
		var handler = new MockHttpMessageHandler();
		handler.Expect(HttpMethod.Post, new Uri(apiUrl, "/history/game").ToString())
			.WithExactQueryString(new Dictionary<string, string>
			{
				["hash"] = fakerHash,
			})
			.WithJsonContent<JsonObject>(content =>
				content["brandId"]!.GetValue<int>() == brandId &&
				content["startTime"]!.GetValue<string>() == "2024-07-09 02:45:00Z" &&
				content["endTime"]!.GetValue<string>() == "2024-07-09 03:00:00Z" &&
				content["showAll"]!.GetValue<int>() == 1 &&
				content["page"]!.GetValue<int>() == 1 &&
				content["size"]!.GetValue<int>() == 5000)
			.Respond(HttpStatusCode.OK, JsonContent.Create(JsonNode.Parse("""
			{
				"requestId": "e345736762f5465d91d39a19aebf66d8",
				"error": "0",
				"message": "SUCCESS",
				"total": 1,
				"current": 1,
				"size": 1,
				"pages": 1,
				"records": [
					{
						"roundId": "live_87615669173",
						"playerId": "4ucmqplxb029",
						"brandId": 908,
						"providerCode": "pt",
						"sessionId": "25e1afd4d45c46ffb8dbbbd75fc5da7b",
						"roundType": "normal",
						"roundStatus": "end",
						"gameCode": "ubal",
						"currency": "CNY",
						"platform": "mobile",
						"bets": 10,
						"wins": 0,
						"cancels": 0,
						"transIn": 0,
						"transOut": 0,
						"balanceEnd": 89.55,
						"bonusBalanceEnd": 0,
						"rake": 0,
						"startTime": "2024-07-09 02:49:20.000",
						"endTime": "2024-07-09 02:49:34.808",
						"jpc": 0,
						"jpw": 0,
						"additionalData": {
							"type": "json",
							"value": "{\"launchAlias\":\"7bal_loungebaccarat\",\"tableId\":\"1609\"}",
							"null": true
						},
						"detailUrl": "https://extstg3-cashier01.ptstaging.eu/getgamehistoryurl.php?casino=ptstaging3.33&username=1CLUB__PEG_4ucmqplxb029&ngscode=21484357&remotesessionenddate=2024-07-09+02%3A49%3A33&token=64fa3b7e23dd6167f687b9e34e8744c6da9abba6b7cc269b94bc35312293ecb9&language=EN&showFullHistoryPerBet=false",
						"gameType": "live"
					}
				]
			}
			""")));
		var httpClient = handler.ToHttpClient();
		var fakerEncryptor = Substitute.For<IParameterEncryptor>();
		fakerEncryptor.Encrypt(
			values: Arg.Any<JsonObject>(),
			secretKey: Arg.Is(secretKey))
			.Returns(fakerHash);
		var fakerLogger = NullLogger<GameApiClient>.Instance;
		var apiClient = new GameApiClient(fakerLogger, httpClient, fakerEncryptor);

		var actual = await apiClient.HistoryGameAsync(
			apiBaseUrl: apiUrl,
			brandId: brandId,
			secretKey: secretKey,
			startTime: startTime,
			endTime: endTime,
			page: page,
			size: size,
			cancellationToken: default)
			.ToArrayAsync();

		Assert.NotNull(actual);
		var historyGame = actual.FirstOrDefault();
		Assert.NotNull(historyGame);
		Assert.Equal("4ucmqplxb029", historyGame.PlayerCode);
		Assert.Equal("live_87615669173", historyGame.ExternalRoundId);
		Assert.Equal("25e1afd4d45c46ffb8dbbbd75fc5da7b", historyGame.ExternalBetId);
		Assert.Equal("ubal", historyGame.ExternalGameId);
		Assert.Equal(new DateTimeOffset(2024, 07, 09, 02, 49, 20, 000, TimeSpan.Zero), historyGame.ExternalCreatedOn);
		Assert.Equal(new DateTimeOffset(2024, 07, 09, 02, 49, 34, 808, TimeSpan.Zero), historyGame.ExternalClosedOn);
		Assert.Equal("normal", historyGame.RoundType);
		Assert.Equal(10.0m, historyGame.Bet);
		Assert.Equal(0.0m, historyGame.Cancels);
		Assert.Equal(0.0m, historyGame.Settle);
		Assert.Equal(0.0m, historyGame.JackpotContribution);
		Assert.Equal(0.0m, historyGame.JackpotWin);
		Assert.NotEmpty(historyGame.AdditionalData);
	}
}
