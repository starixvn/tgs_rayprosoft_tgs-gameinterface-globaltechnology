using System.Text.Json.Nodes;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Tgs.GameInterface.GlobalTechnology.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.GlobalTechnology.IntegrationTest;

public class GameInterfaceGlobalTechnologyClient_GameRecordTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var dataApiUrl = new Uri("https://api-stg.gmgiantgold.com");
		var brandId = 908;
		var secretKey = "SMT1ZCVRI3uqtOXNV93q";
		var startTime = DateTimeOffset.UtcNow.AddMinutes(-15);
		var endTime = DateTimeOffset.UtcNow;
		var page = 1;
		var pageSize = 5000;

		var result = new GameRecordResult(
			PlayerCode: "4ucmqplxb029",
			ExternalBetId: "live_87615669175",
			ExternalGameId: "ubal",
			ExternalCreatedOn: new DateTimeOffset(2024, 07, 09, 02, 50, 39, 000, TimeSpan.Zero),
			ExternalClosedOn: new DateTimeOffset(2024, 07, 09, 02, 50, 52, 000, TimeSpan.Zero),
			IsFreeGame: false,
			IsBonusGame: false,
			Bet: 10.0m,
			Settle: 20.0m,
			WinLoss: 10.0m,
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

		var web = new GameInterfaceGlobalTechnologyApplication(builder => _ = builder.ConfigureServices(services =>
		{
			var fakeService = Substitute.For<IQueryService<GameRecordQuery, GameRecordResult>>();

			var query = new GameRecordQuery
			{
				ApiUrl = dataApiUrl,
				BrandId = brandId,
				SecretKey = secretKey,
				StartTime = startTime,
				EndTime = endTime,
				Page = page,
				Size = pageSize
			};

			_ = fakeService.QueryAsync(
				query: Arg.Is(query),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(new[] { result }.ToAsyncEnumerable());

			_ = services.AddTransient(_ => fakeService);
		}));

		var sut = new Client.GameInterfaceGlobalTechnologyClient(new Protos.GameInterfaceGlobalTechnology.GameInterfaceGlobalTechnologyClient(
			GrpcChannel.ForAddress(
				address: web.Server.BaseAddress,
				channelOptions: new GrpcChannelOptions { HttpHandler = web.Server.CreateHandler() })));

		var actual = await sut.GameRecordAsync(dataApiUrl, brandId, secretKey, startTime, endTime, page, pageSize).ToArrayAsync();

		Assert.NotNull(actual);
		var info = actual.FirstOrDefault();
		Assert.NotNull(info);
		Assert.Equal(result.PlayerCode, info.PlayerCode);
		Assert.Equal(result.ExternalBetId, info.ExternalBetId);
		Assert.Equal(result.ExternalGameId, info.ExternalGameId);
		Assert.Equal(result.ExternalCreatedOn, info.ExternalCreatedOn.ToDateTimeOffset());
		Assert.Equal(result.ExternalClosedOn, info.ExternalClosedOn.ToDateTimeOffset());
		Assert.Equal(result.IsFreeGame, info.IsFreeGame);
		Assert.Equal(result.IsBonusGame, info.IsBonusGame);
		Assert.Equal(result.Bet, info.Bet.ToDecimal());
		Assert.Equal(result.Settle, info.Settle.ToDecimal());
		Assert.Equal(result.WinLoss, info.WinLoss.ToDecimal());
		Assert.Equal(result.JackpotContribution, info.JackpotContribution.ToDecimal());
		Assert.Equal(result.JackpotWin, info.JackpotWin.ToDecimal());
		Assert.NotEmpty(info.AdditionalData);
	}
}
