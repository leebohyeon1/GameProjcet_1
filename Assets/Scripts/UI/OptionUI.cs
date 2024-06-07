using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    private List<Resolution> uniqueResolutions;

    public TMP_Dropdown frameRateDropdown;  
    private readonly List<int> frameRates = new List<int> { 30, 45, 60, 75, 120, 144, 240 };

    public TMP_Dropdown ScreenModeDropdown;
    public enum ScreenMode
    {
        FullScreenWindow = 0,
        Window = 1
    }

    int resolutionIndex;
    int frameRateIndex;
    int screenModeIndex;

    //==========================================================

    void Start()
    {
        ClearResolution();
        ClearFrame();
        ClearScreenMode();

        SetInitialResolution();
        SaveOption();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SaveBtn();
        }
    }
    //==========================================================

    #region Resolution

    void ClearResolution()
    {
        Resolution[] allResolutions = Screen.resolutions;
        uniqueResolutions = allResolutions.Distinct(new ResolutionComparer()).ToList();
        resolutionDropdown.ClearOptions();

        List<string> resolutionOptions = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < uniqueResolutions.Count; i++)
        {
            string option = uniqueResolutions[i].width + " x " + uniqueResolutions[i].height;
            resolutionOptions.Add(option);

            if (uniqueResolutions[i].width == Screen.currentResolution.width &&
                uniqueResolutions[i].height == Screen.currentResolution.height)
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
        Resolution resolution = uniqueResolutions[resolutionIndex];
        this.resolutionIndex = resolutionIndex;
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
            for (int i = 0; i < uniqueResolutions.Count; i++)
            {
                if (uniqueResolutions[i].width == currentResolution.width && uniqueResolutions[i].height == currentResolution.height)
                {
                    this.resolutionIndex = i;
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

        int savedFrameRateIndex = PlayerPrefs.GetInt("frameRate", -1);
        if (savedFrameRateIndex == -1)
        {
            // 저장된 프레임 레이트 값이 없으면 기본값으로 60프레임 설정
            savedFrameRateIndex = frameRates.IndexOf(60);
            frameRateIndex = savedFrameRateIndex;
            //PlayerPrefs.Save();
        }

        frameRateDropdown.value = savedFrameRateIndex;
        frameRateDropdown.RefreshShownValue();

        frameRateDropdown.onValueChanged.AddListener(SetFrameRate);

        SetFrameRate(savedFrameRateIndex);
    }

    public void SetFrameRate(int frameRateIndex)
    {
        // 프레임 레이트 값 저장 및 적용
        int frameRate = frameRates[frameRateIndex];
        this.frameRateIndex =  frameRateIndex;

        Application.targetFrameRate = frameRate;
    }
    #endregion

    #region ScreenMode
    void ClearScreenMode()
    {
        List<string> options = new List<string> { "전체화면", "창모드" };

        ScreenModeDropdown.ClearOptions();
        ScreenModeDropdown.AddOptions(options);
        ScreenModeDropdown.onValueChanged.AddListener(index => ChangeFullScreenMode((ScreenMode)index));

        int screenModeIndex = PlayerPrefs.GetInt("screenMode", -1);
        if (screenModeIndex == -1)
        {
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.FullScreenWindow);
        }
        else
        {
            switch (screenModeIndex)
            {
                case 0:
                    Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.FullScreenWindow);
                    break;
                case 1:
                    Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.Windowed);
                    break;
            }
        }
       
    }

    private void ChangeFullScreenMode(ScreenMode mode)
    {
        switch (mode)
        {
            case ScreenMode.FullScreenWindow:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                screenModeIndex = (int)ScreenMode.FullScreenWindow;
                break;
            case ScreenMode.Window:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                screenModeIndex = (int)ScreenMode.Window;
                break;
        }
    }
    #endregion

    #region Btn
    public void SaveBtn()
    {
        SaveOption();
        UIManager.Instance.OptionUISet(false);
    }

    public void ResetBtn()
    {
        ResetOption();
    }

    public void ExitBtn()
    {
       Application.Quit();
    }
    #endregion
    //==========================================================

    void SaveOption()
    {
        PlayerPrefs.SetInt("resolution",resolutionIndex);
        PlayerPrefs.SetInt("frameRate", frameRateIndex);
        PlayerPrefs.SetInt("screenMode", screenModeIndex);
        PlayerPrefs.Save();
    }

    void ResetOption()
    {
        resolutionIndex = PlayerPrefs.GetInt("resolution");
        frameRateIndex = PlayerPrefs.GetInt("frameRate");
        screenModeIndex = PlayerPrefs.GetInt("screenMode");

        SetResolution(resolutionIndex);
        resolutionDropdown.value = resolutionIndex;
        resolutionDropdown.RefreshShownValue();
        SetFrameRate(frameRateIndex);
        frameRateDropdown.value = frameRateIndex;
        frameRateDropdown.RefreshShownValue();
        ChangeFullScreenMode((ScreenMode)screenModeIndex);
        ScreenModeDropdown.value = screenModeIndex;
        ScreenModeDropdown.RefreshShownValue();
    }
}

//==========================================================

public class ResolutionComparer : IEqualityComparer<Resolution>
{
    public bool Equals(Resolution x, Resolution y)
    {
        return x.width == y.width && x.height == y.height;
    }

    public int GetHashCode(Resolution obj)
    {
        return obj.width.GetHashCode() ^ obj.height.GetHashCode();
    }
}