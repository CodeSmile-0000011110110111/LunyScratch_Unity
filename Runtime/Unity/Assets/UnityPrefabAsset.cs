using UnityEngine;

namespace LunyScratch
{
	/// <summary>
	/// Unity implementation of a prefab asset wrapper.
	/// </summary>
	[System.Serializable]
 public sealed class UnityPrefabAsset : IEnginePrefabAsset
	{
		[SerializeField] private GameObject _prefab;

		public UnityPrefabAsset() { }
		public UnityPrefabAsset(GameObject prefab) { _prefab = prefab; }

		public GameObject Prefab => _prefab;
	}
}
