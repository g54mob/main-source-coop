namespace Mimicraft.VoxelEditor.Core
{
	public interface IUndoableCommand
	{
		string Description { get; }

		int RetainedCells { get; }

		void Undo();

		void Redo();
	}
}
