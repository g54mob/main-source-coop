using System;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Features.ItemCollisionModule.Scripts;
using Features.ItemDamageModule.Scripts;
using Features.ItemsModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.GuillotineModule.Scripts.Items
{
	[NetworkBehaviourWeaved(1)]
	public class ItemGuillotineExecutable : NetworkBehaviour, IGuillotineExecutable
	{
		[SerializeField]
		private MonoItem _monoItem;

		[SerializeField]
		private Collider _itemCollider;

		[SerializeField]
		private EventReference _executionSound;

		[SerializeField]
		private SoundSourceBehaviour _soundSourceBehaviour;

		private IItemCostReduceService _itemCostReduceService;

		private IAudioService _audioService;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("ShouldQuitExecution", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _ShouldQuitExecution;

		[field: SerializeField]
		public bool IsRuntime { get; private set; }

		public NetworkObject NetworkObject => base.Object;

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe bool ShouldQuitExecution
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ItemGuillotineExecutable.ShouldQuitExecution. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean((int*)((byte*)Ptr + 0));
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ItemGuillotineExecutable.ShouldQuitExecution. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)((byte*)Ptr + 0) = new NetworkBool(value);
			}
		}

		public bool CanBeExecuted => true;

		[Inject]
		public void InjectDependencies(IItemCostReduceService itemCostReduceService, IAudioService audioService)
		{
			_itemCostReduceService = itemCostReduceService;
			_audioService = audioService;
		}

		public void PrepareForExecution(Transform poseBlueprint)
		{
		}

		public void Execute()
		{
			_itemCostReduceService.BreakItemCompletely(new ItemCollisionData(_monoItem, 0f, 1f, _itemCollider));
		}

		public void QuitExecution()
		{
		}

		public void PlayExecutionSound()
		{
			_audioService.PlayOneShotAttached(_executionSound, _soundSourceBehaviour);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			ShouldQuitExecution = _ShouldQuitExecution;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_ShouldQuitExecution = ShouldQuitExecution;
		}
	}
}
