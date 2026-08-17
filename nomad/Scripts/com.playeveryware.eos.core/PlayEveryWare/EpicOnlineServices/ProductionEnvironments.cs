using PlayEveryWare.Common;

namespace PlayEveryWare.EpicOnlineServices
{
	public class ProductionEnvironments
	{
		public SetOfNamed<Deployment> Deployments { get; } = new SetOfNamed<Deployment>("Deployment");

		public SetOfNamed<SandboxId> Sandboxes { get; } = new SetOfNamed<SandboxId>("Sandbox");

		public ProductionEnvironments()
		{
			Sandboxes.SetRemovePredicate(CanSandboxBeRemoved);
		}

		public bool TryGetFirstDefinedNamedDeployment(out Named<Deployment> deployment)
		{
			deployment = null;
			foreach (Named<Deployment> deployment2 in Deployments)
			{
				if (deployment2.Value.IsComplete)
				{
					deployment = deployment2;
					break;
				}
			}
			return deployment != null;
		}

		private bool CanSandboxBeRemoved(SandboxId sandbox)
		{
			foreach (Named<Deployment> deployment in Deployments)
			{
				if (deployment.Value.SandboxId.Equals(sandbox))
				{
					return false;
				}
			}
			return true;
		}

		public bool AddDeployment(Deployment deployment)
		{
			Sandboxes.Add(deployment.SandboxId);
			return Deployments.Add(deployment);
		}
	}
}
