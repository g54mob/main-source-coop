using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace Zorro.Recorder.ImageEncoding
{
	[BurstCompile]
	public struct EncodePngJob : IJob
	{
		[ReadOnly]
		[DeallocateOnJobCompletion]
		public NativeArray<byte> Input;

		public uint Width;

		public uint Height;

		public NativeList<byte> Output;

		public unsafe void Execute()
		{
			NativeArray<byte> nativeArray = ImageConversion.EncodeNativeArrayToPNG(Input, GraphicsFormat.R8G8B8A8_SRGB, Width, Height);
			Output.Resize(nativeArray.Length, NativeArrayOptions.UninitializedMemory);
			void* unsafeBufferPointerWithoutChecks = NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks(nativeArray);
			UnsafeUtility.MemCpy(NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<byte>(Output), unsafeBufferPointerWithoutChecks, nativeArray.Length * UnsafeUtility.SizeOf<byte>());
			nativeArray.Dispose();
		}
	}
}
