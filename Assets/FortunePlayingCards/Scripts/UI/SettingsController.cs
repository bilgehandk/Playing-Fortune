using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    // UI Elements
    public Button soundOnButton;
    public Button soundOffButton;
    public Button reportButton;
    public Button closeSettingButton;
    public TMP_Dropdown languageDropdown;
    public GameObject reportPopup;
    public GameObject header;

    // Sound setting
    private bool isSoundOn;

    void Start()
    {
        // Initialize settings
        InitializeSettings();

        // Add listeners
        soundOnButton.onClick.AddListener(TurnSoundOn);
        soundOffButton.onClick.AddListener(TurnSoundOff);
        reportButton.onClick.AddListener(ShowReportPopup);
        closeSettingButton.onClick.AddListener(CloseSettings);

        // Language change event handler using the singleton LocalizationManager
        languageDropdown.onValueChanged.AddListener(delegate { LocalizationManager.Instance.ChangeLanguage((SupportedLanguages)languageDropdown.value); });
        
        // Hide report pop-up initially
        reportPopup.SetActive(false);

        // Update sound button visibility
        UpdateSoundButtons();
    }

    // Initialize the settings based on saved preferences
    private void InitializeSettings()
    {
        // Load sound setting
        if (PlayerPrefs.HasKey("IsSoundActive"))
        {
            isSoundOn = PlayerPrefs.GetInt("IsSoundActive") == 1;
        }

        // Load language setting
        if (PlayerPrefs.HasKey("Language"))
        {
            languageDropdown.value = PlayerPrefs.GetInt("Language");
        }

        // Update sound button visibility
        UpdateSoundButtons();
    }

    // Turn the sound on
    private void TurnSoundOn()
    {
        isSoundOn = true;

        // Save the setting
        PlayerPrefs.SetInt("IsSoundActive", 1);
        PlayerPrefs.Save();
        GameManager.Instance.ToggleBackgroundMusic(isSoundOn);

        // Update sound button visibility
        UpdateSoundButtons();
    }

    // Turn the sound off
    private void TurnSoundOff()
    {
        isSoundOn = false;

        // Save the setting
        PlayerPrefs.SetInt("IsSoundActive", 0);
        PlayerPrefs.Save();
        GameManager.Instance.ToggleBackgroundMusic(isSoundOn);

        // Update sound button visibility
        UpdateSoundButtons();
    }

    // Update the visibility of the sound buttons
    private void UpdateSoundButtons()
    {
        soundOnButton.gameObject.SetActive(!isSoundOn);
        soundOffButton.gameObject.SetActive(isSoundOn);
    }

    // Show the report pop-up
    private void ShowReportPopup()
    {
        reportPopup.SetActive(true);
        header.gameObject.SetActive(false);
        languageDropdown.gameObject.SetActive(false);
        soundOnButton.gameObject.SetActive(false);
        soundOffButton.gameObject.SetActive(false);
        reportButton.gameObject.SetActive(false);
        closeSettingButton.gameObject.SetActive(false);
    }

    // Close the report pop-up (Call this from a close button on the pop-up)
    public void CloseSettings()
    {
        GameManager.Instance.LoadScene(SceneType.Entrance);
    }
}
