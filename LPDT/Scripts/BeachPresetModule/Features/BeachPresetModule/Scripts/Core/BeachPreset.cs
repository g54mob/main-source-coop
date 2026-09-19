using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Features.BeachPresetModule.Scripts.Core
{
	[CreateAssetMenu(fileName = "BeachPreset_Default", menuName = "Configurations/Beach Presets/BeachPreset")]
	public class BeachPreset : ScriptableObject
	{
		[Tooltip("Designer-facing name for this beach preset.")]
		[SerializeField]
		private string _presetName;

		[Tooltip("Beach layout data used by relative placement behaviours.")]
		[SerializeField]
		private BeachLayout _layout = new BeachLayout();

		[Tooltip("Composable behaviours that are applied when this preset becomes active.")]
		[SerializeReference]
		private List<BeachBehaviour> _behaviours = new List<BeachBehaviour>();

		public string PresetName
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(_presetName))
				{
					return _presetName;
				}
				return base.name;
			}
		}

		public BeachLayout Layout => _layout;

		public IReadOnlyList<BeachBehaviour> Behaviours => _behaviours;

		public void Apply(BeachPresetRuntimeContext context, DiContainer container)
		{
			InjectBehaviours(container);
			foreach (BeachBehaviour item in GetEnabledBehavioursInApplyOrder())
			{
				item.Apply(context);
			}
		}

		public void Clear(BeachPresetRuntimeContext context)
		{
			foreach (BeachBehaviour item in GetEnabledBehavioursInClearOrder())
			{
				item.Clear(context);
			}
		}

		public BeachValidationResult ValidateAll()
		{
			BeachValidationResult beachValidationResult = new BeachValidationResult();
			if (_layout == null)
			{
				beachValidationResult.AddError("Beach layout is missing.");
			}
			else
			{
				beachValidationResult.Merge(_layout.Validate());
			}
			if (_behaviours == null || _behaviours.Count == 0)
			{
				beachValidationResult.AddWarning("Preset has no behaviours.");
				return beachValidationResult;
			}
			for (int i = 0; i < _behaviours.Count; i++)
			{
				BeachBehaviour beachBehaviour = _behaviours[i];
				if (beachBehaviour == null)
				{
					beachValidationResult.AddError($"Behaviour #{i + 1} is empty. Select a behaviour type or remove the entry.");
				}
				else
				{
					beachValidationResult.Merge(beachBehaviour.Validate(this));
				}
			}
			return beachValidationResult;
		}

		public IEnumerable<BeachBehaviour> GetEnabledBehavioursInApplyOrder()
		{
			return from behaviour in _behaviours
				where behaviour?.Enabled ?? false
				orderby behaviour.Order
				select behaviour;
		}

		public IEnumerable<BeachBehaviour> GetEnabledBehavioursInClearOrder()
		{
			return from behaviour in _behaviours
				where behaviour?.Enabled ?? false
				orderby behaviour.Order descending
				select behaviour;
		}

		internal void InjectBehaviours(DiContainer container)
		{
			if (container == null)
			{
				return;
			}
			foreach (BeachBehaviour item in _behaviours.Where((BeachBehaviour behaviour) => behaviour != null))
			{
				container.Inject(item);
				item.OnInjected();
			}
		}
	}
}
