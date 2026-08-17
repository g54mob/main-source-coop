using UnityEngine;

namespace VisualDesignCafe.Packages
{
	public class Package : ScriptableObject
	{
		[HideInInspector]
		[SerializeField]
		private string _displayName;

		[SerializeField]
		private string _description;

		[HideInInspector]
		[SerializeField]
		private Version _version;

		[SerializeField]
		private bool _downloadInBackground = true;

		[HideInInspector]
		[SerializeField]
		private int _priority = 0;

		[SerializeField]
		private string _documentationUrl;

		[SerializeField]
		private string _demoScene;

		[SerializeField]
		private bool _pingAsset = true;

		public string DisplayName => _displayName;

		public Version Version => _version;

		public bool DownloadInBackground => _downloadInBackground;

		public int Priority => _priority;

		public string DocumentationUrl => _documentationUrl;

		public string DemoScene => _demoScene;

		public bool Ping => _pingAsset;

		public string Description => _description;
	}
}
