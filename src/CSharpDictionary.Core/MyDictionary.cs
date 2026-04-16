using System.Collections;

namespace CSharpDictionary.Core;

public class MyDictionary<TKey , Tvalue> : IEnumerable<KeyValuePair<TKey, Tvalue>>
{
	private const int DefaultCapacity = 16;
	private const double LoadFactor = 0.75f; 

	private LinkedList<KeyValuePair<TKey , Tvalue>> ?[] _buckets;
	
	public int Count { get; private set; }
	
	
	public MyDictionary()
	{
		_buckets = new LinkedList<KeyValuePair<TKey , Tvalue>>[DefaultCapacity];
	}
	
	public IEnumerator<KeyValuePair<TKey, Tvalue>> GetEnumerator()
	{
		foreach (var bucket in _buckets)
		{
			if (bucket == null) continue;

			foreach (var pair in bucket)
				yield return pair;
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
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
		
		if ((double)Count / _buckets.Length > LoadFactor)
			Resize();
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

	private void Resize()
	{
		var newBuckets = new LinkedList<KeyValuePair<TKey , Tvalue>>?[_buckets.Length * 2];

		foreach (var bucket in _buckets)
		{
			if (bucket == null) continue;

			foreach (var pair in bucket)
			{
				int newIndex = Math.Abs(pair.Key!.GetHashCode()) % newBuckets.Length;
				newBuckets[newIndex] ??= new LinkedList<KeyValuePair<TKey , Tvalue>>();
				newBuckets[newIndex]!.AddLast(pair);
			}
		}
	}

	
}
