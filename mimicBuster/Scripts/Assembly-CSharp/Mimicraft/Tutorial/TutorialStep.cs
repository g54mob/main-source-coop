using System;
using Mimicraft.VoxelEditor;
using Mimicraft.VoxelEditor.Core;

namespace Mimicraft.Tutorial
{
	public class TutorialStep
	{
		public string TitleKey;

		public string BodyKey;

		public string HintKey;

		public string ClipName;

		public EditorState? Highlight;

		public EditorState[] Unlocks;

		public bool UnlocksExtras;

		public Func<bool> IsComplete;

		public Func<VoxelGrid> Target;

		public Func<(int done, int total)> Progress;

		public Action DoIt;

		public bool DemoByUndo = true;

		public Action OnEnter;

		public Action OnExit;

		public bool NeedsManualAdvance => IsComplete == null;
	}
}
