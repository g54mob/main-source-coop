using System;
using System.Collections.Generic;
using Features.AudioDevicesModule.Scripts;
using Features.GameUpdaterModule;
using Features.SettingsMenuModule.Scripts.Data;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public class MicrophoneDeviceSettingsItemPresenter : PresenterBehaviour<MicrophoneDeviceSettingsItemViewBase>
	{
		private const float CaptureDevicePollIntervalSeconds = 0.5f;

		private readonly NotAppliedSettingsModel _notAppliedSettingsModel;

		private readonly IGameUpdater _gameUpdater;

		private readonly CurrentSettingsModel _currentSettingsModel;

		private string _cachedMicrophoneDevice;

		private readonly MicrophoneModel _microphoneModel;

		private float _lastCaptureDevicePollTime = float.NegativeInfinity;

		public MicrophoneDeviceSettingsItemPresenter(NotAppliedSettingsModel notAppliedSettingsModel, IGameUpdater gameUpdater, CurrentSettingsModel currentSettingsModel, MicrophoneModel microphoneModel)
		{
			_notAppliedSettingsModel = notAppliedSettingsModel;
			_gameUpdater = gameUpdater;
			_currentSettingsModel = currentSettingsModel;
			_microphoneModel = microphoneModel;
		}

		protected override void OnViewSet()
		{
			base.OnViewSet();
			MicrophoneDeviceSettingsItemViewBase view = base.View;
			view.OnMicrophoneChanged = (Action<string>)Delegate.Combine(view.OnMicrophoneChanged, new Action<string>(SaveMicrophone));
			_gameUpdater.OnFixedUpdate += UpdateMicrophoneSettings;
			_microphoneModel.RequestCaptureDeviceListRefresh();
			UpdateMicrophoneSettings();
			Initialize();
		}

		private void Initialize()
		{
			string microphoneDevice = _currentSettingsModel.MicrophoneDevice;
			TrySetCurrentMicrophone(microphoneDevice);
		}

		protected override void OnDisposed()
		{
			base.OnDisposed();
			MicrophoneDeviceSettingsItemViewBase view = base.View;
			view.OnMicrophoneChanged = (Action<string>)Delegate.Remove(view.OnMicrophoneChanged, new Action<string>(SaveMicrophone));
			_gameUpdater.OnFixedUpdate -= UpdateMicrophoneSettings;
		}

		private void UpdateMicrophoneSettings()
		{
			if (Time.unscaledTime - _lastCaptureDevicePollTime >= 0.5f)
			{
				_lastCaptureDevicePollTime = Time.unscaledTime;
				_microphoneModel.RequestCaptureDeviceListRefresh();
			}
			string currentMicrophone = (_notAppliedSettingsModel.IsMicrophoneDeviceChanged ? _notAppliedSettingsModel.MicrophoneDevice : _currentSettingsModel.MicrophoneDevice);
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, int> item in _microphoneModel.MicrophonesDriverID)
			{
				list.Add(item.Key);
			}
			base.View.SetMicrophoneSettingsOptions(list, currentMicrophone);
		}

		private void SaveMicrophone(string microphoneDevice)
		{
			_cachedMicrophoneDevice = microphoneDevice;
			_notAppliedSettingsModel.MicrophoneDevice = microphoneDevice;
		}

		private void TrySetCurrentMicrophone(string lastDeviceName)
		{
			int startIndex = 0;
			if (!string.IsNullOrEmpty(lastDeviceName))
			{
				int num = 0;
				foreach (KeyValuePair<string, int> item in _microphoneModel.MicrophonesDriverID)
				{
					if (item.Key == lastDeviceName)
					{
						startIndex = num;
						break;
					}
					num++;
				}
			}
			base.View.SetStartIndex(startIndex);
			SaveMicrophone(lastDeviceName);
		}
	}
}
