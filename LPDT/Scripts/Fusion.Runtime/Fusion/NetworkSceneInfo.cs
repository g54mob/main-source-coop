using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine.SceneManagement;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 52)]
	[NetworkStructWeaved(13)]
	public struct NetworkSceneInfo : INetworkStruct, IEquatable<NetworkSceneInfo>
	{
		public const int MaxScenes = 8;

		[FieldOffset(0)]
		private NetworkSceneInfoDefaultFlags _flags;

		[FieldOffset(4)]
		private SceneRef _scene0;

		[FieldOffset(8)]
		private SceneRef _scene1;

		[FieldOffset(12)]
		private SceneRef _scene2;

		[FieldOffset(16)]
		private SceneRef _scene3;

		[FieldOffset(20)]
		private SceneRef _scene4;

		[FieldOffset(24)]
		private SceneRef _scene5;

		[FieldOffset(28)]
		private SceneRef _scene6;

		[FieldOffset(32)]
		private SceneRef _scene7;

		[FieldOffset(36)]
		private NetworkLoadSceneParameters _sceneMeta0;

		[FieldOffset(38)]
		private NetworkLoadSceneParameters _sceneMeta1;

		[FieldOffset(40)]
		private NetworkLoadSceneParameters _sceneMeta2;

		[FieldOffset(42)]
		private NetworkLoadSceneParameters _sceneMeta3;

		[FieldOffset(44)]
		private NetworkLoadSceneParameters _sceneMeta4;

		[FieldOffset(46)]
		private NetworkLoadSceneParameters _sceneMeta5;

		[FieldOffset(48)]
		private NetworkLoadSceneParameters _sceneMeta6;

		[FieldOffset(50)]
		private NetworkLoadSceneParameters _sceneMeta7;

		public const int SIZE = 52;

		public const int WORD_COUNT = 13;

		internal const int BYTE_OF__FLAGS = 0;

		internal const int BYTE_COUNT_OF__FLAGS = 4;

		internal const int BYTE_OF__SCENE0 = 4;

		internal const int BYTE_COUNT_OF__SCENE0 = 4;

		internal const int BYTE_OF__SCENE1 = 8;

		internal const int BYTE_COUNT_OF__SCENE1 = 4;

		internal const int BYTE_OF__SCENE2 = 12;

		internal const int BYTE_COUNT_OF__SCENE2 = 4;

		internal const int BYTE_OF__SCENE3 = 16;

		internal const int BYTE_COUNT_OF__SCENE3 = 4;

		internal const int BYTE_OF__SCENE4 = 20;

		internal const int BYTE_COUNT_OF__SCENE4 = 4;

		internal const int BYTE_OF__SCENE5 = 24;

		internal const int BYTE_COUNT_OF__SCENE5 = 4;

		internal const int BYTE_OF__SCENE6 = 28;

		internal const int BYTE_COUNT_OF__SCENE6 = 4;

		internal const int BYTE_OF__SCENE7 = 32;

		internal const int BYTE_COUNT_OF__SCENE7 = 4;

		internal const int BYTE_OF__SCENE_META0 = 36;

		internal const int BYTE_COUNT_OF__SCENE_META0 = 2;

		internal const int BYTE_OF__SCENE_META1 = 38;

		internal const int BYTE_COUNT_OF__SCENE_META1 = 2;

		internal const int BYTE_OF__SCENE_META2 = 40;

		internal const int BYTE_COUNT_OF__SCENE_META2 = 2;

		internal const int BYTE_OF__SCENE_META3 = 42;

		internal const int BYTE_COUNT_OF__SCENE_META3 = 2;

		internal const int BYTE_OF__SCENE_META4 = 44;

		internal const int BYTE_COUNT_OF__SCENE_META4 = 2;

		internal const int BYTE_OF__SCENE_META5 = 46;

		internal const int BYTE_COUNT_OF__SCENE_META5 = 2;

		internal const int BYTE_OF__SCENE_META6 = 48;

		internal const int BYTE_COUNT_OF__SCENE_META6 = 2;

		internal const int BYTE_OF__SCENE_META7 = 50;

		internal const int BYTE_COUNT_OF__SCENE_META7 = 2;

		private const uint __STATIC_ASSERT_ENSURE_PERFECT_FIT = 1u;

		public readonly ReadOnlySpan<SceneRef> Scenes => MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(in _scene0), SceneCount);

		public readonly ReadOnlySpan<NetworkLoadSceneParameters> SceneParams => MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(in _sceneMeta0), SceneCount);

		public int SceneCount
		{
			readonly get
			{
				return (int)(_flags & NetworkSceneInfoDefaultFlags.SceneCountMask);
			}
			private set
			{
				_flags = (NetworkSceneInfoDefaultFlags)(((uint)_flags & 0xFFFFFFF0u) | (uint)(value & 0xF));
			}
		}

		public int Version
		{
			readonly get
			{
				return (int)(_flags & NetworkSceneInfoDefaultFlags.ConterMask) >> 4;
			}
			private set
			{
				_flags = (NetworkSceneInfoDefaultFlags)(((uint)_flags & 0xFFF0000Fu) | (uint)((value << 4) & 0xFFFF0));
			}
		}

		public readonly int IndexOf(SceneRef sceneRef, NetworkLoadSceneParameters sceneParams)
		{
			for (int i = 0; i < SceneCount; i++)
			{
				if (Scenes[i] == sceneRef && SceneParams[i] == sceneParams)
				{
					return i;
				}
			}
			return -1;
		}

		public readonly int IndexOf((SceneRef SceneRef, NetworkLoadSceneParameters SceneParams) scene)
		{
			return IndexOf(scene.SceneRef, scene.SceneParams);
		}

		public int AddSceneRef(SceneRef sceneRef, LoadSceneMode loadSceneMode = LoadSceneMode.Single, LocalPhysicsMode localPhysicsMode = LocalPhysicsMode.None, bool activeOnLoad = false)
		{
			return AddSceneRef(sceneRef, (NetworkLoadSceneParametersFlags)(((loadSceneMode == LoadSceneMode.Single) ? 1u : 0u) | (uint)(((localPhysicsMode & LocalPhysicsMode.Physics2D) != LocalPhysicsMode.None) ? 2 : 0) | (uint)(((localPhysicsMode & LocalPhysicsMode.Physics3D) != LocalPhysicsMode.None) ? 4 : 0) | (uint)(activeOnLoad ? 8 : 0)));
		}

		internal int AddSceneRef(SceneRef sceneRef, NetworkLoadSceneParametersFlags flags)
		{
			if (flags.Has(NetworkLoadSceneParametersFlags.Single))
			{
				Span<SceneRef> span = MemoryMarshal.CreateSpan(ref _scene0, SceneCount);
				Span<NetworkLoadSceneParameters> span2 = MemoryMarshal.CreateSpan(ref _sceneMeta0, SceneCount);
				span.Clear();
				span2.Clear();
				SceneCount = 0;
			}
			if (SceneCount >= 8)
			{
				return -1;
			}
			int num = SceneCount++;
			Unsafe.Add(ref _sceneMeta0, num) = new NetworkLoadSceneParameters(new NetworkSceneLoadId((byte)Version), flags);
			Unsafe.Add(ref _scene0, num) = sceneRef;
			int version = Version + 1;
			Version = version;
			return num;
		}

		public bool RemoveSceneRef(SceneRef sceneRef)
		{
			Span<SceneRef> span = MemoryMarshal.CreateSpan(ref _scene0, SceneCount);
			Span<NetworkLoadSceneParameters> span2 = MemoryMarshal.CreateSpan(ref _sceneMeta0, SceneCount);
			for (int i = 0; i < SceneCount; i++)
			{
				if (Scenes[i] == sceneRef)
				{
					for (int j = i + 1; j < SceneCount; j++)
					{
						span[j - 1] = Scenes[j];
						span2[j - 1] = SceneParams[j];
					}
					span[SceneCount - 1] = default(SceneRef);
					span2[SceneCount - 1] = default(NetworkLoadSceneParameters);
					int version = Version + 1;
					Version = version;
					version = SceneCount - 1;
					SceneCount = version;
					return true;
				}
			}
			return false;
		}

		public override readonly string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("[Scenes: ");
			for (int i = 0; i < SceneCount; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append("[");
				stringBuilder.Append(Scenes[i].ToString(brackets: false, prefix: false));
				stringBuilder.Append(", Id: ");
				stringBuilder.Append(SceneParams[i].LoadId);
				if (SceneParams[i].Flags != 0)
				{
					stringBuilder.Append(", ");
					stringBuilder.Append(SceneParams[i].Flags);
				}
				stringBuilder.Append("]");
			}
			stringBuilder.Append("]");
			return stringBuilder.ToString();
		}

		public unsafe readonly bool Equals(NetworkSceneInfo other)
		{
			fixed (NetworkSceneInfo* ptr = &this)
			{
				return FusionUnsafe.Compare(ptr, &other, 52) == 0;
			}
		}

		public override readonly bool Equals(object obj)
		{
			return obj is NetworkSceneInfo other && Equals(other);
		}

		public unsafe override readonly int GetHashCode()
		{
			fixed (NetworkSceneInfo* data = &this)
			{
				return HashCodeUtilities.GetHashCodeDeterministic(data);
			}
		}

		public static implicit operator NetworkSceneInfo(SceneRef sceneRef)
		{
			NetworkSceneInfo result = default(NetworkSceneInfo);
			result.AddSceneRef(sceneRef);
			return result;
		}
	}
}
