using System;
using System.Collections.Generic;
using Features.NetworkTelemetry.Scripts;
using MessagePack.Formatters;

namespace MessagePack
{
	internal class GeneratedMessagePackResolver : IFormatterResolver
	{
		private static class FormatterCache<T>
		{
			internal static readonly IMessagePackFormatter<T> Formatter;

			static FormatterCache()
			{
				object formatter = GeneratedMessagePackResolverGetFormatterHelper.GetFormatter(typeof(T));
				if (formatter != null)
				{
					Formatter = (IMessagePackFormatter<T>)formatter;
				}
			}
		}

		private static class GeneratedMessagePackResolverGetFormatterHelper
		{
			private static readonly Dictionary<Type, int> closedTypeLookup = new Dictionary<Type, int>(18)
			{
				{
					typeof(Dictionary<string, string>),
					0
				},
				{
					typeof(List<EnemyPositionSample>),
					1
				},
				{
					typeof(List<FrameTimingSample>),
					2
				},
				{
					typeof(List<FusionTelemetryStatSample>),
					3
				},
				{
					typeof(List<GrabObjectPositionSample>),
					4
				},
				{
					typeof(List<LogEntry>),
					5
				},
				{
					typeof(List<NetworkObjectBandwidthSample>),
					6
				},
				{
					typeof(List<PositionSample>),
					7
				},
				{
					typeof(List<TelemetryEvent>),
					8
				},
				{
					typeof(TelemetryPacket),
					9
				},
				{
					typeof(EnemyPositionSample),
					10
				},
				{
					typeof(FrameTimingSample),
					11
				},
				{
					typeof(FusionTelemetryStatSample),
					12
				},
				{
					typeof(GrabObjectPositionSample),
					13
				},
				{
					typeof(LogEntry),
					14
				},
				{
					typeof(NetworkObjectBandwidthSample),
					15
				},
				{
					typeof(PositionSample),
					16
				},
				{
					typeof(TelemetryEvent),
					17
				}
			};

			internal static object GetFormatter(Type t)
			{
				if (closedTypeLookup.TryGetValue(t, out var value))
				{
					return value switch
					{
						0 => new DictionaryFormatter<string, string>(), 
						1 => new ListFormatter<EnemyPositionSample>(), 
						2 => new ListFormatter<FrameTimingSample>(), 
						3 => new ListFormatter<FusionTelemetryStatSample>(), 
						4 => new ListFormatter<GrabObjectPositionSample>(), 
						5 => new ListFormatter<LogEntry>(), 
						6 => new ListFormatter<NetworkObjectBandwidthSample>(), 
						7 => new ListFormatter<PositionSample>(), 
						8 => new ListFormatter<TelemetryEvent>(), 
						9 => new Features.NetworkTelemetry.Scripts.TelemetryPacketFormatter(), 
						10 => new Features.NetworkTelemetry.Scripts.EnemyPositionSampleFormatter(), 
						11 => new Features.NetworkTelemetry.Scripts.FrameTimingSampleFormatter(), 
						12 => new Features.NetworkTelemetry.Scripts.FusionTelemetryStatSampleFormatter(), 
						13 => new Features.NetworkTelemetry.Scripts.GrabObjectPositionSampleFormatter(), 
						14 => new Features.NetworkTelemetry.Scripts.LogEntryFormatter(), 
						15 => new Features.NetworkTelemetry.Scripts.NetworkObjectBandwidthSampleFormatter(), 
						16 => new Features.NetworkTelemetry.Scripts.PositionSampleFormatter(), 
						17 => new Features.NetworkTelemetry.Scripts.TelemetryEventFormatter(), 
						_ => null, 
					};
				}
				return null;
			}
		}

		internal class Features
		{
			internal class NetworkTelemetry
			{
				internal class Scripts
				{
					internal sealed class TelemetryPacketFormatter : IMessagePackFormatter<TelemetryPacket>, IMessagePackFormatter
					{
						public void Serialize(ref MessagePackWriter writer, TelemetryPacket value, MessagePackSerializerOptions options)
						{
							if (value == null)
							{
								writer.WriteNil();
								return;
							}
							IFormatterResolver resolver = options.Resolver;
							writer.WriteArrayHeader(13);
							resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.SessionId, options);
							resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.PlayerId, options);
							resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.SessionPlayerId, options);
							resolver.GetFormatterWithVerify<List<PositionSample>>().Serialize(ref writer, value.Positions, options);
							resolver.GetFormatterWithVerify<List<LogEntry>>().Serialize(ref writer, value.Logs, options);
							resolver.GetFormatterWithVerify<List<TelemetryEvent>>().Serialize(ref writer, value.Events, options);
							resolver.GetFormatterWithVerify<List<FusionTelemetryStatSample>>().Serialize(ref writer, value.FusionTelemetryStats, options);
							resolver.GetFormatterWithVerify<List<EnemyPositionSample>>().Serialize(ref writer, value.EnemyPositions, options);
							resolver.GetFormatterWithVerify<List<GrabObjectPositionSample>>().Serialize(ref writer, value.GrabObjectPositions, options);
							resolver.GetFormatterWithVerify<List<FrameTimingSample>>().Serialize(ref writer, value.FrameTimingSamples, options);
							resolver.GetFormatterWithVerify<List<NetworkObjectBandwidthSample>>().Serialize(ref writer, value.NetworkObjectBandwidthSamples, options);
							resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GitCommitShort, options);
							resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.AppVersion, options);
						}

						public TelemetryPacket Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
						{
							if (reader.TryReadNil())
							{
								return null;
							}
							options.Security.DepthStep(ref reader);
							IFormatterResolver resolver = options.Resolver;
							int num = reader.ReadArrayHeader();
							TelemetryPacket telemetryPacket = new TelemetryPacket();
							for (int i = 0; i < num; i++)
							{
								switch (i)
								{
								case 0:
									telemetryPacket.SessionId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
									break;
								case 1:
									telemetryPacket.PlayerId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
									break;
								case 2:
									telemetryPacket.SessionPlayerId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
									break;
								case 3:
									reader.Skip();
									break;
								case 4:
									reader.Skip();
									break;
								case 5:
									reader.Skip();
									break;
								case 6:
									reader.Skip();
									break;
								case 7:
									reader.Skip();
									break;
								case 8:
									reader.Skip();
									break;
								case 9:
									reader.Skip();
									break;
								case 10:
									reader.Skip();
									break;
								case 11:
									telemetryPacket.GitCommitShort = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
									break;
								case 12:
									telemetryPacket.AppVersion = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
									break;
								default:
									reader.Skip();
									break;
								}
							}
							reader.Depth--;
							return telemetryPacket;
						}
					}

					internal sealed class EnemyPositionSampleFormatter : IMessagePackFormatter<EnemyPositionSample>, IMessagePackFormatter
					{
						public void Serialize(ref MessagePackWriter writer, EnemyPositionSample value, MessagePackSerializerOptions options)
						{
							IFormatterResolver resolver = options.Resolver;
							writer.WriteArrayHeader(8);
							writer.Write(value.SessionTimeMs);
							resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.EnemyKind, options);
							resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.NetworkObjectId, options);
							writer.Write(value.X);
							writer.Write(value.Y);
							writer.Write(value.Z);
							writer.Write(value.Yaw);
							writer.Write(value.CurrentState);
						}

						public EnemyPositionSample Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
						{
							if (reader.TryReadNil())
							{
								throw new InvalidOperationException("typecode is null, struct not supported");
							}
							options.Security.DepthStep(ref reader);
							IFormatterResolver resolver = options.Resolver;
							int num = reader.ReadArrayHeader();
							EnemyPositionSample result = default(EnemyPositionSample);
							for (int i = 0; i < num; i++)
							{
								switch (i)
								{
								case 0:
									result.SessionTimeMs = reader.ReadInt32();
									break;
								case 1:
									result.EnemyKind = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
									break;
								case 2:
									result.NetworkObjectId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
									break;
								case 3:
									result.X = reader.ReadSingle();
									break;
								case 4:
									result.Y = reader.ReadSingle();
									break;
								case 5:
									result.Z = reader.ReadSingle();
									break;
								case 6:
									result.Yaw = reader.ReadSingle();
									break;
								case 7:
									result.CurrentState = reader.ReadInt32();
									break;
								default:
									reader.Skip();
									break;
								}
							}
							reader.Depth--;
							return result;
						}
					}

					internal sealed class FrameTimingSampleFormatter : IMessagePackFormatter<FrameTimingSample>, IMessagePackFormatter
					{
						public void Serialize(ref MessagePackWriter writer, FrameTimingSample value, MessagePackSerializerOptions options)
						{
							writer.WriteArrayHeader(9);
							writer.Write(value.SessionTimeMs);
							writer.Write(value.PingMs);
							writer.Write(value.Fps);
							writer.Write(value.GpuFrameTimeMs);
							writer.Write(value.CpuMainThreadFrameTimeMs);
							writer.Write(value.CpuRenderThreadFrameTimeMs);
							writer.Write(value.CpuMainThreadPresentWaitTimeMs);
							writer.Write(value.ProcessCpuUsagePct);
							writer.Write(value.ProcessRamUsageBytes);
						}

						public FrameTimingSample Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
						{
							if (reader.TryReadNil())
							{
								throw new InvalidOperationException("typecode is null, struct not supported");
							}
							options.Security.DepthStep(ref reader);
							int num = reader.ReadArrayHeader();
							FrameTimingSample result = default(FrameTimingSample);
							for (int i = 0; i < num; i++)
							{
								switch (i)
								{
								case 0:
									result.SessionTimeMs = reader.ReadInt32();
									break;
								case 1:
									result.PingMs = reader.ReadInt32();
									break;
								case 2:
									result.Fps = reader.ReadSingle();
									break;
								case 3:
									result.GpuFrameTimeMs = reader.ReadDouble();
									break;
								case 4:
									result.CpuMainThreadFrameTimeMs = reader.ReadDouble();
									break;
								case 5:
									result.CpuRenderThreadFrameTimeMs = reader.ReadDouble();
									break;
								case 6:
									result.CpuMainThreadPresentWaitTimeMs = reader.ReadDouble();
									break;
								case 7:
									result.ProcessCpuUsagePct = reader.ReadSingle();
									break;
								case 8:
									result.ProcessRamUsageBytes = reader.ReadInt64();
									break;
								default:
									reader.Skip();
									break;
								}
							}
							reader.Depth--;
							return result;
						}
					}

					internal sealed class FusionTelemetryStatSampleFormatter : IMessagePackFormatter<FusionTelemetryStatSample>, IMessagePackFormatter
					{
						public void Serialize(ref MessagePackWriter writer, FusionTelemetryStatSample value, MessagePackSerializerOptions options)
						{
							writer.WriteArrayHeader(18);
							writer.Write(value.SessionTimeMs);
							writer.Write(value.LocalRttSeconds);
							writer.Write(value.CloudRttSeconds);
							writer.Write(value.Player1Id);
							writer.Write(value.Player1Rtt);
							writer.Write(value.Player2Id);
							writer.Write(value.Player2Rtt);
							writer.Write(value.Player3Id);
							writer.Write(value.Player3Rtt);
							writer.Write(value.Player4Id);
							writer.Write(value.Player4Rtt);
							writer.Write(value.PhotonBytesInTotal);
							writer.Write(value.PhotonBytesOutTotal);
							writer.Write(value.PhotonBytesInDelta);
							writer.Write(value.PhotonBytesOutDelta);
							writer.Write(value.PhotonPeerRttMs);
							writer.Write(value.PhotonPacketLossByCrc);
							writer.Write(value.PhotonResentReliableCommands);
						}

						public FusionTelemetryStatSample Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
						{
							if (reader.TryReadNil())
							{
								throw new InvalidOperationException("typecode is null, struct not supported");
							}
							options.Security.DepthStep(ref reader);
							int num = reader.ReadArrayHeader();
							FusionTelemetryStatSample result = default(FusionTelemetryStatSample);
							for (int i = 0; i < num; i++)
							{
								switch (i)
								{
								case 0:
									result.SessionTimeMs = reader.ReadInt32();
									break;
								case 1:
									result.LocalRttSeconds = reader.ReadSingle();
									break;
								case 2:
									result.CloudRttSeconds = reader.ReadSingle();
									break;
								case 3:
									result.Player1Id = reader.ReadInt32();
									break;
								case 4:
									result.Player1Rtt = reader.ReadSingle();
									break;
								case 5:
									result.Player2Id = reader.ReadInt32();
									break;
								case 6:
									result.Player2Rtt = reader.ReadSingle();
									break;
								case 7:
									result.Player3Id = reader.ReadInt32();
									break;
								case 8:
									result.Player3Rtt = reader.ReadSingle();
									break;
								case 9:
									result.Player4Id = reader.ReadInt32();
									break;
								case 10:
									result.Player4Rtt = reader.ReadSingle();
									break;
								case 11:
									result.PhotonBytesInTotal = reader.ReadInt64();
									break;
								case 12:
									result.PhotonBytesOutTotal = reader.ReadInt64();
									break;
								case 13:
									result.PhotonBytesInDelta = reader.ReadInt64();
									break;
								case 14:
									result.PhotonBytesOutDelta = reader.ReadInt64();
									break;
								case 15:
									result.PhotonPeerRttMs = reader.ReadInt32();
									break;
								case 16:
									result.PhotonPacketLossByCrc = reader.ReadInt64();
									break;
								case 17:
									result.PhotonResentReliableCommands = reader.ReadInt64();
									break;
								default:
									reader.Skip();
									break;
								}
							}
							reader.Depth--;
							return result;
						}
					}

					internal sealed class GrabObjectPositionSampleFormatter : IMessagePackFormatter<GrabObjectPositionSample>, IMessagePackFormatter
					{
						public void Serialize(ref MessagePackWriter writer, GrabObjectPositionSample value, MessagePackSerializerOptions options)
						{
							IFormatterResolver resolver = options.Resolver;
							writer.WriteArrayHeader(8);
							writer.Write(value.SessionTimeMs);
							resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.GrabObjectType, options);
							resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.NetworkObjectId, options);
							writer.Write(value.X);
							writer.Write(value.Y);
							writer.Write(value.Z);
							writer.Write(value.Yaw);
							writer.Write(value.IsAuthority);
						}

						public GrabObjectPositionSample Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
						{
							if (reader.TryReadNil())
							{
								throw new InvalidOperationException("typecode is null, struct not supported");
							}
							options.Security.DepthStep(ref reader);
							IFormatterResolver resolver = options.Resolver;
							int num = reader.ReadArrayHeader();
							GrabObjectPositionSample result = default(GrabObjectPositionSample);
							for (int i = 0; i < num; i++)
							{
								switch (i)
								{
								case 0:
									result.SessionTimeMs = reader.ReadInt32();
									break;
								case 1:
									result.GrabObjectType = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
									break;
								case 2:
									result.NetworkObjectId = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
									break;
								case 3:
									result.X = reader.ReadSingle();
									break;
								case 4:
									result.Y = reader.ReadSingle();
									break;
								case 5:
									result.Z = reader.ReadSingle();
									break;
								case 6:
									result.Yaw = reader.ReadSingle();
									break;
								case 7:
									result.IsAuthority = reader.ReadBoolean();
									break;
								default:
									reader.Skip();
									break;
								}
							}
							reader.Depth--;
							return result;
						}
					}

					internal sealed class LogEntryFormatter : IMessagePackFormatter<LogEntry>, IMessagePackFormatter
					{
						public void Serialize(ref MessagePackWriter writer, LogEntry value, MessagePackSerializerOptions options)
						{
							IFormatterResolver resolver = options.Resolver;
							writer.WriteArrayHeader(4);
							writer.Write(value.LocalTimeMs);
							writer.Write(value.SessionTimeMs);
							resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.Level, options);
							resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.Message, options);
						}

						public LogEntry Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
						{
							if (reader.TryReadNil())
							{
								throw new InvalidOperationException("typecode is null, struct not supported");
							}
							options.Security.DepthStep(ref reader);
							IFormatterResolver resolver = options.Resolver;
							int num = reader.ReadArrayHeader();
							LogEntry result = default(LogEntry);
							for (int i = 0; i < num; i++)
							{
								switch (i)
								{
								case 0:
									result.LocalTimeMs = reader.ReadInt64();
									break;
								case 1:
									result.SessionTimeMs = reader.ReadInt32();
									break;
								case 2:
									result.Level = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
									break;
								case 3:
									result.Message = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
									break;
								default:
									reader.Skip();
									break;
								}
							}
							reader.Depth--;
							return result;
						}
					}

					internal sealed class NetworkObjectBandwidthSampleFormatter : IMessagePackFormatter<NetworkObjectBandwidthSample>, IMessagePackFormatter
					{
						public void Serialize(ref MessagePackWriter writer, NetworkObjectBandwidthSample value, MessagePackSerializerOptions options)
						{
							IFormatterResolver resolver = options.Resolver;
							writer.WriteArrayHeader(4);
							writer.Write(value.SessionTimeMs);
							resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.ObjectName, options);
							writer.Write(value.InBandwidth);
							writer.Write(value.OutBandwidth);
						}

						public NetworkObjectBandwidthSample Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
						{
							if (reader.TryReadNil())
							{
								throw new InvalidOperationException("typecode is null, struct not supported");
							}
							options.Security.DepthStep(ref reader);
							IFormatterResolver resolver = options.Resolver;
							int num = reader.ReadArrayHeader();
							NetworkObjectBandwidthSample result = default(NetworkObjectBandwidthSample);
							for (int i = 0; i < num; i++)
							{
								switch (i)
								{
								case 0:
									result.SessionTimeMs = reader.ReadInt32();
									break;
								case 1:
									result.ObjectName = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
									break;
								case 2:
									result.InBandwidth = reader.ReadSingle();
									break;
								case 3:
									result.OutBandwidth = reader.ReadSingle();
									break;
								default:
									reader.Skip();
									break;
								}
							}
							reader.Depth--;
							return result;
						}
					}

					internal sealed class PositionSampleFormatter : IMessagePackFormatter<PositionSample>, IMessagePackFormatter
					{
						public void Serialize(ref MessagePackWriter writer, PositionSample value, MessagePackSerializerOptions options)
						{
							writer.WriteArrayHeader(5);
							writer.Write(value.SessionTimeMs);
							writer.Write(value.X);
							writer.Write(value.Y);
							writer.Write(value.Z);
							writer.Write(value.Yaw);
						}

						public PositionSample Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
						{
							if (reader.TryReadNil())
							{
								throw new InvalidOperationException("typecode is null, struct not supported");
							}
							options.Security.DepthStep(ref reader);
							int num = reader.ReadArrayHeader();
							PositionSample result = default(PositionSample);
							for (int i = 0; i < num; i++)
							{
								switch (i)
								{
								case 0:
									result.SessionTimeMs = reader.ReadInt32();
									break;
								case 1:
									result.X = reader.ReadSingle();
									break;
								case 2:
									result.Y = reader.ReadSingle();
									break;
								case 3:
									result.Z = reader.ReadSingle();
									break;
								case 4:
									result.Yaw = reader.ReadSingle();
									break;
								default:
									reader.Skip();
									break;
								}
							}
							reader.Depth--;
							return result;
						}
					}

					internal sealed class TelemetryEventFormatter : IMessagePackFormatter<TelemetryEvent>, IMessagePackFormatter
					{
						public void Serialize(ref MessagePackWriter writer, TelemetryEvent value, MessagePackSerializerOptions options)
						{
							IFormatterResolver resolver = options.Resolver;
							writer.WriteArrayHeader(4);
							writer.Write(value.LocalTimeMs);
							writer.Write(value.SessionTimeMs);
							resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.Name, options);
							resolver.GetFormatterWithVerify<Dictionary<string, string>>().Serialize(ref writer, value.Extras, options);
						}

						public TelemetryEvent Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
						{
							if (reader.TryReadNil())
							{
								throw new InvalidOperationException("typecode is null, struct not supported");
							}
							options.Security.DepthStep(ref reader);
							IFormatterResolver resolver = options.Resolver;
							int num = reader.ReadArrayHeader();
							TelemetryEvent result = default(TelemetryEvent);
							for (int i = 0; i < num; i++)
							{
								switch (i)
								{
								case 0:
									result.LocalTimeMs = reader.ReadInt64();
									break;
								case 1:
									result.SessionTimeMs = reader.ReadInt32();
									break;
								case 2:
									result.Name = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
									break;
								case 3:
									result.Extras = resolver.GetFormatterWithVerify<Dictionary<string, string>>().Deserialize(ref reader, options);
									break;
								default:
									reader.Skip();
									break;
								}
							}
							reader.Depth--;
							return result;
						}
					}
				}
			}
		}

		public static readonly IFormatterResolver Instance = new GeneratedMessagePackResolver();

		private GeneratedMessagePackResolver()
		{
		}

		public IMessagePackFormatter<T> GetFormatter<T>()
		{
			return FormatterCache<T>.Formatter;
		}
	}
}
