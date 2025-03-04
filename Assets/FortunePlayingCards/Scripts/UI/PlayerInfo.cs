using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{

    public TMP_InputField nameInputField;
    public TMP_Dropdown genderDropdown;
    public TMP_InputField ageInputField;
    public TMP_Dropdown relationshipDropdown;
    public Button readUserInfoButton;
    public GameObject alertPopUp;
    public Button alertPopUpButton;

    private void Start()
    {
        readUserInfoButton.onClick.AddListener(ReadUserInfo);
        alertPopUpButton.onClick.AddListener(CloseAlertPopUp);
    }

    public void ReadUserInfo()
    {
        int genderIndex = genderDropdown.value;
        string genderType = genderDropdown.options[genderIndex].text;

        int relationshipIndex = relationshipDropdown.value;
        string relationshipType = relationshipDropdown.options[relationshipIndex].text;
        
        PlayerPrefs.SetString("name", nameInputField.text);
        PlayerPrefs.SetString("gender", genderType);
        PlayerPrefs.SetString("age", ageInputField.text);
        PlayerPrefs.SetString("relationship", relationshipType);

        if (nameInputField.text == "" && ageInputField.text == "")
            alertPopUp.gameObject.SetActive(true);
        else
            GameManager.Instance.LoadScene(SceneType.Entrance);
    }

    public void CloseAlertPopUp()
    {
        alertPopUp.gameObject.SetActive(false);
    }
}
