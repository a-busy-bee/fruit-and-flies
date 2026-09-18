using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using System;
using System.Collections.Generic;

[Serializable] public class SoundBase
{
    public AudioClip clip;

    public bool loop;

    [Range(0f, 1f)]
    public float volume;

    [HideInInspector]
    public AudioSource source;
}
[Serializable] public class Sound : SoundBase
{
    public AudioManager.SoundType type;
}
[Serializable] public class Music : SoundBase
{
    public AudioManager.MusicType type;
}


public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    public enum MusicType
    {
        baseTheme
    }
    public enum SoundType
    {
        appleCrunch,    // not rotted apple
        appleSquish,    // rotted apple
        plateTap,
        eggYolkSquish,
        eggYolkTap
    }

    [SerializeField] private Sound[] sounds;
    private SoundBase currentSFX;

    [SerializeField] private Music[] themes;
    [SerializeField] private AudioSource themeSource; // child obj
    [SerializeField] private AudioSource nextThemeSource; // child obj
    [SerializeField] private AudioMixer audioMixer;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }

        // SFX
        GameObject audioManagerObj = gameObject.transform.GetChild(0).gameObject;
        foreach (Sound s in sounds)
        {
            s.source = audioManagerObj.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0];
        }

    }

    private void Start()
    {
        /*float masterVol = SaveManager.instance.GetSettings_VolumeMaster();
        float musicVol = SaveManager.instance.GetSettings_VolumeMusic();
        float sfxVol = SaveManager.instance.GetSettings_VolumeSFX();
        ChangeVolume(AudioSlider.VolumeType.Master, masterVol);
        ChangeVolume(AudioSlider.VolumeType.Music, musicVol);
        ChangeVolume(AudioSlider.VolumeType.SFX, sfxVol);*/

        //SetTheme(MusicType.baseTheme);
    }

    #region SFX
    public void PlaySound(SoundType name, bool randomize = false)
    {
        SoundBase s = Array.Find(sounds, sound => sound.type == name);

        if (s == null || s.source == null) return;

        if (randomize)
        {
            s.source.pitch = UnityEngine.Random.Range(0.85f, 1.25f);
        }

        s.source.loop = s.loop;
        s.source.Play();
    }

    public void StopSound(SoundType name)
    {
        SoundBase s = Array.Find(sounds, sound => sound.type == name);

        if (s == null || s.source == null) return;

        s.source.Stop();
    }

    public void CrossfadeSFX(SoundType nextTrackName, float duration)
    {
        SoundBase nextTrack = Array.Find(sounds, sound => sound.type == nextTrackName);

        if (nextTrack == null) return;

        // check if alr playing
        if (currentSFX != null && currentSFX == nextTrack)
        {
            if (!currentSFX.source.isPlaying) currentSFX.source.Play();
            return;
        }
        StartCoroutine(FadeSFXRoutine(currentSFX, nextTrack, duration));
    }

    private IEnumerator FadeSFXRoutine(SoundBase oldTrack, SoundBase newTrack, float duration)
    {
        float currentTime = 0;

        newTrack.volume = 0;
        newTrack.source.volume = 0f;
        newTrack.source.Play();

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / duration;

            if (oldTrack != null)
                oldTrack.source.volume = Mathf.Lerp(1f, 0f, t);

            newTrack.source.volume = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        if (oldTrack != null)
        {
            oldTrack.source.Stop();
        }

        newTrack.source.volume = 1f;
        currentSFX = newTrack;
    }

    #endregion

    #region Music
    public void SetTheme(MusicType name) // only called in title screen when everything starts up 
    {
        Music theme = Array.Find(themes, sound => sound.type == name);

        if (theme == null) return;

        themeSource.loop = theme.loop;
        themeSource.volume = theme.volume;
        themeSource.clip = theme.clip;
        themeSource.Play();
    }

    public void SwitchTheme(MusicType name)
    {
        MusicType nextName = name;
        Music theme = Array.Find(themes, theme => theme.type == nextName);

        CrossfadeMusic(theme, 0.5f);
    }

    public void CrossfadeMusic(Music nextTrack, float duration)
    {
        if (!themeSource.isPlaying)
        {
            themeSource.Play();
        }

        if (themeSource.clip == nextTrack.clip) return;

        nextThemeSource.clip = nextTrack.clip;
        nextThemeSource.loop = nextTrack.loop;
        nextThemeSource.volume = 0f;
        StartCoroutine(FadeMusicRoutine(themeSource, nextThemeSource, duration));
    }

    private IEnumerator FadeMusicRoutine(AudioSource oldTrack, AudioSource newTrack, float duration)
    {
        float currentTime = 0;

        newTrack.volume = 0;
        newTrack.volume = 0f;
        newTrack.Play();

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / duration;

            if (oldTrack != null)
                oldTrack.volume = Mathf.Lerp(1f, 0f, t);

            newTrack.volume = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        if (oldTrack != null)
        {
            oldTrack.Stop();
        }

        newTrack.volume = 1f;

        AudioSource temp = themeSource; // the ol' switcheroo
        themeSource = nextThemeSource;
        nextThemeSource = temp;
    }

    #endregion

    /*public void ChangeVolume(AudioSlider.VolumeType volumeType, float newVol)
    {
        switch (volumeType)
        {
            case AudioSlider.VolumeType.Master:
                var masterGroup = audioMixer.FindMatchingGroups("Master")[0];
                if (masterGroup.audioMixer.GetFloat("VolumeMaster", out float volumeMaster))
                {
                    masterGroup.audioMixer.SetFloat("VolumeMaster", newVol);
                }

                SaveManager.instance.SaveSettings(SaveManager.SettingsType.volumeMaster, newVol.ToString());
                break;

            case AudioSlider.VolumeType.Music:
                var musicGroup = audioMixer.FindMatchingGroups("Music")[0];
                if (musicGroup.audioMixer.GetFloat("VolumeMusic", out float volumeMusic))
                {
                    musicGroup.audioMixer.SetFloat("VolumeMusic", newVol);
                }
                SaveManager.instance.SaveSettings(SaveManager.SettingsType.volumeMusic, newVol.ToString());
                break;

            case AudioSlider.VolumeType.SFX:
                var sfxGroup = audioMixer.FindMatchingGroups("SFX")[0];
                if (sfxGroup.audioMixer.GetFloat("VolumeSFX", out float volumeSFX))
                {
                    sfxGroup.audioMixer.SetFloat("VolumeSFX", newVol);
                }
                SaveManager.instance.SaveSettings(SaveManager.SettingsType.volumeSFX, newVol.ToString());
                break;
        }
    }*/
}
