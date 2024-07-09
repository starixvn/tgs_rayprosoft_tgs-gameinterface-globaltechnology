using System.Text.Json.Nodes;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Tgs.GameInterface.GlobalTechnology.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.GlobalTechnology.IntegrationTest;

public class GameInterfaceGlobalTechnologyClient_GameUrlPlaytechByLiveTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("https://api-stg.gmgiantgold.com");
		var launcherApiUrl = new Uri("https://api-stg.gmgiantgold.com");
		var brandId = 908;
		var secretKey = "SMT1ZCVRI3uqtOXNV93q";
		var playerCode = "4ucmqplxb029";
		var currency = "CNY";
		var language = "CN";
		var isMobile = false;

		var result = new GameUrlResult(
			IsPlayerExist: true,
			GameUrl: "https://api-stg.gmgiantgold.com/launcher");

		var web = new GameInterfacePragmaticPlayApplication(builder => _ = builder.ConfigureServices(services =>
		{
			var fakeService = Substitute.For<IGetService<GameUrlQuery, GameUrlResult>>();

			var query = new GameUrlQuery
			{
				ApiUrl = apiUrl,
				GameLauncherUrl = launcherApiUrl,
				BrandId = brandId,
				SecretKey = secretKey,
				PlayerCode = playerCode,
				Currency = currency,
				Language = language,
				IsMobile = isMobile,
				GameCode = "bal"
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

		var actual = await sut.GameUrlPlaytechByLiveAsync(apiUrl, launcherApiUrl, brandId, secretKey, playerCode, currency, language, isMobile);

		Assert.NotNull(actual);
		Assert.True(result.IsPlayerExist);
		Assert.NotEmpty(actual.GameUrl);
	}
}
