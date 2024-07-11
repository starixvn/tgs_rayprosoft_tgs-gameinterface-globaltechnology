using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tgs.GameInterface.GlobalTechnology.Core.ApplicationServices;
public record GameRecordDetailQuery
{
	/// <summary>
	/// The API URL.
	/// </summary>
	/// <example>http://gm-stage-data.gmgiantgold.com/</example>
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
	/// The external provider code
	/// </summary>
	/// <example>pt</example>
	public string ExternalProviderCode { get; set; } = null!;
	/// <summary>
	/// The external bet identifier
	/// </summary>
	/// <example>gpas_vA0nc4ZybDj8eXLBVdKf</example>
	public string ExternalBetId { get; set; } = null!;
	/// <summary>
	/// The external end time
	/// </summary>
	/// <example>2024-07-10T17:18:05+08:00</example>
	public DateTimeOffset ExternalEndTime { get; set; }
	/// <summary>
	/// The language
	/// </summary>
	/// <example>CN</example>
	public string Language { get; set; } = null!;
}
