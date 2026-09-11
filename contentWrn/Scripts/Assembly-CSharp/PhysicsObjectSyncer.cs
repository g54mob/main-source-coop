using Photon.Pun;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using Zorro.Core;
using Zorro.Core.Serizalization;

public class PhysicsObjectSyncer : MonoBehaviour, IPunObservable
{
	private Rigidbody m_rigidbody;

	private PhotonView m_photonView;

	private Vector3 m_newPos;

	private Vector3 m_oldPos;

	private Quaternion m_newRot;

	private Quaternion m_oldRot;

	private float m_tickTime;

	private bool m_firstRead;

	private float m_lastTickRead;

	private void Awake()
	{
		m_photonView = GetComponent<PhotonView>();
		m_tickTime = 1f / (float)PhotonNetwork.SerializationRate;
	}

	private void Update()
	{
		if (m_rigidbody == null)
		{
			m_rigidbody = GetComponentInChildren<Rigidbody>();
		}
		if (m_rigidbody == null)
		{
			return;
		}
		if (m_photonView.IsMine)
		{
			if (m_rigidbody.transform.position.y < -200f && !PhotonGameLobbyHandler.IsSurface)
			{
				PhotonNetwork.Destroy(m_photonView.gameObject);
			}
			return;
		}
		m_rigidbody.isKinematic = !m_photonView.IsMine;
		if (!m_firstRead)
		{
			float t = Mathf.InverseLerp(m_lastTickRead, m_lastTickRead + m_tickTime, Time.time);
			m_rigidbody.transform.SetPositionAndRotation(Vector3.Lerp(m_oldPos, m_newPos, t), Quaternion.Lerp(m_oldRot, m_newRot, t));
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (m_rigidbody == null)
		{
			return;
		}
		if (stream.IsWriting)
		{
			BinarySerializer binarySerializer = new BinarySerializer(12, Allocator.Temp);
			Vector3 position = m_rigidbody.transform.position;
			binarySerializer.WriteHalf((half)position.x);
			binarySerializer.WriteHalf((half)position.y);
			binarySerializer.WriteHalf((half)position.z);
			byte[] obj = binarySerializer.buffer.ToByteArray();
			stream.SendNext(obj);
			stream.SendNext(m_rigidbody.transform.rotation);
			binarySerializer.Dispose();
			return;
		}
		m_oldPos = m_newPos;
		m_oldRot = m_newRot;
		BinaryDeserializer binaryDeserializer = new BinaryDeserializer((byte[])stream.ReceiveNext(), Allocator.Temp);
		half half5 = binaryDeserializer.ReadHalf();
		half half6 = binaryDeserializer.ReadHalf();
		m_newPos = new Vector3(z: binaryDeserializer.ReadHalf(), x: half5, y: half6);
		m_newRot = (Quaternion)stream.ReceiveNext();
		if (m_firstRead)
		{
			m_firstRead = false;
			m_oldPos = m_newPos;
			m_oldRot = m_newRot;
		}
		m_lastTickRead = Time.time;
		binaryDeserializer.Dispose();
	}

	public void SetInitialPos(Vector3 pos, Quaternion rot)
	{
		m_firstRead = false;
		m_newPos = pos;
		m_newRot = rot;
		m_oldPos = pos;
		m_oldRot = rot;
	}
}
