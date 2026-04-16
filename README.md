# CSharpDictionary

A custom generic `Dictionary<TKey, TValue>` implementation in C#, built from scratch as a learning exercise.

## How it works

A **hash table** with **chaining** for collision resolution.

- Each key is hashed and mapped to a bucket index via `% buckets.Length`
- Each bucket is a `LinkedList` to handle collisions
- When load factor exceeds **0.75** — the array doubles and all entries are rehashed

## Features

| Method | Description |
|---|---|
| `Add(key, value)` | Adds a key-value pair |
| `Get(key)` | Returns value by key, throws `KeyNotFoundException` if missing |
| `Remove(key)` | Removes a pair, returns `true` if found |
| `ContainsKey(key)` | Returns `true` if key exists |
| `Count` | Number of elements currently stored |
| `foreach` | Iteration via `IEnumerable<KeyValuePair<TKey, TValue>>` |