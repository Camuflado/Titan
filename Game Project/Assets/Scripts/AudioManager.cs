using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{

    public enum AudioChannel { Master, Effects, Music};  
    public float masterVolumePercent { get; private set; }
    public  float effectsVolumePercent { get; private set; }
    public float musicVolumePercent { get; private set; }
    //float effectsVolumePercent = 1; <<<<< alterou-se disto para ser possivel dar load das definições

    AudioSource effects2DSources;
    AudioSource[] musicSources;
    int activeMusicSourceIndex;

    public static AudioManager instance;

    //Para o som vir do player
    Transform audioListener;
    Transform playerT;

    SoundLibrary library;

    void Awake()
    {
        if (instance != null) //Não criar vários Audios se já estiver criado
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            library = GetComponent<SoundLibrary>();

            musicSources = new AudioSource[2];
            for (int i = 0; i < 2; i++)
            {
                GameObject newMusicSource = new GameObject("Music source " + (i + 1));
                musicSources[i] = newMusicSource.AddComponent<AudioSource>();
                newMusicSource.transform.parent = transform;
            }

            //Som 2D
            GameObject newEffects2DSource = new GameObject("2d effects source");
            effects2DSources = newEffects2DSource.AddComponent<AudioSource>();
            newEffects2DSource.transform.parent = transform;

            //Para o som vir do player
            audioListener = FindObjectOfType<AudioListener>().transform;
            if (FindObjectOfType<Player>() != null)
                playerT = FindObjectOfType<Player>().transform;

            //Ler player preferences
            masterVolumePercent = PlayerPrefs.GetFloat("master vol", 1);
            effectsVolumePercent = PlayerPrefs.GetFloat("effects vol", 1);
            musicVolumePercent = PlayerPrefs.GetFloat("music vol", 1);
        }
    }

    void Update()
    {
        //Para o som vir do player
        if (playerT != null) //Se o player estiver vivo
        {
            audioListener.position = playerT.position; //Posição do audio no player;
        }

    }

    public void SetVolume(float volumePercent, AudioChannel channel)
    {
        switch(channel)
        {
            case AudioChannel.Master:
                masterVolumePercent = volumePercent;
                break;
            case AudioChannel.Effects:
                effectsVolumePercent  = volumePercent;
                break;
            case AudioChannel.Music:
                musicVolumePercent = volumePercent;
                break;
        }

        musicSources[0].volume = musicVolumePercent * masterVolumePercent;
        musicSources[1].volume = musicVolumePercent * masterVolumePercent;

        //Gravar player preferences
        PlayerPrefs.SetFloat("master vol", masterVolumePercent);
        PlayerPrefs.SetFloat("effects vol", effectsVolumePercent);
        PlayerPrefs.SetFloat("music vol", musicVolumePercent);
        PlayerPrefs.Save();
    }

    public void PlayMusic(AudioClip clip, float fadeDuration = 1)
    {
        activeMusicSourceIndex = 1 - activeMusicSourceIndex;
        musicSources[activeMusicSourceIndex].clip = clip;
        musicSources[activeMusicSourceIndex].Play();

        StartCoroutine(AnimateMusicCrossfade(fadeDuration));
    }

    public void PlaySound(AudioClip clip, Vector3 pos) //Tocar Sons
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, pos, effectsVolumePercent * masterVolumePercent);
        }
    }

    public void PlaySound(string soundName, Vector3 pos)
    {
        PlaySound(library.GetClipFromName(soundName), pos); 
    }

    public void PlaySound2D(string soundName)
    {
        effects2DSources.PlayOneShot(library.GetClipFromName(soundName), effectsVolumePercent * masterVolumePercent); 
    }

    IEnumerator AnimateMusicCrossfade(float duration)
    {
        float percent = 0;

        while (percent < 1)
        {
            percent +=  Time.deltaTime * 1 / duration;
            musicSources[activeMusicSourceIndex].volume = Mathf.Lerp(0, musicVolumePercent * masterVolumePercent, percent);
            musicSources[1-activeMusicSourceIndex].volume = Mathf.Lerp(musicVolumePercent * masterVolumePercent, 0, percent);
            yield return null;
        }
    }

	
}
