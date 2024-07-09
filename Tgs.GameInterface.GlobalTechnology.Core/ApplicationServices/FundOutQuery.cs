namespace Tgs.GameInterface.GlobalTechnology.Core.ApplicationServices;

public record FundOutQuery
{
	/// <summary>
	/// The API URL.
	/// </summary>
	/// <example>https://api-stg.gmgiantgold.com</example>
	public Uri ApiUrl { get; set; } = null!;

	/// <summary>
	/// The brand identifier.
	/// </summary>
	/// <example>908</example>
	public int BrandId { get; set; }

	/// <summary>
	///  The secret key.
	/// </summary>
	/// <example>SMT1ZCVRI3uqtOXNV93q</example>
	public string SecretKey { get; set; } = null!;

	/// <summary>
	/// The player code.
	/// </summary>
	/// <example>4ucmqplxb029</example>
	public string PlayerCode { get; set; } = null!;

	/// <summary>
	/// The currency.
	/// </summary>
	/// <example>CNY</example>
	public string Currency { get; set; } = null!;

	/// <summary>
	/// The amount.
	/// </summary>
	/// <example>10.0</example>
	public decimal Amount { get; set; }
	/// <summary>
	/// The order identifier.
	/// </summary>
	public string OrderId { get; set; } = null!;
}
