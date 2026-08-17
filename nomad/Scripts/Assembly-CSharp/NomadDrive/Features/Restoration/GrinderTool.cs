using Ami.BroAudio;
using Ami.BroAudio.Data;
using EvilCore.Audio;
using EvilCore.DynamicCasting;
using PaintCore;
using PaintIn3D;
using UnityEngine;

namespace NomadDrive.Features.Restoration
{
	public class GrinderTool : RestorationTool
	{
		[Header("Grinder Settings")]
		[SerializeField]
		[Range(1f, 30f)]
		private float _hardness = 12f;

		[SerializeField]
		[Range(0f, 1f)]
		private float _opacity = 0.7f;

		[Header("Target Groups")]
		[SerializeField]
		private int _dirtGroupIndex = 201;

		[SerializeField]
		private int _rustGroupIndex = 202;

		[SerializeField]
		private int _polishGroupIndex = 203;

		[SerializeField]
		private int _paintMaskGroupIndex = 204;

		[SerializeField]
		private int _metalMaskGroupIndex = 205;

		[Header("Disc Animation")]
		[SerializeField]
		private DiscRotationAnimator discAnimator;

		[Header("Spark Effect")]
		[SerializeField]
		private Transform _sparkTransform;

		[Header("Audio")]
		[SerializeField]
		private SoundID grindLoopSound;

		[SerializeField]
		private AudioParameter grindingParameter;

		private AudioHandle _loopHandle;

		private bool _loopPlaying;

		private bool _wasToolActiveRemote;

		protected override float GetBaseOpacity()
		{
			return _opacity;
		}

		protected override void ConfigurePaintSpheres()
		{
			if (_paintSpheres != null && _paintSpheres.Length >= 5)
			{
				ConfigureSphere(_paintSpheres[0], _dirtGroupIndex);
				ConfigureSphere(_paintSpheres[1], _rustGroupIndex);
				ConfigureSphere(_paintSpheres[2], _paintMaskGroupIndex);
				ConfigureSphere(_paintSpheres[3], _polishGroupIndex);
				ConfigureSphere(_paintSpheres[4], _metalMaskGroupIndex);
			}
		}

		private void ConfigureSphere(CwPaintSphere sphere, int groupIndex)
		{
			if (!(sphere == null))
			{
				sphere.Group = new CwGroup(groupIndex);
				sphere.Color = Color.black;
				sphere.Opacity = _opacity;
				sphere.Hardness = _hardness;
				sphere.BlendMode = CwBlendMode.Replace(new Vector4(1f, 0f, 0f, 0f));
			}
		}

		protected override void SetupPaintSpheresOnToolTip()
		{
			if (!(_toolTip == null))
			{
				CwPaintSphere cwPaintSphere = CreateOrFindSphere("Sphere_Dirt");
				CwPaintSphere cwPaintSphere2 = CreateOrFindSphere("Sphere_Rust");
				CwPaintSphere cwPaintSphere3 = CreateOrFindSphere("Sphere_Paint");
				CwPaintSphere cwPaintSphere4 = CreateOrFindSphere("Sphere_Polish");
				CwPaintSphere cwPaintSphere5 = CreateOrFindSphere("Sphere_Metal");
				_paintSpheres = new CwPaintSphere[5] { cwPaintSphere, cwPaintSphere2, cwPaintSphere3, cwPaintSphere4, cwPaintSphere5 };
				ConfigurePaintSpheres();
			}
		}

		private CwPaintSphere CreateOrFindSphere(string name)
		{
			Transform transform = _toolTip.Find(name);
			if (transform != null)
			{
				return transform.GetComponent<CwPaintSphere>() ?? transform.gameObject.AddComponent<CwPaintSphere>();
			}
			GameObject obj = new GameObject(name);
			obj.transform.SetParent(_toolTip);
			obj.transform.localPosition = Vector3.zero;
			return obj.AddComponent<CwPaintSphere>();
		}

		protected override void OnSurfaceDetected(CastResult result)
		{
			base.OnSurfaceDetected(result);
			UpdateSparkOrientation();
		}

		private void UpdateSparkOrientation()
		{
			if (!(_sparkTransform == null) && _isToolActive && _snapHelper.IsSnapped)
			{
				Vector3 snapNormal = _snapHelper.SnapNormal;
				Vector3 normalized = Vector3.Cross(snapNormal, base.transform.up).normalized;
				if (normalized.sqrMagnitude < 0.01f)
				{
					normalized = Vector3.Cross(snapNormal, base.transform.right).normalized;
				}
				_sparkTransform.rotation = Quaternion.LookRotation(normalized, snapNormal);
			}
		}

		protected override void Update()
		{
			base.Update();
			if (!base.isOwned)
			{
				if (_isToolActive && !_wasToolActiveRemote)
				{
					discAnimator?.StartAcceleration();
					StartLoopAudio();
				}
				else if (!_isToolActive && _wasToolActiveRemote)
				{
					discAnimator?.StartDeceleration();
					StopLoopAudio();
				}
				_wasToolActiveRemote = _isToolActive;
			}
		}

		public override void EnableTool()
		{
			base.EnableTool();
			if (_isToolActive)
			{
				discAnimator?.StartAcceleration();
				if (base.isOwned)
				{
					StartLoopAudio();
				}
			}
		}

		public override void DisableTool()
		{
			base.DisableTool();
			discAnimator?.StartDeceleration();
			StopLoopAudio();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			StopLoopAudio();
		}

		private void StartLoopAudio()
		{
			if (AudioManager != null && grindLoopSound.IsValid() && (!_loopPlaying || !_loopHandle.IsValid))
			{
				if (_loopHandle.IsValid)
				{
					AudioManager.ReleaseInstance(_loopHandle);
					_loopHandle = AudioHandle.Invalid;
				}
				_loopHandle = AudioManager.PlayLoopRegion(grindLoopSound, base.gameObject);
				AudioManager.SetParameter(_loopHandle, grindingParameter, value: true);
				_loopPlaying = true;
			}
		}

		private void StopLoopAudio()
		{
			if (_loopPlaying)
			{
				AudioManager?.StopLoopRegion(_loopHandle);
				_loopPlaying = false;
			}
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
