namespace Febucci.UI.Core
{
	public abstract class BehaviorSine : BehaviorBase
	{
		protected float amplitude = 1f;

		protected float frequency = 1f;

		protected float waveSize = 0.08f;

		public override void SetModifier(string modifierName, string modifierValue)
		{
			switch (modifierName)
			{
			case "a":
				ApplyModifierTo(ref amplitude, modifierValue);
				break;
			case "f":
				ApplyModifierTo(ref frequency, modifierValue);
				break;
			case "w":
				ApplyModifierTo(ref waveSize, modifierValue);
				break;
			}
		}

		public override string ToString()
		{
			return $"freq: {frequency}\n" + $"ampl: {amplitude}\n" + $"waveSize: {waveSize}" + "\n" + base.ToString();
		}
	}
}
