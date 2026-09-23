using DG.Tweening;
using Mimicraft.Localization;
using Mimicraft.Networking;
using Mimicraft.VoxelEditor;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.UI
{
	public class SizeMultiplierView : MonoBehaviour
	{
		[Tooltip("Left empty, this object's own TextMeshProUGUI is used.")]
		[SerializeField]
		private TextMeshProUGUI label;

		[Tooltip("Colour by size. Left key is the smallest body, right key the largest - a calm colour rising to a warning one reads as 'this is getting risky' without a legend.")]
		[SerializeField]
		private Gradient sizeGradient = new Gradient();

		[Tooltip("Size ratio the gradient's right-hand end corresponds to.")]
		[SerializeField]
		[Min(1.1f)]
		private float gradientMaxRatio = 3f;

		[Tooltip("How much of a change is worth animating. Below this the number is still updated, just without the punch - a body being extruded voxel by voxel would otherwise punch on every single step.")]
		[SerializeField]
		[Min(0.001f)]
		private float punchThreshold = 0.02f;

		[SerializeField]
		[Min(0.05f)]
		private float punchDuration = 0.25f;

		[Tooltip("Left empty, found in the scene.")]
		[SerializeField]
		private RoundManager roundManager;

		private PlayerVoxelBody body;

		private float appliedRatio = -1f;

		private Tween punch;

		private void Awake()
		{
			if (label == null)
			{
				label = GetComponent<TextMeshProUGUI>();
			}
			if (label == null)
			{
				base.enabled = false;
			}
		}

		private void OnDestroy()
		{
			punch?.Kill();
		}

		private void Update()
		{
			if (!ShouldShow())
			{
				SetShown(shown: false);
				appliedRatio = -1f;
				return;
			}
			SetShown(shown: true);
			float bodySizeRatio = body.BodySizeRatio;
			if (!Mathf.Approximately(bodySizeRatio, appliedRatio))
			{
				bool flag = appliedRatio >= 0f && Mathf.Abs(bodySizeRatio - appliedRatio) >= punchThreshold;
				appliedRatio = bodySizeRatio;
				label.text = Loc.Format("SizeMultiplier", bodySizeRatio.ToString("0.0"));
				label.color = sizeGradient.Evaluate(Mathf.InverseLerp(1f, gradientMaxRatio, bodySizeRatio));
				if (flag)
				{
					punch?.Kill(complete: true);
					punch = label.transform.DOPunchScale(Vector3.one * 0.25f, punchDuration, 8).SetEase(Ease.OutCubic).SetUpdate(isIndependentUpdate: true);
				}
			}
		}

		private bool ShouldShow()
		{
			if (VoxelEditorSettings.IsMovementMode)
			{
				return false;
			}
			if (roundManager == null)
			{
				roundManager = GameModeController.Current as RoundManager;
				if (roundManager == null)
				{
					return false;
				}
			}
			if (!roundManager.IsSpawned || roundManager.LocalRole != PlayerRole.Hider)
			{
				return false;
			}
			if (TryResolveBody())
			{
				return body.HasVoxelBody;
			}
			return false;
		}

		private bool TryResolveBody()
		{
			if (body != null)
			{
				return true;
			}
			NetworkManager singleton = NetworkManager.Singleton;
			if (singleton == null || !singleton.IsClient || singleton.LocalClient.PlayerObject == null)
			{
				return false;
			}
			body = singleton.LocalClient.PlayerObject.GetComponent<PlayerVoxelBody>();
			return body != null;
		}

		private void SetShown(bool shown)
		{
			if (label.gameObject.activeSelf != shown)
			{
				label.gameObject.SetActive(shown);
			}
		}
	}
}
