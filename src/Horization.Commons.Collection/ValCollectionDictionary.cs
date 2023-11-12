using System.Collections;
using System.Runtime.CompilerServices;

namespace Horization.Commons.Collection;

/// <summary>
/// Represents a dictionary which implements <see cref="ICollection{T}"/>
/// </summary>
/// <typeparam name="TWrapped">The type of the wrapped content</typeparam>
/// <typeparam name="TKey">The type of keys in the dictionary</typeparam>
/// <typeparam name="TValue">The type of values in the dictionary</typeparam>
public class ValCollectionDictionary<TWrapped, TKey, TValue>
	: VirtualDictionary<TKey, TValue>,
		IDictionary<TKey, TValue>, ICollection<TValue>,
		IWrapper<ValCollectionDictionary<TWrapped, TKey, TValue>, TWrapped>
	where TWrapped : IDictionary<TKey, TValue>
	where TKey : notnull
{
	/// <summary>
	/// The factory used to select key from a value
	/// </summary>
	public readonly Func<TValue, TKey> KeySelector;

	/// <inheritdoc />
	TWrapped IWrapper<ValCollectionDictionary<TWrapped, TKey, TValue>, TWrapped>.Wrapped => (TWrapped)DictionaryImpl;

	/// <inheritdoc />
	public bool PublicWrapped { get; }

	/// <summary>
	/// Construct a new instance
	/// </summary>
	/// <param name="dictionaryImpl">The <see cref="VirtualDictionary{TKey,TValue}.DictionaryImpl"/></param>
	/// <param name="keySelector">The <see cref="KeySelector"/></param>
	/// <param name="publicWrapped">The <see cref="PublicWrapped"/></param>
	public ValCollectionDictionary(TWrapped dictionaryImpl, Func<TValue, TKey> keySelector, bool publicWrapped = false) :
		base(dictionaryImpl)
	{
		KeySelector = keySelector;
		PublicWrapped = publicWrapped;
	}

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator() => Values.GetEnumerator();

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public virtual void Add(TValue item) => Add(KeySelector(item), item);

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public virtual bool Contains(TValue item) => Values.Contains(item);

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public virtual void CopyTo(TValue[] array, int arrayIndex) => Values.CopyTo(array, arrayIndex);

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public virtual bool Remove(TValue item) => Remove(KeySelector(item));

	/// <inheritdoc/>
	public static implicit operator TWrapped(ValCollectionDictionary<TWrapped, TKey, TValue> @this) =>
		((IWrapper<ValCollectionDictionary<TWrapped, TKey, TValue>, TWrapped>)@this).Unwrap();
}
