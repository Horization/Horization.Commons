using System.Reactive.Subjects;
using System.Runtime.CompilerServices;

namespace Horization.Commons.Collection.Reactive;

/// <summary>
/// Represents a set which implements <see cref="IObservable{T}"/>
/// </summary>
/// <typeparam name="T">The type of elements in the set</typeparam>
public class ObservableSet<T> : VirtualSet<T>, IObservable<ObservableSet<T>.Notification>
{
	private readonly SubjectBase<Notification> _subject = new Subject<Notification>();

	/// <summary>
	/// Construct a new instance
	/// </summary>
	/// <param name="setImpl"></param>
	public ObservableSet(ISet<T> setImpl) : base(setImpl)
	{
	}

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public IDisposable Subscribe(IObserver<Notification> observer) => _subject.Subscribe(observer);

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public override bool Add(T item)
	{
		var added = base.Add(item);
		if (added)
			_subject.OnNext(new Notification(ObservableSet.Event.OnUpdated | ObservableSet.Event.OnAdded, this, item, null));
		return added;
	}

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public override bool Remove(T item)
	{
		var removed = base.Remove(item);
		if (removed)
			_subject.OnNext(new Notification(ObservableSet.Event.OnUpdated | ObservableSet.Event.OnRemoved, this, item, null));
		return removed;
	}

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public override void Clear()
	{
		base.Clear();
		_subject.OnNext(new Notification(ObservableSet.Event.OnUpdated | ObservableSet.Event.OnCleared, this, default, null));
	}

	// ReSharper disable PossibleMultipleEnumeration
	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public override void ExceptWith(IEnumerable<T> other)
	{
		_subject.OnNext(new Notification(ObservableSet.Event.OnExceptWith, this, default, other));
		base.ExceptWith(other);
	}

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public override void IntersectWith(IEnumerable<T> other)
	{
		_subject.OnNext(new Notification(ObservableSet.Event.OnIntersectWith, this, default, other));
		base.IntersectWith(other);
	}

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public override void UnionWith(IEnumerable<T> other)
	{
		_subject.OnNext(new Notification(ObservableSet.Event.OnUnionWith, this, default, other));
		base.UnionWith(other);
	}

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public override void SymmetricExceptWith(IEnumerable<T> other)
	{
		_subject.OnNext(new Notification(ObservableSet.Event.OnSymmetricExceptWith, this, default, other));
		base.SymmetricExceptWith(other);
	}

	/// <summary>
	/// Used to carry details
	/// </summary>
	/// <param name="Event">The type of the event</param>
	/// <param name="This">The instance which pushed this notification</param>
	/// <param name="Item">The item which has been operated</param>
	/// <param name="OtherSet">The other <see cref="ISet{T}"/> which is used to operate</param>
	public readonly record struct Notification(ObservableSet.Event Event, ObservableSet<T> This, T? Item, IEnumerable<T>? OtherSet);
}

#pragma warning disable CS1591
public static class ObservableSet {
#pragma warning restore CS1591
	/// <summary>
	/// Represents a set of events which may take place
	/// </summary>
	[Flags]
	public enum Event
	{
		/// <summary>
		/// When the set has been updated
		/// </summary>
		OnUpdated = 1 << 0,
		/// <summary>
		/// When the set has been updated by <see cref="ISet{T}.Add"/>
		/// </summary>
		OnAdded = 1 << 1,
		/// <summary>
		/// When the set has been updated by <see cref="ISet{T}.Remove"/>
		/// </summary>
		OnRemoved = 1 << 2,
		/// <summary>
		/// When the set is being operated by <see cref="ISet{T}.ExceptWith"/>
		/// </summary>
		OnExceptWith = 1 << 3,
		/// <summary>
		/// When the set is being operated by <see cref="ISet{T}.IntersectWith"/>
		/// </summary>
		OnIntersectWith = 1 << 4,
		/// <summary>
		/// When the set is being operated by <see cref="ISet{T}.UnionWith"/>
		/// </summary>
		OnUnionWith = 1 << 5,
		/// <summary>
		/// When the set is being operated by <see cref="ISet{T}.SymmetricExceptWith"/>
		/// </summary>
		OnSymmetricExceptWith = 1 << 6,
		/// <summary>
		/// When the set has been updated by <see cref="ISet{T}.Clear"/>
		/// </summary>
		OnCleared = 1 << 7
	}
}
