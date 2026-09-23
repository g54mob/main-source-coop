using System.Collections.Generic;
using System.Text;
using Mimicraft.Networking;
using Mimicraft.UI;
using Mimicraft.VoxelEditor;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;

namespace Mimicraft.Gameplay
{
	public class RoundCinematicDirector : MonoBehaviour
	{
		private readonly struct Shot
		{
			public readonly float Yaw;

			public readonly float Pitch;

			public readonly float Distance;

			public Shot(float yaw, float pitch, float distance)
			{
				Yaw = yaw;
				Pitch = pitch;
				Distance = distance;
			}
		}

		[Tooltip("Left empty, found in the scene.")]
		[SerializeField]
		private RoundManager roundManager;

		[Tooltip("Left empty, ScreenFadeView.Instance is used. Without one the cut back to the player's own camera is a hard jump instead of a fade.")]
		[SerializeField]
		private ScreenFadeView screenFade;

		[Header("Timing")]
		[Tooltip("Seconds spent travelling to each target. MUST match RoundManager.CinematicFlySeconds - the server sizes the phase with that number and this walks the same schedule.")]
		[SerializeField]
		private float flySeconds = 0.6f;

		[Tooltip("Seconds orbiting each target. MUST match RoundManager.CinematicOrbitSeconds.")]
		[SerializeField]
		private float orbitSeconds = 1.5f;

		[Header("Framing")]
		[Tooltip("Kameranın Modelci'den uzaklığı. Sabit - duvar için içeri çekilmez; bkz. GetOrbitPose.")]
		[SerializeField]
		private float distance = 4f;

		[Tooltip("Camera height above the target's pivot.")]
		[SerializeField]
		private float height = 1.5f;

		[Tooltip("How far around the target the dolly sweeps, in degrees.")]
		[SerializeField]
		private float orbitDegrees = 10f;

		[Tooltip("Length of the fade to black at the very end.")]
		[SerializeField]
		private float fadeSeconds = 0.5f;

		[Header("Modelci konturu")]
		[Tooltip("Sinematik boyunca Modelcilerin etrafına çizilen kontur rengi. HDR - 1'in üstüne çıkarmak bloom varsa parlamayı sağlar, yani 'emission' bu.")]
		[ColorUsage(false, true)]
		[SerializeField]
		private Color outlineColor = new Color(1.6f, 1.3f, 0.25f);

		[Tooltip("Konturun kalınlığı, modelin kendi boyutunun oranı olarak. 0.03 = %3 daha büyük bir kopya. 0 konturu tamamen kapatır.")]
		[SerializeField]
		[Range(0f, 0.2f)]
		private float outlineThickness = 0.035f;

		private readonly List<Transform> targets = new List<Transform>();

		private readonly List<GameObject> outlineHulls = new List<GameObject>();

		private Material outlineMaterial;

		private Transform cameraTransform;

		private readonly List<Behaviour> suspended = new List<Behaviour>();

		private readonly List<Renderer> hiddenRenderers = new List<Renderer>();

		private Vector3 startPosition;

		private Quaternion startRotation;

		private bool running;

		private Transform debugTarget;

		private double debugUntil;

		private readonly Dictionary<int, Shot> shots = new Dictionary<int, Shot>();

		private const float CandidateStepDegrees = 15f;

		private const float MinDistance = 1.6f;

		private const float WallMargin = 0.4f;

		private const float CameraClearance = 0.25f;

		public static bool IsPlaying { get; private set; }

		private static int ShotMask => -1957;

		private void Awake()
		{
			if (roundManager == null)
			{
				roundManager = Object.FindFirstObjectByType<RoundManager>();
			}
		}

		private void OnDisable()
		{
			Stop();
		}

		private void LateUpdate()
		{
			if (debugUntil > 0.0)
			{
				DriveDebug();
			}
			else
			{
				if (roundManager == null || !roundManager.IsSpawned)
				{
					return;
				}
				double time = roundManager.NetworkManager.ServerTime.Time;
				double value = roundManager.CinematicEndServerTime.Value;
				bool flag = roundManager.CurrentPhase.Value == RoundPhase.RoundEnd && time < value;
				if (flag && !running)
				{
					Begin(value);
				}
				if (running)
				{
					if (!flag)
					{
						Stop();
					}
					else
					{
						Drive(time, value);
					}
				}
			}
		}

		private void Begin(double end)
		{
			CollectTargets();
			if (targets.Count == 0)
			{
				return;
			}
			Camera main = Camera.main;
			if (main == null)
			{
				return;
			}
			cameraTransform = main.transform;
			startPosition = cameraTransform.position;
			startRotation = cameraTransform.rotation;
			suspended.Clear();
			Behaviour[] components = cameraTransform.GetComponents<Behaviour>();
			foreach (Behaviour behaviour in components)
			{
				if (!(behaviour is Camera) && !(behaviour is AudioListener) && behaviour.enabled)
				{
					behaviour.enabled = false;
					suspended.Add(behaviour);
				}
			}
			hiddenRenderers.Clear();
			Renderer[] componentsInChildren = cameraTransform.GetComponentsInChildren<Renderer>(includeInactive: true);
			foreach (Renderer renderer in componentsInChildren)
			{
				if (renderer.enabled)
				{
					renderer.enabled = false;
					hiddenRenderers.Add(renderer);
				}
			}
			ApplyOutlines();
			running = true;
			IsPlaying = true;
			AudioLibrary.PlayOneShotClip((AudioLibrary.Instance != null) ? AudioLibrary.Instance.cinematicInClip : null);
		}

		private void ApplyOutlines()
		{
			if (outlineThickness <= 0.0001f)
			{
				return;
			}
			foreach (Transform target in targets)
			{
				if (!(target == null))
				{
					VoxelModel[] componentsInChildren = target.GetComponentsInChildren<VoxelModel>(includeInactive: false);
					foreach (VoxelModel piece in componentsInChildren)
					{
						AddHull(piece);
					}
				}
			}
			Debug.Log($"[RoundCinematicDirector] Kontur: {outlineHulls.Count} parca, {targets.Count} " + $"Modelci, kalinlik {outlineThickness}, renk {outlineColor}.", this);
		}

		private void AddHull(VoxelModel piece)
		{
			MeshFilter component = piece.GetComponent<MeshFilter>();
			if (!(component == null) && !(component.sharedMesh == null))
			{
				Material material = OutlineMaterial();
				if (!(material == null))
				{
					GameObject gameObject = new GameObject("CinematicOutline");
					gameObject.transform.SetParent(piece.transform, worldPositionStays: false);
					gameObject.layer = piece.gameObject.layer;
					gameObject.AddComponent<MeshFilter>().sharedMesh = component.sharedMesh;
					MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
					meshRenderer.sharedMaterial = material;
					meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
					meshRenderer.receiveShadows = false;
					float num = 1f + outlineThickness;
					Vector3 center = component.sharedMesh.bounds.center;
					gameObject.transform.localScale = Vector3.one * num;
					gameObject.transform.localPosition = center * (1f - num);
					outlineHulls.Add(gameObject);
				}
			}
		}

		private Material OutlineMaterial()
		{
			if (outlineMaterial != null)
			{
				return outlineMaterial;
			}
			Shader shader = Shader.Find("Mimicraft/OutlineHull");
			if (shader == null)
			{
				Debug.LogWarning("[RoundCinematicDirector] 'Mimicraft/OutlineHull' bulunamadi - Modelciler konturusuz gosterilecek.", this);
				return null;
			}
			outlineMaterial = new Material(shader);
			outlineMaterial.SetColor("_Color", outlineColor);
			return outlineMaterial;
		}

		private void ClearOutlines()
		{
			foreach (GameObject outlineHull in outlineHulls)
			{
				if (outlineHull != null)
				{
					Object.Destroy(outlineHull);
				}
			}
			outlineHulls.Clear();
		}

		private void Stop()
		{
			if (!running)
			{
				return;
			}
			running = false;
			IsPlaying = false;
			debugTarget = null;
			debugUntil = 0.0;
			AudioLibrary.PlayOneShotClip((AudioLibrary.Instance != null) ? AudioLibrary.Instance.cinematicOutClip : null);
			foreach (Behaviour item in suspended)
			{
				if (item != null)
				{
					item.enabled = true;
				}
			}
			suspended.Clear();
			foreach (Renderer hiddenRenderer in hiddenRenderers)
			{
				if (hiddenRenderer != null)
				{
					hiddenRenderer.enabled = true;
				}
			}
			hiddenRenderers.Clear();
			ClearOutlines();
			shots.Clear();
			Fade()?.FadeTo(0f, fadeSeconds);
		}

		private void CollectTargets()
		{
			targets.Clear();
			if (debugTarget != null)
			{
				targets.Add(debugTarget);
			}
			else
			{
				if (roundManager == null)
				{
					return;
				}
				NetworkManager networkManager = roundManager.NetworkManager;
				if (networkManager == null || networkManager.SpawnManager == null)
				{
					return;
				}
				foreach (PlayerRevealEntry reveal in roundManager.Reveals)
				{
					if (reveal.Role != PlayerRole.Hider || reveal.Health <= 0 || networkManager.SpawnManager.SpawnedObjects == null)
					{
						continue;
					}
					foreach (KeyValuePair<ulong, NetworkObject> spawnedObject in networkManager.SpawnManager.SpawnedObjects)
					{
						if (spawnedObject.Value != null && spawnedObject.Value.IsPlayerObject && spawnedObject.Value.OwnerClientId == reveal.ClientId)
						{
							targets.Add(spawnedObject.Value.transform);
							break;
						}
					}
				}
			}
		}

		private void Drive(double now, double end)
		{
			double num = (double)(flySeconds + (float)targets.Count * (flySeconds + orbitSeconds)) - (end - now);
			if (num < 0.0)
			{
				num = 0.0;
			}
			if (num < (double)flySeconds)
			{
				float t = Mathf.SmoothStep(0f, 1f, (float)(num / (double)flySeconds));
				ApproachTarget(0, t, 0f);
				return;
			}
			double num2 = num - (double)flySeconds;
			double num3 = orbitSeconds + flySeconds;
			int num4 = Mathf.Clamp((int)(num2 / num3), 0, targets.Count - 1);
			double num5 = num2 - (double)num4 * num3;
			if (num5 < (double)orbitSeconds)
			{
				Orbit(num4, (float)(num5 / (double)orbitSeconds));
			}
			else if (num4 + 1 < targets.Count)
			{
				float t2 = Mathf.SmoothStep(0f, 1f, (float)((num5 - (double)orbitSeconds) / (double)flySeconds));
				ApproachTarget(num4 + 1, t2, 1f);
			}
			else
			{
				Orbit(num4, 1f);
			}
			double num6 = end - now;
			if (num6 <= (double)fadeSeconds)
			{
				Fade()?.FadeTo(1f, Mathf.Max((float)num6, 0.01f));
			}
		}

		private void ApproachTarget(int index, float t, float fromOrbit)
		{
			GetOrbitPose(index, 0f, out var position, out var rotation);
			Vector3 position2 = startPosition;
			Quaternion rotation2 = startRotation;
			if (fromOrbit > 0.5f && index > 0)
			{
				GetOrbitPose(index - 1, 1f, out position2, out rotation2);
			}
			cameraTransform.SetPositionAndRotation(Vector3.Lerp(position2, position, t), Quaternion.Slerp(rotation2, rotation, t));
		}

		private void Orbit(int index, float t)
		{
			GetOrbitPose(index, t, out var position, out var rotation);
			cameraTransform.SetPositionAndRotation(position, rotation);
		}

		private void GetOrbitPose(int index, float t, out Vector3 position, out Quaternion rotation)
		{
			int num = Mathf.Clamp(index, 0, targets.Count - 1);
			Transform transform = targets[num];
			if (transform == null)
			{
				position = startPosition;
				rotation = startRotation;
				return;
			}
			Vector3 vector = FocusPoint(transform);
			float num2 = ((num % 2 == 0) ? 1f : (-1f)) * orbitDegrees;
			Shot shot = ResolveShot(num, vector);
			Vector3 vector2 = Direction(shot.Yaw + Mathf.Lerp((0f - num2) * 0.5f, num2 * 0.5f, t), shot.Pitch);
			position = vector + vector2 * shot.Distance;
			rotation = Quaternion.LookRotation(vector - position, Vector3.up);
		}

		private Vector3 FocusPoint(Transform target)
		{
			PlayerVoxelBody component = target.GetComponent<PlayerVoxelBody>();
			if (component != null && component.TryGetBodyCenterWorld(out var center))
			{
				return center;
			}
			return target.position + Vector3.up * height;
		}

		private Shot ResolveShot(int index, Vector3 focus)
		{
			if (shots.TryGetValue(index, out var value))
			{
				return value;
			}
			float num = (float)index * 137.50777f;
			float sweepDegrees = Mathf.Abs(orbitDegrees);
			float yaw = num;
			float num2 = -1f;
			int num3 = Mathf.CeilToInt(12f);
			for (int i = 0; i <= num3; i++)
			{
				for (int num4 = 1; num4 >= -1; num4 -= 2)
				{
					if (i != 0 || num4 >= 0)
					{
						float num5 = num + (float)(num4 * i) * 15f;
						if (SweepRoom(focus, num5, sweepDegrees, distance, out var room))
						{
							Shot shot = new Shot(num5, 0f, distance);
							shots[index] = shot;
							return shot;
						}
						if (room > num2)
						{
							num2 = room;
							yaw = num5;
						}
					}
				}
			}
			float num6 = num2 - 0.4f;
			if (num6 >= 1.6f)
			{
				Shot shot2 = new Shot(yaw, 0f, Mathf.Min(num6, distance));
				shots[index] = shot2;
				return shot2;
			}
			float a = distance;
			if (Physics.Raycast(focus, Vector3.up, out var hitInfo, distance, ShotMask, QueryTriggerInteraction.Ignore))
			{
				a = hitInfo.distance - 0.4f;
			}
			Shot shot3 = new Shot(yaw, 70f, Mathf.Max(1.6f, Mathf.Min(a, distance)));
			shots[index] = shot3;
			return shot3;
		}

		private bool SweepRoom(Vector3 focus, float yaw, float sweepDegrees, float range, out float room)
		{
			room = range;
			bool result = true;
			for (int i = 0; i <= 2; i++)
			{
				Vector3 vector = Direction(yaw + Mathf.Lerp((0f - sweepDegrees) * 0.5f, sweepDegrees * 0.5f, (float)i * 0.5f), 0f);
				Vector3 position = focus + vector * range;
				float num = range;
				if (Physics.Raycast(focus, vector, out var hitInfo, range + 0.25f, ShotMask, QueryTriggerInteraction.Ignore))
				{
					result = false;
					num = Mathf.Min(hitInfo.distance, range);
				}
				if (Physics.CheckSphere(position, 0.25f, ShotMask, QueryTriggerInteraction.Ignore))
				{
					result = false;
				}
				if (num < room)
				{
					room = num;
				}
			}
			return result;
		}

		private static Vector3 Direction(float yaw, float pitch)
		{
			return Quaternion.Euler(0f - pitch, yaw, 0f) * Vector3.forward;
		}

		private ScreenFadeView Fade()
		{
			if (!(screenFade != null))
			{
				return ScreenFadeView.Instance;
			}
			return screenFade;
		}

		public static string DebugPlay(Transform target, bool probeOnly)
		{
			if (target == null)
			{
				return "cinematic: oynatilacak bir govde yok.";
			}
			RoundCinematicDirector roundCinematicDirector = Object.FindFirstObjectByType<RoundCinematicDirector>(FindObjectsInactive.Exclude);
			if (roundCinematicDirector == null)
			{
				GameObject obj = new GameObject("RoundCinematicDirector");
				Object.DontDestroyOnLoad(obj);
				roundCinematicDirector = obj.AddComponent<RoundCinematicDirector>();
			}
			if (roundCinematicDirector.running)
			{
				return "cinematic: zaten oynuyor.";
			}
			string text = roundCinematicDirector.Probe(target);
			if (probeOnly)
			{
				return text;
			}
			roundCinematicDirector.debugTarget = target;
			double num = roundCinematicDirector.flySeconds + 1f * (roundCinematicDirector.flySeconds + roundCinematicDirector.orbitSeconds);
			roundCinematicDirector.debugUntil = Time.timeAsDouble + num;
			return text + $"\ncinematic: {num:0.0} saniye oynatiliyor.";
		}

		private void DriveDebug()
		{
			double timeAsDouble = Time.timeAsDouble;
			if (!running)
			{
				Begin(debugUntil);
				if (!running)
				{
					debugTarget = null;
					debugUntil = 0.0;
					return;
				}
			}
			if (timeAsDouble >= debugUntil)
			{
				Stop();
			}
			else
			{
				Drive(timeAsDouble, debugUntil);
			}
		}

		public string Probe(Transform target)
		{
			Vector3 vector = FocusPoint(target);
			float radius = ProbeRadius();
			int num = -1957;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("[Cinematic] govde '").Append(target.name).Append("' odak=")
				.Append(Fmt(vector))
				.Append(" mesafe=")
				.Append(distance)
				.Append(" yukseklik=")
				.Append(height)
				.Append(" sonda-yaricap=")
				.Append(radius.ToString("0.00"))
				.Append(" maske=")
				.Append(num);
			PlayerVoxelBody component = target.GetComponent<PlayerVoxelBody>();
			stringBuilder.Append("\n  govde: ").Append((component != null) ? "PlayerVoxelBody var" : "PlayerVoxelBody YOK (odak = pozisyon + yukseklik)");
			shots.Remove(0);
			Shot shot = ResolveShot(0, vector);
			shots.Remove(0);
			string value = ((shot.Pitch > 1f) ? "yukaridan (3. kademe)" : ((shot.Distance < distance - 0.01f) ? "iceri cekilmis (2. kademe)" : "tam mesafe (1. kademe)"));
			float num2 = orbitDegrees;
			stringBuilder.Append("\n  SECILEN CEKIM: ").Append(value).Append(" yaw=")
				.Append(shot.Yaw.ToString("0.0"))
				.Append(" pitch=")
				.Append(shot.Pitch.ToString("0.0"))
				.Append(" mesafe=")
				.Append(shot.Distance.ToString("0.00"))
				.Append(" (+- ")
				.Append((num2 * 0.5f).ToString("0.0"))
				.Append(" yay):");
			for (int i = 0; i <= 4; i++)
			{
				float t = (float)i / 4f;
				float yaw = shot.Yaw + Mathf.Lerp((0f - num2) * 0.5f, num2 * 0.5f, t);
				Vector3 position = vector + Direction(yaw, shot.Pitch) * shot.Distance;
				stringBuilder.Append("\n    t=").Append(t.ToString("0.00")).Append(' ')
					.Append(DescribePoint(position, vector, radius, num));
			}
			stringBuilder.Append("\n  PUSULA (").Append(distance).Append("m, 30 derecede bir):");
			int num3 = 0;
			for (int j = 0; j < 12; j++)
			{
				float angle = (float)j * 30f;
				Vector3 vector2 = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward;
				Vector3 vector3 = vector + vector2 * distance;
				bool num4 = Physics.CheckSphere(vector3, radius, num, QueryTriggerInteraction.Ignore);
				RaycastHit hitInfo;
				bool flag = Physics.Linecast(vector3, vector, out hitInfo, num, QueryTriggerInteraction.Ignore);
				if (!num4 && !flag)
				{
					num3++;
				}
				stringBuilder.Append("\n    ").Append(angle.ToString("000")).Append(" derece ")
					.Append(DescribePoint(vector3, vector, radius, num));
			}
			stringBuilder.Append("\n  temiz yon sayisi: ").Append(num3).Append(" / 12");
			RaycastHit hitInfo2;
			bool flag2 = Physics.Raycast(vector, Vector3.up, out hitInfo2, distance + 2f, num, QueryTriggerInteraction.Ignore);
			stringBuilder.Append("\n  tavan: ").Append(flag2 ? $"{hitInfo2.distance:0.00}m ('{hitInfo2.collider.name}')" : "engel yok");
			string text = stringBuilder.ToString();
			Debug.Log(text, this);
			return text;
		}

		private static string DescribePoint(Vector3 position, Vector3 focus, float radius, int mask)
		{
			bool flag = Physics.CheckSphere(position, radius, mask, QueryTriggerInteraction.Ignore);
			RaycastHit hitInfo;
			bool num = Physics.Linecast(position, focus, out hitInfo, mask, QueryTriggerInteraction.Ignore);
			string text = (flag ? "ICERDE" : "acikta");
			string text2 = (num ? $"gorus KAPALI '{hitInfo.collider.name}' {hitInfo.distance:0.00}m" : "gorus acik");
			return Fmt(position) + " " + text + ", " + text2;
		}

		private static float ProbeRadius()
		{
			return 0.25f;
		}

		private static string Fmt(Vector3 v)
		{
			return $"({v.x:0.00}, {v.y:0.00}, {v.z:0.00})";
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetOnPlay()
		{
			IsPlaying = false;
		}
	}
}
