using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using Portningsbolaget;
using UnityEngine;

public class EscapePlayerHandler : MonoBehaviour
{
	public GameObject cellPrefab;

	private Dictionary<int, EscapePlayerCellUI> m_PlayerCells = new Dictionary<int, EscapePlayerCellUI>();

	private List<int> m_cellsToRemove = new List<int>(4);

	private MonoBehaviour m_coroutineRunner;

	private Coroutine m_assignCoroutine;

	private Coroutine m_updateCoroutine;

	public MonoBehaviour CoroutineRunner => m_coroutineRunner;

	public void Initialise()
	{
		GameObject gameObject = new GameObject("[PlayerCell Runner]");
		m_coroutineRunner = gameObject.AddComponent<EmptyBehaviour>();
		Object.DontDestroyOnLoad(gameObject);
		m_updateCoroutine = m_coroutineRunner.StartCoroutine(OnUpdate());
	}

	private void OnDestroy()
	{
		if (m_assignCoroutine != null)
		{
			m_coroutineRunner?.StopCoroutine(m_assignCoroutine);
		}
		if (m_updateCoroutine != null)
		{
			m_coroutineRunner?.StopCoroutine(m_updateCoroutine);
		}
		if (m_coroutineRunner != null)
		{
			Object.Destroy(m_coroutineRunner.gameObject);
		}
	}

	private IEnumerator OnAssignPlayer()
	{
		yield return null;
	}

	private IEnumerator OnUpdate()
	{
		while (true)
		{
			UpdateList();
			yield return null;
		}
	}

	private void UpdateList()
	{
		Photon.Realtime.Player[] playerListOthers = PhotonNetwork.PlayerListOthers;
		Photon.Realtime.Player[] array = playerListOthers;
		foreach (Photon.Realtime.Player player in array)
		{
			if (!m_PlayerCells.ContainsKey(player.ActorNumber))
			{
				EscapePlayerCellUI component = Object.Instantiate(cellPrefab, base.transform).GetComponent<EscapePlayerCellUI>();
				component.Setup(this, player);
				m_PlayerCells.Add(player.ActorNumber, component);
			}
		}
		HashSet<int> hashSet = playerListOthers.Select((Photon.Realtime.Player player2) => player2.ActorNumber).ToHashSet();
		m_cellsToRemove.Clear();
		foreach (KeyValuePair<int, EscapePlayerCellUI> playerCell in m_PlayerCells)
		{
			if (hashSet.Contains(playerCell.Key))
			{
				playerCell.Value.UpdateCell();
			}
			else
			{
				m_cellsToRemove.Add(playerCell.Key);
			}
		}
		foreach (int item in m_cellsToRemove)
		{
			Object.Destroy(m_PlayerCells[item].gameObject);
			m_PlayerCells.Remove(item);
		}
	}
}
