using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Tgs.GameInterface.GlobalTechnology.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.GlobalTechnology.IntegrationTest;

public class GameInterfaceGlobalTechnologyClient_GameRecordDetail
{
	[Fact]
	public async Task Normal_Process()
	{
		var dataApiUrl = new Uri("https://api-stg.gmgiantgold.com");
		var brandId = 908;
		var secretKey = "SMT1ZCVRI3uqtOXNV93q";
		var externalProviderCode = "pt";
		var externalBetId = "gpas_vA0nc4ZybDj8eXLBVdKf";
		var externalEndTime = DateTimeOffset.Parse("2024-07-10T17:18:05+08:00");
		var language = "CN";

		var result = new GameRecordDetailResult(
			IsSupport: true,
			DetailUrl: "https://extstg3-cashier01.ptstaging.eu/");

		var web = new GameInterfaceGlobalTechnologyApplication(builder => _ = builder.ConfigureServices(services =>
		{
			var fakeService = Substitute.For<IGetService<GameRecordDetailQuery, GameRecordDetailResult>>();

			var query = new GameRecordDetailQuery
			{
				ApiUrl = dataApiUrl,
				BrandId = brandId,
				SecretKey = secretKey,
				ExternalProviderCode = externalProviderCode,
				ExternalBetId = externalBetId,
				ExternalEndTime = externalEndTime,
				Language = language
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

		var actual = await sut.GameRecordDetailAsync(dataApiUrl, brandId, secretKey, externalProviderCode, externalBetId, externalEndTime, language);

		Assert.NotNull(actual);
		Assert.True(actual.IsSupport);
		Assert.NotEmpty(actual.DetailUrl);
	}
}
