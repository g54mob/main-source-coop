using System.Collections.Generic;
using Mirror;
using UnityEngine;

[DefaultExecutionOrder(100)]
public class NameTagManager : MonoBehaviour
{
	[Header("Canvas (Screen Space - Overlay)")]
	public Canvas canvas;

	public NameTagUI tagPrefab;

	[Header("Mesafe")]
	public float visibleDistance = 40f;

	[Header("Ölçek (mesafeye göre)")]
	public float referenceDistance = 10f;

	public float minScale = 0.6f;

	public float maxScale = 1.6f;

	[Header("Yumuşatma (glitch önleme)")]
	[Tooltip("Ekran pozisyonu yumuşatma hızı (0 = kapalı)")]
	public float positionSmoothing = 20f;

	[Tooltip("Kamera arkası geçişinde gizlemek için z eşiği")]
	public float minDepth = 0.2f;

	private readonly List<NameTag> _tags = new List<NameTag>();

	private Transform _camTransform;

	private Camera _cam;

	private PlayerRoleData _localRole;

	private float _localRoleCacheTime;

	private bool _suppressAll;

	public static NameTagManager Instance { get; private set; }

	private void Awake()
	{
		Instance = this;
	}

	public void SetSuppressAll(bool suppress)
	{
		_suppressAll = suppress;
		if (!suppress)
		{
			return;
		}
		foreach (NameTag tag in _tags)
		{
			if (tag != null && tag.UI != null && tag.UI.gameObject.activeSelf)
			{
				tag.UI.gameObject.SetActive(value: false);
			}
		}
	}

	public void Register(NameTag tag)
	{
		if (!_tags.Contains(tag))
		{
			_tags.Add(tag);
		}
	}

	public void Unregister(NameTag tag)
	{
		_tags.Remove(tag);
		if (tag != null && tag.UI != null)
		{
			Object.Destroy(tag.UI.gameObject);
		}
	}

	private void LateUpdate()
	{
		if (_suppressAll)
		{
			return;
		}
		if (_camTransform == null || _cam == null)
		{
			ResolveLocalCamera();
			if (_cam == null)
			{
				return;
			}
		}
		if (Time.time - _localRoleCacheTime > 0.2f)
		{
			_localRole = GetLocalPlayerRole();
			_localRoleCacheTime = Time.time;
		}
		Vector3 position = _camTransform.position;
		for (int i = 0; i < _tags.Count; i++)
		{
			NameTag nameTag = _tags[i];
			if (nameTag == null || nameTag.UI == null)
			{
				continue;
			}
			Vector3 anchorPosition = nameTag.GetAnchorPosition();
			float num = Vector3.Distance(position, anchorPosition);
			bool flag = EvaluateTag(nameTag, num);
			Vector3 zero = Vector3.zero;
			if (flag)
			{
				zero = _cam.WorldToScreenPoint(anchorPosition);
				if (zero.z < minDepth)
				{
					flag = false;
				}
				else if (float.IsNaN(zero.x) || float.IsNaN(zero.y))
				{
					flag = false;
				}
			}
			if (nameTag.UI.gameObject.activeSelf != flag)
			{
				nameTag.UI.gameObject.SetActive(flag);
			}
			if (flag)
			{
				nameTag.UI.UpdateWorldPosition(anchorPosition, _cam, positionSmoothing);
				float scale = Mathf.Lerp(maxScale, minScale, Mathf.InverseLerp(referenceDistance, visibleDistance, num));
				nameTag.UI.SetScale(scale);
				nameTag.UI.SetDeadAlpha(nameTag.IsDead);
				nameTag.UI.SetBuzzWarning(nameTag.BuzzWarningSeconds);
				nameTag.UI.SetSpeaking(nameTag.IsSpeaking);
			}
		}
	}

	private bool EvaluateTag(NameTag tag, float dist)
	{
		if (tag.IsLocalPlayerTag)
		{
			return false;
		}
		if (_localRole == null || tag.RoleData == null)
		{
			return false;
		}
		if (string.IsNullOrEmpty(tag.GetUsername()))
		{
			return false;
		}
		if (dist > visibleDistance)
		{
			return false;
		}
		if (!_localRole.RolesLocked || !tag.RoleData.RolesLocked)
		{
			return true;
		}
		PlayerRole role = _localRole.Role;
		PlayerRole role2 = tag.RoleData.Role;
		if (role == PlayerRole.Hunter)
		{
			return role2 == PlayerRole.Hunter;
		}
		return role2 == PlayerRole.Animal;
	}

	private void ResolveLocalCamera()
	{
		if (!(NetworkClient.localPlayer == null))
		{
			NetworkedCameraController component = NetworkClient.localPlayer.GetComponent<NetworkedCameraController>();
			if (component != null && component.playerCamera != null)
			{
				_cam = component.playerCamera;
				_camTransform = _cam.transform;
			}
			else if (Camera.main != null)
			{
				_cam = Camera.main;
				_camTransform = _cam.transform;
			}
		}
	}

	private PlayerRoleData GetLocalPlayerRole()
	{
		if (NetworkClient.localPlayer == null)
		{
			return null;
		}
		return NetworkClient.localPlayer.GetComponent<PlayerRoleData>();
	}
}
