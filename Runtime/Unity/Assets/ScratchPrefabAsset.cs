using System;
using UnityEngine;

namespace LunyScratch
{
	/// <summary>
	/// Unity implementation of a prefab asset wrapper.
	/// </summary>
	[Serializable]
	public sealed class ScratchPrefabAsset : IEnginePrefabAsset
	{
		[SerializeField] private GameObject _prefab;

		public GameObject Prefab => _prefab;

		public ScratchPrefabAsset() {}
		public ScratchPrefabAsset(GameObject prefab) => _prefab = prefab;
	}
}
