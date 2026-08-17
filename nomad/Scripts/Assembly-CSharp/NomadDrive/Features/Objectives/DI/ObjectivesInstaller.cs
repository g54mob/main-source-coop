using System;
using EvilCore.DI.Core;
using NomadDrive.Features.Objectives.Networking;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Objectives.DI
{
	[Serializable]
	public class ObjectivesInstaller : MonoInstaller
	{
		[SerializeField]
		private ObjectivesService objectivesServiceReference;

		[SerializeField]
		private ObjectivesPanel objectivesPanelReference;

		[SerializeField]
		private ObjectiveDiscoveryBanner objectiveDiscoveryBannerReference;

		[SerializeField]
		private ObjectiveDatabase objectiveDatabaseReference;

		[Tooltip("Optional. Wire the scene's ObjectivesNetworkSync NetworkBehaviour to enable MustSync objectives. Leave null for single-player builds.")]
		[SerializeField]
		private ObjectivesNetworkSync objectivesNetworkSyncReference;

		public override void Install(IContainerBuilder builder)
		{
			if (objectiveDatabaseReference != null)
			{
				builder.RegisterInstance(objectiveDatabaseReference);
			}
			RegisterIfNotNull(builder, objectivesPanelReference, delegate(RegistrationBuilder c)
			{
				c.As<ObjectivesPanel>();
			});
			RegisterIfNotNull(builder, objectiveDiscoveryBannerReference, delegate(RegistrationBuilder c)
			{
				c.As<ObjectiveDiscoveryBanner>();
			});
			RegisterIfNotNull(builder, objectivesServiceReference, delegate(RegistrationBuilder c)
			{
				c.As<IObjectivesService>().AsSelf();
			});
			RegisterIfNotNull(builder, objectivesNetworkSyncReference, delegate(RegistrationBuilder c)
			{
				c.As<ObjectivesNetworkSync>();
			});
		}
	}
}
