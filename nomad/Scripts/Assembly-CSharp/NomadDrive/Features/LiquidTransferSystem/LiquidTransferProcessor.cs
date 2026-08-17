using Ami.BroAudio;
using Ami.BroAudio.Data;
using Cysharp.Threading.Tasks;
using EvilCore.Audio;
using UnityEngine;
using UnityEngine.Events;

namespace NomadDrive.Features.LiquidTransferSystem
{
	public class LiquidTransferProcessor
	{
		private readonly UnityEvent<ILiquidContainer> _onTransferStarted = new UnityEvent<ILiquidContainer>();

		private readonly UnityEvent _onTransferCompleted = new UnityEvent();

		private readonly UnityEvent _onTransferCanceled = new UnityEvent();

		private readonly UnityEvent _onTransferPaused = new UnityEvent();

		private readonly ILiquidTransferActions _sourceActions;

		private readonly ILiquidTransferActions _targetActions;

		private readonly LiquidContainerComponent _sourceComponent;

		private readonly LiquidContainerComponent _targetComponent;

		private readonly IAudioManager _audioManager;

		private AudioHandle _liquidTransferEventInstance;

		private AudioParameter _activeParameter;

		public ILiquidContainer Source { get; }

		public ILiquidContainer Target { get; }

		public float TransferredAmount { get; private set; }

		public float FillingSpeed { get; }

		public bool IsActive { get; private set; }

		public FuelTransferState State { get; private set; }

		public LiquidTransferProcessor(ILiquidContainer source, ILiquidContainer target, IAudioManager audioManager)
		{
			Source = source;
			Target = target;
			_sourceComponent = source as LiquidContainerComponent;
			_targetComponent = target as LiquidContainerComponent;
			FillingSpeed = source.FillingSpeed;
			State = FuelTransferState.Pending;
			_audioManager = audioManager;
			SubscribeTransferActions();
		}

		public LiquidTransferProcessor(ILiquidContainer target, float fillingSpeed, IAudioManager audioManager)
		{
			Source = null;
			Target = target;
			_targetComponent = target as LiquidContainerComponent;
			FillingSpeed = fillingSpeed;
			State = FuelTransferState.Pending;
			_audioManager = audioManager;
			SubscribeTransferActions();
		}

		private void SubscribeTransferActions()
		{
			if (_sourceActions != null)
			{
				_onTransferStarted.AddListener(_sourceActions.SetLiquidTransferStartedActions);
				_onTransferCompleted.AddListener(_sourceActions.SetLiquidTransferCompletedActions);
			}
			if (_targetActions != null)
			{
				_onTransferStarted.AddListener(_targetActions.SetLiquidTransferStartedActions);
				_onTransferCompleted.AddListener(_targetActions.SetLiquidTransferCompletedActions);
			}
		}

		private void UnsubscribeTransferActions()
		{
			if (_sourceActions != null)
			{
				_onTransferStarted.RemoveListener(_sourceActions.SetLiquidTransferStartedActions);
				_onTransferCompleted.RemoveListener(_sourceActions.SetLiquidTransferCompletedActions);
			}
			if (_targetActions != null)
			{
				_onTransferStarted.RemoveListener(_targetActions.SetLiquidTransferStartedActions);
				_onTransferCompleted.RemoveListener(_targetActions.SetLiquidTransferCompletedActions);
			}
		}

		public void Start()
		{
			FuelTransferState state = State;
			if (state == FuelTransferState.Cancelled || state == FuelTransferState.Completed || state == FuelTransferState.Failed || IsActive)
			{
				return;
			}
			if (GetTransferRequestResult() != TransferRequestResult.Allowed)
			{
				State = FuelTransferState.Failed;
				return;
			}
			StartTransferAudio();
			IsActive = true;
			State = FuelTransferState.InProgress;
			_onTransferStarted.Invoke(Target);
			if (Target.State == LiquidContainerState.Empty && Source.CurrentLiquidType != LiquidType.Empty)
			{
				Target.CmdSetLiquidType(Source.CurrentLiquidType);
			}
			TransferProcessFreelyAsync().Forget();
		}

		public void Start(float amountToComplete, LiquidType liquidType)
		{
			FuelTransferState state = State;
			if (state == FuelTransferState.Cancelled || state == FuelTransferState.Completed || state == FuelTransferState.Failed)
			{
				return;
			}
			IsActive = true;
			State = FuelTransferState.InProgress;
			_onTransferStarted.Invoke(Target);
			if (Target.State == LiquidContainerState.Empty)
			{
				LiquidType liquidType2 = Source?.CurrentLiquidType ?? liquidType;
				if (liquidType2 != LiquidType.Empty)
				{
					Target.CmdSetLiquidType(liquidType2);
				}
			}
			StartTransferAudio();
			TransferProcessByCompleteAmountAsync(amountToComplete).Forget();
		}

		private async UniTaskVoid TransferProcessFreelyAsync()
		{
			while (IsActive)
			{
				float num = FillingSpeed * Time.deltaTime;
				if (_sourceComponent != null && _targetComponent != null)
				{
					_sourceComponent.RequestTransferTo(_targetComponent, num);
					TransferredAmount += num;
				}
				else
				{
					float num2 = Source.Drain(num);
					float num3 = num - num2;
					float num4 = Target.Fill(num3);
					if (num4 > 0f)
					{
						Source.Fill(num4);
					}
					TransferredAmount += num3 - num4;
				}
				if (Source.State == LiquidContainerState.Empty || Target.State == LiquidContainerState.Full)
				{
					Complete();
					break;
				}
				await UniTask.Yield();
			}
		}

		private async UniTaskVoid TransferProcessByCompleteAmountAsync(float amountToComplete)
		{
			while (IsActive)
			{
				float num = FillingSpeed * Time.deltaTime;
				if (Target.CurrentAmount + num > amountToComplete)
				{
					num = amountToComplete - Target.CurrentAmount;
				}
				TransferredAmount += num;
				Target.Fill(num);
				if (Target.CurrentAmount >= Target.Capacity || Target.CurrentAmount >= amountToComplete)
				{
					Complete();
					break;
				}
				await UniTask.Yield();
			}
		}

		public void Complete()
		{
			if (IsActive)
			{
				IsActive = false;
				StopTransferAudio();
				State = FuelTransferState.Completed;
				_onTransferCompleted.Invoke();
				UnsubscribeTransferActions();
			}
		}

		public void Pause()
		{
			IsActive = false;
			StopTransferAudio();
			State = FuelTransferState.Paused;
			_onTransferPaused.Invoke();
			UnsubscribeTransferActions();
		}

		public void Cancel()
		{
			if (IsActive || State == FuelTransferState.InProgress)
			{
				IsActive = false;
				StopTransferAudio();
				State = FuelTransferState.Cancelled;
				_onTransferCanceled.Invoke();
				_sourceActions?.SetLiquidTransferCancelledActions();
				_targetActions?.SetLiquidTransferCancelledActions();
				UnsubscribeTransferActions();
			}
		}

		private void StartTransferAudio()
		{
			ILiquidContainer liquidContainer = ((Source != null && Source.TransferSound.IsValid()) ? Source : Target);
			SoundID transferSound = liquidContainer.TransferSound;
			_activeParameter = liquidContainer.TransferActiveParameter;
			if (transferSound.IsValid())
			{
				_liquidTransferEventInstance = _audioManager.PlayEvent(transferSound, liquidContainer.Position);
				if (_activeParameter.IsValid())
				{
					_audioManager.SetParameter(_liquidTransferEventInstance, _activeParameter, value: true);
				}
			}
		}

		private void StopTransferAudio()
		{
			if (_liquidTransferEventInstance.IsValid)
			{
				if (_activeParameter.IsValid())
				{
					_audioManager.SetParameter(_liquidTransferEventInstance, _activeParameter, value: false);
				}
				_audioManager.StopEvent(_liquidTransferEventInstance);
				_liquidTransferEventInstance = default(AudioHandle);
				_activeParameter = default(AudioParameter);
			}
		}

		public TransferRequestResult GetTransferRequestResult()
		{
			if (Source == null || Target == null)
			{
				return TransferRequestResult.Error;
			}
			if (Source.TransferType == LiquidTransferType.In)
			{
				return TransferRequestResult.SourceOutputNotAllowed;
			}
			if (Target.TransferType == LiquidTransferType.Out)
			{
				return TransferRequestResult.TargetInputNotAllowed;
			}
			if (Source.CurrentLiquidType != LiquidType.Empty && (Source.CurrentLiquidType & Target.AllowedLiquidTypes) == 0)
			{
				return TransferRequestResult.IncompatibleLiquid;
			}
			if (Source.CurrentLiquidType != LiquidType.Empty && Target.CurrentLiquidType != LiquidType.Empty && Source.CurrentLiquidType != Target.CurrentLiquidType)
			{
				return TransferRequestResult.IncompatibleLiquid;
			}
			if (Source.State == LiquidContainerState.Empty)
			{
				return TransferRequestResult.SourceEmpty;
			}
			if (Target.State == LiquidContainerState.Full)
			{
				return TransferRequestResult.TargetFull;
			}
			return TransferRequestResult.Allowed;
		}
	}
}
