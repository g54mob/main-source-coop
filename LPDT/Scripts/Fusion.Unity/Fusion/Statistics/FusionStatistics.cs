using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fusion.Statistics
{
	[ScriptHelp(BackColor = ScriptHeaderBackColor.Orange)]
	[DisallowMultipleComponent]
	public class FusionStatistics : SimulationBehaviour, ISpawned, IPublicFacingInterface, IDespawned, IAfterUpdate
	{
		public const ScriptHeaderBackColor StatisticsBackColor = ScriptHeaderBackColor.Orange;

		private const string STATS_ROOT_PREFAB_PATH = "FusionStatsResources/FusionStatisticsRoot";

		private const string STATS_DEFAULT_CONFIG_ASSET_PATH = "FusionStatsResources/FusionStatisticsDefaultConfig";

		private FusionStatisticsRoot _statsRootPrefab;

		private FusionStatisticsRoot _statsRootInstance;

		private List<FusionStatisticsPage> _pages;

		private int _currentPage;

		private float _refreshTime;

		private int _updatesPerSecond;

		private float _updateTime = 1f;

		public static Canvas GlobalStatisticsCanvas { get; private set; }

		public static FusionStatisticsConfig Config { get; private set; }

		public static int EstimateFusionAfterUpdatesPerSecond { get; private set; }

		public FusionStatisticsRoot Root => _statsRootInstance;

		public List<FusionStatisticsPage> Pages => _pages;

		private void Awake()
		{
			GetResources();
			if (_statsRootPrefab == null)
			{
				DestroyWithError("Error loading the required assets for Fusion Statistics. Make sure that the following paths are valid for the Fusion Statistics resource assets: \n 1. FusionStatsResources/FusionStatisticsRoot \n 2. FusionStatsResources/FusionStatisticsDefaultConfig");
			}
		}

		private void GetResources()
		{
			Config = Resources.Load<FusionStatisticsConfig>("FusionStatsResources/FusionStatisticsDefaultConfig");
			_statsRootPrefab = Resources.Load<FusionStatisticsRoot>("FusionStatsResources/FusionStatisticsRoot");
		}

		private void DestroyWithError(string error)
		{
			Debug.LogError(error, this);
			base.Runner?.RemoveGlobal(this);
			UnityEngine.Object.Destroy(this);
		}

		void ISpawned.Spawned()
		{
			if (!base.Runner.TryGetFusionStatistics(out var statisticsManager))
			{
				DestroyWithError("Could not get Fusion Statistics Manager.");
				return;
			}
			InitCanvasRoot();
			SetupPages(statisticsManager);
			HandleActiveRoot();
			Root.SetupPagesDropdown();
			if (_pages.Count > 0)
			{
				_pages[_currentPage].Open();
			}
		}

		private void HandleActiveRoot()
		{
			if (GlobalStatisticsCanvas.GetComponentsInChildren<FusionStatisticsRoot>(includeInactive: true).Length > 1)
			{
				_statsRootInstance.gameObject.SetActive(value: false);
			}
			else
			{
				FusionStatisticsRoot.SetActiveRoot(_statsRootInstance);
			}
		}

		private void SetupPages(FusionStatisticsManager statisticsManager)
		{
			List<FusionStatisticsPage> statisticsPages = Config.StatisticsPages;
			_pages = new List<FusionStatisticsPage>();
			foreach (FusionStatisticsPage item2 in statisticsPages)
			{
				FusionStatisticsPage item = UnityEngine.Object.Instantiate(item2, Root.PagesContent);
				_pages.Add(item);
			}
			_currentPage = 0;
			foreach (FusionStatisticsPage page in _pages)
			{
				page.SetupPage(base.Runner, statisticsManager, this);
			}
		}

		private void InitCanvasRoot()
		{
			if (!GlobalStatisticsCanvas)
			{
				GlobalStatisticsCanvas = CanvasCreator.CreateRootCanvas("StatisticsRootCanvas");
				UnityEngine.Object.DontDestroyOnLoad(GlobalStatisticsCanvas);
			}
			_statsRootInstance = UnityEngine.Object.Instantiate(_statsRootPrefab, GlobalStatisticsCanvas.transform);
			_statsRootInstance.GetComponent<FusionStatisticsRoot>().SetupStatistics(this);
			FusionStatisticsRoot.Roots.Add(_statsRootInstance);
			Image component = _statsRootInstance.GetComponent<Image>();
			Color color = component.color;
			color.a = Config.BackgroundOpacity;
			component.color = color;
		}

		public void ChangePage(int newPage)
		{
			if (newPage >= 0 && newPage < _pages.Count)
			{
				_pages[_currentPage].Close();
				_currentPage = newPage;
				_pages[_currentPage].Open();
			}
		}

		public void ChangePage(FusionStatisticsPage page)
		{
			for (int i = 0; i < _pages.Count; i++)
			{
				if (!(_pages[i] != page))
				{
					ChangePage(i);
					break;
				}
			}
		}

		private void Update()
		{
			if (!base.Runner || _pages == null)
			{
				return;
			}
			if (!_statsRootInstance)
			{
				base.Runner.RemoveStatistics();
			}
			else
			{
				if (!_statsRootInstance.gameObject.activeInHierarchy)
				{
					return;
				}
				if (_refreshTime <= 0f)
				{
					_refreshTime = 1f / (float)Config.PageRefreshRate;
					if (_statsRootInstance.IsVisible)
					{
						_pages[_currentPage]?.Render();
					}
				}
				else
				{
					_refreshTime -= Time.deltaTime;
				}
			}
		}

		public void AfterUpdate()
		{
			if (_updateTime > 0f)
			{
				_updateTime -= Time.deltaTime;
				_updatesPerSecond++;
				if (_updateTime <= 0f)
				{
					_updateTime = 1f;
					EstimateFusionAfterUpdatesPerSecond = _updatesPerSecond;
					_updatesPerSecond = 0;
				}
			}
			_pages?[_currentPage]?.AfterFusionUpdate();
		}

		public void Despawned(NetworkRunner runner, bool hasState)
		{
			if ((bool)_statsRootInstance)
			{
				FusionStatisticsRoot.Roots.Remove(_statsRootInstance);
				UnityEngine.Object.Destroy(_statsRootInstance.gameObject);
			}
		}
	}
}
