using System;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace Features.PostProcessingModule.Scripts
{
	public class PostProcessingModel
	{
		private readonly Dictionary<PostProcessingType, Volume> _activeVolumes = new Dictionary<PostProcessingType, Volume>();

		private readonly Dictionary<Volume, List<VignetteEffect>> _appliedVignettedEffects = new Dictionary<Volume, List<VignetteEffect>>();

		public IReadOnlyDictionary<PostProcessingType, Volume> ActiveVolumes => _activeVolumes;

		public IReadOnlyDictionary<Volume, List<VignetteEffect>> AppliedVignettedEffects => _appliedVignettedEffects;

		public bool IsVignetteDisabled { get; set; }

		public event Action<PostProcessingType, Volume> OnVolumeRegistered;

		public event Action<PostProcessingType> OnVolumeUnRegistered;

		public void RegisterVolume(PostProcessingType ppType, Volume volume)
		{
			if (!_activeVolumes.TryAdd(ppType, volume))
			{
				_activeVolumes[ppType] = volume;
			}
			this.OnVolumeRegistered?.Invoke(ppType, volume);
		}

		public void UnregisterVolume(PostProcessingType ppType)
		{
			_activeVolumes.Remove(ppType);
			this.OnVolumeUnRegistered?.Invoke(ppType);
		}

		public void ApplyVignetteEffect(Volume volume, VignetteEffect vignetteEffect)
		{
			if (!_appliedVignettedEffects.TryAdd(volume, new List<VignetteEffect> { vignetteEffect }))
			{
				_appliedVignettedEffects[volume].Add(vignetteEffect);
			}
		}

		public void RemoveVignetteEffect(Volume volume, VignetteEffect vignetteEffect)
		{
			if (_appliedVignettedEffects.TryGetValue(volume, out var value))
			{
				value.Remove(vignetteEffect);
			}
		}

		public bool ContainsEffect(Volume volume, VignetteEffect vignetteEffect)
		{
			if (_appliedVignettedEffects.TryGetValue(volume, out var value))
			{
				return value.Contains(vignetteEffect);
			}
			return false;
		}
	}
}
