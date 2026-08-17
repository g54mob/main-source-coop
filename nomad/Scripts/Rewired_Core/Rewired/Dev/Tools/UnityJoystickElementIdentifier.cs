using Rewired.Interfaces;
using Rewired.Internal;
using UnityEngine;

namespace Rewired.Dev.Tools
{
	[AddComponentMenu("")]
	[RequireComponent(typeof(GUIText))]
	public sealed class UnityJoystickElementIdentifier : MonoBehaviour
	{
		private IElementIdentifierTool WPruedtHXhtPrXPmkEnYBTuiikZMA;

		public void Awake()
		{
			WPruedtHXhtPrXPmkEnYBTuiikZMA = new pBmaXVMBljaEYtpvxkWtsgYGDkCA();
			WPruedtHXhtPrXPmkEnYBTuiikZMA.Initialize(GUIText.CreateLogger(base.gameObject));
		}

		public void Start()
		{
			WPruedtHXhtPrXPmkEnYBTuiikZMA.Start();
		}

		public void Update()
		{
			WPruedtHXhtPrXPmkEnYBTuiikZMA.Update();
		}

		public void OnDestroy()
		{
			WPruedtHXhtPrXPmkEnYBTuiikZMA.OnDestroy();
		}
	}
}
