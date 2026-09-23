using System.Collections.Generic;
using DG.Tweening;
using Mimicraft.Networking;
using Mimicraft.VoxelEditor;
using Mimicraft.VoxelEditor.Core;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	public class WallClingSquash : MonoBehaviour
	{
		[Header("Şiddet")]
		[Tooltip("Modelin yapıştığı yüzeye doğru ne kadar ezildiği (oran). 0.18 = %18 incelme.")]
		[SerializeField]
		[Range(0f, 0.5f)]
		private float squash = 0.18f;

		[Tooltip("Ezilirken diğer iki eksende ne kadar şiştiği (oran). Hacim korunumu şart değil - hangisi daha iyi hissettiriyorsa.")]
		[SerializeField]
		[Range(0f, 0.5f)]
		private float stretch = 0.1f;

		[Header("Zamanlama")]
		[Tooltip("Tüm salınımın süresi (saniye). Sonunda ölçek tam olarak normale döner.")]
		[SerializeField]
		[Min(0.05f)]
		private float duration = 0.45f;

		[Tooltip("Elastik salınımın periyodu. Küçük değer = daha sık, daha sert titreşim. 0 = DOTween'in kendi varsayılanı.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float wobblePeriod = 0.4f;

		[Tooltip("İki yapışma arasında en az bu kadar saniye geçmeli. Bir köşede duvarla tavan arasında gidip gelmek yoksa sürekli yeniden tetikler.")]
		[SerializeField]
		[Min(0f)]
		private float cooldown = 0.3f;

		private PlayerMovement movement;

		private PlayerVoxelBody voxelBody;

		private PlayerEditSession editSession;

		private Tween pulse;

		private byte lastGrabCount;

		private bool hasGrabCount;

		private float nextAllowedTime;

		private readonly List<VoxelModel> pieces = new List<VoxelModel>();

		private readonly List<Vector3> pivots = new List<Vector3>();

		private readonly List<Vector3> appliedOffsets = new List<Vector3>();

		private int pulseBodyVersion = -1;

		private void Awake()
		{
			movement = GetComponent<PlayerMovement>();
			voxelBody = GetComponent<PlayerVoxelBody>();
			editSession = GetComponent<PlayerEditSession>();
		}

		private void OnDisable()
		{
			Stop();
		}

		private void LateUpdate()
		{
			if (movement != null)
			{
				byte wallGrabCount = movement.WallGrabCount;
				bool num = hasGrabCount && wallGrabCount != lastGrabCount;
				lastGrabCount = wallGrabCount;
				hasGrabCount = true;
				if (num)
				{
					Play();
				}
			}
			if (pulse != null && ((editSession != null && editSession.IsEditing) || voxelBody == null || !voxelBody.HasVoxelBody || voxelBody.BodyVersion != pulseBodyVersion))
			{
				Stop();
			}
		}

		private void Play()
		{
			if (voxelBody == null || !voxelBody.HasVoxelBody || (editSession != null && editSession.IsEditing) || Time.time < nextAllowedTime)
			{
				return;
			}
			nextAllowedTime = Time.time + cooldown;
			Stop();
			foreach (VoxelModel item in voxelBody.BodyPieces())
			{
				pieces.Add(item);
				pivots.Add(Middle(item));
				appliedOffsets.Add(Vector3.zero);
			}
			if (pieces.Count != 0)
			{
				pulseBodyVersion = voxelBody.BodyVersion;
				pulse = DOVirtual.Float(1f, 0f, duration, Write).SetEase(Ease.OutElastic, 1f, wobblePeriod).SetLink(base.gameObject)
					.OnKill(delegate
					{
						pulse = null;
					});
			}
		}

		private void Stop()
		{
			if (pulse != null)
			{
				Tween t = pulse;
				pulse = null;
				t.Kill();
			}
			Write(0f);
			pieces.Clear();
			pivots.Clear();
			appliedOffsets.Clear();
			pulseBodyVersion = -1;
		}

		private void Write(float amount)
		{
			for (int i = 0; i < pieces.Count; i++)
			{
				VoxelModel voxelModel = pieces[i];
				if (!(voxelModel == null))
				{
					Vector3 vector = Vector3.one * voxelModel.VoxelSize;
					Vector3 vector2 = vector + Vector3.Scale(vector, Pulse(voxelModel)) * amount;
					Vector3 vector3 = voxelModel.transform.localRotation * Vector3.Scale(vector - vector2, pivots[i]);
					voxelModel.transform.localScale = vector2;
					voxelModel.transform.localPosition += vector3 - appliedOffsets[i];
					appliedOffsets[i] = vector3;
				}
			}
		}

		private static Vector3 Middle(VoxelModel piece)
		{
			if (piece.Grid == null || !GridBounds.TryCompute(piece.Grid, out var min, out var max))
			{
				return Vector3.zero;
			}
			return new Vector3(min.x + max.x + 1, min.y + max.y + 1, min.z + max.z + 1) * 0.5f;
		}

		private Vector3 Pulse(VoxelModel piece)
		{
			Vector3 vector = Quaternion.Inverse(piece.transform.localRotation) * Vector3.forward;
			return new Vector3(Mathf.Lerp(stretch, 0f - squash, Mathf.Abs(vector.x)), Mathf.Lerp(stretch, 0f - squash, Mathf.Abs(vector.y)), Mathf.Lerp(stretch, 0f - squash, Mathf.Abs(vector.z)));
		}
	}
}
