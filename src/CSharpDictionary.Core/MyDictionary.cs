namespace CSharpDictionary.Core;

public class MyDictionary<TKey , Tvalue>
{
	private const int DefaultCapacity = 16;

	private readonly LinkedList<KeyValuePair<TKey , Tvalue>> ?[] _buckets;
	
	public int Count { get; private set; }
	
	
	public MyDictionary()
	{
		_buckets = new LinkedList<KeyValuePair<TKey , Tvalue>>[DefaultCapacity];
	}

	private int GetBucketIndex(TKey  key)
	{
		int hash = key!.GetHashCode();
		return Math.Abs(hash) % _buckets.Length;
	}

	public void Add(TKey  key, Tvalue value)
	{
		int index = GetBucketIndex(key);

		_buckets[index] ??= new LinkedList<KeyValuePair<TKey , Tvalue>>();
		_buckets[index]!.AddLast(new KeyValuePair<TKey , Tvalue>(key, value));
		
		Count++;
	}

	public Tvalue Get(TKey  key)
	{
		int index = GetBucketIndex(key);
		var bucket = _buckets[index];
		
		if (bucket != null)
			foreach (var pair in bucket)
				if (pair.Key!.Equals(key))
					return pair.Value;

		throw new KeyNotFoundException($"Key {key} not found");
	}

	public bool ContainsKey(TKey  key)
	{
		int index = GetBucketIndex(key);
		var bucket = _buckets[index];
		
		if (bucket != null)
			foreach (var pair in bucket)
				if(pair.Key!.Equals(key))
					return true;
		
		return false;
	}

	public bool Remove(TKey  key)
	{
		int index = GetBucketIndex(key);
		var bucket = _buckets[index];

		if (bucket != null)
		{
			var node = bucket.First;
			while (node != null)
			{
				if (node.Value.Key!.Equals(key))
				{
					bucket.Remove(node);
					Count--;
					return true;
				}
				node = node.Next;
			}
		}

		return false;
	}
	
}
