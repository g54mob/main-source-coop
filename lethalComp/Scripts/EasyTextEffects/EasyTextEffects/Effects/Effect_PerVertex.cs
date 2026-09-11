using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace EasyTextEffects.Effects
{
	[CreateAssetMenu(fileName = "PerVertex", menuName = "Easy Text Effects/5. Per Vertex", order = 5)]
	public class Effect_PerVertex : TextEffectInstance
	{
		[Space(10f)]
		public List<TextEffectInstance> topLeftEffects = new List<TextEffectInstance>();

		[Space(10f)]
		public List<TextEffectInstance> topRightEffects = new List<TextEffectInstance>();

		[Space(10f)]
		public List<TextEffectInstance> bottomLeftEffects = new List<TextEffectInstance>();

		[Space(10f)]
		public List<TextEffectInstance> bottomRightEffects = new List<TextEffectInstance>();

		public override bool IsComplete => topLeftEffects.Concat(topRightEffects).Concat(bottomLeftEffects).Concat(bottomRightEffects)
			.Any((TextEffectInstance _effect) => _effect != null && _effect.IsComplete);

		private void OnValidate()
		{
			if (topLeftEffects.Contains(this))
			{
				Debug.LogError("Per Vertex effect can't contain itself");
				topLeftEffects.Remove(this);
			}
			if (topRightEffects.Contains(this))
			{
				Debug.LogError("Per Vertex effect can't contain itself");
				topRightEffects.Remove(this);
			}
			if (bottomLeftEffects.Contains(this))
			{
				Debug.LogError("Per Vertex effect can't contain itself");
				bottomLeftEffects.Remove(this);
			}
			if (bottomRightEffects.Contains(this))
			{
				Debug.LogError("Per Vertex effect can't contain itself");
				bottomRightEffects.Remove(this);
			}
		}

		public override void ApplyEffect(TMP_TextInfo _textInfo, int _charIndex, int _startVertex = 0, int _endVertex = 3)
		{
			if (CheckCanApplyEffect(_charIndex))
			{
				topLeftEffects.ForEach(delegate(TextEffectInstance _effect)
				{
					_effect?.ApplyEffect(_textInfo, _charIndex, 1, 1);
				});
				topRightEffects.ForEach(delegate(TextEffectInstance _effect)
				{
					_effect?.ApplyEffect(_textInfo, _charIndex, 2, 2);
				});
				bottomLeftEffects.ForEach(delegate(TextEffectInstance _effect)
				{
					_effect?.ApplyEffect(_textInfo, _charIndex, 0, 0);
				});
				bottomRightEffects.ForEach(delegate(TextEffectInstance _effect)
				{
					_effect?.ApplyEffect(_textInfo, _charIndex, 3);
				});
			}
		}

		public override void StartEffect(TextEffectEntry entry)
		{
			base.StartEffect(entry);
			foreach (List<TextEffectInstance> item in new List<List<TextEffectInstance>> { topLeftEffects, topRightEffects, bottomLeftEffects, bottomRightEffects })
			{
				foreach (TextEffectInstance item2 in item)
				{
					if ((bool)item2)
					{
						item2.startCharIndex = startCharIndex;
						item2.charLength = charLength;
						item2.StartEffect(entry);
					}
				}
			}
		}

		public override void StopEffect()
		{
			base.StopEffect();
			topLeftEffects.ForEach(delegate(TextEffectInstance _effect)
			{
				_effect?.StopEffect();
			});
			topRightEffects.ForEach(delegate(TextEffectInstance _effect)
			{
				_effect?.StopEffect();
			});
			bottomLeftEffects.ForEach(delegate(TextEffectInstance _effect)
			{
				_effect?.StopEffect();
			});
			bottomRightEffects.ForEach(delegate(TextEffectInstance _effect)
			{
				_effect?.StopEffect();
			});
		}

		public override TextEffectInstance Instantiate()
		{
			Effect_PerVertex effect_PerVertex = Object.Instantiate(this);
			effect_PerVertex.topLeftEffects = topLeftEffects.Select((TextEffectInstance _effect) => _effect?.Instantiate()).ToList();
			effect_PerVertex.topRightEffects = topRightEffects.Select((TextEffectInstance _effect) => _effect?.Instantiate()).ToList();
			effect_PerVertex.bottomLeftEffects = bottomLeftEffects.Select((TextEffectInstance _effect) => _effect?.Instantiate()).ToList();
			effect_PerVertex.bottomRightEffects = bottomRightEffects.Select((TextEffectInstance _effect) => _effect?.Instantiate()).ToList();
			return effect_PerVertex;
		}
	}
}
