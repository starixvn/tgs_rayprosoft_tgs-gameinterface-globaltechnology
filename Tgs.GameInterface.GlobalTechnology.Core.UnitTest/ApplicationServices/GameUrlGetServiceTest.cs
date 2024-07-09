using System.Web;
using NSubstitute;
using Tgs.GameInterface.GlobalTechnology.Core.ApplicationServices;
using Tgs.GameInterface.GlobalTechnology.Core.Models;

namespace Tgs.GameInterface.GlobalTechnology.Core.UnitTest.ApplicationServices;

public class GameUrlGetServiceTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var query = new GameUrlQuery
		{
			ApiUrl = new Uri("https://api-stg.gmgiantgold.com/"),
			GameLauncherUrl = new Uri("https://api-stg.gmgiantgold.com/"),
			BrandId = 908,
			SecretKey = "SMT1ZCVRI3uqtOXNV93q",
			PlayerCode = "4ucmqplxb029",
			Currency = "CNY",
			Language = "CN",
			IsMobile = false,
			GameCode = "gpas_dostormspp_pop"
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

		var sut = new GameUrlGetService(
			fakeClient);

		var actual = await sut.GetAsync(query);

		Assert.NotNull(actual);
		Assert.True(actual.IsPlayerExist);

		var uri = new Uri(actual.GameUrl);
		var builder = new UriBuilder(uri);
		var queryString = HttpUtility.ParseQueryString(builder.Query);
		Assert.Equal("gpas_dostormspp_pop", queryString["gameCode"]);
		Assert.Equal(resp.Token, queryString["token"]);
		Assert.Equal("web", queryString["platform"]);
		Assert.Equal("CN", queryString["language"]);
		Assert.Equal("4ucmqplxb029", queryString["playerId"]);
		Assert.Equal("908", queryString["brandId"]);
		Assert.Equal("1", queryString["mode"]);
		Assert.Equal("CNY", queryString["currency"]);
	}
}
