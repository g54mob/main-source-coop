using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Fusion
{
	internal static class BehaviourUtils
	{
		public struct DeferredJoin
		{
			public IEnumerable _enumerable;

			public override readonly string ToString()
			{
				return string.Join(", ", _enumerable.Cast<object>());
			}
		}

		internal struct NameDeferred
		{
			private Behaviour _behaviour;

			public NameDeferred(Behaviour behaviour)
			{
				_behaviour = behaviour;
			}

			public static explicit operator NameDeferred(Behaviour behaviour)
			{
				return new NameDeferred(behaviour);
			}

			public static implicit operator string(NameDeferred wrapper)
			{
				return wrapper.ToString();
			}

			public override readonly string ToString()
			{
				if (IsNull(_behaviour))
				{
					return "(null)";
				}
				return _behaviour.DebugNameThreadSafe;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ContractAnnotation("obj:null=>true")]
		public static bool IsNull(Behaviour obj)
		{
			return (object)obj == null;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ContractAnnotation("obj:null=>false")]
		public static bool IsNotNull(Behaviour obj)
		{
			return (object)obj != null;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ContractAnnotation("obj:null=>false")]
		public static bool IsAlive(NetworkRunner obj)
		{
			return obj;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ContractAnnotation("obj:null=>true")]
		public static bool IsNotAlive(NetworkRunner obj)
		{
			return !obj;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ContractAnnotation("obj:null=>false")]
		public static bool IsAlive(SimulationBehaviour obj)
		{
			return obj?.Flags.HasNot(SimulationBehaviourRuntimeFlags.IsUnityDestroyed) ?? false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ContractAnnotation("obj:null=>true")]
		public static bool IsNotAlive(SimulationBehaviour obj)
		{
			return obj?.Flags.Has(SimulationBehaviourRuntimeFlags.IsUnityDestroyed) ?? true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ContractAnnotation("obj:null=>false")]
		public static bool IsAlive(NetworkObject obj)
		{
			return obj?.RuntimeFlags.HasNot(NetworkObjectRuntimeFlags.IsDestroyed) ?? false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ContractAnnotation("obj:null=>true")]
		public static bool IsNotAlive(NetworkObject obj)
		{
			return obj?.RuntimeFlags.Has(NetworkObjectRuntimeFlags.IsDestroyed) ?? true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsSame(Behaviour a, Behaviour b)
		{
			return (object)a == b;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsSameNotNull(Behaviour a, Behaviour b)
		{
			return (object)a != null && (object)a == b;
		}

		public static NameDeferred GetName(Behaviour obj)
		{
			return new NameDeferred(obj);
		}

		public static DeferredJoin Join(IEnumerable objects)
		{
			return new DeferredJoin
			{
				_enumerable = objects
			};
		}
	}
}
