using System;
using Mimicraft.VoxelEditor.Core;
using UnityEngine;

namespace Mimicraft.UI
{
	public static class ModelEditSession
	{
		private static bool dirty;

		private static bool subscribed;

		public static IModelEditSession Active { get; private set; }

		public static bool HasUnsavedChanges
		{
			get
			{
				if (Active != null)
				{
					return dirty;
				}
				return false;
			}
		}

		public static event Action DirtyChanged;

		public static void Begin(IModelEditSession session)
		{
			Active = session;
			SetDirty(value: false);
			Subscribe();
		}

		public static void End(IModelEditSession session)
		{
			if (Active == session)
			{
				Active = null;
				Unsubscribe();
				SetDirty(value: false);
			}
		}

		public static void MarkSaved()
		{
			SetDirty(value: false);
		}

		public static void MarkChanged()
		{
			SetDirty(value: true);
		}

		private static void SetDirty(bool value)
		{
			if (dirty != value)
			{
				dirty = value;
				ModelEditSession.DirtyChanged?.Invoke();
			}
		}

		private static void Subscribe()
		{
			if (!subscribed)
			{
				subscribed = true;
				UndoManager.EditStepApplied += OnEditStep;
			}
		}

		private static void Unsubscribe()
		{
			if (subscribed)
			{
				subscribed = false;
				UndoManager.EditStepApplied -= OnEditStep;
			}
		}

		private static void OnEditStep(IUndoableCommand command, UndoManager.EditStepKind kind)
		{
			MarkChanged();
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetOnPlay()
		{
			Active = null;
			dirty = false;
			subscribed = false;
			ModelEditSession.DirtyChanged = null;
		}
	}
}
