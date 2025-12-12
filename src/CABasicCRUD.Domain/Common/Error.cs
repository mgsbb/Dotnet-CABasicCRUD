namespace CABasicCRUD.Domain.Common;

public class Error
{
    public string Code { get; private set; }
    public string Message { get; private set; }

    public Error(string code, string message)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Error code must be provided", nameof(code));

        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Error message must be provided", nameof(message));

        Code = code;
        Message = message;
    }
}
