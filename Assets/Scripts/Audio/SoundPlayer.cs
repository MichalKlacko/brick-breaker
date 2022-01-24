using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundPlayer : MonoBehaviour
{
    public static SoundPlayer Instance;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioListener audioListener;


    public SoundSystem SoundSystem;


    // Start is called before the first frame update
    void Start()
    {
        SoundSystem = new SoundSystem();
        SoundSystem.Init(mixer);
        SoundSystem.SetCurrentAudioListener(audioListener);

    }

    public static void PlaySound(SoundDef sound)
    {
        if(SoundPlayer.Instance != null)
        {
            SoundPlayer.Instance.SoundSystem.Play(sound);
        }
    }

    private void Update()
    {
        SoundSystem.Update();
    }
}
