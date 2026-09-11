using UnityEngine;
using pworld.Scripts.Extensions;
using pworld.Scripts.PPhys;

namespace pworld.Scripts
{
	public class PCameraLook : MonoBehaviour
	{
		[SerializeReference]
		public ITimeSource timeSource = new DefaultTime();

		public bool usePInput = true;

		public float sensitity = 1f;

		public Vector2 minMaxPitch = new Vector2(-180f, 180f);

		public Vector2 minMaxYaw = new Vector2(-180f, 180f);

		public float pitch;

		public float yaw;

		private Transform pitchTrans;

		private Transform yawTrans;

		private void Awake()
		{
			yawTrans = base.transform.GetChild(0).transform;
			pitchTrans = yawTrans.GetChild(0).transform;
		}

		public void Update()
		{
			if (usePInput)
			{
				PitchJawInput(PSingleton<PInput>.Me.mouseD);
			}
			Rotate();
		}

		public void Generate()
		{
			pitchTrans = new GameObject("pitchTransform").transform;
			yawTrans = new GameObject("yawTransform").transform;
			yawTrans.parent = base.transform;
			yawTrans.localPosition = Vector3.zero;
			yawTrans.localRotation = Quaternion.identity;
			pitchTrans.parent = yawTrans;
			pitchTrans.localPosition = Vector3.zero;
			pitchTrans.localRotation = Quaternion.identity;
		}

		private void ClampPitchYaw()
		{
			pitch.PClamp(minMaxPitch.x, minMaxPitch.y);
			yaw.PClamp(minMaxYaw.x, minMaxYaw.y);
		}

		public void PitchJawInput(Vector2 mouseDelta)
		{
			pitch += (0f - mouseDelta.y) * sensitity;
			yaw += mouseDelta.x * sensitity;
			pitch %= 360f;
			yaw %= 360f;
			ClampPitchYaw();
		}

		private void Rotate()
		{
			pitchTrans.localRotation = Quaternion.Euler(pitch, 0f, 0f);
			yawTrans.localRotation = Quaternion.Euler(0f, yaw, 0f);
		}
	}
}
