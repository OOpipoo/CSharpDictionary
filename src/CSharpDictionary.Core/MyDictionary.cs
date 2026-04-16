namespace CSharpDictionary.Core;

public class MyDictionary<Tkey, Tvalue>
{
	private const int DefaultCapacity = 16;

	private readonly LinkedList<KeyValuePair<Tkey, Tvalue>> ?[] _buckets;
	
	public MyDictionary()
	{
		_buckets = new LinkedList<KeyValuePair<Tkey, Tvalue>>[DefaultCapacity];
	}

	private int GetBucketIndex(Tkey key)
	{
		int hash = key!.GetHashCode();
		return Math.Abs(hash) % _buckets.Length;
	}

	public void Add(Tkey key, Tvalue value)
	{
		int index = GetBucketIndex(key);

		_buckets[index] ??= new LinkedList<KeyValuePair<Tkey, Tvalue>>();
		_buckets[index]!.AddLast(new KeyValuePair<Tkey, Tvalue>(key, value));
	}

	public Tvalue Get(Tkey key)
	{
		int index = GetBucketIndex(key);
		var bucket = _buckets[index];
		
		if (bucket != null)
			foreach (var pair in bucket)
				if (pair.Key!.Equals(key))
					return pair.Value;

		throw new KeyNotFoundException($"Key {key} not found");
	}
	
	
	
}
