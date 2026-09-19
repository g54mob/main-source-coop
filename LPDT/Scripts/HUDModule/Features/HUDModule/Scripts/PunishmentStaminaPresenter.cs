using System.Collections;
using Features.CoroutineUtils.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Features.StatsUsageModule.Scripts.StatsData;
using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity;
using UnityEngine;

namespace Features.HUDModule.Scripts
{
	[PublicAPI]
	public class PunishmentStaminaPresenter : PresenterBehaviour<PunishmentStaminaViewBase>
	{
		private readonly SpawnedEntityStatsModel _spawnedEntityStatsModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly ICoroutineRunner _coroutineRunner;

		private IStat _punishmentStaminaStat;

		private IStat _staminaStat;

		private Color _defaultStaminaTextColor;

		private Color _defaultMaxStaminaTextColor;

		private Color _defaultStaminaInsideIconColor;

		private Color _defaultStaminaOutsideIconColor;

		private Coroutine _staminaShakeCoroutine;

		private float _previousStaminaValue;

		private Vector2 _staminaIconRectOrigin;

		private Vector2 _textRectOrigin;

		public PunishmentStaminaPresenter(SpawnedEntityStatsModel spawnedEntityStatsModel, MultiplayerModel multiplayerModel, ICoroutineRunner coroutineRunner)
		{
			_spawnedEntityStatsModel = spawnedEntityStatsModel;
			_multiplayerModel = multiplayerModel;
			_coroutineRunner = coroutineRunner;
		}

		protected override void OnViewSet()
		{
			base.OnViewSet();
			_defaultStaminaTextColor = base.View.StaminaText.color;
			_defaultMaxStaminaTextColor = base.View.StaminaMaxText.color;
			_defaultStaminaInsideIconColor = base.View.StaminaInsideIcon.color;
			_defaultStaminaOutsideIconColor = base.View.StaminaOutsideIcon.color;
			_staminaIconRectOrigin = base.View.StaminaIconRect.anchoredPosition;
			_textRectOrigin = base.View.TextRect.anchoredPosition;
		}

		protected override void OnViewEnabled()
		{
			_spawnedEntityStatsModel.OnPlayerStatRegistered -= TrackPlayerStat;
			_spawnedEntityStatsModel.OnPlayerStatRegistered += TrackPlayerStat;
			if (_spawnedEntityStatsModel.PlayerStats.ContainsKey(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId))
			{
				TrackPlayerStat(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId);
			}
		}

		protected override void OnViewDisabled()
		{
			_spawnedEntityStatsModel.OnPlayerStatRegistered -= TrackPlayerStat;
			if (_punishmentStaminaStat != null)
			{
				_staminaStat.OnFullValueChanged -= UpdateStaminaText;
			}
		}

		private void TrackPlayerStat(int playerId)
		{
			if (_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId == playerId)
			{
				if (_staminaStat != null)
				{
					_staminaStat.OnFullValueChanged -= UpdateStaminaText;
				}
				_punishmentStaminaStat = _spawnedEntityStatsModel.PlayerStats[playerId].GetStat(EntityStatType.PunishmentStamina);
				_staminaStat = _spawnedEntityStatsModel.PlayerStats[playerId].GetStat(EntityStatType.Stamina);
				_staminaStat.OnFullValueChanged += UpdateStaminaText;
				UpdateStaminaText(_punishmentStaminaStat.MaxValue);
			}
		}

		private void UpdateStaminaText(float value)
		{
			if (_staminaStat.MaxValue == 0f || _staminaStat.FullValue > _punishmentStaminaStat.FullValue)
			{
				_previousStaminaValue = float.MaxValue;
				StopStaminaShake();
				return;
			}
			float num = 1f - value / _punishmentStaminaStat.FullValue;
			base.View.StaminaText.color = Color.Lerp(_defaultStaminaTextColor, base.View.StaminaTextMinColor, num);
			base.View.StaminaMaxText.color = Color.Lerp(_defaultMaxStaminaTextColor, base.View.StaminaMaxTextMinColor, num);
			base.View.StaminaInsideIcon.color = Color.Lerp(_defaultStaminaInsideIconColor, base.View.StaminaIconMinColor, num);
			base.View.StaminaOutsideIcon.color = Color.Lerp(_defaultStaminaOutsideIconColor, base.View.StaminaIconMinColor, num);
			_previousStaminaValue = value;
			if (_staminaShakeCoroutine != null)
			{
				_coroutineRunner.StopCoroutine(_staminaShakeCoroutine);
			}
			_staminaShakeCoroutine = _coroutineRunner.StartCoroutine(ShakeStamina(num));
		}

		private IEnumerator ShakeStamina(float intensity)
		{
			float magnitude = base.View.ShakeMaxMagnitude * intensity;
			while (true)
			{
				float num = Time.time * base.View.ShakeFrequency;
				Vector2 vector = new Vector2(Mathf.Sin(num * 0.9f + 3f), Mathf.Cos(num * 1.1f + 1f)) * magnitude;
				base.View.StaminaIconRect.anchoredPosition = _staminaIconRectOrigin + vector;
				base.View.TextRect.anchoredPosition = _textRectOrigin + vector;
				yield return null;
			}
		}

		private void StopStaminaShake()
		{
			if (_staminaShakeCoroutine != null)
			{
				_coroutineRunner.StopCoroutine(_staminaShakeCoroutine);
				_staminaShakeCoroutine = null;
				base.View.StaminaIconRect.anchoredPosition = _staminaIconRectOrigin;
				base.View.TextRect.anchoredPosition = _textRectOrigin;
			}
		}
	}
}
