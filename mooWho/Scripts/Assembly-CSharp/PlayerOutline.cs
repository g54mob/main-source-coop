using System.Collections.Generic;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(PlayerRoleData))]
public class PlayerOutline : NetworkBehaviour
{
	[Header("Outline")]
	public Outline outline;

	public Color outlineColor = Color.yellow;

	public float outlineWidth = 6f;

	[Header("Sinek Vurgusu")]
	[Tooltip("Sinek evresindeyken (PlayerVoiceMonitor) outline bu renge/kalınlığa/moda geçer ve rol matrisini es geçip HERKESE (avcı dahil) görünür — konumu ele veren mekanik zaten bu (bkz. flyGameObject).")]
	public Color flyOutlineColor = new Color(1f, 0.2314f, 0.2314f);

	public float flyOutlineWidth = 5f;

	public Outline.Mode flyOutlineMode;

	private bool _flyHighlightActive;

	private Outline.Mode _normalOutlineMode;

	private PlayerRoleData _roleData;

	private float _recheckTimer;

	private const float RecheckInterval = 0.3f;

	private static readonly List<PlayerOutline> _all = new List<PlayerOutline>();

	public static bool SuppressAll { get; private set; }

	public static void SetSuppressAll(bool suppress)
	{
		SuppressAll = suppress;
		foreach (PlayerOutline item in _all)
		{
			if (item != null)
			{
				item.RefreshOutlineVisibility();
			}
		}
	}

	private void OnEnable()
	{
		_all.Add(this);
	}

	private void OnDisable()
	{
		_all.Remove(this);
	}

	private void Awake()
	{
		_roleData = GetComponent<PlayerRoleData>();
		if (outline == null)
		{
			outline = GetComponent<Outline>();
		}
		if (outline != null)
		{
			_normalOutlineMode = outline.OutlineMode;
			outline.OutlineColor = outlineColor;
			outline.OutlineWidth = outlineWidth;
			outline.enabled = false;
		}
	}

	private void Update()
	{
		_recheckTimer -= Time.deltaTime;
		if (_recheckTimer <= 0f)
		{
			_recheckTimer = 0.3f;
			RefreshOutlineVisibility();
		}
	}

	public void RefreshOutline()
	{
		if (outline == null)
		{
			outline = GetComponent<Outline>();
		}
		if (!(outline == null))
		{
			outline.RebuildRenderers();
			_normalOutlineMode = outline.OutlineMode;
			if (_flyHighlightActive)
			{
				outline.OutlineColor = flyOutlineColor;
				outline.OutlineWidth = flyOutlineWidth;
				outline.OutlineMode = flyOutlineMode;
			}
			else
			{
				outline.OutlineColor = outlineColor;
				outline.OutlineWidth = outlineWidth;
			}
			RefreshOutlineVisibility();
		}
	}

	public void SetFlyHighlight(bool active)
	{
		_flyHighlightActive = active;
		if (outline != null)
		{
			outline.OutlineColor = (active ? flyOutlineColor : outlineColor);
			outline.OutlineWidth = (active ? flyOutlineWidth : outlineWidth);
			outline.OutlineMode = (active ? flyOutlineMode : _normalOutlineMode);
		}
		RefreshOutlineVisibility();
	}

	public void RefreshOutlineVisibility()
	{
		if (!(outline == null))
		{
			bool flag = (_flyHighlightActive ? EvaluateFlyVisibility() : EvaluateVisibility());
			if (outline.enabled != flag)
			{
				outline.enabled = flag;
			}
		}
	}

	private bool EvaluateFlyVisibility()
	{
		if (SuppressAll)
		{
			return false;
		}
		return true;
	}

	private bool EvaluateVisibility()
	{
		if (SuppressAll)
		{
			return false;
		}
		if (base.isLocalPlayer)
		{
			return false;
		}
		NetworkIdentity localPlayer = NetworkClient.localPlayer;
		if (localPlayer == null)
		{
			return false;
		}
		Health component = localPlayer.GetComponent<Health>();
		if (SpectatorController.IsSpectating && (component == null || component.IsDead))
		{
			Health component2 = GetComponent<Health>();
			if (component2 != null && component2.IsDead)
			{
				return false;
			}
			return true;
		}
		PlayerRoleData component3 = localPlayer.GetComponent<PlayerRoleData>();
		if (component3 == null || _roleData == null)
		{
			return false;
		}
		if (!component3.RolesLocked || !_roleData.RolesLocked)
		{
			return false;
		}
		Health component4 = GetComponent<Health>();
		if (component4 != null && component4.IsDead)
		{
			return false;
		}
		PlayerRole role = component3.Role;
		PlayerRole role2 = _roleData.Role;
		if (role == PlayerRole.Hunter)
		{
			return role2 == PlayerRole.Hunter;
		}
		return role2 == PlayerRole.Animal;
	}

	public override bool Weaved()
	{
		return true;
	}
}
