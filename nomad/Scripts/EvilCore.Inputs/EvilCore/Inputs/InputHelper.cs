using Rewired;

namespace EvilCore.Inputs
{
	public static class InputHelper<T> where T : class
	{
		private static int _mapId;

		public static Player Player => ReInput.players.GetPlayer(0);

		public static void SetMapId(int id)
		{
			_mapId = id;
		}

		public static void EnableMap()
		{
			Player.controllers.maps.SetMapsEnabled(state: true, _mapId);
		}

		public static void DisableMap()
		{
			Player.controllers.maps.SetMapsEnabled(state: false, _mapId);
		}
	}
}
