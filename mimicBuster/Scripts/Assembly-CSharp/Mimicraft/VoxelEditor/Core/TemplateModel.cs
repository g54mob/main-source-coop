using System.Collections.Generic;

namespace Mimicraft.VoxelEditor.Core
{
	public class TemplateModel
	{
		public string Name = "";

		public string Category = "";

		public string Tag = "";

		public float VoxelSize = 1f;

		public readonly List<TemplatePiece> Pieces = new List<TemplatePiece>();
	}
}
