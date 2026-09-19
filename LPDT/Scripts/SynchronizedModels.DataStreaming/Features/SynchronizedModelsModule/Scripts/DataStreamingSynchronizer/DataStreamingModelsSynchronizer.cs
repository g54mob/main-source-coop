using System;
using System.Collections.Generic;
using System.Text;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using Fusion.Sockets;
using NetworkServices.NetworkEvents;
using RSG.Muffin.JsonConvertorSubmodule.JsonConvertorModule.Core.Interfaces;
using Zenject;

namespace Features.SynchronizedModelsModule.Scripts.DataStreamingSynchronizer
{
	public class DataStreamingModelsSynchronizer : IInitializable, IDisposable
	{
		private readonly NetworkRunnerEventBus _eventBus;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly List<IDataStreamSynchronizable> _synchronizedModels;

		private readonly List<ICustomDataStreamSynchronizable> _customSynchronizedModels;

		private readonly IJsonConvertor _jsonConvertor;

		public DataStreamingModelsSynchronizer(NetworkRunnerEventBus eventBus, MultiplayerModel multiplayerModel, List<IDataStreamSynchronizable> synchronizedModels, List<ICustomDataStreamSynchronizable> customSynchronizedModels, IJsonConvertor jsonConvertor)
		{
			_eventBus = eventBus;
			_multiplayerModel = multiplayerModel;
			_synchronizedModels = synchronizedModels;
			_customSynchronizedModels = customSynchronizedModels;
			_jsonConvertor = jsonConvertor;
		}

		public void Initialize()
		{
			foreach (IDataStreamSynchronizable synchronizedModel in _synchronizedModels)
			{
				synchronizedModel.CallSynchronize += Synchronize;
				synchronizedModel.SynchronizeRequest += SendRequestToMaster;
			}
			foreach (ICustomDataStreamSynchronizable customSynchronizedModel in _customSynchronizedModels)
			{
				customSynchronizedModel.CallCustomSynchronize += CustomSynchronize;
				customSynchronizedModel.CustomSynchronizeRequest += SendRequestToMaster;
			}
			_eventBus.Subscribe<OnReliableDataReceivedEvent>(OnDataReceived);
		}

		public void Dispose()
		{
			foreach (IDataStreamSynchronizable synchronizedModel in _synchronizedModels)
			{
				synchronizedModel.CallSynchronize -= Synchronize;
				synchronizedModel.SynchronizeRequest -= SendRequestToMaster;
			}
			foreach (ICustomDataStreamSynchronizable customSynchronizedModel in _customSynchronizedModels)
			{
				customSynchronizedModel.CallCustomSynchronize -= CustomSynchronize;
				customSynchronizedModel.CustomSynchronizeRequest -= SendRequestToMaster;
			}
			_eventBus.Unsubscribe<OnReliableDataReceivedEvent>(OnDataReceived);
		}

		private void OnDataReceived(OnReliableDataReceivedEvent onReliableDataReceivedEvent)
		{
			if (TryConvertReceivedDataToPlayerId(onReliableDataReceivedEvent, out var playerIdDataHolder) && _multiplayerModel.NetworkRunner.IsSharedModeMasterClient)
			{
				bool flag = false;
				foreach (IDataStreamSynchronizable synchronizedModel in _synchronizedModels)
				{
					ReliableKey reliableKey = synchronizedModel.GetReliableKey();
					if (CompareReliableKeys(reliableKey, onReliableDataReceivedEvent.Key))
					{
						SynchronizeToSpecificPlayer(synchronizedModel, playerIdDataHolder.SendedPlayerId);
						flag = true;
					}
				}
				foreach (ICustomDataStreamSynchronizable customSynchronizedModel in _customSynchronizedModels)
				{
					ReliableKey customReliableKey = customSynchronizedModel.GetCustomReliableKey();
					if (CompareReliableKeys(customReliableKey, onReliableDataReceivedEvent.Key))
					{
						SynchronizeToSpecificPlayer(customSynchronizedModel, playerIdDataHolder.SendedPlayerId);
						flag = true;
					}
				}
				if (flag)
				{
					return;
				}
			}
			foreach (IDataStreamSynchronizable synchronizedModel2 in _synchronizedModels)
			{
				ReliableKey reliableKey2 = synchronizedModel2.GetReliableKey();
				ReliableKey key = onReliableDataReceivedEvent.Key;
				if (CompareReliableKeys(reliableKey2, key))
				{
					if (onReliableDataReceivedEvent.Data.Length != 0)
					{
						synchronizedModel2.SetDataInBytes(onReliableDataReceivedEvent.Data);
					}
					else
					{
						synchronizedModel2.ReSetDataInBytes();
					}
				}
			}
			foreach (ICustomDataStreamSynchronizable customSynchronizedModel2 in _customSynchronizedModels)
			{
				ReliableKey customReliableKey2 = customSynchronizedModel2.GetCustomReliableKey();
				ReliableKey key2 = onReliableDataReceivedEvent.Key;
				if (CompareReliableKeys(customReliableKey2, key2))
				{
					customSynchronizedModel2.SetCustomDataInBytes(onReliableDataReceivedEvent.Data);
				}
			}
		}

		private bool CompareReliableKeys(ReliableKey key1, ReliableKey key2)
		{
			key1.GetUlongs(out var key3, out var key4);
			key2.GetUlongs(out var key5, out var key6);
			if (key3 == key5)
			{
				return key4 == key6;
			}
			return false;
		}

		private void Synchronize(IDataStreamSynchronizable dataStreamSynchronizable)
		{
			if (!(_multiplayerModel.NetworkRunner == null))
			{
				SendToAllPlayers(dataStreamSynchronizable.GetReliableKey(), dataStreamSynchronizable.TryGetDataInBytes(out var value) ? value : null);
			}
		}

		private void SynchronizeToSpecificPlayer(IDataStreamSynchronizable dataStreamSynchronizable, int playerId)
		{
			if (!(_multiplayerModel.NetworkRunner == null))
			{
				SendToSpecificPlayer(dataStreamSynchronizable.GetReliableKey(), dataStreamSynchronizable.TryGetDataInBytes(out var value) ? value : null, playerId);
			}
		}

		private void CustomSynchronize(ICustomDataStreamSynchronizable customDataStreamSynchronizable)
		{
			if (!(_multiplayerModel.NetworkRunner == null))
			{
				SendToAllPlayers(customDataStreamSynchronizable.GetCustomReliableKey(), customDataStreamSynchronizable.GetCustomDataInBytes());
			}
		}

		private void SynchronizeToSpecificPlayer(ICustomDataStreamSynchronizable customDataStreamSynchronizable, int playerId)
		{
			if (!(_multiplayerModel.NetworkRunner == null))
			{
				SendToSpecificPlayer(customDataStreamSynchronizable.GetCustomReliableKey(), customDataStreamSynchronizable.GetCustomDataInBytes(), playerId);
			}
		}

		private void SendToAllPlayers(ReliableKey reliableKey, byte[] data)
		{
			if (data == null || data.Length == 0)
			{
				foreach (PlayerRef activePlayer in _multiplayerModel.NetworkRunner.ActivePlayers)
				{
					_multiplayerModel.NetworkRunner.SendReliableDataToPlayer(activePlayer, reliableKey, null);
				}
				return;
			}
			foreach (PlayerRef activePlayer2 in _multiplayerModel.NetworkRunner.ActivePlayers)
			{
				_multiplayerModel.NetworkRunner.SendReliableDataToPlayer(activePlayer2, reliableKey, data);
			}
		}

		private void SendToSpecificPlayer(ReliableKey reliableKey, byte[] data, int playerId)
		{
			if (data == null || data.Length == 0)
			{
				foreach (PlayerRef activePlayer in _multiplayerModel.NetworkRunner.ActivePlayers)
				{
					if (activePlayer.PlayerId == playerId)
					{
						_multiplayerModel.NetworkRunner.SendReliableDataToPlayer(activePlayer, reliableKey, null);
					}
				}
				return;
			}
			foreach (PlayerRef activePlayer2 in _multiplayerModel.NetworkRunner.ActivePlayers)
			{
				if (activePlayer2.PlayerId == playerId)
				{
					_multiplayerModel.NetworkRunner.SendReliableDataToPlayer(activePlayer2, reliableKey, data);
				}
			}
		}

		private void SendRequestToMaster(IDataStreamSynchronizable dataStreamSynchronizable, PlayerIdDataHolder playerIdDataHolder)
		{
			if (_multiplayerModel.NetworkMasterClientTracker == null)
			{
				return;
			}
			foreach (PlayerRef activePlayer in _multiplayerModel.NetworkRunner.ActivePlayers)
			{
				if (_multiplayerModel.NetworkMasterClientTracker.CheckMasterClient(activePlayer))
				{
					string s = _jsonConvertor.Convert(playerIdDataHolder);
					_multiplayerModel.NetworkRunner.SendReliableDataToPlayer(activePlayer, dataStreamSynchronizable.GetReliableKey(), Encoding.UTF8.GetBytes(s));
				}
			}
		}

		private void SendRequestToMaster(ICustomDataStreamSynchronizable dataStreamSynchronizable, PlayerIdDataHolder playerIdDataHolder)
		{
			if (_multiplayerModel.NetworkMasterClientTracker == null)
			{
				return;
			}
			foreach (PlayerRef activePlayer in _multiplayerModel.NetworkRunner.ActivePlayers)
			{
				if (_multiplayerModel.NetworkMasterClientTracker.CheckMasterClient(activePlayer))
				{
					string s = _jsonConvertor.Convert(playerIdDataHolder);
					_multiplayerModel.NetworkRunner.SendReliableDataToPlayer(activePlayer, dataStreamSynchronizable.GetCustomReliableKey(), Encoding.UTF8.GetBytes(s));
				}
			}
		}

		private bool TryConvertReceivedDataToPlayerId(OnReliableDataReceivedEvent onReliableDataReceivedEvent, out PlayerIdDataHolder playerIdDataHolder)
		{
			playerIdDataHolder = _jsonConvertor.Convert<PlayerIdDataHolder>(Encoding.UTF8.GetString(onReliableDataReceivedEvent.Data));
			if (playerIdDataHolder != null && playerIdDataHolder.RequestType == "DataStreamSyncRequest")
			{
				return playerIdDataHolder.SendedPlayerId != 0;
			}
			return false;
		}
	}
}
