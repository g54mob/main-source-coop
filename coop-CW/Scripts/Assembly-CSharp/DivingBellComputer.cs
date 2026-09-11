using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DivingBellComputer : MonoBehaviour
{
	public DivingBell m_divingBell;

	public TextMeshProUGUI m_statusText;

	public GameObject m_cellPrefab;

	public Dictionary<Player, DivingBellSuitCellUI> m_spawnedCells = new Dictionary<Player, DivingBellSuitCellUI>();

	private void LateUpdate()
	{
		m_divingBell.StateMachine.CurrentState.SetStatusText(m_statusText);
		if ((Time.frameCount + 2) % 8 != 0)
		{
			return;
		}
		List<Player> players = PlayerHandler.instance.players;
		foreach (Player item in players)
		{
			float dst = Vector3.Distance(item.refs.ragdoll.GetBodypart(BodypartType.Hip).transform.position, m_statusText.transform.position);
			if (m_spawnedCells.TryGetValue(item, out var value))
			{
				value.Set(item, dst);
				continue;
			}
			value = Object.Instantiate(m_cellPrefab, m_cellPrefab.transform.parent).GetComponent<DivingBellSuitCellUI>();
			value.gameObject.SetActive(value: true);
			value.Set(item, dst);
			m_spawnedCells.Add(item, value);
		}
		List<Player> list = new List<Player>();
		foreach (KeyValuePair<Player, DivingBellSuitCellUI> spawnedCell in m_spawnedCells)
		{
			if (!players.Contains(spawnedCell.Key))
			{
				Object.Destroy(spawnedCell.Value.gameObject);
				list.Add(spawnedCell.Key);
			}
		}
		foreach (Player item2 in list)
		{
			m_spawnedCells.Remove(item2);
		}
	}
}
