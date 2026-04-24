using System.Collections;

namespace CSharpDictionary.Core;

public class MyDictionary<TKey , TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
{
	private const int DefaultCapacity = 16;
	private const double LoadFactor = 0.75f; 

	private LinkedList<KeyValuePair<TKey , TValue>> ?[] _buckets;
	
	public int Count { get; private set; }
	
	
	public MyDictionary()
	{
		_buckets = new LinkedList<KeyValuePair<TKey , TValue>>[DefaultCapacity];
	}
	
	public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
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

	public void Add(TKey  key, TValue value)
	{
		if (ContainsKey(key))
			throw new ArgumentException($"An item with the key '{key}' has already been added.");
		
		int index = GetBucketIndex(key);

		_buckets[index] ??= new LinkedList<KeyValuePair<TKey , TValue>>();
		_buckets[index]!.AddLast(new KeyValuePair<TKey , TValue>(key, value));
		
		Count++;
		
		if ((double)Count / _buckets.Length > LoadFactor)
			Resize();
	}

	public TValue Get(TKey  key)
	{
		int index = GetBucketIndex(key);
		var bucket = _buckets[index];
		
		if (bucket != null)
			foreach (var pair in bucket)
				if (pair.Key!.Equals(key))
					return pair.Value;

		throw new KeyNotFoundException($"Key {key} not found");
	}

	public bool TryGetValue(TKey key, out TValue ? value)
	{
		int index = GetBucketIndex(key);
		var bucket = _buckets[index];

		if (bucket != null)
		{
			foreach (var pair in bucket)
				if (pair.Key!.Equals(key))
				{
					value = pair.Value;
					return true;
				}
		}

		value = default;
		return false;
	}
	
	public TValue this[TKey key]
	{
		get => Get(key);

		set
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
						var update = new KeyValuePair<TKey , TValue>(key, value);
						node.Value = update;
						return;
					}
					node =  node.Next;
				}
			}
			
			Add(key, value);
		}
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
		var newBuckets = new LinkedList<KeyValuePair<TKey , TValue>>?[_buckets.Length * 2];

		foreach (var bucket in _buckets)
		{
			if (bucket == null) continue;

			foreach (var pair in bucket)
			{
				int newIndex = Math.Abs(pair.Key!.GetHashCode()) % newBuckets.Length;
				newBuckets[newIndex] ??= new LinkedList<KeyValuePair<TKey , TValue>>();
				newBuckets[newIndex]!.AddLast(pair);
			}
		}
	}

	
}
