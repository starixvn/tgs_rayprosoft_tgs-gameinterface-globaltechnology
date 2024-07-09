using System.Text.Json.Nodes;
using NSubstitute;
using Tgs.GameInterface.GlobalTechnology.Core.ApplicationServices;
using Tgs.GameInterface.GlobalTechnology.Core.Models;

namespace Tgs.GameInterface.GlobalTechnology.Core.UnitTest.ApplicationServices;
public class FundStatusGetServiceTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var query = new FundStatusQuery
		{
			ApiUrl = new Uri("https://api-stg.gmgiantgold.com"),
			BrandId = 908,
			SecretKey = "SMT1ZCVRI3uqtOXNV93q",
			OrderId = "36B92C00-23E4-4209-B71E-2F7CB752759A"
		};

		var fakeClient = Substitute.For<IGameApiClient>();

		_ = fakeClient.PaymentPlayerCheckTransAsync(
			apiBaseUrl: Arg.Is(query.ApiUrl),
			brandId: Arg.Is(query.BrandId),
			secretKey: Arg.Is(query.SecretKey),
			extTransId: Arg.Is(query.OrderId),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(true);

		var sut = new FundStatusGetService(
			fakeClient);

		var actual = await sut.GetAsync(query);

		Assert.True(actual);
	}
}
