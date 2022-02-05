namespace LibMtpSharp.Enums;

public enum DebugLevelEnum
{
    LIBMTP_DEBUG_NONE =	0x00,
    LIBMTP_DEBUG_PTP  = 0x01,
    LIBMTP_DEBUG_PLST = 0x02,
    LIBMTP_DEBUG_USB  = 0x04,
    LIBMTP_DEBUG_DATA = 0x08,
    LIBMTP_DEBUG_ALL  = 0xFF
}