using System.Text.Json.Nodes;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Tgs.GameInterface.GlobalTechnology.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.GlobalTechnology.IntegrationTest;

public class GameInterfaceGlobalTechnologyClient_FundOutTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("https://api-stg.gmgiantgold.com");
		var brandId = 908;
		var secretKey = "SMT1ZCVRI3uqtOXNV93q";
		var playerCode = "4ucmqplxb029";
		var currency = "CNY";
		var orderId = "36B92C00-23E4-4209-B71E-2F7CB752759A";
		var amount = 10.0m;

		var result = new FundResult(
			OrderId: "36B92C00-23E4-4209-B71E-2F7CB752759A",
			ExternalOrderId: "1810189791626473474",
			Balance: 10.0m,
			Resp: new JsonObject
			{
				["requestId"] = "97d0d7dc196340469ee2df169225e56f",
				["transId"] = "1810189791626473474",
				["extTransId"] = "36B92C00-23E4-4209-B71E-2F7CB752759A",
			}.ToJsonString());

		var web = new GameInterfaceGlobalTechnologyClient(builder => _ = builder.ConfigureServices(services =>
		{
			var fakeService = Substitute.For<IGetService<FundOutQuery, FundResult>>();

			var query = new FundOutQuery
			{
				ApiUrl = apiUrl,
				BrandId = brandId,
				SecretKey = secretKey,
				PlayerCode = playerCode,
				Currency = currency,
				OrderId = orderId,
				Amount = amount
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

		var actual = await sut.FundOutAsync(apiUrl, brandId, secretKey, playerCode, currency, orderId, amount);

		Assert.NotNull(actual);
		Assert.Equal(result.OrderId, actual.OrderId);
		Assert.Equal(result.ExternalOrderId, actual.ExternalOrderId);
		Assert.NotEmpty(actual.Resp);
	}
}
