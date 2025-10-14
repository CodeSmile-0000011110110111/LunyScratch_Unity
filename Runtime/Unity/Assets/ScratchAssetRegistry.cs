using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_2021_1_OR_NEWER
#endif

namespace LunyScratch
{
	/// <summary>
	/// ScriptableObject that stores mappings from asset paths to engine-specific asset wrappers.
	/// Built/updated by editor scripts and used at runtime for fast lookups.
	/// </summary>
	public sealed class ScratchAssetRegistry : ScriptableObject, AssetRegistry.IAssetRegistry
	{
		[SerializeField] private List<PrefabEntry> _prefabs = new();
		[SerializeField] private List<UIEntry> _ui = new();
		[SerializeField] private List<AudioEntry> _audio = new();

		// Optional placeholders to return when requested asset is missing.
		[SerializeField] private ScratchPrefabAsset _defaultPrefab;
		[SerializeField] private ScratchUIAsset _defaultUI;
		[SerializeField] private ScratchAudioAsset _defaultAudio;

		[NonSerialized] private Dictionary<String, ScratchPrefabAsset> _prefabMap;
		[NonSerialized] private Dictionary<String, ScratchUIAsset> _uiMap;
		[NonSerialized] private Dictionary<String, ScratchAudioAsset> _audioMap;

		// Expose for editor builder
		public List<(String path, ScratchPrefabAsset asset)> PrefabsForEditor
		{
			get
			{
				var list = new List<(String, ScratchPrefabAsset)>(_prefabs.Count);
				foreach (var e in _prefabs) list.Add((e.Path, e.Asset));
				return list;
			}
		}

		public List<(String path, ScratchUIAsset asset)> UIForEditor
		{
			get
			{
				var list = new List<(String, ScratchUIAsset)>(_ui.Count);
				foreach (var e in _ui)
					list.Add((e.Path, e.Asset));
				return list;
			}
		}

		public List<(String path, ScratchAudioAsset asset)> AudioForEditor
		{
			get
			{
				var list = new List<(String, ScratchAudioAsset)>(_audio.Count);
				foreach (var e in _audio)
					list.Add((e.Path, e.Asset));
				return list;
			}
		}

		private static TVal TryGet<TVal>(Dictionary<String, TVal> map, String path) where TVal : class
		{
			if (map == null || String.IsNullOrEmpty(path)) return null;

			return map.TryGetValue(path, out var v) ? v : null;
		}

		public T Get<T>(String path) where T : class, IEngineAsset
		{
			if (typeof(T) == typeof(IEnginePrefabAsset) || typeof(T) == typeof(ScratchPrefabAsset))
				return TryGet(_prefabMap, path) as T;

			if (typeof(T) == typeof(IEngineUIAsset) || typeof(T) == typeof(ScratchUIAsset))
				return TryGet(_uiMap, path) as T;

			if (typeof(T) == typeof(IEngineAudioAsset) || typeof(T) == typeof(ScratchAudioAsset))
				return TryGet(_audioMap, path) as T;

			return null;
		}

		public IEngineAsset Get(String path, Type assetType)
		{
			if (assetType == typeof(IEnginePrefabAsset) || assetType == typeof(ScratchPrefabAsset))
				return TryGet(_prefabMap, path);
			if (assetType == typeof(IEngineUIAsset) || assetType == typeof(ScratchUIAsset))
				return TryGet(_uiMap, path);
			if (assetType == typeof(IEngineAudioAsset) || assetType == typeof(ScratchAudioAsset))
				return TryGet(_audioMap, path);

			return null;
		}

		public T GetPlaceholder<T>() where T : class, IEngineAsset
		{
			if (typeof(T) == typeof(IEnginePrefabAsset) || typeof(T) == typeof(ScratchPrefabAsset))
				return _defaultPrefab as T;
			if (typeof(T) == typeof(IEngineUIAsset) || typeof(T) == typeof(ScratchUIAsset))
				return _defaultUI as T;
			if (typeof(T) == typeof(IEngineAudioAsset) || typeof(T) == typeof(ScratchAudioAsset))
				return _defaultAudio as T;

			return null;
		}

		public IEngineAsset GetPlaceholder(Type assetType)
		{
			if (assetType == typeof(IEnginePrefabAsset) || assetType == typeof(ScratchPrefabAsset))
				return _defaultPrefab;
			if (assetType == typeof(IEngineUIAsset) || assetType == typeof(ScratchUIAsset))
				return _defaultUI;
			if (assetType == typeof(IEngineAudioAsset) || assetType == typeof(ScratchAudioAsset))
				return _defaultAudio;

			return null;
		}

		private void OnEnable() => BuildDictionaries();

		public void BuildDictionaries()
		{
			_prefabMap = new Dictionary<String, ScratchPrefabAsset>(StringComparer.OrdinalIgnoreCase);
			foreach (var e in _prefabs)
				if (!String.IsNullOrEmpty(e?.Path))
					_prefabMap[e.Path] = e.Asset;

			_uiMap = new Dictionary<String, ScratchUIAsset>(StringComparer.OrdinalIgnoreCase);
			foreach (var e in _ui)
				if (!String.IsNullOrEmpty(e?.Path))
					_uiMap[e.Path] = e.Asset;

			_audioMap = new Dictionary<String, ScratchAudioAsset>(StringComparer.OrdinalIgnoreCase);
			foreach (var e in _audio)
				if (!String.IsNullOrEmpty(e?.Path))
					_audioMap[e.Path] = e.Asset;
		}

		public void SetPrefabsForEditor(List<(String path, ScratchPrefabAsset asset)> items)
		{
			_prefabs.Clear();
			foreach (var (path, asset) in items)
				_prefabs.Add(new PrefabEntry { Path = path, Asset = asset });
		}

		public void SetUIForEditor(List<(String path, ScratchUIAsset asset)> items)
		{
			_ui.Clear();
			foreach (var (path, asset) in items)
				_ui.Add(new UIEntry { Path = path, Asset = asset });
		}

		public void SetAudioForEditor(List<(String path, ScratchAudioAsset asset)> items)
		{
			_audio.Clear();
			foreach (var (path, asset) in items)
				_audio.Add(new AudioEntry { Path = path, Asset = asset });
		}

		[Serializable] private sealed class PrefabEntry
		{
			public String Path;
			public ScratchPrefabAsset Asset;
		}

		[Serializable] private sealed class UIEntry
		{
			public String Path;
			public ScratchUIAsset Asset;
		}

		[Serializable] private sealed class AudioEntry
		{
			public String Path;
			public ScratchAudioAsset Asset;
		}
	}
}
