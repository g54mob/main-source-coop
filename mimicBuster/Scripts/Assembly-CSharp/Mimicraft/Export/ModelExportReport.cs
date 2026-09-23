namespace Mimicraft.Export
{
	public readonly struct ModelExportReport
	{
		public readonly string FbxPath;

		public readonly int Quads;

		public readonly int Objects;

		public readonly int TextureWidth;

		public readonly int TextureHeight;

		public ModelExportReport(string fbxPath, int quads, int objects, int textureWidth, int textureHeight)
		{
			FbxPath = fbxPath;
			Quads = quads;
			Objects = objects;
			TextureWidth = textureWidth;
			TextureHeight = textureHeight;
		}
	}
}
