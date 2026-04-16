using CSharpDictionary.Core;

namespace CSharpDictionary.Tests;

public class MyDictionaryTests
{
    [Fact]
    public void Add_And_Get_ReturnCorrectValue()
    {
        var dict = new MyDictionary<string, int>();
        dict.Add("box", 33);
        Assert.Equal(33, dict.Get("box"));
    }
    
    [Fact]
    public void Get_KeyNotFound_ThrowsException()
    {
        var dict = new MyDictionary<string, int>();
        Assert.Throws<KeyNotFoundException>(() => dict.Get("box"));
    }
}
