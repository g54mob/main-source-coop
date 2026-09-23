using System;
using System.Collections.Generic;

namespace Mimicraft.VoxelEditor.Core
{
	public static class UndoManager
	{
		public enum EditStepKind
		{
			Push = 0,
			Undo = 1,
			Redo = 2
		}

		private const int MaxHistory = 100;

		private const int MaxRetainedCells = 400000;

		private static readonly List<IUndoableCommand> undoStack = new List<IUndoableCommand>();

		private static readonly List<IUndoableCommand> redoStack = new List<IUndoableCommand>();

		private static int retainedCells;

		public static bool CanUndo => undoStack.Count > 0;

		public static bool CanRedo => redoStack.Count > 0;

		public static IReadOnlyList<IUndoableCommand> UndoStack => undoStack;

		public static int RetainedCells => retainedCells;

		public static event Action<IUndoableCommand, EditStepKind> EditStepApplied;

		public static void Push(IUndoableCommand command)
		{
			foreach (IUndoableCommand item in redoStack)
			{
				retainedCells -= item.RetainedCells;
			}
			redoStack.Clear();
			undoStack.Add(command);
			retainedCells += command.RetainedCells;
			Trim();
			UndoManager.EditStepApplied?.Invoke(command, EditStepKind.Push);
		}

		private static void Trim()
		{
			while (undoStack.Count > 1 && (undoStack.Count > 100 || retainedCells > 400000))
			{
				retainedCells -= undoStack[0].RetainedCells;
				undoStack.RemoveAt(0);
			}
		}

		public static void Undo()
		{
			if (undoStack.Count != 0)
			{
				int index = undoStack.Count - 1;
				IUndoableCommand undoableCommand = undoStack[index];
				undoStack.RemoveAt(index);
				undoableCommand.Undo();
				redoStack.Add(undoableCommand);
				UndoManager.EditStepApplied?.Invoke(undoableCommand, EditStepKind.Undo);
			}
		}

		public static void Redo()
		{
			if (redoStack.Count != 0)
			{
				int index = redoStack.Count - 1;
				IUndoableCommand undoableCommand = redoStack[index];
				redoStack.RemoveAt(index);
				undoableCommand.Redo();
				undoStack.Add(undoableCommand);
				UndoManager.EditStepApplied?.Invoke(undoableCommand, EditStepKind.Redo);
			}
		}

		public static void Clear()
		{
			undoStack.Clear();
			redoStack.Clear();
			retainedCells = 0;
		}
	}
}
