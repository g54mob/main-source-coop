using UnityEngine;

namespace NomadDrive.Features.Rope.Scripts
{
	public class BuildTelegraph : MonoBehaviour
	{
		public Transform[] connections;

		public Rope rope;

		private Transform GetConnection(Transform tm, string name)
		{
			for (int i = 0; i < tm.childCount; i++)
			{
				if (tm.GetChild(i).name == name)
				{
					return tm.GetChild(i);
				}
			}
			return null;
		}

		private void Start()
		{
			for (int i = 0; i < base.transform.childCount - 1; i++)
			{
				Transform child = base.transform.GetChild(i);
				Transform child2 = base.transform.GetChild(i + 1);
				for (int j = 0; j < connections.Length; j++)
				{
					Transform connection = GetConnection(child, connections[j].name);
					Transform connection2 = GetConnection(child2, connections[j].name);
					if ((bool)connection && (bool)connection2)
					{
						GameObject obj = Object.Instantiate(rope.gameObject);
						obj.SetActive(value: true);
						obj.transform.parent = connection;
						Rope component = obj.GetComponent<Rope>();
						component.SetStartAttach(connection);
						component.SetEndAttach(connection2);
					}
				}
			}
		}
	}
}
