using NSubstitute;
using Tgs.GameInterface.GlobalTechnology.Core.ApplicationServices;

namespace Tgs.GameInterface.GlobalTechnology.Core.UnitTest.ApplicationServices;

public class PlayerLogoutCommandServiceTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var query = new PlayerLogoutCommand
		{
			ApiUrl = new Uri("https://api-stg.gmgiantgold.com"),
			BrandId = 908,
			SecretKey = "SMT1ZCVRI3uqtOXNV93q",
			PlayerCode = "4ucmqplxb029"
		};

		var fakeClient = Substitute.For<IGameApiClient>();

		var sut = new PlayerLogoutCommandService(
			fakeClient);

		await sut.ExecuteAsync(query);

		_ = fakeClient
			.Received(1)
			.PlayerkickOutAsync(
			apiBaseUrl: Arg.Is(query.ApiUrl),
			brandId: Arg.Is(query.BrandId),
			secretKey: Arg.Is(query.SecretKey),
			playerId: Arg.Is(query.PlayerCode),
			cancellationToken: Arg.Any<CancellationToken>());
	}
}
