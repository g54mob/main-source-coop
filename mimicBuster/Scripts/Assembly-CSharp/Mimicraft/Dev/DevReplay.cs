using System;
using System.Collections.Generic;
using System.Text;
using Mimicraft.Customization;
using Mimicraft.Gameplay;
using Mimicraft.Networking;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Rendering;

namespace Mimicraft.Dev
{
	public class DevReplay : MonoBehaviour
	{
		private class Ghost
		{
			public DevRecording.Actor Actor;

			public GameObject Root;

			public Animator Animator;

			public PlayerWeapons Weapons;

			public RigBuilder Rig;

			public PlayerAnimator Anim;

			public int ShownArmed;

			public Renderer[] Renderers;

			public bool Shown;

			public int ShownWeapon;

			public Transform[] Bones = Array.Empty<Transform>();

			public HiderLimbs Limbs;

			public Animator[] LimbAnimators = Array.Empty<Animator>();

			public PlayerVoxelBody Body;

			public GameObject CharacterModel;

			public CharacterAssembler Assembler;

			public float FlashOffAt;

			public bool Reconstructing;
		}

		public readonly struct GhostHandle
		{
			public readonly DevRecording.Actor Actor;

			public readonly Transform Root;

			public readonly CharacterRigDefinition Rig;

			public readonly Animator Animator;

			public readonly CharacterAssembler Assembler;

			public GhostHandle(DevRecording.Actor actor, Transform root, CharacterRigDefinition rig, Animator animator, CharacterAssembler assembler)
			{
				Actor = actor;
				Root = root;
				Rig = rig;
				Animator = animator;
				Assembler = assembler;
			}
		}

		private static DevReplay instance;

		private DevRecording recording;

		private readonly List<Ghost> ghosts = new List<Ghost>();

		private float time;

		private float speed = 1f;

		private bool paused;

		private readonly List<Renderer> hiddenLive = new List<Renderer>();

		private int lastImpactFrame = -1;

		private string lastShotReport;

		private int lastBodyFrame = -1;

		public static bool Active => instance != null;

		public static DevReplay Instance => instance;

		public float Time => time;

		public float Duration
		{
			get
			{
				if (recording == null)
				{
					return 0f;
				}
				return recording.Duration;
			}
		}

		public bool Paused
		{
			get
			{
				return paused;
			}
			set
			{
				paused = value;
			}
		}

		public float Speed
		{
			get
			{
				return speed;
			}
			set
			{
				speed = Mathf.Clamp(value, 0.05f, 8f);
			}
		}

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

		public int GhostCount => ghosts.Count;

		public DevRecording Recording => recording;

		public int CurrentFrame
		{
			get
			{
				if (recording == null)
				{
					return 0;
				}
				return Mathf.Clamp(Mathf.FloorToInt(time * recording.TickRate), 0, Mathf.Max(0, recording.FrameCount - 1));
			}
		}

		public IEnumerable<GhostHandle> Ghosts()
		{
			foreach (Ghost ghost in ghosts)
			{
				if (ghost.Root != null)
				{
					yield return new GhostHandle(ghost.Actor, ghost.Root.transform, (ghost.Assembler != null) ? ghost.Assembler.Rig : null, ghost.Animator, ghost.Assembler);
				}
			}
		}

		public void Redress(ulong actorId)
		{
			foreach (Ghost ghost in ghosts)
			{
				if (ghost.Actor.Id == actorId && !(ghost.Root == null))
				{
					Dress(ghost.Root, ghost.Actor);
					Neutralise(ghost.Root);
					ShowEverything(ghost.Root);
					SettleRig(ghost.Root);
					RespectVoxelBody(ghost);
					ghost.ShownWeapon = -1;
					ghost.ShownArmed = -1;
					break;
				}
			}
		}

		public void Invalidate(ulong actorId)
		{
			foreach (Ghost ghost in ghosts)
			{
				if (ghost.Actor.Id == actorId)
				{
					ghost.ShownWeapon = -1;
					ghost.ShownArmed = -1;
					break;
				}
			}
		}

		public static bool Begin(DevRecording recording, out string problem)
		{
			Stop();
			if (recording == null || recording.FrameCount == 0)
			{
				problem = "Kayit bos.";
				return false;
			}
			GameObject gameObject = ResolveGhostPrefab();
			if (gameObject == null)
			{
				problem = "NetworkManager yok - once oyuna gir, sonra `play`.";
				return false;
			}
			GameObject obj = new GameObject("DevReplay");
			UnityEngine.Object.DontDestroyOnLoad(obj);
			instance = obj.AddComponent<DevReplay>();
			instance.recording = recording;
			instance.HideLivePlayers();
			instance.Spawn(gameObject);
			DevGhostInspector.Attach(obj);
			ImpactEffects.ClearAll();
			problem = "";
			return true;
		}

		public static void Stop()
		{
			if (instance != null)
			{
				UnityEngine.Object.Destroy(instance.gameObject);
			}
			instance = null;
			DevBodyReconstruct.ForgetAll();
		}

		public void Seek(float seconds)
		{
			float num = Mathf.Clamp(seconds, 0f, Duration);
			if (num < time)
			{
				ImpactEffects.ClearAll();
			}
			time = num;
		}

		private void OnDestroy()
		{
			foreach (Ghost ghost in ghosts)
			{
				if (ghost.Root != null)
				{
					UnityEngine.Object.Destroy(ghost.Root);
				}
			}
			ShowLivePlayers();
			ImpactEffects.ClearAll();
			if (instance == this)
			{
				instance = null;
			}
		}

		private void HideLivePlayers()
		{
			PlayerMovement[] array = UnityEngine.Object.FindObjectsByType<PlayerMovement>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
			for (int i = 0; i < array.Length; i++)
			{
				Renderer[] componentsInChildren = array[i].GetComponentsInChildren<Renderer>(includeInactive: true);
				foreach (Renderer renderer in componentsInChildren)
				{
					if (renderer.enabled)
					{
						renderer.enabled = false;
						hiddenLive.Add(renderer);
					}
				}
			}
		}

		private void ShowLivePlayers()
		{
			foreach (Renderer item in hiddenLive)
			{
				if (item != null)
				{
					item.enabled = true;
				}
			}
			hiddenLive.Clear();
		}

		private static GameObject ResolveGhostPrefab()
		{
			NetworkManager singleton = NetworkManager.Singleton;
			if (singleton == null || singleton.NetworkConfig == null)
			{
				return null;
			}
			if (!singleton.IsListening)
			{
				LobbyPassword.Host(singleton, "");
				ServerTickRate.ApplyForHosting(singleton);
				if (!singleton.StartHost())
				{
					DevConsole.Log("Host baslatilamadi - `play` icin bir oturum gerekiyor.");
					return null;
				}
				DevConsole.Log("Oturum yoktu - host baslatildi.");
			}
			return singleton.NetworkConfig.PlayerPrefab;
		}

		private void Spawn(GameObject prefab)
		{
			foreach (DevRecording.Actor actor in recording.Actors)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(prefab);
				gameObject.name = $"Ghost_{actor.Id}_{actor.DisplayName}";
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				Neutralise(gameObject);
				Dress(gameObject, actor);
				Neutralise(gameObject);
				ShowEverything(gameObject);
				Animator animator = WakeAnimator(gameObject);
				SettleRig(gameObject);
				Animator[] limbAnimators = WakeLimbs(gameObject);
				PlayerCameraRig component = gameObject.GetComponent<PlayerCameraRig>();
				Ghost ghost = new Ghost
				{
					Actor = actor,
					Root = gameObject,
					Animator = animator,
					Weapons = gameObject.GetComponent<PlayerWeapons>(),
					Rig = gameObject.GetComponentInChildren<RigBuilder>(includeInactive: true),
					Anim = gameObject.GetComponentInChildren<PlayerAnimator>(includeInactive: true),
					ShownArmed = -1,
					Renderers = gameObject.GetComponentsInChildren<Renderer>(includeInactive: true),
					Shown = true,
					ShownWeapon = -1,
					Bones = CollectBones(gameObject),
					Limbs = gameObject.GetComponentInChildren<HiderLimbs>(includeInactive: true),
					LimbAnimators = limbAnimators,
					Body = gameObject.GetComponentInChildren<PlayerVoxelBody>(includeInactive: true),
					Assembler = gameObject.GetComponentInChildren<CharacterAssembler>(includeInactive: true),
					CharacterModel = ((component != null) ? component.CharacterModel : null)
				};
				ghosts.Add(ghost);
				RespectVoxelBody(ghost);
			}
		}

		private static void Neutralise(GameObject root)
		{
			NetworkBehaviour[] componentsInChildren = root.GetComponentsInChildren<NetworkBehaviour>(includeInactive: true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
			}
			MonoBehaviour[] componentsInChildren2 = root.GetComponentsInChildren<MonoBehaviour>(includeInactive: true);
			foreach (MonoBehaviour monoBehaviour in componentsInChildren2)
			{
				if (!(monoBehaviour == null) && !(monoBehaviour is DevReplay))
				{
					string text = monoBehaviour.GetType().Namespace;
					if (text != null && text.StartsWith("Mimicraft", StringComparison.Ordinal))
					{
						monoBehaviour.enabled = false;
					}
				}
			}
			CharacterController[] componentsInChildren3 = root.GetComponentsInChildren<CharacterController>(includeInactive: true);
			for (int i = 0; i < componentsInChildren3.Length; i++)
			{
				componentsInChildren3[i].enabled = false;
			}
			Collider[] componentsInChildren4 = root.GetComponentsInChildren<Collider>(includeInactive: true);
			for (int i = 0; i < componentsInChildren4.Length; i++)
			{
				componentsInChildren4[i].enabled = false;
			}
			Rigidbody[] componentsInChildren5 = root.GetComponentsInChildren<Rigidbody>(includeInactive: true);
			for (int i = 0; i < componentsInChildren5.Length; i++)
			{
				componentsInChildren5[i].isKinematic = true;
			}
			AudioListener[] componentsInChildren6 = root.GetComponentsInChildren<AudioListener>(includeInactive: true);
			for (int i = 0; i < componentsInChildren6.Length; i++)
			{
				componentsInChildren6[i].enabled = false;
			}
			Camera[] componentsInChildren7 = root.GetComponentsInChildren<Camera>(includeInactive: true);
			for (int i = 0; i < componentsInChildren7.Length; i++)
			{
				componentsInChildren7[i].enabled = false;
			}
			NetworkObject component = root.GetComponent<NetworkObject>();
			if (component != null)
			{
				component.enabled = false;
			}
		}

		private static void DressWeapon(GameObject instance, DevRecording.Actor actor)
		{
			if (actor.WeaponSkinBytes.Length != 0)
			{
				WeaponSkinAssembler componentInChildren = instance.GetComponentInChildren<WeaponSkinAssembler>(includeInactive: true);
				if (!(componentInChildren == null) && !string.IsNullOrEmpty(componentInChildren.WeaponId) && WeaponSkinFile.TryDecode(actor.WeaponSkinBytes, out var skins) && skins.TryGetValue(componentInChildren.WeaponId, out var value))
				{
					componentInChildren.Apply(value);
				}
			}
		}

		private static void Dress(GameObject root, DevRecording.Actor actor)
		{
			CharacterAssembler componentInChildren = root.GetComponentInChildren<CharacterAssembler>(includeInactive: true);
			if (componentInChildren != null)
			{
				componentInChildren.enabled = true;
			}
			if (componentInChildren != null && actor.CharacterBytes.Length != 0 && CharacterCodec.TryDecode(actor.CharacterBytes, "", out var character))
			{
				CharacterBoxMigration.Apply(character, BoxesFrom(actor), componentInChildren.Rig);
				componentInChildren.Apply(character);
			}
			if (actor.BodyBytes.Length != 0)
			{
				PlayerVoxelBody componentInChildren2 = root.GetComponentInChildren<PlayerVoxelBody>(includeInactive: true);
				if (componentInChildren2 != null)
				{
					componentInChildren2.DevApplyPayload(actor.BodyBytes);
				}
			}
		}

		private static Dictionary<string, Vector3Int> BoxesFrom(DevRecording.Actor actor)
		{
			if (actor.CharacterBoxes == null || actor.CharacterBoxes.Length == 0)
			{
				return null;
			}
			Dictionary<string, Vector3Int> dictionary = new Dictionary<string, Vector3Int>();
			string[] characterBoxes = actor.CharacterBoxes;
			for (int i = 0; i < characterBoxes.Length; i++)
			{
				string[] array = characterBoxes[i]?.Split(' ');
				if (array != null && array.Length == 4 && int.TryParse(array[1], out var result) && int.TryParse(array[2], out var result2) && int.TryParse(array[3], out var result3))
				{
					dictionary[array[0]] = new Vector3Int(result, result2, result3);
				}
			}
			return dictionary;
		}

		private static Animator WakeAnimator(GameObject root)
		{
			PlayerCameraRig component = root.GetComponent<PlayerCameraRig>();
			if (component != null && component.CharacterModel != null && !component.CharacterModel.activeSelf)
			{
				component.CharacterModel.SetActive(value: true);
			}
			PlayerAnimator componentInChildren = root.GetComponentInChildren<PlayerAnimator>(includeInactive: true);
			Animator animator = ((componentInChildren != null && componentInChildren.Animator != null) ? componentInChildren.Animator : root.GetComponentInChildren<Animator>(includeInactive: true));
			if (animator == null)
			{
				return null;
			}
			if (!animator.gameObject.activeSelf)
			{
				animator.gameObject.SetActive(value: true);
			}
			animator.enabled = true;
			animator.applyRootMotion = false;
			animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
			animator.updateMode = AnimatorUpdateMode.UnscaledTime;
			animator.speed = 1f;
			return animator;
		}

		private static void RespectVoxelBody(Ghost ghost)
		{
			if (!(ghost.Body == null) && !(ghost.CharacterModel == null))
			{
				bool flag = !ghost.Body.HasVoxelBody;
				if (ghost.CharacterModel.activeSelf != flag)
				{
					ghost.CharacterModel.SetActive(flag);
				}
			}
		}

		private static Animator[] WakeLimbs(GameObject root)
		{
			HiderLimbs componentInChildren = root.GetComponentInChildren<HiderLimbs>(includeInactive: true);
			if (componentInChildren == null || componentInChildren.LimbRig == null)
			{
				return Array.Empty<Animator>();
			}
			Animator[] componentsInChildren = componentInChildren.LimbRig.GetComponentsInChildren<Animator>(includeInactive: true);
			Animator[] array = componentsInChildren;
			foreach (Animator obj in array)
			{
				obj.updateMode = AnimatorUpdateMode.UnscaledTime;
				obj.cullingMode = AnimatorCullingMode.AlwaysAnimate;
			}
			return componentsInChildren;
		}

		private static Transform[] CollectBones(GameObject root)
		{
			PlayerRagdoll componentInChildren = root.GetComponentInChildren<PlayerRagdoll>(includeInactive: true);
			if (componentInChildren == null)
			{
				return Array.Empty<Transform>();
			}
			IReadOnlyList<Rigidbody> boneBodies = componentInChildren.BoneBodies;
			Transform[] array = new Transform[boneBodies.Count];
			for (int i = 0; i < boneBodies.Count; i++)
			{
				array[i] = ((boneBodies[i] != null) ? boneBodies[i].transform : null);
			}
			return array;
		}

		private static void ShowEverything(GameObject root)
		{
			Renderer[] componentsInChildren = root.GetComponentsInChildren<Renderer>(includeInactive: true);
			foreach (Renderer renderer in componentsInChildren)
			{
				if (!(renderer == null) && renderer.shadowCastingMode == ShadowCastingMode.ShadowsOnly)
				{
					renderer.shadowCastingMode = ShadowCastingMode.On;
				}
			}
		}

		private static void SettleRig(GameObject root)
		{
			RigBuilder componentInChildren = root.GetComponentInChildren<RigBuilder>(includeInactive: true);
			if (!(componentInChildren == null) && componentInChildren.enabled)
			{
				componentInChildren.Build();
			}
		}

		private static void SetShown(Ghost ghost, bool shown)
		{
			if (ghost.Shown == shown)
			{
				return;
			}
			ghost.Shown = shown;
			Renderer[] renderers = ghost.Renderers;
			foreach (Renderer renderer in renderers)
			{
				if (renderer != null)
				{
					renderer.enabled = shown;
				}
			}
		}

		public string DescribeGhosts()
		{
			StringBuilder stringBuilder = new StringBuilder($"{ghosts.Count} hayalet:");
			foreach (Ghost ghost in ghosts)
			{
				if (ghost.Animator == null)
				{
					stringBuilder.Append($"\n  {ghost.Actor.Id}: Animator YOK");
					continue;
				}
				int num = 0;
				string text = "";
				MonoBehaviour[] componentsInChildren = ghost.Root.GetComponentsInChildren<MonoBehaviour>(includeInactive: true);
				foreach (MonoBehaviour monoBehaviour in componentsInChildren)
				{
					if (monoBehaviour == null || !monoBehaviour.enabled)
					{
						continue;
					}
					string text2 = monoBehaviour.GetType().Namespace;
					if (text2 != null && text2.StartsWith("Mimicraft", StringComparison.Ordinal))
					{
						num++;
						if (num <= 6)
						{
							text = text + ((text.Length > 0) ? ", " : "") + monoBehaviour.GetType().Name;
						}
					}
				}
				int num2 = 0;
				int num3 = 0;
				Renderer[] componentsInChildren2 = ghost.Root.GetComponentsInChildren<Renderer>(includeInactive: true);
				foreach (Renderer renderer in componentsInChildren2)
				{
					if (!(renderer == null))
					{
						if (renderer.shadowCastingMode == ShadowCastingMode.ShadowsOnly)
						{
							num2++;
						}
						if (!renderer.enabled)
						{
							num3++;
						}
					}
				}
				stringBuilder.Append($"\n  {ghost.Actor.Id}: konum {ghost.Root.transform.position}" + string.Format(" acik-bizim-bilesen={0}{1}", num, (num > 0) ? (" (" + text + ")") : "") + $" golge-only={num2} kapali-renderer={num3}" + $" renderer={ghost.Renderers.Length}");
				stringBuilder.Append($"\n  {ghost.Actor.Id}: controller=" + ((ghost.Animator.runtimeAnimatorController != null) ? ghost.Animator.runtimeAnimatorController.name : "YOK") + $" enabled={ghost.Animator.enabled}" + $" aktif={ghost.Animator.gameObject.activeInHierarchy}" + $" hiz={ghost.Animator.speed:0.##}" + $" mod={ghost.Animator.updateMode}" + $" kayitli={ghost.Actor.Samples.Count} kare");
				if (ghost.Animator.runtimeAnimatorController != null && ghost.Animator.layerCount > 0)
				{
					AnimatorStateInfo currentAnimatorStateInfo = ghost.Animator.GetCurrentAnimatorStateInfo(0);
					stringBuilder.Append($"\n      state={currentAnimatorStateInfo.fullPathHash} t={currentAnimatorStateInfo.normalizedTime:0.###}" + " rig=" + ((!(ghost.Rig != null)) ? "YOK" : (ghost.Rig.enabled ? "acik" : "kapali")));
				}
				DevRecording.Sample sample = SampleAt(ghost.Actor, Mathf.FloorToInt(time * recording.TickRate));
				if (sample.Layers != null)
				{
					for (int j = 0; j < sample.Layers.Length && j < ghost.Animator.layerCount; j++)
					{
						stringBuilder.Append($"\n      katman {j} '{ghost.Animator.GetLayerName(j)}': " + $"agirlik kayit={sample.Layers[j].Weight:0.##} " + $"canli={ghost.Animator.GetLayerWeight(j):0.##} " + $"hash kayit={sample.Layers[j].Hash} " + $"canli={ghost.Animator.GetCurrentAnimatorStateInfo(j).fullPathHash}");
					}
				}
				stringBuilder.Append("\n      armed kayit=" + ArmedIn(ghost.Actor, sample) + " gosterilen=" + ((ghost.ShownArmed < 0) ? "hic" : ghost.ShownArmed.ToString()) + " builder=" + ((!(ghost.Rig != null)) ? "YOK" : (ghost.Rig.enabled ? "acik" : "kapali")));
				if (ghost.Rig != null)
				{
					foreach (RigLayer layer in ghost.Rig.layers)
					{
						if (layer != null && layer.rig != null)
						{
							stringBuilder.Append($"\n        rig '{layer.rig.name}' = {layer.rig.weight:0.##}");
						}
					}
				}
				for (int k = 0; k < ghost.Actor.Parameters.Length; k++)
				{
					string arg = ghost.Actor.Parameters[k];
					float num4 = ((sample.Parameters != null && k < sample.Parameters.Length) ? sample.Parameters[k] : 0f);
					stringBuilder.Append($"\n      {arg} = {num4:0.##} (animator: {ReadBack(ghost.Animator, arg)})");
				}
			}
			return stringBuilder.ToString();
		}

		private static string ArmedIn(DevRecording.Actor actor, DevRecording.Sample sample)
		{
			if (sample.Parameters == null)
			{
				return "kare yok";
			}
			for (int i = 0; i < actor.Parameters.Length && i < sample.Parameters.Length; i++)
			{
				if (actor.Parameters[i] == "Armed")
				{
					return sample.Parameters[i].ToString("0.##");
				}
			}
			return "yok";
		}

		private static string ReadBack(Animator animator, string name)
		{
			AnimatorControllerParameter[] parameters = animator.parameters;
			foreach (AnimatorControllerParameter animatorControllerParameter in parameters)
			{
				if (!(animatorControllerParameter.name != name))
				{
					return animatorControllerParameter.type switch
					{
						AnimatorControllerParameterType.Float => animator.GetFloat(name).ToString("0.##"), 
						AnimatorControllerParameterType.Bool => animator.GetBool(name).ToString(), 
						AnimatorControllerParameterType.Int => animator.GetInteger(name).ToString(), 
						_ => "-", 
					};
				}
			}
			return "YOK";
		}

		private void Update()
		{
			if (recording == null)
			{
				return;
			}
			if (!paused)
			{
				float num = Mathf.Repeat(time + UnityEngine.Time.unscaledDeltaTime * speed, Mathf.Max(0.001f, Duration));
				if (num < time)
				{
					ImpactEffects.ClearAll();
				}
				time = num;
			}
			ApplyBodies();
			ApplyReconstructs();
			Apply(parametersOnly: true);
			PlayImpacts();
			TickFlashes();
		}

		private void LateUpdate()
		{
			if (recording != null)
			{
				Apply(parametersOnly: false);
				DevFreeCamera.FollowNow();
			}
		}

		private void Apply(bool parametersOnly)
		{
			float num = time * recording.TickRate;
			int num2 = Mathf.FloorToInt(num);
			float num3 = num - (float)num2;
			foreach (Ghost ghost in ghosts)
			{
				DevRecording.Sample sample = SampleAt(ghost.Actor, num2);
				DevRecording.Sample b = SampleAt(ghost.Actor, num2 + 1);
				if (!sample.Present)
				{
					SetShown(ghost, shown: false);
					continue;
				}
				SetShown(ghost, shown: true);
				if (parametersOnly)
				{
					ApplyParameters(ghost, sample);
					ApplyArmed(ghost, sample);
					ApplyLayers(ghost, sample, b, num3);
					ApplyClock(ghost, sample);
					if (ghost.Limbs != null)
					{
						ghost.Limbs.DevTick(sample.Running);
					}
					RespectVoxelBody(ghost);
				}
				else
				{
					Vector3 position = (b.Present ? Vector3.Lerp(sample.Position, b.Position, num3) : sample.Position);
					Quaternion rotation = (b.Present ? Quaternion.Slerp(sample.Rotation, b.Rotation, num3) : sample.Rotation);
					ghost.Root.transform.SetPositionAndRotation(position, rotation);
					ApplyWeapon(ghost, sample);
					ApplyBones(ghost, sample);
					if (ghost.Weapons != null)
					{
						ghost.Weapons.DevFollowGrips();
					}
				}
			}
		}

		private void PlayImpacts()
		{
			if (paused || recording.Events.Count == 0)
			{
				return;
			}
			int num = Mathf.FloorToInt(time * recording.TickRate);
			if (num < lastImpactFrame)
			{
				lastImpactFrame = num;
				return;
			}
			foreach (DevRecording.ImpactEvent @event in recording.Events)
			{
				if (@event.Frame > lastImpactFrame && @event.Frame <= num)
				{
					if (@event.Kind == 200)
					{
						PlayShot(@event);
					}
					else
					{
						ImpactEffects.Replay((ImpactKind)@event.Kind, @event.Position, @event.Normal);
					}
				}
			}
			lastImpactFrame = num;
		}

		private void PlayShot(DevRecording.ImpactEvent impact)
		{
			WeaponDefinition weapon = recording.WeaponAt(impact.Weapon);
			ImpactEffects.SpawnShotSound(impact.Position, weapon);
			Ghost ghost = null;
			foreach (Ghost ghost2 in ghosts)
			{
				if (ghost2.Actor.Id == impact.Actor)
				{
					ghost = ghost2;
					break;
				}
			}
			if (ghost == null)
			{
				ReportShot($"atis {impact.Actor} numarali oyuncunun ama o id'de hayalet yok.");
				return;
			}
			if (ghost.Weapons == null)
			{
				ReportShot($"hayalet {impact.Actor} uzerinde PlayerWeapons yok.");
				return;
			}
			if (ghost.Weapons.TpsVisual == null)
			{
				ReportShot($"hayalet {impact.Actor} ates etti ama elinde TPS silahi yok.");
				return;
			}
			if (!ghost.Weapons.TpsVisual.HasMuzzleFlash)
			{
				ReportShot("'" + ghost.Weapons.TpsVisual.name + "' uzerinde Muzzle Flash objesi atanmamis.");
				return;
			}
			ghost.Weapons.TpsVisual.PlayMuzzleFlash();
			ghost.FlashOffAt = UnityEngine.Time.unscaledTime + ghost.Weapons.TpsVisual.MuzzleFlashLifetime;
		}

		private void ReportShot(string message)
		{
			if (!(message == lastShotReport))
			{
				lastShotReport = message;
				DevConsole.Log("[play] " + message);
			}
		}

		private void TickFlashes()
		{
			foreach (Ghost ghost in ghosts)
			{
				if (!(ghost.FlashOffAt <= 0f) && !(UnityEngine.Time.unscaledTime < ghost.FlashOffAt))
				{
					ghost.FlashOffAt = 0f;
					if (ghost.Weapons != null && ghost.Weapons.TpsVisual != null)
					{
						ghost.Weapons.TpsVisual.StopMuzzleFlash();
					}
				}
			}
		}

		private void ApplyBodies()
		{
			if (recording.BodyChanges.Count == 0)
			{
				return;
			}
			int num = Mathf.FloorToInt(time * recording.TickRate);
			if (num == lastBodyFrame)
			{
				return;
			}
			int num2 = ((num < lastBodyFrame) ? (-1) : lastBodyFrame);
			lastBodyFrame = num;
			foreach (DevRecording.BodyChange bodyChange in recording.BodyChanges)
			{
				if (bodyChange.Frame <= num2 || bodyChange.Frame > num)
				{
					continue;
				}
				foreach (Ghost ghost in ghosts)
				{
					if (ghost.Actor.Id == bodyChange.Actor)
					{
						PlayerVoxelBody componentInChildren = ghost.Root.GetComponentInChildren<PlayerVoxelBody>(includeInactive: true);
						if (componentInChildren != null && bodyChange.Bytes.Length != 0)
						{
							componentInChildren.DevApplyPayload(bodyChange.Bytes);
						}
						break;
					}
				}
			}
		}

		private void ApplyReconstructs()
		{
			if (recording.Reconstructs.Count == 0)
			{
				return;
			}
			foreach (Ghost ghost in ghosts)
			{
				if (ghost.Root == null)
				{
					continue;
				}
				if (!DevBodyReconstruct.TryProgressAt(recording, ghost.Actor.Id, time, out var progress, out var startFrame))
				{
					if (!ghost.Reconstructing)
					{
						continue;
					}
					ghost.Reconstructing = false;
					DevBodyReconstruct.Rewind(ghost.Actor.Id);
					if (ghost.Body != null)
					{
						byte[] array = DevBodyReconstruct.TargetFor(recording, ghost.Actor, CurrentFrame);
						if (array != null && array.Length != 0)
						{
							ghost.Body.DevApplyPayload(array);
						}
					}
					lastBodyFrame = -1;
				}
				else
				{
					if (ghost.Body == null)
					{
						ghost.Body = ghost.Root.GetComponentInChildren<PlayerVoxelBody>(includeInactive: true);
					}
					if (!(ghost.Body == null))
					{
						ghost.Reconstructing = true;
						byte[] target = DevBodyReconstruct.TargetFor(recording, ghost.Actor, startFrame);
						DevBodyReconstruct.Apply(ghost.Body, ghost.Actor.Id, target, progress);
					}
				}
			}
		}

		private static DevRecording.Sample SampleAt(DevRecording.Actor actor, int frame)
		{
			if (frame < 0 || frame >= actor.Samples.Count)
			{
				return default(DevRecording.Sample);
			}
			return actor.Samples[frame];
		}

		private void ApplyWeapon(Ghost ghost, DevRecording.Sample sample)
		{
			if (!(ghost.Weapons == null) && sample.Weapon != ghost.ShownWeapon)
			{
				ghost.ShownWeapon = sample.Weapon;
				ghost.Weapons.DevShowWeapon(recording.WeaponAt(sample.Weapon));
				if (ghost.Weapons.ThirdPersonInstance != null)
				{
					DressWeapon(ghost.Weapons.ThirdPersonInstance, ghost.Actor);
					Neutralise(ghost.Weapons.ThirdPersonInstance);
					ShowEverything(ghost.Weapons.ThirdPersonInstance);
				}
				if (ghost.Anim != null)
				{
					ghost.Anim.SetCustomWeaponHeld(ghost.Weapons.ThirdPersonInstance != null);
				}
			}
		}

		private static void ApplyBones(Ghost ghost, DevRecording.Sample sample)
		{
			if (sample.Bones == null || sample.Bones.Length != ghost.Bones.Length)
			{
				return;
			}
			for (int i = 0; i < ghost.Bones.Length; i++)
			{
				if (ghost.Bones[i] != null)
				{
					ghost.Bones[i].SetLocalPositionAndRotation(sample.Bones[i].Position, sample.Bones[i].Rotation);
				}
			}
		}

		private void ApplyClock(Ghost ghost, DevRecording.Sample sample)
		{
			float num = (paused ? 0f : speed);
			if (ghost.Animator != null)
			{
				float b = ((sample.Layers != null && sample.Layers.Length != 0 && sample.Layers[0].Hash != 0) ? 1f : num);
				if (!Mathf.Approximately(ghost.Animator.speed, b))
				{
					ghost.Animator.speed = b;
				}
			}
			Animator[] limbAnimators = ghost.LimbAnimators;
			foreach (Animator animator in limbAnimators)
			{
				if (animator != null && !Mathf.Approximately(animator.speed, num))
				{
					animator.speed = num;
				}
			}
		}

		private static void ApplyLayers(Ghost ghost, DevRecording.Sample a, DevRecording.Sample b, float blend)
		{
			if (ghost.Animator == null || a.Layers == null)
			{
				return;
			}
			for (int i = 0; i < a.Layers.Length && i < ghost.Animator.layerCount; i++)
			{
				DevRecording.LayerState layerState = a.Layers[i];
				if (i > 0)
				{
					ghost.Animator.SetLayerWeight(i, layerState.Weight);
				}
				if (layerState.Hash == 0)
				{
					continue;
				}
				float num = layerState.NormalizedTime;
				if (b.Present && b.Layers != null && i < b.Layers.Length && b.Layers[i].Hash == layerState.Hash)
				{
					float num2 = b.Layers[i].NormalizedTime;
					if (num2 < num)
					{
						num2 += 1f;
					}
					num = Mathf.Lerp(num, num2, blend);
				}
				ghost.Animator.Play(layerState.Hash, i, num);
			}
		}

		private static void ApplyArmed(Ghost ghost, DevRecording.Sample sample)
		{
			if (ghost.Anim == null || sample.Parameters == null)
			{
				return;
			}
			for (int i = 0; i < ghost.Actor.Parameters.Length && i < sample.Parameters.Length; i++)
			{
				if (!(ghost.Actor.Parameters[i] != "Armed"))
				{
					int num = ((sample.Parameters[i] > 0.5f) ? 1 : 0);
					if (num != ghost.ShownArmed)
					{
						ghost.ShownArmed = num;
						ghost.Anim.SetArmed(num == 1);
					}
					break;
				}
			}
		}

		private static void ApplyParameters(Ghost ghost, DevRecording.Sample sample)
		{
			if (ghost.Animator == null || sample.Parameters == null)
			{
				return;
			}
			for (int i = 0; i < ghost.Actor.Parameters.Length && i < sample.Parameters.Length; i++)
			{
				string text = ghost.Actor.Parameters[i];
				float num = sample.Parameters[i];
				AnimatorControllerParameter[] parameters = ghost.Animator.parameters;
				foreach (AnimatorControllerParameter animatorControllerParameter in parameters)
				{
					if (!(animatorControllerParameter.name != text))
					{
						switch (animatorControllerParameter.type)
						{
						case AnimatorControllerParameterType.Float:
							ghost.Animator.SetFloat(text, num);
							break;
						case AnimatorControllerParameterType.Bool:
							ghost.Animator.SetBool(text, num > 0.5f);
							break;
						case AnimatorControllerParameterType.Int:
							ghost.Animator.SetInteger(text, Mathf.RoundToInt(num));
							break;
						}
						break;
					}
				}
			}
		}
	}
}
