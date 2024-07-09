namespace Tgs.GameInterface.GlobalTechnology.Core;

internal class DateTimeConverter
{
	private readonly TimeZoneInfo _timeZone = TimeZoneInfo.Utc;

	private readonly string _toStringformat = "yyyy-MM-dd HH:mm:ssZ";

	public string DatetimeConvertToString(DateTimeOffset time)
		=> TimeZoneInfo.ConvertTime(time.ToUniversalTime(), _timeZone).ToString(_toStringformat);

	public DateTimeOffset StringConvertToDatetime(string str)
		=> DateTime.TryParse(str, out var date) ? new(date, _timeZone.GetUtcOffset(date)) : DateTimeOffset.UnixEpoch;
}
