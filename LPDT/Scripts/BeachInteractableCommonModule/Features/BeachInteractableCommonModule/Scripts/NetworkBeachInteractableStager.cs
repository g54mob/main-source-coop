using System;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.BeachInteractableCommonModule.Scripts
{
	[NetworkBehaviourWeaved(3)]
	public class NetworkBeachInteractableStager : NetworkBehaviour
	{
		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("BeachInteractableIdentifier", 0, 3)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private BeachInteractableIdentifier _BeachInteractableIdentifier;

		private StagingBeachInteractableModel _stagingBeachInteractableModel;

		[Networked]
		[NetworkedWeaved(0, 3)]
		public unsafe BeachInteractableIdentifier BeachInteractableIdentifier
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing NetworkBeachInteractableStager.BeachInteractableIdentifier. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(BeachInteractableIdentifier*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing NetworkBeachInteractableStager.BeachInteractableIdentifier. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(BeachInteractableIdentifier*)((byte*)Ptr + 0) = value;
			}
		}

		[Inject]
		public void InjectDependencies(StagingBeachInteractableModel stagingBeachInteractableModel)
		{
			_stagingBeachInteractableModel = stagingBeachInteractableModel;
		}

		public override void Spawned()
		{
			_stagingBeachInteractableModel.EnqueueNetworkBeachInteractable(BeachInteractableIdentifier, base.Object);
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_stagingBeachInteractableModel.RemoveStagingBeachInteractableIfCurrent(BeachInteractableIdentifier, base.Object);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			BeachInteractableIdentifier = _BeachInteractableIdentifier;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_BeachInteractableIdentifier = BeachInteractableIdentifier;
		}
	}
}
