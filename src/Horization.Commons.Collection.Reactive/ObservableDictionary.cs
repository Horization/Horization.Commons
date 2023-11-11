using System.Reactive.Subjects;
using System.Runtime.CompilerServices;

namespace Horization.Commons.Collection.Reactive;

/// <summary>
/// Represents a dictionary which implements <see cref="IObservable{T}"/>
/// </summary>
/// <typeparam name="TKey">The type of keys in the dictionary</typeparam>
/// <typeparam name="TValue">The type of values in the dictionary</typeparam>
public class ObservableDictionary<TKey, TValue> : VirtualDictionary<TKey, TValue>, IObservable<ObservableDictionary<TKey,TValue>.Notification> where TKey : notnull
{
	private readonly SubjectBase<Notification> _subject = new Subject<Notification>();

	/// <summary>
	/// Construct a new instance
	/// </summary>
	/// <param name="dictionaryImpl">The actual implementation which would be used</param>
	public ObservableDictionary(IDictionary<TKey, TValue> dictionaryImpl) : base(dictionaryImpl)
	{
	}

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public virtual IDisposable Subscribe(IObserver<Notification> observer) => _subject.Subscribe(observer);

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public override bool Remove(TKey key)
	{
		TValue? value;
		if (_subject.HasObservers) TryGetValue(key, out value);
		else value = default;
		var removed = base.Remove(key);
		if (removed)
			_subject.OnNext(new Notification(ObservableDictionary.Event.OnUpdated | ObservableDictionary.Event.OnRemoved, this, key, value, default));
		return removed;
	}

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public override void Add(TKey key, TValue value)
	{
		base.Add(key, value);
		_subject.OnNext(new Notification(ObservableDictionary.Event.OnUpdated | ObservableDictionary.Event.OnAdded, this, key, value, default));
	}

	/// <inheritdoc />
	public override TValue this[TKey key]
	{
		get => base[key];
		set
		{
			TValue? old;
			if (_subject.HasObservers) TryGetValue(key, out old);
			else old = default;
			base[key] = value;
			_subject.OnNext(new Notification(ObservableDictionary.Event.OnUpdated | ObservableDictionary.Event.OnSet, this, key, value, old));
		}
	}

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public override void Clear()
	{
		base.Clear();
		_subject.OnNext(new Notification(ObservableDictionary.Event.OnUpdated | ObservableDictionary.Event.OnCleared, this, default, default, default));
	}

	/// <summary>
	/// Used to carry details
	/// </summary>
	/// <param name="Event">The type of the event</param>
	/// <param name="This">The instance which pushed this notification</param>
	/// <param name="Key">The key which the operated item is</param>
	/// <param name="Value">The value which has been operated</param>
	/// <param name="OldValue">The old value which has been operated and replaced in set-indexer</param>
	public readonly record struct Notification(ObservableDictionary.Event Event, ObservableDictionary<TKey, TValue> This, TKey? Key, TValue? Value, TValue? OldValue);
}

#pragma warning disable CS1591
public static class ObservableDictionary
#pragma warning restore CS1591
{
	/// <summary>
	/// Represents a set of events which may take place
	/// </summary>
	[Flags]
	public enum Event
	{
		/// <summary>
		/// When the dictionary has been updated
		/// </summary>
		OnUpdated = 1 << 0,
		/// <summary>
		/// When the dictionary has been updated by <see cref="IDictionary{TKey, TValue}.Add(TKey, TValue)"/>
		/// </summary>
		OnAdded = 1 << 1,
		/// <summary>
		/// When the dictionary has been updated by <see cref="IDictionary{TKey,TValue}.set_Item"/>
		/// </summary>
		OnSet = 1 << 2,
		/// <summary>
		/// When the dictionary has been updated by <see cref="IDictionary{TKey,TValue}.Remove(TKey)"/>
		/// </summary>
		OnRemoved = 1 << 3,
		/// <summary>
		/// When the dictionary has been updated by <see cref="IDictionary{TKey,TValue}.Clear"/>
		/// </summary>
		OnCleared = 1 << 4
	}
}
