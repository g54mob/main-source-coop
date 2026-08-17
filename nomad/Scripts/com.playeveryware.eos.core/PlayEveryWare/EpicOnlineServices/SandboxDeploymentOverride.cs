namespace PlayEveryWare.EpicOnlineServices
{
	public class SandboxDeploymentOverride
	{
		[SandboxIDFieldValidator]
		public string sandboxID;

		[GUIDFieldValidator]
		public string deploymentID;
	}
}
