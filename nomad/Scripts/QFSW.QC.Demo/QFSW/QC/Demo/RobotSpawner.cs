using QFSW.QC.Utilities;
using TMPro;
using UnityEngine;

namespace QFSW.QC.Demo
{
	public class RobotSpawner : MonoBehaviour
	{
		[SerializeField]
		private Robot robotPrefab;

		[SerializeField]
		private TextMeshProUGUI text;

		[SerializeField]
		private QuantumTheme theme;

		public int SpawnCount
		{
			[Command("demo.spawn-count", Platform.AllPlatforms, MonoTargetType.Single)]
			get;
			private set; }

		private void Start()
		{
			UpdateText();
			SpawnRobot(3);
			QuantumParser.AddNamespace("QFSW.QC.Demo");
		}

		private void UpdateText()
		{
			if (!theme)
			{
				text.text = $"{SpawnCount} robots spawned";
			}
			else
			{
				text.text = SpawnCount.ToString().ColorText(theme.DefaultReturnValueColor) + " robots spawned";
			}
		}

		[Command("demo.spawn-robot", MonoTargetType.Single, Platform.AllPlatforms)]
		private void SpawnRobot(int count = 1)
		{
			for (int i = 0; i < count; i++)
			{
				SpawnCount++;
				Vector3 position = base.transform.position;
				position += new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
				Object.Instantiate(robotPrefab, position, Quaternion.identity).name = $"Robot {SpawnCount}";
			}
			UpdateText();
		}
	}
}
