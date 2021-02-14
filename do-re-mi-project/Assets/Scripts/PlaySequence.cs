using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySequence : MonoBehaviour
{
    private Dictionary<string, AudioClip> _clips;

    public TextAsset noteSequence;

    public int sampleRate = 44100;

    public int[] _frequencies = new int[]
    {
    //  A    B    C    D    E    F    G    A'
    //  DO   RE   MI   FA   SO   LA   TI   DO
    // See: https://www.audiology.org/sites/default/files/ChasinConversionChart.pdf
        440, 494, 523, 587, 659, 698, 784, 880
    };

    public string[] _frequenceNames = new string[]
    {
        "do", "re", "mi", "fa", "so", "la", "ti", "do'"
    };

    void Start()
    {
        _clips = new Dictionary<string, AudioClip>();

        for (int i = 0; i < _frequencies.Length; i++)
        {
            string frequencyName = _frequenceNames[i];
            AudioClip clip = CreateClip(frequencyName, sampleRate, _frequencies[i]);
            _clips.Add(frequencyName, clip);
        }
    }

    public void PlayNotes()
    {
        string json = noteSequence.text;
        var sequence = JsonUtility.FromJson<NoteSequence>(json);

        StartCoroutine(PlayTheSequence(sequence));
    }

    private IEnumerator PlayTheSequence(NoteSequence sequence)
    {
        var audioSource = GetComponent<AudioSource>();

        for (int i = 0; i < sequence.notes.Length; i++)
        {
            var note = sequence.notes[i];
            var clip = _clips[note.name];
            audioSource.PlayOneShot(clip);
            yield return new WaitForSeconds(note.duration);
            audioSource.Stop();
        }
    }

    private AudioClip CreateClip(string clipName, int samplerate, float frequency)
    {
        var clip = AudioClip.Create(clipName, samplerate, 1, samplerate, false);

        var size = clip.frequency * (int)Mathf.Ceil(clip.length);
        float[] data = new float[size];

        int count = 0;
        while (count < data.Length)
        {
            data[count] = Mathf.Sin(2 * Mathf.PI * frequency * count / samplerate);
            count++;
        }

        clip.SetData(data, 0);
        return clip;
    }
}
