using Photon.Pun;
using UnityEngine;
using Zorro.Core;

public class CDRom : Interactable
{
	public GameObject m_CD;

	public bool m_opened;

	public float Spring = 10f;

	public float Drag = 0.1f;

	public float m_openedPos;

	public float m_closedPos;

	private OneDPhysicsSpring m_spring;

	private Vector3 m_localPos;

	public bool hasCD;

	public ExtractVideoMachine Machine;

	protected override void Awake()
	{
		base.Awake();
		m_localPos = base.transform.localPosition;
		m_spring = new OneDPhysicsSpring
		{
			Spring = Spring,
			Drag = Drag
		};
	}

	private void Start()
	{
		hoverText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.PickUp) + " CD";
	}

	public override bool IsValid(Player player)
	{
		return hasCD;
	}

	public override void Interact(Player player)
	{
		Machine.TryPickupCD(player);
	}

	private void Update()
	{
		m_spring.Drag = Drag;
		m_spring.Spring = Spring;
		m_spring.Update();
		base.transform.localPosition = m_localPos + Vector3.up * m_spring.Current;
		m_CD.SetActive(hasCD);
	}

	private void FixedUpdate()
	{
		m_spring.Target = (m_opened ? m_openedPos : m_closedPos);
		m_spring.FixedUpdate();
	}

	public void Open()
	{
		m_opened = true;
		if (PhotonNetwork.IsMasterClient)
		{
			Machine.SyncCDRomState();
		}
	}

	public void Close()
	{
		m_opened = false;
		if (PhotonNetwork.IsMasterClient)
		{
			Machine.SyncCDRomState();
		}
	}

	public void SetRom(Optionable<VideoHandle> cd)
	{
		hasCD = cd.IsSome;
	}
}
