using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance;

    [Header("BGM Sources")]
    public AudioSource _source1;
    public AudioSource _source2;
    public AudioSource _source3;
    public AudioSource _source4;
    public AudioSource _source5;

    [Header("SFX Sources")]
    public AudioSource _sfxSource;

	private void Start ()
    {
        SingletonFunction();
	}

    private void SingletonFunction()
    {
        if (AudioManager.instance == null)
        {
            AudioManager.instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    
    public void MuteAllMusic()
    {
        _source1.mute = true;
        _source2.mute = true;
        _source3.mute = true;
        _source4.mute = true;
        _source5.mute = true;
    }

    public void Lv1Music()
    {
        _source1.mute = false;
        _source2.mute = true;
        _source3.mute = true;
        _source4.mute = true;
        _source5.mute = true;
    }

    public void Lv2Music()
    {
        _source1.mute = true;
        _source2.mute = false;
        _source3.mute = true;
        _source4.mute = true;
        _source5.mute = true;
    }

    public void Lv3Music()
    {
        _source1.mute = true;
        _source2.mute = true;
        _source3.mute = false;
        _source4.mute = true;
        _source5.mute = true;
    }

    public void Lv4Music()
    {
        _source1.mute = true;
        _source2.mute = true;
        _source3.mute = true;
        _source4.mute = false;
        _source5.mute = true;
    }

    public void Lv5Music()
    {
        _source1.mute = true;
        _source2.mute = true;
        _source3.mute = true;
        _source4.mute = true;
        _source5.mute = false;
    }

    public void PlaySFX(AudioClip _newClip)
    {
        _sfxSource.clip = _newClip;
        _sfxSource.Play();
    }
}
