using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorManager : MonoBehaviour
{
	[Tooltip("Bu sahnelerde cursor HER ZAMAN serbest kalır (menü gibi UI ağırlıklı sahneler). Listede olmayan her sahnede varsayılan KİLİTLİ'dir (açık panel yoksa).")]
	public string[] alwaysFreeScenes = new string[1] { "Menu" };

	private int _uiOpenCount;

	public static CursorManager Instance { get; private set; }

	public bool AnyUIOpen => _uiOpenCount > 0;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		Instance = this;
		Object.DontDestroyOnLoad(base.gameObject);
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnDestroy()
	{
		if (Instance == this)
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
		}
	}

	private void Start()
	{
		RefreshCursor();
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		_uiOpenCount = 0;
		RefreshCursor();
	}

	public void PushUI()
	{
		_uiOpenCount++;
		RefreshCursor();
	}

	public void PopUI()
	{
		_uiOpenCount = Mathf.Max(0, _uiOpenCount - 1);
		RefreshCursor();
	}

	public void ForceUnlock()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	private bool CurrentSceneAlwaysFree()
	{
		string text = SceneManager.GetActiveScene().name;
		string[] array = alwaysFreeScenes;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == text)
			{
				return true;
			}
		}
		return false;
	}

	private void RefreshCursor()
	{
		if (_uiOpenCount > 0 || CurrentSceneAlwaysFree())
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		else
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}
}
