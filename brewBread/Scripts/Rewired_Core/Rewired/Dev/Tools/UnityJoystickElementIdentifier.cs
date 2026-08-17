using Rewired.Interfaces;
using Rewired.Internal;
using UnityEngine;

namespace Rewired.Dev.Tools
{
	[AddComponentMenu("")]
	[RequireComponent(typeof(Rewired.Internal.GUIText))]
	public sealed class UnityJoystickElementIdentifier : MonoBehaviour
	{
		private IElementIdentifierTool oJtDqAekwAkQhTEtjUoPKgSVmvNk;

		public void Awake()
		{
			oJtDqAekwAkQhTEtjUoPKgSVmvNk = new BGvdFRgcyHDHHtVOFGqCyMzedLldb();
			oJtDqAekwAkQhTEtjUoPKgSVmvNk.Initialize(Rewired.Internal.GUIText.CreateLogger(base.gameObject));
		}

		public void Start()
		{
			oJtDqAekwAkQhTEtjUoPKgSVmvNk.Start();
		}

		public void Update()
		{
			oJtDqAekwAkQhTEtjUoPKgSVmvNk.Update();
		}

		public void OnDestroy()
		{
			oJtDqAekwAkQhTEtjUoPKgSVmvNk.OnDestroy();
		}
	}
}
