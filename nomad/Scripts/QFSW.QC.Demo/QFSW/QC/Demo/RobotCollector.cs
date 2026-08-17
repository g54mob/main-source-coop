using QFSW.QC.Utilities;
using TMPro;
using UnityEngine;

namespace QFSW.QC.Demo
{
	public class RobotCollector : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI text;

		[SerializeField]
		private QuantumTheme theme;

		public int RescueCount
		{
			[Command("demo.rescue-count", Platform.AllPlatforms, MonoTargetType.Single)]
			get;
			set; }

		private void Start()
		{
			UpdateText();
		}

		private void UpdateText()
		{
			if (!theme)
			{
				text.text = $"{RescueCount} robots saved";
			}
			else
			{
				text.text = RescueCount.ToString().ColorText(theme.DefaultReturnValueColor) + " robots saved";
			}
		}

		private void OnCollisionEnter2D(Collision2D collision)
		{
			if (collision.gameObject.CompareTag("Player"))
			{
				collision.gameObject.GetComponent<Robot>().Die();
				RescueCount++;
				UpdateText();
			}
		}
	}
}
