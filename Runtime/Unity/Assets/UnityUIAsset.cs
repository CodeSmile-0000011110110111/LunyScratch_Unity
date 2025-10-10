#if UNITY_2021_1_OR_NEWER
using UnityEngine;
using UnityEngine.UIElements;

namespace LunyScratch
{
	/// <summary>
	/// Unity implementation of a UI asset wrapper (UI Toolkit VisualTreeAsset).
	/// </summary>
	[System.Serializable]
 public sealed class UnityUIAsset : IEngineUIAsset
	{
		[SerializeField] private VisualTreeAsset _asset;

		public UnityUIAsset() { }
		public UnityUIAsset(VisualTreeAsset asset) { _asset = asset; }

		public VisualTreeAsset Asset => _asset;
	}
}
#endif
