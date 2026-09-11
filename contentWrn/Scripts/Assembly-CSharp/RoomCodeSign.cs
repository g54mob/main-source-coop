using System.Collections;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class RoomCodeSign : MonoBehaviour
{
	public TMP_Text m_header;

	public TMP_Text m_roomCode;

	public TMP_Text m_offline;

	private IEnumerator Start()
	{
		while (!PhotonNetwork.InRoom)
		{
			yield return null;
		}
		m_roomCode.text = PhotonNetwork.CurrentRoom.Name;
	}

	private void LateUpdate()
	{
		if (PhotonNetwork.OfflineMode || SurfaceNetworkHandler.HasStarted)
		{
			m_header.gameObject.SetActive(value: false);
			m_roomCode.gameObject.SetActive(value: false);
			m_offline.gameObject.SetActive(value: true);
			base.enabled = false;
		}
	}
}
