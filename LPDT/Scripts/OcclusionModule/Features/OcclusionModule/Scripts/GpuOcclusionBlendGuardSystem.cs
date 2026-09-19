using System;
using System.Reflection;
using Features.CameraModelModule;
using Features.GameUpdaterModule;
using GameplayEvents;
using Zenject;

namespace Features.OcclusionModule.Scripts
{
	public class GpuOcclusionBlendGuardSystem : IInitializable, IDisposable
	{
		private readonly IGameUpdater _gameUpdater;

		private readonly CameraModel _cameraModel;

		private readonly GameplayEventBus _gameplayEventBus;

		private PropertyInfo _instanceProperty;

		private FieldInfo _settingsField;

		private FieldInfo _occlusionField;

		private bool _reflectionReady;

		private bool _isLocalRagdolled;

		private bool _isDisabled;

		public GpuOcclusionBlendGuardSystem(IGameUpdater gameUpdater, CameraModel cameraModel, GameplayEventBus gameplayEventBus)
		{
			_gameUpdater = gameUpdater;
			_cameraModel = cameraModel;
			_gameplayEventBus = gameplayEventBus;
		}

		public void Initialize()
		{
			ResolveReflection();
			_gameplayEventBus.Subscribe<OnPlayerTemporaryRagdollStartedGameplayEvent>(OnRagdollStarted);
			_gameplayEventBus.Subscribe<OnPlayerTemporaryRagdollRecoveredGameplayEvent>(OnRagdollRecovered);
			_gameUpdater.OnUpdate += Tick;
		}

		public void Dispose()
		{
			_gameUpdater.OnUpdate -= Tick;
			_gameplayEventBus.Unsubscribe<OnPlayerTemporaryRagdollStartedGameplayEvent>(OnRagdollStarted);
			_gameplayEventBus.Unsubscribe<OnPlayerTemporaryRagdollRecoveredGameplayEvent>(OnRagdollRecovered);
			if (_isDisabled)
			{
				SetOcclusionEnabled(enabled: true);
			}
		}

		private void OnRagdollStarted(OnPlayerTemporaryRagdollStartedGameplayEvent evt)
		{
			if (evt.IsLocal)
			{
				_isLocalRagdolled = true;
			}
		}

		private void OnRagdollRecovered(OnPlayerTemporaryRagdollRecoveredGameplayEvent evt)
		{
			if (evt.IsLocal)
			{
				_isLocalRagdolled = false;
			}
		}

		private void Tick()
		{
			if (_reflectionReady)
			{
				bool flag = _isLocalRagdolled || _cameraModel.IsBlending;
				if (flag != _isDisabled)
				{
					SetOcclusionEnabled(!flag);
				}
			}
		}

		private void SetOcclusionEnabled(bool enabled)
		{
			if (TrySetOcclusionEnabled(enabled))
			{
				_isDisabled = !enabled;
			}
		}

		private void ResolveReflection()
		{
			Type type = FindDrawerType();
			if (!(type == null))
			{
				_instanceProperty = type.GetProperty("instance", BindingFlags.Static | BindingFlags.NonPublic);
				_settingsField = type.GetField("m_Settings", BindingFlags.Instance | BindingFlags.NonPublic);
				if (!(_instanceProperty == null) && !(_settingsField == null))
				{
					_occlusionField = _settingsField.FieldType.GetField("enableOcclusionCulling", BindingFlags.Instance | BindingFlags.Public);
					_reflectionReady = _occlusionField != null;
				}
			}
		}

		private static Type FindDrawerType()
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			for (int i = 0; i < assemblies.Length; i++)
			{
				Type type = assemblies[i].GetType("UnityEngine.Rendering.GPUResidentDrawer");
				if (type != null)
				{
					return type;
				}
			}
			return null;
		}

		private bool TrySetOcclusionEnabled(bool enabled)
		{
			object value = _instanceProperty.GetValue(null);
			if (value == null)
			{
				return false;
			}
			object value2 = _settingsField.GetValue(value);
			_occlusionField.SetValue(value2, enabled);
			_settingsField.SetValue(value, value2);
			return true;
		}
	}
}
