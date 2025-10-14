#if UNITY_2021_1_OR_NEWER
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace LunyScratch
{
	/// <summary>
	/// Unity implementation of a UI asset wrapper (UI Toolkit VisualTreeAsset).
	/// </summary>
	[Serializable]
	public sealed class ScratchUIAsset : IEngineUIAsset
	{
		[SerializeField] private VisualTreeAsset _asset;

		public VisualTreeAsset Asset => _asset;

		public ScratchUIAsset() {}
		public ScratchUIAsset(VisualTreeAsset asset) => _asset = asset;
	}
}
#endif
