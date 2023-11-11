using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Horization.Commons.Collection;

/// <summary>
/// Represents a dictionary which can be inherited
/// </summary>
/// <typeparam name="TKey">The type of keys in the dictionary</typeparam>
/// <typeparam name="TValue">The type of values in the dictionary</typeparam>
public class VirtualDictionary<TKey, TValue> : IDictionary<TKey, TValue> where TKey : notnull
{
	/// <summary>
	/// The actual implementation of <see cref="IDictionary{TKey,TValue}"/>
	/// </summary>
	protected readonly IDictionary<TKey, TValue> DictionaryImpl;

	/// <summary>
	/// Construct a new instance
	/// </summary>
	/// <param name="dictionaryImpl">The actual implementation which would be used</param>
	protected VirtualDictionary(IDictionary<TKey, TValue> dictionaryImpl) => DictionaryImpl = dictionaryImpl;

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public virtual IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => DictionaryImpl.GetEnumerator();

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)DictionaryImpl).GetEnumerator();

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public virtual void Add(KeyValuePair<TKey, TValue> item) => DictionaryImpl.Add(item);

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public virtual void Clear() => DictionaryImpl.Clear();

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public virtual bool Contains(KeyValuePair<TKey, TValue> item) => DictionaryImpl.Contains(item);

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public virtual void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => DictionaryImpl.CopyTo(array, arrayIndex);

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public virtual bool Remove(KeyValuePair<TKey, TValue> item) => DictionaryImpl.Remove(item);

	/// <inheritdoc />
	public virtual int Count => DictionaryImpl.Count;

	/// <inheritdoc />
	public virtual bool IsReadOnly => DictionaryImpl.IsReadOnly;

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public virtual void Add(TKey key, TValue value) => DictionaryImpl.Add(key, value);

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public virtual bool ContainsKey(TKey key) => DictionaryImpl.ContainsKey(key);

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public virtual bool Remove(TKey key) => DictionaryImpl.Remove(key);

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public virtual bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) => DictionaryImpl.TryGetValue(key, out value);

	/// <inheritdoc />
	public virtual TValue this[TKey key]
	{
		get => DictionaryImpl[key];
		set => DictionaryImpl[key] = value;
	}

	/// <inheritdoc />
	public virtual ICollection<TKey> Keys => DictionaryImpl.Keys;

	/// <inheritdoc />
	public virtual ICollection<TValue> Values => DictionaryImpl.Values;
}
