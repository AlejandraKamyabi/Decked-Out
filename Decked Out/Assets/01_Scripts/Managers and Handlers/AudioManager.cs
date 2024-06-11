using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    [SerializeField] private GameObject musicSourceObj;
    [SerializeField] private AudioClip[] musicClip;
    private int trakPlaying = 0;
    private GameObject[] musicPlayer;

    [SerializeField] private GameObject canvas;

    private bool canvasActive = false;

    private GameLoader _loader;
    public enum SFXSound
    {
        Power_Earthquake,
        Power_EoD,
        Power_Newhand,
        Tower_Destroy,
        Tower_Inferno,
        Tower_Organ_Wound,
        Tower_OrganShot_Var1,
        Tower_OrganShot_Var2,
        Tower_OrganShot_Var3,
        Tower_Place,
        UI_Click,
        UI_Negative,
        UI_Positive
    }
    public enum musicSound
    {
        encounter_loop
    }
    private Dictionary<SFXSound, AudioClip> SFXSoundAudioClipDictionary;
    private Dictionary<musicSound, AudioClip> musicSoundAudioClipDictionary;

    private void Awake()
    {
        canvas.SetActive(canvasActive);

        GameObject[] allAudioManager = GameObject.FindGameObjectsWithTag("AudioManager");

        musicPlayer = new GameObject[musicClip.Length];

        for (int i = 0; i < musicPlayer.Length; i++)
        {
            GameObject temp = Instantiate(musicSourceObj, gameObject.transform.position , Quaternion.identity);
            temp.transform.SetParent(gameObject.transform);
            temp.name = i.ToString();
            musicPlayer[i] = temp;
            musicPlayer[i].GetComponent<AudioSource>().clip = musicClip[i];
            musicPlayer[i].SetActive(false);
        }

        if (allAudioManager.Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        SFXSoundAudioClipDictionary = new Dictionary<SFXSound, AudioClip>();
        foreach (SFXSound sound in System.Enum.GetValues(typeof(SFXSound)))
        {
            SFXSoundAudioClipDictionary[sound] = Resources.Load<AudioClip>(sound.ToString());
        }

        musicSoundAudioClipDictionary = new Dictionary<musicSound, AudioClip>();
        foreach (musicSound sound in System.Enum.GetValues(typeof(musicSound)))
        {
            musicSoundAudioClipDictionary[sound] = Resources.Load<AudioClip>(sound.ToString());
        }
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("masterVolume") || PlayerPrefs.HasKey("musicVolume") || PlayerPrefs.HasKey("SFXVolume"))
        {
            LoadVolume();
        }
        else
        {
            setMasterVolume();
            setMusicVolume();
            setSFXVolume();
        }
    }

    public void setMasterVolume()
    {
        float volume = masterSlider.value;
        myMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("masterVolume", volume);
    }
    public void setMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
    public void setSFXVolume()
    {
        float volume = SFXSlider.value;
        myMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    private void LoadVolume()
    {
        if (PlayerPrefs.HasKey("masterVolume"))
        {
            masterSlider.value = PlayerPrefs.GetFloat("masterVolume");
            setMasterVolume();
        }
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
            setMusicVolume();
        }
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");
            setSFXVolume();
        }
    }

    public void playMusicClip(musicSound sound)
    {
        musicSource.clip = musicSoundAudioClipDictionary[sound];
        musicSource.Play();
    }
    public void stopMusicClip()
    {
        musicSource.Stop();
    }

    public void playSFXClip(SFXSound sound)
    {
        SFXSource.PlayOneShot(SFXSoundAudioClipDictionary[sound]);
    }
    public void playSFXClip(SFXSound sound, AudioSource source)
    {
        SFXSource.clip = SFXSoundAudioClipDictionary[sound];
        SFXSource.Play();
    }

    public void StartWaveMucic()
    {
        stopMusicClip();
        foreach (GameObject source in musicPlayer)
        {
            source.SetActive(true);
            source.GetComponent<AudioSource>().Play();
            source.SetActive(false);
        }
        musicPlayer[0].SetActive(true);
    }
    public void PlayNextMuiscTrak()
    {
        trakPlaying++;
        if (trakPlaying < musicPlayer.Length && trakPlaying >= 0)
        {
            foreach (GameObject source in musicPlayer)
            {
                source.SetActive(false);
            }
            musicPlayer[trakPlaying].SetActive(true);
        }
    }
    public void StopWaveMusic()
    {
        foreach (GameObject source in musicPlayer)
        {
            source.SetActive(true);
            source.GetComponent<AudioSource>().Stop();
            source.SetActive(false);
        }
        musicSource.Play();
    }

    public void StopAllAudio()
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>() as AudioSource[];

        foreach (var audioS in allAudioSources)
        {
            if (!audioS.loop)
            {
                audioS.Stop();
            }
        }
    }
    /*
    public void Initialize()
    {
        SFXSoundAudioClipDictionary = new Dictionary<SFXSound, AudioClip>();
        foreach (SFXSound sound in System.Enum.GetValues(typeof(SFXSound)))
        {
            SFXSoundAudioClipDictionary[sound] = Resources.Load<AudioClip>(sound.ToString());
        }

        musicSoundAudioClipDictionary = new Dictionary<musicSound, AudioClip>();
        foreach (musicSound sound in System.Enum.GetValues(typeof(musicSound)))
        {
            musicSoundAudioClipDictionary[sound] = Resources.Load<AudioClip>(sound.ToString());
        }

        if (PlayerPrefs.HasKey("masterVolume") || PlayerPrefs.HasKey("musicVolume") || PlayerPrefs.HasKey("SFXVolume"))
        {
            LoadVolume();
        }
        else
        {
            setMasterVolume();
            setMusicVolume();
            setSFXVolume();
        }
    }
    public void Setting ()
    {
        canvasActive = !canvasActive;
        canvas.SetActive(canvasActive);
    }
    */
    public void LoadTest()
    {
        SceneManager.LoadScene("Test");
    }
    public void CloseMenu()
    {
        canvas.SetActive(false);
    }
}
