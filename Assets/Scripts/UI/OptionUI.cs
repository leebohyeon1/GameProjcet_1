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
        // �ػ� �� ���� �� ����
        Resolution resolution = resolutions[resolutionIndex];
        PlayerPrefs.SetInt("resolution", resolutionIndex);
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    private void SetInitialResolution()
    {
        // ����� �ػ� ���� ������ �� ������ �����ϰ�, ������ ���� �ػ󵵷� ����
        int savedResolutionIndex = PlayerPrefs.GetInt("resolution", -1);

        if (savedResolutionIndex == -1)
        {
            // ����� ���� ���� ��� ���� ����� �ػ󵵷� ����
            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);

            // ���� �ػ󵵸� PlayerPrefs�� ����
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
            // ����� ���� ���� ��� �� ������ ����
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
        // ������ ����Ʈ �� ���� �� ����
        int frameRate = frameRates[frameRateIndex];
        PlayerPrefs.SetInt("frameRate", frameRateIndex);
        Application.targetFrameRate = frameRate;
    }
    #endregion
}
