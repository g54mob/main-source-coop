using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace EasyTextEffects.Effects
{
	[CreateAssetMenu(fileName = "Composite", menuName = "Easy Text Effects/6. Composite", order = 6)]
	public class Effect_Composite : TextEffectInstance
	{
		[Space(10f)]
		public List<TextEffectInstance> effects = new List<TextEffectInstance>();

		public override bool IsComplete => effects.Any((TextEffectInstance _effect) => _effect != null && _effect.IsComplete);

		private void OnValidate()
		{
			if (effects.Contains(this))
			{
				Debug.LogError("Composite effect can't contain itself");
				effects.Remove(this);
			}
		}

		public override void ApplyEffect(TMP_TextInfo _textInfo, int _charIndex, int _startVertex = 0, int _endVertex = 3)
		{
			if (!CheckCanApplyEffect(_charIndex))
			{
				return;
			}
			foreach (TextEffectInstance effect in effects)
			{
				if ((bool)effect)
				{
					effect.ApplyEffect(_textInfo, _charIndex, _startVertex, _endVertex);
				}
			}
		}

		public override void StartEffect(TextEffectEntry entry)
		{
			base.StartEffect(entry);
			foreach (TextEffectInstance effect in effects)
			{
				if ((bool)effect)
				{
					effect.startCharIndex = startCharIndex;
					effect.charLength = charLength;
					effect.StartEffect(entry);
				}
			}
		}

		public override void StopEffect()
		{
			base.StopEffect();
			foreach (TextEffectInstance effect in effects)
			{
				if ((bool)effect)
				{
					effect.StopEffect();
				}
			}
		}

		public override TextEffectInstance Instantiate()
		{
			Effect_Composite effect_Composite = Object.Instantiate(this);
			effect_Composite.effects = new List<TextEffectInstance>();
			foreach (TextEffectInstance effect in effects)
			{
				if ((bool)effect)
				{
					effect_Composite.effects.Add(effect.Instantiate());
				}
			}
			return effect_Composite;
		}
	}
}
