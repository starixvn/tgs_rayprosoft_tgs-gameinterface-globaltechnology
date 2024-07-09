using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Tgs.GameInterface.GlobalTechnology.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.GlobalTechnology.IntegrationTest;

public class GameInterfaceGlobalTechnologyClient_PlayerCreate
{
	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("https://api-stg.gmgiantgold.com");
		var brandId = 908;
		var secretKey = "SMT1ZCVRI3uqtOXNV93q";
		var playerCode = "4ucmqplxb029";
		var currency = "CNY";
		var language = "CN";

		var fakeService = Substitute.For<ICommandService<PlayerCreateCommand>>();

		var web = new GameInterfacePragmaticPlayApplication(builder => _ = builder.ConfigureServices(services => _ = services.AddTransient(_ => fakeService)));

		var sut = new Client.GameInterfaceGlobalTechnologyClient(new Protos.GameInterfaceGlobalTechnology.GameInterfaceGlobalTechnologyClient(
			GrpcChannel.ForAddress(
				address: web.Server.BaseAddress,
				channelOptions: new GrpcChannelOptions { HttpHandler = web.Server.CreateHandler() })));

		await sut.PlayerCreateAsync(apiUrl, brandId, secretKey, playerCode, currency, language);

		_ = fakeService
			.Received(1)
			.ExecuteAsync(
			command: Arg.Is<PlayerCreateCommand>(content => content.ApiUrl == apiUrl &&
			content.BrandId == brandId &&
			content.SecretKey == secretKey &&
			content.PlayerCode == playerCode &&
			content.Currency == currency &&
			content.Language == language),
			cancellationToken: Arg.Any<CancellationToken>());
	}
}
