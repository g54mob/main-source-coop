using UnityEngine;

namespace FluffyUnderware.Curvy.ThirdParty.LibTessDotNet
{
	public static class LibTessVector3Extension
	{
		public static Vec3 Vec3(this Vector3 v)
		{
			return new Vec3
			{
				X = v.x,
				Y = v.y,
				Z = v.z
			};
		}

		public static ContourVertex ContourVertex(this Vector3 v)
		{
			return new ContourVertex
			{
				Position = v.Vec3()
			};
		}
	}
}
