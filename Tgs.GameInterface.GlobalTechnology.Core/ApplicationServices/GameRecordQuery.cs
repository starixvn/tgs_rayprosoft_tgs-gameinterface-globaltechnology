using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tgs.GameInterface.GlobalTechnology.Core.ApplicationServices;
public record GameRecordQuery
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
	/// The start time
	/// </summary>
	/// <example>2024-07-08T15:00:00+08:00</example>
	public DateTimeOffset StartTime { get; set; }

	/// <summary>
	/// The end time
	/// </summary>
	/// <example>2024-07-08T15:15:00+08:00</example>
	public DateTimeOffset EndTime { get; set; }
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
