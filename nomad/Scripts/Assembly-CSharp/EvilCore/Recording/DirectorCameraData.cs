using System;
using UnityEngine;

namespace EvilCore.Recording
{
	[Serializable]
	public class DirectorCameraData
	{
		public string cameraName = "Camera";

		public DirectorCameraBehavior behaviorType;

		public float holdDuration = 5f;

		public TransitionType transitionType = TransitionType.Blend;

		public float blendDuration = 1f;

		public Vector3 position;

		public Quaternion rotation = Quaternion.identity;

		public float fieldOfView = 60f;

		public float smoothing;

		public float hiddenSchemaInputSpeed = 1f;

		public bool executeOnHide = true;

		public bool depthOfField;

		public float focusDistance = 10f;

		public bool nearBlur = true;

		public float nearBlurRange = 4f;

		public bool farBlur;

		public float farBlurRange = 15f;

		public bool autoFocusOnTarget;

		public StaticSettings staticSettings = new StaticSettings();

		public OrbitSettings orbitSettings = new OrbitSettings();

		public DollySettings dollySettings = new DollySettings();

		public FollowSettings followSettings = new FollowSettings();

		public PanTiltSettings panTiltSettings = new PanTiltSettings();

		public ZoomSettings zoomSettings = new ZoomSettings();

		public HandheldSettings handheldSettings = new HandheldSettings();

		public TurntableSettings turntableSettings = new TurntableSettings();
	}
}
