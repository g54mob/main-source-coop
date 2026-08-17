using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : StaticInstance<LoadingBar>
{
	[SerializeField]
	private Slider _loadingBar;

	public float Value
	{
		get
		{
			return _loadingBar.value;
		}
		set
		{
			_loadingBar.value = value;
		}
	}
}
