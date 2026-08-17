using System;
using UnityEngine;

namespace NWH.VehiclePhysics2
{
	[Serializable]
	public abstract class VehicleComponent
	{
		[NonSerialized]
		public VehicleController vehicleController;

		[NonSerialized]
		[Tooltip("    Contains info about component's state.")]
		public StateDefinition state = new StateDefinition();

		private bool _wasDisabledByParent;

		public bool IsActive
		{
			get
			{
				if (state.isEnabled)
				{
					return state.initialized;
				}
				return false;
			}
		}

		public virtual void VC_SetVehicleController(VehicleController vc)
		{
			vehicleController = vc;
		}

		public virtual void VC_LoadStateFromStateSettings()
		{
			string fullName = GetType().FullName;
			LoadStateFromDefinitionsFile(fullName, ref state);
		}

		protected virtual void VC_Initialize()
		{
			state.initialized = true;
		}

		public virtual void VC_Update()
		{
		}

		public virtual void VC_FixedUpdate()
		{
		}

		public virtual bool VC_Enable(bool calledByParent)
		{
			if (!state.isEnabled && calledByParent && !_wasDisabledByParent)
			{
				return false;
			}
			if (!state.initialized)
			{
				VC_Initialize();
				if (!state.initialized)
				{
					state.isEnabled = false;
					return false;
				}
			}
			state.isEnabled = true;
			return true;
		}

		public virtual bool VC_Disable(bool calledByParent)
		{
			if (!state.initialized)
			{
				return false;
			}
			if (!state.isEnabled)
			{
				return false;
			}
			_wasDisabledByParent = calledByParent;
			state.isEnabled = false;
			return true;
		}

		public virtual void VC_DrawGizmos()
		{
		}

		public virtual void VC_SetDefaults()
		{
		}

		public virtual void VC_Validate(VehicleController vc)
		{
		}

		private void LoadStateFromDefinitionsFile(string fullTypeName, ref StateDefinition state)
		{
			if (!(vehicleController.stateSettings == null))
			{
				StateDefinition definition = vehicleController.stateSettings.GetDefinition(fullTypeName);
				if (definition != null)
				{
					state.isEnabled = definition.isEnabled;
					state.lodIndex = definition.lodIndex;
					state.fullName = fullTypeName;
				}
				else
				{
					Debug.Log("State definition " + fullTypeName + " could not be loaded. Refreshing the list of available components.");
					vehicleController.stateSettings?.Reload();
				}
			}
		}

		public virtual void UpdateLOD()
		{
			if (!Application.isPlaying)
			{
				Debug.LogWarning("Trying to run UpdateState out of play mode on " + GetType().Name + ".");
			}
			else if (vehicleController == null)
			{
				Debug.LogWarning("Trying to run UpdateState with no VehicleController reference set on " + GetType().Name + ".");
			}
			else
			{
				if (state.lodIndex < 0)
				{
					return;
				}
				if (vehicleController.activeLODIndex <= state.lodIndex)
				{
					if (!state.isEnabled)
					{
						VC_Enable(calledByParent: false);
					}
				}
				else if (state.isEnabled)
				{
					VC_Disable(calledByParent: false);
				}
			}
		}

		public virtual void ToggleState()
		{
			if (state.isEnabled)
			{
				VC_Disable(calledByParent: false);
			}
			else
			{
				VC_Enable(calledByParent: false);
			}
		}
	}
}
