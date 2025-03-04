using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance { get; private set; }
    private Dictionary<SupportedLanguages, string> localeCodes;

    void Awake()
    {
        // Ensure there's only one instance of the LocalizationManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates
        }

        // Locale kodlarını enum ile eşleştiriyoruz
        localeCodes = new Dictionary<SupportedLanguages, string>
        {
            { SupportedLanguages.Arabic, "ar" },
            { SupportedLanguages.English, "en" },
            { SupportedLanguages.Italian, "it" },
            { SupportedLanguages.Portuguese, "pt" },
            { SupportedLanguages.Spanish, "es" },
            { SupportedLanguages.Turkish, "tr" }
        };
    }

    void Start()
    {
        // Eğer daha önce bir dil kaydedildiyse onu kullan, yoksa sistem diline göre ayarla
        if (PlayerPrefs.HasKey("Language"))
        {
            int savedLanguageIndex = PlayerPrefs.GetInt("Language");
            ChangeLanguage((SupportedLanguages)savedLanguageIndex);
        }
        else
        {
            SetLanguageBasedOnSystem();
        }
    }

    // Cihazın sistem diline göre oyunun dilini ayarla
    void SetLanguageBasedOnSystem()
    {
        SystemLanguage systemLanguage = Application.systemLanguage;

        SupportedLanguages languageToSet = SupportedLanguages.English; // Varsayılan olarak İngilizce

        switch (systemLanguage)
        {
            case SystemLanguage.Arabic:
                languageToSet = SupportedLanguages.Arabic;
                break;
            case SystemLanguage.English:
                languageToSet = SupportedLanguages.English;
                break;
            case SystemLanguage.Italian:
                languageToSet = SupportedLanguages.Italian;
                break;
            case SystemLanguage.Portuguese:
                languageToSet = SupportedLanguages.Portuguese;
                break;
            case SystemLanguage.Spanish:
                languageToSet = SupportedLanguages.Spanish;
                break;
            case SystemLanguage.Turkish:
                languageToSet = SupportedLanguages.Turkish;
                break;
            default:
                languageToSet = SupportedLanguages.English; // Varsayılan dil olarak İngilizce kullan
                break;
        }

        // Cihazın sistem diline göre dil ayarını yap
        ChangeLanguage(languageToSet);
    }

    // Enum'a göre dil değiştirme işlemi
    public void ChangeLanguage(SupportedLanguages language)
    {
        if (!localeCodes.ContainsKey(language)) return;

        string localeCode = localeCodes[language];

        // Locale'i bul ve ayarla
        Locale selectedLocale = LocalizationSettings.AvailableLocales.GetLocale(localeCode);
        if (selectedLocale != null)
        {
            LocalizationSettings.SelectedLocale = selectedLocale;

            // Seçilen dili kaydet
            PlayerPrefs.SetInt("Language", (int)language);
            PlayerPrefs.Save();
        }
    }

    // Aktif dilin enum türünden döndürülmesi
    public SupportedLanguages GetCurrentLanguage()
    {
        string currentLocaleCode = LocalizationSettings.SelectedLocale.Identifier.Code;

        foreach (var pair in localeCodes)
        {
            if (pair.Value == currentLocaleCode)
            {
                return pair.Key;
            }
        }

        return SupportedLanguages.English; // Eğer bulunamazsa varsayılan olarak İngilizce döner
    }
}



public enum SupportedLanguages
{
    Arabic,
    English,
    Italian,
    Portuguese,
    Spanish,
    Turkish
}
