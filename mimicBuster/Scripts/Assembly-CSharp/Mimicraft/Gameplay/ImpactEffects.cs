using System;
using Mimicraft.Customization;
using Mimicraft.UI;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	public static class ImpactEffects
	{
		private const float BurstLifetime = 0.6f;

		private static Transform container;

		private static bool warnedAboutSharedShot;

		private static AudioClip hitClip;

		private static AudioClip killClip;

		private static AudioClip hurtClip;

		private static Transform Container
		{
			get
			{
				if (container != null)
				{
					return container;
				}
				GameObject gameObject = new GameObject("ImpactEffects");
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				container = gameObject.transform;
				return container;
			}
		}

		public static event Action<ImpactKind, Vector3, Vector3> Happened;

		public static event Action<ulong, Vector3, WeaponDefinition> ShotFired;

		public static void Adopt(GameObject spawned)
		{
			if (spawned != null)
			{
				spawned.transform.SetParent(Container, worldPositionStays: true);
			}
		}

		public static void ClearAll()
		{
			if (!(container == null))
			{
				for (int num = container.childCount - 1; num >= 0; num--)
				{
					UnityEngine.Object.DestroyImmediate(container.GetChild(num).gameObject);
				}
			}
		}

		public static void ReportShot(ulong shooterId, Vector3 position, WeaponDefinition weapon)
		{
			ImpactEffects.ShotFired?.Invoke(shooterId, position, weapon);
		}

		public static void Replay(ImpactKind kind, Vector3 position, Vector3 normal)
		{
			switch (kind)
			{
			case ImpactKind.Wall:
				SpawnWallEffect(position, normal);
				break;
			case ImpactKind.Hit:
				SpawnHitEffect(position, normal);
				break;
			case ImpactKind.Kill:
				SpawnKillEffect(position, normal);
				break;
			case ImpactKind.Cling:
				SpawnClingEffect(position, normal, attached: true);
				break;
			case ImpactKind.Release:
				SpawnClingEffect(position, normal, attached: false);
				break;
			case ImpactKind.PunchWall:
				SpawnPunchWallEffect(position, normal, 0);
				break;
			case ImpactKind.ChargeImpact:
				SpawnChargeImpactEffect(position, normal);
				break;
			}
		}

		public static void SpawnHitEffect(Vector3 position, Vector3 normal = default(Vector3), bool withSound = true)
		{
			if (!EffectLibrary.TrySpawn((EffectLibrary.Instance != null) ? EffectLibrary.Instance.ModelHitPrefab : null, position, normal))
			{
				SpawnBurst(position, new Color(1f, 0.85f, 0.2f), 0.15f, 10);
			}
			if (withSound)
			{
				PlayAtPoint(GetHitClip(), position, 0.7f);
			}
			ImpactEffects.Happened?.Invoke(ImpactKind.Hit, position, normal);
		}

		public static void SpawnTracer(Vector3 from, Vector3 to)
		{
			BulletTracer.Spawn(from, to);
		}

		public static void PlayKillConfirm()
		{
			AudioLibrary.PlayOneShotClip(AudioLibrary.Or((AudioLibrary.Instance != null) ? AudioLibrary.Instance.killConfirmClip : null, GetHitClip()));
		}

		public static void SpawnWallEffect(Vector3 position, Vector3 normal, bool withSound = true)
		{
			if (!EffectLibrary.TrySpawn((EffectLibrary.Instance != null) ? EffectLibrary.Instance.WallHitPrefab : null, position, normal))
			{
				SpawnBurst(position, new Color(0.8f, 0.8f, 0.75f), 0.1f, 6);
			}
			if (EffectLibrary.Instance != null)
			{
				EffectLibrary.TrySpawnMark(EffectLibrary.Instance.BulletHolePrefab, position, normal, EffectLibrary.Instance.BulletHoleLifetime, UnityEngine.Random.Range(0f, 360f));
			}
			if (withSound)
			{
				PlayAtPoint(AudioLibrary.Or((AudioLibrary.Instance != null) ? AudioLibrary.Instance.wallHitClip : null, GetHitClip()), position, 0.4f);
			}
			ImpactEffects.Happened?.Invoke(ImpactKind.Wall, position, normal);
		}

		public static void SpawnPunchEffect(Vector3 position, Vector3 normal = default(Vector3))
		{
			if (!EffectLibrary.TrySpawn((EffectLibrary.Instance != null) ? EffectLibrary.Instance.HitDustPrefab : null, position, normal))
			{
				SpawnBurst(position, new Color(0.85f, 0.8f, 0.7f), 0.12f, 8);
			}
			PlayAtPoint(AudioLibrary.Or(AudioLibrary.NextPunchHitClip(), GetHitClip()), position, 0.7f);
		}

		public static void SpawnPunchWallEffect(Vector3 position, Vector3 normal, int layer)
		{
			PunchSurface punchSurface = EffectLibrary.FindPunchSurface(layer);
			if (!EffectLibrary.TrySpawn(punchSurface?.effectPrefab, position, normal))
			{
				SpawnBurst(position, new Color(0.78f, 0.76f, 0.72f), 0.14f, 9);
			}
			PlayAtPoint(AudioLibrary.Or(punchSurface?.PickClip(), AudioLibrary.Or(AudioLibrary.NextPunchHitClip(), GetHitClip())), position, punchSurface?.volume ?? 0.8f, punchSurface?.PickPitch() ?? 1f);
			ImpactEffects.Happened?.Invoke(ImpactKind.PunchWall, position, normal);
		}

		public static void SpawnChargeImpactEffect(Vector3 position, Vector3 normal, float size = 1f)
		{
			float num = Mathf.Clamp(size, 0.75f, 2f);
			if (!EffectLibrary.TrySpawn((EffectLibrary.Instance != null) ? EffectLibrary.Instance.ChargeImpactPrefab : null, position, normal, num))
			{
				SpawnBurst(position, new Color(0.85f, 0.8f, 0.7f), 0.18f * num, 14);
			}
			PlayAtPoint(AudioLibrary.Or(AudioLibrary.NextChargeImpactClip(), AudioLibrary.Or(AudioLibrary.NextPunchHitClip(), GetHitClip())), position, 0.9f, Mathf.Lerp(1.1f, 0.85f, Mathf.InverseLerp(0.75f, 2f, num)));
			ImpactEffects.Happened?.Invoke(ImpactKind.ChargeImpact, position, normal);
		}

		public static void SpawnTutorialCelebration(Vector3 position)
		{
			if (!EffectLibrary.TrySpawn((EffectLibrary.Instance != null) ? EffectLibrary.Instance.TutorialCelebrationPrefab : null, position, Vector3.up))
			{
				SpawnBurst(position, new Color(1f, 0.85f, 0.3f), 0.22f, 24);
			}
			AudioLibrary.PlayOneShotClip(AudioLibrary.Or((AudioLibrary.Instance != null) ? AudioLibrary.Instance.tutorialStepClip : null, (AudioLibrary.Instance != null) ? AudioLibrary.Instance.roundSuccessClip : null));
		}

		public static void SpawnKillEffect(Vector3 position, Vector3 normal = default(Vector3))
		{
			if (!EffectLibrary.TrySpawn((EffectLibrary.Instance != null) ? EffectLibrary.Instance.KillPrefab : null, position, normal))
			{
				SpawnBurst(position, new Color(0.9f, 0.2f, 0.2f), 0.3f, 22);
			}
			PlayAtPoint(GetKillClip(), position, 0.85f);
			ImpactEffects.Happened?.Invoke(ImpactKind.Kill, position, normal);
		}

		public static void SpawnClingEffect(Vector3 position, Vector3 normal, bool attached, float radius = 0f)
		{
			EffectLibrary instance = EffectLibrary.Instance;
			GameObject prefab = null;
			if (instance != null)
			{
				prefab = ((attached || instance.WallReleasePrefab == null) ? instance.WallClingPrefab : instance.WallReleasePrefab);
			}
			radius *= EffectLibrary.ClingRadiusScale;
			if (EffectLibrary.TrySpawn(prefab, position, normal, out var spawned))
			{
				ApplyShapeRadius(spawned, radius);
			}
			else
			{
				SpawnBurst(position, new Color(0.8f, 0.8f, 0.75f), (radius > 0f) ? (radius * 0.5f) : (attached ? 0.12f : 0.08f), attached ? 8 : 5);
			}
			ImpactEffects.Happened?.Invoke(attached ? ImpactKind.Cling : ImpactKind.Release, position, normal);
		}

		private static void ApplyShapeRadius(GameObject spawned, float radius)
		{
			if (spawned == null || radius <= 0f)
			{
				return;
			}
			ParticleSystem[] componentsInChildren = spawned.GetComponentsInChildren<ParticleSystem>(includeInactive: true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				ParticleSystem.ShapeModule shape = componentsInChildren[i].shape;
				if (shape.enabled)
				{
					shape.radius = radius;
				}
			}
		}

		public static void SpawnDoubleJumpEffect(Vector3 position)
		{
			if (!EffectLibrary.TrySpawn((EffectLibrary.Instance != null) ? EffectLibrary.Instance.DoubleJumpPrefab : null, position, Vector3.up))
			{
				SpawnBurst(position, new Color(0.85f, 0.9f, 1f), 0.2f, 14);
			}
		}

		public static void SpawnShotSound(Vector3 position, WeaponDefinition weapon = null, WeaponSoundKind sound = WeaponSoundKind.Normal)
		{
			AudioClip audioClip = ((weapon != null) ? weapon.PickFireClip(sound) : null);
			if (audioClip == null && !warnedAboutSharedShot)
			{
				warnedAboutSharedShot = true;
				Debug.LogWarning((weapon == null) ? "[ImpactEffects] Ates sesi icin silah gelmedi (PlayerWeapons.Equipped bos) - ortak AudioLibrary sesi calinacak." : ("[ImpactEffects] '" + weapon.DisplayName + "' silahinin Fire Clips listesi bos - ortak AudioLibrary sesi calinacak."));
			}
			if (audioClip != null)
			{
				PlayAtPoint(audioClip, position, weapon.FireVolumeFor(sound));
				return;
			}
			AudioClip audioClip2 = ((AudioLibrary.Instance != null) ? AudioLibrary.Instance.shotClip : null);
			if (audioClip2 != null)
			{
				PlayAtPoint(audioClip2, position, 0.9f);
			}
		}

		public static void SpawnDeathSound(Vector3 position, bool wasHunter, CharacterVoice voice = CharacterVoice.Male)
		{
			PlayAtPoint(AudioLibrary.Or(AudioLibrary.PickDeath(wasHunter, voice), GetKillClip()), position, 0.85f);
		}

		public static void PlayAtPoint(AudioClip clip, Vector3 position, float volume, float pitch = 1f)
		{
			if (!(clip == null))
			{
				GameObject gameObject = new GameObject("OneShotAudio");
				gameObject.transform.SetParent(Container, worldPositionStays: false);
				gameObject.transform.position = position;
				AudioSource audioSource = gameObject.AddComponent<AudioSource>();
				audioSource.clip = clip;
				audioSource.volume = volume * AudioLibrary.VolumeOf(clip);
				audioSource.pitch = pitch;
				SpatialAudio.Apply(audioSource);
				audioSource.Play();
				UnityEngine.Object.Destroy(gameObject, clip.length / Mathf.Max(0.01f, pitch) + 0.1f);
			}
		}

		public static AudioClip GetHurtClip()
		{
			return AudioLibrary.Or((AudioLibrary.Instance != null) ? AudioLibrary.Instance.missPenaltyClip : null, hurtClip ?? (hurtClip = BuildTone(180f, 0.15f, 0.8f)));
		}

		private static AudioClip GetHitClip()
		{
			return hitClip ?? (hitClip = BuildTone(880f, 0.08f, 0.9f));
		}

		private static AudioClip GetKillClip()
		{
			return killClip ?? (killClip = BuildTone(220f, 0.35f, 0.55f));
		}

		private static void SpawnBurst(Vector3 position, Color color, float size, int count)
		{
			GameObject gameObject = new GameObject("ImpactBurst");
			gameObject.transform.SetParent(Container, worldPositionStays: false);
			gameObject.transform.position = position;
			ParticleSystem particleSystem = gameObject.AddComponent<ParticleSystem>();
			particleSystem.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmittingAndClear);
			ParticleSystem.MainModule main = particleSystem.main;
			main.duration = 0.6f;
			main.loop = false;
			main.startLifetime = 0.42000002f;
			main.startSpeed = 3f;
			main.startSize = size;
			main.startColor = color;
			main.simulationSpace = ParticleSystemSimulationSpace.World;
			ParticleSystem.EmissionModule emission = particleSystem.emission;
			emission.rateOverTime = 0f;
			emission.SetBursts(new ParticleSystem.Burst[1]
			{
				new ParticleSystem.Burst(0f, (short)count)
			});
			ParticleSystem.ShapeModule shape = particleSystem.shape;
			shape.shapeType = ParticleSystemShapeType.Sphere;
			shape.radius = 0.05f;
			ParticleSystem.ColorOverLifetimeModule colorOverLifetime = particleSystem.colorOverLifetime;
			colorOverLifetime.enabled = true;
			Gradient gradient = new Gradient();
			gradient.SetKeys(new GradientColorKey[2]
			{
				new GradientColorKey(color, 0f),
				new GradientColorKey(color, 1f)
			}, new GradientAlphaKey[2]
			{
				new GradientAlphaKey(1f, 0f),
				new GradientAlphaKey(0f, 1f)
			});
			colorOverLifetime.color = gradient;
			particleSystem.Play();
			UnityEngine.Object.Destroy(gameObject, 0.8f);
		}

		private static AudioClip BuildTone(float frequency, float seconds, float amplitude)
		{
			int num = Mathf.RoundToInt(44100f * seconds);
			float[] array = new float[num];
			int num2 = Mathf.Max(1, num / 6);
			for (int i = 0; i < num; i++)
			{
				float num3 = Mathf.Min(1f, (float)Mathf.Min(i, num - 1 - i) / (float)num2);
				array[i] = Mathf.Sin(MathF.PI * 2f * frequency * (float)i / 44100f) * num3 * amplitude;
			}
			AudioClip audioClip = AudioClip.Create("ImpactTone", num, 1, 44100, stream: false);
			audioClip.SetData(array, 0);
			return audioClip;
		}
	}
}
