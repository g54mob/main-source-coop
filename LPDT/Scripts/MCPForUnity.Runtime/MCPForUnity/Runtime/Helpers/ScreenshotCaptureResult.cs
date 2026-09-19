namespace MCPForUnity.Runtime.Helpers
{
	public readonly struct ScreenshotCaptureResult
	{
		public string FullPath { get; }

		public string ProjectRelativePath { get; }

		public int SuperSize { get; }

		public bool IsAsync { get; }

		public string ImageBase64 { get; }

		public int ImageWidth { get; }

		public int ImageHeight { get; }

		public ScreenshotCaptureResult(string fullPath, string projectRelativePath, int superSize)
			: this(fullPath, projectRelativePath, superSize, isAsync: false, null, 0, 0)
		{
		}

		public ScreenshotCaptureResult(string fullPath, string projectRelativePath, int superSize, bool isAsync)
			: this(fullPath, projectRelativePath, superSize, isAsync, null, 0, 0)
		{
		}

		public ScreenshotCaptureResult(string fullPath, string projectRelativePath, int superSize, bool isAsync, string imageBase64, int imageWidth, int imageHeight)
		{
			FullPath = fullPath;
			ProjectRelativePath = projectRelativePath;
			SuperSize = superSize;
			IsAsync = isAsync;
			ImageBase64 = imageBase64;
			ImageWidth = imageWidth;
			ImageHeight = imageHeight;
		}
	}
}
