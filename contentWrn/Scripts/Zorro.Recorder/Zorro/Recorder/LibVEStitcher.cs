using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using PlayEveryWare.VideoEncoding;
using UnityEngine;

namespace Zorro.Recorder
{
	public class LibVEStitcher
	{
		public static IEnumerator StitchVideosCoroutine(string[] videos, string outputDirectory, Action<bool> callback)
		{
			Task<bool> stitchingTask = Task.Run(() => StitchVideos(videos, outputDirectory));
			yield return new WaitUntil(() => stitchingTask.IsCompleted);
			Debug.Log("Finished stitching, invoking callback");
			callback(stitchingTask.Result);
		}

		public static bool StitchVideos(string[] videos, string outputDirectory)
		{
			string path = Path.Combine(outputDirectory, "videoList.txt");
			string outputFilename = Path.Combine(outputDirectory, "fullRecording.webm");
			string text = "";
			foreach (string text2 in videos)
			{
				if (text2.Contains('\''))
				{
					Debug.LogWarning("Video path contains single quote character \"'\", will attempt to escape it: " + text2);
				}
				text = text + "file '" + text2.Replace("'", "'\\''") + "'\n";
				if (!File.Exists(text2))
				{
					Debug.LogError("Video File Not Found: " + text2);
				}
			}
			File.WriteAllText(path, text);
			Debug.Log("[LibVeStitcher] Initializing LibVE, encoder, and decoder");
			if (!LibVEInitializer.InitializeLibVE())
			{
				throw new Exception("[LibVeStitcher] Failed to initialize video encoding, won't be able to record or extract clips");
			}
			IntPtr handle = IntPtr.Zero;
			ve_config config = LibVEInitializer.GetNewVeConfig(outputFilename);
			if (!NativeVeMethods.initialize_encoding(ref config, out handle))
			{
				Debug.LogError("[LibVEStitcher] Failed to initialize video encoding.");
				return false;
			}
			if (!NativeVeMethods.initialize_decoding(ref config, handle))
			{
				Debug.LogError("[LibVeStitcher] Failed to initialize video decoding");
				return false;
			}
			if (handle == IntPtr.Zero)
			{
				Debug.LogError("[LibVeStitcher] Failed to get instance handle");
				return false;
			}
			Debug.Log("[LibVeStitcher] Stitching videos together: " + text);
			IntPtr[] array = new IntPtr[videos.Length];
			for (int j = 0; j < videos.Length; j++)
			{
				array[j] = Marshal.StringToHGlobalUni(videos[j]);
			}
			GCHandle gCHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
			IntPtr array2 = gCHandle.AddrOfPinnedObject();
			if (!NativeVeMethods.combine_webm_files(handle, array2, videos.Length))
			{
				Debug.LogError("[LibVeStitcher] Failed to combine files");
			}
			else
			{
				Debug.Log("[LibVeStitcher] Done with combining files for fullRecording.webm");
			}
			for (int k = 0; k < videos.Length; k++)
			{
				Marshal.FreeHGlobal(array[k]);
				array[k] = IntPtr.Zero;
			}
			gCHandle.Free();
			array2 = IntPtr.Zero;
			Console.WriteLine("[LibVeStitcher] Finalizing on fullRecording.webm");
			if (!NativeVeMethods.finalize(handle))
			{
				Debug.LogError("[LibVeStitcher] Failed to finalize fullRecording.webm");
				return false;
			}
			Debug.Log("[LibVeStitcher] Successfully finalized fullRecording.webm");
			return true;
		}
	}
}
