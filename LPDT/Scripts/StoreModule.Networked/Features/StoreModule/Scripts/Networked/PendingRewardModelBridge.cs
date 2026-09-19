using System;
using Features.NetworkedModelRuntime;
using Fusion;
using UnityEngine;

namespace Features.StoreModule.Scripts.Networked
{
	public class PendingRewardModelBridge : IDisposable, INetworkedModelShadowBridge
	{
		private readonly PendingRewardModel _pendingRewardModel;

		private PendingRewardNetworkObject _pendingRewardNetworkObject;

		private bool _hasPendingTargetLevelNumber;

		private int _pendingTargetLevelNumber;

		private bool _hasPendingSlot0CardNetId;

		private int _pendingSlot0CardNetId;

		private bool _hasPendingSlot0CardDataId;

		private int _pendingSlot0CardDataId;

		private bool _hasPendingSlot0ColorPacked;

		private int _pendingSlot0ColorPacked;

		private bool _hasPendingSlot0TargetPlayerId;

		private int _pendingSlot0TargetPlayerId;

		private bool _hasPendingSlot1CardNetId;

		private int _pendingSlot1CardNetId;

		private bool _hasPendingSlot1CardDataId;

		private int _pendingSlot1CardDataId;

		private bool _hasPendingSlot1ColorPacked;

		private int _pendingSlot1ColorPacked;

		private bool _hasPendingSlot1TargetPlayerId;

		private int _pendingSlot1TargetPlayerId;

		private bool _hasPendingSlot2CardNetId;

		private int _pendingSlot2CardNetId;

		private bool _hasPendingSlot2CardDataId;

		private int _pendingSlot2CardDataId;

		private bool _hasPendingSlot2ColorPacked;

		private int _pendingSlot2ColorPacked;

		private bool _hasPendingSlot2TargetPlayerId;

		private int _pendingSlot2TargetPlayerId;

		private bool _hasPendingSlot3CardNetId;

		private int _pendingSlot3CardNetId;

		private bool _hasPendingSlot3CardDataId;

		private int _pendingSlot3CardDataId;

		private bool _hasPendingSlot3ColorPacked;

		private int _pendingSlot3ColorPacked;

		private bool _hasPendingSlot3TargetPlayerId;

		private int _pendingSlot3TargetPlayerId;

		private bool _hasPendingSlot4CardNetId;

		private int _pendingSlot4CardNetId;

		private bool _hasPendingSlot4CardDataId;

		private int _pendingSlot4CardDataId;

		private bool _hasPendingSlot4ColorPacked;

		private int _pendingSlot4ColorPacked;

		private bool _hasPendingSlot4TargetPlayerId;

		private int _pendingSlot4TargetPlayerId;

		private bool _hasPendingSlot5CardNetId;

		private int _pendingSlot5CardNetId;

		private bool _hasPendingSlot5CardDataId;

		private int _pendingSlot5CardDataId;

		private bool _hasPendingSlot5ColorPacked;

		private int _pendingSlot5ColorPacked;

		private bool _hasPendingSlot5TargetPlayerId;

		private int _pendingSlot5TargetPlayerId;

		private bool _hasPendingSlot6CardNetId;

		private int _pendingSlot6CardNetId;

		private bool _hasPendingSlot6CardDataId;

		private int _pendingSlot6CardDataId;

		private bool _hasPendingSlot6ColorPacked;

		private int _pendingSlot6ColorPacked;

		private bool _hasPendingSlot6TargetPlayerId;

		private int _pendingSlot6TargetPlayerId;

		private bool _hasPendingSlot7CardNetId;

		private int _pendingSlot7CardNetId;

		private bool _hasPendingSlot7CardDataId;

		private int _pendingSlot7CardDataId;

		private bool _hasPendingSlot7ColorPacked;

		private int _pendingSlot7ColorPacked;

		private bool _hasPendingSlot7TargetPlayerId;

		private int _pendingSlot7TargetPlayerId;

		private bool _hasPendingSlot8CardNetId;

		private int _pendingSlot8CardNetId;

		private bool _hasPendingSlot8CardDataId;

		private int _pendingSlot8CardDataId;

		private bool _hasPendingSlot8ColorPacked;

		private int _pendingSlot8ColorPacked;

		private bool _hasPendingSlot8TargetPlayerId;

		private int _pendingSlot8TargetPlayerId;

		private bool _hasPendingSlot9CardNetId;

		private int _pendingSlot9CardNetId;

		private bool _hasPendingSlot9CardDataId;

		private int _pendingSlot9CardDataId;

		private bool _hasPendingSlot9ColorPacked;

		private int _pendingSlot9ColorPacked;

		private bool _hasPendingSlot9TargetPlayerId;

		private int _pendingSlot9TargetPlayerId;

		private bool _hasPendingSlot10CardNetId;

		private int _pendingSlot10CardNetId;

		private bool _hasPendingSlot10CardDataId;

		private int _pendingSlot10CardDataId;

		private bool _hasPendingSlot10ColorPacked;

		private int _pendingSlot10ColorPacked;

		private bool _hasPendingSlot10TargetPlayerId;

		private int _pendingSlot10TargetPlayerId;

		private bool _hasPendingSlot11CardNetId;

		private int _pendingSlot11CardNetId;

		private bool _hasPendingSlot11CardDataId;

		private int _pendingSlot11CardDataId;

		private bool _hasPendingSlot11ColorPacked;

		private int _pendingSlot11ColorPacked;

		private bool _hasPendingSlot11TargetPlayerId;

		private int _pendingSlot11TargetPlayerId;

		private bool _hasPendingSlot12CardNetId;

		private int _pendingSlot12CardNetId;

		private bool _hasPendingSlot12CardDataId;

		private int _pendingSlot12CardDataId;

		private bool _hasPendingSlot12ColorPacked;

		private int _pendingSlot12ColorPacked;

		private bool _hasPendingSlot12TargetPlayerId;

		private int _pendingSlot12TargetPlayerId;

		private bool _hasPendingSlot13CardNetId;

		private int _pendingSlot13CardNetId;

		private bool _hasPendingSlot13CardDataId;

		private int _pendingSlot13CardDataId;

		private bool _hasPendingSlot13ColorPacked;

		private int _pendingSlot13ColorPacked;

		private bool _hasPendingSlot13TargetPlayerId;

		private int _pendingSlot13TargetPlayerId;

		private bool _hasPendingSlot14CardNetId;

		private int _pendingSlot14CardNetId;

		private bool _hasPendingSlot14CardDataId;

		private int _pendingSlot14CardDataId;

		private bool _hasPendingSlot14ColorPacked;

		private int _pendingSlot14ColorPacked;

		private bool _hasPendingSlot14TargetPlayerId;

		private int _pendingSlot14TargetPlayerId;

		private bool _hasPendingSlot15CardNetId;

		private int _pendingSlot15CardNetId;

		private bool _hasPendingSlot15CardDataId;

		private int _pendingSlot15CardDataId;

		private bool _hasPendingSlot15ColorPacked;

		private int _pendingSlot15ColorPacked;

		private bool _hasPendingSlot15TargetPlayerId;

		private int _pendingSlot15TargetPlayerId;

		private bool _hasPendingSlot16CardNetId;

		private int _pendingSlot16CardNetId;

		private bool _hasPendingSlot16CardDataId;

		private int _pendingSlot16CardDataId;

		private bool _hasPendingSlot16ColorPacked;

		private int _pendingSlot16ColorPacked;

		private bool _hasPendingSlot16TargetPlayerId;

		private int _pendingSlot16TargetPlayerId;

		private bool _hasPendingSlot17CardNetId;

		private int _pendingSlot17CardNetId;

		private bool _hasPendingSlot17CardDataId;

		private int _pendingSlot17CardDataId;

		private bool _hasPendingSlot17ColorPacked;

		private int _pendingSlot17ColorPacked;

		private bool _hasPendingSlot17TargetPlayerId;

		private int _pendingSlot17TargetPlayerId;

		private bool _hasPendingSlot18CardNetId;

		private int _pendingSlot18CardNetId;

		private bool _hasPendingSlot18CardDataId;

		private int _pendingSlot18CardDataId;

		private bool _hasPendingSlot18ColorPacked;

		private int _pendingSlot18ColorPacked;

		private bool _hasPendingSlot18TargetPlayerId;

		private int _pendingSlot18TargetPlayerId;

		private bool _hasPendingSlot19CardNetId;

		private int _pendingSlot19CardNetId;

		private bool _hasPendingSlot19CardDataId;

		private int _pendingSlot19CardDataId;

		private bool _hasPendingSlot19ColorPacked;

		private int _pendingSlot19ColorPacked;

		private bool _hasPendingSlot19TargetPlayerId;

		private int _pendingSlot19TargetPlayerId;

		private bool _hasPendingSlot20CardNetId;

		private int _pendingSlot20CardNetId;

		private bool _hasPendingSlot20CardDataId;

		private int _pendingSlot20CardDataId;

		private bool _hasPendingSlot20ColorPacked;

		private int _pendingSlot20ColorPacked;

		private bool _hasPendingSlot20TargetPlayerId;

		private int _pendingSlot20TargetPlayerId;

		private bool _hasPendingSlot21CardNetId;

		private int _pendingSlot21CardNetId;

		private bool _hasPendingSlot21CardDataId;

		private int _pendingSlot21CardDataId;

		private bool _hasPendingSlot21ColorPacked;

		private int _pendingSlot21ColorPacked;

		private bool _hasPendingSlot21TargetPlayerId;

		private int _pendingSlot21TargetPlayerId;

		private bool _hasPendingSlot22CardNetId;

		private int _pendingSlot22CardNetId;

		private bool _hasPendingSlot22CardDataId;

		private int _pendingSlot22CardDataId;

		private bool _hasPendingSlot22ColorPacked;

		private int _pendingSlot22ColorPacked;

		private bool _hasPendingSlot22TargetPlayerId;

		private int _pendingSlot22TargetPlayerId;

		private bool _hasPendingSlot23CardNetId;

		private int _pendingSlot23CardNetId;

		private bool _hasPendingSlot23CardDataId;

		private int _pendingSlot23CardDataId;

		private bool _hasPendingSlot23ColorPacked;

		private int _pendingSlot23ColorPacked;

		private bool _hasPendingSlot23TargetPlayerId;

		private int _pendingSlot23TargetPlayerId;

		private bool _hasPendingSlot24CardNetId;

		private int _pendingSlot24CardNetId;

		private bool _hasPendingSlot24CardDataId;

		private int _pendingSlot24CardDataId;

		private bool _hasPendingSlot24ColorPacked;

		private int _pendingSlot24ColorPacked;

		private bool _hasPendingSlot24TargetPlayerId;

		private int _pendingSlot24TargetPlayerId;

		private bool _hasPendingSlot25CardNetId;

		private int _pendingSlot25CardNetId;

		private bool _hasPendingSlot25CardDataId;

		private int _pendingSlot25CardDataId;

		private bool _hasPendingSlot25ColorPacked;

		private int _pendingSlot25ColorPacked;

		private bool _hasPendingSlot25TargetPlayerId;

		private int _pendingSlot25TargetPlayerId;

		private bool _hasPendingSlot26CardNetId;

		private int _pendingSlot26CardNetId;

		private bool _hasPendingSlot26CardDataId;

		private int _pendingSlot26CardDataId;

		private bool _hasPendingSlot26ColorPacked;

		private int _pendingSlot26ColorPacked;

		private bool _hasPendingSlot26TargetPlayerId;

		private int _pendingSlot26TargetPlayerId;

		private bool _hasPendingSlot27CardNetId;

		private int _pendingSlot27CardNetId;

		private bool _hasPendingSlot27CardDataId;

		private int _pendingSlot27CardDataId;

		private bool _hasPendingSlot27ColorPacked;

		private int _pendingSlot27ColorPacked;

		private bool _hasPendingSlot27TargetPlayerId;

		private int _pendingSlot27TargetPlayerId;

		private bool _hasPendingSlot28CardNetId;

		private int _pendingSlot28CardNetId;

		private bool _hasPendingSlot28CardDataId;

		private int _pendingSlot28CardDataId;

		private bool _hasPendingSlot28ColorPacked;

		private int _pendingSlot28ColorPacked;

		private bool _hasPendingSlot28TargetPlayerId;

		private int _pendingSlot28TargetPlayerId;

		private bool _hasPendingSlot29CardNetId;

		private int _pendingSlot29CardNetId;

		private bool _hasPendingSlot29CardDataId;

		private int _pendingSlot29CardDataId;

		private bool _hasPendingSlot29ColorPacked;

		private int _pendingSlot29ColorPacked;

		private bool _hasPendingSlot29TargetPlayerId;

		private int _pendingSlot29TargetPlayerId;

		private bool _hasPendingSlot30CardNetId;

		private int _pendingSlot30CardNetId;

		private bool _hasPendingSlot30CardDataId;

		private int _pendingSlot30CardDataId;

		private bool _hasPendingSlot30ColorPacked;

		private int _pendingSlot30ColorPacked;

		private bool _hasPendingSlot30TargetPlayerId;

		private int _pendingSlot30TargetPlayerId;

		private bool _hasPendingSlot31CardNetId;

		private int _pendingSlot31CardNetId;

		private bool _hasPendingSlot31CardDataId;

		private int _pendingSlot31CardDataId;

		private bool _hasPendingSlot31ColorPacked;

		private int _pendingSlot31ColorPacked;

		private bool _hasPendingSlot31TargetPlayerId;

		private int _pendingSlot31TargetPlayerId;

		public PendingRewardModelBridge(PendingRewardModel pendingRewardModel)
		{
			_pendingRewardModel = pendingRewardModel;
		}

		public void Bind(PendingRewardNetworkObject pendingRewardNetworkObject)
		{
			Unbind();
			_pendingRewardNetworkObject = pendingRewardNetworkObject;
			_pendingRewardNetworkObject.OnNetworkedTargetLevelNumberChanged += HandleNetworkedTargetLevelNumberChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot0CardNetIdChanged += HandleNetworkedSlot0CardNetIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot0CardDataIdChanged += HandleNetworkedSlot0CardDataIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot0ColorPackedChanged += HandleNetworkedSlot0ColorPackedChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot0TargetPlayerIdChanged += HandleNetworkedSlot0TargetPlayerIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot1CardNetIdChanged += HandleNetworkedSlot1CardNetIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot1CardDataIdChanged += HandleNetworkedSlot1CardDataIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot1ColorPackedChanged += HandleNetworkedSlot1ColorPackedChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot1TargetPlayerIdChanged += HandleNetworkedSlot1TargetPlayerIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot2CardNetIdChanged += HandleNetworkedSlot2CardNetIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot2CardDataIdChanged += HandleNetworkedSlot2CardDataIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot2ColorPackedChanged += HandleNetworkedSlot2ColorPackedChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot2TargetPlayerIdChanged += HandleNetworkedSlot2TargetPlayerIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot3CardNetIdChanged += HandleNetworkedSlot3CardNetIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot3CardDataIdChanged += HandleNetworkedSlot3CardDataIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot3ColorPackedChanged += HandleNetworkedSlot3ColorPackedChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot3TargetPlayerIdChanged += HandleNetworkedSlot3TargetPlayerIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot4CardNetIdChanged += HandleNetworkedSlot4CardNetIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot4CardDataIdChanged += HandleNetworkedSlot4CardDataIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot4ColorPackedChanged += HandleNetworkedSlot4ColorPackedChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot4TargetPlayerIdChanged += HandleNetworkedSlot4TargetPlayerIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot5CardNetIdChanged += HandleNetworkedSlot5CardNetIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot5CardDataIdChanged += HandleNetworkedSlot5CardDataIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot5ColorPackedChanged += HandleNetworkedSlot5ColorPackedChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot5TargetPlayerIdChanged += HandleNetworkedSlot5TargetPlayerIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot6CardNetIdChanged += HandleNetworkedSlot6CardNetIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot6CardDataIdChanged += HandleNetworkedSlot6CardDataIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot6ColorPackedChanged += HandleNetworkedSlot6ColorPackedChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot6TargetPlayerIdChanged += HandleNetworkedSlot6TargetPlayerIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot7CardNetIdChanged += HandleNetworkedSlot7CardNetIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot7CardDataIdChanged += HandleNetworkedSlot7CardDataIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot7ColorPackedChanged += HandleNetworkedSlot7ColorPackedChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot7TargetPlayerIdChanged += HandleNetworkedSlot7TargetPlayerIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot8CardNetIdChanged += HandleNetworkedSlot8CardNetIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot8CardDataIdChanged += HandleNetworkedSlot8CardDataIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot8ColorPackedChanged += HandleNetworkedSlot8ColorPackedChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot8TargetPlayerIdChanged += HandleNetworkedSlot8TargetPlayerIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot9CardNetIdChanged += HandleNetworkedSlot9CardNetIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot9CardDataIdChanged += HandleNetworkedSlot9CardDataIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot9ColorPackedChanged += HandleNetworkedSlot9ColorPackedChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot9TargetPlayerIdChanged += HandleNetworkedSlot9TargetPlayerIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot10CardNetIdChanged += HandleNetworkedSlot10CardNetIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot10CardDataIdChanged += HandleNetworkedSlot10CardDataIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot10ColorPackedChanged += HandleNetworkedSlot10ColorPackedChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot10TargetPlayerIdChanged += HandleNetworkedSlot10TargetPlayerIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot11CardNetIdChanged += HandleNetworkedSlot11CardNetIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot11CardDataIdChanged += HandleNetworkedSlot11CardDataIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot11ColorPackedChanged += HandleNetworkedSlot11ColorPackedChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot11TargetPlayerIdChanged += HandleNetworkedSlot11TargetPlayerIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot12CardNetIdChanged += HandleNetworkedSlot12CardNetIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot12CardDataIdChanged += HandleNetworkedSlot12CardDataIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot12ColorPackedChanged += HandleNetworkedSlot12ColorPackedChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot12TargetPlayerIdChanged += HandleNetworkedSlot12TargetPlayerIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot13CardNetIdChanged += HandleNetworkedSlot13CardNetIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot13CardDataIdChanged += HandleNetworkedSlot13CardDataIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot13ColorPackedChanged += HandleNetworkedSlot13ColorPackedChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot13TargetPlayerIdChanged += HandleNetworkedSlot13TargetPlayerIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot14CardNetIdChanged += HandleNetworkedSlot14CardNetIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot14CardDataIdChanged += HandleNetworkedSlot14CardDataIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot14ColorPackedChanged += HandleNetworkedSlot14ColorPackedChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot14TargetPlayerIdChanged += HandleNetworkedSlot14TargetPlayerIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot15CardNetIdChanged += HandleNetworkedSlot15CardNetIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot15CardDataIdChanged += HandleNetworkedSlot15CardDataIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot15ColorPackedChanged += HandleNetworkedSlot15ColorPackedChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot15TargetPlayerIdChanged += HandleNetworkedSlot15TargetPlayerIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot16CardNetIdChanged += HandleNetworkedSlot16CardNetIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot16CardDataIdChanged += HandleNetworkedSlot16CardDataIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot16ColorPackedChanged += HandleNetworkedSlot16ColorPackedChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot16TargetPlayerIdChanged += HandleNetworkedSlot16TargetPlayerIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot17CardNetIdChanged += HandleNetworkedSlot17CardNetIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot17CardDataIdChanged += HandleNetworkedSlot17CardDataIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot17ColorPackedChanged += HandleNetworkedSlot17ColorPackedChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot17TargetPlayerIdChanged += HandleNetworkedSlot17TargetPlayerIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot18CardNetIdChanged += HandleNetworkedSlot18CardNetIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot18CardDataIdChanged += HandleNetworkedSlot18CardDataIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot18ColorPackedChanged += HandleNetworkedSlot18ColorPackedChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot18TargetPlayerIdChanged += HandleNetworkedSlot18TargetPlayerIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot19CardNetIdChanged += HandleNetworkedSlot19CardNetIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot19CardDataIdChanged += HandleNetworkedSlot19CardDataIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot19ColorPackedChanged += HandleNetworkedSlot19ColorPackedChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot19TargetPlayerIdChanged += HandleNetworkedSlot19TargetPlayerIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot20CardNetIdChanged += HandleNetworkedSlot20CardNetIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot20CardDataIdChanged += HandleNetworkedSlot20CardDataIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot20ColorPackedChanged += HandleNetworkedSlot20ColorPackedChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot20TargetPlayerIdChanged += HandleNetworkedSlot20TargetPlayerIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot21CardNetIdChanged += HandleNetworkedSlot21CardNetIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot21CardDataIdChanged += HandleNetworkedSlot21CardDataIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot21ColorPackedChanged += HandleNetworkedSlot21ColorPackedChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot21TargetPlayerIdChanged += HandleNetworkedSlot21TargetPlayerIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot22CardNetIdChanged += HandleNetworkedSlot22CardNetIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot22CardDataIdChanged += HandleNetworkedSlot22CardDataIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot22ColorPackedChanged += HandleNetworkedSlot22ColorPackedChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot22TargetPlayerIdChanged += HandleNetworkedSlot22TargetPlayerIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot23CardNetIdChanged += HandleNetworkedSlot23CardNetIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot23CardDataIdChanged += HandleNetworkedSlot23CardDataIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot23ColorPackedChanged += HandleNetworkedSlot23ColorPackedChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot23TargetPlayerIdChanged += HandleNetworkedSlot23TargetPlayerIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot24CardNetIdChanged += HandleNetworkedSlot24CardNetIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot24CardDataIdChanged += HandleNetworkedSlot24CardDataIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot24ColorPackedChanged += HandleNetworkedSlot24ColorPackedChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot24TargetPlayerIdChanged += HandleNetworkedSlot24TargetPlayerIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot25CardNetIdChanged += HandleNetworkedSlot25CardNetIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot25CardDataIdChanged += HandleNetworkedSlot25CardDataIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot25ColorPackedChanged += HandleNetworkedSlot25ColorPackedChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot25TargetPlayerIdChanged += HandleNetworkedSlot25TargetPlayerIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot26CardNetIdChanged += HandleNetworkedSlot26CardNetIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot26CardDataIdChanged += HandleNetworkedSlot26CardDataIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot26ColorPackedChanged += HandleNetworkedSlot26ColorPackedChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot26TargetPlayerIdChanged += HandleNetworkedSlot26TargetPlayerIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot27CardNetIdChanged += HandleNetworkedSlot27CardNetIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot27CardDataIdChanged += HandleNetworkedSlot27CardDataIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot27ColorPackedChanged += HandleNetworkedSlot27ColorPackedChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot27TargetPlayerIdChanged += HandleNetworkedSlot27TargetPlayerIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot28CardNetIdChanged += HandleNetworkedSlot28CardNetIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot28CardDataIdChanged += HandleNetworkedSlot28CardDataIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot28ColorPackedChanged += HandleNetworkedSlot28ColorPackedChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot28TargetPlayerIdChanged += HandleNetworkedSlot28TargetPlayerIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot29CardNetIdChanged += HandleNetworkedSlot29CardNetIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot29CardDataIdChanged += HandleNetworkedSlot29CardDataIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot29ColorPackedChanged += HandleNetworkedSlot29ColorPackedChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot29TargetPlayerIdChanged += HandleNetworkedSlot29TargetPlayerIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot30CardNetIdChanged += HandleNetworkedSlot30CardNetIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot30CardDataIdChanged += HandleNetworkedSlot30CardDataIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot30ColorPackedChanged += HandleNetworkedSlot30ColorPackedChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot30TargetPlayerIdChanged += HandleNetworkedSlot30TargetPlayerIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot31CardNetIdChanged += HandleNetworkedSlot31CardNetIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot31CardDataIdChanged += HandleNetworkedSlot31CardDataIdChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot31ColorPackedChanged += HandleNetworkedSlot31ColorPackedChanged;
			_pendingRewardNetworkObject.OnNetworkedSlot31TargetPlayerIdChanged += HandleNetworkedSlot31TargetPlayerIdChanged;
			_pendingRewardNetworkObject.OnAuthoritativeTick += HandleAuthoritativeTick;
			_pendingRewardNetworkObject.OnDespawned += HandleDespawned;
			_pendingRewardModel.TargetLevelNumber.BindWriter(WriteTargetLevelNumber);
			_pendingRewardModel.Slot0CardNetId.BindWriter(WriteSlot0CardNetId);
			_pendingRewardModel.Slot0CardDataId.BindWriter(WriteSlot0CardDataId);
			_pendingRewardModel.Slot0ColorPacked.BindWriter(WriteSlot0ColorPacked);
			_pendingRewardModel.Slot0TargetPlayerId.BindWriter(WriteSlot0TargetPlayerId);
			_pendingRewardModel.Slot1CardNetId.BindWriter(WriteSlot1CardNetId);
			_pendingRewardModel.Slot1CardDataId.BindWriter(WriteSlot1CardDataId);
			_pendingRewardModel.Slot1ColorPacked.BindWriter(WriteSlot1ColorPacked);
			_pendingRewardModel.Slot1TargetPlayerId.BindWriter(WriteSlot1TargetPlayerId);
			_pendingRewardModel.Slot2CardNetId.BindWriter(WriteSlot2CardNetId);
			_pendingRewardModel.Slot2CardDataId.BindWriter(WriteSlot2CardDataId);
			_pendingRewardModel.Slot2ColorPacked.BindWriter(WriteSlot2ColorPacked);
			_pendingRewardModel.Slot2TargetPlayerId.BindWriter(WriteSlot2TargetPlayerId);
			_pendingRewardModel.Slot3CardNetId.BindWriter(WriteSlot3CardNetId);
			_pendingRewardModel.Slot3CardDataId.BindWriter(WriteSlot3CardDataId);
			_pendingRewardModel.Slot3ColorPacked.BindWriter(WriteSlot3ColorPacked);
			_pendingRewardModel.Slot3TargetPlayerId.BindWriter(WriteSlot3TargetPlayerId);
			_pendingRewardModel.Slot4CardNetId.BindWriter(WriteSlot4CardNetId);
			_pendingRewardModel.Slot4CardDataId.BindWriter(WriteSlot4CardDataId);
			_pendingRewardModel.Slot4ColorPacked.BindWriter(WriteSlot4ColorPacked);
			_pendingRewardModel.Slot4TargetPlayerId.BindWriter(WriteSlot4TargetPlayerId);
			_pendingRewardModel.Slot5CardNetId.BindWriter(WriteSlot5CardNetId);
			_pendingRewardModel.Slot5CardDataId.BindWriter(WriteSlot5CardDataId);
			_pendingRewardModel.Slot5ColorPacked.BindWriter(WriteSlot5ColorPacked);
			_pendingRewardModel.Slot5TargetPlayerId.BindWriter(WriteSlot5TargetPlayerId);
			_pendingRewardModel.Slot6CardNetId.BindWriter(WriteSlot6CardNetId);
			_pendingRewardModel.Slot6CardDataId.BindWriter(WriteSlot6CardDataId);
			_pendingRewardModel.Slot6ColorPacked.BindWriter(WriteSlot6ColorPacked);
			_pendingRewardModel.Slot6TargetPlayerId.BindWriter(WriteSlot6TargetPlayerId);
			_pendingRewardModel.Slot7CardNetId.BindWriter(WriteSlot7CardNetId);
			_pendingRewardModel.Slot7CardDataId.BindWriter(WriteSlot7CardDataId);
			_pendingRewardModel.Slot7ColorPacked.BindWriter(WriteSlot7ColorPacked);
			_pendingRewardModel.Slot7TargetPlayerId.BindWriter(WriteSlot7TargetPlayerId);
			_pendingRewardModel.Slot8CardNetId.BindWriter(WriteSlot8CardNetId);
			_pendingRewardModel.Slot8CardDataId.BindWriter(WriteSlot8CardDataId);
			_pendingRewardModel.Slot8ColorPacked.BindWriter(WriteSlot8ColorPacked);
			_pendingRewardModel.Slot8TargetPlayerId.BindWriter(WriteSlot8TargetPlayerId);
			_pendingRewardModel.Slot9CardNetId.BindWriter(WriteSlot9CardNetId);
			_pendingRewardModel.Slot9CardDataId.BindWriter(WriteSlot9CardDataId);
			_pendingRewardModel.Slot9ColorPacked.BindWriter(WriteSlot9ColorPacked);
			_pendingRewardModel.Slot9TargetPlayerId.BindWriter(WriteSlot9TargetPlayerId);
			_pendingRewardModel.Slot10CardNetId.BindWriter(WriteSlot10CardNetId);
			_pendingRewardModel.Slot10CardDataId.BindWriter(WriteSlot10CardDataId);
			_pendingRewardModel.Slot10ColorPacked.BindWriter(WriteSlot10ColorPacked);
			_pendingRewardModel.Slot10TargetPlayerId.BindWriter(WriteSlot10TargetPlayerId);
			_pendingRewardModel.Slot11CardNetId.BindWriter(WriteSlot11CardNetId);
			_pendingRewardModel.Slot11CardDataId.BindWriter(WriteSlot11CardDataId);
			_pendingRewardModel.Slot11ColorPacked.BindWriter(WriteSlot11ColorPacked);
			_pendingRewardModel.Slot11TargetPlayerId.BindWriter(WriteSlot11TargetPlayerId);
			_pendingRewardModel.Slot12CardNetId.BindWriter(WriteSlot12CardNetId);
			_pendingRewardModel.Slot12CardDataId.BindWriter(WriteSlot12CardDataId);
			_pendingRewardModel.Slot12ColorPacked.BindWriter(WriteSlot12ColorPacked);
			_pendingRewardModel.Slot12TargetPlayerId.BindWriter(WriteSlot12TargetPlayerId);
			_pendingRewardModel.Slot13CardNetId.BindWriter(WriteSlot13CardNetId);
			_pendingRewardModel.Slot13CardDataId.BindWriter(WriteSlot13CardDataId);
			_pendingRewardModel.Slot13ColorPacked.BindWriter(WriteSlot13ColorPacked);
			_pendingRewardModel.Slot13TargetPlayerId.BindWriter(WriteSlot13TargetPlayerId);
			_pendingRewardModel.Slot14CardNetId.BindWriter(WriteSlot14CardNetId);
			_pendingRewardModel.Slot14CardDataId.BindWriter(WriteSlot14CardDataId);
			_pendingRewardModel.Slot14ColorPacked.BindWriter(WriteSlot14ColorPacked);
			_pendingRewardModel.Slot14TargetPlayerId.BindWriter(WriteSlot14TargetPlayerId);
			_pendingRewardModel.Slot15CardNetId.BindWriter(WriteSlot15CardNetId);
			_pendingRewardModel.Slot15CardDataId.BindWriter(WriteSlot15CardDataId);
			_pendingRewardModel.Slot15ColorPacked.BindWriter(WriteSlot15ColorPacked);
			_pendingRewardModel.Slot15TargetPlayerId.BindWriter(WriteSlot15TargetPlayerId);
			_pendingRewardModel.Slot16CardNetId.BindWriter(WriteSlot16CardNetId);
			_pendingRewardModel.Slot16CardDataId.BindWriter(WriteSlot16CardDataId);
			_pendingRewardModel.Slot16ColorPacked.BindWriter(WriteSlot16ColorPacked);
			_pendingRewardModel.Slot16TargetPlayerId.BindWriter(WriteSlot16TargetPlayerId);
			_pendingRewardModel.Slot17CardNetId.BindWriter(WriteSlot17CardNetId);
			_pendingRewardModel.Slot17CardDataId.BindWriter(WriteSlot17CardDataId);
			_pendingRewardModel.Slot17ColorPacked.BindWriter(WriteSlot17ColorPacked);
			_pendingRewardModel.Slot17TargetPlayerId.BindWriter(WriteSlot17TargetPlayerId);
			_pendingRewardModel.Slot18CardNetId.BindWriter(WriteSlot18CardNetId);
			_pendingRewardModel.Slot18CardDataId.BindWriter(WriteSlot18CardDataId);
			_pendingRewardModel.Slot18ColorPacked.BindWriter(WriteSlot18ColorPacked);
			_pendingRewardModel.Slot18TargetPlayerId.BindWriter(WriteSlot18TargetPlayerId);
			_pendingRewardModel.Slot19CardNetId.BindWriter(WriteSlot19CardNetId);
			_pendingRewardModel.Slot19CardDataId.BindWriter(WriteSlot19CardDataId);
			_pendingRewardModel.Slot19ColorPacked.BindWriter(WriteSlot19ColorPacked);
			_pendingRewardModel.Slot19TargetPlayerId.BindWriter(WriteSlot19TargetPlayerId);
			_pendingRewardModel.Slot20CardNetId.BindWriter(WriteSlot20CardNetId);
			_pendingRewardModel.Slot20CardDataId.BindWriter(WriteSlot20CardDataId);
			_pendingRewardModel.Slot20ColorPacked.BindWriter(WriteSlot20ColorPacked);
			_pendingRewardModel.Slot20TargetPlayerId.BindWriter(WriteSlot20TargetPlayerId);
			_pendingRewardModel.Slot21CardNetId.BindWriter(WriteSlot21CardNetId);
			_pendingRewardModel.Slot21CardDataId.BindWriter(WriteSlot21CardDataId);
			_pendingRewardModel.Slot21ColorPacked.BindWriter(WriteSlot21ColorPacked);
			_pendingRewardModel.Slot21TargetPlayerId.BindWriter(WriteSlot21TargetPlayerId);
			_pendingRewardModel.Slot22CardNetId.BindWriter(WriteSlot22CardNetId);
			_pendingRewardModel.Slot22CardDataId.BindWriter(WriteSlot22CardDataId);
			_pendingRewardModel.Slot22ColorPacked.BindWriter(WriteSlot22ColorPacked);
			_pendingRewardModel.Slot22TargetPlayerId.BindWriter(WriteSlot22TargetPlayerId);
			_pendingRewardModel.Slot23CardNetId.BindWriter(WriteSlot23CardNetId);
			_pendingRewardModel.Slot23CardDataId.BindWriter(WriteSlot23CardDataId);
			_pendingRewardModel.Slot23ColorPacked.BindWriter(WriteSlot23ColorPacked);
			_pendingRewardModel.Slot23TargetPlayerId.BindWriter(WriteSlot23TargetPlayerId);
			_pendingRewardModel.Slot24CardNetId.BindWriter(WriteSlot24CardNetId);
			_pendingRewardModel.Slot24CardDataId.BindWriter(WriteSlot24CardDataId);
			_pendingRewardModel.Slot24ColorPacked.BindWriter(WriteSlot24ColorPacked);
			_pendingRewardModel.Slot24TargetPlayerId.BindWriter(WriteSlot24TargetPlayerId);
			_pendingRewardModel.Slot25CardNetId.BindWriter(WriteSlot25CardNetId);
			_pendingRewardModel.Slot25CardDataId.BindWriter(WriteSlot25CardDataId);
			_pendingRewardModel.Slot25ColorPacked.BindWriter(WriteSlot25ColorPacked);
			_pendingRewardModel.Slot25TargetPlayerId.BindWriter(WriteSlot25TargetPlayerId);
			_pendingRewardModel.Slot26CardNetId.BindWriter(WriteSlot26CardNetId);
			_pendingRewardModel.Slot26CardDataId.BindWriter(WriteSlot26CardDataId);
			_pendingRewardModel.Slot26ColorPacked.BindWriter(WriteSlot26ColorPacked);
			_pendingRewardModel.Slot26TargetPlayerId.BindWriter(WriteSlot26TargetPlayerId);
			_pendingRewardModel.Slot27CardNetId.BindWriter(WriteSlot27CardNetId);
			_pendingRewardModel.Slot27CardDataId.BindWriter(WriteSlot27CardDataId);
			_pendingRewardModel.Slot27ColorPacked.BindWriter(WriteSlot27ColorPacked);
			_pendingRewardModel.Slot27TargetPlayerId.BindWriter(WriteSlot27TargetPlayerId);
			_pendingRewardModel.Slot28CardNetId.BindWriter(WriteSlot28CardNetId);
			_pendingRewardModel.Slot28CardDataId.BindWriter(WriteSlot28CardDataId);
			_pendingRewardModel.Slot28ColorPacked.BindWriter(WriteSlot28ColorPacked);
			_pendingRewardModel.Slot28TargetPlayerId.BindWriter(WriteSlot28TargetPlayerId);
			_pendingRewardModel.Slot29CardNetId.BindWriter(WriteSlot29CardNetId);
			_pendingRewardModel.Slot29CardDataId.BindWriter(WriteSlot29CardDataId);
			_pendingRewardModel.Slot29ColorPacked.BindWriter(WriteSlot29ColorPacked);
			_pendingRewardModel.Slot29TargetPlayerId.BindWriter(WriteSlot29TargetPlayerId);
			_pendingRewardModel.Slot30CardNetId.BindWriter(WriteSlot30CardNetId);
			_pendingRewardModel.Slot30CardDataId.BindWriter(WriteSlot30CardDataId);
			_pendingRewardModel.Slot30ColorPacked.BindWriter(WriteSlot30ColorPacked);
			_pendingRewardModel.Slot30TargetPlayerId.BindWriter(WriteSlot30TargetPlayerId);
			_pendingRewardModel.Slot31CardNetId.BindWriter(WriteSlot31CardNetId);
			_pendingRewardModel.Slot31CardDataId.BindWriter(WriteSlot31CardDataId);
			_pendingRewardModel.Slot31ColorPacked.BindWriter(WriteSlot31ColorPacked);
			_pendingRewardModel.Slot31TargetPlayerId.BindWriter(WriteSlot31TargetPlayerId);
			ApplyTargetLevelNumberFromNetwork(_pendingRewardNetworkObject.TargetLevelNumber);
			ApplySlot0CardNetIdFromNetwork(_pendingRewardNetworkObject.Slot0CardNetId);
			ApplySlot0CardDataIdFromNetwork(_pendingRewardNetworkObject.Slot0CardDataId);
			ApplySlot0ColorPackedFromNetwork(_pendingRewardNetworkObject.Slot0ColorPacked);
			ApplySlot0TargetPlayerIdFromNetwork(_pendingRewardNetworkObject.Slot0TargetPlayerId);
			ApplySlot1CardNetIdFromNetwork(_pendingRewardNetworkObject.Slot1CardNetId);
			ApplySlot1CardDataIdFromNetwork(_pendingRewardNetworkObject.Slot1CardDataId);
			ApplySlot1ColorPackedFromNetwork(_pendingRewardNetworkObject.Slot1ColorPacked);
			ApplySlot1TargetPlayerIdFromNetwork(_pendingRewardNetworkObject.Slot1TargetPlayerId);
			ApplySlot2CardNetIdFromNetwork(_pendingRewardNetworkObject.Slot2CardNetId);
			ApplySlot2CardDataIdFromNetwork(_pendingRewardNetworkObject.Slot2CardDataId);
			ApplySlot2ColorPackedFromNetwork(_pendingRewardNetworkObject.Slot2ColorPacked);
			ApplySlot2TargetPlayerIdFromNetwork(_pendingRewardNetworkObject.Slot2TargetPlayerId);
			ApplySlot3CardNetIdFromNetwork(_pendingRewardNetworkObject.Slot3CardNetId);
			ApplySlot3CardDataIdFromNetwork(_pendingRewardNetworkObject.Slot3CardDataId);
			ApplySlot3ColorPackedFromNetwork(_pendingRewardNetworkObject.Slot3ColorPacked);
			ApplySlot3TargetPlayerIdFromNetwork(_pendingRewardNetworkObject.Slot3TargetPlayerId);
			ApplySlot4CardNetIdFromNetwork(_pendingRewardNetworkObject.Slot4CardNetId);
			ApplySlot4CardDataIdFromNetwork(_pendingRewardNetworkObject.Slot4CardDataId);
			ApplySlot4ColorPackedFromNetwork(_pendingRewardNetworkObject.Slot4ColorPacked);
			ApplySlot4TargetPlayerIdFromNetwork(_pendingRewardNetworkObject.Slot4TargetPlayerId);
			ApplySlot5CardNetIdFromNetwork(_pendingRewardNetworkObject.Slot5CardNetId);
			ApplySlot5CardDataIdFromNetwork(_pendingRewardNetworkObject.Slot5CardDataId);
			ApplySlot5ColorPackedFromNetwork(_pendingRewardNetworkObject.Slot5ColorPacked);
			ApplySlot5TargetPlayerIdFromNetwork(_pendingRewardNetworkObject.Slot5TargetPlayerId);
			ApplySlot6CardNetIdFromNetwork(_pendingRewardNetworkObject.Slot6CardNetId);
			ApplySlot6CardDataIdFromNetwork(_pendingRewardNetworkObject.Slot6CardDataId);
			ApplySlot6ColorPackedFromNetwork(_pendingRewardNetworkObject.Slot6ColorPacked);
			ApplySlot6TargetPlayerIdFromNetwork(_pendingRewardNetworkObject.Slot6TargetPlayerId);
			ApplySlot7CardNetIdFromNetwork(_pendingRewardNetworkObject.Slot7CardNetId);
			ApplySlot7CardDataIdFromNetwork(_pendingRewardNetworkObject.Slot7CardDataId);
			ApplySlot7ColorPackedFromNetwork(_pendingRewardNetworkObject.Slot7ColorPacked);
			ApplySlot7TargetPlayerIdFromNetwork(_pendingRewardNetworkObject.Slot7TargetPlayerId);
			ApplySlot8CardNetIdFromNetwork(_pendingRewardNetworkObject.Slot8CardNetId);
			ApplySlot8CardDataIdFromNetwork(_pendingRewardNetworkObject.Slot8CardDataId);
			ApplySlot8ColorPackedFromNetwork(_pendingRewardNetworkObject.Slot8ColorPacked);
			ApplySlot8TargetPlayerIdFromNetwork(_pendingRewardNetworkObject.Slot8TargetPlayerId);
			ApplySlot9CardNetIdFromNetwork(_pendingRewardNetworkObject.Slot9CardNetId);
			ApplySlot9CardDataIdFromNetwork(_pendingRewardNetworkObject.Slot9CardDataId);
			ApplySlot9ColorPackedFromNetwork(_pendingRewardNetworkObject.Slot9ColorPacked);
			ApplySlot9TargetPlayerIdFromNetwork(_pendingRewardNetworkObject.Slot9TargetPlayerId);
			ApplySlot10CardNetIdFromNetwork(_pendingRewardNetworkObject.Slot10CardNetId);
			ApplySlot10CardDataIdFromNetwork(_pendingRewardNetworkObject.Slot10CardDataId);
			ApplySlot10ColorPackedFromNetwork(_pendingRewardNetworkObject.Slot10ColorPacked);
			ApplySlot10TargetPlayerIdFromNetwork(_pendingRewardNetworkObject.Slot10TargetPlayerId);
			ApplySlot11CardNetIdFromNetwork(_pendingRewardNetworkObject.Slot11CardNetId);
			ApplySlot11CardDataIdFromNetwork(_pendingRewardNetworkObject.Slot11CardDataId);
			ApplySlot11ColorPackedFromNetwork(_pendingRewardNetworkObject.Slot11ColorPacked);
			ApplySlot11TargetPlayerIdFromNetwork(_pendingRewardNetworkObject.Slot11TargetPlayerId);
			ApplySlot12CardNetIdFromNetwork(_pendingRewardNetworkObject.Slot12CardNetId);
			ApplySlot12CardDataIdFromNetwork(_pendingRewardNetworkObject.Slot12CardDataId);
			ApplySlot12ColorPackedFromNetwork(_pendingRewardNetworkObject.Slot12ColorPacked);
			ApplySlot12TargetPlayerIdFromNetwork(_pendingRewardNetworkObject.Slot12TargetPlayerId);
			ApplySlot13CardNetIdFromNetwork(_pendingRewardNetworkObject.Slot13CardNetId);
			ApplySlot13CardDataIdFromNetwork(_pendingRewardNetworkObject.Slot13CardDataId);
			ApplySlot13ColorPackedFromNetwork(_pendingRewardNetworkObject.Slot13ColorPacked);
			ApplySlot13TargetPlayerIdFromNetwork(_pendingRewardNetworkObject.Slot13TargetPlayerId);
			ApplySlot14CardNetIdFromNetwork(_pendingRewardNetworkObject.Slot14CardNetId);
			ApplySlot14CardDataIdFromNetwork(_pendingRewardNetworkObject.Slot14CardDataId);
			ApplySlot14ColorPackedFromNetwork(_pendingRewardNetworkObject.Slot14ColorPacked);
			ApplySlot14TargetPlayerIdFromNetwork(_pendingRewardNetworkObject.Slot14TargetPlayerId);
			ApplySlot15CardNetIdFromNetwork(_pendingRewardNetworkObject.Slot15CardNetId);
			ApplySlot15CardDataIdFromNetwork(_pendingRewardNetworkObject.Slot15CardDataId);
			ApplySlot15ColorPackedFromNetwork(_pendingRewardNetworkObject.Slot15ColorPacked);
			ApplySlot15TargetPlayerIdFromNetwork(_pendingRewardNetworkObject.Slot15TargetPlayerId);
			ApplySlot16CardNetIdFromNetwork(_pendingRewardNetworkObject.Slot16CardNetId);
			ApplySlot16CardDataIdFromNetwork(_pendingRewardNetworkObject.Slot16CardDataId);
			ApplySlot16ColorPackedFromNetwork(_pendingRewardNetworkObject.Slot16ColorPacked);
			ApplySlot16TargetPlayerIdFromNetwork(_pendingRewardNetworkObject.Slot16TargetPlayerId);
			ApplySlot17CardNetIdFromNetwork(_pendingRewardNetworkObject.Slot17CardNetId);
			ApplySlot17CardDataIdFromNetwork(_pendingRewardNetworkObject.Slot17CardDataId);
			ApplySlot17ColorPackedFromNetwork(_pendingRewardNetworkObject.Slot17ColorPacked);
			ApplySlot17TargetPlayerIdFromNetwork(_pendingRewardNetworkObject.Slot17TargetPlayerId);
			ApplySlot18CardNetIdFromNetwork(_pendingRewardNetworkObject.Slot18CardNetId);
			ApplySlot18CardDataIdFromNetwork(_pendingRewardNetworkObject.Slot18CardDataId);
			ApplySlot18ColorPackedFromNetwork(_pendingRewardNetworkObject.Slot18ColorPacked);
			ApplySlot18TargetPlayerIdFromNetwork(_pendingRewardNetworkObject.Slot18TargetPlayerId);
			ApplySlot19CardNetIdFromNetwork(_pendingRewardNetworkObject.Slot19CardNetId);
			ApplySlot19CardDataIdFromNetwork(_pendingRewardNetworkObject.Slot19CardDataId);
			ApplySlot19ColorPackedFromNetwork(_pendingRewardNetworkObject.Slot19ColorPacked);
			ApplySlot19TargetPlayerIdFromNetwork(_pendingRewardNetworkObject.Slot19TargetPlayerId);
			ApplySlot20CardNetIdFromNetwork(_pendingRewardNetworkObject.Slot20CardNetId);
			ApplySlot20CardDataIdFromNetwork(_pendingRewardNetworkObject.Slot20CardDataId);
			ApplySlot20ColorPackedFromNetwork(_pendingRewardNetworkObject.Slot20ColorPacked);
			ApplySlot20TargetPlayerIdFromNetwork(_pendingRewardNetworkObject.Slot20TargetPlayerId);
			ApplySlot21CardNetIdFromNetwork(_pendingRewardNetworkObject.Slot21CardNetId);
			ApplySlot21CardDataIdFromNetwork(_pendingRewardNetworkObject.Slot21CardDataId);
			ApplySlot21ColorPackedFromNetwork(_pendingRewardNetworkObject.Slot21ColorPacked);
			ApplySlot21TargetPlayerIdFromNetwork(_pendingRewardNetworkObject.Slot21TargetPlayerId);
			ApplySlot22CardNetIdFromNetwork(_pendingRewardNetworkObject.Slot22CardNetId);
			ApplySlot22CardDataIdFromNetwork(_pendingRewardNetworkObject.Slot22CardDataId);
			ApplySlot22ColorPackedFromNetwork(_pendingRewardNetworkObject.Slot22ColorPacked);
			ApplySlot22TargetPlayerIdFromNetwork(_pendingRewardNetworkObject.Slot22TargetPlayerId);
			ApplySlot23CardNetIdFromNetwork(_pendingRewardNetworkObject.Slot23CardNetId);
			ApplySlot23CardDataIdFromNetwork(_pendingRewardNetworkObject.Slot23CardDataId);
			ApplySlot23ColorPackedFromNetwork(_pendingRewardNetworkObject.Slot23ColorPacked);
			ApplySlot23TargetPlayerIdFromNetwork(_pendingRewardNetworkObject.Slot23TargetPlayerId);
			ApplySlot24CardNetIdFromNetwork(_pendingRewardNetworkObject.Slot24CardNetId);
			ApplySlot24CardDataIdFromNetwork(_pendingRewardNetworkObject.Slot24CardDataId);
			ApplySlot24ColorPackedFromNetwork(_pendingRewardNetworkObject.Slot24ColorPacked);
			ApplySlot24TargetPlayerIdFromNetwork(_pendingRewardNetworkObject.Slot24TargetPlayerId);
			ApplySlot25CardNetIdFromNetwork(_pendingRewardNetworkObject.Slot25CardNetId);
			ApplySlot25CardDataIdFromNetwork(_pendingRewardNetworkObject.Slot25CardDataId);
			ApplySlot25ColorPackedFromNetwork(_pendingRewardNetworkObject.Slot25ColorPacked);
			ApplySlot25TargetPlayerIdFromNetwork(_pendingRewardNetworkObject.Slot25TargetPlayerId);
			ApplySlot26CardNetIdFromNetwork(_pendingRewardNetworkObject.Slot26CardNetId);
			ApplySlot26CardDataIdFromNetwork(_pendingRewardNetworkObject.Slot26CardDataId);
			ApplySlot26ColorPackedFromNetwork(_pendingRewardNetworkObject.Slot26ColorPacked);
			ApplySlot26TargetPlayerIdFromNetwork(_pendingRewardNetworkObject.Slot26TargetPlayerId);
			ApplySlot27CardNetIdFromNetwork(_pendingRewardNetworkObject.Slot27CardNetId);
			ApplySlot27CardDataIdFromNetwork(_pendingRewardNetworkObject.Slot27CardDataId);
			ApplySlot27ColorPackedFromNetwork(_pendingRewardNetworkObject.Slot27ColorPacked);
			ApplySlot27TargetPlayerIdFromNetwork(_pendingRewardNetworkObject.Slot27TargetPlayerId);
			ApplySlot28CardNetIdFromNetwork(_pendingRewardNetworkObject.Slot28CardNetId);
			ApplySlot28CardDataIdFromNetwork(_pendingRewardNetworkObject.Slot28CardDataId);
			ApplySlot28ColorPackedFromNetwork(_pendingRewardNetworkObject.Slot28ColorPacked);
			ApplySlot28TargetPlayerIdFromNetwork(_pendingRewardNetworkObject.Slot28TargetPlayerId);
			ApplySlot29CardNetIdFromNetwork(_pendingRewardNetworkObject.Slot29CardNetId);
			ApplySlot29CardDataIdFromNetwork(_pendingRewardNetworkObject.Slot29CardDataId);
			ApplySlot29ColorPackedFromNetwork(_pendingRewardNetworkObject.Slot29ColorPacked);
			ApplySlot29TargetPlayerIdFromNetwork(_pendingRewardNetworkObject.Slot29TargetPlayerId);
			ApplySlot30CardNetIdFromNetwork(_pendingRewardNetworkObject.Slot30CardNetId);
			ApplySlot30CardDataIdFromNetwork(_pendingRewardNetworkObject.Slot30CardDataId);
			ApplySlot30ColorPackedFromNetwork(_pendingRewardNetworkObject.Slot30ColorPacked);
			ApplySlot30TargetPlayerIdFromNetwork(_pendingRewardNetworkObject.Slot30TargetPlayerId);
			ApplySlot31CardNetIdFromNetwork(_pendingRewardNetworkObject.Slot31CardNetId);
			ApplySlot31CardDataIdFromNetwork(_pendingRewardNetworkObject.Slot31CardDataId);
			ApplySlot31ColorPackedFromNetwork(_pendingRewardNetworkObject.Slot31ColorPacked);
			ApplySlot31TargetPlayerIdFromNetwork(_pendingRewardNetworkObject.Slot31TargetPlayerId);
			_pendingRewardModel.SetAuthorityProvider(() => _pendingRewardNetworkObject.HasStateAuthority);
			_pendingRewardModel.SetAttached(isAttached: true);
		}

		public void Unbind()
		{
			if (!(_pendingRewardNetworkObject == null))
			{
				_pendingRewardNetworkObject.OnNetworkedTargetLevelNumberChanged -= HandleNetworkedTargetLevelNumberChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot0CardNetIdChanged -= HandleNetworkedSlot0CardNetIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot0CardDataIdChanged -= HandleNetworkedSlot0CardDataIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot0ColorPackedChanged -= HandleNetworkedSlot0ColorPackedChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot0TargetPlayerIdChanged -= HandleNetworkedSlot0TargetPlayerIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot1CardNetIdChanged -= HandleNetworkedSlot1CardNetIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot1CardDataIdChanged -= HandleNetworkedSlot1CardDataIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot1ColorPackedChanged -= HandleNetworkedSlot1ColorPackedChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot1TargetPlayerIdChanged -= HandleNetworkedSlot1TargetPlayerIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot2CardNetIdChanged -= HandleNetworkedSlot2CardNetIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot2CardDataIdChanged -= HandleNetworkedSlot2CardDataIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot2ColorPackedChanged -= HandleNetworkedSlot2ColorPackedChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot2TargetPlayerIdChanged -= HandleNetworkedSlot2TargetPlayerIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot3CardNetIdChanged -= HandleNetworkedSlot3CardNetIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot3CardDataIdChanged -= HandleNetworkedSlot3CardDataIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot3ColorPackedChanged -= HandleNetworkedSlot3ColorPackedChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot3TargetPlayerIdChanged -= HandleNetworkedSlot3TargetPlayerIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot4CardNetIdChanged -= HandleNetworkedSlot4CardNetIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot4CardDataIdChanged -= HandleNetworkedSlot4CardDataIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot4ColorPackedChanged -= HandleNetworkedSlot4ColorPackedChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot4TargetPlayerIdChanged -= HandleNetworkedSlot4TargetPlayerIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot5CardNetIdChanged -= HandleNetworkedSlot5CardNetIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot5CardDataIdChanged -= HandleNetworkedSlot5CardDataIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot5ColorPackedChanged -= HandleNetworkedSlot5ColorPackedChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot5TargetPlayerIdChanged -= HandleNetworkedSlot5TargetPlayerIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot6CardNetIdChanged -= HandleNetworkedSlot6CardNetIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot6CardDataIdChanged -= HandleNetworkedSlot6CardDataIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot6ColorPackedChanged -= HandleNetworkedSlot6ColorPackedChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot6TargetPlayerIdChanged -= HandleNetworkedSlot6TargetPlayerIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot7CardNetIdChanged -= HandleNetworkedSlot7CardNetIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot7CardDataIdChanged -= HandleNetworkedSlot7CardDataIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot7ColorPackedChanged -= HandleNetworkedSlot7ColorPackedChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot7TargetPlayerIdChanged -= HandleNetworkedSlot7TargetPlayerIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot8CardNetIdChanged -= HandleNetworkedSlot8CardNetIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot8CardDataIdChanged -= HandleNetworkedSlot8CardDataIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot8ColorPackedChanged -= HandleNetworkedSlot8ColorPackedChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot8TargetPlayerIdChanged -= HandleNetworkedSlot8TargetPlayerIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot9CardNetIdChanged -= HandleNetworkedSlot9CardNetIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot9CardDataIdChanged -= HandleNetworkedSlot9CardDataIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot9ColorPackedChanged -= HandleNetworkedSlot9ColorPackedChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot9TargetPlayerIdChanged -= HandleNetworkedSlot9TargetPlayerIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot10CardNetIdChanged -= HandleNetworkedSlot10CardNetIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot10CardDataIdChanged -= HandleNetworkedSlot10CardDataIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot10ColorPackedChanged -= HandleNetworkedSlot10ColorPackedChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot10TargetPlayerIdChanged -= HandleNetworkedSlot10TargetPlayerIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot11CardNetIdChanged -= HandleNetworkedSlot11CardNetIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot11CardDataIdChanged -= HandleNetworkedSlot11CardDataIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot11ColorPackedChanged -= HandleNetworkedSlot11ColorPackedChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot11TargetPlayerIdChanged -= HandleNetworkedSlot11TargetPlayerIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot12CardNetIdChanged -= HandleNetworkedSlot12CardNetIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot12CardDataIdChanged -= HandleNetworkedSlot12CardDataIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot12ColorPackedChanged -= HandleNetworkedSlot12ColorPackedChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot12TargetPlayerIdChanged -= HandleNetworkedSlot12TargetPlayerIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot13CardNetIdChanged -= HandleNetworkedSlot13CardNetIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot13CardDataIdChanged -= HandleNetworkedSlot13CardDataIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot13ColorPackedChanged -= HandleNetworkedSlot13ColorPackedChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot13TargetPlayerIdChanged -= HandleNetworkedSlot13TargetPlayerIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot14CardNetIdChanged -= HandleNetworkedSlot14CardNetIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot14CardDataIdChanged -= HandleNetworkedSlot14CardDataIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot14ColorPackedChanged -= HandleNetworkedSlot14ColorPackedChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot14TargetPlayerIdChanged -= HandleNetworkedSlot14TargetPlayerIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot15CardNetIdChanged -= HandleNetworkedSlot15CardNetIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot15CardDataIdChanged -= HandleNetworkedSlot15CardDataIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot15ColorPackedChanged -= HandleNetworkedSlot15ColorPackedChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot15TargetPlayerIdChanged -= HandleNetworkedSlot15TargetPlayerIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot16CardNetIdChanged -= HandleNetworkedSlot16CardNetIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot16CardDataIdChanged -= HandleNetworkedSlot16CardDataIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot16ColorPackedChanged -= HandleNetworkedSlot16ColorPackedChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot16TargetPlayerIdChanged -= HandleNetworkedSlot16TargetPlayerIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot17CardNetIdChanged -= HandleNetworkedSlot17CardNetIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot17CardDataIdChanged -= HandleNetworkedSlot17CardDataIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot17ColorPackedChanged -= HandleNetworkedSlot17ColorPackedChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot17TargetPlayerIdChanged -= HandleNetworkedSlot17TargetPlayerIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot18CardNetIdChanged -= HandleNetworkedSlot18CardNetIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot18CardDataIdChanged -= HandleNetworkedSlot18CardDataIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot18ColorPackedChanged -= HandleNetworkedSlot18ColorPackedChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot18TargetPlayerIdChanged -= HandleNetworkedSlot18TargetPlayerIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot19CardNetIdChanged -= HandleNetworkedSlot19CardNetIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot19CardDataIdChanged -= HandleNetworkedSlot19CardDataIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot19ColorPackedChanged -= HandleNetworkedSlot19ColorPackedChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot19TargetPlayerIdChanged -= HandleNetworkedSlot19TargetPlayerIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot20CardNetIdChanged -= HandleNetworkedSlot20CardNetIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot20CardDataIdChanged -= HandleNetworkedSlot20CardDataIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot20ColorPackedChanged -= HandleNetworkedSlot20ColorPackedChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot20TargetPlayerIdChanged -= HandleNetworkedSlot20TargetPlayerIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot21CardNetIdChanged -= HandleNetworkedSlot21CardNetIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot21CardDataIdChanged -= HandleNetworkedSlot21CardDataIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot21ColorPackedChanged -= HandleNetworkedSlot21ColorPackedChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot21TargetPlayerIdChanged -= HandleNetworkedSlot21TargetPlayerIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot22CardNetIdChanged -= HandleNetworkedSlot22CardNetIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot22CardDataIdChanged -= HandleNetworkedSlot22CardDataIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot22ColorPackedChanged -= HandleNetworkedSlot22ColorPackedChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot22TargetPlayerIdChanged -= HandleNetworkedSlot22TargetPlayerIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot23CardNetIdChanged -= HandleNetworkedSlot23CardNetIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot23CardDataIdChanged -= HandleNetworkedSlot23CardDataIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot23ColorPackedChanged -= HandleNetworkedSlot23ColorPackedChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot23TargetPlayerIdChanged -= HandleNetworkedSlot23TargetPlayerIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot24CardNetIdChanged -= HandleNetworkedSlot24CardNetIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot24CardDataIdChanged -= HandleNetworkedSlot24CardDataIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot24ColorPackedChanged -= HandleNetworkedSlot24ColorPackedChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot24TargetPlayerIdChanged -= HandleNetworkedSlot24TargetPlayerIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot25CardNetIdChanged -= HandleNetworkedSlot25CardNetIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot25CardDataIdChanged -= HandleNetworkedSlot25CardDataIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot25ColorPackedChanged -= HandleNetworkedSlot25ColorPackedChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot25TargetPlayerIdChanged -= HandleNetworkedSlot25TargetPlayerIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot26CardNetIdChanged -= HandleNetworkedSlot26CardNetIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot26CardDataIdChanged -= HandleNetworkedSlot26CardDataIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot26ColorPackedChanged -= HandleNetworkedSlot26ColorPackedChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot26TargetPlayerIdChanged -= HandleNetworkedSlot26TargetPlayerIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot27CardNetIdChanged -= HandleNetworkedSlot27CardNetIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot27CardDataIdChanged -= HandleNetworkedSlot27CardDataIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot27ColorPackedChanged -= HandleNetworkedSlot27ColorPackedChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot27TargetPlayerIdChanged -= HandleNetworkedSlot27TargetPlayerIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot28CardNetIdChanged -= HandleNetworkedSlot28CardNetIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot28CardDataIdChanged -= HandleNetworkedSlot28CardDataIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot28ColorPackedChanged -= HandleNetworkedSlot28ColorPackedChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot28TargetPlayerIdChanged -= HandleNetworkedSlot28TargetPlayerIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot29CardNetIdChanged -= HandleNetworkedSlot29CardNetIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot29CardDataIdChanged -= HandleNetworkedSlot29CardDataIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot29ColorPackedChanged -= HandleNetworkedSlot29ColorPackedChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot29TargetPlayerIdChanged -= HandleNetworkedSlot29TargetPlayerIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot30CardNetIdChanged -= HandleNetworkedSlot30CardNetIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot30CardDataIdChanged -= HandleNetworkedSlot30CardDataIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot30ColorPackedChanged -= HandleNetworkedSlot30ColorPackedChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot30TargetPlayerIdChanged -= HandleNetworkedSlot30TargetPlayerIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot31CardNetIdChanged -= HandleNetworkedSlot31CardNetIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot31CardDataIdChanged -= HandleNetworkedSlot31CardDataIdChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot31ColorPackedChanged -= HandleNetworkedSlot31ColorPackedChanged;
				_pendingRewardNetworkObject.OnNetworkedSlot31TargetPlayerIdChanged -= HandleNetworkedSlot31TargetPlayerIdChanged;
				_pendingRewardNetworkObject.OnAuthoritativeTick -= HandleAuthoritativeTick;
				_pendingRewardNetworkObject.OnDespawned -= HandleDespawned;
				_pendingRewardModel.TargetLevelNumber.BindWriter(null);
				_pendingRewardModel.Slot0CardNetId.BindWriter(null);
				_pendingRewardModel.Slot0CardDataId.BindWriter(null);
				_pendingRewardModel.Slot0ColorPacked.BindWriter(null);
				_pendingRewardModel.Slot0TargetPlayerId.BindWriter(null);
				_pendingRewardModel.Slot1CardNetId.BindWriter(null);
				_pendingRewardModel.Slot1CardDataId.BindWriter(null);
				_pendingRewardModel.Slot1ColorPacked.BindWriter(null);
				_pendingRewardModel.Slot1TargetPlayerId.BindWriter(null);
				_pendingRewardModel.Slot2CardNetId.BindWriter(null);
				_pendingRewardModel.Slot2CardDataId.BindWriter(null);
				_pendingRewardModel.Slot2ColorPacked.BindWriter(null);
				_pendingRewardModel.Slot2TargetPlayerId.BindWriter(null);
				_pendingRewardModel.Slot3CardNetId.BindWriter(null);
				_pendingRewardModel.Slot3CardDataId.BindWriter(null);
				_pendingRewardModel.Slot3ColorPacked.BindWriter(null);
				_pendingRewardModel.Slot3TargetPlayerId.BindWriter(null);
				_pendingRewardModel.Slot4CardNetId.BindWriter(null);
				_pendingRewardModel.Slot4CardDataId.BindWriter(null);
				_pendingRewardModel.Slot4ColorPacked.BindWriter(null);
				_pendingRewardModel.Slot4TargetPlayerId.BindWriter(null);
				_pendingRewardModel.Slot5CardNetId.BindWriter(null);
				_pendingRewardModel.Slot5CardDataId.BindWriter(null);
				_pendingRewardModel.Slot5ColorPacked.BindWriter(null);
				_pendingRewardModel.Slot5TargetPlayerId.BindWriter(null);
				_pendingRewardModel.Slot6CardNetId.BindWriter(null);
				_pendingRewardModel.Slot6CardDataId.BindWriter(null);
				_pendingRewardModel.Slot6ColorPacked.BindWriter(null);
				_pendingRewardModel.Slot6TargetPlayerId.BindWriter(null);
				_pendingRewardModel.Slot7CardNetId.BindWriter(null);
				_pendingRewardModel.Slot7CardDataId.BindWriter(null);
				_pendingRewardModel.Slot7ColorPacked.BindWriter(null);
				_pendingRewardModel.Slot7TargetPlayerId.BindWriter(null);
				_pendingRewardModel.Slot8CardNetId.BindWriter(null);
				_pendingRewardModel.Slot8CardDataId.BindWriter(null);
				_pendingRewardModel.Slot8ColorPacked.BindWriter(null);
				_pendingRewardModel.Slot8TargetPlayerId.BindWriter(null);
				_pendingRewardModel.Slot9CardNetId.BindWriter(null);
				_pendingRewardModel.Slot9CardDataId.BindWriter(null);
				_pendingRewardModel.Slot9ColorPacked.BindWriter(null);
				_pendingRewardModel.Slot9TargetPlayerId.BindWriter(null);
				_pendingRewardModel.Slot10CardNetId.BindWriter(null);
				_pendingRewardModel.Slot10CardDataId.BindWriter(null);
				_pendingRewardModel.Slot10ColorPacked.BindWriter(null);
				_pendingRewardModel.Slot10TargetPlayerId.BindWriter(null);
				_pendingRewardModel.Slot11CardNetId.BindWriter(null);
				_pendingRewardModel.Slot11CardDataId.BindWriter(null);
				_pendingRewardModel.Slot11ColorPacked.BindWriter(null);
				_pendingRewardModel.Slot11TargetPlayerId.BindWriter(null);
				_pendingRewardModel.Slot12CardNetId.BindWriter(null);
				_pendingRewardModel.Slot12CardDataId.BindWriter(null);
				_pendingRewardModel.Slot12ColorPacked.BindWriter(null);
				_pendingRewardModel.Slot12TargetPlayerId.BindWriter(null);
				_pendingRewardModel.Slot13CardNetId.BindWriter(null);
				_pendingRewardModel.Slot13CardDataId.BindWriter(null);
				_pendingRewardModel.Slot13ColorPacked.BindWriter(null);
				_pendingRewardModel.Slot13TargetPlayerId.BindWriter(null);
				_pendingRewardModel.Slot14CardNetId.BindWriter(null);
				_pendingRewardModel.Slot14CardDataId.BindWriter(null);
				_pendingRewardModel.Slot14ColorPacked.BindWriter(null);
				_pendingRewardModel.Slot14TargetPlayerId.BindWriter(null);
				_pendingRewardModel.Slot15CardNetId.BindWriter(null);
				_pendingRewardModel.Slot15CardDataId.BindWriter(null);
				_pendingRewardModel.Slot15ColorPacked.BindWriter(null);
				_pendingRewardModel.Slot15TargetPlayerId.BindWriter(null);
				_pendingRewardModel.Slot16CardNetId.BindWriter(null);
				_pendingRewardModel.Slot16CardDataId.BindWriter(null);
				_pendingRewardModel.Slot16ColorPacked.BindWriter(null);
				_pendingRewardModel.Slot16TargetPlayerId.BindWriter(null);
				_pendingRewardModel.Slot17CardNetId.BindWriter(null);
				_pendingRewardModel.Slot17CardDataId.BindWriter(null);
				_pendingRewardModel.Slot17ColorPacked.BindWriter(null);
				_pendingRewardModel.Slot17TargetPlayerId.BindWriter(null);
				_pendingRewardModel.Slot18CardNetId.BindWriter(null);
				_pendingRewardModel.Slot18CardDataId.BindWriter(null);
				_pendingRewardModel.Slot18ColorPacked.BindWriter(null);
				_pendingRewardModel.Slot18TargetPlayerId.BindWriter(null);
				_pendingRewardModel.Slot19CardNetId.BindWriter(null);
				_pendingRewardModel.Slot19CardDataId.BindWriter(null);
				_pendingRewardModel.Slot19ColorPacked.BindWriter(null);
				_pendingRewardModel.Slot19TargetPlayerId.BindWriter(null);
				_pendingRewardModel.Slot20CardNetId.BindWriter(null);
				_pendingRewardModel.Slot20CardDataId.BindWriter(null);
				_pendingRewardModel.Slot20ColorPacked.BindWriter(null);
				_pendingRewardModel.Slot20TargetPlayerId.BindWriter(null);
				_pendingRewardModel.Slot21CardNetId.BindWriter(null);
				_pendingRewardModel.Slot21CardDataId.BindWriter(null);
				_pendingRewardModel.Slot21ColorPacked.BindWriter(null);
				_pendingRewardModel.Slot21TargetPlayerId.BindWriter(null);
				_pendingRewardModel.Slot22CardNetId.BindWriter(null);
				_pendingRewardModel.Slot22CardDataId.BindWriter(null);
				_pendingRewardModel.Slot22ColorPacked.BindWriter(null);
				_pendingRewardModel.Slot22TargetPlayerId.BindWriter(null);
				_pendingRewardModel.Slot23CardNetId.BindWriter(null);
				_pendingRewardModel.Slot23CardDataId.BindWriter(null);
				_pendingRewardModel.Slot23ColorPacked.BindWriter(null);
				_pendingRewardModel.Slot23TargetPlayerId.BindWriter(null);
				_pendingRewardModel.Slot24CardNetId.BindWriter(null);
				_pendingRewardModel.Slot24CardDataId.BindWriter(null);
				_pendingRewardModel.Slot24ColorPacked.BindWriter(null);
				_pendingRewardModel.Slot24TargetPlayerId.BindWriter(null);
				_pendingRewardModel.Slot25CardNetId.BindWriter(null);
				_pendingRewardModel.Slot25CardDataId.BindWriter(null);
				_pendingRewardModel.Slot25ColorPacked.BindWriter(null);
				_pendingRewardModel.Slot25TargetPlayerId.BindWriter(null);
				_pendingRewardModel.Slot26CardNetId.BindWriter(null);
				_pendingRewardModel.Slot26CardDataId.BindWriter(null);
				_pendingRewardModel.Slot26ColorPacked.BindWriter(null);
				_pendingRewardModel.Slot26TargetPlayerId.BindWriter(null);
				_pendingRewardModel.Slot27CardNetId.BindWriter(null);
				_pendingRewardModel.Slot27CardDataId.BindWriter(null);
				_pendingRewardModel.Slot27ColorPacked.BindWriter(null);
				_pendingRewardModel.Slot27TargetPlayerId.BindWriter(null);
				_pendingRewardModel.Slot28CardNetId.BindWriter(null);
				_pendingRewardModel.Slot28CardDataId.BindWriter(null);
				_pendingRewardModel.Slot28ColorPacked.BindWriter(null);
				_pendingRewardModel.Slot28TargetPlayerId.BindWriter(null);
				_pendingRewardModel.Slot29CardNetId.BindWriter(null);
				_pendingRewardModel.Slot29CardDataId.BindWriter(null);
				_pendingRewardModel.Slot29ColorPacked.BindWriter(null);
				_pendingRewardModel.Slot29TargetPlayerId.BindWriter(null);
				_pendingRewardModel.Slot30CardNetId.BindWriter(null);
				_pendingRewardModel.Slot30CardDataId.BindWriter(null);
				_pendingRewardModel.Slot30ColorPacked.BindWriter(null);
				_pendingRewardModel.Slot30TargetPlayerId.BindWriter(null);
				_pendingRewardModel.Slot31CardNetId.BindWriter(null);
				_pendingRewardModel.Slot31CardDataId.BindWriter(null);
				_pendingRewardModel.Slot31ColorPacked.BindWriter(null);
				_pendingRewardModel.Slot31TargetPlayerId.BindWriter(null);
				_pendingRewardNetworkObject = null;
				_hasPendingTargetLevelNumber = false;
				_hasPendingSlot0CardNetId = false;
				_hasPendingSlot0CardDataId = false;
				_hasPendingSlot0ColorPacked = false;
				_hasPendingSlot0TargetPlayerId = false;
				_hasPendingSlot1CardNetId = false;
				_hasPendingSlot1CardDataId = false;
				_hasPendingSlot1ColorPacked = false;
				_hasPendingSlot1TargetPlayerId = false;
				_hasPendingSlot2CardNetId = false;
				_hasPendingSlot2CardDataId = false;
				_hasPendingSlot2ColorPacked = false;
				_hasPendingSlot2TargetPlayerId = false;
				_hasPendingSlot3CardNetId = false;
				_hasPendingSlot3CardDataId = false;
				_hasPendingSlot3ColorPacked = false;
				_hasPendingSlot3TargetPlayerId = false;
				_hasPendingSlot4CardNetId = false;
				_hasPendingSlot4CardDataId = false;
				_hasPendingSlot4ColorPacked = false;
				_hasPendingSlot4TargetPlayerId = false;
				_hasPendingSlot5CardNetId = false;
				_hasPendingSlot5CardDataId = false;
				_hasPendingSlot5ColorPacked = false;
				_hasPendingSlot5TargetPlayerId = false;
				_hasPendingSlot6CardNetId = false;
				_hasPendingSlot6CardDataId = false;
				_hasPendingSlot6ColorPacked = false;
				_hasPendingSlot6TargetPlayerId = false;
				_hasPendingSlot7CardNetId = false;
				_hasPendingSlot7CardDataId = false;
				_hasPendingSlot7ColorPacked = false;
				_hasPendingSlot7TargetPlayerId = false;
				_hasPendingSlot8CardNetId = false;
				_hasPendingSlot8CardDataId = false;
				_hasPendingSlot8ColorPacked = false;
				_hasPendingSlot8TargetPlayerId = false;
				_hasPendingSlot9CardNetId = false;
				_hasPendingSlot9CardDataId = false;
				_hasPendingSlot9ColorPacked = false;
				_hasPendingSlot9TargetPlayerId = false;
				_hasPendingSlot10CardNetId = false;
				_hasPendingSlot10CardDataId = false;
				_hasPendingSlot10ColorPacked = false;
				_hasPendingSlot10TargetPlayerId = false;
				_hasPendingSlot11CardNetId = false;
				_hasPendingSlot11CardDataId = false;
				_hasPendingSlot11ColorPacked = false;
				_hasPendingSlot11TargetPlayerId = false;
				_hasPendingSlot12CardNetId = false;
				_hasPendingSlot12CardDataId = false;
				_hasPendingSlot12ColorPacked = false;
				_hasPendingSlot12TargetPlayerId = false;
				_hasPendingSlot13CardNetId = false;
				_hasPendingSlot13CardDataId = false;
				_hasPendingSlot13ColorPacked = false;
				_hasPendingSlot13TargetPlayerId = false;
				_hasPendingSlot14CardNetId = false;
				_hasPendingSlot14CardDataId = false;
				_hasPendingSlot14ColorPacked = false;
				_hasPendingSlot14TargetPlayerId = false;
				_hasPendingSlot15CardNetId = false;
				_hasPendingSlot15CardDataId = false;
				_hasPendingSlot15ColorPacked = false;
				_hasPendingSlot15TargetPlayerId = false;
				_hasPendingSlot16CardNetId = false;
				_hasPendingSlot16CardDataId = false;
				_hasPendingSlot16ColorPacked = false;
				_hasPendingSlot16TargetPlayerId = false;
				_hasPendingSlot17CardNetId = false;
				_hasPendingSlot17CardDataId = false;
				_hasPendingSlot17ColorPacked = false;
				_hasPendingSlot17TargetPlayerId = false;
				_hasPendingSlot18CardNetId = false;
				_hasPendingSlot18CardDataId = false;
				_hasPendingSlot18ColorPacked = false;
				_hasPendingSlot18TargetPlayerId = false;
				_hasPendingSlot19CardNetId = false;
				_hasPendingSlot19CardDataId = false;
				_hasPendingSlot19ColorPacked = false;
				_hasPendingSlot19TargetPlayerId = false;
				_hasPendingSlot20CardNetId = false;
				_hasPendingSlot20CardDataId = false;
				_hasPendingSlot20ColorPacked = false;
				_hasPendingSlot20TargetPlayerId = false;
				_hasPendingSlot21CardNetId = false;
				_hasPendingSlot21CardDataId = false;
				_hasPendingSlot21ColorPacked = false;
				_hasPendingSlot21TargetPlayerId = false;
				_hasPendingSlot22CardNetId = false;
				_hasPendingSlot22CardDataId = false;
				_hasPendingSlot22ColorPacked = false;
				_hasPendingSlot22TargetPlayerId = false;
				_hasPendingSlot23CardNetId = false;
				_hasPendingSlot23CardDataId = false;
				_hasPendingSlot23ColorPacked = false;
				_hasPendingSlot23TargetPlayerId = false;
				_hasPendingSlot24CardNetId = false;
				_hasPendingSlot24CardDataId = false;
				_hasPendingSlot24ColorPacked = false;
				_hasPendingSlot24TargetPlayerId = false;
				_hasPendingSlot25CardNetId = false;
				_hasPendingSlot25CardDataId = false;
				_hasPendingSlot25ColorPacked = false;
				_hasPendingSlot25TargetPlayerId = false;
				_hasPendingSlot26CardNetId = false;
				_hasPendingSlot26CardDataId = false;
				_hasPendingSlot26ColorPacked = false;
				_hasPendingSlot26TargetPlayerId = false;
				_hasPendingSlot27CardNetId = false;
				_hasPendingSlot27CardDataId = false;
				_hasPendingSlot27ColorPacked = false;
				_hasPendingSlot27TargetPlayerId = false;
				_hasPendingSlot28CardNetId = false;
				_hasPendingSlot28CardDataId = false;
				_hasPendingSlot28ColorPacked = false;
				_hasPendingSlot28TargetPlayerId = false;
				_hasPendingSlot29CardNetId = false;
				_hasPendingSlot29CardDataId = false;
				_hasPendingSlot29ColorPacked = false;
				_hasPendingSlot29TargetPlayerId = false;
				_hasPendingSlot30CardNetId = false;
				_hasPendingSlot30CardDataId = false;
				_hasPendingSlot30ColorPacked = false;
				_hasPendingSlot30TargetPlayerId = false;
				_hasPendingSlot31CardNetId = false;
				_hasPendingSlot31CardDataId = false;
				_hasPendingSlot31ColorPacked = false;
				_hasPendingSlot31TargetPlayerId = false;
				_pendingRewardModel.SetAuthorityProvider(null);
				_pendingRewardModel.SetAttached(isAttached: false);
			}
		}

		public void Dispose()
		{
			Unbind();
		}

		public void BindObject(NetworkObject networkObject)
		{
			Bind(networkObject.GetComponent<PendingRewardNetworkObject>());
		}

		private void HandleDespawned()
		{
			Unbind();
		}

		private void HandleNetworkedTargetLevelNumberChanged(int targetLevelNumber)
		{
			ApplyTargetLevelNumberFromNetwork(targetLevelNumber);
		}

		private void HandleNetworkedSlot0CardNetIdChanged(int slot0CardNetId)
		{
			ApplySlot0CardNetIdFromNetwork(slot0CardNetId);
		}

		private void HandleNetworkedSlot0CardDataIdChanged(int slot0CardDataId)
		{
			ApplySlot0CardDataIdFromNetwork(slot0CardDataId);
		}

		private void HandleNetworkedSlot0ColorPackedChanged(int slot0ColorPacked)
		{
			ApplySlot0ColorPackedFromNetwork(slot0ColorPacked);
		}

		private void HandleNetworkedSlot0TargetPlayerIdChanged(int slot0TargetPlayerId)
		{
			ApplySlot0TargetPlayerIdFromNetwork(slot0TargetPlayerId);
		}

		private void HandleNetworkedSlot1CardNetIdChanged(int slot1CardNetId)
		{
			ApplySlot1CardNetIdFromNetwork(slot1CardNetId);
		}

		private void HandleNetworkedSlot1CardDataIdChanged(int slot1CardDataId)
		{
			ApplySlot1CardDataIdFromNetwork(slot1CardDataId);
		}

		private void HandleNetworkedSlot1ColorPackedChanged(int slot1ColorPacked)
		{
			ApplySlot1ColorPackedFromNetwork(slot1ColorPacked);
		}

		private void HandleNetworkedSlot1TargetPlayerIdChanged(int slot1TargetPlayerId)
		{
			ApplySlot1TargetPlayerIdFromNetwork(slot1TargetPlayerId);
		}

		private void HandleNetworkedSlot2CardNetIdChanged(int slot2CardNetId)
		{
			ApplySlot2CardNetIdFromNetwork(slot2CardNetId);
		}

		private void HandleNetworkedSlot2CardDataIdChanged(int slot2CardDataId)
		{
			ApplySlot2CardDataIdFromNetwork(slot2CardDataId);
		}

		private void HandleNetworkedSlot2ColorPackedChanged(int slot2ColorPacked)
		{
			ApplySlot2ColorPackedFromNetwork(slot2ColorPacked);
		}

		private void HandleNetworkedSlot2TargetPlayerIdChanged(int slot2TargetPlayerId)
		{
			ApplySlot2TargetPlayerIdFromNetwork(slot2TargetPlayerId);
		}

		private void HandleNetworkedSlot3CardNetIdChanged(int slot3CardNetId)
		{
			ApplySlot3CardNetIdFromNetwork(slot3CardNetId);
		}

		private void HandleNetworkedSlot3CardDataIdChanged(int slot3CardDataId)
		{
			ApplySlot3CardDataIdFromNetwork(slot3CardDataId);
		}

		private void HandleNetworkedSlot3ColorPackedChanged(int slot3ColorPacked)
		{
			ApplySlot3ColorPackedFromNetwork(slot3ColorPacked);
		}

		private void HandleNetworkedSlot3TargetPlayerIdChanged(int slot3TargetPlayerId)
		{
			ApplySlot3TargetPlayerIdFromNetwork(slot3TargetPlayerId);
		}

		private void HandleNetworkedSlot4CardNetIdChanged(int slot4CardNetId)
		{
			ApplySlot4CardNetIdFromNetwork(slot4CardNetId);
		}

		private void HandleNetworkedSlot4CardDataIdChanged(int slot4CardDataId)
		{
			ApplySlot4CardDataIdFromNetwork(slot4CardDataId);
		}

		private void HandleNetworkedSlot4ColorPackedChanged(int slot4ColorPacked)
		{
			ApplySlot4ColorPackedFromNetwork(slot4ColorPacked);
		}

		private void HandleNetworkedSlot4TargetPlayerIdChanged(int slot4TargetPlayerId)
		{
			ApplySlot4TargetPlayerIdFromNetwork(slot4TargetPlayerId);
		}

		private void HandleNetworkedSlot5CardNetIdChanged(int slot5CardNetId)
		{
			ApplySlot5CardNetIdFromNetwork(slot5CardNetId);
		}

		private void HandleNetworkedSlot5CardDataIdChanged(int slot5CardDataId)
		{
			ApplySlot5CardDataIdFromNetwork(slot5CardDataId);
		}

		private void HandleNetworkedSlot5ColorPackedChanged(int slot5ColorPacked)
		{
			ApplySlot5ColorPackedFromNetwork(slot5ColorPacked);
		}

		private void HandleNetworkedSlot5TargetPlayerIdChanged(int slot5TargetPlayerId)
		{
			ApplySlot5TargetPlayerIdFromNetwork(slot5TargetPlayerId);
		}

		private void HandleNetworkedSlot6CardNetIdChanged(int slot6CardNetId)
		{
			ApplySlot6CardNetIdFromNetwork(slot6CardNetId);
		}

		private void HandleNetworkedSlot6CardDataIdChanged(int slot6CardDataId)
		{
			ApplySlot6CardDataIdFromNetwork(slot6CardDataId);
		}

		private void HandleNetworkedSlot6ColorPackedChanged(int slot6ColorPacked)
		{
			ApplySlot6ColorPackedFromNetwork(slot6ColorPacked);
		}

		private void HandleNetworkedSlot6TargetPlayerIdChanged(int slot6TargetPlayerId)
		{
			ApplySlot6TargetPlayerIdFromNetwork(slot6TargetPlayerId);
		}

		private void HandleNetworkedSlot7CardNetIdChanged(int slot7CardNetId)
		{
			ApplySlot7CardNetIdFromNetwork(slot7CardNetId);
		}

		private void HandleNetworkedSlot7CardDataIdChanged(int slot7CardDataId)
		{
			ApplySlot7CardDataIdFromNetwork(slot7CardDataId);
		}

		private void HandleNetworkedSlot7ColorPackedChanged(int slot7ColorPacked)
		{
			ApplySlot7ColorPackedFromNetwork(slot7ColorPacked);
		}

		private void HandleNetworkedSlot7TargetPlayerIdChanged(int slot7TargetPlayerId)
		{
			ApplySlot7TargetPlayerIdFromNetwork(slot7TargetPlayerId);
		}

		private void HandleNetworkedSlot8CardNetIdChanged(int slot8CardNetId)
		{
			ApplySlot8CardNetIdFromNetwork(slot8CardNetId);
		}

		private void HandleNetworkedSlot8CardDataIdChanged(int slot8CardDataId)
		{
			ApplySlot8CardDataIdFromNetwork(slot8CardDataId);
		}

		private void HandleNetworkedSlot8ColorPackedChanged(int slot8ColorPacked)
		{
			ApplySlot8ColorPackedFromNetwork(slot8ColorPacked);
		}

		private void HandleNetworkedSlot8TargetPlayerIdChanged(int slot8TargetPlayerId)
		{
			ApplySlot8TargetPlayerIdFromNetwork(slot8TargetPlayerId);
		}

		private void HandleNetworkedSlot9CardNetIdChanged(int slot9CardNetId)
		{
			ApplySlot9CardNetIdFromNetwork(slot9CardNetId);
		}

		private void HandleNetworkedSlot9CardDataIdChanged(int slot9CardDataId)
		{
			ApplySlot9CardDataIdFromNetwork(slot9CardDataId);
		}

		private void HandleNetworkedSlot9ColorPackedChanged(int slot9ColorPacked)
		{
			ApplySlot9ColorPackedFromNetwork(slot9ColorPacked);
		}

		private void HandleNetworkedSlot9TargetPlayerIdChanged(int slot9TargetPlayerId)
		{
			ApplySlot9TargetPlayerIdFromNetwork(slot9TargetPlayerId);
		}

		private void HandleNetworkedSlot10CardNetIdChanged(int slot10CardNetId)
		{
			ApplySlot10CardNetIdFromNetwork(slot10CardNetId);
		}

		private void HandleNetworkedSlot10CardDataIdChanged(int slot10CardDataId)
		{
			ApplySlot10CardDataIdFromNetwork(slot10CardDataId);
		}

		private void HandleNetworkedSlot10ColorPackedChanged(int slot10ColorPacked)
		{
			ApplySlot10ColorPackedFromNetwork(slot10ColorPacked);
		}

		private void HandleNetworkedSlot10TargetPlayerIdChanged(int slot10TargetPlayerId)
		{
			ApplySlot10TargetPlayerIdFromNetwork(slot10TargetPlayerId);
		}

		private void HandleNetworkedSlot11CardNetIdChanged(int slot11CardNetId)
		{
			ApplySlot11CardNetIdFromNetwork(slot11CardNetId);
		}

		private void HandleNetworkedSlot11CardDataIdChanged(int slot11CardDataId)
		{
			ApplySlot11CardDataIdFromNetwork(slot11CardDataId);
		}

		private void HandleNetworkedSlot11ColorPackedChanged(int slot11ColorPacked)
		{
			ApplySlot11ColorPackedFromNetwork(slot11ColorPacked);
		}

		private void HandleNetworkedSlot11TargetPlayerIdChanged(int slot11TargetPlayerId)
		{
			ApplySlot11TargetPlayerIdFromNetwork(slot11TargetPlayerId);
		}

		private void HandleNetworkedSlot12CardNetIdChanged(int slot12CardNetId)
		{
			ApplySlot12CardNetIdFromNetwork(slot12CardNetId);
		}

		private void HandleNetworkedSlot12CardDataIdChanged(int slot12CardDataId)
		{
			ApplySlot12CardDataIdFromNetwork(slot12CardDataId);
		}

		private void HandleNetworkedSlot12ColorPackedChanged(int slot12ColorPacked)
		{
			ApplySlot12ColorPackedFromNetwork(slot12ColorPacked);
		}

		private void HandleNetworkedSlot12TargetPlayerIdChanged(int slot12TargetPlayerId)
		{
			ApplySlot12TargetPlayerIdFromNetwork(slot12TargetPlayerId);
		}

		private void HandleNetworkedSlot13CardNetIdChanged(int slot13CardNetId)
		{
			ApplySlot13CardNetIdFromNetwork(slot13CardNetId);
		}

		private void HandleNetworkedSlot13CardDataIdChanged(int slot13CardDataId)
		{
			ApplySlot13CardDataIdFromNetwork(slot13CardDataId);
		}

		private void HandleNetworkedSlot13ColorPackedChanged(int slot13ColorPacked)
		{
			ApplySlot13ColorPackedFromNetwork(slot13ColorPacked);
		}

		private void HandleNetworkedSlot13TargetPlayerIdChanged(int slot13TargetPlayerId)
		{
			ApplySlot13TargetPlayerIdFromNetwork(slot13TargetPlayerId);
		}

		private void HandleNetworkedSlot14CardNetIdChanged(int slot14CardNetId)
		{
			ApplySlot14CardNetIdFromNetwork(slot14CardNetId);
		}

		private void HandleNetworkedSlot14CardDataIdChanged(int slot14CardDataId)
		{
			ApplySlot14CardDataIdFromNetwork(slot14CardDataId);
		}

		private void HandleNetworkedSlot14ColorPackedChanged(int slot14ColorPacked)
		{
			ApplySlot14ColorPackedFromNetwork(slot14ColorPacked);
		}

		private void HandleNetworkedSlot14TargetPlayerIdChanged(int slot14TargetPlayerId)
		{
			ApplySlot14TargetPlayerIdFromNetwork(slot14TargetPlayerId);
		}

		private void HandleNetworkedSlot15CardNetIdChanged(int slot15CardNetId)
		{
			ApplySlot15CardNetIdFromNetwork(slot15CardNetId);
		}

		private void HandleNetworkedSlot15CardDataIdChanged(int slot15CardDataId)
		{
			ApplySlot15CardDataIdFromNetwork(slot15CardDataId);
		}

		private void HandleNetworkedSlot15ColorPackedChanged(int slot15ColorPacked)
		{
			ApplySlot15ColorPackedFromNetwork(slot15ColorPacked);
		}

		private void HandleNetworkedSlot15TargetPlayerIdChanged(int slot15TargetPlayerId)
		{
			ApplySlot15TargetPlayerIdFromNetwork(slot15TargetPlayerId);
		}

		private void HandleNetworkedSlot16CardNetIdChanged(int slot16CardNetId)
		{
			ApplySlot16CardNetIdFromNetwork(slot16CardNetId);
		}

		private void HandleNetworkedSlot16CardDataIdChanged(int slot16CardDataId)
		{
			ApplySlot16CardDataIdFromNetwork(slot16CardDataId);
		}

		private void HandleNetworkedSlot16ColorPackedChanged(int slot16ColorPacked)
		{
			ApplySlot16ColorPackedFromNetwork(slot16ColorPacked);
		}

		private void HandleNetworkedSlot16TargetPlayerIdChanged(int slot16TargetPlayerId)
		{
			ApplySlot16TargetPlayerIdFromNetwork(slot16TargetPlayerId);
		}

		private void HandleNetworkedSlot17CardNetIdChanged(int slot17CardNetId)
		{
			ApplySlot17CardNetIdFromNetwork(slot17CardNetId);
		}

		private void HandleNetworkedSlot17CardDataIdChanged(int slot17CardDataId)
		{
			ApplySlot17CardDataIdFromNetwork(slot17CardDataId);
		}

		private void HandleNetworkedSlot17ColorPackedChanged(int slot17ColorPacked)
		{
			ApplySlot17ColorPackedFromNetwork(slot17ColorPacked);
		}

		private void HandleNetworkedSlot17TargetPlayerIdChanged(int slot17TargetPlayerId)
		{
			ApplySlot17TargetPlayerIdFromNetwork(slot17TargetPlayerId);
		}

		private void HandleNetworkedSlot18CardNetIdChanged(int slot18CardNetId)
		{
			ApplySlot18CardNetIdFromNetwork(slot18CardNetId);
		}

		private void HandleNetworkedSlot18CardDataIdChanged(int slot18CardDataId)
		{
			ApplySlot18CardDataIdFromNetwork(slot18CardDataId);
		}

		private void HandleNetworkedSlot18ColorPackedChanged(int slot18ColorPacked)
		{
			ApplySlot18ColorPackedFromNetwork(slot18ColorPacked);
		}

		private void HandleNetworkedSlot18TargetPlayerIdChanged(int slot18TargetPlayerId)
		{
			ApplySlot18TargetPlayerIdFromNetwork(slot18TargetPlayerId);
		}

		private void HandleNetworkedSlot19CardNetIdChanged(int slot19CardNetId)
		{
			ApplySlot19CardNetIdFromNetwork(slot19CardNetId);
		}

		private void HandleNetworkedSlot19CardDataIdChanged(int slot19CardDataId)
		{
			ApplySlot19CardDataIdFromNetwork(slot19CardDataId);
		}

		private void HandleNetworkedSlot19ColorPackedChanged(int slot19ColorPacked)
		{
			ApplySlot19ColorPackedFromNetwork(slot19ColorPacked);
		}

		private void HandleNetworkedSlot19TargetPlayerIdChanged(int slot19TargetPlayerId)
		{
			ApplySlot19TargetPlayerIdFromNetwork(slot19TargetPlayerId);
		}

		private void HandleNetworkedSlot20CardNetIdChanged(int slot20CardNetId)
		{
			ApplySlot20CardNetIdFromNetwork(slot20CardNetId);
		}

		private void HandleNetworkedSlot20CardDataIdChanged(int slot20CardDataId)
		{
			ApplySlot20CardDataIdFromNetwork(slot20CardDataId);
		}

		private void HandleNetworkedSlot20ColorPackedChanged(int slot20ColorPacked)
		{
			ApplySlot20ColorPackedFromNetwork(slot20ColorPacked);
		}

		private void HandleNetworkedSlot20TargetPlayerIdChanged(int slot20TargetPlayerId)
		{
			ApplySlot20TargetPlayerIdFromNetwork(slot20TargetPlayerId);
		}

		private void HandleNetworkedSlot21CardNetIdChanged(int slot21CardNetId)
		{
			ApplySlot21CardNetIdFromNetwork(slot21CardNetId);
		}

		private void HandleNetworkedSlot21CardDataIdChanged(int slot21CardDataId)
		{
			ApplySlot21CardDataIdFromNetwork(slot21CardDataId);
		}

		private void HandleNetworkedSlot21ColorPackedChanged(int slot21ColorPacked)
		{
			ApplySlot21ColorPackedFromNetwork(slot21ColorPacked);
		}

		private void HandleNetworkedSlot21TargetPlayerIdChanged(int slot21TargetPlayerId)
		{
			ApplySlot21TargetPlayerIdFromNetwork(slot21TargetPlayerId);
		}

		private void HandleNetworkedSlot22CardNetIdChanged(int slot22CardNetId)
		{
			ApplySlot22CardNetIdFromNetwork(slot22CardNetId);
		}

		private void HandleNetworkedSlot22CardDataIdChanged(int slot22CardDataId)
		{
			ApplySlot22CardDataIdFromNetwork(slot22CardDataId);
		}

		private void HandleNetworkedSlot22ColorPackedChanged(int slot22ColorPacked)
		{
			ApplySlot22ColorPackedFromNetwork(slot22ColorPacked);
		}

		private void HandleNetworkedSlot22TargetPlayerIdChanged(int slot22TargetPlayerId)
		{
			ApplySlot22TargetPlayerIdFromNetwork(slot22TargetPlayerId);
		}

		private void HandleNetworkedSlot23CardNetIdChanged(int slot23CardNetId)
		{
			ApplySlot23CardNetIdFromNetwork(slot23CardNetId);
		}

		private void HandleNetworkedSlot23CardDataIdChanged(int slot23CardDataId)
		{
			ApplySlot23CardDataIdFromNetwork(slot23CardDataId);
		}

		private void HandleNetworkedSlot23ColorPackedChanged(int slot23ColorPacked)
		{
			ApplySlot23ColorPackedFromNetwork(slot23ColorPacked);
		}

		private void HandleNetworkedSlot23TargetPlayerIdChanged(int slot23TargetPlayerId)
		{
			ApplySlot23TargetPlayerIdFromNetwork(slot23TargetPlayerId);
		}

		private void HandleNetworkedSlot24CardNetIdChanged(int slot24CardNetId)
		{
			ApplySlot24CardNetIdFromNetwork(slot24CardNetId);
		}

		private void HandleNetworkedSlot24CardDataIdChanged(int slot24CardDataId)
		{
			ApplySlot24CardDataIdFromNetwork(slot24CardDataId);
		}

		private void HandleNetworkedSlot24ColorPackedChanged(int slot24ColorPacked)
		{
			ApplySlot24ColorPackedFromNetwork(slot24ColorPacked);
		}

		private void HandleNetworkedSlot24TargetPlayerIdChanged(int slot24TargetPlayerId)
		{
			ApplySlot24TargetPlayerIdFromNetwork(slot24TargetPlayerId);
		}

		private void HandleNetworkedSlot25CardNetIdChanged(int slot25CardNetId)
		{
			ApplySlot25CardNetIdFromNetwork(slot25CardNetId);
		}

		private void HandleNetworkedSlot25CardDataIdChanged(int slot25CardDataId)
		{
			ApplySlot25CardDataIdFromNetwork(slot25CardDataId);
		}

		private void HandleNetworkedSlot25ColorPackedChanged(int slot25ColorPacked)
		{
			ApplySlot25ColorPackedFromNetwork(slot25ColorPacked);
		}

		private void HandleNetworkedSlot25TargetPlayerIdChanged(int slot25TargetPlayerId)
		{
			ApplySlot25TargetPlayerIdFromNetwork(slot25TargetPlayerId);
		}

		private void HandleNetworkedSlot26CardNetIdChanged(int slot26CardNetId)
		{
			ApplySlot26CardNetIdFromNetwork(slot26CardNetId);
		}

		private void HandleNetworkedSlot26CardDataIdChanged(int slot26CardDataId)
		{
			ApplySlot26CardDataIdFromNetwork(slot26CardDataId);
		}

		private void HandleNetworkedSlot26ColorPackedChanged(int slot26ColorPacked)
		{
			ApplySlot26ColorPackedFromNetwork(slot26ColorPacked);
		}

		private void HandleNetworkedSlot26TargetPlayerIdChanged(int slot26TargetPlayerId)
		{
			ApplySlot26TargetPlayerIdFromNetwork(slot26TargetPlayerId);
		}

		private void HandleNetworkedSlot27CardNetIdChanged(int slot27CardNetId)
		{
			ApplySlot27CardNetIdFromNetwork(slot27CardNetId);
		}

		private void HandleNetworkedSlot27CardDataIdChanged(int slot27CardDataId)
		{
			ApplySlot27CardDataIdFromNetwork(slot27CardDataId);
		}

		private void HandleNetworkedSlot27ColorPackedChanged(int slot27ColorPacked)
		{
			ApplySlot27ColorPackedFromNetwork(slot27ColorPacked);
		}

		private void HandleNetworkedSlot27TargetPlayerIdChanged(int slot27TargetPlayerId)
		{
			ApplySlot27TargetPlayerIdFromNetwork(slot27TargetPlayerId);
		}

		private void HandleNetworkedSlot28CardNetIdChanged(int slot28CardNetId)
		{
			ApplySlot28CardNetIdFromNetwork(slot28CardNetId);
		}

		private void HandleNetworkedSlot28CardDataIdChanged(int slot28CardDataId)
		{
			ApplySlot28CardDataIdFromNetwork(slot28CardDataId);
		}

		private void HandleNetworkedSlot28ColorPackedChanged(int slot28ColorPacked)
		{
			ApplySlot28ColorPackedFromNetwork(slot28ColorPacked);
		}

		private void HandleNetworkedSlot28TargetPlayerIdChanged(int slot28TargetPlayerId)
		{
			ApplySlot28TargetPlayerIdFromNetwork(slot28TargetPlayerId);
		}

		private void HandleNetworkedSlot29CardNetIdChanged(int slot29CardNetId)
		{
			ApplySlot29CardNetIdFromNetwork(slot29CardNetId);
		}

		private void HandleNetworkedSlot29CardDataIdChanged(int slot29CardDataId)
		{
			ApplySlot29CardDataIdFromNetwork(slot29CardDataId);
		}

		private void HandleNetworkedSlot29ColorPackedChanged(int slot29ColorPacked)
		{
			ApplySlot29ColorPackedFromNetwork(slot29ColorPacked);
		}

		private void HandleNetworkedSlot29TargetPlayerIdChanged(int slot29TargetPlayerId)
		{
			ApplySlot29TargetPlayerIdFromNetwork(slot29TargetPlayerId);
		}

		private void HandleNetworkedSlot30CardNetIdChanged(int slot30CardNetId)
		{
			ApplySlot30CardNetIdFromNetwork(slot30CardNetId);
		}

		private void HandleNetworkedSlot30CardDataIdChanged(int slot30CardDataId)
		{
			ApplySlot30CardDataIdFromNetwork(slot30CardDataId);
		}

		private void HandleNetworkedSlot30ColorPackedChanged(int slot30ColorPacked)
		{
			ApplySlot30ColorPackedFromNetwork(slot30ColorPacked);
		}

		private void HandleNetworkedSlot30TargetPlayerIdChanged(int slot30TargetPlayerId)
		{
			ApplySlot30TargetPlayerIdFromNetwork(slot30TargetPlayerId);
		}

		private void HandleNetworkedSlot31CardNetIdChanged(int slot31CardNetId)
		{
			ApplySlot31CardNetIdFromNetwork(slot31CardNetId);
		}

		private void HandleNetworkedSlot31CardDataIdChanged(int slot31CardDataId)
		{
			ApplySlot31CardDataIdFromNetwork(slot31CardDataId);
		}

		private void HandleNetworkedSlot31ColorPackedChanged(int slot31ColorPacked)
		{
			ApplySlot31ColorPackedFromNetwork(slot31ColorPacked);
		}

		private void HandleNetworkedSlot31TargetPlayerIdChanged(int slot31TargetPlayerId)
		{
			ApplySlot31TargetPlayerIdFromNetwork(slot31TargetPlayerId);
		}

		private void ApplyTargetLevelNumberFromNetwork(int targetLevelNumber)
		{
			_pendingRewardModel.TargetLevelNumber.ApplyFromNetwork(targetLevelNumber);
		}

		private void ApplySlot0CardNetIdFromNetwork(int slot0CardNetId)
		{
			_pendingRewardModel.Slot0CardNetId.ApplyFromNetwork(slot0CardNetId);
		}

		private void ApplySlot0CardDataIdFromNetwork(int slot0CardDataId)
		{
			_pendingRewardModel.Slot0CardDataId.ApplyFromNetwork(slot0CardDataId);
		}

		private void ApplySlot0ColorPackedFromNetwork(int slot0ColorPacked)
		{
			_pendingRewardModel.Slot0ColorPacked.ApplyFromNetwork(slot0ColorPacked);
		}

		private void ApplySlot0TargetPlayerIdFromNetwork(int slot0TargetPlayerId)
		{
			_pendingRewardModel.Slot0TargetPlayerId.ApplyFromNetwork(slot0TargetPlayerId);
		}

		private void ApplySlot1CardNetIdFromNetwork(int slot1CardNetId)
		{
			_pendingRewardModel.Slot1CardNetId.ApplyFromNetwork(slot1CardNetId);
		}

		private void ApplySlot1CardDataIdFromNetwork(int slot1CardDataId)
		{
			_pendingRewardModel.Slot1CardDataId.ApplyFromNetwork(slot1CardDataId);
		}

		private void ApplySlot1ColorPackedFromNetwork(int slot1ColorPacked)
		{
			_pendingRewardModel.Slot1ColorPacked.ApplyFromNetwork(slot1ColorPacked);
		}

		private void ApplySlot1TargetPlayerIdFromNetwork(int slot1TargetPlayerId)
		{
			_pendingRewardModel.Slot1TargetPlayerId.ApplyFromNetwork(slot1TargetPlayerId);
		}

		private void ApplySlot2CardNetIdFromNetwork(int slot2CardNetId)
		{
			_pendingRewardModel.Slot2CardNetId.ApplyFromNetwork(slot2CardNetId);
		}

		private void ApplySlot2CardDataIdFromNetwork(int slot2CardDataId)
		{
			_pendingRewardModel.Slot2CardDataId.ApplyFromNetwork(slot2CardDataId);
		}

		private void ApplySlot2ColorPackedFromNetwork(int slot2ColorPacked)
		{
			_pendingRewardModel.Slot2ColorPacked.ApplyFromNetwork(slot2ColorPacked);
		}

		private void ApplySlot2TargetPlayerIdFromNetwork(int slot2TargetPlayerId)
		{
			_pendingRewardModel.Slot2TargetPlayerId.ApplyFromNetwork(slot2TargetPlayerId);
		}

		private void ApplySlot3CardNetIdFromNetwork(int slot3CardNetId)
		{
			_pendingRewardModel.Slot3CardNetId.ApplyFromNetwork(slot3CardNetId);
		}

		private void ApplySlot3CardDataIdFromNetwork(int slot3CardDataId)
		{
			_pendingRewardModel.Slot3CardDataId.ApplyFromNetwork(slot3CardDataId);
		}

		private void ApplySlot3ColorPackedFromNetwork(int slot3ColorPacked)
		{
			_pendingRewardModel.Slot3ColorPacked.ApplyFromNetwork(slot3ColorPacked);
		}

		private void ApplySlot3TargetPlayerIdFromNetwork(int slot3TargetPlayerId)
		{
			_pendingRewardModel.Slot3TargetPlayerId.ApplyFromNetwork(slot3TargetPlayerId);
		}

		private void ApplySlot4CardNetIdFromNetwork(int slot4CardNetId)
		{
			_pendingRewardModel.Slot4CardNetId.ApplyFromNetwork(slot4CardNetId);
		}

		private void ApplySlot4CardDataIdFromNetwork(int slot4CardDataId)
		{
			_pendingRewardModel.Slot4CardDataId.ApplyFromNetwork(slot4CardDataId);
		}

		private void ApplySlot4ColorPackedFromNetwork(int slot4ColorPacked)
		{
			_pendingRewardModel.Slot4ColorPacked.ApplyFromNetwork(slot4ColorPacked);
		}

		private void ApplySlot4TargetPlayerIdFromNetwork(int slot4TargetPlayerId)
		{
			_pendingRewardModel.Slot4TargetPlayerId.ApplyFromNetwork(slot4TargetPlayerId);
		}

		private void ApplySlot5CardNetIdFromNetwork(int slot5CardNetId)
		{
			_pendingRewardModel.Slot5CardNetId.ApplyFromNetwork(slot5CardNetId);
		}

		private void ApplySlot5CardDataIdFromNetwork(int slot5CardDataId)
		{
			_pendingRewardModel.Slot5CardDataId.ApplyFromNetwork(slot5CardDataId);
		}

		private void ApplySlot5ColorPackedFromNetwork(int slot5ColorPacked)
		{
			_pendingRewardModel.Slot5ColorPacked.ApplyFromNetwork(slot5ColorPacked);
		}

		private void ApplySlot5TargetPlayerIdFromNetwork(int slot5TargetPlayerId)
		{
			_pendingRewardModel.Slot5TargetPlayerId.ApplyFromNetwork(slot5TargetPlayerId);
		}

		private void ApplySlot6CardNetIdFromNetwork(int slot6CardNetId)
		{
			_pendingRewardModel.Slot6CardNetId.ApplyFromNetwork(slot6CardNetId);
		}

		private void ApplySlot6CardDataIdFromNetwork(int slot6CardDataId)
		{
			_pendingRewardModel.Slot6CardDataId.ApplyFromNetwork(slot6CardDataId);
		}

		private void ApplySlot6ColorPackedFromNetwork(int slot6ColorPacked)
		{
			_pendingRewardModel.Slot6ColorPacked.ApplyFromNetwork(slot6ColorPacked);
		}

		private void ApplySlot6TargetPlayerIdFromNetwork(int slot6TargetPlayerId)
		{
			_pendingRewardModel.Slot6TargetPlayerId.ApplyFromNetwork(slot6TargetPlayerId);
		}

		private void ApplySlot7CardNetIdFromNetwork(int slot7CardNetId)
		{
			_pendingRewardModel.Slot7CardNetId.ApplyFromNetwork(slot7CardNetId);
		}

		private void ApplySlot7CardDataIdFromNetwork(int slot7CardDataId)
		{
			_pendingRewardModel.Slot7CardDataId.ApplyFromNetwork(slot7CardDataId);
		}

		private void ApplySlot7ColorPackedFromNetwork(int slot7ColorPacked)
		{
			_pendingRewardModel.Slot7ColorPacked.ApplyFromNetwork(slot7ColorPacked);
		}

		private void ApplySlot7TargetPlayerIdFromNetwork(int slot7TargetPlayerId)
		{
			_pendingRewardModel.Slot7TargetPlayerId.ApplyFromNetwork(slot7TargetPlayerId);
		}

		private void ApplySlot8CardNetIdFromNetwork(int slot8CardNetId)
		{
			_pendingRewardModel.Slot8CardNetId.ApplyFromNetwork(slot8CardNetId);
		}

		private void ApplySlot8CardDataIdFromNetwork(int slot8CardDataId)
		{
			_pendingRewardModel.Slot8CardDataId.ApplyFromNetwork(slot8CardDataId);
		}

		private void ApplySlot8ColorPackedFromNetwork(int slot8ColorPacked)
		{
			_pendingRewardModel.Slot8ColorPacked.ApplyFromNetwork(slot8ColorPacked);
		}

		private void ApplySlot8TargetPlayerIdFromNetwork(int slot8TargetPlayerId)
		{
			_pendingRewardModel.Slot8TargetPlayerId.ApplyFromNetwork(slot8TargetPlayerId);
		}

		private void ApplySlot9CardNetIdFromNetwork(int slot9CardNetId)
		{
			_pendingRewardModel.Slot9CardNetId.ApplyFromNetwork(slot9CardNetId);
		}

		private void ApplySlot9CardDataIdFromNetwork(int slot9CardDataId)
		{
			_pendingRewardModel.Slot9CardDataId.ApplyFromNetwork(slot9CardDataId);
		}

		private void ApplySlot9ColorPackedFromNetwork(int slot9ColorPacked)
		{
			_pendingRewardModel.Slot9ColorPacked.ApplyFromNetwork(slot9ColorPacked);
		}

		private void ApplySlot9TargetPlayerIdFromNetwork(int slot9TargetPlayerId)
		{
			_pendingRewardModel.Slot9TargetPlayerId.ApplyFromNetwork(slot9TargetPlayerId);
		}

		private void ApplySlot10CardNetIdFromNetwork(int slot10CardNetId)
		{
			_pendingRewardModel.Slot10CardNetId.ApplyFromNetwork(slot10CardNetId);
		}

		private void ApplySlot10CardDataIdFromNetwork(int slot10CardDataId)
		{
			_pendingRewardModel.Slot10CardDataId.ApplyFromNetwork(slot10CardDataId);
		}

		private void ApplySlot10ColorPackedFromNetwork(int slot10ColorPacked)
		{
			_pendingRewardModel.Slot10ColorPacked.ApplyFromNetwork(slot10ColorPacked);
		}

		private void ApplySlot10TargetPlayerIdFromNetwork(int slot10TargetPlayerId)
		{
			_pendingRewardModel.Slot10TargetPlayerId.ApplyFromNetwork(slot10TargetPlayerId);
		}

		private void ApplySlot11CardNetIdFromNetwork(int slot11CardNetId)
		{
			_pendingRewardModel.Slot11CardNetId.ApplyFromNetwork(slot11CardNetId);
		}

		private void ApplySlot11CardDataIdFromNetwork(int slot11CardDataId)
		{
			_pendingRewardModel.Slot11CardDataId.ApplyFromNetwork(slot11CardDataId);
		}

		private void ApplySlot11ColorPackedFromNetwork(int slot11ColorPacked)
		{
			_pendingRewardModel.Slot11ColorPacked.ApplyFromNetwork(slot11ColorPacked);
		}

		private void ApplySlot11TargetPlayerIdFromNetwork(int slot11TargetPlayerId)
		{
			_pendingRewardModel.Slot11TargetPlayerId.ApplyFromNetwork(slot11TargetPlayerId);
		}

		private void ApplySlot12CardNetIdFromNetwork(int slot12CardNetId)
		{
			_pendingRewardModel.Slot12CardNetId.ApplyFromNetwork(slot12CardNetId);
		}

		private void ApplySlot12CardDataIdFromNetwork(int slot12CardDataId)
		{
			_pendingRewardModel.Slot12CardDataId.ApplyFromNetwork(slot12CardDataId);
		}

		private void ApplySlot12ColorPackedFromNetwork(int slot12ColorPacked)
		{
			_pendingRewardModel.Slot12ColorPacked.ApplyFromNetwork(slot12ColorPacked);
		}

		private void ApplySlot12TargetPlayerIdFromNetwork(int slot12TargetPlayerId)
		{
			_pendingRewardModel.Slot12TargetPlayerId.ApplyFromNetwork(slot12TargetPlayerId);
		}

		private void ApplySlot13CardNetIdFromNetwork(int slot13CardNetId)
		{
			_pendingRewardModel.Slot13CardNetId.ApplyFromNetwork(slot13CardNetId);
		}

		private void ApplySlot13CardDataIdFromNetwork(int slot13CardDataId)
		{
			_pendingRewardModel.Slot13CardDataId.ApplyFromNetwork(slot13CardDataId);
		}

		private void ApplySlot13ColorPackedFromNetwork(int slot13ColorPacked)
		{
			_pendingRewardModel.Slot13ColorPacked.ApplyFromNetwork(slot13ColorPacked);
		}

		private void ApplySlot13TargetPlayerIdFromNetwork(int slot13TargetPlayerId)
		{
			_pendingRewardModel.Slot13TargetPlayerId.ApplyFromNetwork(slot13TargetPlayerId);
		}

		private void ApplySlot14CardNetIdFromNetwork(int slot14CardNetId)
		{
			_pendingRewardModel.Slot14CardNetId.ApplyFromNetwork(slot14CardNetId);
		}

		private void ApplySlot14CardDataIdFromNetwork(int slot14CardDataId)
		{
			_pendingRewardModel.Slot14CardDataId.ApplyFromNetwork(slot14CardDataId);
		}

		private void ApplySlot14ColorPackedFromNetwork(int slot14ColorPacked)
		{
			_pendingRewardModel.Slot14ColorPacked.ApplyFromNetwork(slot14ColorPacked);
		}

		private void ApplySlot14TargetPlayerIdFromNetwork(int slot14TargetPlayerId)
		{
			_pendingRewardModel.Slot14TargetPlayerId.ApplyFromNetwork(slot14TargetPlayerId);
		}

		private void ApplySlot15CardNetIdFromNetwork(int slot15CardNetId)
		{
			_pendingRewardModel.Slot15CardNetId.ApplyFromNetwork(slot15CardNetId);
		}

		private void ApplySlot15CardDataIdFromNetwork(int slot15CardDataId)
		{
			_pendingRewardModel.Slot15CardDataId.ApplyFromNetwork(slot15CardDataId);
		}

		private void ApplySlot15ColorPackedFromNetwork(int slot15ColorPacked)
		{
			_pendingRewardModel.Slot15ColorPacked.ApplyFromNetwork(slot15ColorPacked);
		}

		private void ApplySlot15TargetPlayerIdFromNetwork(int slot15TargetPlayerId)
		{
			_pendingRewardModel.Slot15TargetPlayerId.ApplyFromNetwork(slot15TargetPlayerId);
		}

		private void ApplySlot16CardNetIdFromNetwork(int slot16CardNetId)
		{
			_pendingRewardModel.Slot16CardNetId.ApplyFromNetwork(slot16CardNetId);
		}

		private void ApplySlot16CardDataIdFromNetwork(int slot16CardDataId)
		{
			_pendingRewardModel.Slot16CardDataId.ApplyFromNetwork(slot16CardDataId);
		}

		private void ApplySlot16ColorPackedFromNetwork(int slot16ColorPacked)
		{
			_pendingRewardModel.Slot16ColorPacked.ApplyFromNetwork(slot16ColorPacked);
		}

		private void ApplySlot16TargetPlayerIdFromNetwork(int slot16TargetPlayerId)
		{
			_pendingRewardModel.Slot16TargetPlayerId.ApplyFromNetwork(slot16TargetPlayerId);
		}

		private void ApplySlot17CardNetIdFromNetwork(int slot17CardNetId)
		{
			_pendingRewardModel.Slot17CardNetId.ApplyFromNetwork(slot17CardNetId);
		}

		private void ApplySlot17CardDataIdFromNetwork(int slot17CardDataId)
		{
			_pendingRewardModel.Slot17CardDataId.ApplyFromNetwork(slot17CardDataId);
		}

		private void ApplySlot17ColorPackedFromNetwork(int slot17ColorPacked)
		{
			_pendingRewardModel.Slot17ColorPacked.ApplyFromNetwork(slot17ColorPacked);
		}

		private void ApplySlot17TargetPlayerIdFromNetwork(int slot17TargetPlayerId)
		{
			_pendingRewardModel.Slot17TargetPlayerId.ApplyFromNetwork(slot17TargetPlayerId);
		}

		private void ApplySlot18CardNetIdFromNetwork(int slot18CardNetId)
		{
			_pendingRewardModel.Slot18CardNetId.ApplyFromNetwork(slot18CardNetId);
		}

		private void ApplySlot18CardDataIdFromNetwork(int slot18CardDataId)
		{
			_pendingRewardModel.Slot18CardDataId.ApplyFromNetwork(slot18CardDataId);
		}

		private void ApplySlot18ColorPackedFromNetwork(int slot18ColorPacked)
		{
			_pendingRewardModel.Slot18ColorPacked.ApplyFromNetwork(slot18ColorPacked);
		}

		private void ApplySlot18TargetPlayerIdFromNetwork(int slot18TargetPlayerId)
		{
			_pendingRewardModel.Slot18TargetPlayerId.ApplyFromNetwork(slot18TargetPlayerId);
		}

		private void ApplySlot19CardNetIdFromNetwork(int slot19CardNetId)
		{
			_pendingRewardModel.Slot19CardNetId.ApplyFromNetwork(slot19CardNetId);
		}

		private void ApplySlot19CardDataIdFromNetwork(int slot19CardDataId)
		{
			_pendingRewardModel.Slot19CardDataId.ApplyFromNetwork(slot19CardDataId);
		}

		private void ApplySlot19ColorPackedFromNetwork(int slot19ColorPacked)
		{
			_pendingRewardModel.Slot19ColorPacked.ApplyFromNetwork(slot19ColorPacked);
		}

		private void ApplySlot19TargetPlayerIdFromNetwork(int slot19TargetPlayerId)
		{
			_pendingRewardModel.Slot19TargetPlayerId.ApplyFromNetwork(slot19TargetPlayerId);
		}

		private void ApplySlot20CardNetIdFromNetwork(int slot20CardNetId)
		{
			_pendingRewardModel.Slot20CardNetId.ApplyFromNetwork(slot20CardNetId);
		}

		private void ApplySlot20CardDataIdFromNetwork(int slot20CardDataId)
		{
			_pendingRewardModel.Slot20CardDataId.ApplyFromNetwork(slot20CardDataId);
		}

		private void ApplySlot20ColorPackedFromNetwork(int slot20ColorPacked)
		{
			_pendingRewardModel.Slot20ColorPacked.ApplyFromNetwork(slot20ColorPacked);
		}

		private void ApplySlot20TargetPlayerIdFromNetwork(int slot20TargetPlayerId)
		{
			_pendingRewardModel.Slot20TargetPlayerId.ApplyFromNetwork(slot20TargetPlayerId);
		}

		private void ApplySlot21CardNetIdFromNetwork(int slot21CardNetId)
		{
			_pendingRewardModel.Slot21CardNetId.ApplyFromNetwork(slot21CardNetId);
		}

		private void ApplySlot21CardDataIdFromNetwork(int slot21CardDataId)
		{
			_pendingRewardModel.Slot21CardDataId.ApplyFromNetwork(slot21CardDataId);
		}

		private void ApplySlot21ColorPackedFromNetwork(int slot21ColorPacked)
		{
			_pendingRewardModel.Slot21ColorPacked.ApplyFromNetwork(slot21ColorPacked);
		}

		private void ApplySlot21TargetPlayerIdFromNetwork(int slot21TargetPlayerId)
		{
			_pendingRewardModel.Slot21TargetPlayerId.ApplyFromNetwork(slot21TargetPlayerId);
		}

		private void ApplySlot22CardNetIdFromNetwork(int slot22CardNetId)
		{
			_pendingRewardModel.Slot22CardNetId.ApplyFromNetwork(slot22CardNetId);
		}

		private void ApplySlot22CardDataIdFromNetwork(int slot22CardDataId)
		{
			_pendingRewardModel.Slot22CardDataId.ApplyFromNetwork(slot22CardDataId);
		}

		private void ApplySlot22ColorPackedFromNetwork(int slot22ColorPacked)
		{
			_pendingRewardModel.Slot22ColorPacked.ApplyFromNetwork(slot22ColorPacked);
		}

		private void ApplySlot22TargetPlayerIdFromNetwork(int slot22TargetPlayerId)
		{
			_pendingRewardModel.Slot22TargetPlayerId.ApplyFromNetwork(slot22TargetPlayerId);
		}

		private void ApplySlot23CardNetIdFromNetwork(int slot23CardNetId)
		{
			_pendingRewardModel.Slot23CardNetId.ApplyFromNetwork(slot23CardNetId);
		}

		private void ApplySlot23CardDataIdFromNetwork(int slot23CardDataId)
		{
			_pendingRewardModel.Slot23CardDataId.ApplyFromNetwork(slot23CardDataId);
		}

		private void ApplySlot23ColorPackedFromNetwork(int slot23ColorPacked)
		{
			_pendingRewardModel.Slot23ColorPacked.ApplyFromNetwork(slot23ColorPacked);
		}

		private void ApplySlot23TargetPlayerIdFromNetwork(int slot23TargetPlayerId)
		{
			_pendingRewardModel.Slot23TargetPlayerId.ApplyFromNetwork(slot23TargetPlayerId);
		}

		private void ApplySlot24CardNetIdFromNetwork(int slot24CardNetId)
		{
			_pendingRewardModel.Slot24CardNetId.ApplyFromNetwork(slot24CardNetId);
		}

		private void ApplySlot24CardDataIdFromNetwork(int slot24CardDataId)
		{
			_pendingRewardModel.Slot24CardDataId.ApplyFromNetwork(slot24CardDataId);
		}

		private void ApplySlot24ColorPackedFromNetwork(int slot24ColorPacked)
		{
			_pendingRewardModel.Slot24ColorPacked.ApplyFromNetwork(slot24ColorPacked);
		}

		private void ApplySlot24TargetPlayerIdFromNetwork(int slot24TargetPlayerId)
		{
			_pendingRewardModel.Slot24TargetPlayerId.ApplyFromNetwork(slot24TargetPlayerId);
		}

		private void ApplySlot25CardNetIdFromNetwork(int slot25CardNetId)
		{
			_pendingRewardModel.Slot25CardNetId.ApplyFromNetwork(slot25CardNetId);
		}

		private void ApplySlot25CardDataIdFromNetwork(int slot25CardDataId)
		{
			_pendingRewardModel.Slot25CardDataId.ApplyFromNetwork(slot25CardDataId);
		}

		private void ApplySlot25ColorPackedFromNetwork(int slot25ColorPacked)
		{
			_pendingRewardModel.Slot25ColorPacked.ApplyFromNetwork(slot25ColorPacked);
		}

		private void ApplySlot25TargetPlayerIdFromNetwork(int slot25TargetPlayerId)
		{
			_pendingRewardModel.Slot25TargetPlayerId.ApplyFromNetwork(slot25TargetPlayerId);
		}

		private void ApplySlot26CardNetIdFromNetwork(int slot26CardNetId)
		{
			_pendingRewardModel.Slot26CardNetId.ApplyFromNetwork(slot26CardNetId);
		}

		private void ApplySlot26CardDataIdFromNetwork(int slot26CardDataId)
		{
			_pendingRewardModel.Slot26CardDataId.ApplyFromNetwork(slot26CardDataId);
		}

		private void ApplySlot26ColorPackedFromNetwork(int slot26ColorPacked)
		{
			_pendingRewardModel.Slot26ColorPacked.ApplyFromNetwork(slot26ColorPacked);
		}

		private void ApplySlot26TargetPlayerIdFromNetwork(int slot26TargetPlayerId)
		{
			_pendingRewardModel.Slot26TargetPlayerId.ApplyFromNetwork(slot26TargetPlayerId);
		}

		private void ApplySlot27CardNetIdFromNetwork(int slot27CardNetId)
		{
			_pendingRewardModel.Slot27CardNetId.ApplyFromNetwork(slot27CardNetId);
		}

		private void ApplySlot27CardDataIdFromNetwork(int slot27CardDataId)
		{
			_pendingRewardModel.Slot27CardDataId.ApplyFromNetwork(slot27CardDataId);
		}

		private void ApplySlot27ColorPackedFromNetwork(int slot27ColorPacked)
		{
			_pendingRewardModel.Slot27ColorPacked.ApplyFromNetwork(slot27ColorPacked);
		}

		private void ApplySlot27TargetPlayerIdFromNetwork(int slot27TargetPlayerId)
		{
			_pendingRewardModel.Slot27TargetPlayerId.ApplyFromNetwork(slot27TargetPlayerId);
		}

		private void ApplySlot28CardNetIdFromNetwork(int slot28CardNetId)
		{
			_pendingRewardModel.Slot28CardNetId.ApplyFromNetwork(slot28CardNetId);
		}

		private void ApplySlot28CardDataIdFromNetwork(int slot28CardDataId)
		{
			_pendingRewardModel.Slot28CardDataId.ApplyFromNetwork(slot28CardDataId);
		}

		private void ApplySlot28ColorPackedFromNetwork(int slot28ColorPacked)
		{
			_pendingRewardModel.Slot28ColorPacked.ApplyFromNetwork(slot28ColorPacked);
		}

		private void ApplySlot28TargetPlayerIdFromNetwork(int slot28TargetPlayerId)
		{
			_pendingRewardModel.Slot28TargetPlayerId.ApplyFromNetwork(slot28TargetPlayerId);
		}

		private void ApplySlot29CardNetIdFromNetwork(int slot29CardNetId)
		{
			_pendingRewardModel.Slot29CardNetId.ApplyFromNetwork(slot29CardNetId);
		}

		private void ApplySlot29CardDataIdFromNetwork(int slot29CardDataId)
		{
			_pendingRewardModel.Slot29CardDataId.ApplyFromNetwork(slot29CardDataId);
		}

		private void ApplySlot29ColorPackedFromNetwork(int slot29ColorPacked)
		{
			_pendingRewardModel.Slot29ColorPacked.ApplyFromNetwork(slot29ColorPacked);
		}

		private void ApplySlot29TargetPlayerIdFromNetwork(int slot29TargetPlayerId)
		{
			_pendingRewardModel.Slot29TargetPlayerId.ApplyFromNetwork(slot29TargetPlayerId);
		}

		private void ApplySlot30CardNetIdFromNetwork(int slot30CardNetId)
		{
			_pendingRewardModel.Slot30CardNetId.ApplyFromNetwork(slot30CardNetId);
		}

		private void ApplySlot30CardDataIdFromNetwork(int slot30CardDataId)
		{
			_pendingRewardModel.Slot30CardDataId.ApplyFromNetwork(slot30CardDataId);
		}

		private void ApplySlot30ColorPackedFromNetwork(int slot30ColorPacked)
		{
			_pendingRewardModel.Slot30ColorPacked.ApplyFromNetwork(slot30ColorPacked);
		}

		private void ApplySlot30TargetPlayerIdFromNetwork(int slot30TargetPlayerId)
		{
			_pendingRewardModel.Slot30TargetPlayerId.ApplyFromNetwork(slot30TargetPlayerId);
		}

		private void ApplySlot31CardNetIdFromNetwork(int slot31CardNetId)
		{
			_pendingRewardModel.Slot31CardNetId.ApplyFromNetwork(slot31CardNetId);
		}

		private void ApplySlot31CardDataIdFromNetwork(int slot31CardDataId)
		{
			_pendingRewardModel.Slot31CardDataId.ApplyFromNetwork(slot31CardDataId);
		}

		private void ApplySlot31ColorPackedFromNetwork(int slot31ColorPacked)
		{
			_pendingRewardModel.Slot31ColorPacked.ApplyFromNetwork(slot31ColorPacked);
		}

		private void ApplySlot31TargetPlayerIdFromNetwork(int slot31TargetPlayerId)
		{
			_pendingRewardModel.Slot31TargetPlayerId.ApplyFromNetwork(slot31TargetPlayerId);
		}

		private bool WriteTargetLevelNumber(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.TargetLevelNumber was written without state authority; the write was ignored.");
				return false;
			}
			_pendingTargetLevelNumber = value;
			_hasPendingTargetLevelNumber = true;
			return true;
		}

		private bool WriteSlot0CardNetId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot0CardNetId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot0CardNetId = value;
			_hasPendingSlot0CardNetId = true;
			return true;
		}

		private bool WriteSlot0CardDataId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot0CardDataId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot0CardDataId = value;
			_hasPendingSlot0CardDataId = true;
			return true;
		}

		private bool WriteSlot0ColorPacked(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot0ColorPacked was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot0ColorPacked = value;
			_hasPendingSlot0ColorPacked = true;
			return true;
		}

		private bool WriteSlot0TargetPlayerId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot0TargetPlayerId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot0TargetPlayerId = value;
			_hasPendingSlot0TargetPlayerId = true;
			return true;
		}

		private bool WriteSlot1CardNetId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot1CardNetId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot1CardNetId = value;
			_hasPendingSlot1CardNetId = true;
			return true;
		}

		private bool WriteSlot1CardDataId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot1CardDataId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot1CardDataId = value;
			_hasPendingSlot1CardDataId = true;
			return true;
		}

		private bool WriteSlot1ColorPacked(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot1ColorPacked was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot1ColorPacked = value;
			_hasPendingSlot1ColorPacked = true;
			return true;
		}

		private bool WriteSlot1TargetPlayerId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot1TargetPlayerId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot1TargetPlayerId = value;
			_hasPendingSlot1TargetPlayerId = true;
			return true;
		}

		private bool WriteSlot2CardNetId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot2CardNetId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot2CardNetId = value;
			_hasPendingSlot2CardNetId = true;
			return true;
		}

		private bool WriteSlot2CardDataId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot2CardDataId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot2CardDataId = value;
			_hasPendingSlot2CardDataId = true;
			return true;
		}

		private bool WriteSlot2ColorPacked(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot2ColorPacked was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot2ColorPacked = value;
			_hasPendingSlot2ColorPacked = true;
			return true;
		}

		private bool WriteSlot2TargetPlayerId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot2TargetPlayerId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot2TargetPlayerId = value;
			_hasPendingSlot2TargetPlayerId = true;
			return true;
		}

		private bool WriteSlot3CardNetId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot3CardNetId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot3CardNetId = value;
			_hasPendingSlot3CardNetId = true;
			return true;
		}

		private bool WriteSlot3CardDataId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot3CardDataId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot3CardDataId = value;
			_hasPendingSlot3CardDataId = true;
			return true;
		}

		private bool WriteSlot3ColorPacked(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot3ColorPacked was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot3ColorPacked = value;
			_hasPendingSlot3ColorPacked = true;
			return true;
		}

		private bool WriteSlot3TargetPlayerId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot3TargetPlayerId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot3TargetPlayerId = value;
			_hasPendingSlot3TargetPlayerId = true;
			return true;
		}

		private bool WriteSlot4CardNetId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot4CardNetId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot4CardNetId = value;
			_hasPendingSlot4CardNetId = true;
			return true;
		}

		private bool WriteSlot4CardDataId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot4CardDataId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot4CardDataId = value;
			_hasPendingSlot4CardDataId = true;
			return true;
		}

		private bool WriteSlot4ColorPacked(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot4ColorPacked was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot4ColorPacked = value;
			_hasPendingSlot4ColorPacked = true;
			return true;
		}

		private bool WriteSlot4TargetPlayerId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot4TargetPlayerId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot4TargetPlayerId = value;
			_hasPendingSlot4TargetPlayerId = true;
			return true;
		}

		private bool WriteSlot5CardNetId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot5CardNetId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot5CardNetId = value;
			_hasPendingSlot5CardNetId = true;
			return true;
		}

		private bool WriteSlot5CardDataId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot5CardDataId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot5CardDataId = value;
			_hasPendingSlot5CardDataId = true;
			return true;
		}

		private bool WriteSlot5ColorPacked(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot5ColorPacked was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot5ColorPacked = value;
			_hasPendingSlot5ColorPacked = true;
			return true;
		}

		private bool WriteSlot5TargetPlayerId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot5TargetPlayerId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot5TargetPlayerId = value;
			_hasPendingSlot5TargetPlayerId = true;
			return true;
		}

		private bool WriteSlot6CardNetId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot6CardNetId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot6CardNetId = value;
			_hasPendingSlot6CardNetId = true;
			return true;
		}

		private bool WriteSlot6CardDataId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot6CardDataId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot6CardDataId = value;
			_hasPendingSlot6CardDataId = true;
			return true;
		}

		private bool WriteSlot6ColorPacked(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot6ColorPacked was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot6ColorPacked = value;
			_hasPendingSlot6ColorPacked = true;
			return true;
		}

		private bool WriteSlot6TargetPlayerId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot6TargetPlayerId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot6TargetPlayerId = value;
			_hasPendingSlot6TargetPlayerId = true;
			return true;
		}

		private bool WriteSlot7CardNetId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot7CardNetId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot7CardNetId = value;
			_hasPendingSlot7CardNetId = true;
			return true;
		}

		private bool WriteSlot7CardDataId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot7CardDataId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot7CardDataId = value;
			_hasPendingSlot7CardDataId = true;
			return true;
		}

		private bool WriteSlot7ColorPacked(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot7ColorPacked was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot7ColorPacked = value;
			_hasPendingSlot7ColorPacked = true;
			return true;
		}

		private bool WriteSlot7TargetPlayerId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot7TargetPlayerId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot7TargetPlayerId = value;
			_hasPendingSlot7TargetPlayerId = true;
			return true;
		}

		private bool WriteSlot8CardNetId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot8CardNetId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot8CardNetId = value;
			_hasPendingSlot8CardNetId = true;
			return true;
		}

		private bool WriteSlot8CardDataId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot8CardDataId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot8CardDataId = value;
			_hasPendingSlot8CardDataId = true;
			return true;
		}

		private bool WriteSlot8ColorPacked(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot8ColorPacked was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot8ColorPacked = value;
			_hasPendingSlot8ColorPacked = true;
			return true;
		}

		private bool WriteSlot8TargetPlayerId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot8TargetPlayerId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot8TargetPlayerId = value;
			_hasPendingSlot8TargetPlayerId = true;
			return true;
		}

		private bool WriteSlot9CardNetId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot9CardNetId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot9CardNetId = value;
			_hasPendingSlot9CardNetId = true;
			return true;
		}

		private bool WriteSlot9CardDataId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot9CardDataId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot9CardDataId = value;
			_hasPendingSlot9CardDataId = true;
			return true;
		}

		private bool WriteSlot9ColorPacked(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot9ColorPacked was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot9ColorPacked = value;
			_hasPendingSlot9ColorPacked = true;
			return true;
		}

		private bool WriteSlot9TargetPlayerId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot9TargetPlayerId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot9TargetPlayerId = value;
			_hasPendingSlot9TargetPlayerId = true;
			return true;
		}

		private bool WriteSlot10CardNetId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot10CardNetId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot10CardNetId = value;
			_hasPendingSlot10CardNetId = true;
			return true;
		}

		private bool WriteSlot10CardDataId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot10CardDataId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot10CardDataId = value;
			_hasPendingSlot10CardDataId = true;
			return true;
		}

		private bool WriteSlot10ColorPacked(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot10ColorPacked was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot10ColorPacked = value;
			_hasPendingSlot10ColorPacked = true;
			return true;
		}

		private bool WriteSlot10TargetPlayerId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot10TargetPlayerId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot10TargetPlayerId = value;
			_hasPendingSlot10TargetPlayerId = true;
			return true;
		}

		private bool WriteSlot11CardNetId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot11CardNetId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot11CardNetId = value;
			_hasPendingSlot11CardNetId = true;
			return true;
		}

		private bool WriteSlot11CardDataId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot11CardDataId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot11CardDataId = value;
			_hasPendingSlot11CardDataId = true;
			return true;
		}

		private bool WriteSlot11ColorPacked(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot11ColorPacked was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot11ColorPacked = value;
			_hasPendingSlot11ColorPacked = true;
			return true;
		}

		private bool WriteSlot11TargetPlayerId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot11TargetPlayerId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot11TargetPlayerId = value;
			_hasPendingSlot11TargetPlayerId = true;
			return true;
		}

		private bool WriteSlot12CardNetId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot12CardNetId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot12CardNetId = value;
			_hasPendingSlot12CardNetId = true;
			return true;
		}

		private bool WriteSlot12CardDataId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot12CardDataId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot12CardDataId = value;
			_hasPendingSlot12CardDataId = true;
			return true;
		}

		private bool WriteSlot12ColorPacked(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot12ColorPacked was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot12ColorPacked = value;
			_hasPendingSlot12ColorPacked = true;
			return true;
		}

		private bool WriteSlot12TargetPlayerId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot12TargetPlayerId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot12TargetPlayerId = value;
			_hasPendingSlot12TargetPlayerId = true;
			return true;
		}

		private bool WriteSlot13CardNetId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot13CardNetId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot13CardNetId = value;
			_hasPendingSlot13CardNetId = true;
			return true;
		}

		private bool WriteSlot13CardDataId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot13CardDataId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot13CardDataId = value;
			_hasPendingSlot13CardDataId = true;
			return true;
		}

		private bool WriteSlot13ColorPacked(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot13ColorPacked was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot13ColorPacked = value;
			_hasPendingSlot13ColorPacked = true;
			return true;
		}

		private bool WriteSlot13TargetPlayerId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot13TargetPlayerId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot13TargetPlayerId = value;
			_hasPendingSlot13TargetPlayerId = true;
			return true;
		}

		private bool WriteSlot14CardNetId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot14CardNetId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot14CardNetId = value;
			_hasPendingSlot14CardNetId = true;
			return true;
		}

		private bool WriteSlot14CardDataId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot14CardDataId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot14CardDataId = value;
			_hasPendingSlot14CardDataId = true;
			return true;
		}

		private bool WriteSlot14ColorPacked(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot14ColorPacked was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot14ColorPacked = value;
			_hasPendingSlot14ColorPacked = true;
			return true;
		}

		private bool WriteSlot14TargetPlayerId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot14TargetPlayerId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot14TargetPlayerId = value;
			_hasPendingSlot14TargetPlayerId = true;
			return true;
		}

		private bool WriteSlot15CardNetId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot15CardNetId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot15CardNetId = value;
			_hasPendingSlot15CardNetId = true;
			return true;
		}

		private bool WriteSlot15CardDataId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot15CardDataId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot15CardDataId = value;
			_hasPendingSlot15CardDataId = true;
			return true;
		}

		private bool WriteSlot15ColorPacked(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot15ColorPacked was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot15ColorPacked = value;
			_hasPendingSlot15ColorPacked = true;
			return true;
		}

		private bool WriteSlot15TargetPlayerId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot15TargetPlayerId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot15TargetPlayerId = value;
			_hasPendingSlot15TargetPlayerId = true;
			return true;
		}

		private bool WriteSlot16CardNetId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot16CardNetId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot16CardNetId = value;
			_hasPendingSlot16CardNetId = true;
			return true;
		}

		private bool WriteSlot16CardDataId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot16CardDataId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot16CardDataId = value;
			_hasPendingSlot16CardDataId = true;
			return true;
		}

		private bool WriteSlot16ColorPacked(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot16ColorPacked was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot16ColorPacked = value;
			_hasPendingSlot16ColorPacked = true;
			return true;
		}

		private bool WriteSlot16TargetPlayerId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot16TargetPlayerId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot16TargetPlayerId = value;
			_hasPendingSlot16TargetPlayerId = true;
			return true;
		}

		private bool WriteSlot17CardNetId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot17CardNetId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot17CardNetId = value;
			_hasPendingSlot17CardNetId = true;
			return true;
		}

		private bool WriteSlot17CardDataId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot17CardDataId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot17CardDataId = value;
			_hasPendingSlot17CardDataId = true;
			return true;
		}

		private bool WriteSlot17ColorPacked(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot17ColorPacked was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot17ColorPacked = value;
			_hasPendingSlot17ColorPacked = true;
			return true;
		}

		private bool WriteSlot17TargetPlayerId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot17TargetPlayerId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot17TargetPlayerId = value;
			_hasPendingSlot17TargetPlayerId = true;
			return true;
		}

		private bool WriteSlot18CardNetId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot18CardNetId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot18CardNetId = value;
			_hasPendingSlot18CardNetId = true;
			return true;
		}

		private bool WriteSlot18CardDataId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot18CardDataId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot18CardDataId = value;
			_hasPendingSlot18CardDataId = true;
			return true;
		}

		private bool WriteSlot18ColorPacked(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot18ColorPacked was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot18ColorPacked = value;
			_hasPendingSlot18ColorPacked = true;
			return true;
		}

		private bool WriteSlot18TargetPlayerId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot18TargetPlayerId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot18TargetPlayerId = value;
			_hasPendingSlot18TargetPlayerId = true;
			return true;
		}

		private bool WriteSlot19CardNetId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot19CardNetId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot19CardNetId = value;
			_hasPendingSlot19CardNetId = true;
			return true;
		}

		private bool WriteSlot19CardDataId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot19CardDataId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot19CardDataId = value;
			_hasPendingSlot19CardDataId = true;
			return true;
		}

		private bool WriteSlot19ColorPacked(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot19ColorPacked was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot19ColorPacked = value;
			_hasPendingSlot19ColorPacked = true;
			return true;
		}

		private bool WriteSlot19TargetPlayerId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot19TargetPlayerId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot19TargetPlayerId = value;
			_hasPendingSlot19TargetPlayerId = true;
			return true;
		}

		private bool WriteSlot20CardNetId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot20CardNetId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot20CardNetId = value;
			_hasPendingSlot20CardNetId = true;
			return true;
		}

		private bool WriteSlot20CardDataId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot20CardDataId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot20CardDataId = value;
			_hasPendingSlot20CardDataId = true;
			return true;
		}

		private bool WriteSlot20ColorPacked(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot20ColorPacked was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot20ColorPacked = value;
			_hasPendingSlot20ColorPacked = true;
			return true;
		}

		private bool WriteSlot20TargetPlayerId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot20TargetPlayerId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot20TargetPlayerId = value;
			_hasPendingSlot20TargetPlayerId = true;
			return true;
		}

		private bool WriteSlot21CardNetId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot21CardNetId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot21CardNetId = value;
			_hasPendingSlot21CardNetId = true;
			return true;
		}

		private bool WriteSlot21CardDataId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot21CardDataId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot21CardDataId = value;
			_hasPendingSlot21CardDataId = true;
			return true;
		}

		private bool WriteSlot21ColorPacked(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot21ColorPacked was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot21ColorPacked = value;
			_hasPendingSlot21ColorPacked = true;
			return true;
		}

		private bool WriteSlot21TargetPlayerId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot21TargetPlayerId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot21TargetPlayerId = value;
			_hasPendingSlot21TargetPlayerId = true;
			return true;
		}

		private bool WriteSlot22CardNetId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot22CardNetId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot22CardNetId = value;
			_hasPendingSlot22CardNetId = true;
			return true;
		}

		private bool WriteSlot22CardDataId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot22CardDataId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot22CardDataId = value;
			_hasPendingSlot22CardDataId = true;
			return true;
		}

		private bool WriteSlot22ColorPacked(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot22ColorPacked was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot22ColorPacked = value;
			_hasPendingSlot22ColorPacked = true;
			return true;
		}

		private bool WriteSlot22TargetPlayerId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot22TargetPlayerId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot22TargetPlayerId = value;
			_hasPendingSlot22TargetPlayerId = true;
			return true;
		}

		private bool WriteSlot23CardNetId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot23CardNetId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot23CardNetId = value;
			_hasPendingSlot23CardNetId = true;
			return true;
		}

		private bool WriteSlot23CardDataId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot23CardDataId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot23CardDataId = value;
			_hasPendingSlot23CardDataId = true;
			return true;
		}

		private bool WriteSlot23ColorPacked(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot23ColorPacked was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot23ColorPacked = value;
			_hasPendingSlot23ColorPacked = true;
			return true;
		}

		private bool WriteSlot23TargetPlayerId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot23TargetPlayerId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot23TargetPlayerId = value;
			_hasPendingSlot23TargetPlayerId = true;
			return true;
		}

		private bool WriteSlot24CardNetId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot24CardNetId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot24CardNetId = value;
			_hasPendingSlot24CardNetId = true;
			return true;
		}

		private bool WriteSlot24CardDataId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot24CardDataId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot24CardDataId = value;
			_hasPendingSlot24CardDataId = true;
			return true;
		}

		private bool WriteSlot24ColorPacked(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot24ColorPacked was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot24ColorPacked = value;
			_hasPendingSlot24ColorPacked = true;
			return true;
		}

		private bool WriteSlot24TargetPlayerId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot24TargetPlayerId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot24TargetPlayerId = value;
			_hasPendingSlot24TargetPlayerId = true;
			return true;
		}

		private bool WriteSlot25CardNetId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot25CardNetId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot25CardNetId = value;
			_hasPendingSlot25CardNetId = true;
			return true;
		}

		private bool WriteSlot25CardDataId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot25CardDataId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot25CardDataId = value;
			_hasPendingSlot25CardDataId = true;
			return true;
		}

		private bool WriteSlot25ColorPacked(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot25ColorPacked was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot25ColorPacked = value;
			_hasPendingSlot25ColorPacked = true;
			return true;
		}

		private bool WriteSlot25TargetPlayerId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot25TargetPlayerId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot25TargetPlayerId = value;
			_hasPendingSlot25TargetPlayerId = true;
			return true;
		}

		private bool WriteSlot26CardNetId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot26CardNetId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot26CardNetId = value;
			_hasPendingSlot26CardNetId = true;
			return true;
		}

		private bool WriteSlot26CardDataId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot26CardDataId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot26CardDataId = value;
			_hasPendingSlot26CardDataId = true;
			return true;
		}

		private bool WriteSlot26ColorPacked(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot26ColorPacked was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot26ColorPacked = value;
			_hasPendingSlot26ColorPacked = true;
			return true;
		}

		private bool WriteSlot26TargetPlayerId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot26TargetPlayerId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot26TargetPlayerId = value;
			_hasPendingSlot26TargetPlayerId = true;
			return true;
		}

		private bool WriteSlot27CardNetId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot27CardNetId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot27CardNetId = value;
			_hasPendingSlot27CardNetId = true;
			return true;
		}

		private bool WriteSlot27CardDataId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot27CardDataId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot27CardDataId = value;
			_hasPendingSlot27CardDataId = true;
			return true;
		}

		private bool WriteSlot27ColorPacked(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot27ColorPacked was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot27ColorPacked = value;
			_hasPendingSlot27ColorPacked = true;
			return true;
		}

		private bool WriteSlot27TargetPlayerId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot27TargetPlayerId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot27TargetPlayerId = value;
			_hasPendingSlot27TargetPlayerId = true;
			return true;
		}

		private bool WriteSlot28CardNetId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot28CardNetId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot28CardNetId = value;
			_hasPendingSlot28CardNetId = true;
			return true;
		}

		private bool WriteSlot28CardDataId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot28CardDataId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot28CardDataId = value;
			_hasPendingSlot28CardDataId = true;
			return true;
		}

		private bool WriteSlot28ColorPacked(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot28ColorPacked was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot28ColorPacked = value;
			_hasPendingSlot28ColorPacked = true;
			return true;
		}

		private bool WriteSlot28TargetPlayerId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot28TargetPlayerId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot28TargetPlayerId = value;
			_hasPendingSlot28TargetPlayerId = true;
			return true;
		}

		private bool WriteSlot29CardNetId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot29CardNetId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot29CardNetId = value;
			_hasPendingSlot29CardNetId = true;
			return true;
		}

		private bool WriteSlot29CardDataId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot29CardDataId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot29CardDataId = value;
			_hasPendingSlot29CardDataId = true;
			return true;
		}

		private bool WriteSlot29ColorPacked(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot29ColorPacked was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot29ColorPacked = value;
			_hasPendingSlot29ColorPacked = true;
			return true;
		}

		private bool WriteSlot29TargetPlayerId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot29TargetPlayerId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot29TargetPlayerId = value;
			_hasPendingSlot29TargetPlayerId = true;
			return true;
		}

		private bool WriteSlot30CardNetId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot30CardNetId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot30CardNetId = value;
			_hasPendingSlot30CardNetId = true;
			return true;
		}

		private bool WriteSlot30CardDataId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot30CardDataId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot30CardDataId = value;
			_hasPendingSlot30CardDataId = true;
			return true;
		}

		private bool WriteSlot30ColorPacked(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot30ColorPacked was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot30ColorPacked = value;
			_hasPendingSlot30ColorPacked = true;
			return true;
		}

		private bool WriteSlot30TargetPlayerId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot30TargetPlayerId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot30TargetPlayerId = value;
			_hasPendingSlot30TargetPlayerId = true;
			return true;
		}

		private bool WriteSlot31CardNetId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot31CardNetId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot31CardNetId = value;
			_hasPendingSlot31CardNetId = true;
			return true;
		}

		private bool WriteSlot31CardDataId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot31CardDataId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot31CardDataId = value;
			_hasPendingSlot31CardDataId = true;
			return true;
		}

		private bool WriteSlot31ColorPacked(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot31ColorPacked was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot31ColorPacked = value;
			_hasPendingSlot31ColorPacked = true;
			return true;
		}

		private bool WriteSlot31TargetPlayerId(int value)
		{
			if (!_pendingRewardModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PendingRewardModel.Slot31TargetPlayerId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot31TargetPlayerId = value;
			_hasPendingSlot31TargetPlayerId = true;
			return true;
		}

		private bool IsAuthorityMigratingToMaster()
		{
			if (_pendingRewardNetworkObject == null || _pendingRewardNetworkObject.Object == null || _pendingRewardNetworkObject.Runner == null)
			{
				return false;
			}
			if (_pendingRewardNetworkObject.Runner.IsSharedModeMasterClient)
			{
				return (_pendingRewardNetworkObject.Object.Flags & NetworkObjectFlags.MasterClientObject) != 0;
			}
			return false;
		}

		private void HandleAuthoritativeTick()
		{
			if (_hasPendingTargetLevelNumber)
			{
				_hasPendingTargetLevelNumber = false;
				_pendingRewardNetworkObject.TryWriteTargetLevelNumber(_pendingTargetLevelNumber);
			}
			if (_hasPendingSlot0CardNetId)
			{
				_hasPendingSlot0CardNetId = false;
				_pendingRewardNetworkObject.TryWriteSlot0CardNetId(_pendingSlot0CardNetId);
			}
			if (_hasPendingSlot0CardDataId)
			{
				_hasPendingSlot0CardDataId = false;
				_pendingRewardNetworkObject.TryWriteSlot0CardDataId(_pendingSlot0CardDataId);
			}
			if (_hasPendingSlot0ColorPacked)
			{
				_hasPendingSlot0ColorPacked = false;
				_pendingRewardNetworkObject.TryWriteSlot0ColorPacked(_pendingSlot0ColorPacked);
			}
			if (_hasPendingSlot0TargetPlayerId)
			{
				_hasPendingSlot0TargetPlayerId = false;
				_pendingRewardNetworkObject.TryWriteSlot0TargetPlayerId(_pendingSlot0TargetPlayerId);
			}
			if (_hasPendingSlot1CardNetId)
			{
				_hasPendingSlot1CardNetId = false;
				_pendingRewardNetworkObject.TryWriteSlot1CardNetId(_pendingSlot1CardNetId);
			}
			if (_hasPendingSlot1CardDataId)
			{
				_hasPendingSlot1CardDataId = false;
				_pendingRewardNetworkObject.TryWriteSlot1CardDataId(_pendingSlot1CardDataId);
			}
			if (_hasPendingSlot1ColorPacked)
			{
				_hasPendingSlot1ColorPacked = false;
				_pendingRewardNetworkObject.TryWriteSlot1ColorPacked(_pendingSlot1ColorPacked);
			}
			if (_hasPendingSlot1TargetPlayerId)
			{
				_hasPendingSlot1TargetPlayerId = false;
				_pendingRewardNetworkObject.TryWriteSlot1TargetPlayerId(_pendingSlot1TargetPlayerId);
			}
			if (_hasPendingSlot2CardNetId)
			{
				_hasPendingSlot2CardNetId = false;
				_pendingRewardNetworkObject.TryWriteSlot2CardNetId(_pendingSlot2CardNetId);
			}
			if (_hasPendingSlot2CardDataId)
			{
				_hasPendingSlot2CardDataId = false;
				_pendingRewardNetworkObject.TryWriteSlot2CardDataId(_pendingSlot2CardDataId);
			}
			if (_hasPendingSlot2ColorPacked)
			{
				_hasPendingSlot2ColorPacked = false;
				_pendingRewardNetworkObject.TryWriteSlot2ColorPacked(_pendingSlot2ColorPacked);
			}
			if (_hasPendingSlot2TargetPlayerId)
			{
				_hasPendingSlot2TargetPlayerId = false;
				_pendingRewardNetworkObject.TryWriteSlot2TargetPlayerId(_pendingSlot2TargetPlayerId);
			}
			if (_hasPendingSlot3CardNetId)
			{
				_hasPendingSlot3CardNetId = false;
				_pendingRewardNetworkObject.TryWriteSlot3CardNetId(_pendingSlot3CardNetId);
			}
			if (_hasPendingSlot3CardDataId)
			{
				_hasPendingSlot3CardDataId = false;
				_pendingRewardNetworkObject.TryWriteSlot3CardDataId(_pendingSlot3CardDataId);
			}
			if (_hasPendingSlot3ColorPacked)
			{
				_hasPendingSlot3ColorPacked = false;
				_pendingRewardNetworkObject.TryWriteSlot3ColorPacked(_pendingSlot3ColorPacked);
			}
			if (_hasPendingSlot3TargetPlayerId)
			{
				_hasPendingSlot3TargetPlayerId = false;
				_pendingRewardNetworkObject.TryWriteSlot3TargetPlayerId(_pendingSlot3TargetPlayerId);
			}
			if (_hasPendingSlot4CardNetId)
			{
				_hasPendingSlot4CardNetId = false;
				_pendingRewardNetworkObject.TryWriteSlot4CardNetId(_pendingSlot4CardNetId);
			}
			if (_hasPendingSlot4CardDataId)
			{
				_hasPendingSlot4CardDataId = false;
				_pendingRewardNetworkObject.TryWriteSlot4CardDataId(_pendingSlot4CardDataId);
			}
			if (_hasPendingSlot4ColorPacked)
			{
				_hasPendingSlot4ColorPacked = false;
				_pendingRewardNetworkObject.TryWriteSlot4ColorPacked(_pendingSlot4ColorPacked);
			}
			if (_hasPendingSlot4TargetPlayerId)
			{
				_hasPendingSlot4TargetPlayerId = false;
				_pendingRewardNetworkObject.TryWriteSlot4TargetPlayerId(_pendingSlot4TargetPlayerId);
			}
			if (_hasPendingSlot5CardNetId)
			{
				_hasPendingSlot5CardNetId = false;
				_pendingRewardNetworkObject.TryWriteSlot5CardNetId(_pendingSlot5CardNetId);
			}
			if (_hasPendingSlot5CardDataId)
			{
				_hasPendingSlot5CardDataId = false;
				_pendingRewardNetworkObject.TryWriteSlot5CardDataId(_pendingSlot5CardDataId);
			}
			if (_hasPendingSlot5ColorPacked)
			{
				_hasPendingSlot5ColorPacked = false;
				_pendingRewardNetworkObject.TryWriteSlot5ColorPacked(_pendingSlot5ColorPacked);
			}
			if (_hasPendingSlot5TargetPlayerId)
			{
				_hasPendingSlot5TargetPlayerId = false;
				_pendingRewardNetworkObject.TryWriteSlot5TargetPlayerId(_pendingSlot5TargetPlayerId);
			}
			if (_hasPendingSlot6CardNetId)
			{
				_hasPendingSlot6CardNetId = false;
				_pendingRewardNetworkObject.TryWriteSlot6CardNetId(_pendingSlot6CardNetId);
			}
			if (_hasPendingSlot6CardDataId)
			{
				_hasPendingSlot6CardDataId = false;
				_pendingRewardNetworkObject.TryWriteSlot6CardDataId(_pendingSlot6CardDataId);
			}
			if (_hasPendingSlot6ColorPacked)
			{
				_hasPendingSlot6ColorPacked = false;
				_pendingRewardNetworkObject.TryWriteSlot6ColorPacked(_pendingSlot6ColorPacked);
			}
			if (_hasPendingSlot6TargetPlayerId)
			{
				_hasPendingSlot6TargetPlayerId = false;
				_pendingRewardNetworkObject.TryWriteSlot6TargetPlayerId(_pendingSlot6TargetPlayerId);
			}
			if (_hasPendingSlot7CardNetId)
			{
				_hasPendingSlot7CardNetId = false;
				_pendingRewardNetworkObject.TryWriteSlot7CardNetId(_pendingSlot7CardNetId);
			}
			if (_hasPendingSlot7CardDataId)
			{
				_hasPendingSlot7CardDataId = false;
				_pendingRewardNetworkObject.TryWriteSlot7CardDataId(_pendingSlot7CardDataId);
			}
			if (_hasPendingSlot7ColorPacked)
			{
				_hasPendingSlot7ColorPacked = false;
				_pendingRewardNetworkObject.TryWriteSlot7ColorPacked(_pendingSlot7ColorPacked);
			}
			if (_hasPendingSlot7TargetPlayerId)
			{
				_hasPendingSlot7TargetPlayerId = false;
				_pendingRewardNetworkObject.TryWriteSlot7TargetPlayerId(_pendingSlot7TargetPlayerId);
			}
			if (_hasPendingSlot8CardNetId)
			{
				_hasPendingSlot8CardNetId = false;
				_pendingRewardNetworkObject.TryWriteSlot8CardNetId(_pendingSlot8CardNetId);
			}
			if (_hasPendingSlot8CardDataId)
			{
				_hasPendingSlot8CardDataId = false;
				_pendingRewardNetworkObject.TryWriteSlot8CardDataId(_pendingSlot8CardDataId);
			}
			if (_hasPendingSlot8ColorPacked)
			{
				_hasPendingSlot8ColorPacked = false;
				_pendingRewardNetworkObject.TryWriteSlot8ColorPacked(_pendingSlot8ColorPacked);
			}
			if (_hasPendingSlot8TargetPlayerId)
			{
				_hasPendingSlot8TargetPlayerId = false;
				_pendingRewardNetworkObject.TryWriteSlot8TargetPlayerId(_pendingSlot8TargetPlayerId);
			}
			if (_hasPendingSlot9CardNetId)
			{
				_hasPendingSlot9CardNetId = false;
				_pendingRewardNetworkObject.TryWriteSlot9CardNetId(_pendingSlot9CardNetId);
			}
			if (_hasPendingSlot9CardDataId)
			{
				_hasPendingSlot9CardDataId = false;
				_pendingRewardNetworkObject.TryWriteSlot9CardDataId(_pendingSlot9CardDataId);
			}
			if (_hasPendingSlot9ColorPacked)
			{
				_hasPendingSlot9ColorPacked = false;
				_pendingRewardNetworkObject.TryWriteSlot9ColorPacked(_pendingSlot9ColorPacked);
			}
			if (_hasPendingSlot9TargetPlayerId)
			{
				_hasPendingSlot9TargetPlayerId = false;
				_pendingRewardNetworkObject.TryWriteSlot9TargetPlayerId(_pendingSlot9TargetPlayerId);
			}
			if (_hasPendingSlot10CardNetId)
			{
				_hasPendingSlot10CardNetId = false;
				_pendingRewardNetworkObject.TryWriteSlot10CardNetId(_pendingSlot10CardNetId);
			}
			if (_hasPendingSlot10CardDataId)
			{
				_hasPendingSlot10CardDataId = false;
				_pendingRewardNetworkObject.TryWriteSlot10CardDataId(_pendingSlot10CardDataId);
			}
			if (_hasPendingSlot10ColorPacked)
			{
				_hasPendingSlot10ColorPacked = false;
				_pendingRewardNetworkObject.TryWriteSlot10ColorPacked(_pendingSlot10ColorPacked);
			}
			if (_hasPendingSlot10TargetPlayerId)
			{
				_hasPendingSlot10TargetPlayerId = false;
				_pendingRewardNetworkObject.TryWriteSlot10TargetPlayerId(_pendingSlot10TargetPlayerId);
			}
			if (_hasPendingSlot11CardNetId)
			{
				_hasPendingSlot11CardNetId = false;
				_pendingRewardNetworkObject.TryWriteSlot11CardNetId(_pendingSlot11CardNetId);
			}
			if (_hasPendingSlot11CardDataId)
			{
				_hasPendingSlot11CardDataId = false;
				_pendingRewardNetworkObject.TryWriteSlot11CardDataId(_pendingSlot11CardDataId);
			}
			if (_hasPendingSlot11ColorPacked)
			{
				_hasPendingSlot11ColorPacked = false;
				_pendingRewardNetworkObject.TryWriteSlot11ColorPacked(_pendingSlot11ColorPacked);
			}
			if (_hasPendingSlot11TargetPlayerId)
			{
				_hasPendingSlot11TargetPlayerId = false;
				_pendingRewardNetworkObject.TryWriteSlot11TargetPlayerId(_pendingSlot11TargetPlayerId);
			}
			if (_hasPendingSlot12CardNetId)
			{
				_hasPendingSlot12CardNetId = false;
				_pendingRewardNetworkObject.TryWriteSlot12CardNetId(_pendingSlot12CardNetId);
			}
			if (_hasPendingSlot12CardDataId)
			{
				_hasPendingSlot12CardDataId = false;
				_pendingRewardNetworkObject.TryWriteSlot12CardDataId(_pendingSlot12CardDataId);
			}
			if (_hasPendingSlot12ColorPacked)
			{
				_hasPendingSlot12ColorPacked = false;
				_pendingRewardNetworkObject.TryWriteSlot12ColorPacked(_pendingSlot12ColorPacked);
			}
			if (_hasPendingSlot12TargetPlayerId)
			{
				_hasPendingSlot12TargetPlayerId = false;
				_pendingRewardNetworkObject.TryWriteSlot12TargetPlayerId(_pendingSlot12TargetPlayerId);
			}
			if (_hasPendingSlot13CardNetId)
			{
				_hasPendingSlot13CardNetId = false;
				_pendingRewardNetworkObject.TryWriteSlot13CardNetId(_pendingSlot13CardNetId);
			}
			if (_hasPendingSlot13CardDataId)
			{
				_hasPendingSlot13CardDataId = false;
				_pendingRewardNetworkObject.TryWriteSlot13CardDataId(_pendingSlot13CardDataId);
			}
			if (_hasPendingSlot13ColorPacked)
			{
				_hasPendingSlot13ColorPacked = false;
				_pendingRewardNetworkObject.TryWriteSlot13ColorPacked(_pendingSlot13ColorPacked);
			}
			if (_hasPendingSlot13TargetPlayerId)
			{
				_hasPendingSlot13TargetPlayerId = false;
				_pendingRewardNetworkObject.TryWriteSlot13TargetPlayerId(_pendingSlot13TargetPlayerId);
			}
			if (_hasPendingSlot14CardNetId)
			{
				_hasPendingSlot14CardNetId = false;
				_pendingRewardNetworkObject.TryWriteSlot14CardNetId(_pendingSlot14CardNetId);
			}
			if (_hasPendingSlot14CardDataId)
			{
				_hasPendingSlot14CardDataId = false;
				_pendingRewardNetworkObject.TryWriteSlot14CardDataId(_pendingSlot14CardDataId);
			}
			if (_hasPendingSlot14ColorPacked)
			{
				_hasPendingSlot14ColorPacked = false;
				_pendingRewardNetworkObject.TryWriteSlot14ColorPacked(_pendingSlot14ColorPacked);
			}
			if (_hasPendingSlot14TargetPlayerId)
			{
				_hasPendingSlot14TargetPlayerId = false;
				_pendingRewardNetworkObject.TryWriteSlot14TargetPlayerId(_pendingSlot14TargetPlayerId);
			}
			if (_hasPendingSlot15CardNetId)
			{
				_hasPendingSlot15CardNetId = false;
				_pendingRewardNetworkObject.TryWriteSlot15CardNetId(_pendingSlot15CardNetId);
			}
			if (_hasPendingSlot15CardDataId)
			{
				_hasPendingSlot15CardDataId = false;
				_pendingRewardNetworkObject.TryWriteSlot15CardDataId(_pendingSlot15CardDataId);
			}
			if (_hasPendingSlot15ColorPacked)
			{
				_hasPendingSlot15ColorPacked = false;
				_pendingRewardNetworkObject.TryWriteSlot15ColorPacked(_pendingSlot15ColorPacked);
			}
			if (_hasPendingSlot15TargetPlayerId)
			{
				_hasPendingSlot15TargetPlayerId = false;
				_pendingRewardNetworkObject.TryWriteSlot15TargetPlayerId(_pendingSlot15TargetPlayerId);
			}
			if (_hasPendingSlot16CardNetId)
			{
				_hasPendingSlot16CardNetId = false;
				_pendingRewardNetworkObject.TryWriteSlot16CardNetId(_pendingSlot16CardNetId);
			}
			if (_hasPendingSlot16CardDataId)
			{
				_hasPendingSlot16CardDataId = false;
				_pendingRewardNetworkObject.TryWriteSlot16CardDataId(_pendingSlot16CardDataId);
			}
			if (_hasPendingSlot16ColorPacked)
			{
				_hasPendingSlot16ColorPacked = false;
				_pendingRewardNetworkObject.TryWriteSlot16ColorPacked(_pendingSlot16ColorPacked);
			}
			if (_hasPendingSlot16TargetPlayerId)
			{
				_hasPendingSlot16TargetPlayerId = false;
				_pendingRewardNetworkObject.TryWriteSlot16TargetPlayerId(_pendingSlot16TargetPlayerId);
			}
			if (_hasPendingSlot17CardNetId)
			{
				_hasPendingSlot17CardNetId = false;
				_pendingRewardNetworkObject.TryWriteSlot17CardNetId(_pendingSlot17CardNetId);
			}
			if (_hasPendingSlot17CardDataId)
			{
				_hasPendingSlot17CardDataId = false;
				_pendingRewardNetworkObject.TryWriteSlot17CardDataId(_pendingSlot17CardDataId);
			}
			if (_hasPendingSlot17ColorPacked)
			{
				_hasPendingSlot17ColorPacked = false;
				_pendingRewardNetworkObject.TryWriteSlot17ColorPacked(_pendingSlot17ColorPacked);
			}
			if (_hasPendingSlot17TargetPlayerId)
			{
				_hasPendingSlot17TargetPlayerId = false;
				_pendingRewardNetworkObject.TryWriteSlot17TargetPlayerId(_pendingSlot17TargetPlayerId);
			}
			if (_hasPendingSlot18CardNetId)
			{
				_hasPendingSlot18CardNetId = false;
				_pendingRewardNetworkObject.TryWriteSlot18CardNetId(_pendingSlot18CardNetId);
			}
			if (_hasPendingSlot18CardDataId)
			{
				_hasPendingSlot18CardDataId = false;
				_pendingRewardNetworkObject.TryWriteSlot18CardDataId(_pendingSlot18CardDataId);
			}
			if (_hasPendingSlot18ColorPacked)
			{
				_hasPendingSlot18ColorPacked = false;
				_pendingRewardNetworkObject.TryWriteSlot18ColorPacked(_pendingSlot18ColorPacked);
			}
			if (_hasPendingSlot18TargetPlayerId)
			{
				_hasPendingSlot18TargetPlayerId = false;
				_pendingRewardNetworkObject.TryWriteSlot18TargetPlayerId(_pendingSlot18TargetPlayerId);
			}
			if (_hasPendingSlot19CardNetId)
			{
				_hasPendingSlot19CardNetId = false;
				_pendingRewardNetworkObject.TryWriteSlot19CardNetId(_pendingSlot19CardNetId);
			}
			if (_hasPendingSlot19CardDataId)
			{
				_hasPendingSlot19CardDataId = false;
				_pendingRewardNetworkObject.TryWriteSlot19CardDataId(_pendingSlot19CardDataId);
			}
			if (_hasPendingSlot19ColorPacked)
			{
				_hasPendingSlot19ColorPacked = false;
				_pendingRewardNetworkObject.TryWriteSlot19ColorPacked(_pendingSlot19ColorPacked);
			}
			if (_hasPendingSlot19TargetPlayerId)
			{
				_hasPendingSlot19TargetPlayerId = false;
				_pendingRewardNetworkObject.TryWriteSlot19TargetPlayerId(_pendingSlot19TargetPlayerId);
			}
			if (_hasPendingSlot20CardNetId)
			{
				_hasPendingSlot20CardNetId = false;
				_pendingRewardNetworkObject.TryWriteSlot20CardNetId(_pendingSlot20CardNetId);
			}
			if (_hasPendingSlot20CardDataId)
			{
				_hasPendingSlot20CardDataId = false;
				_pendingRewardNetworkObject.TryWriteSlot20CardDataId(_pendingSlot20CardDataId);
			}
			if (_hasPendingSlot20ColorPacked)
			{
				_hasPendingSlot20ColorPacked = false;
				_pendingRewardNetworkObject.TryWriteSlot20ColorPacked(_pendingSlot20ColorPacked);
			}
			if (_hasPendingSlot20TargetPlayerId)
			{
				_hasPendingSlot20TargetPlayerId = false;
				_pendingRewardNetworkObject.TryWriteSlot20TargetPlayerId(_pendingSlot20TargetPlayerId);
			}
			if (_hasPendingSlot21CardNetId)
			{
				_hasPendingSlot21CardNetId = false;
				_pendingRewardNetworkObject.TryWriteSlot21CardNetId(_pendingSlot21CardNetId);
			}
			if (_hasPendingSlot21CardDataId)
			{
				_hasPendingSlot21CardDataId = false;
				_pendingRewardNetworkObject.TryWriteSlot21CardDataId(_pendingSlot21CardDataId);
			}
			if (_hasPendingSlot21ColorPacked)
			{
				_hasPendingSlot21ColorPacked = false;
				_pendingRewardNetworkObject.TryWriteSlot21ColorPacked(_pendingSlot21ColorPacked);
			}
			if (_hasPendingSlot21TargetPlayerId)
			{
				_hasPendingSlot21TargetPlayerId = false;
				_pendingRewardNetworkObject.TryWriteSlot21TargetPlayerId(_pendingSlot21TargetPlayerId);
			}
			if (_hasPendingSlot22CardNetId)
			{
				_hasPendingSlot22CardNetId = false;
				_pendingRewardNetworkObject.TryWriteSlot22CardNetId(_pendingSlot22CardNetId);
			}
			if (_hasPendingSlot22CardDataId)
			{
				_hasPendingSlot22CardDataId = false;
				_pendingRewardNetworkObject.TryWriteSlot22CardDataId(_pendingSlot22CardDataId);
			}
			if (_hasPendingSlot22ColorPacked)
			{
				_hasPendingSlot22ColorPacked = false;
				_pendingRewardNetworkObject.TryWriteSlot22ColorPacked(_pendingSlot22ColorPacked);
			}
			if (_hasPendingSlot22TargetPlayerId)
			{
				_hasPendingSlot22TargetPlayerId = false;
				_pendingRewardNetworkObject.TryWriteSlot22TargetPlayerId(_pendingSlot22TargetPlayerId);
			}
			if (_hasPendingSlot23CardNetId)
			{
				_hasPendingSlot23CardNetId = false;
				_pendingRewardNetworkObject.TryWriteSlot23CardNetId(_pendingSlot23CardNetId);
			}
			if (_hasPendingSlot23CardDataId)
			{
				_hasPendingSlot23CardDataId = false;
				_pendingRewardNetworkObject.TryWriteSlot23CardDataId(_pendingSlot23CardDataId);
			}
			if (_hasPendingSlot23ColorPacked)
			{
				_hasPendingSlot23ColorPacked = false;
				_pendingRewardNetworkObject.TryWriteSlot23ColorPacked(_pendingSlot23ColorPacked);
			}
			if (_hasPendingSlot23TargetPlayerId)
			{
				_hasPendingSlot23TargetPlayerId = false;
				_pendingRewardNetworkObject.TryWriteSlot23TargetPlayerId(_pendingSlot23TargetPlayerId);
			}
			if (_hasPendingSlot24CardNetId)
			{
				_hasPendingSlot24CardNetId = false;
				_pendingRewardNetworkObject.TryWriteSlot24CardNetId(_pendingSlot24CardNetId);
			}
			if (_hasPendingSlot24CardDataId)
			{
				_hasPendingSlot24CardDataId = false;
				_pendingRewardNetworkObject.TryWriteSlot24CardDataId(_pendingSlot24CardDataId);
			}
			if (_hasPendingSlot24ColorPacked)
			{
				_hasPendingSlot24ColorPacked = false;
				_pendingRewardNetworkObject.TryWriteSlot24ColorPacked(_pendingSlot24ColorPacked);
			}
			if (_hasPendingSlot24TargetPlayerId)
			{
				_hasPendingSlot24TargetPlayerId = false;
				_pendingRewardNetworkObject.TryWriteSlot24TargetPlayerId(_pendingSlot24TargetPlayerId);
			}
			if (_hasPendingSlot25CardNetId)
			{
				_hasPendingSlot25CardNetId = false;
				_pendingRewardNetworkObject.TryWriteSlot25CardNetId(_pendingSlot25CardNetId);
			}
			if (_hasPendingSlot25CardDataId)
			{
				_hasPendingSlot25CardDataId = false;
				_pendingRewardNetworkObject.TryWriteSlot25CardDataId(_pendingSlot25CardDataId);
			}
			if (_hasPendingSlot25ColorPacked)
			{
				_hasPendingSlot25ColorPacked = false;
				_pendingRewardNetworkObject.TryWriteSlot25ColorPacked(_pendingSlot25ColorPacked);
			}
			if (_hasPendingSlot25TargetPlayerId)
			{
				_hasPendingSlot25TargetPlayerId = false;
				_pendingRewardNetworkObject.TryWriteSlot25TargetPlayerId(_pendingSlot25TargetPlayerId);
			}
			if (_hasPendingSlot26CardNetId)
			{
				_hasPendingSlot26CardNetId = false;
				_pendingRewardNetworkObject.TryWriteSlot26CardNetId(_pendingSlot26CardNetId);
			}
			if (_hasPendingSlot26CardDataId)
			{
				_hasPendingSlot26CardDataId = false;
				_pendingRewardNetworkObject.TryWriteSlot26CardDataId(_pendingSlot26CardDataId);
			}
			if (_hasPendingSlot26ColorPacked)
			{
				_hasPendingSlot26ColorPacked = false;
				_pendingRewardNetworkObject.TryWriteSlot26ColorPacked(_pendingSlot26ColorPacked);
			}
			if (_hasPendingSlot26TargetPlayerId)
			{
				_hasPendingSlot26TargetPlayerId = false;
				_pendingRewardNetworkObject.TryWriteSlot26TargetPlayerId(_pendingSlot26TargetPlayerId);
			}
			if (_hasPendingSlot27CardNetId)
			{
				_hasPendingSlot27CardNetId = false;
				_pendingRewardNetworkObject.TryWriteSlot27CardNetId(_pendingSlot27CardNetId);
			}
			if (_hasPendingSlot27CardDataId)
			{
				_hasPendingSlot27CardDataId = false;
				_pendingRewardNetworkObject.TryWriteSlot27CardDataId(_pendingSlot27CardDataId);
			}
			if (_hasPendingSlot27ColorPacked)
			{
				_hasPendingSlot27ColorPacked = false;
				_pendingRewardNetworkObject.TryWriteSlot27ColorPacked(_pendingSlot27ColorPacked);
			}
			if (_hasPendingSlot27TargetPlayerId)
			{
				_hasPendingSlot27TargetPlayerId = false;
				_pendingRewardNetworkObject.TryWriteSlot27TargetPlayerId(_pendingSlot27TargetPlayerId);
			}
			if (_hasPendingSlot28CardNetId)
			{
				_hasPendingSlot28CardNetId = false;
				_pendingRewardNetworkObject.TryWriteSlot28CardNetId(_pendingSlot28CardNetId);
			}
			if (_hasPendingSlot28CardDataId)
			{
				_hasPendingSlot28CardDataId = false;
				_pendingRewardNetworkObject.TryWriteSlot28CardDataId(_pendingSlot28CardDataId);
			}
			if (_hasPendingSlot28ColorPacked)
			{
				_hasPendingSlot28ColorPacked = false;
				_pendingRewardNetworkObject.TryWriteSlot28ColorPacked(_pendingSlot28ColorPacked);
			}
			if (_hasPendingSlot28TargetPlayerId)
			{
				_hasPendingSlot28TargetPlayerId = false;
				_pendingRewardNetworkObject.TryWriteSlot28TargetPlayerId(_pendingSlot28TargetPlayerId);
			}
			if (_hasPendingSlot29CardNetId)
			{
				_hasPendingSlot29CardNetId = false;
				_pendingRewardNetworkObject.TryWriteSlot29CardNetId(_pendingSlot29CardNetId);
			}
			if (_hasPendingSlot29CardDataId)
			{
				_hasPendingSlot29CardDataId = false;
				_pendingRewardNetworkObject.TryWriteSlot29CardDataId(_pendingSlot29CardDataId);
			}
			if (_hasPendingSlot29ColorPacked)
			{
				_hasPendingSlot29ColorPacked = false;
				_pendingRewardNetworkObject.TryWriteSlot29ColorPacked(_pendingSlot29ColorPacked);
			}
			if (_hasPendingSlot29TargetPlayerId)
			{
				_hasPendingSlot29TargetPlayerId = false;
				_pendingRewardNetworkObject.TryWriteSlot29TargetPlayerId(_pendingSlot29TargetPlayerId);
			}
			if (_hasPendingSlot30CardNetId)
			{
				_hasPendingSlot30CardNetId = false;
				_pendingRewardNetworkObject.TryWriteSlot30CardNetId(_pendingSlot30CardNetId);
			}
			if (_hasPendingSlot30CardDataId)
			{
				_hasPendingSlot30CardDataId = false;
				_pendingRewardNetworkObject.TryWriteSlot30CardDataId(_pendingSlot30CardDataId);
			}
			if (_hasPendingSlot30ColorPacked)
			{
				_hasPendingSlot30ColorPacked = false;
				_pendingRewardNetworkObject.TryWriteSlot30ColorPacked(_pendingSlot30ColorPacked);
			}
			if (_hasPendingSlot30TargetPlayerId)
			{
				_hasPendingSlot30TargetPlayerId = false;
				_pendingRewardNetworkObject.TryWriteSlot30TargetPlayerId(_pendingSlot30TargetPlayerId);
			}
			if (_hasPendingSlot31CardNetId)
			{
				_hasPendingSlot31CardNetId = false;
				_pendingRewardNetworkObject.TryWriteSlot31CardNetId(_pendingSlot31CardNetId);
			}
			if (_hasPendingSlot31CardDataId)
			{
				_hasPendingSlot31CardDataId = false;
				_pendingRewardNetworkObject.TryWriteSlot31CardDataId(_pendingSlot31CardDataId);
			}
			if (_hasPendingSlot31ColorPacked)
			{
				_hasPendingSlot31ColorPacked = false;
				_pendingRewardNetworkObject.TryWriteSlot31ColorPacked(_pendingSlot31ColorPacked);
			}
			if (_hasPendingSlot31TargetPlayerId)
			{
				_hasPendingSlot31TargetPlayerId = false;
				_pendingRewardNetworkObject.TryWriteSlot31TargetPlayerId(_pendingSlot31TargetPlayerId);
			}
		}
	}
}
