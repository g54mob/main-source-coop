using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace Edgegap.Editor
{
	public class ButtonShaker
	{
		private const string SHAKE_START_CLASS = "shakeStart";

		private const string SHAKE_STOP_CLASS = "shakeEnd";

		private Button targetButton;

		public ButtonShaker(Button buttonToShake)
		{
			targetButton = buttonToShake;
		}

		public async Task ApplyShakeAsync(int msDelayBetweenShakes = 40, int iterations = 2)
		{
			for (int i = 0; i < iterations; i++)
			{
				await shakeOnce(msDelayBetweenShakes);
			}
		}

		private async Task shakeOnce(int msDelayBetweenShakes)
		{
			targetButton.AddToClassList("shakeStart");
			await Task.Delay(msDelayBetweenShakes);
			targetButton.RemoveFromClassList("shakeStart");
			targetButton.AddToClassList("shakeEnd");
			await Task.Delay(msDelayBetweenShakes);
			targetButton.RemoveFromClassList("shakeEnd");
		}
	}
}
