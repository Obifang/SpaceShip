using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public float FadeTime = 1f;
    public float FadeMultiplier = 1.5f;

    private bool _faded = true;

    public void Play(AudioSource source)
    {
        if (source.isPlaying) {
            return;
        }

        source.Play();
    }

    public void FadeAudioToStop(float timeToFade, AudioSource source)
    {
        if (!source.isPlaying || !_faded) {
            return;
        }
        _faded = false;
        this.StartCoroutine(FadeToStop(FadeTime, source));
    }

    IEnumerator FadeToStop(float fadeTime, AudioSource audioSource)
    {
        Debug.Log("Begin Fade");
        float timer = 0;
        float audioOriginalVolume = audioSource.volume;
        while (timer < fadeTime) {
            audioSource.volume = Mathf.Lerp(audioSource.volume, 0, Time.deltaTime * fadeTime * FadeMultiplier);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        audioSource.Stop();
        audioSource.volume = audioOriginalVolume;
        _faded = true;
    }
}
