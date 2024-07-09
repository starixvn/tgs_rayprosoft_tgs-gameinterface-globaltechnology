namespace Tgs.GameInterface.GlobalTechnology.Core.ApplicationServices;

public record PlayerCreateCommand
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
	/// The language.
	/// </summary>
	/// <example>CH</example>
	public string Language { get; set; } = null!;
}
