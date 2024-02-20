global using Xunit;
using MLinq;

class MyClass
{
    void MyMethod()
    {
        Result<int> result = Result<int>.Success(1);
        Assert.True(true);
    }
}