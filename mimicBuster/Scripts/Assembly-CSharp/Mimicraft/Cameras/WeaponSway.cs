using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mimicraft.Cameras
{
	public class WeaponSway : MonoBehaviour
	{
		[SerializeField]
		private CharacterController controller;

		[Header("Sway (mouse look)")]
		[SerializeField]
		private float swayAmount = 0.015f;

		[SerializeField]
		private float swayRotationAmount = 3f;

		[SerializeField]
		private float maxSwayOffset = 0.05f;

		[SerializeField]
		private float swaySmoothing = 8f;

		[Header("Bob (walking)")]
		[SerializeField]
		private float bobFrequency = 1.8f;

		[SerializeField]
		private float bobHorizontalAmount = 0.012f;

		[SerializeField]
		private float bobVerticalAmount = 0.02f;

		[SerializeField]
		private float bobSmoothing = 10f;

		[Tooltip("Movement speed (units/sec) that reaches full bob amplitude - PlayerMovement's Hunter walk speed.")]
		[SerializeField]
		private float referenceMoveSpeed = 4f;

		[Header("Recoil (on fire)")]
		[SerializeField]
		private float recoilKickBack = 0.05f;

		[SerializeField]
		private float recoilKickUpDegrees = 6f;

		[SerializeField]
		private float recoilRecoverySpeed = 10f;

		[Tooltip("Caps how far consecutive shots (fired faster than the recovery above can settle) can stack the kick.")]
		[SerializeField]
		private float maxRecoilKickBack = 0.12f;

		[SerializeField]
		private float maxRecoilKickUpDegrees = 18f;

		[Header("Muzzle Flash (on fire)")]
		[Tooltip("Effect object living INSIDE the player prefab, at the muzzle. Assigned, it is what plays on every shot. Left empty, the procedural point light below is used instead - which is what this did before there was an authored effect to play.")]
		[SerializeField]
		private GameObject muzzleFlash;

		[SerializeField]
		private float muzzleFlashForwardOffset = 0.4f;

		[SerializeField]
		private float muzzleFlashLifetime = 0.05f;

		[SerializeField]
		private float muzzleFlashRange = 3f;

		[SerializeField]
		private float muzzleFlashIntensity = 6f;

		private const float TwoPi = MathF.PI * 2f;

		private Vector3 restLocalPosition;

		private Quaternion restLocalRotation;

		private Vector3 swayPositionOffset;

		private Quaternion swayRotationOffset = Quaternion.identity;

		private Vector3 bobOffset;

		private float bobTimer;

		private float aimSwayScale = 1f;

		private float aimBobScale = 1f;

		private Vector3 recoilPositionOffset;

		private float recoilPitch;

		private float authoredRecoverySpeed;

		public string DebugState => $"rest yerel {restLocalRotation.eulerAngles:F1} sway {swayRotationOffset.eulerAngles:F1} " + $"recoilPitch {recoilPitch:0.00} recoilPos {recoilPositionOffset:F3} recovery {recoilRecoverySpeed:0.#}";

		public void SetController(CharacterController controller)
		{
			this.controller = controller;
		}

		public void SetAimScale(float sway, float bob)
		{
			aimSwayScale = Mathf.Max(0f, sway);
			aimBobScale = Mathf.Max(0f, bob);
		}

		public void ConfigureRecoilOnly()
		{
			swayAmount = 0f;
			swayRotationAmount = 0f;
			bobHorizontalAmount = 0f;
			bobVerticalAmount = 0f;
		}

		public void TriggerRecoil()
		{
			TriggerRecoil(recoilKickBack, recoilKickUpDegrees);
		}

		public void TriggerRecoil(float kickBack, float kickUpDegrees)
		{
			recoilPositionOffset.z = Mathf.Clamp(recoilPositionOffset.z - kickBack, 0f - maxRecoilKickBack, 0f);
			recoilPitch = Mathf.Clamp(recoilPitch + kickUpDegrees, 0f, maxRecoilKickUpDegrees);
		}

		public void SetRecoilRecoverySpeed(float speed)
		{
			recoilRecoverySpeed = ((speed > 0f) ? speed : authoredRecoverySpeed);
		}

		public void TriggerMuzzleFlash(Transform muzzle = null)
		{
			if (muzzleFlash != null)
			{
				PlayAuthoredMuzzleFlash();
				return;
			}
			Vector3 position = ((muzzle != null) ? muzzle.position : (base.transform.position + base.transform.forward * muzzleFlashForwardOffset));
			Quaternion rotation = ((muzzle != null) ? muzzle.rotation : base.transform.rotation);
			GameObject obj = new GameObject("MuzzleFlash");
			obj.transform.SetPositionAndRotation(position, rotation);
			Light light = obj.AddComponent<Light>();
			light.type = LightType.Point;
			light.color = new Color(1f, 0.75f, 0.35f);
			light.range = muzzleFlashRange;
			light.intensity = muzzleFlashIntensity;
			UnityEngine.Object.Destroy(obj, muzzleFlashLifetime);
		}

		private void PlayAuthoredMuzzleFlash()
		{
			if (!muzzleFlash.activeSelf)
			{
				muzzleFlash.SetActive(value: true);
			}
			bool flag = false;
			ParticleSystem[] componentsInChildren = muzzleFlash.GetComponentsInChildren<ParticleSystem>(includeInactive: true);
			foreach (ParticleSystem obj in componentsInChildren)
			{
				obj.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmittingAndClear);
				obj.Play(withChildren: true);
				flag = true;
			}
			if (!flag)
			{
				CancelInvoke("HideMuzzleFlash");
				Invoke("HideMuzzleFlash", muzzleFlashLifetime);
			}
		}

		private void HideMuzzleFlash()
		{
			if (muzzleFlash != null)
			{
				muzzleFlash.SetActive(value: false);
			}
		}

		private void Awake()
		{
			restLocalPosition = base.transform.localPosition;
			restLocalRotation = base.transform.localRotation;
			authoredRecoverySpeed = recoilRecoverySpeed;
			if (controller == null)
			{
				controller = GetComponentInParent<CharacterController>();
			}
		}

		private void LateUpdate()
		{
			if (!GameMenuState.LookCaptured)
			{
				UpdateSway();
				UpdateBob();
				UpdateRecoil();
				base.transform.localPosition = restLocalPosition + swayPositionOffset + bobOffset + recoilPositionOffset;
				base.transform.localRotation = restLocalRotation * swayRotationOffset * Quaternion.Euler(0f - recoilPitch, 0f, 0f);
			}
		}

		private void UpdateRecoil()
		{
			recoilPositionOffset = Vector3.Lerp(recoilPositionOffset, Vector3.zero, recoilRecoverySpeed * Time.deltaTime);
			recoilPitch = Mathf.Lerp(recoilPitch, 0f, recoilRecoverySpeed * Time.deltaTime);
		}

		private void UpdateSway()
		{
			Vector2 vector = ((Mouse.current != null) ? Mouse.current.delta.ReadValue() : Vector2.zero);
			Vector3 b = new Vector3(Mathf.Clamp((0f - vector.x) * swayAmount, 0f - maxSwayOffset, maxSwayOffset), Mathf.Clamp((0f - vector.y) * swayAmount, 0f - maxSwayOffset, maxSwayOffset), 0f) * aimSwayScale;
			Quaternion b2 = Quaternion.Euler(vector.y * swayRotationAmount * aimSwayScale, (0f - vector.x) * swayRotationAmount * aimSwayScale, 0f);
			swayPositionOffset = Vector3.Lerp(swayPositionOffset, b, swaySmoothing * Time.deltaTime);
			swayRotationOffset = Quaternion.Slerp(swayRotationOffset, b2, swaySmoothing * Time.deltaTime);
		}

		private void UpdateBob()
		{
			Vector3 vector = ((controller != null) ? controller.velocity : Vector3.zero);
			vector.y = 0f;
			float num = Mathf.Clamp01(vector.magnitude / Mathf.Max(referenceMoveSpeed, 0.01f));
			int num2;
			if (num > 0.05f)
			{
				if (!(controller == null))
				{
					num2 = (controller.isGrounded ? 1 : 0);
					if (num2 == 0)
					{
						goto IL_0096;
					}
				}
				else
				{
					num2 = 1;
				}
				bobTimer += Time.deltaTime * bobFrequency * (MathF.PI * 2f);
				goto IL_00a1;
			}
			num2 = 0;
			goto IL_0096;
			IL_00a1:
			Vector3 b = ((num2 != 0) ? (new Vector3(Mathf.Sin(bobTimer) * bobHorizontalAmount * num, Mathf.Abs(Mathf.Sin(bobTimer)) * bobVerticalAmount * num, 0f) * aimBobScale) : Vector3.zero);
			bobOffset = Vector3.Lerp(bobOffset, b, bobSmoothing * Time.deltaTime);
			return;
			IL_0096:
			bobTimer = 0f;
			goto IL_00a1;
		}
	}
}
