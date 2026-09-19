using System;
using System.Reflection;
using System.Text;
using Fusion;
using Photon.Client;
using Photon.Realtime;

namespace Features.NetworkTelemetry.Scripts.Internal
{
	internal static class PhotonRealtimePeerLocator
	{
		private readonly struct ReflectionHop
		{
			public FieldInfo Field { get; }

			public PropertyInfo Property { get; }

			public bool IsEmpty
			{
				get
				{
					if (Field == null)
					{
						return Property == null;
					}
					return false;
				}
			}

			public static ReflectionHop FromField(FieldInfo field)
			{
				return new ReflectionHop(field, null);
			}

			public static ReflectionHop FromProperty(PropertyInfo property)
			{
				return new ReflectionHop(null, property);
			}

			private ReflectionHop(FieldInfo field, PropertyInfo property)
			{
				Field = field;
				Property = property;
			}

			public bool TryRead(object host, out object value)
			{
				value = null;
				if (host == null)
				{
					return false;
				}
				if (Field != null)
				{
					try
					{
						value = Field.GetValue(host);
					}
					catch (Exception)
					{
						return false;
					}
					return value != null;
				}
				if (Property != null)
				{
					MethodInfo getMethod = Property.GetGetMethod(nonPublic: true);
					if (getMethod == null)
					{
						return false;
					}
					try
					{
						value = getMethod.Invoke(host, null);
					}
					catch (Exception)
					{
						return false;
					}
					return value != null;
				}
				return false;
			}
		}

		private sealed class CommunicatorClientBinding
		{
			private FieldInfo ClientField { get; set; }

			private PropertyInfo ClientProperty { get; set; }

			public static CommunicatorClientBinding FromField(FieldInfo field)
			{
				return new CommunicatorClientBinding
				{
					ClientField = field
				};
			}

			public static CommunicatorClientBinding FromProperty(PropertyInfo property)
			{
				return new CommunicatorClientBinding
				{
					ClientProperty = property
				};
			}

			public bool TryReadClientObject(object communicator, out object clientObject)
			{
				clientObject = null;
				if (communicator == null)
				{
					return false;
				}
				if (ClientField != null)
				{
					try
					{
						clientObject = ClientField.GetValue(communicator);
					}
					catch (Exception)
					{
						return false;
					}
					return clientObject != null;
				}
				if (ClientProperty != null)
				{
					MethodInfo getMethod = ClientProperty.GetGetMethod(nonPublic: true);
					if (getMethod == null)
					{
						return false;
					}
					try
					{
						clientObject = getMethod.Invoke(communicator, null);
					}
					catch (Exception)
					{
						return false;
					}
					return clientObject != null;
				}
				return false;
			}
		}

		private sealed class PhotonPeerResolutionCache
		{
			private enum RouteKind
			{
				PhotonRealtimeClientField = 0,
				PhotonRealtimeClientProperty = 1,
				FusionCloudCommunicator = 2
			}

			private RouteKind Route { get; set; }

			private FieldInfo RunnerCloudServicesField { get; set; }

			private FieldInfo PhotonRealtimeClientField { get; set; }

			private PropertyInfo PhotonRealtimeClientProperty { get; set; }

			private ReflectionHop CommunicatorHopFromCloudServices { get; set; }

			private ReflectionHop CommunicatorHopFromIntermediate { get; set; }

			private bool CommunicatorUsesTwoHops { get; set; }

			private CommunicatorClientBinding CommunicatorClient { get; set; }

			public static PhotonPeerResolutionCache CreatePhotonRealtimeClientFieldRoute(FieldInfo runnerCloudServicesField, FieldInfo cloudServicesRealtimeClientField)
			{
				return new PhotonPeerResolutionCache
				{
					Route = RouteKind.PhotonRealtimeClientField,
					RunnerCloudServicesField = runnerCloudServicesField,
					PhotonRealtimeClientField = cloudServicesRealtimeClientField
				};
			}

			public static PhotonPeerResolutionCache CreatePhotonRealtimeClientPropertyRoute(FieldInfo runnerCloudServicesField, PropertyInfo cloudServicesRealtimeClientProperty)
			{
				return new PhotonPeerResolutionCache
				{
					Route = RouteKind.PhotonRealtimeClientProperty,
					RunnerCloudServicesField = runnerCloudServicesField,
					PhotonRealtimeClientProperty = cloudServicesRealtimeClientProperty
				};
			}

			public static PhotonPeerResolutionCache CreateFusionCloudCommunicatorRoute(FieldInfo runnerCloudServicesField, ReflectionHop communicatorHopFromCloudServices, ReflectionHop communicatorHopFromIntermediate, bool twoHops, CommunicatorClientBinding communicatorClient)
			{
				return new PhotonPeerResolutionCache
				{
					Route = RouteKind.FusionCloudCommunicator,
					RunnerCloudServicesField = runnerCloudServicesField,
					CommunicatorHopFromCloudServices = communicatorHopFromCloudServices,
					CommunicatorHopFromIntermediate = communicatorHopFromIntermediate,
					CommunicatorUsesTwoHops = twoHops,
					CommunicatorClient = communicatorClient
				};
			}

			public bool TryApply(NetworkRunner runner, out PhotonPeer peer)
			{
				peer = null;
				if (runner == null || RunnerCloudServicesField == null)
				{
					return false;
				}
				object value;
				try
				{
					value = RunnerCloudServicesField.GetValue(runner);
				}
				catch (Exception)
				{
					return false;
				}
				if (value == null)
				{
					return false;
				}
				return Route switch
				{
					RouteKind.PhotonRealtimeClientField => TryApplyPhotonRealtimeClientField(value, out peer), 
					RouteKind.PhotonRealtimeClientProperty => TryApplyPhotonRealtimeClientProperty(value, out peer), 
					RouteKind.FusionCloudCommunicator => TryApplyFusionCloudCommunicator(value, out peer), 
					_ => false, 
				};
			}

			private bool TryApplyPhotonRealtimeClientField(object cloudServicesHolder, out PhotonPeer peer)
			{
				peer = null;
				if (PhotonRealtimeClientField == null)
				{
					return false;
				}
				object value;
				try
				{
					value = PhotonRealtimeClientField.GetValue(cloudServicesHolder);
				}
				catch (Exception)
				{
					return false;
				}
				if (!(value is RealtimeClient realtimeClient))
				{
					return false;
				}
				peer = realtimeClient.RealtimePeer;
				return peer != null;
			}

			private bool TryApplyPhotonRealtimeClientProperty(object cloudServicesHolder, out PhotonPeer peer)
			{
				peer = null;
				if (PhotonRealtimeClientProperty == null)
				{
					return false;
				}
				MethodInfo getMethod = PhotonRealtimeClientProperty.GetGetMethod(nonPublic: true);
				if (getMethod == null)
				{
					return false;
				}
				object obj;
				try
				{
					obj = getMethod.Invoke(cloudServicesHolder, null);
				}
				catch (Exception)
				{
					return false;
				}
				if (!(obj is RealtimeClient realtimeClient))
				{
					return false;
				}
				peer = realtimeClient.RealtimePeer;
				return peer != null;
			}

			private bool TryApplyFusionCloudCommunicator(object cloudServicesHolder, out PhotonPeer peer)
			{
				peer = null;
				object host = cloudServicesHolder;
				if (!CommunicatorHopFromCloudServices.TryRead(host, out var value))
				{
					return false;
				}
				host = value;
				if (CommunicatorUsesTwoHops)
				{
					if (!CommunicatorHopFromIntermediate.TryRead(host, out value))
					{
						return false;
					}
					host = value;
				}
				if (!IsFusionCloudCommunicatorType(host?.GetType()))
				{
					return false;
				}
				if (!CommunicatorClient.TryReadClientObject(host, out var clientObject))
				{
					return false;
				}
				return TryGetPhotonPeerFromRealtimeClientLikeInstance(clientObject, out peer);
			}
		}

		private const BindingFlags InstanceBindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

		private const BindingFlags DeclaredInstanceBindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

		private const string RunnerCloudServicesFieldName = "_cloudServices";

		private static readonly string[] CloudServicesRealtimeClientFieldCandidates = new string[3] { "realtimeClient", "_realtimeClient", "<RealtimeClient>k__BackingField" };

		private const string CloudServicesRealtimeClientPropertyName = "RealtimeClient";

		private const string RealtimePeerPropertyName = "RealtimePeer";

		private const string FusionNamespaceName = "Fusion";

		private const string FusionCloudCommunicatorTypeName = "CloudCommunicator";

		private static readonly string[] CloudServicesCommunicatorFieldCandidates = new string[4] { "_communicator", "_cloudCommunicator", "Communicator", "<Communicator>k__BackingField" };

		private static readonly string[] CloudServicesCommunicatorPropertyCandidates = new string[2] { "Communicator", "CloudCommunicator" };

		private static readonly string[] CloudServicesIntermediateFieldCandidates = new string[2] { "_communication", "_cloudCommunication" };

		private static readonly string[] CloudServicesIntermediatePropertyCandidates = new string[1] { "Communication" };

		private static readonly string[] CloudCommunicatorClientFieldCandidates = new string[4] { "<Client>k__BackingField", "_client", "client", "Client" };

		private const string CloudCommunicatorClientPropertyName = "Client";

		private static NetworkRunner _cachedRunnerIdentity;

		private static PhotonPeerResolutionCache _resolutionCache;

		public static bool TryGetPhotonPeer(NetworkRunner runner, out PhotonPeer peer)
		{
			peer = null;
			if (runner == null)
			{
				return false;
			}
			if ((object)runner != _cachedRunnerIdentity)
			{
				_cachedRunnerIdentity = runner;
				_resolutionCache = null;
			}
			if (_resolutionCache != null && _resolutionCache.TryApply(runner, out peer))
			{
				return true;
			}
			string resolveTrace;
			return TryGetPhotonPeer(runner, out peer, out resolveTrace);
		}

		public static bool TryGetPhotonPeer(NetworkRunner runner, out PhotonPeer peer, out string resolveTrace)
		{
			peer = null;
			resolveTrace = string.Empty;
			StringBuilder stringBuilder = new StringBuilder();
			if (runner == null)
			{
				stringBuilder.AppendLine("Step 0 FAIL: runner is null.");
				resolveTrace = stringBuilder.ToString();
				return false;
			}
			if ((object)runner != _cachedRunnerIdentity)
			{
				_cachedRunnerIdentity = runner;
				_resolutionCache = null;
			}
			if (_resolutionCache != null && _resolutionCache.TryApply(runner, out peer))
			{
				stringBuilder.AppendLine("CACHE HIT: Reused reflection path for this NetworkRunner instance.");
				stringBuilder.AppendLine("Step 6 OK: RealtimePeer assigned.");
				resolveTrace = stringBuilder.ToString();
				return true;
			}
			_resolutionCache = null;
			if (!TryDiscoverResolutionPath(runner, stringBuilder, out var cache, out peer))
			{
				resolveTrace = stringBuilder.ToString();
				return false;
			}
			_resolutionCache = cache;
			stringBuilder.AppendLine("CACHE SET: Reflection path captured for this NetworkRunner instance.");
			stringBuilder.AppendLine("Step 6 OK: RealtimePeer assigned.");
			resolveTrace = stringBuilder.ToString();
			return true;
		}

		public static void InvalidateCache()
		{
			_cachedRunnerIdentity = null;
			_resolutionCache = null;
		}

		private static bool TryDiscoverResolutionPath(NetworkRunner runner, StringBuilder traceBuilder, out PhotonPeerResolutionCache cache, out PhotonPeer peer)
		{
			cache = null;
			peer = null;
			FieldInfo fieldInfo = FindDeclaredInstanceField(runner.GetType(), "_cloudServices");
			if (fieldInfo == null)
			{
				traceBuilder.AppendLine("Step 1 FAIL: No instance field '_cloudServices' on " + runner.GetType().FullName + " or base types.");
				return false;
			}
			traceBuilder.AppendLine("Step 1 OK: Field '_cloudServices' (" + fieldInfo.FieldType.FullName + ").");
			object value = fieldInfo.GetValue(runner);
			if (value == null)
			{
				traceBuilder.AppendLine("Step 2 FAIL: '_cloudServices' is null (Fusion cloud services not ready yet).");
				return false;
			}
			traceBuilder.AppendLine("Step 2 OK: Holder instance type " + value.GetType().FullName + ".");
			Type type = value.GetType();
			if (type.Namespace != "Fusion" || type.Name != "CloudServices")
			{
				traceBuilder.AppendLine("Step 3 FAIL: Expected Fusion.CloudServices; actual type is " + type.FullName + ".");
				return false;
			}
			traceBuilder.AppendLine("Step 3 OK: Holder runtime type is Fusion.CloudServices.");
			if (TryDiscoverPhotonRealtimeClientRoute(fieldInfo, value, traceBuilder, out cache, out peer))
			{
				return true;
			}
			if (TryDiscoverFusionCloudCommunicatorRoute(fieldInfo, value, traceBuilder, out cache, out peer))
			{
				return true;
			}
			AppendCloudServicesHierarchyMemberHints(value.GetType(), traceBuilder);
			return false;
		}

		private static bool TryDiscoverPhotonRealtimeClientRoute(FieldInfo cloudServicesField, object cloudServicesHolder, StringBuilder traceBuilder, out PhotonPeerResolutionCache cache, out PhotonPeer peer)
		{
			cache = null;
			peer = null;
			Type type = cloudServicesHolder.GetType();
			for (int i = 0; i < CloudServicesRealtimeClientFieldCandidates.Length; i++)
			{
				string text = CloudServicesRealtimeClientFieldCandidates[i];
				string text2 = $"4{(char)(97 + i)}";
				FieldInfo fieldInfo = FindDeclaredInstanceField(type, text);
				if (fieldInfo == null)
				{
					traceBuilder.AppendLine("Step " + text2 + ": No instance field '" + text + "' on " + type.FullName + " or base types.");
					continue;
				}
				if (!typeof(RealtimeClient).IsAssignableFrom(fieldInfo.FieldType))
				{
					traceBuilder.AppendLine("Step " + text2 + " SKIP: Field '" + text + "' type " + fieldInfo.FieldType.FullName + " is not assignable to Photon.Realtime.RealtimeClient.");
					continue;
				}
				if (!(fieldInfo.GetValue(cloudServicesHolder) is RealtimeClient { RealtimePeer: var realtimePeer }))
				{
					traceBuilder.AppendLine("Step " + text2 + " FAIL: Field '" + text + "' is null or not a Photon.Realtime.RealtimeClient instance.");
					continue;
				}
				if (realtimePeer == null)
				{
					traceBuilder.AppendLine("Step " + text2 + " FAIL: RealtimeClient.RealtimePeer is null.");
					continue;
				}
				traceBuilder.AppendLine("Step " + text2 + " OK: Field '" + text + "' (" + fieldInfo.FieldType.FullName + ").");
				traceBuilder.AppendLine("Step 5 OK: PhotonPeer from Photon.Realtime.RealtimeClient.");
				peer = realtimePeer;
				cache = PhotonPeerResolutionCache.CreatePhotonRealtimeClientFieldRoute(cloudServicesField, fieldInfo);
				return true;
			}
			PropertyInfo propertyInfo = FindDeclaredInstanceProperty(type, "RealtimeClient");
			if (propertyInfo == null)
			{
				traceBuilder.AppendLine("Step 4e: No instance property 'RealtimeClient' on CloudServices via Photon.Realtime (trying communicator path).");
				return false;
			}
			if (!typeof(RealtimeClient).IsAssignableFrom(propertyInfo.PropertyType))
			{
				traceBuilder.AppendLine("Step 4e SKIP: Property 'RealtimeClient' type " + propertyInfo.PropertyType.FullName + " is not assignable to Photon.Realtime.RealtimeClient.");
				return false;
			}
			MethodInfo getMethod = propertyInfo.GetGetMethod(nonPublic: true);
			if (getMethod == null)
			{
				traceBuilder.AppendLine("Step 4e FAIL: Property 'RealtimeClient' has no accessible getter.");
				return false;
			}
			if (!(getMethod.Invoke(cloudServicesHolder, null) is RealtimeClient { RealtimePeer: var realtimePeer2 }))
			{
				traceBuilder.AppendLine("Step 4e FAIL: Property 'RealtimeClient' returned null or not Photon.Realtime.RealtimeClient.");
				return false;
			}
			if (realtimePeer2 == null)
			{
				traceBuilder.AppendLine("Step 4e FAIL: RealtimeClient.RealtimePeer is null.");
				return false;
			}
			traceBuilder.AppendLine("Step 4e OK: Property 'RealtimeClient' (" + propertyInfo.PropertyType.FullName + ").");
			traceBuilder.AppendLine("Step 5 OK: PhotonPeer from Photon.Realtime.RealtimeClient.");
			peer = realtimePeer2;
			cache = PhotonPeerResolutionCache.CreatePhotonRealtimeClientPropertyRoute(cloudServicesField, propertyInfo);
			return true;
		}

		private static bool TryDiscoverFusionCloudCommunicatorRoute(FieldInfo cloudServicesField, object cloudServicesHolder, StringBuilder traceBuilder, out PhotonPeerResolutionCache cache, out PhotonPeer peer)
		{
			cache = null;
			peer = null;
			Type type = cloudServicesHolder.GetType();
			string[] cloudServicesCommunicatorFieldCandidates = CloudServicesCommunicatorFieldCandidates;
			foreach (string text in cloudServicesCommunicatorFieldCandidates)
			{
				FieldInfo fieldInfo = FindDeclaredInstanceField(type, text);
				if (!(fieldInfo == null))
				{
					object value = fieldInfo.GetValue(cloudServicesHolder);
					if (TryFinalizeCommunicatorRoute(cloudServicesField, ReflectionHop.FromField(fieldInfo), default(ReflectionHop), value, traceBuilder, "4h field '" + text + "'", out cache, out peer))
					{
						return true;
					}
				}
			}
			cloudServicesCommunicatorFieldCandidates = CloudServicesCommunicatorPropertyCandidates;
			foreach (string text2 in cloudServicesCommunicatorFieldCandidates)
			{
				PropertyInfo propertyInfo = FindDeclaredInstanceProperty(type, text2);
				if (propertyInfo == null)
				{
					continue;
				}
				MethodInfo getMethod = propertyInfo.GetGetMethod(nonPublic: true);
				if (!(getMethod == null))
				{
					object communicatorObject;
					try
					{
						communicatorObject = getMethod.Invoke(cloudServicesHolder, null);
					}
					catch (Exception)
					{
						continue;
					}
					if (TryFinalizeCommunicatorRoute(cloudServicesField, ReflectionHop.FromProperty(propertyInfo), default(ReflectionHop), communicatorObject, traceBuilder, "4h property '" + text2 + "'", out cache, out peer))
					{
						return true;
					}
				}
			}
			cloudServicesCommunicatorFieldCandidates = CloudServicesIntermediateFieldCandidates;
			foreach (string text3 in cloudServicesCommunicatorFieldCandidates)
			{
				FieldInfo fieldInfo2 = FindDeclaredInstanceField(type, text3);
				if (fieldInfo2 == null)
				{
					continue;
				}
				object value2 = fieldInfo2.GetValue(cloudServicesHolder);
				if (value2 == null)
				{
					continue;
				}
				ReflectionHop hopFromCloudServices = ReflectionHop.FromField(fieldInfo2);
				string[] cloudServicesCommunicatorFieldCandidates2 = CloudServicesCommunicatorFieldCandidates;
				foreach (string text4 in cloudServicesCommunicatorFieldCandidates2)
				{
					FieldInfo fieldInfo3 = FindDeclaredInstanceField(value2.GetType(), text4);
					if (!(fieldInfo3 == null))
					{
						object value3 = fieldInfo3.GetValue(value2);
						if (TryFinalizeCommunicatorRoute(cloudServicesField, hopFromCloudServices, ReflectionHop.FromField(fieldInfo3), value3, traceBuilder, "4h nested field '" + text3 + "' → '" + text4 + "'", out cache, out peer))
						{
							return true;
						}
					}
				}
				cloudServicesCommunicatorFieldCandidates2 = CloudServicesCommunicatorPropertyCandidates;
				foreach (string text5 in cloudServicesCommunicatorFieldCandidates2)
				{
					PropertyInfo propertyInfo2 = FindDeclaredInstanceProperty(value2.GetType(), text5);
					if (propertyInfo2 == null)
					{
						continue;
					}
					MethodInfo getMethod2 = propertyInfo2.GetGetMethod(nonPublic: true);
					if (!(getMethod2 == null))
					{
						object communicatorObject2;
						try
						{
							communicatorObject2 = getMethod2.Invoke(value2, null);
						}
						catch (Exception)
						{
							continue;
						}
						if (TryFinalizeCommunicatorRoute(cloudServicesField, hopFromCloudServices, ReflectionHop.FromProperty(propertyInfo2), communicatorObject2, traceBuilder, "4h nested field '" + text3 + "' → property '" + text5 + "'", out cache, out peer))
						{
							return true;
						}
					}
				}
			}
			cloudServicesCommunicatorFieldCandidates = CloudServicesIntermediatePropertyCandidates;
			foreach (string text6 in cloudServicesCommunicatorFieldCandidates)
			{
				PropertyInfo propertyInfo3 = FindDeclaredInstanceProperty(type, text6);
				if (propertyInfo3 == null)
				{
					continue;
				}
				MethodInfo getMethod3 = propertyInfo3.GetGetMethod(nonPublic: true);
				if (getMethod3 == null)
				{
					continue;
				}
				object obj;
				try
				{
					obj = getMethod3.Invoke(cloudServicesHolder, null);
				}
				catch (Exception)
				{
					continue;
				}
				if (obj == null)
				{
					continue;
				}
				ReflectionHop hopFromCloudServices2 = ReflectionHop.FromProperty(propertyInfo3);
				string[] cloudServicesCommunicatorFieldCandidates2 = CloudServicesCommunicatorFieldCandidates;
				foreach (string text7 in cloudServicesCommunicatorFieldCandidates2)
				{
					FieldInfo fieldInfo4 = FindDeclaredInstanceField(obj.GetType(), text7);
					if (!(fieldInfo4 == null))
					{
						object value4 = fieldInfo4.GetValue(obj);
						if (TryFinalizeCommunicatorRoute(cloudServicesField, hopFromCloudServices2, ReflectionHop.FromField(fieldInfo4), value4, traceBuilder, "4h nested property '" + text6 + "' → field '" + text7 + "'", out cache, out peer))
						{
							return true;
						}
					}
				}
				cloudServicesCommunicatorFieldCandidates2 = CloudServicesCommunicatorPropertyCandidates;
				foreach (string text8 in cloudServicesCommunicatorFieldCandidates2)
				{
					PropertyInfo propertyInfo4 = FindDeclaredInstanceProperty(obj.GetType(), text8);
					if (propertyInfo4 == null)
					{
						continue;
					}
					MethodInfo getMethod4 = propertyInfo4.GetGetMethod(nonPublic: true);
					if (!(getMethod4 == null))
					{
						object communicatorObject3;
						try
						{
							communicatorObject3 = getMethod4.Invoke(obj, null);
						}
						catch (Exception)
						{
							continue;
						}
						if (TryFinalizeCommunicatorRoute(cloudServicesField, hopFromCloudServices2, ReflectionHop.FromProperty(propertyInfo4), communicatorObject3, traceBuilder, "4h nested property '" + text6 + "' → property '" + text8 + "'", out cache, out peer))
						{
							return true;
						}
					}
				}
			}
			traceBuilder.AppendLine("Step 4h FAIL: No pinned CloudServices → CloudCommunicator → Client path produced RealtimePeer (extend name lists if Fusion layout differs).");
			return false;
		}

		private static bool TryFinalizeCommunicatorRoute(FieldInfo cloudServicesField, ReflectionHop hopFromCloudServices, ReflectionHop hopFromIntermediate, object communicatorObject, StringBuilder traceBuilder, string routeDescription, out PhotonPeerResolutionCache cache, out PhotonPeer peer)
		{
			cache = null;
			peer = null;
			if (!IsFusionCloudCommunicatorType(communicatorObject?.GetType()))
			{
				return false;
			}
			if (!TryBuildCommunicatorClientBinding(communicatorObject, out var binding))
			{
				return false;
			}
			if (!binding.TryReadClientObject(communicatorObject, out var clientObject))
			{
				return false;
			}
			if (!TryGetPhotonPeerFromRealtimeClientLikeInstance(clientObject, out var peer2))
			{
				return false;
			}
			traceBuilder.AppendLine("Step 4h OK (" + routeDescription + ") → Fusion.CloudCommunicator → Client → RealtimePeer.");
			traceBuilder.AppendLine("Step 5 OK: PhotonPeer via Fusion.CloudCommunicator.Client.");
			peer = peer2;
			bool flag = !hopFromIntermediate.IsEmpty;
			cache = PhotonPeerResolutionCache.CreateFusionCloudCommunicatorRoute(cloudServicesField, hopFromCloudServices, flag ? hopFromIntermediate : default(ReflectionHop), flag, binding);
			return true;
		}

		private static bool TryBuildCommunicatorClientBinding(object communicator, out CommunicatorClientBinding binding)
		{
			binding = null;
			Type type = communicator.GetType();
			string[] cloudCommunicatorClientFieldCandidates = CloudCommunicatorClientFieldCandidates;
			PhotonPeer peer;
			foreach (string fieldName in cloudCommunicatorClientFieldCandidates)
			{
				FieldInfo fieldInfo = FindDeclaredInstanceField(type, fieldName);
				if (!(fieldInfo == null))
				{
					object value;
					try
					{
						value = fieldInfo.GetValue(communicator);
					}
					catch (Exception)
					{
						continue;
					}
					if (TryGetPhotonPeerFromRealtimeClientLikeInstance(value, out peer))
					{
						binding = CommunicatorClientBinding.FromField(fieldInfo);
						return true;
					}
				}
			}
			PropertyInfo propertyInfo = FindDeclaredInstanceProperty(type, "Client");
			if (propertyInfo != null)
			{
				MethodInfo getMethod = propertyInfo.GetGetMethod(nonPublic: true);
				if (getMethod != null)
				{
					object clientLikeObject;
					try
					{
						clientLikeObject = getMethod.Invoke(communicator, null);
					}
					catch (Exception)
					{
						clientLikeObject = null;
					}
					if (TryGetPhotonPeerFromRealtimeClientLikeInstance(clientLikeObject, out peer))
					{
						binding = CommunicatorClientBinding.FromProperty(propertyInfo);
						return true;
					}
				}
			}
			return false;
		}

		private static bool IsFusionCloudCommunicatorType(Type type)
		{
			if (type != null && type.Namespace == "Fusion")
			{
				return type.Name == "CloudCommunicator";
			}
			return false;
		}

		private static bool TryGetPhotonPeerFromRealtimeClientLikeInstance(object clientLikeObject, out PhotonPeer peer)
		{
			peer = null;
			if (clientLikeObject == null)
			{
				return false;
			}
			if (clientLikeObject is RealtimeClient realtimeClient)
			{
				peer = realtimeClient.RealtimePeer;
				return peer != null;
			}
			PropertyInfo propertyInfo = FindInstancePropertyDeclaredOnHierarchy(clientLikeObject.GetType(), "RealtimePeer");
			if (propertyInfo == null)
			{
				return false;
			}
			MethodInfo getMethod = propertyInfo.GetGetMethod(nonPublic: true);
			if (getMethod == null)
			{
				return false;
			}
			object obj;
			try
			{
				obj = getMethod.Invoke(clientLikeObject, null);
			}
			catch (Exception)
			{
				return false;
			}
			peer = obj as PhotonPeer;
			return peer != null;
		}

		private static PropertyInfo FindInstancePropertyDeclaredOnHierarchy(Type startType, string propertyName)
		{
			Type type = startType;
			while (type != null)
			{
				PropertyInfo property = type.GetProperty(propertyName, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (property != null && property.GetIndexParameters().Length == 0)
				{
					return property;
				}
				type = type.BaseType;
			}
			return null;
		}

		private static void AppendCloudServicesHierarchyMemberHints(Type cloudServicesType, StringBuilder traceBuilder)
		{
			traceBuilder.AppendLine("Step 4 FINAL: Declared instance members by type (Fusion often puts storage on bases):");
			Type type = cloudServicesType;
			while (type != null)
			{
				traceBuilder.AppendLine("  --- " + type.FullName + " ---");
				FieldInfo[] fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (fields.Length == 0)
				{
					traceBuilder.AppendLine("    (no fields)");
				}
				else
				{
					FieldInfo[] array = fields;
					foreach (FieldInfo fieldInfo in array)
					{
						traceBuilder.AppendLine("    field " + fieldInfo.FieldType.FullName + " " + fieldInfo.Name);
					}
				}
				PropertyInfo[] properties = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (properties.Length == 0)
				{
					traceBuilder.AppendLine("    (no properties)");
				}
				else
				{
					PropertyInfo[] array2 = properties;
					foreach (PropertyInfo propertyInfo in array2)
					{
						traceBuilder.AppendLine("    property " + propertyInfo.PropertyType.FullName + " " + propertyInfo.Name);
					}
				}
				type = type.BaseType;
			}
		}

		private static FieldInfo FindDeclaredInstanceField(Type declaringOrDerivedType, string fieldName)
		{
			Type type = declaringOrDerivedType;
			while (type != null)
			{
				FieldInfo field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (field != null)
				{
					return field;
				}
				type = type.BaseType;
			}
			return null;
		}

		private static PropertyInfo FindDeclaredInstanceProperty(Type declaringOrDerivedType, string propertyName)
		{
			Type type = declaringOrDerivedType;
			while (type != null)
			{
				PropertyInfo property = type.GetProperty(propertyName, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (property != null && property.GetIndexParameters().Length == 0)
				{
					return property;
				}
				type = type.BaseType;
			}
			return null;
		}
	}
}
