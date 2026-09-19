using System;
using System.Collections.Generic;

namespace UnityHFSM.Inspection
{
	public abstract class StateMachinePath : IEquatable<StateMachinePath>
	{
		public static readonly StateMachinePath Root = RootStateMachinePath.instance;

		public readonly StateMachinePath parentPath;

		public bool IsRoot => this is RootStateMachinePath;

		public abstract string LastNodeName { get; }

		protected StateMachinePath(StateMachinePath parentPath)
		{
			this.parentPath = parentPath;
		}

		public bool IsChildPathOf(StateMachinePath parent)
		{
			StateMachinePath stateMachinePath = parentPath;
			while (stateMachinePath != null)
			{
				if (stateMachinePath == parent)
				{
					return true;
				}
				stateMachinePath = stateMachinePath.parentPath;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as StateMachinePath);
		}

		public abstract override int GetHashCode();

		public abstract bool Equals(StateMachinePath other);

		public abstract override string ToString();

		public static bool operator ==(StateMachinePath left, StateMachinePath right)
		{
			if ((object)left == null && (object)right == null)
			{
				return true;
			}
			return left?.Equals(right) ?? false;
		}

		public static bool operator !=(StateMachinePath left, StateMachinePath right)
		{
			return !(left == right);
		}

		public StateMachinePath Join<TStateId>(TStateId name)
		{
			return new StateMachinePath<TStateId>(this, name);
		}
	}
	public class StateMachinePath<TStateId> : StateMachinePath, IEquatable<StateMachinePath<TStateId>>
	{
		public readonly TStateId name;

		public override string LastNodeName => name.ToString();

		public StateMachinePath(TStateId name)
			: base(null)
		{
			this.name = name;
		}

		public StateMachinePath(StateMachinePath parentPath, TStateId name)
			: base(parentPath)
		{
			this.name = name;
		}

		public override string ToString()
		{
			return parentPath?.ToString() + "/" + name.ToString();
		}

		public override bool Equals(StateMachinePath path)
		{
			return Equals(path as StateMachinePath<TStateId>);
		}

		public bool Equals(StateMachinePath<TStateId> other)
		{
			if ((object)other == null)
			{
				return false;
			}
			if ((object)parentPath == null && (object)other.parentPath != null)
			{
				return false;
			}
			if ((object)parentPath != null && (object)other.parentPath == null)
			{
				return false;
			}
			if (!EqualityComparer<TStateId>.Default.Equals(name, other.name))
			{
				return false;
			}
			return parentPath?.Equals(other.parentPath) ?? true;
		}

		public override int GetHashCode()
		{
			int hashCode = EqualityComparer<TStateId>.Default.GetHashCode(name);
			if ((object)parentPath != null)
			{
				return HashCode.Combine(parentPath.GetHashCode(), hashCode);
			}
			return hashCode;
		}
	}
}
