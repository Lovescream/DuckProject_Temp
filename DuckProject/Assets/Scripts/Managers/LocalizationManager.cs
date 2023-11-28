using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LocalizationManager {

    #region Properties

    public int LanguageCount { get; private set; }
    public bool CanChangeLanguage => Initialized && !isLanguageChanging;
    public float InitializeProgress => LocalizationSettings.InitializationOperation.PercentComplete;
    public bool Initialized { get; private set; }

    #endregion

    #region Fields

    private int currentLanguageIndex;
    private bool isLanguageChanging;

    #endregion

    public void Initialize() {
        Main.StartCoroutine(InitializeLocalization());
    }

    private IEnumerator InitializeLocalization() {
        Initialized = false;
        yield return LocalizationSettings.InitializationOperation;
        LanguageCount = LocalizationSettings.AvailableLocales.Locales.Count;
        Initialized = true;

        ChangeLocale(1);
    }

    public string Get(string table, string key) {
        Locale locale = LocalizationSettings.SelectedLocale;
        return LocalizationSettings.StringDatabase.GetLocalizedString(table, key, locale);
    }

    public void ChangeNextLanguage() {
        if (!CanChangeLanguage) return;
        if (currentLanguageIndex == LanguageCount - 1)
            ChangeLocale(0);
        else ChangeLocale(++currentLanguageIndex);
    }
    public void ChangePrevLanguage() {
        if (!CanChangeLanguage) return;
        if (currentLanguageIndex == 0) ChangeLocale(LanguageCount - 1);
        else ChangeLocale(--currentLanguageIndex);
    }


    private void ChangeLocale(int index) {
        if (isLanguageChanging) return;

        currentLanguageIndex = index;
        Main.StartCoroutine(LocaleChange(currentLanguageIndex));
    }
    private IEnumerator LocaleChange(int index) {
        isLanguageChanging = true;

        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];

        isLanguageChanging = false;
    }
}