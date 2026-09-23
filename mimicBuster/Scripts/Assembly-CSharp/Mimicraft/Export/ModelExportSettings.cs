using System;

namespace Mimicraft.Export
{
	[Serializable]
	public class ModelExportSettings
	{
		public float scale = 1f;

		public int pixelsPerVoxel = 1;

		public int padding = 1;

		public bool powerOfTwo = true;

		public bool mergePieces;

		public bool embedTexture;

		public ExportPivot pivot;
	}
}
