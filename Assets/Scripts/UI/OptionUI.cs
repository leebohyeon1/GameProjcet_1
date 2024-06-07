using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OptionUI : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    public TMP_Dropdown frameRateDropdown;
    private readonly List<int> frameRates = new List<int> { 30, 60, 120, 144, 240 };
    //==========================================================

    void Start()
    {
        ClearResolution();
        ClearFrame();

        SetInitialResolution();
     
    }

    void Update()
    {
        
    }
    //==========================================================

    #region Resolution

    void ClearResolution()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> resolutionOptions = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            resolutionOptions.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = PlayerPrefs.GetInt("resolution", currentResolutionIndex);
        resolutionDropdown.RefreshShownValue();

        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    public void SetResolution(int resolutionIndex)
    {
        // 해상도 값 저장 및 적용
        Resolution resolution = resolutions[resolutionIndex];
        PlayerPrefs.SetInt("resolution", resolutionIndex);
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    private void SetInitialResolution()
    {
        // 저장된 해상도 값이 있으면 그 값으로 설정하고, 없으면 현재 해상도로 설정
        int savedResolutionIndex = PlayerPrefs.GetInt("resolution", -1);

        if (savedResolutionIndex == -1)
        {
            // 저장된 값이 없을 경우 현재 모니터 해상도로 설정
            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);

            // 현재 해상도를 PlayerPrefs에 저장
            for (int i = 0; i < resolutions.Length; i++)
            {
                if (resolutions[i].width == currentResolution.width && resolutions[i].height == currentResolution.height)
                {
                    PlayerPrefs.SetInt("resolution", i);
                    break;
                }
            }
        }
        else
        {
            // 저장된 값이 있을 경우 그 값으로 설정
            SetResolution(savedResolutionIndex);
        }
    }

    #endregion

    #region FrameRate
    void ClearFrame()
    {
        frameRateDropdown.ClearOptions();
        List<string> frameRateOptions = frameRates.ConvertAll(rate => rate + " FPS");
        frameRateDropdown.AddOptions(frameRateOptions);

        int savedFrameRateIndex = PlayerPrefs.GetInt("frameRate", frameRates.IndexOf(60));
        frameRateDropdown.value = savedFrameRateIndex;
        frameRateDropdown.RefreshShownValue();

        frameRateDropdown.onValueChanged.AddListener(SetFrameRate);

        SetFrameRate(savedFrameRateIndex);
    }

    public void SetFrameRate(int frameRateIndex)
    {
        // 프레임 레이트 값 저장 및 적용
        int frameRate = frameRates[frameRateIndex];
        PlayerPrefs.SetInt("frameRate", frameRateIndex);
        Application.targetFrameRate = frameRate;
    }
    #endregion
}
