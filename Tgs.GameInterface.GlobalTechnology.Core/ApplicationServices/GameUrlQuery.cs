using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tgs.GameInterface.GlobalTechnology.Core.ApplicationServices;
public record GameUrlQuery
{
	/// <summary>
	/// The API URL.
	/// </summary>
	/// <example>https://api-stg.gmgiantgold.com/</example>
	public Uri ApiUrl { get; set; } = null!;
	/// <summary>
	/// The game launcher URL
	/// </summary>
	/// <example>https://api-stg.gmgiantgold.com/</example>
	public Uri GameLauncherUrl { get; set; } = null!;

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
	/// The language
	/// </summary>
	/// <example>CN</example>
	public string Language { get; set; } = null!;

	/// <summary>
	/// The is mobile
	/// </summary>
	/// <example>False</example>
	public bool IsMobile { get; set; }

	/// <summary>
	/// The game code
	/// </summary>
	/// <example>gpas_dostormspp_pop</example>
	public string GameCode { get; set; } = null!;
}
