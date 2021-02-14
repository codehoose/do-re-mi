using System.Collections.Generic;
using UnityEngine;

public class PlaySequence : MonoBehaviour
{
    private AudioClip _clip;

    void Start()
    {
        int samplerate = 44100;
        float frequency = 440;

        _clip = AudioClip.Create(frequency.ToString(), samplerate, 1, samplerate, false);
        CreateClip(_clip, samplerate, frequency);

        var audioSource = GetComponent<AudioSource>();
        audioSource.clip = _clip;
        audioSource.loop = true;
        audioSource.Play();
    }

    private void CreateClip(AudioClip clip, int samplerate, float frequency)
    {
        var size = clip.frequency * (int)Mathf.Ceil(clip.length);
        float[] data = new float[size];

        int count = 0;
        while (count < data.Length)
        {
            data[count] = Mathf.Sin(2 * Mathf.PI * frequency * count / samplerate);
            count++;
        }

        clip.SetData(data, 0);
    }
}
