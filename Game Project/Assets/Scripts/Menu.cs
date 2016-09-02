using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Menu : MonoBehaviour
{
    public GameObject mainMenuHolder;
    public GameObject optionsMenuHolder;

    public Slider[] volumeSliders;
    public Toggle[] resolutionToggles;
    public int[] screenWidths;
    int activeScreenResIndex;

    void Start()
    {
        //Load os PlayerPrefs
        activeScreenResIndex = PlayerPrefs.GetInt("screen res index");
        bool isFullscreen = (PlayerPrefs.GetInt("fullscreen") == 1) ? true : false;

        //Colocar os audios no volume gravado
        volumeSliders[0].value = AudioManager.instance.masterVolumePercent;
        volumeSliders[1].value = AudioManager.instance.musicVolumePercent;
        volumeSliders[2].value = AudioManager.instance.effectsVolumePercent;

        //Colocar a resolution gravada
        for(int i = 0; i < resolutionToggles.Length; i++)
        {
            resolutionToggles[i].isOn = i == activeScreenResIndex;
        }

        //Colocar fullscreen gravado
        SetFullscreen(isFullscreen);

    }

    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void OptionsMenu()
    {
        mainMenuHolder.SetActive(false);
        optionsMenuHolder.SetActive(true);
    }

    public void MainMenu()
    {
        mainMenuHolder.SetActive(true);
        optionsMenuHolder.SetActive(false);
    }

    public void SetScreenResolution(int i)
    {
        if (resolutionToggles[i].isOn)
        {
            activeScreenResIndex = i;
            float aspectRatio = 16 / 9f;
            Screen.SetResolution(screenWidths[i], (int)(screenWidths[i] / aspectRatio), false);
            PlayerPrefs.SetInt("screen res index", activeScreenResIndex);
            PlayerPrefs.Save();
        }
    }

    public void SetFullscreen(bool isFullscreen)
    {
        for (int i = 0; i < resolutionToggles.Length; i++)
        {
            resolutionToggles[i].interactable = !isFullscreen;
        }

        if (isFullscreen)
        {
            Resolution[] allResolutions = Screen.resolutions;
            Resolution maxResolution = allResolutions[allResolutions.Length - 1];
            Screen.SetResolution(maxResolution.width, maxResolution.height, true);
        }
        else
        {
            SetScreenResolution(activeScreenResIndex);
        }
        PlayerPrefs.SetInt("fullscreen", ((isFullscreen) ? 1 : 0)); //Se for fullscreen grava 1, se não grava 0 
        PlayerPrefs.Save();
    }

    public void SetMasterVolume(float value)
    {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Master);
    }

    public void SetMusicVolume(float value)
    {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Music);
    }

    public void SetEffectsVolume(float value)
    {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Effects);
    }

    public void QualityVeryLow()
    {
        QualitySettings.SetQualityLevel(0);
        Debug.Log("Quality changed to Very Low");
    }

    public void QualityLow()
    {
        QualitySettings.SetQualityLevel(1);
        Debug.Log("Quality changed to Low");
    }

    public void QualityNormal()
    {
        QualitySettings.SetQualityLevel(2);
        Debug.Log("Quality changed to Normal");
    }

    public void QualityHigh() //Default
    {
        QualitySettings.SetQualityLevel(3);
        Debug.Log("Quality changed to High");
    }

    public void QualityUltra()
    {
        QualitySettings.SetQualityLevel(4);
        Debug.Log("Quality changed to Ultra");
    }

    public void GetQuality()
    {
        Debug.Log("Quality: " + QualitySettings.GetQualityLevel());
    }

    public void vSyncON()
    {
        QualitySettings.vSyncCount = 1;
        Debug.Log("vSync On");
    }

    public void vSyncOFF()
    {
        QualitySettings.vSyncCount = 0;
        Debug.Log("vSync Off");
    }

    public void NoAA()
    {
        QualitySettings.antiAliasing = 0;
        Debug.Log("0 AA");
    }

    public void x2AA() //Default
    {
        QualitySettings.antiAliasing = 2;
        Debug.Log("2x AA");
    }

    public void x4AA()
    {
        QualitySettings.antiAliasing = 4;
        Debug.Log("4x AA");
    }

    public void x8AA()
    {
        QualitySettings.antiAliasing = 8;
        Debug.Log("8x AA");
    }

    //AudioManager já grava sozinho por isso não é preciso fazer PlayerPrefs nem PlayerPrefs.Save();
}