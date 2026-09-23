namespace Mimicraft.VoxelEditor.Core
{
	public readonly struct TemplateListEntry
	{
		public readonly string FilePath;

		public readonly string ModelName;

		public readonly string Category;

		public readonly string Tag;

		public readonly int PieceCount;

		public TemplateListEntry(string filePath, string modelName, string category, string tag, int pieceCount = 1)
		{
			FilePath = filePath;
			ModelName = modelName;
			Category = category ?? "";
			Tag = tag ?? "";
			PieceCount = pieceCount;
		}
	}
}
