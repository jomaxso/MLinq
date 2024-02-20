namespace MLinq;

[System.Runtime.CompilerServices.InlineArray(MaxLength)]
internal struct StringBuffer
{
    private const int MaxLength = 150;
    public char Current;
        
    public string ToString(int length)
    {
        if (length > MaxLength)
            throw new Exception();
        
        Span<char> span = stackalloc char[length];
            
        for (var i = 0; i < length; i++)
            span[i] = this[i];
            
        return new string(span);
    }

    public static StringBuffer Create(ReadOnlySpan<char> values)
    {
        if (values.Length > MaxLength)
            throw new Exception();
            
        var codeBuffer = new StringBuffer();
        
        for (var i = 0; i < values.Length; i++)
            codeBuffer[i] = values[i];

        return codeBuffer;
    }
}