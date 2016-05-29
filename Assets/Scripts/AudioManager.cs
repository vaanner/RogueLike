using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

<<<<<<< HEAD
    private static AudioManager _instance;
    public AudioSource efxAudioSource;  //播放音效
    public AudioSource bgAudioSource;   //播放背景音乐
    public float minPitch = 0.9f;   //最低播放速度
    public float maxPitch = 1.1f;   //最快播放速度
	// Use this for initialization
    public static AudioManager Instance
    {
        get
        {
            return _instance;
        }
    }
	void Awake()
    {
        _instance = this;
    }
	public void RandomPlayClips(params AudioClip[] clips)
    {
         float pitch = Random.Range(minPitch,maxPitch);
         int index = Random.Range(0,clips.Length);
         AudioClip clip = clips[index];
         efxAudioSource.clip = clip;
         efxAudioSource.pitch = pitch;
         efxAudioSource.Play();
    }
    public void bgMusicStop()
    {
        bgAudioSource.Stop();
    }
=======
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
>>>>>>> origin/master
}
