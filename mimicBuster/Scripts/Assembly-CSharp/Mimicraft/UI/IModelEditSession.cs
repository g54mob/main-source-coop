using Mimicraft.VoxelEditor.Core;

namespace Mimicraft.UI
{
	public interface IModelEditSession
	{
		bool CanResetModel { get; }

		bool SupportsPrimitives { get; }

		void ResetModel();

		void SaveModel();

		void ApplyPrimitive(VoxelPrimitive shape);
	}
}
