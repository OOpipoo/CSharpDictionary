using CSharpDictionary.Core;

namespace CSharpDictionary.Tests;

public class MyConcurrentDictionaryTests
{
	[Fact]
	public void Add_And_Get_ReturnsCorrectValue()
	{
		var dict = new MyConcurrentDictionary<string, int>();
		dict.Add("key", 42);
		Assert.Equal(42, dict.Get("key"));
	}

	[Fact]
	public void Add_MultipleThreads_NoDataLoss()
	{
		var dict = new MyConcurrentDictionary<string, int>();
		var threads = new List<Thread>();

		for (int i = 0; i < 100; i++)
		{
			int copy = i;
			threads.Add(new Thread(() => dict.Add($"key{copy}", copy)));
		}

		threads.ForEach(t => t.Start());
		threads.ForEach(t => t.Join());

		Assert.Equal(100, dict.Count);
	}

	[Fact]
	public void Remove_MultipleThreads_ConsistentCount()
	{
		var dict = new MyConcurrentDictionary<string, int>();

		for (int i = 0; i < 100; i++)
			dict.Add($"key{i}", i);

		var threads = new List<Thread>();

		for (int i = 0; i < 50; i++)
		{
			int copy = i;
			threads.Add(new Thread(() => dict.Remove($"key{copy}")));
		}

		threads.ForEach(t => t.Start());
		threads.ForEach(t => t.Join());

		Assert.Equal(50, dict.Count);
	}
}