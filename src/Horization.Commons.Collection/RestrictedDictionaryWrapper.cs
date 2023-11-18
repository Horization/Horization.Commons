using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Horization.Commons.Collection;

/// <summary>
/// Represents a dictionary with operation restriction
/// </summary>
/// <typeparam name="TWrapped">The type of the wrapped content</typeparam>
/// <typeparam name="TKey">The type of keys in the dictionary</typeparam>
/// <typeparam name="TValue">The type of values in the dictionary</typeparam>
/// <remarks>
/// Just a wrapper for VirtualDictionary with a restriction policy, it may be SLOW
/// </remarks>
public class RestrictedDictionaryWrapper<TWrapped, TKey, TValue>
	: VirtualDictionary<TKey, TValue>,
		IWrapper<RestrictedDictionaryWrapper<TWrapped, TKey, TValue>, TWrapped>
	where TKey : notnull
{
	/// <summary>
	/// The restriction policy
	/// </summary>
	public readonly RestrictedDictionaryWrapper.OperationRestriction Restriction;

	/// <inheritdoc />
	TWrapped IWrapper<RestrictedDictionaryWrapper<TWrapped, TKey, TValue>, TWrapped>.Wrapped => (TWrapped)DictionaryImpl;


	/// <inheritdoc />
	public bool PublicWrapped { get; }

	/// <summary>
	/// Construct a new instance
	/// </summary>
	/// <param name="dictionaryImpl">The <see cref="VirtualDictionary{TKey,TValue}.DictionaryImpl"/></param>
	/// <param name="restriction">The <see cref="Restriction"/></param>
	/// <param name="publicWrapped">The <see cref="PublicWrapped"/></param>
	public RestrictedDictionaryWrapper(IDictionary<TKey, TValue> dictionaryImpl, RestrictedDictionaryWrapper.OperationRestriction restriction,
		bool publicWrapped = false) : base(dictionaryImpl)
	{
		Restriction = restriction;
		PublicWrapped = publicWrapped;
	}

	/// <inheritdoc />
	public override int Count =>
		(Restriction & RestrictedDictionaryWrapper.OperationRestriction.CountItems) != 0
			? throw new InvalidOperationException()
			: base.Count;

	/// <inheritdoc />
	public override ICollection<TKey> Keys =>
		(Restriction & RestrictedDictionaryWrapper.OperationRestriction.EnumerateKeys) != 0
			? throw new InvalidOperationException()
			: base.Keys;

	/// <inheritdoc />
	public override ICollection<TValue> Values =>
		(Restriction & RestrictedDictionaryWrapper.OperationRestriction.EnumerateValues) != 0
			? throw new InvalidOperationException()
			: base.Values;

	/// <inheritdoc />
	public override TValue this[TKey key]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => (Restriction & RestrictedDictionaryWrapper.OperationRestriction.GetItem) != 0 ? throw new InvalidOperationException() : base[key];
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		set
		{
			if ((Restriction & RestrictedDictionaryWrapper.OperationRestriction.SetItem) != 0) throw new InvalidOperationException();
			base[key] = value;
		}
	}

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public override void Add(TKey key, TValue value)
	{
		if ((Restriction & RestrictedDictionaryWrapper.OperationRestriction.AddItem) != 0) throw new InvalidOperationException();
		base.Add(key, value);
	}

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public override void Clear()
	{
		if ((Restriction & RestrictedDictionaryWrapper.OperationRestriction.Clear) != 0) throw new InvalidOperationException();
		base.Clear();
	}

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public override bool Remove(TKey key)
	{
		if ((Restriction & RestrictedDictionaryWrapper.OperationRestriction.RemoveItem) != 0) throw new InvalidOperationException();
		return base.Remove(key);
	}

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public override IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
	{
		if ((Restriction & RestrictedDictionaryWrapper.OperationRestriction.EnumerateItems) != 0) throw new InvalidOperationException();
		return base.GetEnumerator();
	}

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public override bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
	{
		if ((Restriction & RestrictedDictionaryWrapper.OperationRestriction.AttemptGettingValue) != 0) throw new InvalidOperationException();
		return base.TryGetValue(key, out value);
	}

	/// <inheritdoc />
	public static implicit operator TWrapped(RestrictedDictionaryWrapper<TWrapped, TKey, TValue> @this)
		=> ((IWrapper<RestrictedDictionaryWrapper<TWrapped, TKey, TWrapped>, TWrapped>)@this).Unwrap();
}

#pragma warning disable CS1591
public static class RestrictedDictionaryWrapper
#pragma warning restore CS1591
{
	/// <summary>
	/// Represents a set of operations which would be restricted
	/// </summary>
	[Flags]
	public enum OperationRestriction
	{
		/// <summary>
		/// Represents <see cref="IDictionary{TKey,TValue}.get_Item"/>
		/// </summary>
		GetItem = 1 << 0,

		/// <summary>
		/// Represents <see cref="IDictionary{TKey,TValue}.set_Item"/>
		/// </summary>
		SetItem = 1 << 1,

		/// <summary>
		/// Represents <see cref="IDictionary{TKey,TValue}.Add(TKey, TValue)"/>
		/// </summary>
		AddItem = 1 << 2,

		/// <summary>
		/// Represents <see cref="IDictionary{TKey,TValue}.Clear"/>
		/// </summary>
		Clear = 1 << 3,

		/// <summary>
		/// Represents <see cref="IDictionary{TKey,TValue}.Count"/>
		/// </summary>
		CountItems = 1 << 4,

		/// <summary>
		/// Represents <see cref="IDictionary{TKey,TValue}.Keys"/>
		/// </summary>
		EnumerateKeys = 1 << 5,

		/// <summary>
		/// Represents <see cref="IDictionary{TKey,TValue}.Values"/>
		/// </summary>
		EnumerateValues = 1 << 6,

		/// <summary>
		/// Represents <see cref="IDictionary{TKey,TValue}.Remove(TKey)"/>
		/// </summary>
		RemoveItem = 1 << 7,

		/// <summary>
		/// Represents <see cref="IDictionary{TKey,TValue}.TryGetValue"/>
		/// </summary>
		AttemptGettingValue = 1 << 8,

		/// <summary>
		/// Represents <see cref="IDictionary{TKey,TValue}.GetEnumerator"/>
		/// </summary>
		EnumerateItems = 1 << 9
	}
}
