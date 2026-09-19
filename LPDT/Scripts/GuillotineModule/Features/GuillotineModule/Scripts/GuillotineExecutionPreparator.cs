using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.GuillotineModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class GuillotineExecutionPreparator : NetworkBehaviour
	{
		[SerializeField]
		private LayerMask _executeLayerMask;

		[SerializeField]
		private Transform _defaultPoseBlueprint;

		[SerializeField]
		private float _executePreparationDuration = 0.5f;

		private GuillotineExecuteDataHolder _guillotineExecuteDataHolder;

		private readonly Dictionary<IGuillotineExecutable, Coroutine> _preparationCoroutines = new Dictionary<IGuillotineExecutable, Coroutine>();

		private readonly List<IGuillotineExecutable> _executablesToQuit = new List<IGuillotineExecutable>();

		[Inject]
		public void InjectDependencies(GuillotineExecuteDataHolder guillotineExecuteDataHolder)
		{
			_guillotineExecuteDataHolder = guillotineExecuteDataHolder;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			foreach (Coroutine value in _preparationCoroutines.Values)
			{
				StopCoroutine(value);
			}
			_preparationCoroutines.Clear();
			foreach (IGuillotineExecutable item in _guillotineExecuteDataHolder.GuillotineExecutablesInRange)
			{
				if (!(item?.NetworkObject == null) && item.NetworkObject.IsValid && item.NetworkObject.HasStateAuthority)
				{
					item.QuitExecution();
				}
			}
			_guillotineExecuteDataHolder.GuillotineExecutablesInRange.Clear();
		}

		private void Update()
		{
			ProcessQuitExecutionRequests();
		}

		private void OnTriggerEnter(Collider other)
		{
			if (TryGetGuillotineExecutable(other, out var guillotineExecutable))
			{
				StartPreparationIfNeeded(guillotineExecutable);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (TryGetGuillotineExecutable(other, out var guillotineExecutable))
			{
				if (_preparationCoroutines.Remove(guillotineExecutable, out var value))
				{
					StopCoroutine(value);
				}
				_guillotineExecuteDataHolder.GuillotineExecutablesInRange.Remove(guillotineExecutable);
			}
		}

		private void StartPreparationIfNeeded(IGuillotineExecutable guillotineExecutable)
		{
			if (!_preparationCoroutines.ContainsKey(guillotineExecutable))
			{
				_preparationCoroutines.Add(guillotineExecutable, StartCoroutine(RegisterExecutableAfterDelay(guillotineExecutable)));
			}
		}

		private IEnumerator RegisterExecutableAfterDelay(IGuillotineExecutable guillotineExecutable)
		{
			yield return new WaitForSeconds(_executePreparationDuration);
			_preparationCoroutines.Remove(guillotineExecutable);
			if (!_guillotineExecuteDataHolder.GuillotineExecutablesInRange.Contains(guillotineExecutable))
			{
				_guillotineExecuteDataHolder.GuillotineExecutablesInRange.Add(guillotineExecutable);
				if (guillotineExecutable.NetworkObject.HasStateAuthority)
				{
					guillotineExecutable.PrepareForExecution(_defaultPoseBlueprint);
				}
			}
		}

		private bool TryGetGuillotineExecutable(Collider other, out IGuillotineExecutable guillotineExecutable)
		{
			guillotineExecutable = null;
			if ((_executeLayerMask.value & (1 << other.gameObject.layer)) == 0)
			{
				return false;
			}
			if (other.TryGetComponent<IGuillotineExecutable>(out guillotineExecutable) && !guillotineExecutable.IsRuntime)
			{
				return guillotineExecutable.CanBeExecuted;
			}
			return false;
		}

		private void ProcessQuitExecutionRequests()
		{
			_executablesToQuit.Clear();
			foreach (IGuillotineExecutable item in _guillotineExecuteDataHolder.GuillotineExecutablesInRange)
			{
				if (item?.NetworkObject == null || !item.NetworkObject.IsValid)
				{
					_executablesToQuit.Add(item);
				}
				else if (item.ShouldQuitExecution)
				{
					_executablesToQuit.Add(item);
				}
			}
			foreach (IGuillotineExecutable item2 in _executablesToQuit)
			{
				QuitExecution(item2);
			}
		}

		private void QuitExecution(IGuillotineExecutable executable)
		{
			_guillotineExecuteDataHolder.GuillotineExecutablesInRange.Remove(executable);
			if (!(executable?.NetworkObject == null) && executable.NetworkObject.IsValid && executable.NetworkObject.HasStateAuthority)
			{
				executable.QuitExecution();
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
