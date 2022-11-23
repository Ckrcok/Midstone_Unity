using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class OptionsScreen : MonoBehaviour
{
    public Toggle fullscreenToggle, vsyncToggle;

    public List<ResolutionItem> resolutions = new List<ResolutionItem>();

    private int currentResolution;

    public TMP_Text resolutionText;

    public AudioMixer mixer;

    public TMP_Text masterText, musicText, sfxText;
    public Slider masterSlider, musicSlider, sfxSlider;

    // Start is called before the first frame update
    void Start()
    {
        fullscreenToggle.isOn = Screen.fullScreen;

        if (QualitySettings.vSyncCount == 0)
        {
            vsyncToggle.isOn = false;
        }
        else
        {
            vsyncToggle.isOn = true;
        }

        bool foundResolution = false;
        for (int i = 0; i < resolutions.Count; i++)
        {
            if (Screen.width == resolutions[i].horizontal && Screen.height == resolutions[i].vertical)
            {
                foundResolution = true;

                currentResolution = i;

                UpdateResolutionText();
            }
        }    

        if (!foundResolution)
        {
            ResolutionItem newResolution = new ResolutionItem();
            newResolution.horizontal = Screen.width;
            newResolution.vertical = Screen.height;

            resolutions.Add(newResolution);
            currentResolution = resolutions.Count - 1;

            UpdateResolutionText();
        }

        float masterVolume = 0f;
        float musicVolume = 0f;
        float sfxVolume = 0f;

        mixer.GetFloat("MasterVolume", out masterVolume);
        masterSlider.value = masterVolume;
        mixer.GetFloat("MusicVolume", out musicVolume);
        musicSlider.value = musicVolume;
        mixer.GetFloat("SFXVolume", out sfxVolume);
        sfxSlider.value = sfxVolume;

        masterText.text = Mathf.RoundToInt(masterSlider.value + 80).ToString();
        musicText.text = Mathf.RoundToInt(musicSlider.value + 80).ToString();
        sfxText.text = Mathf.RoundToInt(sfxSlider.value + 80).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResolutionLeftButton()
    {
        currentResolution--;
        if(currentResolution < 0)
        {
            currentResolution = 0;
        }

        UpdateResolutionText();
    }

    public void ResolutionRightButton()
    {
        currentResolution++;
        if (currentResolution > resolutions.Count - 1)
        {
            currentResolution = resolutions.Count - 1;
        }

        UpdateResolutionText();
    }

    public void UpdateResolutionText()
    {
        resolutionText.text = resolutions[currentResolution].horizontal.ToString() + " x " + resolutions[currentResolution].vertical.ToString();
    }

    public void ApplyGraphics()
    {
        //Screen.fullScreen = fullscreenToggle.isOn;

        if (vsyncToggle.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }

        Screen.SetResolution(resolutions[currentResolution].horizontal, resolutions[currentResolution].vertical, fullscreenToggle.isOn);
    }

    public void SetMasterVolume()
    {
        masterText.text = Mathf.RoundToInt(masterSlider.value +80).ToString();
        mixer.SetFloat("MasterVolume", masterSlider.value);

        PlayerPrefs.SetFloat("MasterVolume", masterSlider.value);
    }

    public void SetMusicVolume()
    {
        musicText.text = Mathf.RoundToInt(musicSlider.value + 80).ToString();
        mixer.SetFloat("MusicVolume", musicSlider.value);

        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
    }

    public void SetSFXrVolume()
    {
        sfxText.text = Mathf.RoundToInt(sfxSlider.value + 80).ToString();
        mixer.SetFloat("SFXVolume", sfxSlider.value);

        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
    }
}

[System.Serializable]
public class ResolutionItem
{
    public int horizontal, vertical;
}
