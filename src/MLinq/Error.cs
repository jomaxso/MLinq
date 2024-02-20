namespace MLinq;

public readonly record struct Error
{
    internal static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error Unknown = new("Error.Unknown", "An unknown or unspecified error has occurred.");
    public static readonly Error NullValue = new("Error.NullValue", "Value cannot be null.");

    private readonly int _codeLength;
    private readonly StringBuffer _codeBuffer;
    
    private readonly int _messageLength;
    private readonly StringBuffer _messageBuffer;
    
    public Error(string Code, string Message)
    {
        _codeLength = Code.Length;
        _codeBuffer = StringBuffer.Create(Code);
        
        _messageLength = Message.Length;
        _messageBuffer = StringBuffer.Create(Message);
    }

    public string Code => _codeBuffer.ToString(_codeLength);
    public string Message => _messageBuffer.ToString(_messageLength);
}