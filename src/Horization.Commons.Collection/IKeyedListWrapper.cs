namespace Horization.Commons.Collection;

/// <summary>
/// Represents a list which is also a dictionary
/// </summary>
/// <typeparam name="TSelf">The type of the derived class</typeparam>
/// <typeparam name="TWrapped">The type of the wrapped content</typeparam>
/// <typeparam name="TKey">The type of the keys</typeparam>
/// <typeparam name="TValue">The type of elements in the list</typeparam>
/// <remarks>
/// Each operation this list dose adding TWICE, this means the performance is not optimal.
/// </remarks>
public interface IKeyedListWrapper<in TSelf, out TWrapped, TKey, TValue>
	: IWrapper<TSelf, TWrapped>,
		IList<TValue> where TKey : notnull
	where TSelf : IKeyedListWrapper<TSelf, TWrapped, TKey, TValue>
{
	/// <inheritdoc cref="IDictionary{TKey,TValue}.Remove(TKey)"/>
	bool Remove(TKey key);

	/// <inheritdoc cref="IDictionary{TKey,TValue}.this[TKey]"/>
	TValue this[TKey key] { get; }

	/// <summary>
	/// The dictionary which is used to build key/value mapping
	/// </summary>
	protected IDictionary<TKey, TValue> Dictionary { get; }

	/// <summary>
	/// The factory used to select key from a value
	/// </summary>
	public Func<TValue, TKey> KeySelector { get; }
}
