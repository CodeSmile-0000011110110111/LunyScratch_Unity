using UnityEngine;

namespace LunyScratch
{
	/// <summary>
	/// Unity implementation of an audio asset wrapper.
	/// </summary>
	[System.Serializable]
 public sealed class UnityAudioAsset : IEngineAudioAsset
	{
		[SerializeField] private AudioClip _clip;

		public UnityAudioAsset() { }
		public UnityAudioAsset(AudioClip clip) { _clip = clip; }

		public AudioClip Clip => _clip;
	}
}
