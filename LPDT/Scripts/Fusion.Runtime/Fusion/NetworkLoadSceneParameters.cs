using System;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 2)]
	public readonly struct NetworkLoadSceneParameters : IEquatable<NetworkLoadSceneParameters>
	{
		[FieldOffset(0)]
		public readonly NetworkSceneLoadId LoadId;

		[FieldOffset(1)]
		private readonly NetworkLoadSceneParametersFlags _flags;

		public const int SIZE = 2;

		public const int WORD_COUNT = 1;

		internal const int BYTE_OF_LOAD_ID = 0;

		internal const int BYTE_COUNT_OF_LOAD_ID = 1;

		internal const int BYTE_OF__FLAGS = 1;

		internal const int BYTE_COUNT_OF__FLAGS = 1;

		private const uint __STATIC_ASSERT_ENSURE_PERFECT_FIT = 1u;

		public LoadSceneMode LoadSceneMode => (!_flags.Has(NetworkLoadSceneParametersFlags.Single)) ? LoadSceneMode.Additive : LoadSceneMode.Single;

		public LocalPhysicsMode LocalPhysicsMode => (LocalPhysicsMode)((_flags.Has(NetworkLoadSceneParametersFlags.LocalPhysics3D) ? 2 : 0) | (_flags.Has(NetworkLoadSceneParametersFlags.LocalPhysics2D) ? 1 : 0));

		public LoadSceneParameters LoadSceneParameters => new LoadSceneParameters(LoadSceneMode, LocalPhysicsMode);

		public bool IsActiveOnLoad => _flags.Has(NetworkLoadSceneParametersFlags.ActiveOnLoad);

		public bool IsSingleLoad => _flags.Has(NetworkLoadSceneParametersFlags.Single);

		public bool IsLocalPhysics2D => _flags.Has(NetworkLoadSceneParametersFlags.LocalPhysics2D);

		public bool IsLocalPhysics3D => _flags.Has(NetworkLoadSceneParametersFlags.LocalPhysics3D);

		internal NetworkLoadSceneParametersFlags Flags => _flags;

		internal NetworkLoadSceneParameters(NetworkSceneLoadId loadId, NetworkLoadSceneParametersFlags flags)
		{
			LoadId = loadId;
			_flags = flags;
		}

		public bool Equals(NetworkLoadSceneParameters other)
		{
			return _flags == other._flags && LoadId.Equals(other.LoadId);
		}

		public override bool Equals(object obj)
		{
			return obj is NetworkLoadSceneParameters other && Equals(other);
		}

		public override int GetHashCode()
		{
			return ((int)_flags * 397) ^ LoadId.GetHashCode();
		}

		public static bool operator ==(NetworkLoadSceneParameters left, NetworkLoadSceneParameters right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(NetworkLoadSceneParameters left, NetworkLoadSceneParameters right)
		{
			return !left.Equals(right);
		}

		public override string ToString()
		{
			return $"[Flags: {_flags}, LoadId: {LoadId}]";
		}
	}
}
