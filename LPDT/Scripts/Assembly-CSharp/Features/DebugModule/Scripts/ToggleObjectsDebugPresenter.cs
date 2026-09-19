using System;
using System.Collections.Generic;
using Features.LineArmModule.Scripts.Data;
using Features.ObiCollidersChunk.Scripts;
using JetBrains.Annotations;
using Obi;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using UnityEngine.Rendering;

namespace Features.DebugModule.Scripts
{
	[UsedImplicitly]
	public class ToggleObjectsDebugPresenter : PresenterBehaviour<ToggleObjectsDebugViewBase>
	{
		private static readonly List<Light> CachedLights = new List<Light>();

		private static readonly List<Renderer> CachedRenderers = new List<Renderer>();

		private static readonly List<Volume> CachedVolumes = new List<Volume>();

		private static readonly List<GameObject> CachedObiObjects = new List<GameObject>();

		private static bool _isInitialized;

		private readonly LineArmGrabSettingsModel _lineArmGrabSettingsModel;

		public ToggleObjectsDebugPresenter(LineArmGrabSettingsModel lineArmGrabSettingsModel)
		{
			_lineArmGrabSettingsModel = lineArmGrabSettingsModel;
		}

		protected override void OnViewEnabled()
		{
			if (!_isInitialized)
			{
				CacheSceneObjects();
			}
			ToggleObjectsDebugViewBase view = base.View;
			view.OnToggleAdditionalLights = (Action<bool>)Delegate.Combine(view.OnToggleAdditionalLights, new Action<bool>(OnToggleAdditionalLightsHandler));
			ToggleObjectsDebugViewBase view2 = base.View;
			view2.OnToggleObiRopes = (Action<bool>)Delegate.Combine(view2.OnToggleObiRopes, new Action<bool>(OnToggleObiRopesHandler));
			ToggleObjectsDebugViewBase view3 = base.View;
			view3.OnToggleGeometry = (Action<bool>)Delegate.Combine(view3.OnToggleGeometry, new Action<bool>(OnToggleGeometryHandler));
			ToggleObjectsDebugViewBase view4 = base.View;
			view4.OnTogglePostProcessing = (Action<bool>)Delegate.Combine(view4.OnTogglePostProcessing, new Action<bool>(OnTogglePostProcessingHandler));
			ToggleObjectsDebugViewBase view5 = base.View;
			view5.OnToggleLatchGrab = (Action<bool>)Delegate.Combine(view5.OnToggleLatchGrab, new Action<bool>(OnToggleLatchGrabHandler));
			_lineArmGrabSettingsModel.OnLatchGrabEnabledChanged += RefreshLatchGrabToggle;
			base.View.RefreshLatchGrabToggle(_lineArmGrabSettingsModel.IsLatchGrabEnabled);
		}

		protected override void OnViewDisabled()
		{
			ToggleObjectsDebugViewBase view = base.View;
			view.OnToggleAdditionalLights = (Action<bool>)Delegate.Remove(view.OnToggleAdditionalLights, new Action<bool>(OnToggleAdditionalLightsHandler));
			ToggleObjectsDebugViewBase view2 = base.View;
			view2.OnToggleObiRopes = (Action<bool>)Delegate.Remove(view2.OnToggleObiRopes, new Action<bool>(OnToggleObiRopesHandler));
			ToggleObjectsDebugViewBase view3 = base.View;
			view3.OnToggleGeometry = (Action<bool>)Delegate.Remove(view3.OnToggleGeometry, new Action<bool>(OnToggleGeometryHandler));
			ToggleObjectsDebugViewBase view4 = base.View;
			view4.OnTogglePostProcessing = (Action<bool>)Delegate.Remove(view4.OnTogglePostProcessing, new Action<bool>(OnTogglePostProcessingHandler));
			ToggleObjectsDebugViewBase view5 = base.View;
			view5.OnToggleLatchGrab = (Action<bool>)Delegate.Remove(view5.OnToggleLatchGrab, new Action<bool>(OnToggleLatchGrabHandler));
			_lineArmGrabSettingsModel.OnLatchGrabEnabledChanged -= RefreshLatchGrabToggle;
		}

		private static void CacheSceneObjects()
		{
			_isInitialized = true;
			Light[] array = UnityEngine.Object.FindObjectsByType<Light>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
			foreach (Light light in array)
			{
				if (light.type != LightType.Directional)
				{
					CachedLights.Add(light);
				}
			}
			Renderer[] array2 = UnityEngine.Object.FindObjectsByType<Renderer>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
			foreach (Renderer item in array2)
			{
				CachedRenderers.Add(item);
			}
			Volume[] array3 = UnityEngine.Object.FindObjectsByType<Volume>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
			foreach (Volume item2 in array3)
			{
				CachedVolumes.Add(item2);
			}
			ObiRope[] array4 = UnityEngine.Object.FindObjectsByType<ObiRope>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
			ObiColliderChunk[] array5 = UnityEngine.Object.FindObjectsByType<ObiColliderChunk>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
			ObiRope[] array6 = array4;
			foreach (ObiRope obiRope in array6)
			{
				if (!(obiRope == null))
				{
					GameObject gameObject = obiRope.gameObject;
					CachedObiObjects.Add(gameObject);
				}
			}
			ObiColliderChunk[] array7 = array5;
			foreach (ObiColliderChunk obiColliderChunk in array7)
			{
				if (!(obiColliderChunk == null))
				{
					GameObject gameObject2 = obiColliderChunk.gameObject;
					CachedObiObjects.Add(gameObject2);
				}
			}
		}

		private void OnToggleAdditionalLightsHandler(bool isEnabled)
		{
			foreach (Light cachedLight in CachedLights)
			{
				if (!(cachedLight == null))
				{
					cachedLight.enabled = isEnabled;
				}
			}
		}

		private void OnToggleGeometryHandler(bool isEnabled)
		{
			foreach (Renderer cachedRenderer in CachedRenderers)
			{
				if (!(cachedRenderer == null))
				{
					cachedRenderer.enabled = isEnabled;
				}
			}
		}

		private void OnTogglePostProcessingHandler(bool isEnabled)
		{
			foreach (Volume cachedVolume in CachedVolumes)
			{
				if (!(cachedVolume == null))
				{
					cachedVolume.enabled = isEnabled;
				}
			}
		}

		private void OnToggleObiRopesHandler(bool isEnabled)
		{
			foreach (GameObject cachedObiObject in CachedObiObjects)
			{
				if (!(cachedObiObject == null))
				{
					cachedObiObject.SetActive(isEnabled);
				}
			}
		}

		private void OnToggleLatchGrabHandler(bool isEnabled)
		{
			_lineArmGrabSettingsModel.SetLatchGrabEnabled(isEnabled);
		}

		private void RefreshLatchGrabToggle(bool isEnabled)
		{
			base.View.RefreshLatchGrabToggle(isEnabled);
		}
	}
}
