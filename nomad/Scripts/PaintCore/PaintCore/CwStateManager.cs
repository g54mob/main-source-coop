using System;

namespace PaintCore
{
	public static class CwStateManager
	{
		private static bool allStatesStored;

		private static bool potentiallyStoreStates;

		public static bool CanUndo
		{
			get
			{
				foreach (CwPaintableTexture instance in CwPaintableTexture.Instances)
				{
					if (instance.CanUndo)
					{
						return true;
					}
				}
				return false;
			}
		}

		public static bool CanRedo
		{
			get
			{
				foreach (CwPaintableTexture instance in CwPaintableTexture.Instances)
				{
					if (instance.CanRedo)
					{
						return true;
					}
				}
				return false;
			}
		}

		public static bool PotentiallyStoreStates
		{
			get
			{
				return potentiallyStoreStates;
			}
			set
			{
				potentiallyStoreStates = value;
			}
		}

		public static bool AllStatesStored
		{
			get
			{
				return allStatesStored;
			}
			set
			{
				allStatesStored = value;
			}
		}

		public static event Action OnPreUndoAll;

		public static event Action OnPreRedoAll;

		public static void StoreAllStates()
		{
			allStatesStored = true;
			potentiallyStoreStates = false;
			foreach (CwPaintableTexture instance in CwPaintableTexture.Instances)
			{
				instance.StoreState();
			}
		}

		public static void PotentiallyStoreAllStates()
		{
			potentiallyStoreStates = true;
		}

		public static void ClearAllStates()
		{
			foreach (CwPaintableTexture instance in CwPaintableTexture.Instances)
			{
				instance.ClearStates();
			}
		}

		public static void UndoAll()
		{
			if (CwStateManager.OnPreUndoAll != null)
			{
				CwStateManager.OnPreUndoAll();
			}
			foreach (CwPaintableTexture instance in CwPaintableTexture.Instances)
			{
				instance.Undo();
			}
		}

		public static void RedoAll()
		{
			if (CwStateManager.OnPreRedoAll != null)
			{
				CwStateManager.OnPreRedoAll();
			}
			foreach (CwPaintableTexture instance in CwPaintableTexture.Instances)
			{
				instance.Redo();
			}
		}
	}
}
