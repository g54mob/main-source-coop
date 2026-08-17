using System;
using System.Collections.Generic;
using UnityEngine;

namespace EvilCore.EvilSave.Tests
{
	[Serializable]
	public class PlayerSaveData
	{
		public string playerName;

		public int level;

		public float experience;

		public Vector3 position;

		public Quaternion rotation;

		public List<string> inventory;

		public Dictionary<string, int> stats;

		public TestIntEnum state;
	}
}
