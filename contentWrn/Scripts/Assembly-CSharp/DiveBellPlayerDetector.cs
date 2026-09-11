using System.Collections.Generic;
using UnityEngine;

public class DiveBellPlayerDetector : MonoBehaviour
{
	public Transform[] m_detectors;

	private HashSet<Player> m_players = new HashSet<Player>(4);

	public ICollection<Player> CheckForPlayers()
	{
		List<Player> players = PlayerHandler.instance.players;
		m_players.Clear();
		Transform[] detectors = m_detectors;
		foreach (Transform transform in detectors)
		{
			float num = transform.lossyScale.x * 0.5f;
			num *= num;
			foreach (Player item in players)
			{
				if ((transform.position - item.Center()).sqrMagnitude < num)
				{
					m_players.Add(item);
				}
			}
		}
		return m_players;
	}
}
