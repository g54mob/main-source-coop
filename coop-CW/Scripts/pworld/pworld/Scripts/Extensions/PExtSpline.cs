using Unity.Collections;
using UnityEngine.Splines;

namespace pworld.Scripts.Extensions
{
	public static class PExtSpline
	{
		public static NativeSpline PGetNative(this Spline me, Allocator allocator = Allocator.TempJob)
		{
			return new NativeSpline(me, allocator);
		}
	}
}
