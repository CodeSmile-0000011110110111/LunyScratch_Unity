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
	public sealed class UnityAssetRegistry : ScriptableObject, AssetRegistry.IAssetRegistry
	{
		[SerializeField] private List<PrefabEntry> _prefabs = new();
		[SerializeField] private List<UIEntry> _ui = new();
		[SerializeField] private List<AudioEntry> _audio = new();

		// Optional placeholders to return when requested asset is missing.
		[SerializeField] private UnityPrefabAsset _defaultPrefab;
		[SerializeField] private UnityUIAsset _defaultUI;
		[SerializeField] private UnityAudioAsset _defaultAudio;

		[NonSerialized] private Dictionary<String, UnityPrefabAsset> _prefabMap;
		[NonSerialized] private Dictionary<String, UnityUIAsset> _uiMap;
		[NonSerialized] private Dictionary<String, UnityAudioAsset> _audioMap;

		// Expose for editor builder
		public List<(String path, UnityPrefabAsset asset)> PrefabsForEditor
		{
			get
			{
				var list = new List<(String, UnityPrefabAsset)>(_prefabs.Count);
				foreach (var e in _prefabs) list.Add((e.Path, e.Asset));
				return list;
			}
		}

		public List<(String path, UnityUIAsset asset)> UIForEditor
		{
			get
			{
				var list = new List<(String, UnityUIAsset)>(_ui.Count);
				foreach (var e in _ui)
					list.Add((e.Path, e.Asset));
				return list;
			}
		}

		public List<(String path, UnityAudioAsset asset)> AudioForEditor
		{
			get
			{
				var list = new List<(String, UnityAudioAsset)>(_audio.Count);
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
			if (typeof(T) == typeof(IEnginePrefabAsset) || typeof(T) == typeof(UnityPrefabAsset))
				return TryGet(_prefabMap, path) as T;

			if (typeof(T) == typeof(IEngineUIAsset) || typeof(T) == typeof(UnityUIAsset))
				return TryGet(_uiMap, path) as T;

			if (typeof(T) == typeof(IEngineAudioAsset) || typeof(T) == typeof(UnityAudioAsset))
				return TryGet(_audioMap, path) as T;

			return null;
		}

		public IEngineAsset Get(String path, Type assetType)
		{
			if (assetType == typeof(IEnginePrefabAsset) || assetType == typeof(UnityPrefabAsset))
				return TryGet(_prefabMap, path);
			if (assetType == typeof(IEngineUIAsset) || assetType == typeof(UnityUIAsset))
				return TryGet(_uiMap, path);
			if (assetType == typeof(IEngineAudioAsset) || assetType == typeof(UnityAudioAsset))
				return TryGet(_audioMap, path);

			return null;
		}

		public T GetPlaceholder<T>() where T : class, IEngineAsset
		{
			if (typeof(T) == typeof(IEnginePrefabAsset) || typeof(T) == typeof(UnityPrefabAsset))
				return _defaultPrefab as T;
			if (typeof(T) == typeof(IEngineUIAsset) || typeof(T) == typeof(UnityUIAsset))
				return _defaultUI as T;
			if (typeof(T) == typeof(IEngineAudioAsset) || typeof(T) == typeof(UnityAudioAsset))
				return _defaultAudio as T;

			return null;
		}

		public IEngineAsset GetPlaceholder(Type assetType)
		{
			if (assetType == typeof(IEnginePrefabAsset) || assetType == typeof(UnityPrefabAsset))
				return _defaultPrefab;
			if (assetType == typeof(IEngineUIAsset) || assetType == typeof(UnityUIAsset))
				return _defaultUI;
			if (assetType == typeof(IEngineAudioAsset) || assetType == typeof(UnityAudioAsset))
				return _defaultAudio;

			return null;
		}

		private void OnEnable() => BuildDictionaries();

		public void BuildDictionaries()
		{
			_prefabMap = new Dictionary<String, UnityPrefabAsset>(StringComparer.OrdinalIgnoreCase);
			foreach (var e in _prefabs)
				if (!String.IsNullOrEmpty(e?.Path))
					_prefabMap[e.Path] = e.Asset;

			_uiMap = new Dictionary<String, UnityUIAsset>(StringComparer.OrdinalIgnoreCase);
			foreach (var e in _ui)
				if (!String.IsNullOrEmpty(e?.Path))
					_uiMap[e.Path] = e.Asset;

			_audioMap = new Dictionary<String, UnityAudioAsset>(StringComparer.OrdinalIgnoreCase);
			foreach (var e in _audio)
				if (!String.IsNullOrEmpty(e?.Path))
					_audioMap[e.Path] = e.Asset;
		}

		public void SetPrefabsForEditor(List<(String path, UnityPrefabAsset asset)> items)
		{
			_prefabs.Clear();
			foreach (var (path, asset) in items)
				_prefabs.Add(new PrefabEntry { Path = path, Asset = asset });
		}

		public void SetUIForEditor(List<(String path, UnityUIAsset asset)> items)
		{
			_ui.Clear();
			foreach (var (path, asset) in items)
				_ui.Add(new UIEntry { Path = path, Asset = asset });
		}

		public void SetAudioForEditor(List<(String path, UnityAudioAsset asset)> items)
		{
			_audio.Clear();
			foreach (var (path, asset) in items)
				_audio.Add(new AudioEntry { Path = path, Asset = asset });
		}

		[Serializable] private sealed class PrefabEntry
		{
			public String Path;
			public UnityPrefabAsset Asset;
		}

		[Serializable] private sealed class UIEntry
		{
			public String Path;
			public UnityUIAsset Asset;
		}

		[Serializable] private sealed class AudioEntry
		{
			public String Path;
			public UnityAudioAsset Asset;
		}
	}
}
