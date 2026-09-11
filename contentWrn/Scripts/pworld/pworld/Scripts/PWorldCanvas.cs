using UnityEngine;

namespace pworld.Scripts
{
	public class PWorldCanvas : MonoBehaviour
	{
		private static PWorldCanvas me;

		public static PWorldCanvas Me
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
