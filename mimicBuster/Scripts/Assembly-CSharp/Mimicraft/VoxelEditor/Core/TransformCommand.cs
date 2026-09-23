using UnityEngine;

namespace Mimicraft.VoxelEditor.Core
{
	public class TransformCommand : IUndoableCommand
	{
		private readonly Transform target;

		private readonly Vector3 beforePosition;

		private readonly Quaternion beforeRotation;

		private readonly Vector3 afterPosition;

		private readonly Quaternion afterRotation;

		public string Description { get; }

		public int RetainedCells => 0;

		public TransformCommand(Transform target, Vector3 beforePosition, Quaternion beforeRotation, Vector3 afterPosition, Quaternion afterRotation, string description)
		{
			this.target = target;
			this.beforePosition = beforePosition;
			this.beforeRotation = beforeRotation;
			this.afterPosition = afterPosition;
			this.afterRotation = afterRotation;
			Description = description;
		}

		public void Undo()
		{
			target.position = beforePosition;
			target.rotation = beforeRotation;
		}

		public void Redo()
		{
			target.position = afterPosition;
			target.rotation = afterRotation;
		}
	}
}
