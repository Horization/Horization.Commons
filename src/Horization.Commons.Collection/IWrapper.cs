namespace Horization.Commons.Collection;

/// <summary>
/// Represents a wrapper for a wrapped value
/// </summary>
/// <typeparam name="TSelf">The type of the wrapper</typeparam>
/// <typeparam name="TWrapped">The type of the wrapped content</typeparam>
public interface IWrapper<in TSelf, out TWrapped> where TSelf : IWrapper<TSelf, TWrapped>
{
	/// <summary>
	/// The wrapped content
	/// </summary>
	protected TWrapped Wrapped { get; }

	/// <summary>
	/// Whether to public wrapped content
	/// </summary>
	public bool PublicWrapped { get; }

#if NET7_0_OR_GREATER
	/// <summary>
	/// Implicitly unwrap content
	/// </summary>
	/// <param name="this">The wrapper</param>
	/// <returns>The unwrapped content</returns>
	public static abstract implicit operator TWrapped(TSelf @this);
#endif

	/// <summary>
	/// Unwrap content
	/// </summary>
	/// <returns>The unwrapped content</returns>
	/// <exception cref="InvalidOperationException">The wrapped content is not public.</exception>
	public TWrapped Unwrap() =>
		PublicWrapped ? Wrapped : throw new InvalidOperationException("The wrapped content is not public.");
}
