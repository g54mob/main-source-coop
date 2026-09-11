using System.Collections.Generic;
using UnityEngine;
using pworld.Scripts.Extensions;

namespace pworld.Scripts
{
	public class TLSEntries : ScriptableObject
	{
		public List<SerializedCollection<GameObject>> back = new List<SerializedCollection<GameObject>>();

		public List<SerializedCollection<GameObject>> forward = new List<SerializedCollection<GameObject>>();
	}
}
