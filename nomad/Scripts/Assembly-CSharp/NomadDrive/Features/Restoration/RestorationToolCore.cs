using System;
using EvilCore.EvilPack.EvilLogger;
using PaintIn3D;
using UnityEngine;

namespace NomadDrive.Features.Restoration
{
	public class RestorationToolCore
	{
		public Transform ToolTip;

		public CwHitBetween HitBetween;

		public CwPaintSphere[] PaintSpheres;

		public ParticleSystem ToolEffect;

		public float HitInterval;

		public float FastMovementRadiusMultiplier;

		public float ActiveRadius;

		public bool EnablePressureResponse;

		public float MinPressureOpacity;

		public float MaxMovementSpeed;

		public bool IsToolActive;

		private Vector3 _lastToolTipPosition;

		private float _currentMovementSpeed;

		private float _baseOpacity;

		public void Initialize(float baseOpacity)
		{
			_baseOpacity = baseOpacity;
		}

		public void ValidateSetup(Transform ownerTransform, string typeName)
		{
			if (ToolTip == null)
			{
				ToolTip = ownerTransform.Find("ToolTip");
				if (ToolTip == null)
				{
					EvilLogger.LogError("[" + typeName + "] ToolTip transform not found! Create a child GameObject named 'ToolTip'.", "ValidateSetup", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Restoration\\Scripts\\Shared\\RestorationToolCore.cs", 44);
				}
			}
			if (HitBetween == null && ToolTip != null)
			{
				HitBetween = ToolTip.GetComponent<CwHitBetween>();
				if (HitBetween == null)
				{
					EvilLogger.LogError("[" + typeName + "] CwHitBetween not found on ToolTip! Add CwHitBetween component.", "ValidateSetup", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Restoration\\Scripts\\Shared\\RestorationToolCore.cs", 51);
				}
			}
			if (PaintSpheres == null || PaintSpheres.Length == 0)
			{
				PaintSpheres = ownerTransform.GetComponentsInChildren<CwPaintSphere>();
				if (PaintSpheres.Length == 0)
				{
					EvilLogger.LogError("[" + typeName + "] No CwPaintSphere components found!", "ValidateSetup", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Restoration\\Scripts\\Shared\\RestorationToolCore.cs", 58);
				}
			}
		}

		public void UpdatePressureSensitivity()
		{
			if (ToolTip == null)
			{
				return;
			}
			_currentMovementSpeed = Vector3.Distance(ToolTip.position, _lastToolTipPosition) / Time.deltaTime;
			_lastToolTipPosition = ToolTip.position;
			float t = Mathf.Clamp01(_currentMovementSpeed / MaxMovementSpeed);
			float opacity = Mathf.Lerp(_baseOpacity, MinPressureOpacity, t);
			CwPaintSphere[] paintSpheres = PaintSpheres;
			foreach (CwPaintSphere cwPaintSphere in paintSpheres)
			{
				if (cwPaintSphere != null)
				{
					cwPaintSphere.Opacity = opacity;
				}
			}
		}

		public void ApplyToolStateToSpheres(float radius, float opacity)
		{
			if (PaintSpheres == null)
			{
				return;
			}
			CwPaintSphere[] paintSpheres = PaintSpheres;
			foreach (CwPaintSphere cwPaintSphere in paintSpheres)
			{
				if (!(cwPaintSphere == null))
				{
					cwPaintSphere.Radius = radius;
					if (opacity > 0f)
					{
						cwPaintSphere.Opacity = opacity;
					}
				}
			}
		}

		public void ApplyHitDetectionSettings()
		{
			if (!(HitBetween == null))
			{
				HitBetween.Interval = HitInterval;
			}
		}

		public void OnHitSettingsChanged()
		{
			if (IsToolActive)
			{
				ApplyHitDetectionSettings();
			}
		}

		public void OnRadiusMultiplierChanged()
		{
			if (!IsToolActive)
			{
				return;
			}
			float radius = ActiveRadius * FastMovementRadiusMultiplier;
			CwPaintSphere[] paintSpheres = PaintSpheres;
			foreach (CwPaintSphere cwPaintSphere in paintSpheres)
			{
				if (cwPaintSphere != null)
				{
					cwPaintSphere.Radius = radius;
				}
			}
		}

		public void SetHitPresetUltraSmooth()
		{
			HitInterval = 0f;
			FastMovementRadiusMultiplier = 2f;
			OnHitSettingsChanged();
			OnRadiusMultiplierChanged();
		}

		public void SetHitPresetBalanced()
		{
			HitInterval = 0f;
			FastMovementRadiusMultiplier = 1.5f;
			OnHitSettingsChanged();
			OnRadiusMultiplierChanged();
		}

		public void SetHitPresetPrecision()
		{
			HitInterval = 0f;
			FastMovementRadiusMultiplier = 1f;
			OnHitSettingsChanged();
			OnRadiusMultiplierChanged();
		}

		public (float radius, float opacity) EnableToolLocal()
		{
			if (IsToolActive)
			{
				return (radius: 0f, opacity: 0f);
			}
			IsToolActive = true;
			if (ToolTip != null)
			{
				_lastToolTipPosition = ToolTip.position;
			}
			float num = ActiveRadius * FastMovementRadiusMultiplier;
			float baseOpacity = _baseOpacity;
			ApplyToolStateToSpheres(num, baseOpacity);
			if (HitBetween != null)
			{
				HitBetween.enabled = true;
				ApplyHitDetectionSettings();
			}
			if (ToolEffect != null)
			{
				ToolEffect.Play();
			}
			return (radius: num, opacity: baseOpacity);
		}

		public void DisableToolLocal()
		{
			IsToolActive = false;
			ApplyToolStateToSpheres(0f, 0f);
			if (HitBetween != null)
			{
				HitBetween.enabled = false;
			}
			if (ToolEffect != null)
			{
				ToolEffect.Stop();
			}
		}

		public void FullAutoSetup(Transform ownerTransform, string typeName, Action setupPaintSpheres)
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
			Transform transform = ToolTip.Find("PointA");
			if (transform == null)
			{
				GameObject gameObject2 = new GameObject("PointA");
				gameObject2.transform.SetParent(ToolTip);
				gameObject2.transform.localPosition = Vector3.back * 0.1f;
				transform = gameObject2.transform;
			}
			Transform transform2 = ToolTip.Find("PointB");
			if (transform2 == null)
			{
				GameObject gameObject3 = new GameObject("PointB");
				gameObject3.transform.SetParent(ToolTip);
				gameObject3.transform.localPosition = Vector3.zero;
				transform2 = gameObject3.transform;
			}
			HitBetween = ToolTip.GetComponent<CwHitBetween>();
			if (HitBetween == null)
			{
				HitBetween = ToolTip.gameObject.AddComponent<CwHitBetween>();
			}
			HitBetween.PointA = transform;
			HitBetween.PointB = transform2;
			HitBetween.Interval = HitInterval;
			setupPaintSpheres?.Invoke();
			PaintSpheres = ToolTip.GetComponentsInChildren<CwPaintSphere>();
		}

		public void FindComponents(Transform ownerTransform, string typeName)
		{
			if (ToolTip == null)
			{
				ToolTip = ownerTransform.Find("ToolTip");
			}
			if (ToolTip != null && HitBetween == null)
			{
				HitBetween = ToolTip.GetComponent<CwHitBetween>();
			}
			PaintSpheres = ownerTransform.GetComponentsInChildren<CwPaintSphere>();
		}
	}
}
