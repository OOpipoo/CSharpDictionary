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

    [Fact]
    public void Count_AfterAdd_ReturnsCorrectCount()
    {
        var dict = new MyDictionary<string, int>();
        dict.Add("a", 1);
        dict.Add("b", 2);
        Assert.Equal(2, dict.Count);
    }

    [Fact]
    public void ContainsKey_ExistingKey_ReturnsTrue()
    {
        var dict = new MyDictionary<string, int>();
        dict.Add("hello", 99);
        Assert.True(dict.ContainsKey("hello"));
    }

    [Fact]
    public void ContainsKey_MissingKey_ReturnsFalse()
    {
        var dict = new MyDictionary<string, int>();
        Assert.False(dict.ContainsKey("nope"));
    }

    [Fact]
    public void Remove_ExistingKey_ReturnsTrueAndDecrementsCount()
    {
        var dict = new MyDictionary<string, int>();
        dict.Add("x", 10);
        bool removed = dict.Remove("x");
        Assert.True(removed);
        Assert.Equal(0, dict.Count);
        Assert.False(dict.ContainsKey("x"));
    }

    [Fact]
    public void Remove_MissingKey_ReturnsFalse()
    {
        var dict = new MyDictionary<string, int>();
        Assert.False(dict.Remove("ghost"));
    }
}
