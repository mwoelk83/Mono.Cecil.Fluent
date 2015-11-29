
// MS reference source:
// https://github.com/Microsoft/referencesource/blob/master/mscorlib/system/collections/generic/dictionary.cs
// MS license:
// https://github.com/Microsoft/referencesource/blob/master/LICENSE.txt

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Mono.Cecil.Fluent.Utils
{
	[DebuggerTypeProxy(typeof(FastDictionary<,>.DictionaryDebugView<,>))]
	[DebuggerDisplay("Count = {Count}")]
	internal sealed class FastDictionary<TKey, TValue> : IDictionary<TKey, TValue>
		where TKey : class
		//where TKey : IEquatable<TKey>
	{
		private struct Entry
		{
			public int HashCode;    // Lower 31 bits of hash code, -1 if unused
			public int Next;        // Index of Next entry, -1 if last
			public TKey Key;        // Key of entry
			public TValue Value;    // Value of entry
		}

		private int _bucketsCount;
		private int[] _buckets;
		private Entry[] _entries;
		private int _count;
		private int _version;
		private int _freeList;
		private int _freeCount;

		public FastDictionary()
		{
			_bucketsCount = 5;
			_buckets = new int[5];
			for (var i = 0; i < 5; i++)
				_buckets[i] = -1;
			_entries = new Entry[5];
			_freeList = -1;
		}

		public FastDictionary(int capacity)
		{
			var size = PrimeHelper.GetFastPrime(capacity <= 0 ? 7 : capacity);
			_bucketsCount = size;
			_buckets = new int[size];
			for (var i = 0; i < _bucketsCount; i++)
				_buckets[i] = -1;
			_entries = new Entry[size];
			_freeList = -1;
		}

		public int Count => _count - _freeCount;

		private KeyCollection _keys;
		public KeyCollection Keys => _keys ?? (_keys = new KeyCollection(this));

		ICollection<TKey> IDictionary<TKey, TValue>.Keys => _keys ?? (_keys = new KeyCollection(this));

		private ValueCollection _values;

		public ValueCollection Values => _values ?? (_values = new ValueCollection(this));

		ICollection<TValue> IDictionary<TKey, TValue>.Values => _values ?? (_values = new ValueCollection(this));

		public TValue this[TKey key]
		{
			get
			{
				if (key == null)
					throw new ArgumentNullException(nameof(key));

				var hashCode = key.GetHashCode() & 0x7FFFFFFF;
				for (var i = _buckets[hashCode % _bucketsCount]; i >= 0; i = _entries[i].Next)
				{
					if (_entries[i].HashCode != hashCode)
						continue;
					if (_entries[i].Key == key)
						return _entries[i].Value;
				}
				throw new KeyNotFoundException();
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				Insert(key, value);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Add(TKey key, TValue value)
		{
			Insert(key, value);
		}

		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> keyValuePair)
		{
			Add(keyValuePair.Key, keyValuePair.Value);
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> keyValuePair)
		{
			var i = FindEntry(keyValuePair.Key);
			if (i >= 0 && EqualityComparer<TValue>.Default.Equals(_entries[i].Value, keyValuePair.Value))
			{
				return true;
			}
			return false;
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> keyValuePair)
		{
			var i = FindEntry(keyValuePair.Key);
			if (i >= 0 && EqualityComparer<TValue>.Default.Equals(_entries[i].Value, keyValuePair.Value))
			{
				Remove(keyValuePair.Key);
				return true;
			}
			return false;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Clear()
		{
			if (_count > 0)
			{
				for (var i = 0; i < _bucketsCount; i++)
					_buckets[i] = -1;
				Array.Clear(_entries, 0, _count);
				_freeList = -1;
				_count = 0;
				_freeCount = 0;
				_version++;
			}
		}

		public bool ContainsKey(TKey key)
		{
			if (key == null)
				throw new ArgumentNullException(nameof(key));

			var hashCode = key.GetHashCode() & 0x7FFFFFFF;
			for (var i = _buckets[hashCode % _bucketsCount]; i >= 0; i = _entries[i].Next)
			{
				if (_entries[i].HashCode != hashCode)
					continue;
				if (_entries[i].Key == key)
					return true;
			}
			return false;
		}

		public bool ContainsValue(TValue value)
		{
			if (value == null)
			{
				for (var i = 0; i < _count; i++)
				{
					if (_entries[i].HashCode >= 0 && _entries[i].Value == null)
						return true;
				}
			}
			else
			{
				for (var i = 0; i < _count; i++)
				{
					if (_entries[i].HashCode >= 0 && EqualityComparer<TValue>.Default.Equals(_entries[i].Value, value))
						return true;
				}
			}
			return false;
		}

		private void CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
		{
			throw new NotImplementedException();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Enumerator GetEnumerator()
		{
			return new Enumerator(this);
		}

		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
		{
			return new Enumerator(this);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private int FindEntry(TKey key)
		{
			if (key == null)
				throw new ArgumentNullException(nameof(key));

			var hashCode = key.GetHashCode() & 0x7FFFFFFF;
			for (var i = _buckets[hashCode % _bucketsCount]; i >= 0; i = _entries[i].Next)
			{
				if (_entries[i].HashCode != hashCode)
					continue;
				if (_entries[i].Key == key)
					return i;
			}
			return -1;
		}

		private void Insert(TKey key, TValue value)
		{
			if (key == null)
				throw new ArgumentNullException(nameof(key));


			var hashCode = key.GetHashCode() & 0x7FFFFFFF;
			for (var i = _buckets[hashCode % _bucketsCount]; i >= 0; i = _entries[i].Next)
			{
				if (_entries[i].HashCode != hashCode || _entries[i].Key != key)
					continue;
				_entries[i].Value = value;
				_version++;
				return;
			}
			int index;
			if (_freeCount > 0)
			{
				index = _freeList;
				_freeList = _entries[index].Next;
				_freeCount--;
			}
			else
			{
				if (_count == _bucketsCount)
					Resize();
				index = _count;
				_count++;
			}
			var bucket = hashCode % _bucketsCount;
			_entries[index].HashCode = hashCode;
			_entries[index].Next = _buckets[bucket];
			_entries[index].Key = key;
			_entries[index].Value = value;
			_buckets[bucket] = index;
			_version++;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void Resize()
		{
			var newSize = PrimeHelper.GetFastPrime(_count * 2);

			_bucketsCount = newSize;
			var newBuckets = new int[newSize];
			for (var i = 0; i < newSize; i++)
				newBuckets[i] = -1;

			var newEntries = new Entry[newSize];
			Array.Copy(_entries, 0, newEntries, 0, _count);
			for (var i = 0; i < _count; i++)
			{
				var bucket = newEntries[i].HashCode % newSize;
				newEntries[i].Next = newBuckets[bucket];
				newBuckets[bucket] = i;
			}
			_buckets = newBuckets;
			_entries = newEntries;
		}

		public bool Remove(TKey key)
		{
			if (key == null)
				throw new ArgumentNullException(nameof(key));

			var hashCode = key.GetHashCode() & 0x7FFFFFFF;
			var bucket = hashCode % _bucketsCount;
			var last = -1;
			for (var i = _buckets[bucket]; i >= 0; last = i, i = _entries[i].Next)
			{
				if (_entries[i].HashCode == hashCode && _entries[i].Key == key)
				{
					if (last < 0)
					{
						_buckets[bucket] = _entries[i].Next;
					}
					else
					{
						_entries[last].Next = _entries[i].Next;
					}
					_entries[i].HashCode = -1;
					_entries[i].Next = _freeList;
					_entries[i].Key = default(TKey);
					_entries[i].Value = default(TValue);
					_freeList = i;
					_freeCount++;
					_version++;
					return true;
				}
			}
			return false;
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			if (key == null)
				throw new ArgumentNullException(nameof(key));

			var hashCode = key.GetHashCode() & 0x7FFFFFFF;
			for (var i = _buckets[hashCode % _bucketsCount]; i >= 0; i = _entries[i].Next)
			{
				var entry = _entries[i];
				if (entry.HashCode == hashCode && entry.Key == key)
				{
					value = entry.Value;
					return true;
				}
			}
			value = default(TValue);
			return false;
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
		{
			CopyTo(array, index);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new Enumerator(this);
		}

		public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>
		{
			private readonly FastDictionary<TKey, TValue> _dictionary;
			private readonly int _version;
			private int _index;
			private KeyValuePair<TKey, TValue> _current;

			internal Enumerator(FastDictionary<TKey, TValue> dictionary)
			{
				_dictionary = dictionary;
				_version = dictionary._version;
				_index = 0;
				_current = new KeyValuePair<TKey, TValue>();
			}

			public bool MoveNext()
			{
				if (_version != _dictionary._version)
				{
					throw new InvalidOperationException("InvalidOperation_EnumFailedVersion");
				}

				// Use unsigned comparison since we set index to dictionary._count+1 when the enumeration ends.
				// dictionary._count+1 could be negative if dictionary._count is Int32.MaxValue
				while ((uint)_index < (uint)_dictionary._count)
				{
					if (_dictionary._entries[_index].HashCode >= 0)
					{
						_current = new KeyValuePair<TKey, TValue>(_dictionary._entries[_index].Key, _dictionary._entries[_index].Value);
						_index++;
						return true;
					}
					_index++;
				}

				_index = _dictionary._count + 1;
				_current = new KeyValuePair<TKey, TValue>();
				return false;
			}

			public KeyValuePair<TKey, TValue> Current => _current;

			public void Dispose()
			{
			}

			object IEnumerator.Current
			{
				get
				{
					if (_index == 0 || (_index == _dictionary._count + 1))
						throw new InvalidOperationException("InvalidOperation_EnumOpCantHappen");

					return new KeyValuePair<TKey, TValue>(_current.Key, _current.Value);
				}
			}

			void IEnumerator.Reset()
			{
				if (_version != _dictionary._version)
				{
					throw new InvalidOperationException("InvalidOperation_EnumFailedVersion");
				}

				_index = 0;
				_current = new KeyValuePair<TKey, TValue>();
			}
		}

		[DebuggerDisplay("Count = {Count}")]
		public sealed class KeyCollection : ICollection<TKey>
		{
			private readonly FastDictionary<TKey, TValue> _dictionary;

			public KeyCollection(FastDictionary<TKey, TValue> dictionary)
			{
				if (dictionary == null)
				{
					throw new ArgumentNullException(nameof(dictionary));
				}
				_dictionary = dictionary;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public KeyCollectionEnumerator GetEnumerator()
			{
				return new KeyCollectionEnumerator(_dictionary);
			}

			public void CopyTo(TKey[] array, int index)
			{
				throw new NotImplementedException();
			}

			public int Count => _dictionary.Count;

			bool ICollection<TKey>.IsReadOnly => true;

			void ICollection<TKey>.Add(TKey item)
			{
				throw new NotSupportedException("NotSupported_KeyCollectionSet");
			}

			void ICollection<TKey>.Clear()
			{
				throw new NotSupportedException("NotSupported_KeyCollectionSet");
			}

			bool ICollection<TKey>.Contains(TKey item)
			{
				return _dictionary.ContainsKey(item);
			}

			bool ICollection<TKey>.Remove(TKey item)
			{
				throw new NotSupportedException("NotSupported_KeyCollectionSet");
			}

			IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator()
			{
				return new KeyCollectionEnumerator(_dictionary);
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return new KeyCollectionEnumerator(_dictionary);
			}

			public struct KeyCollectionEnumerator : IEnumerator<TKey>
			{
				private readonly FastDictionary<TKey, TValue> _dictionary;
				private int _index;
				private readonly int _version;
				private TKey _currentKey;

				internal KeyCollectionEnumerator(FastDictionary<TKey, TValue> dictionary)
				{
					_dictionary = dictionary;
					_version = dictionary._version;
					_index = 0;
					_currentKey = default(TKey);
				}

				public void Dispose()
				{
				}

				public bool MoveNext()
				{
					if (_version != _dictionary._version)
						throw new InvalidOperationException("InvalidOperation_EnumFailedVersion");

					while ((uint)_index < (uint)_dictionary._count)
					{
						if (_dictionary._entries[_index].HashCode >= 0)
						{
							_currentKey = _dictionary._entries[_index].Key;
							_index++;
							return true;
						}
						_index++;
					}

					_index = _dictionary._count + 1;
					_currentKey = default(TKey);
					return false;
				}

				public TKey Current => _currentKey;

				object IEnumerator.Current => _currentKey;

				void IEnumerator.Reset()
				{
					if (_version != _dictionary._version)
						throw new InvalidOperationException("InvalidOperation_EnumFailedVersion");

					_index = 0;
					_currentKey = default(TKey);
				}
			}
		}

		[DebuggerDisplay("Count = {Count}")]
		public sealed class ValueCollection : ICollection<TValue>
		{
			private readonly FastDictionary<TKey, TValue> _dictionary;

			public ValueCollection(FastDictionary<TKey, TValue> dictionary)
			{
				if (dictionary == null)
				{
					throw new ArgumentNullException(nameof(dictionary));
				}
				_dictionary = dictionary;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ValueCollectionEnumerator GetEnumerator()
			{
				return new ValueCollectionEnumerator(_dictionary);
			}

			public void CopyTo(TValue[] array, int index)
			{
				throw new NotImplementedException();
			}

			public int Count => _dictionary.Count;

			bool ICollection<TValue>.IsReadOnly => true;

			void ICollection<TValue>.Add(TValue item)
			{
				throw new NotSupportedException("NotSupported_ValueCollectionSet");
			}

			bool ICollection<TValue>.Remove(TValue item)
			{
				throw new NotSupportedException("NotSupported_ValueCollectionSet");
			}

			void ICollection<TValue>.Clear()
			{
				throw new NotSupportedException("NotSupported_ValueCollectionSet");
			}

			bool ICollection<TValue>.Contains(TValue item)
			{
				return _dictionary.ContainsValue(item);
			}

			IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
			{
				return new ValueCollectionEnumerator(_dictionary);
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return new ValueCollectionEnumerator(_dictionary);
			}

			public struct ValueCollectionEnumerator : IEnumerator<TValue>
			{
				private readonly FastDictionary<TKey, TValue> _dictionary;
				private int _index;
				private readonly int _version;
				private TValue _currentValue;

				internal ValueCollectionEnumerator(FastDictionary<TKey, TValue> dictionary)
				{
					_dictionary = dictionary;
					_version = dictionary._version;
					_index = 0;
					_currentValue = default(TValue);
				}

				public void Dispose()
				{
				}

				public bool MoveNext()
				{
					if (_version != _dictionary._version)
					{
						throw new InvalidOperationException("InvalidOperation_EnumFailedVersion");
					}

					while ((uint)_index < (uint)_dictionary._count)
					{
						if (_dictionary._entries[_index].HashCode >= 0)
						{
							_currentValue = _dictionary._entries[_index].Value;
							_index++;
							return true;
						}
						_index++;
					}
					_index = _dictionary._count + 1;
					_currentValue = default(TValue);
					return false;
				}

				public TValue Current => _currentValue;

				object IEnumerator.Current => _currentValue;

				void IEnumerator.Reset()
				{
					if (_version != _dictionary._version)
						throw new InvalidOperationException("InvalidOperation_EnumFailedVersion");

					_index = 0;
					_currentValue = default(TValue);
				}
			}
		}

		internal sealed class DictionaryDebugView<K, V>
		{
			private IDictionary<K, V> dict;

			public DictionaryDebugView(IDictionary<K, V> dictionary)
			{
				dict = dictionary;
			}

			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public KeyValuePair<K, V>[] Items => dict.ToArray();
		}
	}

	internal static class PrimeHelper
	{
		internal static int GetFastPrime(int min)
		{
			if (min <= 47)
			{
				if (min <= 7)
					return min <= 3 ? 3 : 7;
				if (min <= 29)
					return min <= 17 ? 17 : 29;
				return 47;
			}
			if (min <= 1103)
			{
				if (min <= 107)
					return min <= 71 ? 71 : 107;
				if (min <= 239)
					return min <= 163 ? 163 : 239;
				if (min <= 521)
					return min <= 353 ? 353 : 521;
				return min <= 761 ? 761 : 1103;
			}
			if (min <= 8419)
			{
				if (min <= 1597) return 1597;
				if (min <= 1931) return 1931;
				if (min <= 2801) return 2801;
				if (min <= 4049) return 4049;
				if (min <= 5839) return 5839;
				return 8419;

			}
			if (min <= 75431)
			{
				if (min <= 12143) return 12143;
				if (min <= 17519) return 17519;
				if (min <= 25229) return 25229;
				if (min <= 36353) return 36353;
				if (min <= 52361) return 52361;
				return 75431;
			}
			if (min <= 968897)
			{
				if (min <= 108631) return 108631;
				if (min <= 156437) return 156437;
				if (min <= 225307) return 225307;
				if (min <= 324449) return 324449;
				if (min <= 467237) return 467237;
				if (min <= 672827) return 672827;
				return 968897;
			}

			if (min <= 1395263) return 1395263;
			if (min <= 2009191) return 2009191;
			if (min <= 2893249) return 2893249;
			if (min <= 4166287) return 4166287;
			if (min <= 5999471) return 5999471;
			if (min <= 7199369) return 7199369;

			//outside of our predefined table.
			//compute the hard way.
			for (var i = (min | 1); i < int.MaxValue; i += 2)
			{
				var isprime = true;
				var limit = (int)Math.Sqrt(i);
				for (var divisor = 3; divisor <= limit; divisor += 2)
				{
					if (i % divisor != 0)
						continue;
					isprime = false;
					break;
				}
				if (isprime)
					return i;
			}
			return min;
		}
	}
}
