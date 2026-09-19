using Features.PlayerSkinModule.Scripts;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.HeadcrabEnemy
{
	public class HeadcrabAnimatorPresenter : MonoBehaviour
	{
		[SerializeField]
		private HeadcrabEnemy _enemy;

		[SerializeField]
		private HeadcrabEnemyContext _context;

		private readonly int _jumpStartHash = Animator.StringToHash("JumpStart");

		private readonly int _jumpEndHash = Animator.StringToHash("JumpEnd");

		private readonly int _idleHash = Animator.StringToHash("Idle");

		private SkinModel _skinModel;

		[Inject]
		private void InjectDependencies(SkinModel skinModel)
		{
			_skinModel = skinModel;
		}

		public void TriggerJumpStart()
		{
			if (_context != null && _context.NetworkedAnimator != null)
			{
				_context.NetworkedAnimator.SetTrigger(_jumpStartHash);
			}
		}

		public void TriggerJumpEnd()
		{
			if (_context != null && _context.NetworkedAnimator != null)
			{
				_context.NetworkedAnimator.SetTrigger(_jumpEndHash);
			}
		}

		public void ResetToIdle()
		{
			if (!(_context == null) && !(_context.NetworkedAnimator == null))
			{
				_context.NetworkedAnimator.ResetTrigger(_jumpStartHash);
				_context.NetworkedAnimator.ResetTrigger(_jumpEndHash);
				_context.NetworkedAnimator.SetTrigger(_idleHash);
			}
		}

		private void Update()
		{
			if (!(_enemy == null) && !(_enemy.Object == null) && _enemy.Object.IsValid)
			{
				if (_context != null)
				{
					_context.UpdateFmod3DAttributes();
				}
				UpdateVisibilityLocal();
			}
		}

		private void UpdateVisibilityLocal()
		{
			if (_context == null || _context.VisibilityHandler == null || _enemy == null || _enemy.Runner == null)
			{
				return;
			}
			if (_context.WithVisibilityChange)
			{
				int playerId = _enemy.Runner.LocalPlayer.PlayerId;
				if (_skinModel != null && _skinModel.AllCharacterVisibility.TryGetValue(playerId, out var value) && value.IsMainVisible())
				{
					_context.VisibilityHandler.EnableVisibility();
				}
				else
				{
					_context.VisibilityHandler.DisableVisibility();
				}
			}
			else
			{
				_context.VisibilityHandler.EnableVisibility();
			}
		}
	}
}
