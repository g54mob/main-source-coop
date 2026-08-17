using FMOD;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.Serialization;

namespace FMODUnity
{
	[AddComponentMenu("FMOD Studio/FMOD Studio Global Parameter Trigger")]
	public class StudioGlobalParameterTrigger : EventHandler
	{
		[ParamRef]
		[FormerlySerializedAs("parameter")]
		public string Parameter;

		public EmitterGameEvent TriggerEvent;

		[FormerlySerializedAs("value")]
		public float Value;

		private PARAMETER_DESCRIPTION parameterDescription;

		public PARAMETER_DESCRIPTION ParameterDescription => parameterDescription;

		private RESULT Lookup()
		{
			return RuntimeManager.StudioSystem.getParameterDescriptionByName(Parameter, out parameterDescription);
		}

		private void Awake()
		{
			if (string.IsNullOrEmpty(parameterDescription.name))
			{
				Lookup();
			}
		}

		protected override void HandleGameEvent(EmitterGameEvent gameEvent)
		{
			if (TriggerEvent == gameEvent)
			{
				TriggerParameters();
			}
		}

		public void TriggerParameters()
		{
			if (!string.IsNullOrEmpty(Parameter))
			{
				RESULT rESULT = RuntimeManager.StudioSystem.setParameterByID(parameterDescription.id, Value);
				if (rESULT != RESULT.OK)
				{
					RuntimeUtils.DebugLogError($"[FMOD] StudioGlobalParameterTrigger failed to set parameter {Parameter} : result = {rESULT}");
				}
			}
		}
	}
}
