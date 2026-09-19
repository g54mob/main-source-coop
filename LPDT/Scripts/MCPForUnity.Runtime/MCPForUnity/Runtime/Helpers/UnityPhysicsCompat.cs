using System;
using System.Reflection;
using UnityEngine;

namespace MCPForUnity.Runtime.Helpers
{
	public static class UnityPhysicsCompat
	{
		public enum SimulationMode
		{
			FixedUpdate = 0,
			Update = 1,
			Script = 2,
			Unknown = 3
		}

		private static PropertyInfo _physics2DAutoSync;

		private static bool _physics2DProbed;

		private static PropertyInfo _physicsAutoSync;

		private static bool _physicsProbed;

		private static PropertyInfo _physicsSimulationMode;

		private static bool _physicsSimulationModeProbed;

		private static PropertyInfo _physicsAutoSimulation;

		private static bool _physicsAutoSimulationProbed;

		private static PropertyInfo Physics2DAutoSyncProp
		{
			get
			{
				if (!_physics2DProbed)
				{
					_physics2DProbed = true;
					_physics2DAutoSync = typeof(Physics2D).GetProperty("autoSyncTransforms", BindingFlags.Static | BindingFlags.Public);
				}
				return _physics2DAutoSync;
			}
		}

		private static PropertyInfo PhysicsAutoSyncProp
		{
			get
			{
				if (!_physicsProbed)
				{
					_physicsProbed = true;
					_physicsAutoSync = typeof(Physics).GetProperty("autoSyncTransforms", BindingFlags.Static | BindingFlags.Public);
				}
				return _physicsAutoSync;
			}
		}

		private static PropertyInfo PhysicsSimulationModeProp
		{
			get
			{
				if (!_physicsSimulationModeProbed)
				{
					_physicsSimulationModeProbed = true;
					_physicsSimulationMode = typeof(Physics).GetProperty("simulationMode", BindingFlags.Static | BindingFlags.Public);
				}
				return _physicsSimulationMode;
			}
		}

		private static PropertyInfo PhysicsAutoSimulationProp
		{
			get
			{
				if (!_physicsAutoSimulationProbed)
				{
					_physicsAutoSimulationProbed = true;
					_physicsAutoSimulation = typeof(Physics).GetProperty("autoSimulation", BindingFlags.Static | BindingFlags.Public);
				}
				return _physicsAutoSimulation;
			}
		}

		public static bool? GetPhysics2DAutoSyncTransforms()
		{
			PropertyInfo physics2DAutoSyncProp = Physics2DAutoSyncProp;
			if (physics2DAutoSyncProp == null || !physics2DAutoSyncProp.CanRead)
			{
				return null;
			}
			try
			{
				return (bool)physics2DAutoSyncProp.GetValue(null);
			}
			catch
			{
				return null;
			}
		}

		public static bool TrySetPhysics2DAutoSyncTransforms(bool value)
		{
			PropertyInfo physics2DAutoSyncProp = Physics2DAutoSyncProp;
			if (physics2DAutoSyncProp == null || !physics2DAutoSyncProp.CanWrite)
			{
				return false;
			}
			try
			{
				physics2DAutoSyncProp.SetValue(null, value);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public static bool? GetPhysicsAutoSyncTransforms()
		{
			PropertyInfo physicsAutoSyncProp = PhysicsAutoSyncProp;
			if (physicsAutoSyncProp == null || !physicsAutoSyncProp.CanRead)
			{
				return null;
			}
			try
			{
				return (bool)physicsAutoSyncProp.GetValue(null);
			}
			catch
			{
				return null;
			}
		}

		public static bool TrySetPhysicsAutoSyncTransforms(bool value)
		{
			PropertyInfo physicsAutoSyncProp = PhysicsAutoSyncProp;
			if (physicsAutoSyncProp == null || !physicsAutoSyncProp.CanWrite)
			{
				return false;
			}
			try
			{
				physicsAutoSyncProp.SetValue(null, value);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public static SimulationMode GetPhysicsSimulationMode()
		{
			PropertyInfo physicsSimulationModeProp = PhysicsSimulationModeProp;
			if (physicsSimulationModeProp != null && physicsSimulationModeProp.CanRead)
			{
				try
				{
					return ParseSimulationMode(physicsSimulationModeProp.GetValue(null)?.ToString());
				}
				catch
				{
				}
			}
			PropertyInfo physicsAutoSimulationProp = PhysicsAutoSimulationProp;
			if (physicsAutoSimulationProp != null && physicsAutoSimulationProp.CanRead)
			{
				try
				{
					return (!(bool)physicsAutoSimulationProp.GetValue(null)) ? SimulationMode.Script : SimulationMode.FixedUpdate;
				}
				catch
				{
				}
			}
			return SimulationMode.Unknown;
		}

		public static bool TrySetPhysicsSimulationMode(SimulationMode mode)
		{
			PropertyInfo physicsSimulationModeProp = PhysicsSimulationModeProp;
			if (physicsSimulationModeProp != null && physicsSimulationModeProp.CanWrite)
			{
				try
				{
					object value = Enum.Parse(physicsSimulationModeProp.PropertyType, mode.ToString(), ignoreCase: true);
					physicsSimulationModeProp.SetValue(null, value);
					return true;
				}
				catch
				{
				}
			}
			PropertyInfo physicsAutoSimulationProp = PhysicsAutoSimulationProp;
			if (physicsAutoSimulationProp != null && physicsAutoSimulationProp.CanWrite)
			{
				try
				{
					switch (mode)
					{
					case SimulationMode.FixedUpdate:
						physicsAutoSimulationProp.SetValue(null, true);
						return true;
					case SimulationMode.Script:
						physicsAutoSimulationProp.SetValue(null, false);
						return true;
					}
				}
				catch
				{
				}
			}
			return false;
		}

		private static SimulationMode ParseSimulationMode(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return SimulationMode.Unknown;
			}
			return s.ToLowerInvariant() switch
			{
				"fixedupdate" => SimulationMode.FixedUpdate, 
				"update" => SimulationMode.Update, 
				"script" => SimulationMode.Script, 
				_ => SimulationMode.Unknown, 
			};
		}
	}
}
