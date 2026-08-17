using System;
using System.Linq;
using UnityEngine;

namespace QFSW.QC.Actions
{
	public class GetKey : ICommandAction
	{
		private KeyCode _key;

		private readonly Action<KeyCode> _onKey;

		private static readonly KeyCode[] KeyCodes = (from KeyCode k in Enum.GetValues(typeof(KeyCode))
			where k < KeyCode.Mouse0
			select k).ToArray();

		public bool IsFinished
		{
			get
			{
				_key = GetCurrentKeyDown();
				return _key != KeyCode.None;
			}
		}

		public bool StartsIdle => true;

		public GetKey(Action<KeyCode> onKey)
		{
			_onKey = onKey;
		}

		private KeyCode GetCurrentKeyDown()
		{
			return KeyCodes.FirstOrDefault(InputHelper.GetKeyDown);
		}

		public void Start(ActionContext context)
		{
		}

		public void Finalize(ActionContext context)
		{
			_onKey(_key);
		}
	}
}
