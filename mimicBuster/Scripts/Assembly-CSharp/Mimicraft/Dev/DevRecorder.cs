using System;
using System.Collections.Generic;
using Mimicraft.Customization;
using Mimicraft.Gameplay;
using Mimicraft.Networking;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.Dev
{
	public class DevRecorder : MonoBehaviour
	{
		private static DevRecorder instance;

		private DevRecording recording;

		private float nextSampleTime;

		private int frame;

		private readonly Dictionary<ulong, DevRecording.Actor> tracks = new Dictionary<ulong, DevRecording.Actor>();

		private readonly Dictionary<ulong, Animator> animators = new Dictionary<ulong, Animator>();

		private readonly Dictionary<ulong, PlayerVoxelBody> bodies = new Dictionary<ulong, PlayerVoxelBody>();

		private readonly Dictionary<ulong, byte[]> lastBodies = new Dictionary<ulong, byte[]>();

		public const byte ShotKind = 200;

		private const float TailSeconds = 6f;

		private bool sawRound;

		private float stopAt = -1f;

		private readonly HashSet<ulong> settled = new HashSet<ulong>();

		public static bool Active => instance != null;

		public static DevRecorder Instance => instance;

		public string Name
		{
			get
			{
				if (recording == null)
				{
					return "";
				}
				return recording.Name;
			}
		}

		public int Frames => frame;

		public float Seconds
		{
			get
			{
				if (recording == null || !(recording.TickRate > 0f))
				{
					return 0f;
				}
				return (float)frame / recording.TickRate;
			}
		}

		public int ActorCount => tracks.Count;

		public static void Begin(string name, float tickRate)
		{
			Stop(save: true);
			GameObject obj = new GameObject("DevRecorder");
			UnityEngine.Object.DontDestroyOnLoad(obj);
			instance = obj.AddComponent<DevRecorder>();
			instance.recording = new DevRecording
			{
				Name = DevRecording.SanitizeName(name),
				MapId = CurrentMapId(),
				TickRate = Mathf.Clamp(tickRate, 5f, 120f)
			};
		}

		public static string Stop(bool save)
		{
			if (instance == null)
			{
				return "";
			}
			string text = "";
			if (save && instance.frame > 0)
			{
				instance.recording.NoteFrame(instance.frame);
				text = DevRecording.PathFor(instance.recording.Name);
				instance.recording.Save(text);
			}
			UnityEngine.Object.Destroy(instance.gameObject);
			instance = null;
			return text;
		}

		private void OnEnable()
		{
			ImpactEffects.Happened += OnImpact;
			ImpactEffects.ShotFired += OnShot;
		}

		private void OnDisable()
		{
			ImpactEffects.Happened -= OnImpact;
			ImpactEffects.ShotFired -= OnShot;
		}

		private void OnShot(ulong shooterId, Vector3 position, WeaponDefinition weapon)
		{
			if (recording != null && recording.Events.Count < 200000)
			{
				recording.Events.Add(new DevRecording.ImpactEvent
				{
					Frame = frame,
					Kind = 200,
					Position = position,
					Actor = shooterId,
					Weapon = recording.WeaponIndex((weapon != null) ? weapon.WeaponId : "")
				});
			}
		}

		private void OnImpact(ImpactKind kind, Vector3 position, Vector3 normal)
		{
			if (recording != null && recording.Events.Count < 200000)
			{
				recording.Events.Add(new DevRecording.ImpactEvent
				{
					Frame = frame,
					Kind = (byte)kind,
					Position = position,
					Normal = normal
				});
			}
		}

		private void OnDestroy()
		{
			if (instance == this)
			{
				instance = null;
			}
		}

		private void LateUpdate()
		{
			if (recording != null)
			{
				StopIfRoundEnded();
				if (!(instance == null) && !(Time.unscaledTime < nextSampleTime))
				{
					nextSampleTime = Time.unscaledTime + 1f / recording.TickRate;
					SampleFrame();
				}
			}
		}

		private void StopIfRoundEnded()
		{
			RoundManager roundManager = UnityEngine.Object.FindFirstObjectByType<RoundManager>();
			if (roundManager == null)
			{
				return;
			}
			RoundPhase value = roundManager.CurrentPhase.Value;
			if (value == RoundPhase.Prep || value == RoundPhase.Hunt)
			{
				sawRound = true;
				stopAt = -1f;
			}
			else if (sawRound)
			{
				if (stopAt < 0f)
				{
					stopAt = Time.unscaledTime + 6f;
				}
				else if (!(Time.unscaledTime < stopAt))
				{
					string text = Stop(save: true);
					DevConsole.Log((text.Length > 0) ? ("Round bitti - kayit durduruldu: " + text) : "Round bitti - kayit durduruldu (bos).");
				}
			}
		}

		private void SampleFrame()
		{
			PlayerMovement[] array = UnityEngine.Object.FindObjectsByType<PlayerMovement>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
			foreach (PlayerMovement playerMovement in array)
			{
				NetworkObject component = playerMovement.GetComponent<NetworkObject>();
				if (!(component == null))
				{
					DevRecording.Actor actor = Track(component, playerMovement);
					Fill(actor, playerMovement);
				}
			}
			foreach (DevRecording.Actor value in tracks.Values)
			{
				while (value.Samples.Count <= frame)
				{
					value.Samples.Add(default(DevRecording.Sample));
				}
			}
			SampleBodies();
			frame++;
		}

		private void SampleBodies()
		{
			foreach (KeyValuePair<ulong, PlayerVoxelBody> body in bodies)
			{
				if (!(body.Value == null))
				{
					byte[] array = body.Value.ModelData.Value.Data ?? Array.Empty<byte>();
					if ((!lastBodies.TryGetValue(body.Key, out var value) || value != array) && (value == null || !Same(value, array)))
					{
						lastBodies[body.Key] = array;
						recording.BodyChanges.Add(new DevRecording.BodyChange
						{
							Frame = frame,
							Actor = body.Key,
							Bytes = array
						});
					}
				}
			}
		}

		private static bool Same(byte[] a, byte[] b)
		{
			if (a.Length != b.Length)
			{
				return false;
			}
			for (int i = 0; i < a.Length; i++)
			{
				if (a[i] != b[i])
				{
					return false;
				}
			}
			return true;
		}

		private DevRecording.Actor Track(NetworkObject netObject, PlayerMovement player)
		{
			ulong ownerClientId = netObject.OwnerClientId;
			if (tracks.TryGetValue(ownerClientId, out var value))
			{
				TopUp(value, player);
				return value;
			}
			DevRecording.Actor actor = new DevRecording.Actor
			{
				Id = ownerClientId
			};
			PlayerVoxelBody component = player.GetComponent<PlayerVoxelBody>();
			if (component != null)
			{
				bodies[ownerClientId] = component;
			}
			TopUp(actor, player);
			Animator animator = ResolveAnimator(player);
			if (animator != null && animator.runtimeAnimatorController != null)
			{
				animators[ownerClientId] = animator;
				List<string> list = new List<string>();
				AnimatorControllerParameter[] parameters = animator.parameters;
				foreach (AnimatorControllerParameter animatorControllerParameter in parameters)
				{
					if (animatorControllerParameter.type != AnimatorControllerParameterType.Trigger)
					{
						list.Add(animatorControllerParameter.name);
					}
				}
				actor.Parameters = list.ToArray();
				actor.LayerCount = animator.layerCount;
			}
			settled.Remove(ownerClientId);
			while (actor.Samples.Count < frame)
			{
				actor.Samples.Add(default(DevRecording.Sample));
			}
			tracks[ownerClientId] = actor;
			recording.Actors.Add(actor);
			return actor;
		}

		private void TopUp(DevRecording.Actor actor, PlayerMovement player)
		{
			if (settled.Contains(actor.Id))
			{
				return;
			}
			bool flag = true;
			if (actor.CharacterBytes.Length == 0)
			{
				PlayerCharacterAppearance component = player.GetComponent<PlayerCharacterAppearance>();
				if (component != null)
				{
					actor.CharacterBytes = component.CharacterBytes;
				}
				if (actor.CharacterBytes.Length != 0)
				{
					actor.CharacterBoxes = CaptureCharacterBoxes(player);
				}
				flag &= actor.CharacterBytes.Length != 0;
			}
			if (actor.BodyBytes.Length == 0)
			{
				PlayerVoxelBody component2 = player.GetComponent<PlayerVoxelBody>();
				if (component2 != null)
				{
					actor.BodyBytes = component2.ModelData.Value.Data ?? Array.Empty<byte>();
				}
			}
			if (actor.WeaponSkinBytes.Length == 0)
			{
				PlayerWeaponSkinsSync component3 = player.GetComponent<PlayerWeaponSkinsSync>();
				if (component3 != null)
				{
					actor.WeaponSkinBytes = component3.LoadoutBytes;
				}
				flag &= actor.WeaponSkinBytes.Length != 0;
			}
			if (string.IsNullOrEmpty(actor.WeaponId))
			{
				PlayerWeapons component4 = player.GetComponent<PlayerWeapons>();
				if (component4 != null && component4.Equipped != null)
				{
					actor.WeaponId = component4.Equipped.WeaponId;
				}
			}
			if (actor.Role == 0)
			{
				RoundManager roundManager = UnityEngine.Object.FindFirstObjectByType<RoundManager>();
				if (roundManager != null && NetworkManager.Singleton != null && NetworkManager.Singleton.IsServer)
				{
					actor.Role = (byte)roundManager.GetServerRole(actor.Id);
				}
				flag &= actor.Role != 0;
			}
			if (flag)
			{
				settled.Add(actor.Id);
			}
		}

		private void Fill(DevRecording.Actor actor, PlayerMovement player)
		{
			PlayerWeapons component = player.GetComponent<PlayerWeapons>();
			DevRecording.Sample sample = new DevRecording.Sample
			{
				Present = true,
				Position = player.transform.position,
				Rotation = player.transform.rotation,
				Weapon = recording.WeaponIndex((component != null && component.Equipped != null && player.IsArmed) ? component.Equipped.WeaponId : ""),
				Running = player.IsRunning
			};
			if (animators.TryGetValue(actor.Id, out var value) && value != null)
			{
				sample.Parameters = new float[actor.Parameters.Length];
				for (int i = 0; i < actor.Parameters.Length; i++)
				{
					sample.Parameters[i] = ReadParameter(value, actor.Parameters[i]);
				}
				sample.Layers = new DevRecording.LayerState[actor.LayerCount];
				for (int j = 0; j < sample.Layers.Length && j < value.layerCount; j++)
				{
					AnimatorStateInfo currentAnimatorStateInfo = value.GetCurrentAnimatorStateInfo(j);
					sample.Layers[j] = new DevRecording.LayerState
					{
						Hash = currentAnimatorStateInfo.fullPathHash,
						NormalizedTime = currentAnimatorStateInfo.normalizedTime,
						Weight = value.GetLayerWeight(j)
					};
				}
			}
			PlayerRagdoll component2 = player.GetComponent<PlayerRagdoll>();
			if (component2 != null && component2.IsLimp)
			{
				IReadOnlyList<Rigidbody> boneBodies = component2.BoneBodies;
				DevRecording.BonePose[] array = new DevRecording.BonePose[boneBodies.Count];
				for (int k = 0; k < boneBodies.Count; k++)
				{
					Transform transform = ((boneBodies[k] != null) ? boneBodies[k].transform : null);
					if (!(transform == null))
					{
						array[k] = new DevRecording.BonePose
						{
							Position = transform.localPosition,
							Rotation = transform.localRotation
						};
					}
				}
				sample.Bones = array;
			}
			while (actor.Samples.Count < frame)
			{
				actor.Samples.Add(default(DevRecording.Sample));
			}
			if (actor.Samples.Count == frame)
			{
				actor.Samples.Add(sample);
			}
			else
			{
				actor.Samples[frame] = sample;
			}
		}

		private static Animator ResolveAnimator(PlayerMovement player)
		{
			PlayerAnimator componentInChildren = player.GetComponentInChildren<PlayerAnimator>(includeInactive: true);
			if (componentInChildren != null && componentInChildren.Animator != null)
			{
				return componentInChildren.Animator;
			}
			return player.GetComponentInChildren<Animator>(includeInactive: true);
		}

		private static float ReadParameter(Animator animator, string name)
		{
			AnimatorControllerParameter[] parameters = animator.parameters;
			foreach (AnimatorControllerParameter animatorControllerParameter in parameters)
			{
				if (!(animatorControllerParameter.name != name))
				{
					return animatorControllerParameter.type switch
					{
						AnimatorControllerParameterType.Float => animator.GetFloat(name), 
						AnimatorControllerParameterType.Bool => animator.GetBool(name) ? 1f : 0f, 
						AnimatorControllerParameterType.Int => animator.GetInteger(name), 
						_ => 0f, 
					};
				}
			}
			return 0f;
		}

		private static string CurrentMapId()
		{
			LobbySettingsSync lobbySettingsSync = UnityEngine.Object.FindFirstObjectByType<LobbySettingsSync>();
			if (!(lobbySettingsSync != null) || !lobbySettingsSync.IsSpawned)
			{
				return "";
			}
			return lobbySettingsSync.CurrentSettings.MapId.ToString();
		}

		private static string[] CaptureCharacterBoxes(Component player)
		{
			CharacterAssembler componentInChildren = player.GetComponentInChildren<CharacterAssembler>(includeInactive: true);
			if (componentInChildren == null || componentInChildren.Rig == null)
			{
				return Array.Empty<string>();
			}
			List<string> list = new List<string>();
			foreach (CharacterPartDefinition part in componentInChildren.Rig.Parts)
			{
				if (!(part == null) && !string.IsNullOrEmpty(part.PartId))
				{
					Vector3Int boxSize = part.BoxSize;
					list.Add($"{part.PartId} {boxSize.x} {boxSize.y} {boxSize.z}");
				}
			}
			return list.ToArray();
		}
	}
}
