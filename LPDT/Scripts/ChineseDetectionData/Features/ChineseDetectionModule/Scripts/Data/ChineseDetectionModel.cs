using System;

namespace Features.ChineseDetectionModule.Scripts.Data
{
	public class ChineseDetectionModel
	{
		public bool IsChineseAudience { get; private set; }

		public event Action<bool> OnIsChineseAudienceChanged;

		public void SetIsChineseAudience(bool isChineseAudience)
		{
			if (IsChineseAudience != isChineseAudience)
			{
				IsChineseAudience = isChineseAudience;
				this.OnIsChineseAudienceChanged?.Invoke(isChineseAudience);
			}
		}
	}
}
