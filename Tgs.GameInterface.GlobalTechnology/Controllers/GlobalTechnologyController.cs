using Microsoft.AspNetCore.Mvc;
using Tgs.GameInterface.GlobalTechnology.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.GlobalTechnology.Controllers;

[Route("GT")]
[ApiController]
public class GlobalTechnologyController : ControllerBase
{
	/// <summary>
	/// Players the create asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="command">The command.</param>
	/// <returns></returns>
	[HttpPost("player/create", Name = "PlayerCreate")]
	public ValueTask PlayerCreateAsync(
		[FromServices] ICommandService<PlayerCreateCommand> service,
		[FromBody] PlayerCreateCommand command)
		=> service.ExecuteAsync(
			command: command,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Games the token asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="query">The query.</param>
	/// <returns></returns>
	[HttpPost("GameToken")]
	public ValueTask<GameTokenResult> GameTokenAsync(
		[FromServices] IGetService<GameTokenQuery, GameTokenResult> service,
		[FromBody] GameTokenQuery query)
		=> service.GetAsync(
			query: query,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Games the URL asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="query">The query.</param>
	/// <returns></returns>
	[HttpPost("gameurl")]
	public ValueTask<GameUrlResult> GameUrlAsync(
		[FromServices] IGetService<GameUrlQuery, GameUrlResult> service,
		[FromBody] GameUrlQuery query)
		=> service.GetAsync(
			query: query,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Games the record asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	[HttpPost("game/record/", Name = "GameRecord")]
	public IAsyncEnumerable<GameRecordResult> GameRecordAsync(
			[FromServices] IQueryService<GameRecordQuery, GameRecordResult> service,
			GameRecordQuery source)
		=> service.QueryAsync(
			query: source,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Games the record detail asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	[HttpPost("game/record/detail", Name = "GameRecordDetail")]
	public ValueTask<GameRecordDetailResult> GameRecordDetailAsync(
			[FromServices] IGetService<GameRecordDetailQuery, GameRecordDetailResult> service,
			GameRecordDetailQuery source)
		=> service.GetAsync(
			query: source,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Players the balance asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="query">The query.</param>
	/// <returns></returns>
	[HttpPost("player/balance", Name = "PlayerBalance")]
	public ValueTask<PlayerBalanceResult> PlayerBalanceAsync(
		[FromServices] IGetService<PlayerBalanceQuery, PlayerBalanceResult> service,
		[FromBody] PlayerBalanceQuery query)
		=> service.GetAsync(
			query: query,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Players the logout asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="command">The command.</param>
	/// <returns></returns>
	[HttpPost("player/logout", Name = "PlayerLogout")]
	public ValueTask PlayerLogoutAsync(
		[FromServices] ICommandService<PlayerLogoutCommand> service,
		PlayerLogoutCommand command)
		=> service.ExecuteAsync(
			command: command,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Players the balance all asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="query">The query.</param>
	/// <returns></returns>
	[HttpPost("player/balance/all")]
	public IAsyncEnumerable<PlayerBalanceAllResult> PlayerBalanceAllAsync(
		[FromServices] IQueryService<PlayerBalanceAllQuery, PlayerBalanceAllResult> service,
		[FromBody] PlayerBalanceAllQuery query)
		=> service.QueryAsync(
			query: query,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Funds the brand balance asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="query">The query.</param>
	/// <returns></returns>
	[HttpPost("fund/brand/balance")]
	public IAsyncEnumerable<FundBrandBalanceResult> FundBrandBalanceAsync(
		[FromServices] IQueryService<FundBrandBalanceQuery, FundBrandBalanceResult> service,
		[FromBody] FundBrandBalanceQuery query)
		=> service.QueryAsync(
			query: query,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Funds the out asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="query">The query.</param>
	/// <returns></returns>
	[HttpPost("fund/out", Name = "FundOut")]
	public ValueTask<FundResult> FundOutAsync(
		[FromServices] IGetService<FundOutQuery, FundResult> service,
		[FromBody] FundOutQuery query)
		=> service.GetAsync(
			query: query,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Funds the in asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="query">The query.</param>
	/// <returns></returns>
	[HttpPost("fund/in", Name = "FundIn")]
	public ValueTask<FundResult> FundInAsync(
		[FromServices] IGetService<FundInQuery, FundResult> service,
		[FromBody] FundInQuery query)
		=> service.GetAsync(
			query: query,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Funds the status asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="query">The query.</param>
	/// <returns></returns>
	[HttpPost("fund/Status", Name = "FundStatus")]
	public ValueTask<bool> FundStatusAsync(
		[FromServices] IGetService<FundStatusQuery, bool> service,
		[FromBody] FundStatusQuery query)
		=> service.GetAsync(
			query: query,
			cancellationToken: HttpContext.RequestAborted);
}
