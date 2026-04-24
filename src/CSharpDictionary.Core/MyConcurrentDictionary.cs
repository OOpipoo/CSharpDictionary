namespace CSharpDictionary.Core;

public class MyConcurrentDictionary<TKey, TValue>
{
	private const int DefaultCapacity = 16;
	private const double LoadFactor = 0.75;

	private LinkedList<KeyValuePair<TKey, TValue>> ?[] _buckets;
	private readonly object[] _locks;
	
	private int _count;
	public int Count => _count;

	public MyConcurrentDictionary()
	{
		_buckets = new LinkedList<KeyValuePair<TKey, TValue>>[DefaultCapacity];
		_locks = new object[DefaultCapacity];

		for (int i = 0; i < DefaultCapacity; i++)
		{
			_locks[i] = new object();
		}
	}

	private int GetBucketIndex(TKey key)
	{
		int hash = key!.GetHashCode();
		return Math.Abs(hash) % _buckets.Length;
	}

	private int GetLockIndex(int bucketIndex)
	{
		return bucketIndex % _locks.Length;
	}

	public void Add(TKey key, TValue value)
	{
		int index = GetBucketIndex(key);
		int lockIndex = GetLockIndex(index);

		lock (_locks[lockIndex])
		{
			if (ContainsKey(key))
				throw new ArgumentException($"Key '{key}' already exists.");

			_buckets[index] ??= new LinkedList<KeyValuePair<TKey, TValue>>();
			_buckets[index]!.AddLast(new KeyValuePair<TKey, TValue>(key, value));
			Interlocked.Increment(ref _count);

			if ((double)_count / _buckets.Length > LoadFactor)
				Resize();
		}
	}

	public TValue Get(TKey key)
	{
		int index = GetBucketIndex(key);
		int lockIndex = GetLockIndex(index);

		lock (_locks[lockIndex])
		{
			var bucket = _buckets[index];
			if (bucket != null)
				foreach(var pair in bucket)
					if (pair.Key!.Equals(key))
						return pair.Value;
		}
		
		throw new KeyNotFoundException($"Key '{key}' not found.");
	}

	public bool TryGetValue(TKey key, out TValue ? value)
	{
		int index = GetBucketIndex(key);
		int lockIndex = GetLockIndex(index);

		lock (_locks[lockIndex])
		{
			var bucket = _buckets[index];
			if (bucket != null)
			{
				foreach(var pair in bucket)
					if (pair.Key!.Equals(key))
					{
						value = pair.Value;
						return true;
					}
			}
		}

		value = default;
		return false;
	}

	public bool Remove(TKey key)
	{
		int index = GetBucketIndex(key);
		int lockIndex = GetLockIndex(index);

		lock (_locks[lockIndex])
		{
			var bucket = _buckets[index];
			if (bucket != null)
			{
				var node = bucket.First;
				while (node != null)
				{
					if (node.Value.Key!.Equals(key))
					{
						bucket.Remove(node);
						Interlocked.Decrement(ref _count);
						return true;
					}
					node = node.Next;
				}
			}
		}

		return false;
	}

	public bool ContainsKey(TKey key)
	{
		int index = GetBucketIndex(key);
		var bucket = _buckets[index];

		if (bucket != null)
			foreach (var pair in bucket)
				if (pair.Key!.Equals(key))
					return true;

		return false;
	}
	
	private void Resize()
	{
		var newBuckets = new LinkedList<KeyValuePair<TKey, TValue>>?[_buckets.Length * 2];

		foreach (var bucket in _buckets)
		{
			if (bucket == null) continue;
			foreach (var pair in bucket)
			{
				int newIndex = Math.Abs(pair.Key!.GetHashCode()) % newBuckets.Length;
				newBuckets[newIndex] ??= new LinkedList<KeyValuePair<TKey, TValue>>();
				newBuckets[newIndex]!.AddLast(pair);
			}
		}

		_buckets = newBuckets;
	}
}