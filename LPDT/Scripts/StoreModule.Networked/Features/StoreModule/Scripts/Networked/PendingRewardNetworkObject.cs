using System;
using Features.NetworkedModelRuntime;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.StoreModule.Scripts.Networked
{
	[NetworkBehaviourWeaved(129)]
	public class PendingRewardNetworkObject : NetworkBehaviour
	{
		private INetworkedModelInstanceProvider _networkedModelInstanceProvider;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("TargetLevelNumber", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _TargetLevelNumber;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot0CardNetId", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot0CardNetId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot0CardDataId", 2, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot0CardDataId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot0ColorPacked", 3, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot0ColorPacked;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot0TargetPlayerId", 4, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot0TargetPlayerId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot1CardNetId", 5, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot1CardNetId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot1CardDataId", 6, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot1CardDataId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot1ColorPacked", 7, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot1ColorPacked;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot1TargetPlayerId", 8, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot1TargetPlayerId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot2CardNetId", 9, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot2CardNetId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot2CardDataId", 10, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot2CardDataId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot2ColorPacked", 11, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot2ColorPacked;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot2TargetPlayerId", 12, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot2TargetPlayerId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot3CardNetId", 13, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot3CardNetId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot3CardDataId", 14, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot3CardDataId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot3ColorPacked", 15, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot3ColorPacked;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot3TargetPlayerId", 16, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot3TargetPlayerId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot4CardNetId", 17, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot4CardNetId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot4CardDataId", 18, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot4CardDataId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot4ColorPacked", 19, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot4ColorPacked;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot4TargetPlayerId", 20, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot4TargetPlayerId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot5CardNetId", 21, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot5CardNetId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot5CardDataId", 22, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot5CardDataId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot5ColorPacked", 23, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot5ColorPacked;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot5TargetPlayerId", 24, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot5TargetPlayerId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot6CardNetId", 25, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot6CardNetId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot6CardDataId", 26, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot6CardDataId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot6ColorPacked", 27, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot6ColorPacked;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot6TargetPlayerId", 28, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot6TargetPlayerId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot7CardNetId", 29, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot7CardNetId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot7CardDataId", 30, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot7CardDataId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot7ColorPacked", 31, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot7ColorPacked;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot7TargetPlayerId", 32, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot7TargetPlayerId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot8CardNetId", 33, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot8CardNetId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot8CardDataId", 34, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot8CardDataId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot8ColorPacked", 35, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot8ColorPacked;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot8TargetPlayerId", 36, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot8TargetPlayerId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot9CardNetId", 37, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot9CardNetId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot9CardDataId", 38, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot9CardDataId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot9ColorPacked", 39, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot9ColorPacked;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot9TargetPlayerId", 40, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot9TargetPlayerId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot10CardNetId", 41, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot10CardNetId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot10CardDataId", 42, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot10CardDataId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot10ColorPacked", 43, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot10ColorPacked;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot10TargetPlayerId", 44, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot10TargetPlayerId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot11CardNetId", 45, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot11CardNetId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot11CardDataId", 46, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot11CardDataId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot11ColorPacked", 47, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot11ColorPacked;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot11TargetPlayerId", 48, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot11TargetPlayerId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot12CardNetId", 49, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot12CardNetId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot12CardDataId", 50, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot12CardDataId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot12ColorPacked", 51, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot12ColorPacked;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot12TargetPlayerId", 52, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot12TargetPlayerId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot13CardNetId", 53, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot13CardNetId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot13CardDataId", 54, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot13CardDataId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot13ColorPacked", 55, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot13ColorPacked;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot13TargetPlayerId", 56, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot13TargetPlayerId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot14CardNetId", 57, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot14CardNetId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot14CardDataId", 58, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot14CardDataId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot14ColorPacked", 59, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot14ColorPacked;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot14TargetPlayerId", 60, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot14TargetPlayerId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot15CardNetId", 61, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot15CardNetId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot15CardDataId", 62, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot15CardDataId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot15ColorPacked", 63, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot15ColorPacked;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot15TargetPlayerId", 64, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot15TargetPlayerId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot16CardNetId", 65, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot16CardNetId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot16CardDataId", 66, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot16CardDataId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot16ColorPacked", 67, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot16ColorPacked;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot16TargetPlayerId", 68, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot16TargetPlayerId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot17CardNetId", 69, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot17CardNetId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot17CardDataId", 70, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot17CardDataId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot17ColorPacked", 71, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot17ColorPacked;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot17TargetPlayerId", 72, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot17TargetPlayerId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot18CardNetId", 73, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot18CardNetId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot18CardDataId", 74, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot18CardDataId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot18ColorPacked", 75, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot18ColorPacked;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot18TargetPlayerId", 76, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot18TargetPlayerId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot19CardNetId", 77, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot19CardNetId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot19CardDataId", 78, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot19CardDataId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot19ColorPacked", 79, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot19ColorPacked;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot19TargetPlayerId", 80, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot19TargetPlayerId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot20CardNetId", 81, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot20CardNetId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot20CardDataId", 82, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot20CardDataId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot20ColorPacked", 83, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot20ColorPacked;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot20TargetPlayerId", 84, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot20TargetPlayerId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot21CardNetId", 85, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot21CardNetId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot21CardDataId", 86, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot21CardDataId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot21ColorPacked", 87, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot21ColorPacked;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot21TargetPlayerId", 88, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot21TargetPlayerId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot22CardNetId", 89, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot22CardNetId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot22CardDataId", 90, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot22CardDataId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot22ColorPacked", 91, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot22ColorPacked;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot22TargetPlayerId", 92, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot22TargetPlayerId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot23CardNetId", 93, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot23CardNetId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot23CardDataId", 94, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot23CardDataId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot23ColorPacked", 95, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot23ColorPacked;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot23TargetPlayerId", 96, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot23TargetPlayerId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot24CardNetId", 97, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot24CardNetId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot24CardDataId", 98, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot24CardDataId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot24ColorPacked", 99, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot24ColorPacked;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot24TargetPlayerId", 100, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot24TargetPlayerId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot25CardNetId", 101, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot25CardNetId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot25CardDataId", 102, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot25CardDataId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot25ColorPacked", 103, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot25ColorPacked;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot25TargetPlayerId", 104, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot25TargetPlayerId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot26CardNetId", 105, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot26CardNetId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot26CardDataId", 106, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot26CardDataId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot26ColorPacked", 107, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot26ColorPacked;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot26TargetPlayerId", 108, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot26TargetPlayerId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot27CardNetId", 109, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot27CardNetId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot27CardDataId", 110, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot27CardDataId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot27ColorPacked", 111, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot27ColorPacked;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot27TargetPlayerId", 112, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot27TargetPlayerId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot28CardNetId", 113, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot28CardNetId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot28CardDataId", 114, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot28CardDataId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot28ColorPacked", 115, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot28ColorPacked;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot28TargetPlayerId", 116, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot28TargetPlayerId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot29CardNetId", 117, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot29CardNetId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot29CardDataId", 118, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot29CardDataId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot29ColorPacked", 119, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot29ColorPacked;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot29TargetPlayerId", 120, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot29TargetPlayerId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot30CardNetId", 121, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot30CardNetId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot30CardDataId", 122, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot30CardDataId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot30ColorPacked", 123, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot30ColorPacked;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot30TargetPlayerId", 124, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot30TargetPlayerId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot31CardNetId", 125, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot31CardNetId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot31CardDataId", 126, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot31CardDataId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot31ColorPacked", 127, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot31ColorPacked;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot31TargetPlayerId", 128, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot31TargetPlayerId;

		[Networked]
		[OnChangedRender("OnTargetLevelNumberChangedRender")]
		[NetworkedWeaved(0, 1)]
		public unsafe int TargetLevelNumber
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.TargetLevelNumber. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(int*)((byte*)Ptr + 0);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.TargetLevelNumber. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(int*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot0CardNetIdChangedRender")]
		[NetworkedWeaved(1, 1)]
		public unsafe int Slot0CardNetId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot0CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[1];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot0CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[1] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot0CardDataIdChangedRender")]
		[NetworkedWeaved(2, 1)]
		public unsafe int Slot0CardDataId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot0CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[2];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot0CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[2] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot0ColorPackedChangedRender")]
		[NetworkedWeaved(3, 1)]
		public unsafe int Slot0ColorPacked
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot0ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[3];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot0ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[3] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot0TargetPlayerIdChangedRender")]
		[NetworkedWeaved(4, 1)]
		public unsafe int Slot0TargetPlayerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot0TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[4];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot0TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[4] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot1CardNetIdChangedRender")]
		[NetworkedWeaved(5, 1)]
		public unsafe int Slot1CardNetId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot1CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[5];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot1CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[5] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot1CardDataIdChangedRender")]
		[NetworkedWeaved(6, 1)]
		public unsafe int Slot1CardDataId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot1CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[6];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot1CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[6] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot1ColorPackedChangedRender")]
		[NetworkedWeaved(7, 1)]
		public unsafe int Slot1ColorPacked
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot1ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[7];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot1ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[7] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot1TargetPlayerIdChangedRender")]
		[NetworkedWeaved(8, 1)]
		public unsafe int Slot1TargetPlayerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot1TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[8];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot1TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[8] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot2CardNetIdChangedRender")]
		[NetworkedWeaved(9, 1)]
		public unsafe int Slot2CardNetId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot2CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[9];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot2CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[9] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot2CardDataIdChangedRender")]
		[NetworkedWeaved(10, 1)]
		public unsafe int Slot2CardDataId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot2CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[10];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot2CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[10] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot2ColorPackedChangedRender")]
		[NetworkedWeaved(11, 1)]
		public unsafe int Slot2ColorPacked
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot2ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[11];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot2ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[11] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot2TargetPlayerIdChangedRender")]
		[NetworkedWeaved(12, 1)]
		public unsafe int Slot2TargetPlayerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot2TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[12];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot2TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[12] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot3CardNetIdChangedRender")]
		[NetworkedWeaved(13, 1)]
		public unsafe int Slot3CardNetId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot3CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[13];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot3CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[13] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot3CardDataIdChangedRender")]
		[NetworkedWeaved(14, 1)]
		public unsafe int Slot3CardDataId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot3CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[14];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot3CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[14] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot3ColorPackedChangedRender")]
		[NetworkedWeaved(15, 1)]
		public unsafe int Slot3ColorPacked
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot3ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[15];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot3ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[15] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot3TargetPlayerIdChangedRender")]
		[NetworkedWeaved(16, 1)]
		public unsafe int Slot3TargetPlayerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot3TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[16];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot3TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[16] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot4CardNetIdChangedRender")]
		[NetworkedWeaved(17, 1)]
		public unsafe int Slot4CardNetId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot4CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[17];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot4CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[17] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot4CardDataIdChangedRender")]
		[NetworkedWeaved(18, 1)]
		public unsafe int Slot4CardDataId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot4CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[18];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot4CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[18] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot4ColorPackedChangedRender")]
		[NetworkedWeaved(19, 1)]
		public unsafe int Slot4ColorPacked
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot4ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[19];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot4ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[19] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot4TargetPlayerIdChangedRender")]
		[NetworkedWeaved(20, 1)]
		public unsafe int Slot4TargetPlayerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot4TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[20];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot4TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[20] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot5CardNetIdChangedRender")]
		[NetworkedWeaved(21, 1)]
		public unsafe int Slot5CardNetId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot5CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[21];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot5CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[21] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot5CardDataIdChangedRender")]
		[NetworkedWeaved(22, 1)]
		public unsafe int Slot5CardDataId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot5CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[22];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot5CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[22] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot5ColorPackedChangedRender")]
		[NetworkedWeaved(23, 1)]
		public unsafe int Slot5ColorPacked
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot5ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[23];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot5ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[23] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot5TargetPlayerIdChangedRender")]
		[NetworkedWeaved(24, 1)]
		public unsafe int Slot5TargetPlayerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot5TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[24];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot5TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[24] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot6CardNetIdChangedRender")]
		[NetworkedWeaved(25, 1)]
		public unsafe int Slot6CardNetId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot6CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[25];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot6CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[25] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot6CardDataIdChangedRender")]
		[NetworkedWeaved(26, 1)]
		public unsafe int Slot6CardDataId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot6CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[26];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot6CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[26] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot6ColorPackedChangedRender")]
		[NetworkedWeaved(27, 1)]
		public unsafe int Slot6ColorPacked
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot6ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[27];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot6ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[27] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot6TargetPlayerIdChangedRender")]
		[NetworkedWeaved(28, 1)]
		public unsafe int Slot6TargetPlayerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot6TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[28];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot6TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[28] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot7CardNetIdChangedRender")]
		[NetworkedWeaved(29, 1)]
		public unsafe int Slot7CardNetId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot7CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[29];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot7CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[29] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot7CardDataIdChangedRender")]
		[NetworkedWeaved(30, 1)]
		public unsafe int Slot7CardDataId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot7CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[30];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot7CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[30] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot7ColorPackedChangedRender")]
		[NetworkedWeaved(31, 1)]
		public unsafe int Slot7ColorPacked
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot7ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[31];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot7ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[31] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot7TargetPlayerIdChangedRender")]
		[NetworkedWeaved(32, 1)]
		public unsafe int Slot7TargetPlayerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot7TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[32];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot7TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[32] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot8CardNetIdChangedRender")]
		[NetworkedWeaved(33, 1)]
		public unsafe int Slot8CardNetId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot8CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[33];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot8CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[33] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot8CardDataIdChangedRender")]
		[NetworkedWeaved(34, 1)]
		public unsafe int Slot8CardDataId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot8CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[34];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot8CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[34] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot8ColorPackedChangedRender")]
		[NetworkedWeaved(35, 1)]
		public unsafe int Slot8ColorPacked
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot8ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[35];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot8ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[35] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot8TargetPlayerIdChangedRender")]
		[NetworkedWeaved(36, 1)]
		public unsafe int Slot8TargetPlayerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot8TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[36];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot8TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[36] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot9CardNetIdChangedRender")]
		[NetworkedWeaved(37, 1)]
		public unsafe int Slot9CardNetId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot9CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[37];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot9CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[37] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot9CardDataIdChangedRender")]
		[NetworkedWeaved(38, 1)]
		public unsafe int Slot9CardDataId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot9CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[38];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot9CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[38] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot9ColorPackedChangedRender")]
		[NetworkedWeaved(39, 1)]
		public unsafe int Slot9ColorPacked
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot9ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[39];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot9ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[39] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot9TargetPlayerIdChangedRender")]
		[NetworkedWeaved(40, 1)]
		public unsafe int Slot9TargetPlayerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot9TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[40];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot9TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[40] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot10CardNetIdChangedRender")]
		[NetworkedWeaved(41, 1)]
		public unsafe int Slot10CardNetId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot10CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[41];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot10CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[41] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot10CardDataIdChangedRender")]
		[NetworkedWeaved(42, 1)]
		public unsafe int Slot10CardDataId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot10CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[42];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot10CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[42] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot10ColorPackedChangedRender")]
		[NetworkedWeaved(43, 1)]
		public unsafe int Slot10ColorPacked
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot10ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[43];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot10ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[43] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot10TargetPlayerIdChangedRender")]
		[NetworkedWeaved(44, 1)]
		public unsafe int Slot10TargetPlayerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot10TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[44];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot10TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[44] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot11CardNetIdChangedRender")]
		[NetworkedWeaved(45, 1)]
		public unsafe int Slot11CardNetId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot11CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[45];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot11CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[45] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot11CardDataIdChangedRender")]
		[NetworkedWeaved(46, 1)]
		public unsafe int Slot11CardDataId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot11CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[46];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot11CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[46] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot11ColorPackedChangedRender")]
		[NetworkedWeaved(47, 1)]
		public unsafe int Slot11ColorPacked
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot11ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[47];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot11ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[47] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot11TargetPlayerIdChangedRender")]
		[NetworkedWeaved(48, 1)]
		public unsafe int Slot11TargetPlayerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot11TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[48];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot11TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[48] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot12CardNetIdChangedRender")]
		[NetworkedWeaved(49, 1)]
		public unsafe int Slot12CardNetId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot12CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[49];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot12CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[49] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot12CardDataIdChangedRender")]
		[NetworkedWeaved(50, 1)]
		public unsafe int Slot12CardDataId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot12CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[50];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot12CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[50] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot12ColorPackedChangedRender")]
		[NetworkedWeaved(51, 1)]
		public unsafe int Slot12ColorPacked
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot12ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[51];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot12ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[51] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot12TargetPlayerIdChangedRender")]
		[NetworkedWeaved(52, 1)]
		public unsafe int Slot12TargetPlayerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot12TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[52];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot12TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[52] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot13CardNetIdChangedRender")]
		[NetworkedWeaved(53, 1)]
		public unsafe int Slot13CardNetId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot13CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[53];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot13CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[53] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot13CardDataIdChangedRender")]
		[NetworkedWeaved(54, 1)]
		public unsafe int Slot13CardDataId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot13CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[54];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot13CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[54] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot13ColorPackedChangedRender")]
		[NetworkedWeaved(55, 1)]
		public unsafe int Slot13ColorPacked
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot13ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[55];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot13ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[55] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot13TargetPlayerIdChangedRender")]
		[NetworkedWeaved(56, 1)]
		public unsafe int Slot13TargetPlayerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot13TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[56];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot13TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[56] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot14CardNetIdChangedRender")]
		[NetworkedWeaved(57, 1)]
		public unsafe int Slot14CardNetId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot14CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[57];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot14CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[57] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot14CardDataIdChangedRender")]
		[NetworkedWeaved(58, 1)]
		public unsafe int Slot14CardDataId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot14CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[58];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot14CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[58] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot14ColorPackedChangedRender")]
		[NetworkedWeaved(59, 1)]
		public unsafe int Slot14ColorPacked
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot14ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[59];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot14ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[59] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot14TargetPlayerIdChangedRender")]
		[NetworkedWeaved(60, 1)]
		public unsafe int Slot14TargetPlayerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot14TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[60];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot14TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[60] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot15CardNetIdChangedRender")]
		[NetworkedWeaved(61, 1)]
		public unsafe int Slot15CardNetId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot15CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[61];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot15CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[61] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot15CardDataIdChangedRender")]
		[NetworkedWeaved(62, 1)]
		public unsafe int Slot15CardDataId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot15CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[62];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot15CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[62] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot15ColorPackedChangedRender")]
		[NetworkedWeaved(63, 1)]
		public unsafe int Slot15ColorPacked
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot15ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[63];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot15ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[63] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot15TargetPlayerIdChangedRender")]
		[NetworkedWeaved(64, 1)]
		public unsafe int Slot15TargetPlayerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot15TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[64];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot15TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[64] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot16CardNetIdChangedRender")]
		[NetworkedWeaved(65, 1)]
		public unsafe int Slot16CardNetId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot16CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[65];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot16CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[65] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot16CardDataIdChangedRender")]
		[NetworkedWeaved(66, 1)]
		public unsafe int Slot16CardDataId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot16CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[66];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot16CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[66] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot16ColorPackedChangedRender")]
		[NetworkedWeaved(67, 1)]
		public unsafe int Slot16ColorPacked
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot16ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[67];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot16ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[67] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot16TargetPlayerIdChangedRender")]
		[NetworkedWeaved(68, 1)]
		public unsafe int Slot16TargetPlayerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot16TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[68];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot16TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[68] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot17CardNetIdChangedRender")]
		[NetworkedWeaved(69, 1)]
		public unsafe int Slot17CardNetId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot17CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[69];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot17CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[69] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot17CardDataIdChangedRender")]
		[NetworkedWeaved(70, 1)]
		public unsafe int Slot17CardDataId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot17CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[70];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot17CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[70] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot17ColorPackedChangedRender")]
		[NetworkedWeaved(71, 1)]
		public unsafe int Slot17ColorPacked
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot17ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[71];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot17ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[71] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot17TargetPlayerIdChangedRender")]
		[NetworkedWeaved(72, 1)]
		public unsafe int Slot17TargetPlayerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot17TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[72];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot17TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[72] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot18CardNetIdChangedRender")]
		[NetworkedWeaved(73, 1)]
		public unsafe int Slot18CardNetId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot18CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[73];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot18CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[73] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot18CardDataIdChangedRender")]
		[NetworkedWeaved(74, 1)]
		public unsafe int Slot18CardDataId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot18CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[74];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot18CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[74] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot18ColorPackedChangedRender")]
		[NetworkedWeaved(75, 1)]
		public unsafe int Slot18ColorPacked
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot18ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[75];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot18ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[75] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot18TargetPlayerIdChangedRender")]
		[NetworkedWeaved(76, 1)]
		public unsafe int Slot18TargetPlayerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot18TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[76];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot18TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[76] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot19CardNetIdChangedRender")]
		[NetworkedWeaved(77, 1)]
		public unsafe int Slot19CardNetId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot19CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[77];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot19CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[77] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot19CardDataIdChangedRender")]
		[NetworkedWeaved(78, 1)]
		public unsafe int Slot19CardDataId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot19CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[78];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot19CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[78] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot19ColorPackedChangedRender")]
		[NetworkedWeaved(79, 1)]
		public unsafe int Slot19ColorPacked
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot19ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[79];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot19ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[79] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot19TargetPlayerIdChangedRender")]
		[NetworkedWeaved(80, 1)]
		public unsafe int Slot19TargetPlayerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot19TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[80];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot19TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[80] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot20CardNetIdChangedRender")]
		[NetworkedWeaved(81, 1)]
		public unsafe int Slot20CardNetId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot20CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[81];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot20CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[81] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot20CardDataIdChangedRender")]
		[NetworkedWeaved(82, 1)]
		public unsafe int Slot20CardDataId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot20CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[82];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot20CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[82] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot20ColorPackedChangedRender")]
		[NetworkedWeaved(83, 1)]
		public unsafe int Slot20ColorPacked
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot20ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[83];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot20ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[83] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot20TargetPlayerIdChangedRender")]
		[NetworkedWeaved(84, 1)]
		public unsafe int Slot20TargetPlayerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot20TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[84];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot20TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[84] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot21CardNetIdChangedRender")]
		[NetworkedWeaved(85, 1)]
		public unsafe int Slot21CardNetId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot21CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[85];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot21CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[85] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot21CardDataIdChangedRender")]
		[NetworkedWeaved(86, 1)]
		public unsafe int Slot21CardDataId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot21CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[86];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot21CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[86] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot21ColorPackedChangedRender")]
		[NetworkedWeaved(87, 1)]
		public unsafe int Slot21ColorPacked
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot21ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[87];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot21ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[87] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot21TargetPlayerIdChangedRender")]
		[NetworkedWeaved(88, 1)]
		public unsafe int Slot21TargetPlayerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot21TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[88];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot21TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[88] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot22CardNetIdChangedRender")]
		[NetworkedWeaved(89, 1)]
		public unsafe int Slot22CardNetId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot22CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[89];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot22CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[89] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot22CardDataIdChangedRender")]
		[NetworkedWeaved(90, 1)]
		public unsafe int Slot22CardDataId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot22CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[90];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot22CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[90] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot22ColorPackedChangedRender")]
		[NetworkedWeaved(91, 1)]
		public unsafe int Slot22ColorPacked
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot22ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[91];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot22ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[91] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot22TargetPlayerIdChangedRender")]
		[NetworkedWeaved(92, 1)]
		public unsafe int Slot22TargetPlayerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot22TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[92];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot22TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[92] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot23CardNetIdChangedRender")]
		[NetworkedWeaved(93, 1)]
		public unsafe int Slot23CardNetId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot23CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[93];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot23CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[93] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot23CardDataIdChangedRender")]
		[NetworkedWeaved(94, 1)]
		public unsafe int Slot23CardDataId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot23CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[94];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot23CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[94] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot23ColorPackedChangedRender")]
		[NetworkedWeaved(95, 1)]
		public unsafe int Slot23ColorPacked
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot23ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[95];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot23ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[95] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot23TargetPlayerIdChangedRender")]
		[NetworkedWeaved(96, 1)]
		public unsafe int Slot23TargetPlayerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot23TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[96];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot23TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[96] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot24CardNetIdChangedRender")]
		[NetworkedWeaved(97, 1)]
		public unsafe int Slot24CardNetId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot24CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[97];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot24CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[97] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot24CardDataIdChangedRender")]
		[NetworkedWeaved(98, 1)]
		public unsafe int Slot24CardDataId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot24CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[98];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot24CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[98] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot24ColorPackedChangedRender")]
		[NetworkedWeaved(99, 1)]
		public unsafe int Slot24ColorPacked
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot24ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[99];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot24ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[99] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot24TargetPlayerIdChangedRender")]
		[NetworkedWeaved(100, 1)]
		public unsafe int Slot24TargetPlayerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot24TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[100];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot24TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[100] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot25CardNetIdChangedRender")]
		[NetworkedWeaved(101, 1)]
		public unsafe int Slot25CardNetId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot25CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[101];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot25CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[101] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot25CardDataIdChangedRender")]
		[NetworkedWeaved(102, 1)]
		public unsafe int Slot25CardDataId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot25CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[102];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot25CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[102] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot25ColorPackedChangedRender")]
		[NetworkedWeaved(103, 1)]
		public unsafe int Slot25ColorPacked
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot25ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[103];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot25ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[103] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot25TargetPlayerIdChangedRender")]
		[NetworkedWeaved(104, 1)]
		public unsafe int Slot25TargetPlayerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot25TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[104];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot25TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[104] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot26CardNetIdChangedRender")]
		[NetworkedWeaved(105, 1)]
		public unsafe int Slot26CardNetId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot26CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[105];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot26CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[105] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot26CardDataIdChangedRender")]
		[NetworkedWeaved(106, 1)]
		public unsafe int Slot26CardDataId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot26CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[106];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot26CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[106] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot26ColorPackedChangedRender")]
		[NetworkedWeaved(107, 1)]
		public unsafe int Slot26ColorPacked
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot26ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[107];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot26ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[107] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot26TargetPlayerIdChangedRender")]
		[NetworkedWeaved(108, 1)]
		public unsafe int Slot26TargetPlayerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot26TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[108];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot26TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[108] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot27CardNetIdChangedRender")]
		[NetworkedWeaved(109, 1)]
		public unsafe int Slot27CardNetId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot27CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[109];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot27CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[109] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot27CardDataIdChangedRender")]
		[NetworkedWeaved(110, 1)]
		public unsafe int Slot27CardDataId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot27CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[110];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot27CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[110] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot27ColorPackedChangedRender")]
		[NetworkedWeaved(111, 1)]
		public unsafe int Slot27ColorPacked
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot27ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[111];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot27ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[111] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot27TargetPlayerIdChangedRender")]
		[NetworkedWeaved(112, 1)]
		public unsafe int Slot27TargetPlayerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot27TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[112];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot27TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[112] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot28CardNetIdChangedRender")]
		[NetworkedWeaved(113, 1)]
		public unsafe int Slot28CardNetId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot28CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[113];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot28CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[113] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot28CardDataIdChangedRender")]
		[NetworkedWeaved(114, 1)]
		public unsafe int Slot28CardDataId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot28CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[114];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot28CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[114] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot28ColorPackedChangedRender")]
		[NetworkedWeaved(115, 1)]
		public unsafe int Slot28ColorPacked
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot28ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[115];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot28ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[115] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot28TargetPlayerIdChangedRender")]
		[NetworkedWeaved(116, 1)]
		public unsafe int Slot28TargetPlayerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot28TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[116];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot28TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[116] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot29CardNetIdChangedRender")]
		[NetworkedWeaved(117, 1)]
		public unsafe int Slot29CardNetId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot29CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[117];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot29CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[117] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot29CardDataIdChangedRender")]
		[NetworkedWeaved(118, 1)]
		public unsafe int Slot29CardDataId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot29CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[118];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot29CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[118] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot29ColorPackedChangedRender")]
		[NetworkedWeaved(119, 1)]
		public unsafe int Slot29ColorPacked
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot29ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[119];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot29ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[119] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot29TargetPlayerIdChangedRender")]
		[NetworkedWeaved(120, 1)]
		public unsafe int Slot29TargetPlayerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot29TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[120];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot29TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[120] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot30CardNetIdChangedRender")]
		[NetworkedWeaved(121, 1)]
		public unsafe int Slot30CardNetId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot30CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[121];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot30CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[121] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot30CardDataIdChangedRender")]
		[NetworkedWeaved(122, 1)]
		public unsafe int Slot30CardDataId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot30CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[122];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot30CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[122] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot30ColorPackedChangedRender")]
		[NetworkedWeaved(123, 1)]
		public unsafe int Slot30ColorPacked
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot30ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[123];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot30ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[123] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot30TargetPlayerIdChangedRender")]
		[NetworkedWeaved(124, 1)]
		public unsafe int Slot30TargetPlayerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot30TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[124];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot30TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[124] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot31CardNetIdChangedRender")]
		[NetworkedWeaved(125, 1)]
		public unsafe int Slot31CardNetId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot31CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[125];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot31CardNetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[125] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot31CardDataIdChangedRender")]
		[NetworkedWeaved(126, 1)]
		public unsafe int Slot31CardDataId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot31CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[126];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot31CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[126] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot31ColorPackedChangedRender")]
		[NetworkedWeaved(127, 1)]
		public unsafe int Slot31ColorPacked
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot31ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[127];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot31ColorPacked. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[127] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot31TargetPlayerIdChangedRender")]
		[NetworkedWeaved(128, 1)]
		public unsafe int Slot31TargetPlayerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot31TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[128];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PendingRewardNetworkObject.Slot31TargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[128] = value;
			}
		}

		public event Action<int> OnNetworkedTargetLevelNumberChanged;

		public event Action<int> OnNetworkedSlot0CardNetIdChanged;

		public event Action<int> OnNetworkedSlot0CardDataIdChanged;

		public event Action<int> OnNetworkedSlot0ColorPackedChanged;

		public event Action<int> OnNetworkedSlot0TargetPlayerIdChanged;

		public event Action<int> OnNetworkedSlot1CardNetIdChanged;

		public event Action<int> OnNetworkedSlot1CardDataIdChanged;

		public event Action<int> OnNetworkedSlot1ColorPackedChanged;

		public event Action<int> OnNetworkedSlot1TargetPlayerIdChanged;

		public event Action<int> OnNetworkedSlot2CardNetIdChanged;

		public event Action<int> OnNetworkedSlot2CardDataIdChanged;

		public event Action<int> OnNetworkedSlot2ColorPackedChanged;

		public event Action<int> OnNetworkedSlot2TargetPlayerIdChanged;

		public event Action<int> OnNetworkedSlot3CardNetIdChanged;

		public event Action<int> OnNetworkedSlot3CardDataIdChanged;

		public event Action<int> OnNetworkedSlot3ColorPackedChanged;

		public event Action<int> OnNetworkedSlot3TargetPlayerIdChanged;

		public event Action<int> OnNetworkedSlot4CardNetIdChanged;

		public event Action<int> OnNetworkedSlot4CardDataIdChanged;

		public event Action<int> OnNetworkedSlot4ColorPackedChanged;

		public event Action<int> OnNetworkedSlot4TargetPlayerIdChanged;

		public event Action<int> OnNetworkedSlot5CardNetIdChanged;

		public event Action<int> OnNetworkedSlot5CardDataIdChanged;

		public event Action<int> OnNetworkedSlot5ColorPackedChanged;

		public event Action<int> OnNetworkedSlot5TargetPlayerIdChanged;

		public event Action<int> OnNetworkedSlot6CardNetIdChanged;

		public event Action<int> OnNetworkedSlot6CardDataIdChanged;

		public event Action<int> OnNetworkedSlot6ColorPackedChanged;

		public event Action<int> OnNetworkedSlot6TargetPlayerIdChanged;

		public event Action<int> OnNetworkedSlot7CardNetIdChanged;

		public event Action<int> OnNetworkedSlot7CardDataIdChanged;

		public event Action<int> OnNetworkedSlot7ColorPackedChanged;

		public event Action<int> OnNetworkedSlot7TargetPlayerIdChanged;

		public event Action<int> OnNetworkedSlot8CardNetIdChanged;

		public event Action<int> OnNetworkedSlot8CardDataIdChanged;

		public event Action<int> OnNetworkedSlot8ColorPackedChanged;

		public event Action<int> OnNetworkedSlot8TargetPlayerIdChanged;

		public event Action<int> OnNetworkedSlot9CardNetIdChanged;

		public event Action<int> OnNetworkedSlot9CardDataIdChanged;

		public event Action<int> OnNetworkedSlot9ColorPackedChanged;

		public event Action<int> OnNetworkedSlot9TargetPlayerIdChanged;

		public event Action<int> OnNetworkedSlot10CardNetIdChanged;

		public event Action<int> OnNetworkedSlot10CardDataIdChanged;

		public event Action<int> OnNetworkedSlot10ColorPackedChanged;

		public event Action<int> OnNetworkedSlot10TargetPlayerIdChanged;

		public event Action<int> OnNetworkedSlot11CardNetIdChanged;

		public event Action<int> OnNetworkedSlot11CardDataIdChanged;

		public event Action<int> OnNetworkedSlot11ColorPackedChanged;

		public event Action<int> OnNetworkedSlot11TargetPlayerIdChanged;

		public event Action<int> OnNetworkedSlot12CardNetIdChanged;

		public event Action<int> OnNetworkedSlot12CardDataIdChanged;

		public event Action<int> OnNetworkedSlot12ColorPackedChanged;

		public event Action<int> OnNetworkedSlot12TargetPlayerIdChanged;

		public event Action<int> OnNetworkedSlot13CardNetIdChanged;

		public event Action<int> OnNetworkedSlot13CardDataIdChanged;

		public event Action<int> OnNetworkedSlot13ColorPackedChanged;

		public event Action<int> OnNetworkedSlot13TargetPlayerIdChanged;

		public event Action<int> OnNetworkedSlot14CardNetIdChanged;

		public event Action<int> OnNetworkedSlot14CardDataIdChanged;

		public event Action<int> OnNetworkedSlot14ColorPackedChanged;

		public event Action<int> OnNetworkedSlot14TargetPlayerIdChanged;

		public event Action<int> OnNetworkedSlot15CardNetIdChanged;

		public event Action<int> OnNetworkedSlot15CardDataIdChanged;

		public event Action<int> OnNetworkedSlot15ColorPackedChanged;

		public event Action<int> OnNetworkedSlot15TargetPlayerIdChanged;

		public event Action<int> OnNetworkedSlot16CardNetIdChanged;

		public event Action<int> OnNetworkedSlot16CardDataIdChanged;

		public event Action<int> OnNetworkedSlot16ColorPackedChanged;

		public event Action<int> OnNetworkedSlot16TargetPlayerIdChanged;

		public event Action<int> OnNetworkedSlot17CardNetIdChanged;

		public event Action<int> OnNetworkedSlot17CardDataIdChanged;

		public event Action<int> OnNetworkedSlot17ColorPackedChanged;

		public event Action<int> OnNetworkedSlot17TargetPlayerIdChanged;

		public event Action<int> OnNetworkedSlot18CardNetIdChanged;

		public event Action<int> OnNetworkedSlot18CardDataIdChanged;

		public event Action<int> OnNetworkedSlot18ColorPackedChanged;

		public event Action<int> OnNetworkedSlot18TargetPlayerIdChanged;

		public event Action<int> OnNetworkedSlot19CardNetIdChanged;

		public event Action<int> OnNetworkedSlot19CardDataIdChanged;

		public event Action<int> OnNetworkedSlot19ColorPackedChanged;

		public event Action<int> OnNetworkedSlot19TargetPlayerIdChanged;

		public event Action<int> OnNetworkedSlot20CardNetIdChanged;

		public event Action<int> OnNetworkedSlot20CardDataIdChanged;

		public event Action<int> OnNetworkedSlot20ColorPackedChanged;

		public event Action<int> OnNetworkedSlot20TargetPlayerIdChanged;

		public event Action<int> OnNetworkedSlot21CardNetIdChanged;

		public event Action<int> OnNetworkedSlot21CardDataIdChanged;

		public event Action<int> OnNetworkedSlot21ColorPackedChanged;

		public event Action<int> OnNetworkedSlot21TargetPlayerIdChanged;

		public event Action<int> OnNetworkedSlot22CardNetIdChanged;

		public event Action<int> OnNetworkedSlot22CardDataIdChanged;

		public event Action<int> OnNetworkedSlot22ColorPackedChanged;

		public event Action<int> OnNetworkedSlot22TargetPlayerIdChanged;

		public event Action<int> OnNetworkedSlot23CardNetIdChanged;

		public event Action<int> OnNetworkedSlot23CardDataIdChanged;

		public event Action<int> OnNetworkedSlot23ColorPackedChanged;

		public event Action<int> OnNetworkedSlot23TargetPlayerIdChanged;

		public event Action<int> OnNetworkedSlot24CardNetIdChanged;

		public event Action<int> OnNetworkedSlot24CardDataIdChanged;

		public event Action<int> OnNetworkedSlot24ColorPackedChanged;

		public event Action<int> OnNetworkedSlot24TargetPlayerIdChanged;

		public event Action<int> OnNetworkedSlot25CardNetIdChanged;

		public event Action<int> OnNetworkedSlot25CardDataIdChanged;

		public event Action<int> OnNetworkedSlot25ColorPackedChanged;

		public event Action<int> OnNetworkedSlot25TargetPlayerIdChanged;

		public event Action<int> OnNetworkedSlot26CardNetIdChanged;

		public event Action<int> OnNetworkedSlot26CardDataIdChanged;

		public event Action<int> OnNetworkedSlot26ColorPackedChanged;

		public event Action<int> OnNetworkedSlot26TargetPlayerIdChanged;

		public event Action<int> OnNetworkedSlot27CardNetIdChanged;

		public event Action<int> OnNetworkedSlot27CardDataIdChanged;

		public event Action<int> OnNetworkedSlot27ColorPackedChanged;

		public event Action<int> OnNetworkedSlot27TargetPlayerIdChanged;

		public event Action<int> OnNetworkedSlot28CardNetIdChanged;

		public event Action<int> OnNetworkedSlot28CardDataIdChanged;

		public event Action<int> OnNetworkedSlot28ColorPackedChanged;

		public event Action<int> OnNetworkedSlot28TargetPlayerIdChanged;

		public event Action<int> OnNetworkedSlot29CardNetIdChanged;

		public event Action<int> OnNetworkedSlot29CardDataIdChanged;

		public event Action<int> OnNetworkedSlot29ColorPackedChanged;

		public event Action<int> OnNetworkedSlot29TargetPlayerIdChanged;

		public event Action<int> OnNetworkedSlot30CardNetIdChanged;

		public event Action<int> OnNetworkedSlot30CardDataIdChanged;

		public event Action<int> OnNetworkedSlot30ColorPackedChanged;

		public event Action<int> OnNetworkedSlot30TargetPlayerIdChanged;

		public event Action<int> OnNetworkedSlot31CardNetIdChanged;

		public event Action<int> OnNetworkedSlot31CardDataIdChanged;

		public event Action<int> OnNetworkedSlot31ColorPackedChanged;

		public event Action<int> OnNetworkedSlot31TargetPlayerIdChanged;

		public event Action OnAuthoritativeTick;

		public event Action OnDespawned;

		[Inject]
		public void InjectDependencies(INetworkedModelInstanceProvider networkedModelInstanceProvider)
		{
			_networkedModelInstanceProvider = networkedModelInstanceProvider;
		}

		public override void Spawned()
		{
			_networkedModelInstanceProvider?.Register(this);
			this.OnNetworkedTargetLevelNumberChanged?.Invoke(TargetLevelNumber);
			this.OnNetworkedSlot0CardNetIdChanged?.Invoke(Slot0CardNetId);
			this.OnNetworkedSlot0CardDataIdChanged?.Invoke(Slot0CardDataId);
			this.OnNetworkedSlot0ColorPackedChanged?.Invoke(Slot0ColorPacked);
			this.OnNetworkedSlot0TargetPlayerIdChanged?.Invoke(Slot0TargetPlayerId);
			this.OnNetworkedSlot1CardNetIdChanged?.Invoke(Slot1CardNetId);
			this.OnNetworkedSlot1CardDataIdChanged?.Invoke(Slot1CardDataId);
			this.OnNetworkedSlot1ColorPackedChanged?.Invoke(Slot1ColorPacked);
			this.OnNetworkedSlot1TargetPlayerIdChanged?.Invoke(Slot1TargetPlayerId);
			this.OnNetworkedSlot2CardNetIdChanged?.Invoke(Slot2CardNetId);
			this.OnNetworkedSlot2CardDataIdChanged?.Invoke(Slot2CardDataId);
			this.OnNetworkedSlot2ColorPackedChanged?.Invoke(Slot2ColorPacked);
			this.OnNetworkedSlot2TargetPlayerIdChanged?.Invoke(Slot2TargetPlayerId);
			this.OnNetworkedSlot3CardNetIdChanged?.Invoke(Slot3CardNetId);
			this.OnNetworkedSlot3CardDataIdChanged?.Invoke(Slot3CardDataId);
			this.OnNetworkedSlot3ColorPackedChanged?.Invoke(Slot3ColorPacked);
			this.OnNetworkedSlot3TargetPlayerIdChanged?.Invoke(Slot3TargetPlayerId);
			this.OnNetworkedSlot4CardNetIdChanged?.Invoke(Slot4CardNetId);
			this.OnNetworkedSlot4CardDataIdChanged?.Invoke(Slot4CardDataId);
			this.OnNetworkedSlot4ColorPackedChanged?.Invoke(Slot4ColorPacked);
			this.OnNetworkedSlot4TargetPlayerIdChanged?.Invoke(Slot4TargetPlayerId);
			this.OnNetworkedSlot5CardNetIdChanged?.Invoke(Slot5CardNetId);
			this.OnNetworkedSlot5CardDataIdChanged?.Invoke(Slot5CardDataId);
			this.OnNetworkedSlot5ColorPackedChanged?.Invoke(Slot5ColorPacked);
			this.OnNetworkedSlot5TargetPlayerIdChanged?.Invoke(Slot5TargetPlayerId);
			this.OnNetworkedSlot6CardNetIdChanged?.Invoke(Slot6CardNetId);
			this.OnNetworkedSlot6CardDataIdChanged?.Invoke(Slot6CardDataId);
			this.OnNetworkedSlot6ColorPackedChanged?.Invoke(Slot6ColorPacked);
			this.OnNetworkedSlot6TargetPlayerIdChanged?.Invoke(Slot6TargetPlayerId);
			this.OnNetworkedSlot7CardNetIdChanged?.Invoke(Slot7CardNetId);
			this.OnNetworkedSlot7CardDataIdChanged?.Invoke(Slot7CardDataId);
			this.OnNetworkedSlot7ColorPackedChanged?.Invoke(Slot7ColorPacked);
			this.OnNetworkedSlot7TargetPlayerIdChanged?.Invoke(Slot7TargetPlayerId);
			this.OnNetworkedSlot8CardNetIdChanged?.Invoke(Slot8CardNetId);
			this.OnNetworkedSlot8CardDataIdChanged?.Invoke(Slot8CardDataId);
			this.OnNetworkedSlot8ColorPackedChanged?.Invoke(Slot8ColorPacked);
			this.OnNetworkedSlot8TargetPlayerIdChanged?.Invoke(Slot8TargetPlayerId);
			this.OnNetworkedSlot9CardNetIdChanged?.Invoke(Slot9CardNetId);
			this.OnNetworkedSlot9CardDataIdChanged?.Invoke(Slot9CardDataId);
			this.OnNetworkedSlot9ColorPackedChanged?.Invoke(Slot9ColorPacked);
			this.OnNetworkedSlot9TargetPlayerIdChanged?.Invoke(Slot9TargetPlayerId);
			this.OnNetworkedSlot10CardNetIdChanged?.Invoke(Slot10CardNetId);
			this.OnNetworkedSlot10CardDataIdChanged?.Invoke(Slot10CardDataId);
			this.OnNetworkedSlot10ColorPackedChanged?.Invoke(Slot10ColorPacked);
			this.OnNetworkedSlot10TargetPlayerIdChanged?.Invoke(Slot10TargetPlayerId);
			this.OnNetworkedSlot11CardNetIdChanged?.Invoke(Slot11CardNetId);
			this.OnNetworkedSlot11CardDataIdChanged?.Invoke(Slot11CardDataId);
			this.OnNetworkedSlot11ColorPackedChanged?.Invoke(Slot11ColorPacked);
			this.OnNetworkedSlot11TargetPlayerIdChanged?.Invoke(Slot11TargetPlayerId);
			this.OnNetworkedSlot12CardNetIdChanged?.Invoke(Slot12CardNetId);
			this.OnNetworkedSlot12CardDataIdChanged?.Invoke(Slot12CardDataId);
			this.OnNetworkedSlot12ColorPackedChanged?.Invoke(Slot12ColorPacked);
			this.OnNetworkedSlot12TargetPlayerIdChanged?.Invoke(Slot12TargetPlayerId);
			this.OnNetworkedSlot13CardNetIdChanged?.Invoke(Slot13CardNetId);
			this.OnNetworkedSlot13CardDataIdChanged?.Invoke(Slot13CardDataId);
			this.OnNetworkedSlot13ColorPackedChanged?.Invoke(Slot13ColorPacked);
			this.OnNetworkedSlot13TargetPlayerIdChanged?.Invoke(Slot13TargetPlayerId);
			this.OnNetworkedSlot14CardNetIdChanged?.Invoke(Slot14CardNetId);
			this.OnNetworkedSlot14CardDataIdChanged?.Invoke(Slot14CardDataId);
			this.OnNetworkedSlot14ColorPackedChanged?.Invoke(Slot14ColorPacked);
			this.OnNetworkedSlot14TargetPlayerIdChanged?.Invoke(Slot14TargetPlayerId);
			this.OnNetworkedSlot15CardNetIdChanged?.Invoke(Slot15CardNetId);
			this.OnNetworkedSlot15CardDataIdChanged?.Invoke(Slot15CardDataId);
			this.OnNetworkedSlot15ColorPackedChanged?.Invoke(Slot15ColorPacked);
			this.OnNetworkedSlot15TargetPlayerIdChanged?.Invoke(Slot15TargetPlayerId);
			this.OnNetworkedSlot16CardNetIdChanged?.Invoke(Slot16CardNetId);
			this.OnNetworkedSlot16CardDataIdChanged?.Invoke(Slot16CardDataId);
			this.OnNetworkedSlot16ColorPackedChanged?.Invoke(Slot16ColorPacked);
			this.OnNetworkedSlot16TargetPlayerIdChanged?.Invoke(Slot16TargetPlayerId);
			this.OnNetworkedSlot17CardNetIdChanged?.Invoke(Slot17CardNetId);
			this.OnNetworkedSlot17CardDataIdChanged?.Invoke(Slot17CardDataId);
			this.OnNetworkedSlot17ColorPackedChanged?.Invoke(Slot17ColorPacked);
			this.OnNetworkedSlot17TargetPlayerIdChanged?.Invoke(Slot17TargetPlayerId);
			this.OnNetworkedSlot18CardNetIdChanged?.Invoke(Slot18CardNetId);
			this.OnNetworkedSlot18CardDataIdChanged?.Invoke(Slot18CardDataId);
			this.OnNetworkedSlot18ColorPackedChanged?.Invoke(Slot18ColorPacked);
			this.OnNetworkedSlot18TargetPlayerIdChanged?.Invoke(Slot18TargetPlayerId);
			this.OnNetworkedSlot19CardNetIdChanged?.Invoke(Slot19CardNetId);
			this.OnNetworkedSlot19CardDataIdChanged?.Invoke(Slot19CardDataId);
			this.OnNetworkedSlot19ColorPackedChanged?.Invoke(Slot19ColorPacked);
			this.OnNetworkedSlot19TargetPlayerIdChanged?.Invoke(Slot19TargetPlayerId);
			this.OnNetworkedSlot20CardNetIdChanged?.Invoke(Slot20CardNetId);
			this.OnNetworkedSlot20CardDataIdChanged?.Invoke(Slot20CardDataId);
			this.OnNetworkedSlot20ColorPackedChanged?.Invoke(Slot20ColorPacked);
			this.OnNetworkedSlot20TargetPlayerIdChanged?.Invoke(Slot20TargetPlayerId);
			this.OnNetworkedSlot21CardNetIdChanged?.Invoke(Slot21CardNetId);
			this.OnNetworkedSlot21CardDataIdChanged?.Invoke(Slot21CardDataId);
			this.OnNetworkedSlot21ColorPackedChanged?.Invoke(Slot21ColorPacked);
			this.OnNetworkedSlot21TargetPlayerIdChanged?.Invoke(Slot21TargetPlayerId);
			this.OnNetworkedSlot22CardNetIdChanged?.Invoke(Slot22CardNetId);
			this.OnNetworkedSlot22CardDataIdChanged?.Invoke(Slot22CardDataId);
			this.OnNetworkedSlot22ColorPackedChanged?.Invoke(Slot22ColorPacked);
			this.OnNetworkedSlot22TargetPlayerIdChanged?.Invoke(Slot22TargetPlayerId);
			this.OnNetworkedSlot23CardNetIdChanged?.Invoke(Slot23CardNetId);
			this.OnNetworkedSlot23CardDataIdChanged?.Invoke(Slot23CardDataId);
			this.OnNetworkedSlot23ColorPackedChanged?.Invoke(Slot23ColorPacked);
			this.OnNetworkedSlot23TargetPlayerIdChanged?.Invoke(Slot23TargetPlayerId);
			this.OnNetworkedSlot24CardNetIdChanged?.Invoke(Slot24CardNetId);
			this.OnNetworkedSlot24CardDataIdChanged?.Invoke(Slot24CardDataId);
			this.OnNetworkedSlot24ColorPackedChanged?.Invoke(Slot24ColorPacked);
			this.OnNetworkedSlot24TargetPlayerIdChanged?.Invoke(Slot24TargetPlayerId);
			this.OnNetworkedSlot25CardNetIdChanged?.Invoke(Slot25CardNetId);
			this.OnNetworkedSlot25CardDataIdChanged?.Invoke(Slot25CardDataId);
			this.OnNetworkedSlot25ColorPackedChanged?.Invoke(Slot25ColorPacked);
			this.OnNetworkedSlot25TargetPlayerIdChanged?.Invoke(Slot25TargetPlayerId);
			this.OnNetworkedSlot26CardNetIdChanged?.Invoke(Slot26CardNetId);
			this.OnNetworkedSlot26CardDataIdChanged?.Invoke(Slot26CardDataId);
			this.OnNetworkedSlot26ColorPackedChanged?.Invoke(Slot26ColorPacked);
			this.OnNetworkedSlot26TargetPlayerIdChanged?.Invoke(Slot26TargetPlayerId);
			this.OnNetworkedSlot27CardNetIdChanged?.Invoke(Slot27CardNetId);
			this.OnNetworkedSlot27CardDataIdChanged?.Invoke(Slot27CardDataId);
			this.OnNetworkedSlot27ColorPackedChanged?.Invoke(Slot27ColorPacked);
			this.OnNetworkedSlot27TargetPlayerIdChanged?.Invoke(Slot27TargetPlayerId);
			this.OnNetworkedSlot28CardNetIdChanged?.Invoke(Slot28CardNetId);
			this.OnNetworkedSlot28CardDataIdChanged?.Invoke(Slot28CardDataId);
			this.OnNetworkedSlot28ColorPackedChanged?.Invoke(Slot28ColorPacked);
			this.OnNetworkedSlot28TargetPlayerIdChanged?.Invoke(Slot28TargetPlayerId);
			this.OnNetworkedSlot29CardNetIdChanged?.Invoke(Slot29CardNetId);
			this.OnNetworkedSlot29CardDataIdChanged?.Invoke(Slot29CardDataId);
			this.OnNetworkedSlot29ColorPackedChanged?.Invoke(Slot29ColorPacked);
			this.OnNetworkedSlot29TargetPlayerIdChanged?.Invoke(Slot29TargetPlayerId);
			this.OnNetworkedSlot30CardNetIdChanged?.Invoke(Slot30CardNetId);
			this.OnNetworkedSlot30CardDataIdChanged?.Invoke(Slot30CardDataId);
			this.OnNetworkedSlot30ColorPackedChanged?.Invoke(Slot30ColorPacked);
			this.OnNetworkedSlot30TargetPlayerIdChanged?.Invoke(Slot30TargetPlayerId);
			this.OnNetworkedSlot31CardNetIdChanged?.Invoke(Slot31CardNetId);
			this.OnNetworkedSlot31CardDataIdChanged?.Invoke(Slot31CardDataId);
			this.OnNetworkedSlot31ColorPackedChanged?.Invoke(Slot31ColorPacked);
			this.OnNetworkedSlot31TargetPlayerIdChanged?.Invoke(Slot31TargetPlayerId);
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_networkedModelInstanceProvider?.Unregister(this);
			this.OnDespawned?.Invoke();
		}

		public override void FixedUpdateNetwork()
		{
			if (base.HasStateAuthority)
			{
				this.OnAuthoritativeTick?.Invoke();
			}
		}

		public bool TryWriteTargetLevelNumber(int targetLevelNumber)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			TargetLevelNumber = targetLevelNumber;
			return true;
		}

		public bool TryWriteSlot0CardNetId(int slot0CardNetId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot0CardNetId = slot0CardNetId;
			return true;
		}

		public bool TryWriteSlot0CardDataId(int slot0CardDataId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot0CardDataId = slot0CardDataId;
			return true;
		}

		public bool TryWriteSlot0ColorPacked(int slot0ColorPacked)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot0ColorPacked = slot0ColorPacked;
			return true;
		}

		public bool TryWriteSlot0TargetPlayerId(int slot0TargetPlayerId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot0TargetPlayerId = slot0TargetPlayerId;
			return true;
		}

		public bool TryWriteSlot1CardNetId(int slot1CardNetId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot1CardNetId = slot1CardNetId;
			return true;
		}

		public bool TryWriteSlot1CardDataId(int slot1CardDataId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot1CardDataId = slot1CardDataId;
			return true;
		}

		public bool TryWriteSlot1ColorPacked(int slot1ColorPacked)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot1ColorPacked = slot1ColorPacked;
			return true;
		}

		public bool TryWriteSlot1TargetPlayerId(int slot1TargetPlayerId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot1TargetPlayerId = slot1TargetPlayerId;
			return true;
		}

		public bool TryWriteSlot2CardNetId(int slot2CardNetId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot2CardNetId = slot2CardNetId;
			return true;
		}

		public bool TryWriteSlot2CardDataId(int slot2CardDataId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot2CardDataId = slot2CardDataId;
			return true;
		}

		public bool TryWriteSlot2ColorPacked(int slot2ColorPacked)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot2ColorPacked = slot2ColorPacked;
			return true;
		}

		public bool TryWriteSlot2TargetPlayerId(int slot2TargetPlayerId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot2TargetPlayerId = slot2TargetPlayerId;
			return true;
		}

		public bool TryWriteSlot3CardNetId(int slot3CardNetId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot3CardNetId = slot3CardNetId;
			return true;
		}

		public bool TryWriteSlot3CardDataId(int slot3CardDataId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot3CardDataId = slot3CardDataId;
			return true;
		}

		public bool TryWriteSlot3ColorPacked(int slot3ColorPacked)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot3ColorPacked = slot3ColorPacked;
			return true;
		}

		public bool TryWriteSlot3TargetPlayerId(int slot3TargetPlayerId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot3TargetPlayerId = slot3TargetPlayerId;
			return true;
		}

		public bool TryWriteSlot4CardNetId(int slot4CardNetId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot4CardNetId = slot4CardNetId;
			return true;
		}

		public bool TryWriteSlot4CardDataId(int slot4CardDataId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot4CardDataId = slot4CardDataId;
			return true;
		}

		public bool TryWriteSlot4ColorPacked(int slot4ColorPacked)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot4ColorPacked = slot4ColorPacked;
			return true;
		}

		public bool TryWriteSlot4TargetPlayerId(int slot4TargetPlayerId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot4TargetPlayerId = slot4TargetPlayerId;
			return true;
		}

		public bool TryWriteSlot5CardNetId(int slot5CardNetId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot5CardNetId = slot5CardNetId;
			return true;
		}

		public bool TryWriteSlot5CardDataId(int slot5CardDataId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot5CardDataId = slot5CardDataId;
			return true;
		}

		public bool TryWriteSlot5ColorPacked(int slot5ColorPacked)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot5ColorPacked = slot5ColorPacked;
			return true;
		}

		public bool TryWriteSlot5TargetPlayerId(int slot5TargetPlayerId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot5TargetPlayerId = slot5TargetPlayerId;
			return true;
		}

		public bool TryWriteSlot6CardNetId(int slot6CardNetId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot6CardNetId = slot6CardNetId;
			return true;
		}

		public bool TryWriteSlot6CardDataId(int slot6CardDataId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot6CardDataId = slot6CardDataId;
			return true;
		}

		public bool TryWriteSlot6ColorPacked(int slot6ColorPacked)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot6ColorPacked = slot6ColorPacked;
			return true;
		}

		public bool TryWriteSlot6TargetPlayerId(int slot6TargetPlayerId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot6TargetPlayerId = slot6TargetPlayerId;
			return true;
		}

		public bool TryWriteSlot7CardNetId(int slot7CardNetId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot7CardNetId = slot7CardNetId;
			return true;
		}

		public bool TryWriteSlot7CardDataId(int slot7CardDataId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot7CardDataId = slot7CardDataId;
			return true;
		}

		public bool TryWriteSlot7ColorPacked(int slot7ColorPacked)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot7ColorPacked = slot7ColorPacked;
			return true;
		}

		public bool TryWriteSlot7TargetPlayerId(int slot7TargetPlayerId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot7TargetPlayerId = slot7TargetPlayerId;
			return true;
		}

		public bool TryWriteSlot8CardNetId(int slot8CardNetId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot8CardNetId = slot8CardNetId;
			return true;
		}

		public bool TryWriteSlot8CardDataId(int slot8CardDataId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot8CardDataId = slot8CardDataId;
			return true;
		}

		public bool TryWriteSlot8ColorPacked(int slot8ColorPacked)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot8ColorPacked = slot8ColorPacked;
			return true;
		}

		public bool TryWriteSlot8TargetPlayerId(int slot8TargetPlayerId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot8TargetPlayerId = slot8TargetPlayerId;
			return true;
		}

		public bool TryWriteSlot9CardNetId(int slot9CardNetId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot9CardNetId = slot9CardNetId;
			return true;
		}

		public bool TryWriteSlot9CardDataId(int slot9CardDataId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot9CardDataId = slot9CardDataId;
			return true;
		}

		public bool TryWriteSlot9ColorPacked(int slot9ColorPacked)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot9ColorPacked = slot9ColorPacked;
			return true;
		}

		public bool TryWriteSlot9TargetPlayerId(int slot9TargetPlayerId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot9TargetPlayerId = slot9TargetPlayerId;
			return true;
		}

		public bool TryWriteSlot10CardNetId(int slot10CardNetId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot10CardNetId = slot10CardNetId;
			return true;
		}

		public bool TryWriteSlot10CardDataId(int slot10CardDataId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot10CardDataId = slot10CardDataId;
			return true;
		}

		public bool TryWriteSlot10ColorPacked(int slot10ColorPacked)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot10ColorPacked = slot10ColorPacked;
			return true;
		}

		public bool TryWriteSlot10TargetPlayerId(int slot10TargetPlayerId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot10TargetPlayerId = slot10TargetPlayerId;
			return true;
		}

		public bool TryWriteSlot11CardNetId(int slot11CardNetId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot11CardNetId = slot11CardNetId;
			return true;
		}

		public bool TryWriteSlot11CardDataId(int slot11CardDataId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot11CardDataId = slot11CardDataId;
			return true;
		}

		public bool TryWriteSlot11ColorPacked(int slot11ColorPacked)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot11ColorPacked = slot11ColorPacked;
			return true;
		}

		public bool TryWriteSlot11TargetPlayerId(int slot11TargetPlayerId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot11TargetPlayerId = slot11TargetPlayerId;
			return true;
		}

		public bool TryWriteSlot12CardNetId(int slot12CardNetId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot12CardNetId = slot12CardNetId;
			return true;
		}

		public bool TryWriteSlot12CardDataId(int slot12CardDataId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot12CardDataId = slot12CardDataId;
			return true;
		}

		public bool TryWriteSlot12ColorPacked(int slot12ColorPacked)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot12ColorPacked = slot12ColorPacked;
			return true;
		}

		public bool TryWriteSlot12TargetPlayerId(int slot12TargetPlayerId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot12TargetPlayerId = slot12TargetPlayerId;
			return true;
		}

		public bool TryWriteSlot13CardNetId(int slot13CardNetId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot13CardNetId = slot13CardNetId;
			return true;
		}

		public bool TryWriteSlot13CardDataId(int slot13CardDataId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot13CardDataId = slot13CardDataId;
			return true;
		}

		public bool TryWriteSlot13ColorPacked(int slot13ColorPacked)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot13ColorPacked = slot13ColorPacked;
			return true;
		}

		public bool TryWriteSlot13TargetPlayerId(int slot13TargetPlayerId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot13TargetPlayerId = slot13TargetPlayerId;
			return true;
		}

		public bool TryWriteSlot14CardNetId(int slot14CardNetId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot14CardNetId = slot14CardNetId;
			return true;
		}

		public bool TryWriteSlot14CardDataId(int slot14CardDataId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot14CardDataId = slot14CardDataId;
			return true;
		}

		public bool TryWriteSlot14ColorPacked(int slot14ColorPacked)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot14ColorPacked = slot14ColorPacked;
			return true;
		}

		public bool TryWriteSlot14TargetPlayerId(int slot14TargetPlayerId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot14TargetPlayerId = slot14TargetPlayerId;
			return true;
		}

		public bool TryWriteSlot15CardNetId(int slot15CardNetId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot15CardNetId = slot15CardNetId;
			return true;
		}

		public bool TryWriteSlot15CardDataId(int slot15CardDataId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot15CardDataId = slot15CardDataId;
			return true;
		}

		public bool TryWriteSlot15ColorPacked(int slot15ColorPacked)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot15ColorPacked = slot15ColorPacked;
			return true;
		}

		public bool TryWriteSlot15TargetPlayerId(int slot15TargetPlayerId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot15TargetPlayerId = slot15TargetPlayerId;
			return true;
		}

		public bool TryWriteSlot16CardNetId(int slot16CardNetId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot16CardNetId = slot16CardNetId;
			return true;
		}

		public bool TryWriteSlot16CardDataId(int slot16CardDataId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot16CardDataId = slot16CardDataId;
			return true;
		}

		public bool TryWriteSlot16ColorPacked(int slot16ColorPacked)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot16ColorPacked = slot16ColorPacked;
			return true;
		}

		public bool TryWriteSlot16TargetPlayerId(int slot16TargetPlayerId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot16TargetPlayerId = slot16TargetPlayerId;
			return true;
		}

		public bool TryWriteSlot17CardNetId(int slot17CardNetId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot17CardNetId = slot17CardNetId;
			return true;
		}

		public bool TryWriteSlot17CardDataId(int slot17CardDataId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot17CardDataId = slot17CardDataId;
			return true;
		}

		public bool TryWriteSlot17ColorPacked(int slot17ColorPacked)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot17ColorPacked = slot17ColorPacked;
			return true;
		}

		public bool TryWriteSlot17TargetPlayerId(int slot17TargetPlayerId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot17TargetPlayerId = slot17TargetPlayerId;
			return true;
		}

		public bool TryWriteSlot18CardNetId(int slot18CardNetId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot18CardNetId = slot18CardNetId;
			return true;
		}

		public bool TryWriteSlot18CardDataId(int slot18CardDataId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot18CardDataId = slot18CardDataId;
			return true;
		}

		public bool TryWriteSlot18ColorPacked(int slot18ColorPacked)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot18ColorPacked = slot18ColorPacked;
			return true;
		}

		public bool TryWriteSlot18TargetPlayerId(int slot18TargetPlayerId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot18TargetPlayerId = slot18TargetPlayerId;
			return true;
		}

		public bool TryWriteSlot19CardNetId(int slot19CardNetId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot19CardNetId = slot19CardNetId;
			return true;
		}

		public bool TryWriteSlot19CardDataId(int slot19CardDataId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot19CardDataId = slot19CardDataId;
			return true;
		}

		public bool TryWriteSlot19ColorPacked(int slot19ColorPacked)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot19ColorPacked = slot19ColorPacked;
			return true;
		}

		public bool TryWriteSlot19TargetPlayerId(int slot19TargetPlayerId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot19TargetPlayerId = slot19TargetPlayerId;
			return true;
		}

		public bool TryWriteSlot20CardNetId(int slot20CardNetId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot20CardNetId = slot20CardNetId;
			return true;
		}

		public bool TryWriteSlot20CardDataId(int slot20CardDataId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot20CardDataId = slot20CardDataId;
			return true;
		}

		public bool TryWriteSlot20ColorPacked(int slot20ColorPacked)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot20ColorPacked = slot20ColorPacked;
			return true;
		}

		public bool TryWriteSlot20TargetPlayerId(int slot20TargetPlayerId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot20TargetPlayerId = slot20TargetPlayerId;
			return true;
		}

		public bool TryWriteSlot21CardNetId(int slot21CardNetId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot21CardNetId = slot21CardNetId;
			return true;
		}

		public bool TryWriteSlot21CardDataId(int slot21CardDataId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot21CardDataId = slot21CardDataId;
			return true;
		}

		public bool TryWriteSlot21ColorPacked(int slot21ColorPacked)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot21ColorPacked = slot21ColorPacked;
			return true;
		}

		public bool TryWriteSlot21TargetPlayerId(int slot21TargetPlayerId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot21TargetPlayerId = slot21TargetPlayerId;
			return true;
		}

		public bool TryWriteSlot22CardNetId(int slot22CardNetId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot22CardNetId = slot22CardNetId;
			return true;
		}

		public bool TryWriteSlot22CardDataId(int slot22CardDataId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot22CardDataId = slot22CardDataId;
			return true;
		}

		public bool TryWriteSlot22ColorPacked(int slot22ColorPacked)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot22ColorPacked = slot22ColorPacked;
			return true;
		}

		public bool TryWriteSlot22TargetPlayerId(int slot22TargetPlayerId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot22TargetPlayerId = slot22TargetPlayerId;
			return true;
		}

		public bool TryWriteSlot23CardNetId(int slot23CardNetId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot23CardNetId = slot23CardNetId;
			return true;
		}

		public bool TryWriteSlot23CardDataId(int slot23CardDataId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot23CardDataId = slot23CardDataId;
			return true;
		}

		public bool TryWriteSlot23ColorPacked(int slot23ColorPacked)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot23ColorPacked = slot23ColorPacked;
			return true;
		}

		public bool TryWriteSlot23TargetPlayerId(int slot23TargetPlayerId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot23TargetPlayerId = slot23TargetPlayerId;
			return true;
		}

		public bool TryWriteSlot24CardNetId(int slot24CardNetId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot24CardNetId = slot24CardNetId;
			return true;
		}

		public bool TryWriteSlot24CardDataId(int slot24CardDataId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot24CardDataId = slot24CardDataId;
			return true;
		}

		public bool TryWriteSlot24ColorPacked(int slot24ColorPacked)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot24ColorPacked = slot24ColorPacked;
			return true;
		}

		public bool TryWriteSlot24TargetPlayerId(int slot24TargetPlayerId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot24TargetPlayerId = slot24TargetPlayerId;
			return true;
		}

		public bool TryWriteSlot25CardNetId(int slot25CardNetId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot25CardNetId = slot25CardNetId;
			return true;
		}

		public bool TryWriteSlot25CardDataId(int slot25CardDataId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot25CardDataId = slot25CardDataId;
			return true;
		}

		public bool TryWriteSlot25ColorPacked(int slot25ColorPacked)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot25ColorPacked = slot25ColorPacked;
			return true;
		}

		public bool TryWriteSlot25TargetPlayerId(int slot25TargetPlayerId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot25TargetPlayerId = slot25TargetPlayerId;
			return true;
		}

		public bool TryWriteSlot26CardNetId(int slot26CardNetId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot26CardNetId = slot26CardNetId;
			return true;
		}

		public bool TryWriteSlot26CardDataId(int slot26CardDataId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot26CardDataId = slot26CardDataId;
			return true;
		}

		public bool TryWriteSlot26ColorPacked(int slot26ColorPacked)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot26ColorPacked = slot26ColorPacked;
			return true;
		}

		public bool TryWriteSlot26TargetPlayerId(int slot26TargetPlayerId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot26TargetPlayerId = slot26TargetPlayerId;
			return true;
		}

		public bool TryWriteSlot27CardNetId(int slot27CardNetId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot27CardNetId = slot27CardNetId;
			return true;
		}

		public bool TryWriteSlot27CardDataId(int slot27CardDataId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot27CardDataId = slot27CardDataId;
			return true;
		}

		public bool TryWriteSlot27ColorPacked(int slot27ColorPacked)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot27ColorPacked = slot27ColorPacked;
			return true;
		}

		public bool TryWriteSlot27TargetPlayerId(int slot27TargetPlayerId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot27TargetPlayerId = slot27TargetPlayerId;
			return true;
		}

		public bool TryWriteSlot28CardNetId(int slot28CardNetId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot28CardNetId = slot28CardNetId;
			return true;
		}

		public bool TryWriteSlot28CardDataId(int slot28CardDataId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot28CardDataId = slot28CardDataId;
			return true;
		}

		public bool TryWriteSlot28ColorPacked(int slot28ColorPacked)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot28ColorPacked = slot28ColorPacked;
			return true;
		}

		public bool TryWriteSlot28TargetPlayerId(int slot28TargetPlayerId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot28TargetPlayerId = slot28TargetPlayerId;
			return true;
		}

		public bool TryWriteSlot29CardNetId(int slot29CardNetId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot29CardNetId = slot29CardNetId;
			return true;
		}

		public bool TryWriteSlot29CardDataId(int slot29CardDataId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot29CardDataId = slot29CardDataId;
			return true;
		}

		public bool TryWriteSlot29ColorPacked(int slot29ColorPacked)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot29ColorPacked = slot29ColorPacked;
			return true;
		}

		public bool TryWriteSlot29TargetPlayerId(int slot29TargetPlayerId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot29TargetPlayerId = slot29TargetPlayerId;
			return true;
		}

		public bool TryWriteSlot30CardNetId(int slot30CardNetId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot30CardNetId = slot30CardNetId;
			return true;
		}

		public bool TryWriteSlot30CardDataId(int slot30CardDataId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot30CardDataId = slot30CardDataId;
			return true;
		}

		public bool TryWriteSlot30ColorPacked(int slot30ColorPacked)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot30ColorPacked = slot30ColorPacked;
			return true;
		}

		public bool TryWriteSlot30TargetPlayerId(int slot30TargetPlayerId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot30TargetPlayerId = slot30TargetPlayerId;
			return true;
		}

		public bool TryWriteSlot31CardNetId(int slot31CardNetId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot31CardNetId = slot31CardNetId;
			return true;
		}

		public bool TryWriteSlot31CardDataId(int slot31CardDataId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot31CardDataId = slot31CardDataId;
			return true;
		}

		public bool TryWriteSlot31ColorPacked(int slot31ColorPacked)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot31ColorPacked = slot31ColorPacked;
			return true;
		}

		public bool TryWriteSlot31TargetPlayerId(int slot31TargetPlayerId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot31TargetPlayerId = slot31TargetPlayerId;
			return true;
		}

		private void OnTargetLevelNumberChangedRender()
		{
			this.OnNetworkedTargetLevelNumberChanged?.Invoke(TargetLevelNumber);
		}

		private void OnSlot0CardNetIdChangedRender()
		{
			this.OnNetworkedSlot0CardNetIdChanged?.Invoke(Slot0CardNetId);
		}

		private void OnSlot0CardDataIdChangedRender()
		{
			this.OnNetworkedSlot0CardDataIdChanged?.Invoke(Slot0CardDataId);
		}

		private void OnSlot0ColorPackedChangedRender()
		{
			this.OnNetworkedSlot0ColorPackedChanged?.Invoke(Slot0ColorPacked);
		}

		private void OnSlot0TargetPlayerIdChangedRender()
		{
			this.OnNetworkedSlot0TargetPlayerIdChanged?.Invoke(Slot0TargetPlayerId);
		}

		private void OnSlot1CardNetIdChangedRender()
		{
			this.OnNetworkedSlot1CardNetIdChanged?.Invoke(Slot1CardNetId);
		}

		private void OnSlot1CardDataIdChangedRender()
		{
			this.OnNetworkedSlot1CardDataIdChanged?.Invoke(Slot1CardDataId);
		}

		private void OnSlot1ColorPackedChangedRender()
		{
			this.OnNetworkedSlot1ColorPackedChanged?.Invoke(Slot1ColorPacked);
		}

		private void OnSlot1TargetPlayerIdChangedRender()
		{
			this.OnNetworkedSlot1TargetPlayerIdChanged?.Invoke(Slot1TargetPlayerId);
		}

		private void OnSlot2CardNetIdChangedRender()
		{
			this.OnNetworkedSlot2CardNetIdChanged?.Invoke(Slot2CardNetId);
		}

		private void OnSlot2CardDataIdChangedRender()
		{
			this.OnNetworkedSlot2CardDataIdChanged?.Invoke(Slot2CardDataId);
		}

		private void OnSlot2ColorPackedChangedRender()
		{
			this.OnNetworkedSlot2ColorPackedChanged?.Invoke(Slot2ColorPacked);
		}

		private void OnSlot2TargetPlayerIdChangedRender()
		{
			this.OnNetworkedSlot2TargetPlayerIdChanged?.Invoke(Slot2TargetPlayerId);
		}

		private void OnSlot3CardNetIdChangedRender()
		{
			this.OnNetworkedSlot3CardNetIdChanged?.Invoke(Slot3CardNetId);
		}

		private void OnSlot3CardDataIdChangedRender()
		{
			this.OnNetworkedSlot3CardDataIdChanged?.Invoke(Slot3CardDataId);
		}

		private void OnSlot3ColorPackedChangedRender()
		{
			this.OnNetworkedSlot3ColorPackedChanged?.Invoke(Slot3ColorPacked);
		}

		private void OnSlot3TargetPlayerIdChangedRender()
		{
			this.OnNetworkedSlot3TargetPlayerIdChanged?.Invoke(Slot3TargetPlayerId);
		}

		private void OnSlot4CardNetIdChangedRender()
		{
			this.OnNetworkedSlot4CardNetIdChanged?.Invoke(Slot4CardNetId);
		}

		private void OnSlot4CardDataIdChangedRender()
		{
			this.OnNetworkedSlot4CardDataIdChanged?.Invoke(Slot4CardDataId);
		}

		private void OnSlot4ColorPackedChangedRender()
		{
			this.OnNetworkedSlot4ColorPackedChanged?.Invoke(Slot4ColorPacked);
		}

		private void OnSlot4TargetPlayerIdChangedRender()
		{
			this.OnNetworkedSlot4TargetPlayerIdChanged?.Invoke(Slot4TargetPlayerId);
		}

		private void OnSlot5CardNetIdChangedRender()
		{
			this.OnNetworkedSlot5CardNetIdChanged?.Invoke(Slot5CardNetId);
		}

		private void OnSlot5CardDataIdChangedRender()
		{
			this.OnNetworkedSlot5CardDataIdChanged?.Invoke(Slot5CardDataId);
		}

		private void OnSlot5ColorPackedChangedRender()
		{
			this.OnNetworkedSlot5ColorPackedChanged?.Invoke(Slot5ColorPacked);
		}

		private void OnSlot5TargetPlayerIdChangedRender()
		{
			this.OnNetworkedSlot5TargetPlayerIdChanged?.Invoke(Slot5TargetPlayerId);
		}

		private void OnSlot6CardNetIdChangedRender()
		{
			this.OnNetworkedSlot6CardNetIdChanged?.Invoke(Slot6CardNetId);
		}

		private void OnSlot6CardDataIdChangedRender()
		{
			this.OnNetworkedSlot6CardDataIdChanged?.Invoke(Slot6CardDataId);
		}

		private void OnSlot6ColorPackedChangedRender()
		{
			this.OnNetworkedSlot6ColorPackedChanged?.Invoke(Slot6ColorPacked);
		}

		private void OnSlot6TargetPlayerIdChangedRender()
		{
			this.OnNetworkedSlot6TargetPlayerIdChanged?.Invoke(Slot6TargetPlayerId);
		}

		private void OnSlot7CardNetIdChangedRender()
		{
			this.OnNetworkedSlot7CardNetIdChanged?.Invoke(Slot7CardNetId);
		}

		private void OnSlot7CardDataIdChangedRender()
		{
			this.OnNetworkedSlot7CardDataIdChanged?.Invoke(Slot7CardDataId);
		}

		private void OnSlot7ColorPackedChangedRender()
		{
			this.OnNetworkedSlot7ColorPackedChanged?.Invoke(Slot7ColorPacked);
		}

		private void OnSlot7TargetPlayerIdChangedRender()
		{
			this.OnNetworkedSlot7TargetPlayerIdChanged?.Invoke(Slot7TargetPlayerId);
		}

		private void OnSlot8CardNetIdChangedRender()
		{
			this.OnNetworkedSlot8CardNetIdChanged?.Invoke(Slot8CardNetId);
		}

		private void OnSlot8CardDataIdChangedRender()
		{
			this.OnNetworkedSlot8CardDataIdChanged?.Invoke(Slot8CardDataId);
		}

		private void OnSlot8ColorPackedChangedRender()
		{
			this.OnNetworkedSlot8ColorPackedChanged?.Invoke(Slot8ColorPacked);
		}

		private void OnSlot8TargetPlayerIdChangedRender()
		{
			this.OnNetworkedSlot8TargetPlayerIdChanged?.Invoke(Slot8TargetPlayerId);
		}

		private void OnSlot9CardNetIdChangedRender()
		{
			this.OnNetworkedSlot9CardNetIdChanged?.Invoke(Slot9CardNetId);
		}

		private void OnSlot9CardDataIdChangedRender()
		{
			this.OnNetworkedSlot9CardDataIdChanged?.Invoke(Slot9CardDataId);
		}

		private void OnSlot9ColorPackedChangedRender()
		{
			this.OnNetworkedSlot9ColorPackedChanged?.Invoke(Slot9ColorPacked);
		}

		private void OnSlot9TargetPlayerIdChangedRender()
		{
			this.OnNetworkedSlot9TargetPlayerIdChanged?.Invoke(Slot9TargetPlayerId);
		}

		private void OnSlot10CardNetIdChangedRender()
		{
			this.OnNetworkedSlot10CardNetIdChanged?.Invoke(Slot10CardNetId);
		}

		private void OnSlot10CardDataIdChangedRender()
		{
			this.OnNetworkedSlot10CardDataIdChanged?.Invoke(Slot10CardDataId);
		}

		private void OnSlot10ColorPackedChangedRender()
		{
			this.OnNetworkedSlot10ColorPackedChanged?.Invoke(Slot10ColorPacked);
		}

		private void OnSlot10TargetPlayerIdChangedRender()
		{
			this.OnNetworkedSlot10TargetPlayerIdChanged?.Invoke(Slot10TargetPlayerId);
		}

		private void OnSlot11CardNetIdChangedRender()
		{
			this.OnNetworkedSlot11CardNetIdChanged?.Invoke(Slot11CardNetId);
		}

		private void OnSlot11CardDataIdChangedRender()
		{
			this.OnNetworkedSlot11CardDataIdChanged?.Invoke(Slot11CardDataId);
		}

		private void OnSlot11ColorPackedChangedRender()
		{
			this.OnNetworkedSlot11ColorPackedChanged?.Invoke(Slot11ColorPacked);
		}

		private void OnSlot11TargetPlayerIdChangedRender()
		{
			this.OnNetworkedSlot11TargetPlayerIdChanged?.Invoke(Slot11TargetPlayerId);
		}

		private void OnSlot12CardNetIdChangedRender()
		{
			this.OnNetworkedSlot12CardNetIdChanged?.Invoke(Slot12CardNetId);
		}

		private void OnSlot12CardDataIdChangedRender()
		{
			this.OnNetworkedSlot12CardDataIdChanged?.Invoke(Slot12CardDataId);
		}

		private void OnSlot12ColorPackedChangedRender()
		{
			this.OnNetworkedSlot12ColorPackedChanged?.Invoke(Slot12ColorPacked);
		}

		private void OnSlot12TargetPlayerIdChangedRender()
		{
			this.OnNetworkedSlot12TargetPlayerIdChanged?.Invoke(Slot12TargetPlayerId);
		}

		private void OnSlot13CardNetIdChangedRender()
		{
			this.OnNetworkedSlot13CardNetIdChanged?.Invoke(Slot13CardNetId);
		}

		private void OnSlot13CardDataIdChangedRender()
		{
			this.OnNetworkedSlot13CardDataIdChanged?.Invoke(Slot13CardDataId);
		}

		private void OnSlot13ColorPackedChangedRender()
		{
			this.OnNetworkedSlot13ColorPackedChanged?.Invoke(Slot13ColorPacked);
		}

		private void OnSlot13TargetPlayerIdChangedRender()
		{
			this.OnNetworkedSlot13TargetPlayerIdChanged?.Invoke(Slot13TargetPlayerId);
		}

		private void OnSlot14CardNetIdChangedRender()
		{
			this.OnNetworkedSlot14CardNetIdChanged?.Invoke(Slot14CardNetId);
		}

		private void OnSlot14CardDataIdChangedRender()
		{
			this.OnNetworkedSlot14CardDataIdChanged?.Invoke(Slot14CardDataId);
		}

		private void OnSlot14ColorPackedChangedRender()
		{
			this.OnNetworkedSlot14ColorPackedChanged?.Invoke(Slot14ColorPacked);
		}

		private void OnSlot14TargetPlayerIdChangedRender()
		{
			this.OnNetworkedSlot14TargetPlayerIdChanged?.Invoke(Slot14TargetPlayerId);
		}

		private void OnSlot15CardNetIdChangedRender()
		{
			this.OnNetworkedSlot15CardNetIdChanged?.Invoke(Slot15CardNetId);
		}

		private void OnSlot15CardDataIdChangedRender()
		{
			this.OnNetworkedSlot15CardDataIdChanged?.Invoke(Slot15CardDataId);
		}

		private void OnSlot15ColorPackedChangedRender()
		{
			this.OnNetworkedSlot15ColorPackedChanged?.Invoke(Slot15ColorPacked);
		}

		private void OnSlot15TargetPlayerIdChangedRender()
		{
			this.OnNetworkedSlot15TargetPlayerIdChanged?.Invoke(Slot15TargetPlayerId);
		}

		private void OnSlot16CardNetIdChangedRender()
		{
			this.OnNetworkedSlot16CardNetIdChanged?.Invoke(Slot16CardNetId);
		}

		private void OnSlot16CardDataIdChangedRender()
		{
			this.OnNetworkedSlot16CardDataIdChanged?.Invoke(Slot16CardDataId);
		}

		private void OnSlot16ColorPackedChangedRender()
		{
			this.OnNetworkedSlot16ColorPackedChanged?.Invoke(Slot16ColorPacked);
		}

		private void OnSlot16TargetPlayerIdChangedRender()
		{
			this.OnNetworkedSlot16TargetPlayerIdChanged?.Invoke(Slot16TargetPlayerId);
		}

		private void OnSlot17CardNetIdChangedRender()
		{
			this.OnNetworkedSlot17CardNetIdChanged?.Invoke(Slot17CardNetId);
		}

		private void OnSlot17CardDataIdChangedRender()
		{
			this.OnNetworkedSlot17CardDataIdChanged?.Invoke(Slot17CardDataId);
		}

		private void OnSlot17ColorPackedChangedRender()
		{
			this.OnNetworkedSlot17ColorPackedChanged?.Invoke(Slot17ColorPacked);
		}

		private void OnSlot17TargetPlayerIdChangedRender()
		{
			this.OnNetworkedSlot17TargetPlayerIdChanged?.Invoke(Slot17TargetPlayerId);
		}

		private void OnSlot18CardNetIdChangedRender()
		{
			this.OnNetworkedSlot18CardNetIdChanged?.Invoke(Slot18CardNetId);
		}

		private void OnSlot18CardDataIdChangedRender()
		{
			this.OnNetworkedSlot18CardDataIdChanged?.Invoke(Slot18CardDataId);
		}

		private void OnSlot18ColorPackedChangedRender()
		{
			this.OnNetworkedSlot18ColorPackedChanged?.Invoke(Slot18ColorPacked);
		}

		private void OnSlot18TargetPlayerIdChangedRender()
		{
			this.OnNetworkedSlot18TargetPlayerIdChanged?.Invoke(Slot18TargetPlayerId);
		}

		private void OnSlot19CardNetIdChangedRender()
		{
			this.OnNetworkedSlot19CardNetIdChanged?.Invoke(Slot19CardNetId);
		}

		private void OnSlot19CardDataIdChangedRender()
		{
			this.OnNetworkedSlot19CardDataIdChanged?.Invoke(Slot19CardDataId);
		}

		private void OnSlot19ColorPackedChangedRender()
		{
			this.OnNetworkedSlot19ColorPackedChanged?.Invoke(Slot19ColorPacked);
		}

		private void OnSlot19TargetPlayerIdChangedRender()
		{
			this.OnNetworkedSlot19TargetPlayerIdChanged?.Invoke(Slot19TargetPlayerId);
		}

		private void OnSlot20CardNetIdChangedRender()
		{
			this.OnNetworkedSlot20CardNetIdChanged?.Invoke(Slot20CardNetId);
		}

		private void OnSlot20CardDataIdChangedRender()
		{
			this.OnNetworkedSlot20CardDataIdChanged?.Invoke(Slot20CardDataId);
		}

		private void OnSlot20ColorPackedChangedRender()
		{
			this.OnNetworkedSlot20ColorPackedChanged?.Invoke(Slot20ColorPacked);
		}

		private void OnSlot20TargetPlayerIdChangedRender()
		{
			this.OnNetworkedSlot20TargetPlayerIdChanged?.Invoke(Slot20TargetPlayerId);
		}

		private void OnSlot21CardNetIdChangedRender()
		{
			this.OnNetworkedSlot21CardNetIdChanged?.Invoke(Slot21CardNetId);
		}

		private void OnSlot21CardDataIdChangedRender()
		{
			this.OnNetworkedSlot21CardDataIdChanged?.Invoke(Slot21CardDataId);
		}

		private void OnSlot21ColorPackedChangedRender()
		{
			this.OnNetworkedSlot21ColorPackedChanged?.Invoke(Slot21ColorPacked);
		}

		private void OnSlot21TargetPlayerIdChangedRender()
		{
			this.OnNetworkedSlot21TargetPlayerIdChanged?.Invoke(Slot21TargetPlayerId);
		}

		private void OnSlot22CardNetIdChangedRender()
		{
			this.OnNetworkedSlot22CardNetIdChanged?.Invoke(Slot22CardNetId);
		}

		private void OnSlot22CardDataIdChangedRender()
		{
			this.OnNetworkedSlot22CardDataIdChanged?.Invoke(Slot22CardDataId);
		}

		private void OnSlot22ColorPackedChangedRender()
		{
			this.OnNetworkedSlot22ColorPackedChanged?.Invoke(Slot22ColorPacked);
		}

		private void OnSlot22TargetPlayerIdChangedRender()
		{
			this.OnNetworkedSlot22TargetPlayerIdChanged?.Invoke(Slot22TargetPlayerId);
		}

		private void OnSlot23CardNetIdChangedRender()
		{
			this.OnNetworkedSlot23CardNetIdChanged?.Invoke(Slot23CardNetId);
		}

		private void OnSlot23CardDataIdChangedRender()
		{
			this.OnNetworkedSlot23CardDataIdChanged?.Invoke(Slot23CardDataId);
		}

		private void OnSlot23ColorPackedChangedRender()
		{
			this.OnNetworkedSlot23ColorPackedChanged?.Invoke(Slot23ColorPacked);
		}

		private void OnSlot23TargetPlayerIdChangedRender()
		{
			this.OnNetworkedSlot23TargetPlayerIdChanged?.Invoke(Slot23TargetPlayerId);
		}

		private void OnSlot24CardNetIdChangedRender()
		{
			this.OnNetworkedSlot24CardNetIdChanged?.Invoke(Slot24CardNetId);
		}

		private void OnSlot24CardDataIdChangedRender()
		{
			this.OnNetworkedSlot24CardDataIdChanged?.Invoke(Slot24CardDataId);
		}

		private void OnSlot24ColorPackedChangedRender()
		{
			this.OnNetworkedSlot24ColorPackedChanged?.Invoke(Slot24ColorPacked);
		}

		private void OnSlot24TargetPlayerIdChangedRender()
		{
			this.OnNetworkedSlot24TargetPlayerIdChanged?.Invoke(Slot24TargetPlayerId);
		}

		private void OnSlot25CardNetIdChangedRender()
		{
			this.OnNetworkedSlot25CardNetIdChanged?.Invoke(Slot25CardNetId);
		}

		private void OnSlot25CardDataIdChangedRender()
		{
			this.OnNetworkedSlot25CardDataIdChanged?.Invoke(Slot25CardDataId);
		}

		private void OnSlot25ColorPackedChangedRender()
		{
			this.OnNetworkedSlot25ColorPackedChanged?.Invoke(Slot25ColorPacked);
		}

		private void OnSlot25TargetPlayerIdChangedRender()
		{
			this.OnNetworkedSlot25TargetPlayerIdChanged?.Invoke(Slot25TargetPlayerId);
		}

		private void OnSlot26CardNetIdChangedRender()
		{
			this.OnNetworkedSlot26CardNetIdChanged?.Invoke(Slot26CardNetId);
		}

		private void OnSlot26CardDataIdChangedRender()
		{
			this.OnNetworkedSlot26CardDataIdChanged?.Invoke(Slot26CardDataId);
		}

		private void OnSlot26ColorPackedChangedRender()
		{
			this.OnNetworkedSlot26ColorPackedChanged?.Invoke(Slot26ColorPacked);
		}

		private void OnSlot26TargetPlayerIdChangedRender()
		{
			this.OnNetworkedSlot26TargetPlayerIdChanged?.Invoke(Slot26TargetPlayerId);
		}

		private void OnSlot27CardNetIdChangedRender()
		{
			this.OnNetworkedSlot27CardNetIdChanged?.Invoke(Slot27CardNetId);
		}

		private void OnSlot27CardDataIdChangedRender()
		{
			this.OnNetworkedSlot27CardDataIdChanged?.Invoke(Slot27CardDataId);
		}

		private void OnSlot27ColorPackedChangedRender()
		{
			this.OnNetworkedSlot27ColorPackedChanged?.Invoke(Slot27ColorPacked);
		}

		private void OnSlot27TargetPlayerIdChangedRender()
		{
			this.OnNetworkedSlot27TargetPlayerIdChanged?.Invoke(Slot27TargetPlayerId);
		}

		private void OnSlot28CardNetIdChangedRender()
		{
			this.OnNetworkedSlot28CardNetIdChanged?.Invoke(Slot28CardNetId);
		}

		private void OnSlot28CardDataIdChangedRender()
		{
			this.OnNetworkedSlot28CardDataIdChanged?.Invoke(Slot28CardDataId);
		}

		private void OnSlot28ColorPackedChangedRender()
		{
			this.OnNetworkedSlot28ColorPackedChanged?.Invoke(Slot28ColorPacked);
		}

		private void OnSlot28TargetPlayerIdChangedRender()
		{
			this.OnNetworkedSlot28TargetPlayerIdChanged?.Invoke(Slot28TargetPlayerId);
		}

		private void OnSlot29CardNetIdChangedRender()
		{
			this.OnNetworkedSlot29CardNetIdChanged?.Invoke(Slot29CardNetId);
		}

		private void OnSlot29CardDataIdChangedRender()
		{
			this.OnNetworkedSlot29CardDataIdChanged?.Invoke(Slot29CardDataId);
		}

		private void OnSlot29ColorPackedChangedRender()
		{
			this.OnNetworkedSlot29ColorPackedChanged?.Invoke(Slot29ColorPacked);
		}

		private void OnSlot29TargetPlayerIdChangedRender()
		{
			this.OnNetworkedSlot29TargetPlayerIdChanged?.Invoke(Slot29TargetPlayerId);
		}

		private void OnSlot30CardNetIdChangedRender()
		{
			this.OnNetworkedSlot30CardNetIdChanged?.Invoke(Slot30CardNetId);
		}

		private void OnSlot30CardDataIdChangedRender()
		{
			this.OnNetworkedSlot30CardDataIdChanged?.Invoke(Slot30CardDataId);
		}

		private void OnSlot30ColorPackedChangedRender()
		{
			this.OnNetworkedSlot30ColorPackedChanged?.Invoke(Slot30ColorPacked);
		}

		private void OnSlot30TargetPlayerIdChangedRender()
		{
			this.OnNetworkedSlot30TargetPlayerIdChanged?.Invoke(Slot30TargetPlayerId);
		}

		private void OnSlot31CardNetIdChangedRender()
		{
			this.OnNetworkedSlot31CardNetIdChanged?.Invoke(Slot31CardNetId);
		}

		private void OnSlot31CardDataIdChangedRender()
		{
			this.OnNetworkedSlot31CardDataIdChanged?.Invoke(Slot31CardDataId);
		}

		private void OnSlot31ColorPackedChangedRender()
		{
			this.OnNetworkedSlot31ColorPackedChanged?.Invoke(Slot31ColorPacked);
		}

		private void OnSlot31TargetPlayerIdChangedRender()
		{
			this.OnNetworkedSlot31TargetPlayerIdChanged?.Invoke(Slot31TargetPlayerId);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			TargetLevelNumber = _TargetLevelNumber;
			Slot0CardNetId = _Slot0CardNetId;
			Slot0CardDataId = _Slot0CardDataId;
			Slot0ColorPacked = _Slot0ColorPacked;
			Slot0TargetPlayerId = _Slot0TargetPlayerId;
			Slot1CardNetId = _Slot1CardNetId;
			Slot1CardDataId = _Slot1CardDataId;
			Slot1ColorPacked = _Slot1ColorPacked;
			Slot1TargetPlayerId = _Slot1TargetPlayerId;
			Slot2CardNetId = _Slot2CardNetId;
			Slot2CardDataId = _Slot2CardDataId;
			Slot2ColorPacked = _Slot2ColorPacked;
			Slot2TargetPlayerId = _Slot2TargetPlayerId;
			Slot3CardNetId = _Slot3CardNetId;
			Slot3CardDataId = _Slot3CardDataId;
			Slot3ColorPacked = _Slot3ColorPacked;
			Slot3TargetPlayerId = _Slot3TargetPlayerId;
			Slot4CardNetId = _Slot4CardNetId;
			Slot4CardDataId = _Slot4CardDataId;
			Slot4ColorPacked = _Slot4ColorPacked;
			Slot4TargetPlayerId = _Slot4TargetPlayerId;
			Slot5CardNetId = _Slot5CardNetId;
			Slot5CardDataId = _Slot5CardDataId;
			Slot5ColorPacked = _Slot5ColorPacked;
			Slot5TargetPlayerId = _Slot5TargetPlayerId;
			Slot6CardNetId = _Slot6CardNetId;
			Slot6CardDataId = _Slot6CardDataId;
			Slot6ColorPacked = _Slot6ColorPacked;
			Slot6TargetPlayerId = _Slot6TargetPlayerId;
			Slot7CardNetId = _Slot7CardNetId;
			Slot7CardDataId = _Slot7CardDataId;
			Slot7ColorPacked = _Slot7ColorPacked;
			Slot7TargetPlayerId = _Slot7TargetPlayerId;
			Slot8CardNetId = _Slot8CardNetId;
			Slot8CardDataId = _Slot8CardDataId;
			Slot8ColorPacked = _Slot8ColorPacked;
			Slot8TargetPlayerId = _Slot8TargetPlayerId;
			Slot9CardNetId = _Slot9CardNetId;
			Slot9CardDataId = _Slot9CardDataId;
			Slot9ColorPacked = _Slot9ColorPacked;
			Slot9TargetPlayerId = _Slot9TargetPlayerId;
			Slot10CardNetId = _Slot10CardNetId;
			Slot10CardDataId = _Slot10CardDataId;
			Slot10ColorPacked = _Slot10ColorPacked;
			Slot10TargetPlayerId = _Slot10TargetPlayerId;
			Slot11CardNetId = _Slot11CardNetId;
			Slot11CardDataId = _Slot11CardDataId;
			Slot11ColorPacked = _Slot11ColorPacked;
			Slot11TargetPlayerId = _Slot11TargetPlayerId;
			Slot12CardNetId = _Slot12CardNetId;
			Slot12CardDataId = _Slot12CardDataId;
			Slot12ColorPacked = _Slot12ColorPacked;
			Slot12TargetPlayerId = _Slot12TargetPlayerId;
			Slot13CardNetId = _Slot13CardNetId;
			Slot13CardDataId = _Slot13CardDataId;
			Slot13ColorPacked = _Slot13ColorPacked;
			Slot13TargetPlayerId = _Slot13TargetPlayerId;
			Slot14CardNetId = _Slot14CardNetId;
			Slot14CardDataId = _Slot14CardDataId;
			Slot14ColorPacked = _Slot14ColorPacked;
			Slot14TargetPlayerId = _Slot14TargetPlayerId;
			Slot15CardNetId = _Slot15CardNetId;
			Slot15CardDataId = _Slot15CardDataId;
			Slot15ColorPacked = _Slot15ColorPacked;
			Slot15TargetPlayerId = _Slot15TargetPlayerId;
			Slot16CardNetId = _Slot16CardNetId;
			Slot16CardDataId = _Slot16CardDataId;
			Slot16ColorPacked = _Slot16ColorPacked;
			Slot16TargetPlayerId = _Slot16TargetPlayerId;
			Slot17CardNetId = _Slot17CardNetId;
			Slot17CardDataId = _Slot17CardDataId;
			Slot17ColorPacked = _Slot17ColorPacked;
			Slot17TargetPlayerId = _Slot17TargetPlayerId;
			Slot18CardNetId = _Slot18CardNetId;
			Slot18CardDataId = _Slot18CardDataId;
			Slot18ColorPacked = _Slot18ColorPacked;
			Slot18TargetPlayerId = _Slot18TargetPlayerId;
			Slot19CardNetId = _Slot19CardNetId;
			Slot19CardDataId = _Slot19CardDataId;
			Slot19ColorPacked = _Slot19ColorPacked;
			Slot19TargetPlayerId = _Slot19TargetPlayerId;
			Slot20CardNetId = _Slot20CardNetId;
			Slot20CardDataId = _Slot20CardDataId;
			Slot20ColorPacked = _Slot20ColorPacked;
			Slot20TargetPlayerId = _Slot20TargetPlayerId;
			Slot21CardNetId = _Slot21CardNetId;
			Slot21CardDataId = _Slot21CardDataId;
			Slot21ColorPacked = _Slot21ColorPacked;
			Slot21TargetPlayerId = _Slot21TargetPlayerId;
			Slot22CardNetId = _Slot22CardNetId;
			Slot22CardDataId = _Slot22CardDataId;
			Slot22ColorPacked = _Slot22ColorPacked;
			Slot22TargetPlayerId = _Slot22TargetPlayerId;
			Slot23CardNetId = _Slot23CardNetId;
			Slot23CardDataId = _Slot23CardDataId;
			Slot23ColorPacked = _Slot23ColorPacked;
			Slot23TargetPlayerId = _Slot23TargetPlayerId;
			Slot24CardNetId = _Slot24CardNetId;
			Slot24CardDataId = _Slot24CardDataId;
			Slot24ColorPacked = _Slot24ColorPacked;
			Slot24TargetPlayerId = _Slot24TargetPlayerId;
			Slot25CardNetId = _Slot25CardNetId;
			Slot25CardDataId = _Slot25CardDataId;
			Slot25ColorPacked = _Slot25ColorPacked;
			Slot25TargetPlayerId = _Slot25TargetPlayerId;
			Slot26CardNetId = _Slot26CardNetId;
			Slot26CardDataId = _Slot26CardDataId;
			Slot26ColorPacked = _Slot26ColorPacked;
			Slot26TargetPlayerId = _Slot26TargetPlayerId;
			Slot27CardNetId = _Slot27CardNetId;
			Slot27CardDataId = _Slot27CardDataId;
			Slot27ColorPacked = _Slot27ColorPacked;
			Slot27TargetPlayerId = _Slot27TargetPlayerId;
			Slot28CardNetId = _Slot28CardNetId;
			Slot28CardDataId = _Slot28CardDataId;
			Slot28ColorPacked = _Slot28ColorPacked;
			Slot28TargetPlayerId = _Slot28TargetPlayerId;
			Slot29CardNetId = _Slot29CardNetId;
			Slot29CardDataId = _Slot29CardDataId;
			Slot29ColorPacked = _Slot29ColorPacked;
			Slot29TargetPlayerId = _Slot29TargetPlayerId;
			Slot30CardNetId = _Slot30CardNetId;
			Slot30CardDataId = _Slot30CardDataId;
			Slot30ColorPacked = _Slot30ColorPacked;
			Slot30TargetPlayerId = _Slot30TargetPlayerId;
			Slot31CardNetId = _Slot31CardNetId;
			Slot31CardDataId = _Slot31CardDataId;
			Slot31ColorPacked = _Slot31ColorPacked;
			Slot31TargetPlayerId = _Slot31TargetPlayerId;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_TargetLevelNumber = TargetLevelNumber;
			_Slot0CardNetId = Slot0CardNetId;
			_Slot0CardDataId = Slot0CardDataId;
			_Slot0ColorPacked = Slot0ColorPacked;
			_Slot0TargetPlayerId = Slot0TargetPlayerId;
			_Slot1CardNetId = Slot1CardNetId;
			_Slot1CardDataId = Slot1CardDataId;
			_Slot1ColorPacked = Slot1ColorPacked;
			_Slot1TargetPlayerId = Slot1TargetPlayerId;
			_Slot2CardNetId = Slot2CardNetId;
			_Slot2CardDataId = Slot2CardDataId;
			_Slot2ColorPacked = Slot2ColorPacked;
			_Slot2TargetPlayerId = Slot2TargetPlayerId;
			_Slot3CardNetId = Slot3CardNetId;
			_Slot3CardDataId = Slot3CardDataId;
			_Slot3ColorPacked = Slot3ColorPacked;
			_Slot3TargetPlayerId = Slot3TargetPlayerId;
			_Slot4CardNetId = Slot4CardNetId;
			_Slot4CardDataId = Slot4CardDataId;
			_Slot4ColorPacked = Slot4ColorPacked;
			_Slot4TargetPlayerId = Slot4TargetPlayerId;
			_Slot5CardNetId = Slot5CardNetId;
			_Slot5CardDataId = Slot5CardDataId;
			_Slot5ColorPacked = Slot5ColorPacked;
			_Slot5TargetPlayerId = Slot5TargetPlayerId;
			_Slot6CardNetId = Slot6CardNetId;
			_Slot6CardDataId = Slot6CardDataId;
			_Slot6ColorPacked = Slot6ColorPacked;
			_Slot6TargetPlayerId = Slot6TargetPlayerId;
			_Slot7CardNetId = Slot7CardNetId;
			_Slot7CardDataId = Slot7CardDataId;
			_Slot7ColorPacked = Slot7ColorPacked;
			_Slot7TargetPlayerId = Slot7TargetPlayerId;
			_Slot8CardNetId = Slot8CardNetId;
			_Slot8CardDataId = Slot8CardDataId;
			_Slot8ColorPacked = Slot8ColorPacked;
			_Slot8TargetPlayerId = Slot8TargetPlayerId;
			_Slot9CardNetId = Slot9CardNetId;
			_Slot9CardDataId = Slot9CardDataId;
			_Slot9ColorPacked = Slot9ColorPacked;
			_Slot9TargetPlayerId = Slot9TargetPlayerId;
			_Slot10CardNetId = Slot10CardNetId;
			_Slot10CardDataId = Slot10CardDataId;
			_Slot10ColorPacked = Slot10ColorPacked;
			_Slot10TargetPlayerId = Slot10TargetPlayerId;
			_Slot11CardNetId = Slot11CardNetId;
			_Slot11CardDataId = Slot11CardDataId;
			_Slot11ColorPacked = Slot11ColorPacked;
			_Slot11TargetPlayerId = Slot11TargetPlayerId;
			_Slot12CardNetId = Slot12CardNetId;
			_Slot12CardDataId = Slot12CardDataId;
			_Slot12ColorPacked = Slot12ColorPacked;
			_Slot12TargetPlayerId = Slot12TargetPlayerId;
			_Slot13CardNetId = Slot13CardNetId;
			_Slot13CardDataId = Slot13CardDataId;
			_Slot13ColorPacked = Slot13ColorPacked;
			_Slot13TargetPlayerId = Slot13TargetPlayerId;
			_Slot14CardNetId = Slot14CardNetId;
			_Slot14CardDataId = Slot14CardDataId;
			_Slot14ColorPacked = Slot14ColorPacked;
			_Slot14TargetPlayerId = Slot14TargetPlayerId;
			_Slot15CardNetId = Slot15CardNetId;
			_Slot15CardDataId = Slot15CardDataId;
			_Slot15ColorPacked = Slot15ColorPacked;
			_Slot15TargetPlayerId = Slot15TargetPlayerId;
			_Slot16CardNetId = Slot16CardNetId;
			_Slot16CardDataId = Slot16CardDataId;
			_Slot16ColorPacked = Slot16ColorPacked;
			_Slot16TargetPlayerId = Slot16TargetPlayerId;
			_Slot17CardNetId = Slot17CardNetId;
			_Slot17CardDataId = Slot17CardDataId;
			_Slot17ColorPacked = Slot17ColorPacked;
			_Slot17TargetPlayerId = Slot17TargetPlayerId;
			_Slot18CardNetId = Slot18CardNetId;
			_Slot18CardDataId = Slot18CardDataId;
			_Slot18ColorPacked = Slot18ColorPacked;
			_Slot18TargetPlayerId = Slot18TargetPlayerId;
			_Slot19CardNetId = Slot19CardNetId;
			_Slot19CardDataId = Slot19CardDataId;
			_Slot19ColorPacked = Slot19ColorPacked;
			_Slot19TargetPlayerId = Slot19TargetPlayerId;
			_Slot20CardNetId = Slot20CardNetId;
			_Slot20CardDataId = Slot20CardDataId;
			_Slot20ColorPacked = Slot20ColorPacked;
			_Slot20TargetPlayerId = Slot20TargetPlayerId;
			_Slot21CardNetId = Slot21CardNetId;
			_Slot21CardDataId = Slot21CardDataId;
			_Slot21ColorPacked = Slot21ColorPacked;
			_Slot21TargetPlayerId = Slot21TargetPlayerId;
			_Slot22CardNetId = Slot22CardNetId;
			_Slot22CardDataId = Slot22CardDataId;
			_Slot22ColorPacked = Slot22ColorPacked;
			_Slot22TargetPlayerId = Slot22TargetPlayerId;
			_Slot23CardNetId = Slot23CardNetId;
			_Slot23CardDataId = Slot23CardDataId;
			_Slot23ColorPacked = Slot23ColorPacked;
			_Slot23TargetPlayerId = Slot23TargetPlayerId;
			_Slot24CardNetId = Slot24CardNetId;
			_Slot24CardDataId = Slot24CardDataId;
			_Slot24ColorPacked = Slot24ColorPacked;
			_Slot24TargetPlayerId = Slot24TargetPlayerId;
			_Slot25CardNetId = Slot25CardNetId;
			_Slot25CardDataId = Slot25CardDataId;
			_Slot25ColorPacked = Slot25ColorPacked;
			_Slot25TargetPlayerId = Slot25TargetPlayerId;
			_Slot26CardNetId = Slot26CardNetId;
			_Slot26CardDataId = Slot26CardDataId;
			_Slot26ColorPacked = Slot26ColorPacked;
			_Slot26TargetPlayerId = Slot26TargetPlayerId;
			_Slot27CardNetId = Slot27CardNetId;
			_Slot27CardDataId = Slot27CardDataId;
			_Slot27ColorPacked = Slot27ColorPacked;
			_Slot27TargetPlayerId = Slot27TargetPlayerId;
			_Slot28CardNetId = Slot28CardNetId;
			_Slot28CardDataId = Slot28CardDataId;
			_Slot28ColorPacked = Slot28ColorPacked;
			_Slot28TargetPlayerId = Slot28TargetPlayerId;
			_Slot29CardNetId = Slot29CardNetId;
			_Slot29CardDataId = Slot29CardDataId;
			_Slot29ColorPacked = Slot29ColorPacked;
			_Slot29TargetPlayerId = Slot29TargetPlayerId;
			_Slot30CardNetId = Slot30CardNetId;
			_Slot30CardDataId = Slot30CardDataId;
			_Slot30ColorPacked = Slot30ColorPacked;
			_Slot30TargetPlayerId = Slot30TargetPlayerId;
			_Slot31CardNetId = Slot31CardNetId;
			_Slot31CardDataId = Slot31CardDataId;
			_Slot31ColorPacked = Slot31ColorPacked;
			_Slot31TargetPlayerId = Slot31TargetPlayerId;
		}
	}
}
