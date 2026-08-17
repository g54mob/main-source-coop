using UnityEngine;

namespace EvilCore.Recording
{
	public class DirectorTimeController
	{
		private static readonly float[] TimeScalePresets = new float[6] { 0.1f, 0.25f, 0.5f, 1f, 2f, 4f };

		private float _originalTimeScale = 1f;

		private float _currentTimeScale = 1f;

		private int _presetIndex = 3;

		public float CurrentTimeScale => _currentTimeScale;

		public float[] Presets => TimeScalePresets;

		public int PresetIndex => _presetIndex;

		public void Enter()
		{
			_originalTimeScale = Time.timeScale;
			_currentTimeScale = Time.timeScale;
			_presetIndex = FindClosestPresetIndex(_currentTimeScale);
		}

		public void Exit()
		{
			Time.timeScale = _originalTimeScale;
			_currentTimeScale = _originalTimeScale;
		}

		public void SetTimeScale(float scale)
		{
			scale = Mathf.Clamp(scale, 0.01f, 10f);
			_currentTimeScale = scale;
			Time.timeScale = scale;
			_presetIndex = FindClosestPresetIndex(scale);
		}

		public void CycleTimeScale(int direction)
		{
			_presetIndex = Mathf.Clamp(_presetIndex + direction, 0, TimeScalePresets.Length - 1);
			SetTimeScale(TimeScalePresets[_presetIndex]);
		}

		private static int FindClosestPresetIndex(float scale)
		{
			int result = 0;
			float num = 3.4028235E+38f;
			for (int i = 0; i < TimeScalePresets.Length; i++)
			{
				float num2 = Mathf.Abs(TimeScalePresets[i] - scale);
				if (num2 < num)
				{
					num = num2;
					result = i;
				}
			}
			return result;
		}
	}
}
