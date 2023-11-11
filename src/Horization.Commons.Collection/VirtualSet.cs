using System.Collections;
using System.Runtime.CompilerServices;

namespace Horization.Commons.Collection;

/// <summary>
/// Represents a set which can be inherited
/// </summary>
/// <typeparam name="T">The type of elements in the set</typeparam>
public class VirtualSet<T> : ISet<T>
{
	/// <summary>
	/// The actual implementation of <see cref="ISet{T}"/>
	/// </summary>
	protected readonly ISet<T> SetImpl;

	/// <summary>
	/// Construct a new instance
	/// </summary>
	/// <param name="setImpl">The actual implementation which would be used</param>
	public VirtualSet(ISet<T> setImpl) => SetImpl = setImpl;

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public virtual IEnumerator<T> GetEnumerator() => SetImpl.GetEnumerator();

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)SetImpl).GetEnumerator();

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	void ICollection<T>.Add(T item) => SetImpl.Add(item);

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public virtual void ExceptWith(IEnumerable<T> other) => SetImpl.ExceptWith(other);

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public virtual void IntersectWith(IEnumerable<T> other) => SetImpl.IntersectWith(other);

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public virtual bool IsProperSubsetOf(IEnumerable<T> other) => SetImpl.IsProperSubsetOf(other);

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public virtual bool IsProperSupersetOf(IEnumerable<T> other) => SetImpl.IsProperSupersetOf(other);

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public virtual bool IsSubsetOf(IEnumerable<T> other) => SetImpl.IsSubsetOf(other);

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public virtual bool IsSupersetOf(IEnumerable<T> other) => SetImpl.IsSupersetOf(other);

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public virtual bool Overlaps(IEnumerable<T> other) => SetImpl.Overlaps(other);

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public virtual bool SetEquals(IEnumerable<T> other) => SetImpl.SetEquals(other);

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public virtual void SymmetricExceptWith(IEnumerable<T> other) => SetImpl.SymmetricExceptWith(other);

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public virtual void UnionWith(IEnumerable<T> other) => SetImpl.UnionWith(other);

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public virtual bool Add(T item) => SetImpl.Add(item);

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public virtual void Clear() => SetImpl.Clear();

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public virtual bool Contains(T item) => SetImpl.Contains(item);

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public virtual void CopyTo(T[] array, int arrayIndex) => SetImpl.CopyTo(array, arrayIndex);

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public virtual bool Remove(T item) => SetImpl.Remove(item);

	/// <inheritdoc />
	public virtual int Count => SetImpl.Count;

	/// <inheritdoc />
	public virtual bool IsReadOnly => SetImpl.IsReadOnly;
}
