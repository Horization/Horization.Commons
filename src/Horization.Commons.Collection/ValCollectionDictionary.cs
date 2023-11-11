using System.Collections;
using System.Runtime.CompilerServices;

namespace Horization.Commons.Collection;

/// <summary>
/// Represents a dictionary which implements <see cref="ICollection{T}"/>
/// </summary>
/// <typeparam name="TKey">The type of keys in the dictionary</typeparam>
/// <typeparam name="TValue">The type of values in the dictionary</typeparam>
public class ValCollectionDictionary<TKey, TValue> : VirtualDictionary<TKey, TValue>, IDictionary<TKey, TValue>, ICollection<TValue> where TKey : notnull
{
	/// <summary>
	/// The factory used to select key from a value
	/// </summary>
	public readonly Func<TValue, TKey> KeySelector;

	/// <summary>
	/// Construct a new instance
	/// </summary>
	/// <param name="dictionaryImpl">The actual implementation which would be used</param>
	/// <param name="keySelector">The <see cref="KeySelector"/></param>
	public ValCollectionDictionary(IDictionary<TKey, TValue> dictionaryImpl, Func<TValue, TKey> keySelector) : base(dictionaryImpl)
	{
		KeySelector = keySelector;
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
}
