using System.Collections;
using System.Runtime.CompilerServices;

namespace Horization.Commons.Collection;

/// <summary>
/// Represents a list which can be inherited
/// </summary>
/// <typeparam name="T">The type of elements in the list</typeparam>
public class VirtualList<T> : IList<T>
{
	/// <summary>
	/// The actual implementation of <see cref="IList{T}"/>
	/// </summary>
	protected readonly IList<T> ListImpl;

	/// <summary>
	/// Construct a new instance
	/// </summary>
	/// <param name="listImpl">The actual implementation which would be used</param>
	public VirtualList(IList<T> listImpl) => ListImpl = listImpl;

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public virtual IEnumerator<T> GetEnumerator() => ListImpl.GetEnumerator();

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)ListImpl).GetEnumerator();

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public virtual void Add(T item) => ListImpl.Add(item);

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public virtual void Clear() => ListImpl.Clear();

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public virtual bool Contains(T item) => ListImpl.Contains(item);

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public virtual void CopyTo(T[] array, int arrayIndex) => ListImpl.CopyTo(array, arrayIndex);

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public virtual bool Remove(T item) => ListImpl.Remove(item);

	/// <inheritdoc />
	public virtual int Count => ListImpl.Count;

	/// <inheritdoc />
	public virtual bool IsReadOnly => ListImpl.IsReadOnly;

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public virtual int IndexOf(T item) => ListImpl.IndexOf(item);

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public virtual void Insert(int index, T item) => ListImpl.Insert(index, item);

	/// <inheritdoc />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public virtual void RemoveAt(int index) => ListImpl.RemoveAt(index);

	/// <inheritdoc />
	public virtual T this[int index]
	{
		get => ListImpl[index];
		set => ListImpl[index] = value;
	}
}
