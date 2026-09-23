using System.Collections.Generic;
using System.Globalization;
using Mimicraft.Customization;
using Mimicraft.Networking;
using Mimicraft.VoxelEditor.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mimicraft.Dev
{
	public class DevGhostInspector : MonoBehaviour
	{
		private const float PickRadius = 90f;

		private const float PanelWidth = 340f;

		private const float Pad = 14f;

		private const float Row = 24f;

		private const float Gap = 4f;

		private const float LabelWidth = 116f;

		private static DevGhostInspector instance;

		private ulong selected;

		private bool hasSelection;

		private bool wholeClip;

		private string status = "";

		private float statusUntil;

		private string openMenu;

		private Rect openMenuAnchor;

		private readonly List<CharacterListEntry> savedCharacters = new List<CharacterListEntry>();

		private bool savedCharactersRead;

		private readonly List<TemplateListEntry> savedTemplates = new List<TemplateListEntry>();

		private bool savedTemplatesRead;

		private readonly List<string> items = new List<string>();

		private string reconstructSeconds = "3";

		private GUIStyle label;

		private GUIStyle dim;

		private GUIStyle header;

		private GUIStyle button;

		private GUIStyle activeButton;

		private GUIStyle chip;

		private GUIStyle chipOn;

		private GUIStyle warn;

		private GUIStyle value;

		private Texture2D panelTexture;

		private Texture2D lineTexture;

		private Texture2D menuTexture;

		private Texture2D tagTexture;

		private const byte HiderRole = 2;

		private const float ItemHeight = 22f;

		private static readonly HumanBodyBones[] MountBones = new HumanBodyBones[12]
		{
			HumanBodyBones.Hips,
			HumanBodyBones.Spine,
			HumanBodyBones.Chest,
			HumanBodyBones.UpperChest,
			HumanBodyBones.Neck,
			HumanBodyBones.Head,
			HumanBodyBones.LeftUpperArm,
			HumanBodyBones.LeftHand,
			HumanBodyBones.RightUpperArm,
			HumanBodyBones.RightHand,
			HumanBodyBones.LeftFoot,
			HumanBodyBones.RightFoot
		};

		public static bool Visible { get; set; } = true;

		private static Camera ViewCamera
		{
			get
			{
				if (!(DevFreeCamera.Camera != null))
				{
					return Camera.main;
				}
				return DevFreeCamera.Camera;
			}
		}

		public static void Attach(GameObject owner)
		{
			instance = owner.AddComponent<DevGhostInspector>();
		}

		private void Awake()
		{
			instance = this;
		}

		private void OnDestroy()
		{
			if (instance == this)
			{
				instance = null;
			}
		}

		private void Update()
		{
			if (Visible && Mouse.current != null && !(DevReplay.Instance == null) && GameMenuState.IsDevPointerOpen && !GameMenuState.IsDevConsoleOpen && Mouse.current.leftButton.wasPressedThisFrame)
			{
				Vector2 pointer = Mouse.current.position.ReadValue();
				if ((!hasSelection || !(pointer.x > (float)Screen.width - 340f)) && !DevReplayBar.ContainsPointer(pointer))
				{
					Pick(pointer);
				}
			}
		}

		private void Pick(Vector2 pointer)
		{
			Camera viewCamera = ViewCamera;
			if (viewCamera == null)
			{
				return;
			}
			float num = 90f;
			bool flag = false;
			ulong actorId = 0uL;
			foreach (DevReplay.GhostHandle item in DevReplay.Instance.Ghosts())
			{
				Vector3 vector = viewCamera.WorldToScreenPoint(item.Root.position + Vector3.up);
				if (!(vector.z <= 0f))
				{
					float num2 = Vector2.Distance(pointer, new Vector2(vector.x, vector.y));
					if (!(num2 >= num))
					{
						num = num2;
						actorId = item.Actor.Id;
						flag = true;
					}
				}
			}
			if (flag)
			{
				Select(actorId);
			}
		}

		private void Select(ulong actorId)
		{
			selected = actorId;
			hasSelection = true;
			savedCharactersRead = false;
			savedTemplatesRead = false;
			openMenu = null;
		}

		private void OnGUI()
		{
			if (!Visible || DevReplay.Instance == null || !GameMenuState.IsDevPointerOpen)
			{
				return;
			}
			EnsureStyles();
			GUI.depth = -800;
			HandleMenuInput();
			DrawTags();
			if (hasSelection)
			{
				DevRecording.Actor actor = FindActor(selected);
				if (actor == null)
				{
					hasSelection = false;
					return;
				}
				DrawPanel(actor);
				DrawOpenMenu(actor);
			}
		}

		private void DrawPanel(DevRecording.Actor actor)
		{
			DevReplay devReplay = DevReplay.Instance;
			DevRecording recording = devReplay.Recording;
			int currentFrame = devReplay.CurrentFrame;
			DevRecording.Sample sample = SampleAt(actor, currentFrame);
			bool flag = actor.Role == 2;
			float num = (flag ? 88f : 0f);
			float height = 498f + num;
			Rect position = new Rect((float)Screen.width - 340f, 0f, 340f, height);
			GUI.DrawTexture(position, panelTexture);
			float num2 = position.x + 14f;
			float num3 = position.width - 28f;
			float y = position.y + 14f;
			GUI.Label(new Rect(num2, y, num3 - 26f, 24f), NameOf(actor), header);
			if (GUI.Button(new Rect(position.xMax - 14f - 22f, y, 22f, 22f), "×", button))
			{
				hasSelection = false;
			}
			Next(ref y);
			GUI.Label(new Rect(num2, y, num3, 24f), $"client {actor.Id}   ·   {RoleName(actor.Role)}   ·   frame {currentFrame}/{recording.FrameCount}", dim);
			Next(ref y);
			DrawChips(num2, ref y, num3);
			Line(num2, ref y, num3);
			DataRow(num2, ref y, num3, "character", actor.CharacterBytes.Length);
			DataRow(num2, ref y, num3, "voxel body", actor.BodyBytes.Length);
			DataRow(num2, ref y, num3, "weapon skins", actor.WeaponSkinBytes.Length);
			Line(num2, ref y, num3);
			GUI.Label(new Rect(num2, y, 116f, 24f), "apply to", dim);
			float num4 = (num3 - 116f) * 0.5f;
			if (GUI.Button(new Rect(num2 + 116f, y, num4, 24f), "from here", wholeClip ? button : activeButton))
			{
				wholeClip = false;
			}
			if (GUI.Button(new Rect(num2 + 116f + num4, y, num4, 24f), "whole clip", wholeClip ? activeButton : button))
			{
				wholeClip = true;
			}
			Next(ref y);
			Line(num2, ref y, num3);
			MenuRow(num2, ref y, num3, "weapon", "weapon", sample.Present ? WeaponLabel(recording, sample.Weapon) : "—");
			MenuRow(num2, ref y, num3, "held", "held", HeldLabel(actor, currentFrame));
			MenuRow(num2, ref y, num3, "animation", "animation", AnimationLabel(actor, currentFrame));
			MenuRow(num2, ref y, num3, "skins", "weapon skins", (actor.WeaponSkinBytes.Length == 0) ? "not recorded" : "recorded");
			MenuRow(num2, ref y, num3, "character", "character", (actor.CharacterBytes.Length == 0) ? "not recorded" : "recorded");
			MenuRow(num2, ref y, num3, "parts", "body parts", PartsLabel(actor));
			MenuRow(num2, ref y, num3, "camera", "camera", CameraLabel(actor));
			if (flag)
			{
				Line(num2, ref y, num3);
				MenuRow(num2, ref y, num3, "model", "voxel model", ModelLabel(actor));
				ReconstructRows(num2, ref y, num3, recording, actor, currentFrame);
			}
			Line(num2, ref y, num3);
			if (GUI.Button(new Rect(num2, y, 150f, 26f), "Save recording", button))
			{
				Save(recording);
			}
			if (Time.unscaledTime < statusUntil)
			{
				GUI.Label(new Rect(num2 + 158f, y, num3 - 158f, 26f), status, dim);
			}
		}

		private void DrawChips(float x, ref float y, float w)
		{
			int num = 0;
			foreach (DevReplay.GhostHandle item in DevReplay.Instance.Ghosts())
			{
				_ = item;
				num++;
			}
			if (num == 0)
			{
				Next(ref y);
				return;
			}
			float num2 = w / (float)num;
			int num3 = 0;
			foreach (DevReplay.GhostHandle item2 in DevReplay.Instance.Ghosts())
			{
				bool flag = item2.Actor.Id == selected;
				if (GUI.Button(new Rect(x + (float)num3++ * num2, y, num2 - 2f, 24f), NameOf(item2.Actor), flag ? activeButton : button))
				{
					Select(item2.Actor.Id);
				}
			}
			Next(ref y);
		}

		private void MenuRow(float x, ref float y, float w, string id, string name, string caption)
		{
			GUI.Label(new Rect(x, y, 116f, 24f), name, dim);
			Rect position = new Rect(x + 116f, y, w - 116f, 24f);
			if (GUI.Button(position, caption + "   ▾", (openMenu == id) ? activeButton : button))
			{
				openMenu = ((openMenu == id) ? null : id);
				openMenuAnchor = position;
			}
			Next(ref y);
		}

		private void ReconstructRows(float x, ref float y, float w, DevRecording recording, DevRecording.Actor actor, int frame)
		{
			DevRecording.Reconstruct found;
			bool flag = recording.TryGetReconstruct(actor.Id, out found);
			GUI.Label(new Rect(x, y, 116f, 24f), "build over", dim);
			float num = 60f;
			reconstructSeconds = GUI.TextField(new Rect(x + 116f, y, num, 24f), reconstructSeconds ?? "", 6);
			GUI.Label(new Rect(x + 116f + num + 6f, y, w, 24f), "seconds", dim);
			Next(ref y);
			float num2 = (w - 116f) * 0.5f;
			if (GUI.Button(new Rect(x + 116f, y, num2, 24f), "reconstruct from now", button))
			{
				if (!float.TryParse(reconstructSeconds, NumberStyles.Float, CultureInfo.InvariantCulture, out var result) || result <= 0f)
				{
					Say("Duration must be a positive number.");
				}
				else if (actor.BodyBytes.Length == 0 && !HasBodyChange(recording, actor.Id))
				{
					Say("No model recorded for this actor.");
				}
				else
				{
					recording.SetReconstruct(actor.Id, frame, result);
					DevBodyReconstruct.Forget(actor.Id);
					Say($"Reconstruct at frame {frame}, {result:0.##}s.");
				}
			}
			if (flag && GUI.Button(new Rect(x + 116f + num2, y, num2, 24f), "clear", button))
			{
				recording.ClearReconstruct(actor.Id);
				DevBodyReconstruct.Forget(actor.Id);
				Say("Reconstruct removed.");
			}
			Next(ref y);
			GUI.Label(new Rect(x, y, w, 24f), flag ? $"builds from frame {found.Frame} over {found.Seconds:0.##}s" : "no reconstruct", flag ? value : dim);
			Next(ref y);
		}

		private static bool HasBodyChange(DevRecording recording, ulong actor)
		{
			foreach (DevRecording.BodyChange bodyChange in recording.BodyChanges)
			{
				if (bodyChange.Actor == actor && bodyChange.Bytes.Length != 0)
				{
					return true;
				}
			}
			return false;
		}

		private void DataRow(float x, ref float y, float w, string name, int bytes)
		{
			GUI.Label(new Rect(x, y, 116f, 24f), name, dim);
			GUI.Label(new Rect(x + 116f, y, w - 116f, 24f), (bytes == 0) ? "not recorded" : $"{(float)bytes / 1024f:0.0} kB", (bytes == 0) ? warn : value);
			Next(ref y);
		}

		private void HandleMenuInput()
		{
			if (openMenu == null || Event.current == null)
			{
				return;
			}
			EventType type = Event.current.type;
			if (type != EventType.MouseDown && type != EventType.MouseUp)
			{
				return;
			}
			DevRecording.Actor actor = FindActor(selected);
			if (actor == null)
			{
				return;
			}
			DevRecording recording = DevReplay.Instance.Recording;
			CharacterRigDefinition rig = RigFor(actor.Id);
			BuildItems(actor, recording, rig);
			Rect rect = MenuRect(items.Count);
			Vector2 mousePosition = Event.current.mousePosition;
			if (!rect.Contains(mousePosition))
			{
				if (type == EventType.MouseDown && !openMenuAnchor.Contains(mousePosition))
				{
					openMenu = null;
				}
				return;
			}
			Event.current.Use();
			if (type != EventType.MouseDown)
			{
				return;
			}
			int num = Mathf.FloorToInt((mousePosition.y - rect.y - 4f) / 22f);
			if (num >= 0 && num < items.Count)
			{
				Choose(actor, recording, rig, DevReplay.Instance.CurrentFrame, num);
				if (openMenu != "parts")
				{
					openMenu = null;
				}
			}
		}

		private Rect MenuRect(int count)
		{
			return new Rect(openMenuAnchor.x, openMenuAnchor.yMax + 2f, openMenuAnchor.width, (float)count * 22f + 8f);
		}

		private void DrawOpenMenu(DevRecording.Actor actor)
		{
			if (openMenu == null)
			{
				return;
			}
			DevRecording recording = DevReplay.Instance.Recording;
			_ = DevReplay.Instance.CurrentFrame;
			CharacterRigDefinition rig = RigFor(actor.Id);
			BuildItems(actor, recording, rig);
			if (items.Count == 0)
			{
				openMenu = null;
				return;
			}
			Rect position = MenuRect(items.Count);
			GUI.DrawTexture(position, menuTexture);
			Vector2 mousePosition = Event.current.mousePosition;
			for (int i = 0; i < items.Count; i++)
			{
				Rect position2 = new Rect(position.x + 4f, position.y + 4f + (float)i * 22f, position.width - 8f, 22f);
				if (position2.Contains(mousePosition))
				{
					GUI.color = new Color(1f, 1f, 1f, 0.1f);
					GUI.DrawTexture(position2, tagTexture);
					GUI.color = Color.white;
				}
				GUI.Label(new Rect(position2.x + 8f, position2.y, position2.width - 8f, position2.height), items[i], position2.Contains(mousePosition) ? label : dim);
			}
		}

		private void BuildItems(DevRecording.Actor actor, DevRecording recording, CharacterRigDefinition rig)
		{
			items.Clear();
			switch (openMenu)
			{
			case "weapon":
			{
				for (int i = 0; i < recording.WeaponIds.Count; i++)
				{
					items.Add(WeaponLabel(recording, i));
				}
				break;
			}
			case "held":
				items.Add("holding");
				items.Add("empty handed");
				break;
			case "animation":
				items.Add("as recorded");
				items.Add("drive from parameters");
				break;
			case "skins":
				items.Add("this machine's weapons");
				{
					foreach (DevRecording.Actor actor2 in recording.Actors)
					{
						if (actor2 != actor && actor2.WeaponSkinBytes.Length != 0)
						{
							items.Add("copy from " + NameOf(actor2));
						}
					}
					break;
				}
			case "parts":
			{
				foreach (CharacterAssembler.BuiltPart item in Parts(actor.Id))
				{
					bool flag = IsShown(actor.Id, item);
					items.Add((flag ? "◉" : "◌") + "  " + item.Definition.DisplayName);
				}
				break;
			}
			case "camera":
				items.Add(DevFreeCamera.IsAttached ? "detach" : "detached (free)");
				{
					foreach (Transform item2 in Mounts(actor.Id))
					{
						items.Add("attach to " + item2.name);
					}
					break;
				}
			case "model":
				ReadSavedTemplates();
				if (savedTemplates.Count == 0)
				{
					items.Add("no saved models");
					break;
				}
				{
					foreach (TemplateListEntry savedTemplate in savedTemplates)
					{
						items.Add(TemplateLabel(savedTemplate));
					}
					break;
				}
			case "character":
				if (rig == null)
				{
					break;
				}
				foreach (CharacterAssetDefinition preset in rig.Presets)
				{
					if (preset != null && preset.asset != null)
					{
						items.Add(PresetName(preset));
					}
				}
				ReadSavedCharacters();
				{
					foreach (CharacterListEntry savedCharacter in savedCharacters)
					{
						items.Add(savedCharacter.CharacterName);
					}
					break;
				}
			}
		}

		private void Choose(DevRecording.Actor actor, DevRecording recording, CharacterRigDefinition rig, int frame, int index)
		{
			int num = ((!wholeClip) ? frame : 0);
			switch (openMenu)
			{
			case "weapon":
				SetWeapon(actor, (byte)index, num);
				break;
			case "held":
				SetArmed(actor, (index == 0) ? 1f : 0f, num);
				break;
			case "animation":
				if (index == 1)
				{
					FreeRun(actor, num);
				}
				else
				{
					Say("Recorded states are only restored by reloading the recording.");
				}
				break;
			case "skins":
				ChooseSkins(actor, recording, index);
				break;
			case "character":
				ChooseCharacter(actor, rig, index);
				break;
			case "model":
				ChooseModel(recording, actor, index, frame);
				break;
			case "parts":
				TogglePart(actor.Id, index);
				break;
			case "camera":
				ChooseCamera(actor, index);
				break;
			}
		}

		private void TogglePart(ulong actorId, int index)
		{
			CharacterAssembler characterAssembler = AssemblerFor(actorId);
			if (characterAssembler == null)
			{
				return;
			}
			int num = 0;
			foreach (CharacterAssembler.BuiltPart item in Parts(actorId))
			{
				if (num++ == index)
				{
					string partId = item.Definition.PartId;
					bool flag = characterAssembler.IsPartVisible(partId);
					characterAssembler.SetPartVisible(partId, !flag);
					Say(item.Definition.DisplayName + " " + (flag ? "hidden" : "shown") + ".");
					break;
				}
			}
		}

		private IEnumerable<CharacterAssembler.BuiltPart> Parts(ulong actorId)
		{
			CharacterAssembler characterAssembler = AssemblerFor(actorId);
			if (characterAssembler == null)
			{
				yield break;
			}
			foreach (CharacterAssembler.BuiltPart builtPart in characterAssembler.BuiltParts)
			{
				if (builtPart.Definition != null)
				{
					yield return builtPart;
				}
			}
		}

		private bool IsShown(ulong actorId, CharacterAssembler.BuiltPart part)
		{
			CharacterAssembler characterAssembler = AssemblerFor(actorId);
			if (characterAssembler != null)
			{
				return characterAssembler.IsPartVisible(part.Definition.PartId);
			}
			return false;
		}

		private string PartsLabel(DevRecording.Actor actor)
		{
			int num = 0;
			int num2 = 0;
			foreach (CharacterAssembler.BuiltPart item in Parts(actor.Id))
			{
				num2++;
				if (IsShown(actor.Id, item))
				{
					num++;
				}
			}
			if (num2 == 0)
			{
				return "none";
			}
			if (num != num2)
			{
				return $"{num} of {num2} shown";
			}
			return $"all {num2} shown";
		}

		private static CharacterAssembler AssemblerFor(ulong actorId)
		{
			foreach (DevReplay.GhostHandle item in DevReplay.Instance.Ghosts())
			{
				if (item.Actor.Id == actorId)
				{
					return item.Assembler;
				}
			}
			return null;
		}

		private void ChooseCamera(DevRecording.Actor actor, int index)
		{
			if (index == 0)
			{
				DevFreeCamera.Detach();
				Say("Camera detached.");
				return;
			}
			int num = 0;
			foreach (Transform item in Mounts(actor.Id))
			{
				if (++num == index)
				{
					if (!DevFreeCamera.Active)
					{
						Say("Turn the free camera on first - `freecam on`.");
						break;
					}
					DevFreeCamera.Attach(item);
					Say("Camera riding " + item.name + ".");
					break;
				}
			}
		}

		private IEnumerable<Transform> Mounts(ulong actorId)
		{
			foreach (DevReplay.GhostHandle ghost in DevReplay.Instance.Ghosts())
			{
				if (ghost.Actor.Id != actorId)
				{
					continue;
				}
				yield return ghost.Root;
				Animator animator = ghost.Animator;
				if (animator == null || !animator.isHuman)
				{
					yield break;
				}
				HumanBodyBones[] mountBones = MountBones;
				foreach (HumanBodyBones humanBoneId in mountBones)
				{
					Transform boneTransform = animator.GetBoneTransform(humanBoneId);
					if (boneTransform != null)
					{
						yield return boneTransform;
					}
				}
				yield break;
			}
		}

		private string CameraLabel(DevRecording.Actor actor)
		{
			Transform attachedTo = DevFreeCamera.AttachedTo;
			if (attachedTo == null)
			{
				return "free";
			}
			foreach (Transform item in Mounts(actor.Id))
			{
				if (item == attachedTo)
				{
					return "on " + attachedTo.name;
				}
			}
			return "on another ghost";
		}

		private void ChooseSkins(DevRecording.Actor actor, DevRecording recording, int index)
		{
			if (index == 0)
			{
				Dictionary<string, WeaponSkinData> dictionary = WeaponSkinStorage.LoadAll();
				if (dictionary == null || dictionary.Count == 0)
				{
					Say("No weapons saved on this machine.");
					return;
				}
				actor.WeaponSkinBytes = WeaponSkinFile.Encode(dictionary);
				DevReplay.Instance.Invalidate(actor.Id);
				Say($"Applied {dictionary.Count} weapon(s).");
				return;
			}
			int num = 0;
			foreach (DevRecording.Actor actor2 in recording.Actors)
			{
				if (actor2 != actor && actor2.WeaponSkinBytes.Length != 0 && ++num == index)
				{
					actor.WeaponSkinBytes = actor2.WeaponSkinBytes;
					DevReplay.Instance.Invalidate(actor.Id);
					Say("Copied " + NameOf(actor2) + "'s weapons.");
					break;
				}
			}
		}

		private static string ModelLabel(DevRecording.Actor actor)
		{
			if (actor.BodyBytes.Length != 0)
			{
				return $"{actor.BodyBytes.Length} B";
			}
			return "not recorded";
		}

		private static string TemplateLabel(TemplateListEntry entry)
		{
			if (!string.IsNullOrEmpty(entry.Tag))
			{
				return entry.ModelName + "  (" + entry.Tag + ")";
			}
			return entry.ModelName;
		}

		private void ReadSavedTemplates()
		{
			if (!savedTemplatesRead)
			{
				savedTemplates.Clear();
				savedTemplates.AddRange(TemplateStorage.ListTemplates());
				savedTemplatesRead = true;
			}
		}

		private void ChooseModel(DevRecording recording, DevRecording.Actor actor, int index, int frame)
		{
			ReadSavedTemplates();
			if (index < 0 || index >= savedTemplates.Count)
			{
				Say("No saved models. Save one from the model library first.");
				return;
			}
			TemplateListEntry templateListEntry = savedTemplates[index];
			TemplateModel templateModel = TemplateStorage.LoadTemplate(templateListEntry.FilePath);
			if (templateModel == null || templateModel.Pieces.Count == 0)
			{
				Say("'" + templateListEntry.ModelName + "' could not be read.");
				return;
			}
			byte[] array = VoxelBodyCodec.Encode(TemplateFile.ToBodyData(templateModel));
			if (wholeClip)
			{
				actor.BodyBytes = array;
				recording.BodyChanges.RemoveAll((DevRecording.BodyChange change) => change.Actor == actor.Id);
			}
			else
			{
				recording.BodyChanges.RemoveAll((DevRecording.BodyChange change) => change.Actor == actor.Id && change.Frame >= frame);
				recording.BodyChanges.Add(new DevRecording.BodyChange
				{
					Frame = frame,
					Actor = actor.Id,
					Bytes = array
				});
				recording.BodyChanges.Sort((DevRecording.BodyChange a, DevRecording.BodyChange b) => a.Frame.CompareTo(b.Frame));
			}
			ApplyModelNow(actor.Id, array);
			Say("Applied model '" + templateListEntry.ModelName + "'.");
		}

		private void ApplyModelNow(ulong actorId, byte[] payload)
		{
			foreach (DevReplay.GhostHandle item in DevReplay.Instance.Ghosts())
			{
				if (item.Actor.Id == actorId && !(item.Root == null))
				{
					PlayerVoxelBody componentInChildren = item.Root.GetComponentInChildren<PlayerVoxelBody>(includeInactive: true);
					if (componentInChildren != null)
					{
						componentInChildren.DevApplyPayload(payload);
					}
					break;
				}
			}
		}

		private void ChooseCharacter(DevRecording.Actor actor, CharacterRigDefinition rig, int index)
		{
			if (rig == null)
			{
				return;
			}
			int num = 0;
			foreach (CharacterAssetDefinition preset in rig.Presets)
			{
				if (preset != null && !(preset.asset == null) && num++ == index)
				{
					SetCharacter(actor, preset.asset.ToCharacterData(rig), PresetName(preset));
					return;
				}
			}
			ReadSavedCharacters();
			foreach (CharacterListEntry savedCharacter in savedCharacters)
			{
				if (num++ == index)
				{
					SetCharacter(actor, CharacterStorage.Load(savedCharacter.FilePath, rig), savedCharacter.CharacterName);
					break;
				}
			}
		}

		private void SetWeapon(DevRecording.Actor actor, byte weapon, int from)
		{
			for (int i = Mathf.Max(0, from); i < actor.Samples.Count; i++)
			{
				DevRecording.Sample sample = actor.Samples[i];
				if (sample.Present)
				{
					sample.Weapon = weapon;
					actor.Samples[i] = sample;
				}
			}
			DevReplay.Instance.Invalidate(actor.Id);
			Say((from == 0) ? "Weapon set for the whole clip." : $"Weapon set from frame {from}.");
		}

		private void SetArmed(DevRecording.Actor actor, float value, int from)
		{
			int num = ParameterIndex(actor, "Armed");
			if (num < 0)
			{
				Say("This actor has no recorded 'Armed' parameter.");
				return;
			}
			for (int i = Mathf.Max(0, from); i < actor.Samples.Count; i++)
			{
				DevRecording.Sample sample = actor.Samples[i];
				if (sample.Present && sample.Parameters != null && num < sample.Parameters.Length)
				{
					sample.Parameters[num] = value;
					actor.Samples[i] = sample;
				}
			}
			DevReplay.Instance.Invalidate(actor.Id);
			Say((from == 0) ? "Held set for the whole clip." : $"Held set from frame {from}.");
		}

		private void FreeRun(DevRecording.Actor actor, int from)
		{
			for (int i = Mathf.Max(0, from); i < actor.Samples.Count; i++)
			{
				DevRecording.Sample sample = actor.Samples[i];
				if (sample.Present && sample.Layers != null)
				{
					for (int j = 0; j < sample.Layers.Length; j++)
					{
						DevRecording.LayerState layerState = sample.Layers[j];
						layerState.Hash = 0;
						sample.Layers[j] = layerState;
					}
					actor.Samples[i] = sample;
				}
			}
			Say((from == 0) ? "Animation handed to the controller for the whole clip." : $"Animation handed to the controller from frame {from}.");
		}

		private void SetCharacter(DevRecording.Actor actor, CharacterData data, string name)
		{
			if (data == null)
			{
				Say("'" + name + "' could not be read.");
				return;
			}
			actor.CharacterBytes = CharacterCodec.Encode(data);
			CharacterAssembler characterAssembler = AssemblerFor(actor.Id);
			if (characterAssembler != null && characterAssembler.Rig != null)
			{
				actor.CharacterBoxes = CurrentBoxes(characterAssembler.Rig);
				DevReplay.Instance.Redress(actor.Id);
				Say("Applied character '" + name + "'.");
			}
			else
			{
				Say("Applied character '" + name + "', but the ghost has no rig - size may be wrong until it is re-dressed.");
				DevReplay.Instance.Redress(actor.Id);
			}
		}

		private static string[] CurrentBoxes(CharacterRigDefinition rig)
		{
			List<string> list = new List<string>();
			foreach (CharacterPartDefinition part in rig.Parts)
			{
				if (!(part == null) && !string.IsNullOrEmpty(part.PartId))
				{
					Vector3Int boxSize = part.BoxSize;
					list.Add($"{part.PartId} {boxSize.x} {boxSize.y} {boxSize.z}");
				}
			}
			return list.ToArray();
		}

		private void Save(DevRecording recording)
		{
			string text = DevRecording.PathFor(recording.Name);
			recording.Save(text);
			Say("Saved.");
			DevConsole.Log("[ghost] Recording updated: " + text);
		}

		private void DrawTags()
		{
			Camera viewCamera = ViewCamera;
			if (viewCamera == null)
			{
				return;
			}
			foreach (DevReplay.GhostHandle item in DevReplay.Instance.Ghosts())
			{
				Vector3 vector = viewCamera.WorldToScreenPoint(item.Root.position + Vector3.up * 2f);
				if (!(vector.z <= 0f))
				{
					bool flag = hasSelection && item.Actor.Id == selected;
					string text = NameOf(item.Actor);
					Vector2 vector2 = chip.CalcSize(new GUIContent(text));
					Rect position = new Rect(vector.x - vector2.x * 0.5f - 8f, (float)Screen.height - vector.y - 11f, vector2.x + 16f, 20f);
					GUI.color = (flag ? new Color(1f, 0.78f, 0.25f, 0.92f) : new Color(0f, 0f, 0f, 0.55f));
					GUI.DrawTexture(position, tagTexture);
					GUI.color = Color.white;
					if (GUI.Button(position, text, flag ? chipOn : chip))
					{
						Select(item.Actor.Id);
					}
				}
			}
		}

		private void ReadSavedCharacters()
		{
			if (!savedCharactersRead)
			{
				savedCharacters.Clear();
				savedCharacters.AddRange(CharacterStorage.List());
				savedCharactersRead = true;
			}
		}

		private static void Next(ref float y)
		{
			y += 28f;
		}

		private void Line(float x, ref float y, float w)
		{
			GUI.DrawTexture(new Rect(x, y - 2f, w, 1f), lineTexture);
			y += 4f;
		}

		private string HeldLabel(DevRecording.Actor actor, int frame)
		{
			int num = ParameterIndex(actor, "Armed");
			if (num < 0)
			{
				return "not recorded";
			}
			DevRecording.Sample sample = SampleAt(actor, frame);
			if (!sample.Present || sample.Parameters == null || num >= sample.Parameters.Length)
			{
				return "—";
			}
			if (!(sample.Parameters[num] > 0.5f))
			{
				return "empty handed";
			}
			return "holding";
		}

		private static string AnimationLabel(DevRecording.Actor actor, int frame)
		{
			DevRecording.Sample sample = SampleAt(actor, frame);
			if (!sample.Present || sample.Layers == null || sample.Layers.Length == 0)
			{
				return "—";
			}
			if (sample.Layers[0].Hash != 0)
			{
				return "as recorded";
			}
			return "from parameters";
		}

		private static string PresetName(CharacterAssetDefinition preset)
		{
			if (!string.IsNullOrWhiteSpace(preset.name))
			{
				return preset.name;
			}
			return preset.asset.name;
		}

		private static string NameOf(DevRecording.Actor actor)
		{
			if (!string.IsNullOrWhiteSpace(actor.DisplayName))
			{
				return actor.DisplayName;
			}
			return $"#{actor.Id}";
		}

		private static string WeaponLabel(DevRecording recording, int index)
		{
			string result = recording.WeaponIdAt(index);
			if (!string.IsNullOrEmpty(result))
			{
				return result;
			}
			return "empty handed";
		}

		private static CharacterRigDefinition RigFor(ulong actorId)
		{
			foreach (DevReplay.GhostHandle item in DevReplay.Instance.Ghosts())
			{
				if (item.Actor.Id == actorId)
				{
					return item.Rig;
				}
			}
			return null;
		}

		private static int ParameterIndex(DevRecording.Actor actor, string name)
		{
			for (int i = 0; i < actor.Parameters.Length; i++)
			{
				if (actor.Parameters[i] == name)
				{
					return i;
				}
			}
			return -1;
		}

		private static DevRecording.Sample SampleAt(DevRecording.Actor actor, int frame)
		{
			if (frame < 0 || frame >= actor.Samples.Count)
			{
				return default(DevRecording.Sample);
			}
			return actor.Samples[frame];
		}

		private static DevRecording.Actor FindActor(ulong id)
		{
			return ((DevReplay.Instance != null) ? DevReplay.Instance.Recording : null)?.Find(id);
		}

		private static string RoleName(byte role)
		{
			return role switch
			{
				1 => "Hunter", 
				2 => "Hider", 
				_ => "no role", 
			};
		}

		private void Say(string message)
		{
			status = message;
			statusUntil = Time.unscaledTime + 4f;
		}

		private static Texture2D Fill(Color colour)
		{
			Texture2D texture2D = new Texture2D(1, 1);
			texture2D.SetPixel(0, 0, colour);
			texture2D.Apply();
			texture2D.hideFlags = HideFlags.HideAndDontSave;
			return texture2D;
		}

		private void EnsureStyles()
		{
			if (label == null)
			{
				panelTexture = Fill(new Color(0.055f, 0.065f, 0.085f, 0.95f));
				menuTexture = Fill(new Color(0.09f, 0.11f, 0.14f, 0.99f));
				lineTexture = Fill(new Color(1f, 1f, 1f, 0.09f));
				tagTexture = Fill(Color.white);
				label = new GUIStyle(GUI.skin.label)
				{
					fontSize = 12,
					alignment = TextAnchor.MiddleLeft
				};
				label.normal.textColor = new Color(0.88f, 0.91f, 0.96f);
				value = new GUIStyle(label);
				dim = new GUIStyle(label)
				{
					fontSize = 11
				};
				dim.normal.textColor = new Color(0.52f, 0.58f, 0.67f);
				warn = new GUIStyle(label)
				{
					fontSize = 11
				};
				warn.normal.textColor = new Color(1f, 0.62f, 0.42f);
				header = new GUIStyle(label)
				{
					fontSize = 14,
					fontStyle = FontStyle.Bold
				};
				header.normal.textColor = Color.white;
				button = new GUIStyle(GUI.skin.button)
				{
					fontSize = 11,
					alignment = TextAnchor.MiddleLeft
				};
				button.padding = new RectOffset(8, 8, 2, 2);
				activeButton = new GUIStyle(button)
				{
					fontStyle = FontStyle.Bold
				};
				activeButton.normal.textColor = new Color(1f, 0.8f, 0.3f);
				activeButton.hover.textColor = new Color(1f, 0.86f, 0.45f);
				chip = new GUIStyle(GUI.skin.label)
				{
					fontSize = 11,
					alignment = TextAnchor.MiddleCenter
				};
				chip.normal.textColor = new Color(0.92f, 0.94f, 0.98f);
				chipOn = new GUIStyle(chip)
				{
					fontStyle = FontStyle.Bold
				};
				chipOn.normal.textColor = new Color(0.09f, 0.1f, 0.12f);
			}
		}
	}
}
