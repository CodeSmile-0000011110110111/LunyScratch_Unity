using System;
using UnityEngine;

namespace LunyScratch
{
	/// <summary>
	/// Unity implementation of an audio asset wrapper.
	/// </summary>
	[Serializable]
	public sealed class ScratchAudioAsset : IEngineAudioAsset
	{
		[SerializeField] private AudioClip _clip;

		public AudioClip Clip => _clip;

		public ScratchAudioAsset() {}
		public ScratchAudioAsset(AudioClip clip) => _clip = clip;
	}
}
