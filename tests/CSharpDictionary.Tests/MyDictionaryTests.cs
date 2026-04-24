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
    
    [Fact]
    public void Add_ManyItems_ResizesAndPreservesAllValues()
    {
        var dict = new MyDictionary<string, int>();

        for (int i = 0; i < 20; i++)
            dict.Add($"key{i}", i);

        Assert.Equal(20, dict.Count);

        for (int i = 0; i < 20; i++)
            Assert.Equal(i, dict.Get($"key{i}"));
    }
    
    [Fact]
    public void Foreach_IteratesAllPairs()
    {
        var dict = new MyDictionary<string, int>();
        dict.Add("a", 1);
        dict.Add("b", 2);
        dict.Add("c", 3);

        var result = new System.Collections.Generic.Dictionary<string, int>();
        foreach (var pair in dict)
            result[pair.Key] = pair.Value;

        Assert.Equal(3, result.Count);
        Assert.Equal(1, result["a"]);
        Assert.Equal(2, result["b"]);
        Assert.Equal(3, result["c"]);
    }

    [Fact]
    public void Indexer_Get_ReturnsCorrectValue()
    {
        var dict = new MyDictionary<string, int>();
        dict.Add("key", 42);
        Assert.Equal(42, dict["key"]);
    }

    [Fact]
    public void Indexer_Set_AddsNewPair()
    {
        var dict = new MyDictionary<string, int>();
        dict["key"] = 42;
        Assert.Equal(42, dict.Get("key"));
        Assert.Equal(1, dict.Count);
    }

    [Fact]
    public void Indexer_Set_UpdateExisingValue()
    {
        var dict = new MyDictionary<string, int>();
        dict["key"] = 42;
        dict["key"] = 99;
        Assert.Equal(99, dict["key"]);
        Assert.Equal(1, dict.Count);
    }
    
    [Fact]
    public void Add_DuplicateKey_ThrowsArgumentException()
    {
        var dict = new MyDictionary<string, int>();
        dict.Add("key", 1);
        Assert.Throws<ArgumentException>(() => dict.Add("key", 2));
    }

    [Fact]
    public void Add_DuplicateKey_DoesNotIncrementCount()
    {
        var dict = new MyDictionary<string, int>();
        dict.Add("key", 1);
        try { dict.Add("key", 2); } catch { }
        Assert.Equal(1, dict.Count);
    }
    
    [Fact]
    public void TryGetValue_ExistingKey_ReturnsTrueAndValue()
    {
        var dict = new MyDictionary<string, int>();
        dict.Add("key", 42);
        bool result = dict.TryGetValue("key", out int value);
        Assert.True(result);
        Assert.Equal(42, value);
    }

    [Fact]
    public void TryGetValue_MissingKey_ReturnsFalse()
    {
        var dict = new MyDictionary<string, int>();
        bool result = dict.TryGetValue("missing", out int value);
        Assert.False(result);
        Assert.Equal(0, value);
    }
}
