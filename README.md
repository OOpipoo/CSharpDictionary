# CSharpDictionary

A custom generic `Dictionary<TKey, TValue>` implementation in C#, built from scratch as a learning exercise.

## How it works

A **hash table** with **chaining** for collision resolution.

- Each key is hashed and mapped to a bucket index via `% buckets.Length`
- Each bucket is a `LinkedList` to handle collisions
- When load factor exceeds **0.75** the array doubles and all entries are rehashed

## MyDictionary

| Method | Description |
|---|---|
| `Add(key, value)` | Adds a key-value pair, throws `ArgumentException` on duplicate key |
| `Get(key)` | Returns value by key, throws `KeyNotFoundException` if missing |
| `Remove(key)` | Removes a pair, returns `true` if found |
| `ContainsKey(key)` | Returns `true` if key exists |
| `TryGetValue(key, out value)` | Returns `false` instead of throwing if key is missing |
| `dict[key]` | Get or set value via indexer |
| `Keys` | Returns all keys |
| `Values` | Returns all values |
| `Count` | Number of elements currently stored |
| `foreach` | Iteration via `IEnumerable<KeyValuePair<TKey, TValue>>` |

## MyConcurrentDictionary

Thread-safe version using **striped locking** - each bucket has its own lock so threads working on different buckets don't block each other.

| Method | Description |
|---|---|
| `Add(key, value)` | Thread-safe add |
| `Get(key)` | Thread-safe get |
| `TryGetValue(key, out value)` | Thread-safe try get |
| `Remove(key)` | Thread-safe remove |
| `Count` | Thread-safe count via `Interlocked` |
