namespace NewsCatcher.Application.Exceptions;

/// <summary>
/// Thrown when the OTP email quota for the same address is exceeded (HTTP 429).
/// </summary>
public sealed class OtpEmailLimitExceededException : Exception
{
    public OtpEmailLimitExceededException()
        : base("A maximum of 3 verification emails can be sent to this address within 24 hours.")
    {
    }
}
