using System;
using Newtonsoft.Json;
using PlayEveryWare.EpicOnlineServices.Utility;

namespace PlayEveryWare.EpicOnlineServices
{
	public struct Deployment : IEquatable<Deployment>
	{
		public SandboxId SandboxId;

		public Guid DeploymentId;

		[JsonIgnore]
		public readonly bool IsComplete
		{
			get
			{
				if (!DeploymentId.Equals(Guid.Empty))
				{
					return !SandboxId.IsEmpty;
				}
				return false;
			}
		}

		public bool Equals(Deployment other)
		{
			if (SandboxId.Equals(other.SandboxId))
			{
				return DeploymentId.Equals(other.DeploymentId);
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj is Deployment other)
			{
				return Equals(other);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return HashUtility.Combine(SandboxId, DeploymentId);
		}

		public override string ToString()
		{
			return string.Format("DeploymentId: {0}, SandboxId: {1}", DeploymentId.ToString("N").ToLower(), SandboxId);
		}
	}
}
