using UnityEngine;

namespace Mimicraft.VoxelEditor.Core
{
	public class ToggleObjectActiveCommand : IUndoableCommand
	{
		private readonly GameObject target;

		private readonly bool activeAfterRedo;

		public string Description { get; }

		public int RetainedCells => 0;

		public ToggleObjectActiveCommand(GameObject target, bool activeAfterRedo, string description)
		{
			this.target = target;
			this.activeAfterRedo = activeAfterRedo;
			Description = description;
		}

		public void Undo()
		{
			target.SetActive(!activeAfterRedo);
		}

		public void Redo()
		{
			target.SetActive(activeAfterRedo);
		}
	}
}
