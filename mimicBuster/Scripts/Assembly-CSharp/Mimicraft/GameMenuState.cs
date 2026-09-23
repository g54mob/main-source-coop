using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mimicraft
{
	public static class GameMenuState
	{
		public static class EscapeDepth
		{
			public const int EditorTool = 10;

			public const int Screen = 20;

			public const int PauseMenu = 30;

			public const int Chat = 40;

			public const int Settings = 60;

			public const int Popup = 100;
		}

		private static bool devConsoleOpen;

		private static bool devPointerOpen;

		private static int escapeConsumedFrame = -1;

		private static int escapeDepth = int.MinValue;

		private static Action escapeAction;

		private static int escapeRequestFrame = -1;

		private static readonly HashSet<UnityEngine.Object> menuOwners = new HashSet<UnityEngine.Object>();

		private static readonly HashSet<UnityEngine.Object> pointerOwners = new HashSet<UnityEngine.Object>();

		private static readonly HashSet<UnityEngine.Object> overlayOwners = new HashSet<UnityEngine.Object>();

		public static bool IsMenuOpen
		{
			get
			{
				if (!AnyMenuOwner && !devConsoleOpen)
				{
					return devPointerOpen;
				}
				return true;
			}
		}

		public static bool EscapeConsumedThisFrame => escapeConsumedFrame == Time.frameCount;

		private static bool AnyMenuOwner
		{
			get
			{
				Prune();
				return menuOwners.Count > 0;
			}
		}

		public static bool IsDevConsoleOpen => devConsoleOpen;

		public static bool IsDevPointerOpen => devPointerOpen;

		public static bool PointerCaptured
		{
			get
			{
				PrunePointers();
				return pointerOwners.Count > 0;
			}
		}

		public static bool OverlayOpen
		{
			get
			{
				PruneOverlays();
				return overlayOwners.Count > 0;
			}
		}

		public static bool InputCaptured
		{
			get
			{
				if (!IsMenuOpen)
				{
					return PointerCaptured;
				}
				return true;
			}
		}

		public static bool LookCaptured
		{
			get
			{
				if (!InputCaptured)
				{
					return OverlayOpen;
				}
				return true;
			}
		}

		public static void RequestEscape(int depth, Action close)
		{
			if (close != null)
			{
				if (escapeRequestFrame != Time.frameCount)
				{
					escapeRequestFrame = Time.frameCount;
					escapeDepth = int.MinValue;
					escapeAction = null;
				}
				if (depth > escapeDepth)
				{
					escapeDepth = depth;
					escapeAction = close;
				}
			}
		}

		public static void ResolveEscape()
		{
			if (escapeRequestFrame == Time.frameCount && escapeAction != null)
			{
				Action action = escapeAction;
				escapeAction = null;
				escapeDepth = int.MinValue;
				ConsumeEscape();
				action();
			}
		}

		public static void SetMenuOpen(UnityEngine.Object owner, bool open)
		{
			if (!(owner == null))
			{
				if (open)
				{
					menuOwners.Add(owner);
				}
				else
				{
					menuOwners.Remove(owner);
				}
				Prune();
			}
		}

		private static void Prune()
		{
			menuOwners.RemoveWhere((UnityEngine.Object owner) => owner == null);
		}

		public static void ClearMenuOwners()
		{
			menuOwners.Clear();
			pointerOwners.Clear();
			overlayOwners.Clear();
		}

		public static void SetDevConsoleOpen(bool open)
		{
			devConsoleOpen = open;
		}

		public static void SetDevPointerOpen(bool open)
		{
			devPointerOpen = open;
		}

		public static void SetPointerCaptured(UnityEngine.Object owner, bool captured)
		{
			if (!(owner == null))
			{
				if (captured)
				{
					pointerOwners.Add(owner);
				}
				else
				{
					pointerOwners.Remove(owner);
				}
				PrunePointers();
			}
		}

		private static void PrunePointers()
		{
			pointerOwners.RemoveWhere((UnityEngine.Object owner) => owner == null);
		}

		public static void SetOverlayOpen(UnityEngine.Object owner, bool open)
		{
			if (!(owner == null))
			{
				if (open)
				{
					overlayOwners.Add(owner);
				}
				else
				{
					overlayOwners.Remove(owner);
				}
				PruneOverlays();
			}
		}

		private static void PruneOverlays()
		{
			overlayOwners.RemoveWhere((UnityEngine.Object owner) => owner == null);
		}

		public static void ConsumeEscape()
		{
			escapeConsumedFrame = Time.frameCount;
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetOnPlay()
		{
			menuOwners.Clear();
			pointerOwners.Clear();
			overlayOwners.Clear();
			devConsoleOpen = false;
			devPointerOpen = false;
			escapeConsumedFrame = -1;
		}
	}
}
