using UnityEngine;
using System;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

[Serializable]
public class Sound
{
    public int Number;
    public AudioClip Clip;
    public AudioMixerGroup mixerGroup;
    [Range(0.0f, 1.0f)]
    public float Volume = 0.7f;
    [Range(0.5f, 1.5f)]
    public float Pitch = 1.0f;

    [Range(0.1f, 0.5f)]
    public float RandomValue = 0.1f;
    [Range(0.1f, 0.5f)]
    public float RandomPitch = 0.1f;

    public bool Loop;

    private AudioSource source;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = Clip;
        source.loop = Loop;
        source.outputAudioMixerGroup = mixerGroup;
    }

    public void Play()
    {
        source.volume = Volume * (1 + Random.Range(-RandomValue / 2.0f, RandomValue / 2.0f));
        source.pitch = Pitch * (1 + Random.Range(-RandomPitch / 2.0f, RandomPitch / 2.0f));
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }
}

public class AudioManager : SingletoneAsComponent<AudioManager>
{

    public static AudioManager Instance
    {
        get { return ((AudioManager)_Instance); }
        set { _Instance = value; }
    }

    //public float soundVolume
    //{
    //    get { return AudioListener.volume; }
    //    set { AudioListener.volume = value; }
    //}

    public bool soundMute
    {
        get { return AudioListener.pause; }
        set { AudioListener.pause = value; }
    }

    [SerializeField]
    Sound[] sounds;


    private void Start()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject go = new GameObject(string.Format("Sound_{0}_{1}", i, sounds[i].Number));
            go.transform.SetParent(transform);
            sounds[i].SetSource(go.AddComponent<AudioSource>());
        }
        PlaySound(0);
    }

    public void PlaySound(int num)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].Number == num)
            {
                sounds[i].Play();
                return;
            }
        }
    }

    public void StopSound(int num)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].Number == num)
            {
                sounds[i].Stop();
                return;
            }
        }
        Debug.LogWarning("AudioManager: Sound not found in list, " + num);
    }
}
