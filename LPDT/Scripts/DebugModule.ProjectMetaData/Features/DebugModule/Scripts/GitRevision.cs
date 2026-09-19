using UnityEngine;

namespace Features.DebugModule.Scripts
{
	public static class GitRevision
	{
		private static bool _resolved;

		private static string _full = string.Empty;

		private static string _short = string.Empty;

		public static string CommitHashFull
		{
			get
			{
				EnsureResolved();
				return _full;
			}
		}

		public static string CommitHashShort
		{
			get
			{
				EnsureResolved();
				return _short;
			}
		}

		private static void EnsureResolved()
		{
			if (_resolved)
			{
				return;
			}
			_resolved = true;
			TextAsset textAsset = Resources.Load<TextAsset>("git_revision");
			if (!(textAsset == null))
			{
				string[] array = textAsset.text.Split('\n');
				if (array.Length != 0)
				{
					_full = array[0].Trim();
				}
				if (array.Length > 1)
				{
					_short = array[1].Trim();
				}
			}
		}
	}
}
