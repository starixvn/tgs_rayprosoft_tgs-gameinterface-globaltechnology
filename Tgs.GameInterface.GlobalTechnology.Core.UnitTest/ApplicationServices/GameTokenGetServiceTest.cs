using NSubstitute;
using Tgs.GameInterface.GlobalTechnology.Core.ApplicationServices;
using Tgs.GameInterface.GlobalTechnology.Core.Models;

namespace Tgs.GameInterface.GlobalTechnology.Core.UnitTest.ApplicationServices;

public class GameTokenGetServiceTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var query = new GameTokenQuery
		{
			ApiUrl = new Uri("https://api-stg.gmgiantgold.com"),
			BrandId = 908,
			SecretKey = "SMT1ZCVRI3uqtOXNV93q",
			PlayerCode = "4ucmqplxb029"
		};

		var fakeClient = Substitute.For<IGameApiClient>();

		var resp = new PlayerGetTokenResp(
			IsPlayerExist: true,
			Token: Guid.NewGuid().ToString());

		_ = fakeClient.PlayerGetTokenAsync(
			apiBaseUrl: Arg.Is(query.ApiUrl),
			brandId: Arg.Is(query.BrandId),
			secretKey: Arg.Is(query.SecretKey),
			playerId: Arg.Is(query.PlayerCode),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(resp);

		var sut = new GameTokenGetService(
			fakeClient);

		var actual = await sut.GetAsync(query);

		Assert.NotNull(actual);
		Assert.True(actual.IsPlayerExist);
		Assert.Equal(resp.Token, actual.Token);
	}
}
