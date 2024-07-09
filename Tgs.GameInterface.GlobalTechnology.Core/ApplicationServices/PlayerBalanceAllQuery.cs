namespace Tgs.GameInterface.GlobalTechnology.Core.ApplicationServices;

public record PlayerBalanceAllQuery
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
	/// The page
	/// </summary>
	/// <example>1</example>
	public int Page { get; set; } = 1;

	/// <summary>
	/// The size
	/// </summary>
	/// <example>5000</example>
	public int Size { get; set; } = 5000;
}
