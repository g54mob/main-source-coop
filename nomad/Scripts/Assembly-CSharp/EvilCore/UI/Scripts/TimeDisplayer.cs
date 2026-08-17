using NomadDrive.Managers.GameTime;
using TMPro;
using UnityEngine;
using VContainer;

namespace EvilCore.UI.Scripts
{
	public class TimeDisplayer : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI inGameTimeText;

		[SerializeField]
		private bool use12HourFormat;

		private ITimeManager _timeManager;

		[Inject]
		public void Construct(ITimeManager timeManager)
		{
			_timeManager = timeManager;
		}

		private void Start()
		{
			_timeManager.OnMinutePassed.AddListener(DisplayTime);
		}

		private void OnDisable()
		{
			if (_timeManager != null)
			{
				_timeManager.OnMinutePassed.RemoveListener(DisplayTime);
			}
		}

		private void DisplayTime()
		{
			inGameTimeText.text = (use12HourFormat ? Format12Hour(_timeManager.CurrentTimeString) : _timeManager.CurrentTimeString);
		}

		private static string Format12Hour(string timeString)
		{
			if (string.IsNullOrEmpty(timeString))
			{
				return timeString;
			}
			int num = timeString.IndexOf(':');
			if (num <= 0)
			{
				return timeString;
			}
			if (!int.TryParse(timeString.Substring(0, num), out var result))
			{
				return timeString;
			}
			if (!int.TryParse(timeString.Substring(num + 1), out var result2))
			{
				return timeString;
			}
			string arg = ((result < 12) ? "AM" : "PM");
			int num2 = result % 12;
			if (num2 == 0)
			{
				num2 = 12;
			}
			return $"{num2}:{result2:D2} {arg}";
		}
	}
}
