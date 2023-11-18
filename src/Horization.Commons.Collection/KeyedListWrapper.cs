using System.Runtime.CompilerServices;

namespace Horization.Commons.Collection;

/// <summary>
/// Represents a list which is also a dictionary
/// </summary>
/// <typeparam name="TWrapped">The type of the wrapped content</typeparam>
/// <typeparam name="TKey">The type of the keys</typeparam>
/// <typeparam name="TValue">The type of elements in the list</typeparam>
/// <remarks>
/// Each operation this list dose adding TWICE, this means the performance is not optimal.
/// </remarks>
public class KeyedListWrapper<TWrapped, TKey, TValue>
	: VirtualList<TValue>,
		IWrapper<KeyedListWrapper<TWrapped, TKey, TValue>, TWrapped>
	where TWrapped : IList<TValue>
	where TKey : notnull
{
	/// <inheritdoc />
	TWrapped IWrapper<KeyedListWrapper<TWrapped, TKey, TValue>, TWrapped>.Wrapped => (TWrapped)ListImpl;

	/// <inheritdoc />
	public bool PublicWrapped { get; }

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
	/// <param name="publicWrapped">The <see cref="PublicWrapped"/></param>
	/// <param name="dictionary">The <see cref="Dictionary"/>, NOTICE: it would be CLEARED it before using</param>
	public KeyedListWrapper(TWrapped listImpl, Func<TValue, TKey> keySelector, bool publicWrapped = false,
		IDictionary<TKey, TValue>? dictionary = null) : base(listImpl)
	{
		Dictionary = dictionary ?? new Dictionary<TKey, TValue>();
		if (Dictionary.Count != 0) Dictionary.Clear();
		KeySelector = keySelector;
		PublicWrapped = publicWrapped;
	}

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public override void Add(TValue item)
	{
		if (Dictionary.TryAdd(KeySelector(item), item)) base.Add(item);
		else throw new ArgumentException("Attempting to add duplicated item");
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
	public override bool Remove(TValue item) => base.Remove(item) && Dictionary.Remove(KeySelector(item));

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public override void Insert(int index, TValue item)
	{
		if (Dictionary.TryAdd(KeySelector(item), item)) base.Insert(index, item);
		else throw new ArgumentException("Attempting to add duplicated item");
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
	public virtual TValue this[TKey key] => Dictionary[key];

	/// <inheritdoc />
	public static implicit operator TWrapped(KeyedListWrapper<TWrapped, TKey, TValue> @this)
		=> ((IWrapper<KeyedListWrapper<TWrapped, TKey, TValue>, TWrapped>)@this).Unwrap();
}
