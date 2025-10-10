using UnityEngine;

namespace LunyScratch
{
	internal sealed partial class UnityActions
	{
		public IEngineObject InstantiatePrefab(IEnginePrefabAsset prefab, ITransform likeTransform)
		{
			var unityPrefab = prefab as UnityPrefabAsset;
			var unityTransform = likeTransform as UnityTransform;
			if (unityPrefab == null || unityTransform == null || unityPrefab.Prefab == null)
			{
				return null;
			}

			var t = unityTransform.Transform;
			var instance = Object.Instantiate(unityPrefab.Prefab, t.position, t.rotation);
			return new UnityGameObject(instance);
		}
	}
}
