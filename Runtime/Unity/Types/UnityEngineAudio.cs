using UnityEngine;

namespace LunyScratch
{
	internal sealed class UnityEngineAudio : IEngineAudio
	{
		private readonly AudioSource _audioSource;

		public UnityEngineAudio(AudioSource audioSource) => _audioSource = audioSource;

		public void Play() => _audioSource.Play();
	}
}
