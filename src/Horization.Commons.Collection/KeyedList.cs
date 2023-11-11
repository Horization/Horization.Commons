using System.Runtime.CompilerServices;

namespace Horization.Commons.Collection;

/// <summary>
/// Represents a list which is also a dictionary
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
/// <remarks>
/// Each operation this list dose adding TWICE, this means the performance is not optimal.
/// </remarks>
public class KeyedList<TKey, TValue> : VirtualList<TValue> where TKey : notnull
{
	/// <summary>
	/// The dictionary which is used to build key/value mapping
	/// </summary>
	protected readonly IDictionary<TKey, TValue> Dictionary;

	/// <summary>
	/// The factory used to select key from a value
	/// </summary>
	public readonly Func<TValue, TKey> KeySelector;

	/// <summary>
	/// Construct a new instance
	/// </summary>
	/// <param name="listImpl">The actual implementation of <see cref="IList{T}"/></param>
	/// <param name="keySelector">The <see cref="KeySelector"/></param>
	/// <param name="dictionary">The <see cref="Dictionary"/>, NOTICE: it would be CLEARED it before using</param>
	public KeyedList(IList<TValue> listImpl, Func<TValue, TKey> keySelector, IDictionary<TKey, TValue>? dictionary = null) : base(listImpl)
	{
		Dictionary = dictionary ?? new Dictionary<TKey, TValue>();
		if (Dictionary.Count != 0) Dictionary.Clear();
		KeySelector = keySelector;
	}

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public override void Add(TValue item)
	{
		if (Dictionary.TryAdd(KeySelector(item), item)) base.Add(item);
		throw new ArgumentException("Attempting to add duplicated item");
	}

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public override void Clear()
	{
		Dictionary.Clear();
		base.Clear();
	}

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public override bool Remove(TValue item)
	{
		return base.Remove(item) && Dictionary.Remove(KeySelector(item));
	}

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public override void Insert(int index, TValue item)
	{
		if (Dictionary.TryAdd(KeySelector(item), item)) base.Insert(index, item);
		throw new ArgumentException("Attempting to add duplicated item");
	}

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public override void RemoveAt(int index)
	{
		var a = base[index];
		base.RemoveAt(index);
		Dictionary.Remove(KeySelector(a));
	}

	/// <inheritdoc cref="IDictionary{TKey,TValue}.Remove(TKey)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public virtual bool Remove(TKey key)
	{
		Dictionary.TryGetValue(key, out var value);
#pragma warning disable CS8604
		return Dictionary.Remove(key) && ListImpl.Remove(value);
#pragma warning restore CS8604
	}

	/// <inheritdoc cref="IDictionary{TKey,TValue}.this[TKey]"/>
	public virtual TValue this[TKey key]  => Dictionary[key];
}
