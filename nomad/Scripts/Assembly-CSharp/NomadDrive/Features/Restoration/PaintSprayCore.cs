using EvilCore.EvilPack.EvilLogger;
using PaintCore;
using PaintIn3D;
using UnityEngine;

namespace NomadDrive.Features.Restoration
{
	public class PaintSprayCore
	{
		public Transform ToolTip;

		public ParticleSystem ParticleSystem;

		public CwHitParticles HitParticles;

		public CwPaintSphere ColorSphere;

		public CwPaintSphere MaskSphere;

		public Color PaintColor;

		public float SphereRadius;

		public float Hardness;

		public float ColorOpacity;

		public float MaskOpacity;

		public float EmissionRate;

		public float ParticleSpeed;

		public float SprayAngle;

		public float ParticleLifetime;

		public int PaintColorGroupIndex;

		public int PaintMaskGroupIndex;

		public bool IsToolActive;

		private ParticleSystem.EmissionModule _emission;

		private bool _emissionCached;

		public void ValidateSetup(Transform ownerTransform)
		{
			if (ToolTip == null)
			{
				ToolTip = ownerTransform.Find("ToolTip");
				if (ToolTip == null)
				{
					EvilLogger.LogError("[PaintSprayTool] ToolTip transform not found! Run 'Full Auto Setup'.", "ValidateSetup", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Restoration\\Scripts\\Shared\\PaintSprayCore.cs", 46);
				}
			}
			if (ParticleSystem == null && ToolTip != null)
			{
				ParticleSystem = ToolTip.GetComponent<ParticleSystem>();
			}
			if (HitParticles == null && ToolTip != null)
			{
				HitParticles = ToolTip.GetComponent<CwHitParticles>();
			}
			CacheEmission();
		}

		private void CacheEmission()
		{
			if (ParticleSystem != null)
			{
				_emission = ParticleSystem.emission;
				_emissionCached = true;
			}
		}

		public void ConfigureSpheres()
		{
			if (ColorSphere != null)
			{
				ColorSphere.Group = new CwGroup(PaintColorGroupIndex);
				ColorSphere.Color = PaintColor;
				ColorSphere.Opacity = ColorOpacity;
				ColorSphere.Radius = SphereRadius;
				ColorSphere.Hardness = Hardness;
				ColorSphere.BlendMode = CwBlendMode.AlphaBlend(new Vector4(1f, 1f, 1f, 1f));
			}
			if (MaskSphere != null)
			{
				MaskSphere.Group = new CwGroup(PaintMaskGroupIndex);
				MaskSphere.Color = Color.white;
				MaskSphere.Opacity = MaskOpacity;
				MaskSphere.Radius = SphereRadius;
				MaskSphere.Hardness = Hardness;
				MaskSphere.BlendMode = CwBlendMode.Additive(new Vector4(1f, 0f, 0f, 0f));
			}
		}

		public void UpdatePaintColor()
		{
			if (ColorSphere != null)
			{
				ColorSphere.Color = PaintColor;
			}
			if (ParticleSystem != null)
			{
				ParticleSystem.MainModule main = ParticleSystem.main;
				main.startColor = new Color(PaintColor.r, PaintColor.g, PaintColor.b, 0.5f);
			}
		}

		public void ConfigureParticleSystem()
		{
			if (!(ParticleSystem == null))
			{
				ParticleSystem.MainModule main = ParticleSystem.main;
				main.startLifetime = ParticleLifetime;
				main.startSpeed = ParticleSpeed;
				main.startSize = 0.02f;
				main.startColor = new Color(PaintColor.r, PaintColor.g, PaintColor.b, 0.5f);
				main.simulationSpace = ParticleSystemSimulationSpace.World;
				main.maxParticles = 500;
				ParticleSystem.EmissionModule emission = ParticleSystem.emission;
				emission.rateOverTime = EmissionRate;
				emission.enabled = false;
				ParticleSystem.ShapeModule shape = ParticleSystem.shape;
				shape.enabled = true;
				shape.shapeType = ParticleSystemShapeType.Cone;
				shape.angle = SprayAngle;
				shape.radius = 0.01f;
				ParticleSystem.CollisionModule collision = ParticleSystem.collision;
				collision.enabled = true;
				collision.type = ParticleSystemCollisionType.World;
				collision.mode = ParticleSystemCollisionMode.Collision3D;
				collision.bounce = 0f;
				collision.lifetimeLoss = 1f;
				collision.sendCollisionMessages = true;
				ParticleSystemRenderer component = ParticleSystem.GetComponent<ParticleSystemRenderer>();
				if (component != null)
				{
					component.renderMode = ParticleSystemRenderMode.Billboard;
				}
			}
		}

		public void EnableToolLocal()
		{
			if (!IsToolActive)
			{
				IsToolActive = true;
				if (_emissionCached)
				{
					_emission.enabled = true;
					ParticleSystem.Play();
				}
			}
		}

		public void DisableToolLocal()
		{
			IsToolActive = false;
			if (_emissionCached)
			{
				_emission.enabled = false;
				ParticleSystem.Stop();
			}
		}

		public void FullAutoSetup(Transform ownerTransform)
		{
			if (ToolTip == null)
			{
				ToolTip = ownerTransform.Find("ToolTip");
				if (ToolTip == null)
				{
					GameObject gameObject = new GameObject("ToolTip");
					gameObject.transform.SetParent(ownerTransform);
					gameObject.transform.localPosition = Vector3.zero;
					ToolTip = gameObject.transform;
				}
			}
			ParticleSystem = ToolTip.GetComponent<ParticleSystem>();
			if (ParticleSystem == null)
			{
				ParticleSystem = ToolTip.gameObject.AddComponent<ParticleSystem>();
			}
			ConfigureParticleSystem();
			HitParticles = ToolTip.GetComponent<CwHitParticles>();
			if (HitParticles == null)
			{
				HitParticles = ToolTip.gameObject.AddComponent<CwHitParticles>();
			}
			HitParticles.Emit = CwHitParticles.EmitType.PointsIn3D;
			HitParticles.PressureMode = CwHitParticles.PressureType.Constant;
			HitParticles.PressureConstant = 1f;
			Transform transform = ToolTip.Find("Sphere_Color");
			if (transform == null)
			{
				GameObject gameObject2 = new GameObject("Sphere_Color");
				gameObject2.transform.SetParent(ToolTip);
				gameObject2.transform.localPosition = Vector3.zero;
				transform = gameObject2.transform;
			}
			ColorSphere = transform.GetComponent<CwPaintSphere>() ?? transform.gameObject.AddComponent<CwPaintSphere>();
			Transform transform2 = ToolTip.Find("Sphere_Mask");
			if (transform2 == null)
			{
				GameObject gameObject3 = new GameObject("Sphere_Mask");
				gameObject3.transform.SetParent(ToolTip);
				gameObject3.transform.localPosition = Vector3.zero;
				transform2 = gameObject3.transform;
			}
			MaskSphere = transform2.GetComponent<CwPaintSphere>() ?? transform2.gameObject.AddComponent<CwPaintSphere>();
			ConfigureSpheres();
			CacheEmission();
			if (_emissionCached)
			{
				_emission.enabled = false;
			}
		}

		public void FindComponents(Transform ownerTransform)
		{
			if (ToolTip == null)
			{
				ToolTip = ownerTransform.Find("ToolTip");
			}
			if (ToolTip != null)
			{
				if (ParticleSystem == null)
				{
					ParticleSystem = ToolTip.GetComponent<ParticleSystem>();
				}
				if (HitParticles == null)
				{
					HitParticles = ToolTip.GetComponent<CwHitParticles>();
				}
				Transform transform = ToolTip.Find("Sphere_Color");
				if (transform != null && ColorSphere == null)
				{
					ColorSphere = transform.GetComponent<CwPaintSphere>();
				}
				Transform transform2 = ToolTip.Find("Sphere_Mask");
				if (transform2 != null && MaskSphere == null)
				{
					MaskSphere = transform2.GetComponent<CwPaintSphere>();
				}
			}
		}
	}
}
