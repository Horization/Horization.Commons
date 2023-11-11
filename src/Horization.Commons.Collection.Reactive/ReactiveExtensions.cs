using System.Reactive.Subjects;
using System.Runtime.CompilerServices;

namespace Horization.Commons.Collection.Reactive;

internal static class ReactiveExtensions
{
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void OnNextIfHavingObservers<T>(this SubjectBase<T> @this, in T value)
	{
		if (@this.HasObservers) @this.OnNext(value);
	}
}
