using Ami.BroAudio;
using Ami.BroAudio.Data;
using EvilCore.Audio;
using PaintCore;
using PaintIn3D;
using PrimeTween;
using UnityEngine;

namespace NomadDrive.Features.Restoration
{
	public class PolishTool : RestorationTool
	{
		[Header("Polish Settings")]
		[SerializeField]
		[Range(1f, 20f)]
		private float _hardness = 10f;

		[SerializeField]
		[Range(0f, 1f)]
		private float _opacity = 0.3f;

		[Header("Target Groups")]
		[SerializeField]
		private int _polishGroupIndex = 203;

		[Header("Disc Animation")]
		[SerializeField]
		private DiscRotationAnimator discAnimator;

		[Header("Shake Animation")]
		[SerializeField]
		private Transform shakeTarget;

		[SerializeField]
		[Range(0f, 0.05f)]
		private float shakeStrength = 0.004f;

		[SerializeField]
		[Range(0f, 5f)]
		private float shakeRotationStrength = 1.5f;

		[SerializeField]
		[Range(1f, 50f)]
		private float shakeFrequency = 25f;

		[Header("Audio")]
		[SerializeField]
		private SoundID polishLoopSound;

		[SerializeField]
		private AudioParameter polishingParameter;

		private AudioHandle _loopHandle;

		private bool _loopPlaying;

		private Tween _shakePositionTween;

		private Tween _shakeRotationTween;

		private bool _wasToolActiveForDisc;

		protected override void ConfigurePaintSpheres()
		{
			if (_paintSpheres != null && _paintSpheres.Length >= 1)
			{
				CwPaintSphere cwPaintSphere = _paintSpheres[0];
				if (!(cwPaintSphere == null))
				{
					cwPaintSphere.Group = new CwGroup(_polishGroupIndex);
					cwPaintSphere.Color = Color.white;
					cwPaintSphere.Opacity = _opacity;
					cwPaintSphere.Hardness = _hardness;
					cwPaintSphere.BlendMode = CwBlendMode.Additive(new Vector4(1f, 0f, 0f, 0f));
				}
			}
		}

		protected override void SetupPaintSpheresOnToolTip()
		{
			if (!(_toolTip == null))
			{
				CwPaintSphere cwPaintSphere = CreateOrFindSphere("Sphere_Polish");
				_paintSpheres = new CwPaintSphere[1] { cwPaintSphere };
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

		public override void EnableTool()
		{
			base.EnableTool();
			if (_isToolActive)
			{
				discAnimator?.StartAcceleration();
				StartShake();
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
			StopShake();
			StopLoopAudio();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			StopLoopAudio();
		}

		protected override void Update()
		{
			base.Update();
			if (!base.isOwned)
			{
				if (_isToolActive && !_wasToolActiveForDisc)
				{
					discAnimator?.StartAcceleration();
					StartShake();
					StartLoopAudio();
				}
				else if (!_isToolActive && _wasToolActiveForDisc)
				{
					discAnimator?.StartDeceleration();
					StopShake();
					StopLoopAudio();
				}
				_wasToolActiveForDisc = _isToolActive;
			}
		}

		private void StartLoopAudio()
		{
			if (AudioManager != null && polishLoopSound.IsValid() && (!_loopPlaying || !_loopHandle.IsValid))
			{
				if (_loopHandle.IsValid)
				{
					AudioManager.ReleaseInstance(_loopHandle);
					_loopHandle = AudioHandle.Invalid;
				}
				_loopHandle = AudioManager.PlayLoopRegion(polishLoopSound, base.gameObject);
				AudioManager.SetParameter(_loopHandle, polishingParameter, value: true);
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

		private void StartShake()
		{
			Transform target = ((shakeTarget != null) ? shakeTarget : base.transform);
			_shakePositionTween.Stop();
			_shakeRotationTween.Stop();
			if (shakeStrength > 0f)
			{
				_shakePositionTween = Tween.ShakeLocalPosition(target, Vector3.one * shakeStrength, 1f, shakeFrequency, enableFalloff: true, Ease.Default, 0f, -1);
			}
			if (shakeRotationStrength > 0f)
			{
				_shakeRotationTween = Tween.ShakeLocalRotation(target, Vector3.one * shakeRotationStrength, 1f, shakeFrequency, enableFalloff: true, Ease.Default, 0f, -1);
			}
		}

		private void StopShake()
		{
			_shakePositionTween.Stop();
			_shakeRotationTween.Stop();
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
