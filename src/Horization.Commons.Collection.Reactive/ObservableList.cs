using System.Reactive.Subjects;
using System.Runtime.CompilerServices;

namespace Horization.Commons.Collection.Reactive;

/// <summary>
/// Represents a list which implements <see cref="IObservable{T}"/>
/// </summary>
/// <typeparam name="T">The type of elements in the list</typeparam>
public class ObservableList<T> : VirtualList<T>, IObservable<ObservableList<T>.Notification>
{
	private readonly SubjectBase<Notification> _subject = new Subject<Notification>();

	/// <summary>
	/// Construct a new instance
	/// </summary>
	/// <param name="listImpl">The actual implementation which would be used</param>
	public ObservableList(IList<T> listImpl) : base(listImpl)
	{
	}

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public IDisposable Subscribe(IObserver<Notification> observer) => _subject.Subscribe(observer);


	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public override void Add(T item)
	{
		base.Add(item);
		_subject.OnNextIfHavingObservers(new Notification(ObservableList.Event.OnUpdated | ObservableList.Event.OnAdded, this, Count - 1, item, default));
	}
	
	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public override void Clear()
	{
		base.Clear();
		_subject.OnNextIfHavingObservers(new Notification(ObservableList.Event.OnUpdated | ObservableList.Event.OnCleared, this, null, default, default));
	}

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public override bool Remove(T item)
	{
		var removed = base.Remove(item);
		if (removed)
			_subject.OnNextIfHavingObservers(new Notification(ObservableList.Event.OnUpdated | ObservableList.Event.OnRemoved, this, null, item, default));
		return removed;
	}
	
	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public override void Insert(int index, T item)
	{
		base.Insert(index, item);
		_subject.OnNextIfHavingObservers(new Notification(ObservableList.Event.OnUpdated | ObservableList.Event.OnInserted, this, index, item, default));
	}

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public override void RemoveAt(int index)
	{
		var item = _subject.HasObservers ? this[index] : default;
		base.RemoveAt(index);
		_subject.OnNextIfHavingObservers(new Notification(ObservableList.Event.OnUpdated | ObservableList.Event.OnRemoved, this, index, item, default));
	}

	/// <inheritdoc />
	public override T this[int index]
	{
		get => base[index];
		set
		{
			var old = _subject.HasObservers ? this[index] : default;
			base[index] = value;
			_subject.OnNext(new Notification(ObservableList.Event.OnUpdated | ObservableList.Event.OnSet, this, index, value, old));
		}
	}

	/// <summary>
	/// Used to carry details
	/// </summary>
	/// <param name="Event">The type of the event</param>
	/// <param name="This">The instance which pushed this notification</param>
	/// <param name="Idx">The index which the operated item is</param>
	/// <param name="Item">The item which has been operated</param>
	/// <param name="OldItem">The old item which has been operated and replaced in set-indexer</param>
	public readonly record struct Notification(ObservableList.Event Event, ObservableList<T> This, int? Idx, T? Item, T? OldItem);
}

#pragma warning disable CS1591
public static class ObservableList
#pragma warning restore CS1591
{
	/// <summary>
	/// Represents a set of events which may take place
	/// </summary>
	[Flags]
	public enum Event
	{
		/// <summary>
		/// When the list has been updated
		/// </summary>
		OnUpdated = 1 << 0,
		/// <summary>
		/// When the list has been updated by <see cref="IList{T}.set_Item"/>
		/// </summary>
		OnSet = 1 << 1,
		/// <summary>
		/// When the list has been updated by <see cref="IList{T}.Add"/>
		/// </summary>
		OnAdded = 1 << 2,
		/// <summary>
		/// When the list has been updated by <see cref="IList{T}.Remove"/>
		/// </summary>
		OnRemoved = 1 << 3,
		/// <summary>
		/// When the list has been updated by <see cref="IList{T}.Clear"/>
		/// </summary>
		OnCleared = 1 << 4,
		/// <summary>
		/// When the list has been updated by <see cref="IList{T}.Insert"/>
		/// </summary>
		OnInserted = 1 << 5
	}
}
