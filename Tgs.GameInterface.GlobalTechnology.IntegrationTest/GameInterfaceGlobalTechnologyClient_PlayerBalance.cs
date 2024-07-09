using System.Text.Json.Nodes;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Tgs.GameInterface.GlobalTechnology.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.GlobalTechnology.IntegrationTest;

public class GameInterfaceGlobalTechnologyClient_PlayerBalance
{
	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("https://api-stg.gmgiantgold.com");
		var brandId = 908;
		var secretKey = "SMT1ZCVRI3uqtOXNV93q";
		var playerCode = "4ucmqplxb029";

		var result = new PlayerBalanceResult(
			IsPlayerExist: true,
			PlayerCode: "4ucmqplxb029",
			Currency: "CNY",
			Balance: 10.0m,
			BonusBalance: 0.0m,
			BrandId: 908);

		var web = new GameInterfacePragmaticPlayApplication(builder => _ = builder.ConfigureServices(services =>
		{
			var fakeService = Substitute.For<IGetService<PlayerBalanceQuery, PlayerBalanceResult>>();

			var query = new PlayerBalanceQuery
			{
				ApiUrl = apiUrl,
				BrandId = brandId,
				SecretKey = secretKey,
				PlayerCode = playerCode
			};

			_ = fakeService.GetAsync(
				query: Arg.Is(query),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(result);

			_ = services.AddTransient(_ => fakeService);
		}));

		var sut = new Client.GameInterfaceGlobalTechnologyClient(new Protos.GameInterfaceGlobalTechnology.GameInterfaceGlobalTechnologyClient(
			GrpcChannel.ForAddress(
				address: web.Server.BaseAddress,
				channelOptions: new GrpcChannelOptions { HttpHandler = web.Server.CreateHandler() })));

		var actual = await sut.PlayerBalanceAsync(apiUrl, brandId, secretKey, playerCode);

		Assert.NotNull(actual);
		Assert.True(result.IsPlayerExist);
		Assert.Equal(result.Balance, actual.Balance.ToDecimal());
	}
}
