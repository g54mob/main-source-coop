using System;
using System.Collections.Generic;
using System.IO;
using Unity.Collections;
using UnityEngine;

namespace Zorro.Recorder
{
	public class RecordingPipe : IDisposable
	{
		private int m_width;

		private int m_height;

		private static string tempPath;

		private string path;

		private Dictionary<string, DirectoryInfo> directories = new Dictionary<string, DirectoryInfo>();

		private static string ErrorTitle;

		private static string ErrorBody;

		public int Width => m_width;

		public int Height => m_height;

		public DirectoryInfo GetDirectory()
		{
			if (!directories.ContainsKey(path))
			{
				directories[path] = new DirectoryInfo(path);
			}
			return directories[path];
		}

		public RecordingPipe(int width, int height, string directory)
		{
			path = directory;
			m_width = width;
			m_height = height;
			directories.Clear();
			DirectoryInfo directory2 = GetDirectory();
			Debug.Log("Recording pipe is using directory: " + directory2.FullName);
			if (directory2.Exists)
			{
				directory2.Delete(recursive: true);
			}
			directory2.Create();
		}

		public void Dispose()
		{
		}

		public void PushFrameData(EncodingStream stream, GpuDataRequest req, List<NativeArray<byte>> results)
		{
			DirectoryInfo directory = GetDirectory();
			if (!directory.Exists)
			{
				directory.Create();
			}
			if (stream.QueueVideoData(results, req.isKeyFrame))
			{
				return;
			}
			Debug.LogWarning("Could not queue additional frame as stream is being finlized. Disposing now.");
			foreach (NativeArray<byte> result in results)
			{
				result.Dispose();
			}
		}

		public static bool CheckErrorPopup(out string title, out string body)
		{
			title = ErrorTitle;
			body = ErrorBody;
			ErrorTitle = null;
			ErrorBody = null;
			return title != null;
		}
	}
}
