using UnityEngine;

namespace pworld.Scripts
{
	public class PCanvas : MonoBehaviour
	{
		private static PCanvas me;

		public static PCanvas Me
		{
			get
			{
				return me;
			}
			private set
			{
				me = value;
			}
		}

		private void Awake()
		{
			Me = this;
		}
	}
}
