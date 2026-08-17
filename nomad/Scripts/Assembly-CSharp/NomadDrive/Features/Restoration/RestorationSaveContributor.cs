using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using EvilCore.EvilSave;
using Mirror;
using NomadDrive.Features.SaveSystem;
using PaintCore;
using UnityEngine;

namespace NomadDrive.Features.Restoration
{
	[DisallowMultipleComponent]
	public class RestorationSaveContributor : MonoBehaviour, INetworkSaveable
	{
		private float[] _restoredPaintAges;

		private bool[][] _restoredHasData;

		public string ContributorKey => "restoration";

		public void CaptureState(EvilWriter writer, ISaveContext ctx)
		{
			List<RestorableVehicleSurface> ownedSurfaces = GetOwnedSurfaces();
			string guid = ResolveOwnerGuid();
			writer.Write((byte)1);
			writer.Write(ownedSurfaces.Count);
			for (int i = 0; i < ownedSurfaces.Count; i++)
			{
				RestorableVehicleSurface restorableVehicleSurface = ownedSurfaces[i];
				writer.Write(restorableVehicleSurface.GetPaintAge());
				WriteTexture(writer, guid, i, 0, restorableVehicleSurface.PaintColorTexture);
				WriteTexture(writer, guid, i, 1, restorableVehicleSurface.DirtMaskTexture);
				WriteTexture(writer, guid, i, 2, restorableVehicleSurface.RustMaskTexture);
				WriteTexture(writer, guid, i, 3, restorableVehicleSurface.PaintMaskTexture);
				WriteTexture(writer, guid, i, 4, restorableVehicleSurface.PolishMaskTexture);
				WriteTexture(writer, guid, i, 5, restorableVehicleSurface.MetalMaskTexture);
			}
		}

		private void WriteTexture(EvilWriter writer, string guid, int ownedIndex, int texType, CwPaintableTexture tex)
		{
			byte[] array = ((tex != null && tex.Activated) ? tex.GetPngData() : null);
			bool flag = array != null && array.Length != 0 && !string.IsNullOrEmpty(guid);
			writer.Write(flag);
			if (flag)
			{
				EvilSave.SaveRaw(SideCarKey(guid, ownedIndex, texType), array);
			}
		}

		public void RestoreSelfState(EvilReader reader, ISaveContext ctx)
		{
			reader.ReadByte();
			int num = reader.ReadInt();
			_restoredPaintAges = new float[num];
			_restoredHasData = new bool[num][];
			for (int i = 0; i < num; i++)
			{
				_restoredPaintAges[i] = reader.ReadFloat();
				bool[] array = new bool[6];
				for (int j = 0; j < 6; j++)
				{
					array[j] = reader.ReadBool();
				}
				_restoredHasData[i] = array;
			}
			if (NetworkServer.active)
			{
				ApplyRestoredSurfacesAsync(this.GetCancellationTokenOnDestroy()).Forget();
			}
		}

		public void RestoreLinks(EvilReader reader, ISaveContext ctx)
		{
		}

		private async UniTaskVoid ApplyRestoredSurfacesAsync(CancellationToken ct)
		{
			await UniTask.Yield(ct);
			string guid = ResolveOwnerGuid();
			if (string.IsNullOrEmpty(guid) || _restoredHasData == null)
			{
				return;
			}
			List<RestorableVehicleSurface> owned = GetOwnedSurfaces();
			RestorationNetworkSync sync = GetComponentInChildren<RestorationNetworkSync>(includeInactive: true);
			int count = Mathf.Min(owned.Count, _restoredHasData.Length);
			for (int i = 0; i < count; i++)
			{
				RestorableVehicleSurface s = owned[i];
				bool[] flags = _restoredHasData[i];
				await ApplyTextureFromSave(guid, i, 0, s.PaintColorTexture, flags[0], ct);
				await ApplyTextureFromSave(guid, i, 1, s.DirtMaskTexture, flags[1], ct);
				await ApplyTextureFromSave(guid, i, 2, s.RustMaskTexture, flags[2], ct);
				await ApplyTextureFromSave(guid, i, 3, s.PaintMaskTexture, flags[3], ct);
				await ApplyTextureFromSave(guid, i, 4, s.PolishMaskTexture, flags[4], ct);
				await ApplyTextureFromSave(guid, i, 5, s.MetalMaskTexture, flags[5], ct);
				float num = _restoredPaintAges[i];
				s.SetPaintAge(num);
				if (sync != null)
				{
					sync.SetPaintAge(s.SurfaceIndex, num);
				}
			}
		}

		private async UniTask ApplyTextureFromSave(string guid, int ownedIndex, int texType, CwPaintableTexture tex, bool hasData, CancellationToken ct)
		{
			if (!hasData || tex == null)
			{
				return;
			}
			string key = SideCarKey(guid, ownedIndex, texType);
			if (!EvilSave.HasKey(key))
			{
				return;
			}
			byte[] data = EvilSave.LoadRaw(key);
			if (data == null || data.Length == 0)
			{
				return;
			}
			for (int i = 0; i < 50; i++)
			{
				if (tex.Activated)
				{
					break;
				}
				if (ct.IsCancellationRequested)
				{
					return;
				}
				await UniTask.Delay(100, ignoreTimeScale: false, PlayerLoopTiming.Update, ct);
			}
			if (tex.Activated)
			{
				tex.LoadFromData(data);
			}
		}

		private string ResolveOwnerGuid()
		{
			PersistentId componentInParent = GetComponentInParent<PersistentId>();
			if (!(componentInParent != null))
			{
				return null;
			}
			return componentInParent.Guid;
		}

		private static string SideCarKey(string guid, int ownedIndex, int texType)
		{
			return $"restoration.{guid}.surf{ownedIndex}.tex{texType}";
		}

		private List<RestorableVehicleSurface> GetOwnedSurfaces()
		{
			List<RestorableVehicleSurface> list = new List<RestorableVehicleSurface>();
			RestorableVehicleSurface[] componentsInChildren = GetComponentsInChildren<RestorableVehicleSurface>(includeInactive: true);
			PersistentObject componentInParent = GetComponentInParent<PersistentObject>(includeInactive: true);
			RestorableVehicleSurface[] array = componentsInChildren;
			foreach (RestorableVehicleSurface restorableVehicleSurface in array)
			{
				if (!(restorableVehicleSurface == null) && restorableVehicleSurface.GetComponentInParent<PersistentObject>(includeInactive: true) == componentInParent)
				{
					list.Add(restorableVehicleSurface);
				}
			}
			return list;
		}
	}
}
