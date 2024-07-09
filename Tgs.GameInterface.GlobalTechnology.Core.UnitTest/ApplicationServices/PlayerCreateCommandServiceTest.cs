using NSubstitute;
using Tgs.GameInterface.GlobalTechnology.Core.ApplicationServices;

namespace Tgs.GameInterface.GlobalTechnology.Core.UnitTest.ApplicationServices;

public class PlayerCreateCommandServiceTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var query = new PlayerCreateCommand
		{
			ApiUrl = new Uri("https://api-stg.gmgiantgold.com"),
			BrandId = 908,
			SecretKey = "SMT1ZCVRI3uqtOXNV93q",
			PlayerCode = "4ucmqplxb029",
			Currency = "CNY",
			Language = "CH"
		};

		var fakeClient = Substitute.For<IGameApiClient>();

		var sut = new PlayerCreateCommandService(
			fakeClient);

		await sut.ExecuteAsync(query);

		_ = fakeClient
			.Received(1)
			.PlayerCreateAsync(
			apiBaseUrl: Arg.Is(query.ApiUrl),
			brandId: Arg.Is(query.BrandId),
			secretKey: Arg.Is(query.SecretKey),
			playerId: Arg.Is(query.PlayerCode),
			currency: Arg.Is(query.Currency),
			language: Arg.Is(query.Language),
			cancellationToken: Arg.Any<CancellationToken>());
	}
}
