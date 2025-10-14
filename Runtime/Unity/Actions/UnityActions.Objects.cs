using UnityEngine;

namespace LunyScratch
{
	internal sealed partial class UnityActions
	{
		public IEngineObject InstantiatePrefab(IEnginePrefabAsset prefab, ITransform likeTransform)
		{
			var unityPrefab = prefab as ScratchPrefabAsset;
			var unityTransform = likeTransform as ScratchTransform;
			if (unityPrefab == null || unityTransform == null || unityPrefab.Prefab == null)
			{
				return null;
			}

			var t = unityTransform.Transform;
			var instance = Object.Instantiate(unityPrefab.Prefab, t.position, t.rotation);
			return new ScratchGameObject(instance);
		}
	}
}
