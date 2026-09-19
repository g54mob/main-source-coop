using Features.PlayerPresenceModule;
using Features.PlayerPresenceModule.Networked;
using Zenject;

namespace Features.SessionManagementModule.Presence
{
	public class SessionPlayerPresenceInstaller : Installer<SessionPlayerPresenceInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<SessionPlayerProfileModel>().AsSingle();
			base.Container.Bind<SessionPlayerBridge>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<SessionPlayerPresenceService>().AsSingle();
			base.Container.Bind<SessionPlayerPresence>().FromMethod(ResolveCurrentPresence).AsTransient();
		}

		private static SessionPlayerPresence ResolveCurrentPresence(InjectContext context)
		{
			SessionPlayerNetworkObject boundObject = context.Container.Resolve<SessionPlayerPresenceService>().BoundObject;
			if (!(boundObject != null))
			{
				return null;
			}
			return new SessionPlayerPresence(boundObject.OwnerId.ToString());
		}
	}
}
