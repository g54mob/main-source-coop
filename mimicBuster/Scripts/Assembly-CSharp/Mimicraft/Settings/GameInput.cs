using System;
using System.Collections.Generic;
using Mimicraft.Localization;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mimicraft.Settings
{
	public static class GameInput
	{
		[Serializable]
		private struct OverrideEntry
		{
			public string command;

			public string path;
		}

		[Serializable]
		private class OverrideSet
		{
			public List<OverrideEntry> entries = new List<OverrideEntry>();
		}

		[Serializable]
		private struct LegacyEntry
		{
			public string action;

			public string path;
		}

		[Serializable]
		private class LegacySet
		{
			public List<LegacyEntry> bindings;
		}

		public readonly struct Command
		{
			public readonly string Id;

			public readonly string LabelKey;

			public readonly InputAction Action;

			public readonly int BindingIndex;

			public string Label => Loc.Get(LabelKey);

			public string Group
			{
				get
				{
					if (Action != null)
					{
						return Action.actionMap.name;
					}
					return "";
				}
			}

			public string DisplayString
			{
				get
				{
					if (Action != null)
					{
						return Action.GetBindingDisplayString(BindingIndex, InputBinding.DisplayStringOptions.DontUseShortDisplayNames);
					}
					return "";
				}
			}

			public string EffectivePath
			{
				get
				{
					if (Action != null && BindingIndex >= 0 && BindingIndex < Action.bindings.Count)
					{
						return Action.bindings[BindingIndex].effectivePath;
					}
					return "";
				}
			}

			public Command(string id, string labelKey, InputAction action, int bindingIndex)
			{
				Id = id;
				LabelKey = labelKey;
				Action = action;
				BindingIndex = bindingIndex;
			}
		}

		public const string CommonMap = "Common";

		public const string MovementMap = "Movement";

		public const string HumanMap = "Human";

		public const string ModelerMap = "Modeler";

		public const string EditorMap = "Editor";

		private static InputActionAsset asset;

		private static InputAction scoreboard;

		private static InputAction chat;

		private static InputAction editorToggle;

		private static InputAction weaponSlot1;

		private static InputAction weaponSlot2;

		private static InputAction weaponSlot3;

		private static InputAction voteYes;

		private static InputAction voteNo;

		private static InputAction voiceAll;

		private static InputAction voiceTeam;

		private static InputAction move;

		private static InputAction run;

		private static InputAction jump;

		private static InputAction crouch;

		private static InputAction fire;

		private static InputAction scope;

		private static InputAction block;

		private static InputAction interact;

		private static InputAction companion;

		private static InputAction shoulderSwap;

		private static InputAction holster;

		private static InputAction weaponMenu;

		private static InputAction climb;

		private static InputAction taunt;

		private static InputAction viewMode;

		private static InputAction toolTransform;

		private static InputAction toolExtrude;

		private static InputAction toolPaint;

		private static InputAction toolLoopCut;

		private static InputAction toolView;

		private static InputAction toggleGrid;

		private static InputAction toggleDimensions;

		private static InputAction toggleMode;

		private static InputAction toggleSubMode;

		private static InputAction toggleExtraMode;

		private static InputAction toggleUnlit;

		private static InputAction toggleHistory;

		private static InputAction toggleLibrary;

		private static List<Command> commands;

		private static readonly string[][] Contexts = new string[3][]
		{
			new string[3] { "Common", "Movement", "Human" },
			new string[3] { "Common", "Movement", "Modeler" },
			new string[3] { "Common", "Modeler", "Editor" }
		};

		private static readonly (string First, string Second)[] Exclusive = new(string, string)[1] { ("Human/Scope", "Human/Block") };

		private static InputActionRebindingExtensions.RebindingOperation activeRebind;

		public static InputAction Scoreboard
		{
			get
			{
				Ensure();
				return scoreboard;
			}
		}

		public static InputAction Chat
		{
			get
			{
				Ensure();
				return chat;
			}
		}

		public static InputAction VoteYes
		{
			get
			{
				Ensure();
				return voteYes;
			}
		}

		public static InputAction VoteNo
		{
			get
			{
				Ensure();
				return voteNo;
			}
		}

		public static InputAction VoiceAll
		{
			get
			{
				Ensure();
				return voiceAll;
			}
		}

		public static InputAction VoiceTeam
		{
			get
			{
				Ensure();
				return voiceTeam;
			}
		}

		public static InputAction Move
		{
			get
			{
				Ensure();
				return move;
			}
		}

		public static InputAction Run
		{
			get
			{
				Ensure();
				return run;
			}
		}

		public static InputAction Jump
		{
			get
			{
				Ensure();
				return jump;
			}
		}

		public static InputAction Crouch
		{
			get
			{
				Ensure();
				return crouch;
			}
		}

		public static InputAction Fire
		{
			get
			{
				Ensure();
				return fire;
			}
		}

		public static InputAction Scope
		{
			get
			{
				Ensure();
				return scope;
			}
		}

		public static InputAction Block
		{
			get
			{
				Ensure();
				return block;
			}
		}

		public static InputAction ShoulderSwap
		{
			get
			{
				Ensure();
				return shoulderSwap;
			}
		}

		public static InputAction Interact
		{
			get
			{
				Ensure();
				return interact;
			}
		}

		public static InputAction Companion
		{
			get
			{
				Ensure();
				return companion;
			}
		}

		public static InputAction Holster
		{
			get
			{
				Ensure();
				return holster;
			}
		}

		public static InputAction WeaponMenu
		{
			get
			{
				Ensure();
				return weaponMenu;
			}
		}

		public static InputAction WeaponSlot1
		{
			get
			{
				Ensure();
				return weaponSlot1;
			}
		}

		public static InputAction WeaponSlot2
		{
			get
			{
				Ensure();
				return weaponSlot2;
			}
		}

		public static InputAction WeaponSlot3
		{
			get
			{
				Ensure();
				return weaponSlot3;
			}
		}

		public static InputAction Climb
		{
			get
			{
				Ensure();
				return climb;
			}
		}

		public static InputAction Taunt
		{
			get
			{
				Ensure();
				return taunt;
			}
		}

		public static InputAction ViewMode
		{
			get
			{
				Ensure();
				return viewMode;
			}
		}

		public static InputAction EditorToggle
		{
			get
			{
				Ensure();
				return editorToggle;
			}
		}

		public static InputAction ToolTransform
		{
			get
			{
				Ensure();
				return toolTransform;
			}
		}

		public static InputAction ToolExtrude
		{
			get
			{
				Ensure();
				return toolExtrude;
			}
		}

		public static InputAction ToolPaint
		{
			get
			{
				Ensure();
				return toolPaint;
			}
		}

		public static InputAction ToolLoopCut
		{
			get
			{
				Ensure();
				return toolLoopCut;
			}
		}

		public static InputAction ToolView
		{
			get
			{
				Ensure();
				return toolView;
			}
		}

		public static InputAction ToggleGrid
		{
			get
			{
				Ensure();
				return toggleGrid;
			}
		}

		public static InputAction ToggleDimensions
		{
			get
			{
				Ensure();
				return toggleDimensions;
			}
		}

		public static InputAction ToggleMode
		{
			get
			{
				Ensure();
				return toggleMode;
			}
		}

		public static InputAction ToggleSubMode
		{
			get
			{
				Ensure();
				return toggleSubMode;
			}
		}

		public static InputAction ToggleExtraMode
		{
			get
			{
				Ensure();
				return toggleExtraMode;
			}
		}

		public static InputAction ToggleUnlit
		{
			get
			{
				Ensure();
				return toggleUnlit;
			}
		}

		public static InputAction ToggleHistory
		{
			get
			{
				Ensure();
				return toggleHistory;
			}
		}

		public static InputAction ToggleLibrary
		{
			get
			{
				Ensure();
				return toggleLibrary;
			}
		}

		public static IReadOnlyList<Command> Commands
		{
			get
			{
				Ensure();
				return commands ?? (commands = BuildCommands());
			}
		}

		public static bool IsBuilt => asset != null;

		public static bool IsRebinding
		{
			get
			{
				if (activeRebind != null)
				{
					return activeRebind.started;
				}
				return false;
			}
		}

		public static event Action BindingsChanged;

		public static string GroupLabelKey(string group)
		{
			return group switch
			{
				"Common" => "Input.Group.Common", 
				"Movement" => "Input.Group.Movement", 
				"Human" => "Input.Group.Human", 
				"Modeler" => "Input.Group.Modeler", 
				"Editor" => "Input.Group.Editor", 
				_ => "", 
			};
		}

		private static void Ensure()
		{
			if (!(asset != null))
			{
				asset = ScriptableObject.CreateInstance<InputActionAsset>();
				asset.name = "MimicraftInput";
				InputActionMap map = asset.AddActionMap("Common");
				scoreboard = Button(map, "Scoreboard", "<Keyboard>/f3");
				voteYes = Button(map, "VoteYes", "<Keyboard>/f1");
				voteNo = Button(map, "VoteNo", "<Keyboard>/f2");
				chat = Button(map, "Chat", "<Keyboard>/enter", "<Keyboard>/numpadEnter");
				voiceAll = Button(map, "VoiceAll", "<Keyboard>/y", "<Mouse>/backButton");
				voiceTeam = Button(map, "VoiceTeam", "<Keyboard>/u", "<Mouse>/forwardButton");
				InputActionMap map2 = asset.AddActionMap("Movement");
				move = map2.AddAction("Move");
				move.AddCompositeBinding("2DVector").With("Up", "<Keyboard>/w").With("Down", "<Keyboard>/s")
					.With("Left", "<Keyboard>/a")
					.With("Right", "<Keyboard>/d");
				run = Button(map2, "Run", "<Keyboard>/leftShift", "<Keyboard>/rightShift");
				jump = Button(map2, "Jump", "<Keyboard>/space");
				InputActionMap map3 = asset.AddActionMap("Human");
				crouch = Button(map3, "Crouch", "<Keyboard>/leftCtrl", "<Keyboard>/c");
				fire = Button(map3, "Fire", "<Mouse>/leftButton");
				scope = Button(map3, "Scope", "<Mouse>/rightButton");
				block = Button(map3, "Block", "<Mouse>/rightButton");
				shoulderSwap = Button(map3, "ShoulderSwap", "<Keyboard>/tab");
				interact = Button(map3, "Interact", "<Keyboard>/f");
				companion = Button(map3, "Companion", "<Keyboard>/b");
				holster = Button(map3, "Holster", "<Keyboard>/h");
				weaponMenu = Button(map3, "WeaponMenu", "<Keyboard>/n");
				weaponSlot1 = Button(map3, "WeaponSlot1", "<Keyboard>/1", "<Keyboard>/numpad1");
				weaponSlot2 = Button(map3, "WeaponSlot2", "<Keyboard>/2", "<Keyboard>/numpad2");
				weaponSlot3 = Button(map3, "WeaponSlot3", "<Keyboard>/3", "<Keyboard>/numpad3");
				InputActionMap map4 = asset.AddActionMap("Modeler");
				climb = Button(map4, "Climb", "<Keyboard>/leftCtrl");
				taunt = Button(map4, "Taunt", "<Keyboard>/t");
				editorToggle = Button(map4, "EditorToggle", "<Keyboard>/tab");
				viewMode = Button(map4, "ViewMode", "<Keyboard>/z");
				InputActionMap map5 = asset.AddActionMap("Editor");
				toolTransform = Button(map5, "ToolTransform", "<Keyboard>/q");
				toolExtrude = Button(map5, "ToolExtrude", "<Keyboard>/w");
				toolPaint = Button(map5, "ToolPaint", "<Keyboard>/e");
				toolLoopCut = Button(map5, "ToolLoopCut", "<Keyboard>/r");
				toolView = Button(map5, "ToolView", "<Keyboard>/v");
				toggleGrid = Button(map5, "ToggleGrid", "<Keyboard>/g");
				toggleDimensions = Button(map5, "ToggleDimensions", "<Keyboard>/d");
				toggleMode = Button(map5, "ToggleMode", "<Keyboard>/x");
				toggleSubMode = Button(map5, "ToggleSubMode", "<Keyboard>/f");
				toggleExtraMode = Button(map5, "ToggleExtraMode", "<Keyboard>/c");
				toggleUnlit = Button(map5, "ToggleUnlit", "<Keyboard>/s");
				toggleHistory = Button(map5, "ToggleHistory", "<Keyboard>/h");
				toggleLibrary = Button(map5, "ToggleLibrary", "<Keyboard>/l");
				LoadOverrides();
				asset.Enable();
			}
		}

		private static InputAction Button(InputActionMap map, string name, params string[] paths)
		{
			InputAction inputAction = map.AddAction(name, InputActionType.Button);
			foreach (string path in paths)
			{
				inputAction.AddBinding(path);
			}
			return inputAction;
		}

		private static void LoadOverrides()
		{
			GameSettings.Load();
			string inputBindings = GameSettings.InputBindings;
			if (!string.IsNullOrEmpty(inputBindings))
			{
				OverrideSet overrideSet = null;
				try
				{
					overrideSet = JsonUtility.FromJson<OverrideSet>(inputBindings);
				}
				catch (Exception ex)
				{
					Debug.LogWarning("[Input] Kayitli tus atamalari okunamadi, varsayilanlara donuldu: " + ex.Message);
					GameSettings.SetInputBindings("");
					return;
				}
				if (overrideSet?.entries == null || overrideSet.entries.Count == 0)
				{
					MigrateLegacyOverrides(inputBindings);
				}
				else
				{
					Apply(overrideSet.entries);
				}
			}
		}

		private static void Apply(List<OverrideEntry> entries)
		{
			foreach (OverrideEntry entry in entries)
			{
				if (!string.IsNullOrEmpty(entry.path))
				{
					if (!TryFind(entry.command, out var command) || command.Action == null)
					{
						Debug.LogWarning("[Input] '" + entry.command + "' diye bir komut kalmamis - o tus atamasi varsayilanina dondu.");
					}
					else
					{
						command.Action.ApplyBindingOverride(command.BindingIndex, entry.path);
					}
				}
			}
		}

		private static void MigrateLegacyOverrides(string json)
		{
			LegacySet legacySet = null;
			try
			{
				legacySet = JsonUtility.FromJson<LegacySet>(json);
			}
			catch (Exception)
			{
			}
			List<OverrideEntry> list = new List<OverrideEntry>();
			if (legacySet?.bindings != null)
			{
				foreach (LegacyEntry binding in legacySet.bindings)
				{
					if (!string.IsNullOrEmpty(binding.action) && !string.IsNullOrEmpty(binding.path) && !(binding.path == "null"))
					{
						list.Add(new OverrideEntry
						{
							command = binding.action,
							path = binding.path
						});
					}
				}
			}
			if (list.Count == 0)
			{
				GameSettings.SetInputBindings("");
				return;
			}
			Apply(list);
			GameSettings.SetInputBindings(Serialize(list));
			Debug.Log($"[Input] {list.Count} eski tus atamasi yeni bicime tasindi.");
		}

		public static void Save()
		{
			Ensure();
			GameSettings.SetInputBindings(Serialize(CurrentOverrides()));
			GameInput.BindingsChanged?.Invoke();
		}

		private static List<OverrideEntry> CurrentOverrides()
		{
			List<OverrideEntry> list = new List<OverrideEntry>();
			foreach (Command command in Commands)
			{
				if (command.Action != null && command.BindingIndex >= 0 && command.BindingIndex < command.Action.bindings.Count)
				{
					string overridePath = command.Action.bindings[command.BindingIndex].overridePath;
					if (!string.IsNullOrEmpty(overridePath))
					{
						list.Add(new OverrideEntry
						{
							command = command.Id,
							path = overridePath
						});
					}
				}
			}
			return list;
		}

		private static string Serialize(List<OverrideEntry> entries)
		{
			if (entries.Count != 0)
			{
				return JsonUtility.ToJson(new OverrideSet
				{
					entries = entries
				});
			}
			return "";
		}

		public static void ResetBinding(Command command)
		{
			if (command.Action != null)
			{
				command.Action.RemoveBindingOverride(command.BindingIndex);
				Save();
			}
		}

		public static void ResetGroup(string group)
		{
			Ensure();
			foreach (Command command in Commands)
			{
				if (command.Group == group)
				{
					command.Action.RemoveBindingOverride(command.BindingIndex);
				}
			}
			Save();
		}

		public static void ResetAll()
		{
			Ensure();
			foreach (Command command in Commands)
			{
				command.Action.RemoveBindingOverride(command.BindingIndex);
			}
			Save();
		}

		private static List<Command> BuildCommands()
		{
			List<Command> list = new List<Command>();
			Add(list, "Input.Scoreboard", scoreboard);
			Add(list, "Input.VoteYes", voteYes);
			Add(list, "Input.VoteNo", voteNo);
			Add(list, "Input.Chat", chat);
			Add(list, "Input.VoiceAll", voiceAll);
			Add(list, "Input.VoiceTeam", voiceTeam);
			AddPart(list, "Input.Forward", move, "Up");
			AddPart(list, "Input.Backward", move, "Down");
			AddPart(list, "Input.Left", move, "Left");
			AddPart(list, "Input.Right", move, "Right");
			Add(list, "Input.Run", run);
			Add(list, "Input.Jump", jump);
			Add(list, "Input.Crouch", crouch);
			Add(list, "Input.Fire", fire);
			Add(list, "Input.Scope", scope);
			Add(list, "Input.Block", block);
			Add(list, "Input.ShoulderSwap", shoulderSwap);
			Add(list, "Input.PickUpUse", interact);
			Add(list, "Input.Companion", companion);
			Add(list, "Input.Holster", holster);
			Add(list, "Input.WeaponMenu", weaponMenu);
			Add(list, "Input.WeaponSlot1", weaponSlot1);
			Add(list, "Input.WeaponSlot2", weaponSlot2);
			Add(list, "Input.WeaponSlot3", weaponSlot3);
			Add(list, "Input.Climb", climb);
			Add(list, "Input.Taunt", taunt);
			Add(list, "Input.ViewMode", viewMode);
			Add(list, "Input.EditorGame", editorToggle);
			Add(list, "Input.Transform", toolTransform);
			Add(list, "Input.Extrude", toolExtrude);
			Add(list, "Input.Paint", toolPaint);
			Add(list, "Input.LoopCut", toolLoopCut);
			Add(list, "Input.View", toolView);
			Add(list, "Input.Grid", toggleGrid);
			Add(list, "Input.Dimensions", toggleDimensions);
			Add(list, "Input.ToggleMode", toggleMode);
			Add(list, "Input.ToggleSubMode", toggleSubMode);
			Add(list, "Input.ToggleExtraMode", toggleExtraMode);
			Add(list, "Input.Unlit", toggleUnlit);
			Add(list, "Input.History", toggleHistory);
			Add(list, "Input.Library", toggleLibrary);
			return list;
		}

		private static void Add(List<Command> list, string labelKey, InputAction action)
		{
			list.Add(new Command(action.actionMap.name + "/" + action.name, labelKey, action, 0));
		}

		private static void AddPart(List<Command> list, string labelKey, InputAction action, string part)
		{
			for (int i = 0; i < action.bindings.Count; i++)
			{
				if (action.bindings[i].isPartOfComposite && string.Equals(action.bindings[i].name, part, StringComparison.OrdinalIgnoreCase))
				{
					list.Add(new Command(action.actionMap.name + "/" + action.name + "#" + part, labelKey, action, i));
					break;
				}
			}
		}

		public static bool TryFind(string id, out Command command)
		{
			if (!string.IsNullOrEmpty(id))
			{
				foreach (Command command2 in Commands)
				{
					if (command2.Id == id)
					{
						command = command2;
						return true;
					}
				}
			}
			command = default(Command);
			return false;
		}

		public static string DisplayFor(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return "";
			}
			if (TryFind(id, out var command))
			{
				return command.DisplayString;
			}
			foreach (Command command2 in Commands)
			{
				int num = command2.Id.IndexOf('/');
				if (num >= 0 && string.Equals(command2.Id.Substring(num + 1), id, StringComparison.OrdinalIgnoreCase))
				{
					return command2.DisplayString;
				}
			}
			return "";
		}

		private static bool IsExclusive(string a, string b)
		{
			(string, string)[] exclusive = Exclusive;
			for (int i = 0; i < exclusive.Length; i++)
			{
				var (text, text2) = exclusive[i];
				if ((a == text && b == text2) || (a == text2 && b == text))
				{
					return true;
				}
			}
			return false;
		}

		private static bool CanCoincide(Command a, Command b)
		{
			if (IsExclusive(a.Id, b.Id))
			{
				return false;
			}
			string text = a.Group;
			string text2 = b.Group;
			if (text == text2)
			{
				return true;
			}
			string[][] contexts = Contexts;
			foreach (string[] array in contexts)
			{
				if (Array.IndexOf(array, text) >= 0 && Array.IndexOf(array, text2) >= 0)
				{
					return true;
				}
			}
			return false;
		}

		public static List<Command> Conflicts()
		{
			List<Command> list = new List<Command>();
			foreach (Command command in Commands)
			{
				if (IsConflicting(command))
				{
					list.Add(command);
				}
			}
			return list;
		}

		public static bool IsConflicting(Command command)
		{
			string effectivePath = command.EffectivePath;
			if (string.IsNullOrEmpty(effectivePath))
			{
				return false;
			}
			foreach (Command command2 in Commands)
			{
				if (command2.Id != command.Id && command2.EffectivePath == effectivePath && CanCoincide(command, command2))
				{
					return true;
				}
			}
			return false;
		}

		public static InputActionRebindingExtensions.RebindingOperation Rebind(Command command, Action onFinished)
		{
			Ensure();
			CancelRebind();
			command.Action.Disable();
			InputActionRebindingExtensions.RebindingOperation rebindingOperation = command.Action.PerformInteractiveRebinding(command.BindingIndex).WithCancelingThrough("<Keyboard>/escape").OnMatchWaitForAnother(0.05f);
			if (!command.EffectivePath.StartsWith("<Mouse>", StringComparison.Ordinal))
			{
				rebindingOperation.WithControlsExcluding("<Mouse>");
			}
			rebindingOperation.OnComplete(delegate(InputActionRebindingExtensions.RebindingOperation op)
			{
				activeRebind = null;
				op.Dispose();
				command.Action.Enable();
				Save();
				onFinished?.Invoke();
			});
			rebindingOperation.OnCancel(delegate(InputActionRebindingExtensions.RebindingOperation op)
			{
				activeRebind = null;
				op.Dispose();
				command.Action.Enable();
				onFinished?.Invoke();
			});
			rebindingOperation.Start();
			activeRebind = rebindingOperation;
			return rebindingOperation;
		}

		public static void CancelRebind()
		{
			if (activeRebind != null)
			{
				InputActionRebindingExtensions.RebindingOperation rebindingOperation = activeRebind;
				activeRebind = null;
				if (rebindingOperation.started)
				{
					rebindingOperation.Cancel();
				}
			}
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetOnPlay()
		{
			if (asset != null)
			{
				asset.Disable();
				UnityEngine.Object.Destroy(asset);
			}
			activeRebind = null;
			asset = null;
			commands = null;
		}
	}
}
