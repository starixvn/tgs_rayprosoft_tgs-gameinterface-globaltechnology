using System.Text.Json.Nodes;
using NSubstitute;
using Tgs.GameInterface.GlobalTechnology.Core.ApplicationServices;
using Tgs.GameInterface.GlobalTechnology.Core.Models;

namespace Tgs.GameInterface.GlobalTechnology.Core.UnitTest.ApplicationServices;

public class GameRecordQueryServiceTest
{
	[Fact]
	public async Task Slots_settle_is_0()
	{
		var query = new GameRecordQuery
		{
			ApiUrl = new Uri("https://api-stg.gmgiantgold.com"),
			BrandId = 908,
			SecretKey = "SMT1ZCVRI3uqtOXNV93q",
			StartTime = new DateTimeOffset(2024, 07, 08, 15, 00, 00, TimeSpan.FromHours(+08)),
			EndTime = new DateTimeOffset(2024, 07, 08, 15, 15, 00, TimeSpan.FromHours(+08)),
			Page = 1,
			Size = 5000
		};

		var fakeClient = Substitute.For<IGameApiClient>();

		var resp = new HistoryGameResp(
			PlayerCode: "4ucmqplxb029",
			ExternalBetId: "gpas_3_CT76ykoNwUhtp7E5X3",
			ExternalGameId: "gpas_dostormspp_pop",
			ExternalCreatedOn: new DateTimeOffset(2024, 07, 08, 07, 06, 01, 000, TimeSpan.Zero),
			ExternalClosedOn: new DateTimeOffset(2024, 07, 08, 07, 06, 02, 608, TimeSpan.Zero),
			RoundType: "normal",
			Bet: 0.25m,
			Cancels: 0.0m,
			Settle: 0.0m,
			JackpotContribution: 0.0075m,
			JackpotWin: 0.0m,
			AdditionalData: JsonNode.Parse("""
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
			""")!.ToJsonString());

		_ = fakeClient.HistoryGameAsync(
			apiBaseUrl: Arg.Is(query.ApiUrl),
			brandId: Arg.Is(query.BrandId),
			secretKey: Arg.Is(query.SecretKey),
			startTime: Arg.Is(query.StartTime),
			endTime: Arg.Is(query.EndTime),
			page: Arg.Is(query.Page),
			size: Arg.Is(query.Size),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(new[] { resp }.ToAsyncEnumerable());

		var sut = new GameRecordQueryService(
			fakeClient);

		var actual = await sut.QueryAsync(query)
			.ToArrayAsync();

		Assert.NotNull(actual);
		var result = actual.FirstOrDefault();
		Assert.NotNull(result);
		Assert.Equal(resp.PlayerCode, result.PlayerCode);
		Assert.Equal(resp.ExternalBetId, result.ExternalBetId);
		Assert.Equal(resp.ExternalGameId, result.ExternalGameId);
		Assert.Equal(resp.ExternalCreatedOn, result.ExternalCreatedOn);
		Assert.Equal(resp.ExternalClosedOn, result.ExternalClosedOn);
		Assert.False(result.IsFreeGame);
		Assert.False(result.IsBonusGame);
		Assert.Equal(0.25m, result.Bet);
		Assert.Equal(0.0m, result.Settle);
		Assert.Equal(-0.25m, result.WinLoss);
		Assert.Equal(0.0075m, result.JackpotContribution);
		Assert.Equal(0.0m, result.JackpotWin);
		Assert.NotEmpty(result.AdditionalData);
	}

	[Fact]
	public async Task Slots_settle_is_not_0()
	{
		var query = new GameRecordQuery
		{
			ApiUrl = new Uri("https://api-stg.gmgiantgold.com"),
			BrandId = 908,
			SecretKey = "SMT1ZCVRI3uqtOXNV93q",
			StartTime = new DateTimeOffset(2024, 07, 08, 15, 00, 00, TimeSpan.FromHours(+08)),
			EndTime = new DateTimeOffset(2024, 07, 08, 15, 15, 00, TimeSpan.FromHours(+08)),
			Page = 1,
			Size = 5000
		};

		var fakeClient = Substitute.For<IGameApiClient>();

		var resp = new HistoryGameResp(
			PlayerCode: "4ucmqplxb029",
			ExternalBetId: "gpas_9LdEl9Rtbt_zDNC0oX8N",
			ExternalGameId: "gpas_dostormspp_pop",
			ExternalCreatedOn: new DateTimeOffset(2024, 07, 08, 07, 06, 05, 000, TimeSpan.Zero),
			ExternalClosedOn: new DateTimeOffset(2024, 07, 08, 07, 06, 06, 000, TimeSpan.Zero),
			RoundType: "normal",
			Bet: 0.25m,
			Cancels: 0.0m,
			Settle: 0.05m,
			JackpotContribution: 0.0075m,
			JackpotWin: 0.0m,
			AdditionalData: JsonNode.Parse("""
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
			""")!.ToJsonString());

		_ = fakeClient.HistoryGameAsync(
			apiBaseUrl: Arg.Is(query.ApiUrl),
			brandId: Arg.Is(query.BrandId),
			secretKey: Arg.Is(query.SecretKey),
			startTime: Arg.Is(query.StartTime),
			endTime: Arg.Is(query.EndTime),
			page: Arg.Is(query.Page),
			size: Arg.Is(query.Size),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(new[] { resp }.ToAsyncEnumerable());

		var sut = new GameRecordQueryService(
			fakeClient);

		var actual = await sut.QueryAsync(query)
			.ToArrayAsync();

		Assert.NotNull(actual);
		var result = actual.FirstOrDefault();
		Assert.NotNull(result);
		Assert.Equal(resp.PlayerCode, result.PlayerCode);
		Assert.Equal(resp.ExternalBetId, result.ExternalBetId);
		Assert.Equal(resp.ExternalGameId, result.ExternalGameId);
		Assert.Equal(resp.ExternalCreatedOn, result.ExternalCreatedOn);
		Assert.Equal(resp.ExternalClosedOn, result.ExternalClosedOn);
		Assert.False(result.IsFreeGame);
		Assert.False(result.IsBonusGame);
		Assert.Equal(0.25m, result.Bet);
		Assert.Equal(0.05m, result.Settle);
		Assert.Equal(-0.20m, result.WinLoss);
		Assert.Equal(0.0075m, result.JackpotContribution);
		Assert.Equal(0.0m, result.JackpotWin);
		Assert.NotEmpty(result.AdditionalData);
	}

	[Fact]
	public async Task Live_settle_is_20()
	{
		var query = new GameRecordQuery
		{
			ApiUrl = new Uri("https://api-stg.gmgiantgold.com"),
			BrandId = 908,
			SecretKey = "SMT1ZCVRI3uqtOXNV93q",
			StartTime = new DateTimeOffset(2024, 07, 08, 15, 00, 00, TimeSpan.FromHours(+08)),
			EndTime = new DateTimeOffset(2024, 07, 08, 15, 15, 00, TimeSpan.FromHours(+08)),
			Page = 1,
			Size = 5000
		};

		var fakeClient = Substitute.For<IGameApiClient>();

		var resp = new HistoryGameResp(
			PlayerCode: "4ucmqplxb029",
			ExternalBetId: "live_87615669175",
			ExternalGameId: "ubal",
			ExternalCreatedOn: new DateTimeOffset(2024, 07, 09, 02, 50, 39, 000, TimeSpan.Zero),
			ExternalClosedOn: new DateTimeOffset(2024, 07, 09, 02, 50, 52, 000, TimeSpan.Zero),
			RoundType: "normal",
			Bet: 10.0m,
			Cancels: 0.0m,
			Settle: 20.0m,
			JackpotContribution: 0.0m,
			JackpotWin: 0.0m,
			AdditionalData: JsonNode.Parse("""
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
			""")!.ToJsonString());

		_ = fakeClient.HistoryGameAsync(
			apiBaseUrl: Arg.Is(query.ApiUrl),
			brandId: Arg.Is(query.BrandId),
			secretKey: Arg.Is(query.SecretKey),
			startTime: Arg.Is(query.StartTime),
			endTime: Arg.Is(query.EndTime),
			page: Arg.Is(query.Page),
			size: Arg.Is(query.Size),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(new[] { resp }.ToAsyncEnumerable());

		var sut = new GameRecordQueryService(
			fakeClient);

		var actual = await sut.QueryAsync(query)
			.ToArrayAsync();

		Assert.NotNull(actual);
		var result = actual.FirstOrDefault();
		Assert.NotNull(result);
		Assert.Equal(resp.PlayerCode, result.PlayerCode);
		Assert.Equal(resp.ExternalBetId, result.ExternalBetId);
		Assert.Equal(resp.ExternalGameId, result.ExternalGameId);
		Assert.Equal(resp.ExternalCreatedOn, result.ExternalCreatedOn);
		Assert.Equal(resp.ExternalClosedOn, result.ExternalClosedOn);
		Assert.False(result.IsFreeGame);
		Assert.False(result.IsBonusGame);
		Assert.Equal(10.0m, result.Bet);
		Assert.Equal(20.0m, result.Settle);
		Assert.Equal(10.0m, result.WinLoss);
		Assert.Equal(0.0m, result.JackpotContribution);
		Assert.Equal(0.0m, result.JackpotWin);
		Assert.NotEmpty(result.AdditionalData);
	}

	[Fact]
	public async Task Live_settle_is_0()
	{
		var query = new GameRecordQuery
		{
			ApiUrl = new Uri("https://api-stg.gmgiantgold.com"),
			BrandId = 908,
			SecretKey = "SMT1ZCVRI3uqtOXNV93q",
			StartTime = new DateTimeOffset(2024, 07, 08, 15, 00, 00, TimeSpan.FromHours(+08)),
			EndTime = new DateTimeOffset(2024, 07, 08, 15, 15, 00, TimeSpan.FromHours(+08)),
			Page = 1,
			Size = 5000
		};

		var fakeClient = Substitute.For<IGameApiClient>();

		var resp = new HistoryGameResp(
			PlayerCode: "4ucmqplxb029",
			ExternalBetId: "live_87615669173",
			ExternalGameId: "ubal",
			ExternalCreatedOn: new DateTimeOffset(2024, 07, 09, 02, 49, 20, 000, TimeSpan.Zero),
			ExternalClosedOn: new DateTimeOffset(2024, 07, 09, 02, 49, 34, 808, TimeSpan.Zero),
			RoundType: "normal",
			Bet: 10.0m,
			Cancels: 0.0m,
			Settle: 0.0m,
			JackpotContribution: 0.0m,
			JackpotWin: 0.0m,
			AdditionalData: JsonNode.Parse("""
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
			""")!.ToJsonString());

		_ = fakeClient.HistoryGameAsync(
			apiBaseUrl: Arg.Is(query.ApiUrl),
			brandId: Arg.Is(query.BrandId),
			secretKey: Arg.Is(query.SecretKey),
			startTime: Arg.Is(query.StartTime),
			endTime: Arg.Is(query.EndTime),
			page: Arg.Is(query.Page),
			size: Arg.Is(query.Size),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(new[] { resp }.ToAsyncEnumerable());

		var sut = new GameRecordQueryService(
			fakeClient);

		var actual = await sut.QueryAsync(query)
			.ToArrayAsync();

		Assert.NotNull(actual);
		var result = actual.FirstOrDefault();
		Assert.NotNull(result);
		Assert.Equal(resp.PlayerCode, result.PlayerCode);
		Assert.Equal(resp.ExternalBetId, result.ExternalBetId);
		Assert.Equal(resp.ExternalGameId, result.ExternalGameId);
		Assert.Equal(resp.ExternalCreatedOn, result.ExternalCreatedOn);
		Assert.Equal(resp.ExternalClosedOn, result.ExternalClosedOn);
		Assert.False(result.IsFreeGame);
		Assert.False(result.IsBonusGame);
		Assert.Equal(10.0m, result.Bet);
		Assert.Equal(0.0m, result.Settle);
		Assert.Equal(-10.0m, result.WinLoss);
		Assert.Equal(0.0m, result.JackpotContribution);
		Assert.Equal(0.0m, result.JackpotWin);
		Assert.NotEmpty(result.AdditionalData);
	}

	[Fact]
	public async Task Live_result_Draw_settle_is_10()
	{
		var query = new GameRecordQuery
		{
			ApiUrl = new Uri("https://api-stg.gmgiantgold.com"),
			BrandId = 908,
			SecretKey = "SMT1ZCVRI3uqtOXNV93q",
			StartTime = new DateTimeOffset(2024, 07, 08, 15, 00, 00, TimeSpan.FromHours(+08)),
			EndTime = new DateTimeOffset(2024, 07, 08, 15, 15, 00, TimeSpan.FromHours(+08)),
			Page = 1,
			Size = 5000
		};

		var fakeClient = Substitute.For<IGameApiClient>();

		var resp = new HistoryGameResp(
			PlayerCode: "4ucmqplxb029",
			ExternalBetId: "live_87615669174",
			ExternalGameId: "ubal",
			ExternalCreatedOn: new DateTimeOffset(2024, 07, 09, 02, 49, 59, 000, TimeSpan.Zero),
			ExternalClosedOn: new DateTimeOffset(2024, 07, 09, 02, 50, 14, 000, TimeSpan.Zero),
			RoundType: "normal",
			Bet: 10.0m,
			Cancels: 0.0m,
			Settle: 10.0m,
			JackpotContribution: 0.0m,
			JackpotWin: 0.0m,
			AdditionalData: JsonNode.Parse("""
			{
				"roundId": "live_87615669174",
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
				"wins": 10,
				"cancels": 0,
				"transIn": 0,
				"transOut": 0,
				"balanceEnd": 89.55,
				"bonusBalanceEnd": 0,
				"rake": 0,
				"startTime": "2024-07-09 02:49:59.000",
				"endTime": "2024-07-09 02:50:14.000",
				"jpc": 0,
				"jpw": 0,
				"additionalData": {
					"type": "json",
					"value": "{\"launchAlias\":\"7bal_loungebaccarat\",\"tableId\":\"1609\"}",
					"null": true
				},
				"detailUrl": "https://extstg3-cashier01.ptstaging.eu/getgamehistoryurl.php?casino=ptstaging3.33&username=1CLUB__PEG_4ucmqplxb029&ngscode=21484358&remotesessionenddate=2024-07-09+02%3A50%3A14&token=c1ce075b1935bf95c8deb690aedba570a874261ea8ee89ae7cb6a9841fc2477b&language=EN&showFullHistoryPerBet=false",
				"gameType": "live"
			}
			""")!.ToJsonString());

		_ = fakeClient.HistoryGameAsync(
			apiBaseUrl: Arg.Is(query.ApiUrl),
			brandId: Arg.Is(query.BrandId),
			secretKey: Arg.Is(query.SecretKey),
			startTime: Arg.Is(query.StartTime),
			endTime: Arg.Is(query.EndTime),
			page: Arg.Is(query.Page),
			size: Arg.Is(query.Size),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(new[] { resp }.ToAsyncEnumerable());

		var sut = new GameRecordQueryService(
			fakeClient);

		var actual = await sut.QueryAsync(query)
			.ToArrayAsync();

		Assert.NotNull(actual);
		var result = actual.FirstOrDefault();
		Assert.NotNull(result);
		Assert.Equal(resp.PlayerCode, result.PlayerCode);
		Assert.Equal(resp.ExternalBetId, result.ExternalBetId);
		Assert.Equal(resp.ExternalGameId, result.ExternalGameId);
		Assert.Equal(resp.ExternalCreatedOn, result.ExternalCreatedOn);
		Assert.Equal(resp.ExternalClosedOn, result.ExternalClosedOn);
		Assert.False(result.IsFreeGame);
		Assert.False(result.IsBonusGame);
		Assert.Equal(10.0m, result.Bet);
		Assert.Equal(10.0m, result.Settle);
		Assert.Equal(0.0m, result.WinLoss);
		Assert.Equal(0.0m, result.JackpotContribution);
		Assert.Equal(0.0m, result.JackpotWin);
		Assert.NotEmpty(result.AdditionalData);
	}
}
