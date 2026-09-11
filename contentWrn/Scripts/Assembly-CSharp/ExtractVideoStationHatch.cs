using System.Collections;
using DefaultNamespace;
using Photon.Pun;
using UnityEngine;
using Zorro.Core;

public class ExtractVideoStationHatch : MonoBehaviour, IThrowTarget
{
	public ExtractVideoMachine Machine;

	public bool m_opened;

	public float Spring = 10f;

	public float Drag = 0.1f;

	public float m_openedRotation;

	public float m_closedRotation;

	private bool onCooldown;

	private OneDPhysicsSpring m_spring;

	[SerializeField]
	private float m_throwImpactForce = 100f;

	private void Awake()
	{
		m_spring = new OneDPhysicsSpring
		{
			Spring = Spring,
			Drag = Drag
		};
	}

	private void Update()
	{
		m_spring.Drag = Drag;
		m_spring.Spring = Spring;
		m_spring.Update();
		base.transform.localRotation = Quaternion.Euler(new Vector3(m_spring.Current, 0f, 0f));
	}

	private void FixedUpdate()
	{
		m_spring.Target = (m_opened ? m_openedRotation : m_closedRotation);
		m_spring.FixedUpdate();
		m_spring = SpringClamp.BounceClamp(m_spring, m_openedRotation - 5.3f, m_closedRotation);
	}

	public void HitByThrowable(ItemInstance item)
	{
		if (m_opened && !onCooldown)
		{
			m_spring.AddForce(m_throwImpactForce);
			if (PhotonNetwork.IsMasterClient)
			{
				StartCoroutine(CloseDelayed());
			}
		}
		StartCoroutine(Cooldown());
		IEnumerator CloseDelayed()
		{
			yield return new WaitForSeconds(0.15f);
			if (Machine.StateMachine.CurrentState is ExtractMachineIdleState)
			{
				Machine.StateMachine.SwitchState<ExtractMachineCheckItemState>();
			}
		}
		IEnumerator Cooldown()
		{
			onCooldown = true;
			yield return new WaitForSeconds(0.7f);
			onCooldown = false;
		}
	}

	public void Close()
	{
		m_opened = false;
		if (PhotonNetwork.IsMasterClient)
		{
			Machine.SyncHatchState();
		}
	}

	public bool IsFullyClosed()
	{
		if (!(m_spring.Current > -0.1f))
		{
			return Mathf.Abs(m_spring.Velocity) < 0.1f;
		}
		return true;
	}

	public void Open()
	{
		m_opened = true;
		if (PhotonNetwork.IsMasterClient)
		{
			Machine.SyncHatchState();
		}
	}

	public bool IsHalfwayOpen()
	{
		return m_spring.Current < -50f;
	}
}
