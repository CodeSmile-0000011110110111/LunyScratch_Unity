using UnityEngine;

namespace LunyScratch
{
	internal sealed class ScratchAudioSource : IEngineAudioSource
	{
		private readonly AudioSource _audioSource;

		public ScratchAudioSource(AudioSource audioSource) => _audioSource = audioSource;

		public void Play() => _audioSource.Play();
	}
}
