using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Mail;
using TMPro;

public class ReportPopUp : MonoBehaviour
{
    public GameObject popUpPanel; // Pop-up paneli
    public GameObject header;
    public TMP_Dropdown languageDropdown;
    public Button soundOn;
    public Button soundOff;
    public Button openReportPopupButton;
    public Button closeSettingsButton;
    public Button closeButton; // Kapatma butonu
    public Button sendButton; // Gönderme butonu
    public TMP_InputField emailInputField; // Email input alanı
    public TMP_InputField messageInputField; // Mesaj input alanı

    void Start()
    {
        closeButton.onClick.AddListener(ClosePopUp);
        sendButton.onClick.AddListener(SendReport);
    }

    
    void ClosePopUp()
    {
        popUpPanel.SetActive(false);
        header.gameObject.SetActive(true);
        languageDropdown.gameObject.SetActive(true);
        
        if (PlayerPrefs.GetInt("IsSoundActive") == 1)
            soundOn.gameObject.SetActive(true);
        else
            soundOff.gameObject.SetActive(true);
        
        openReportPopupButton.gameObject.SetActive(true);
        closeSettingsButton.gameObject.SetActive(true);
    }
    
    void SendReport()
    {
        SendEmail();
        
        ClosePopUp();
    }

    void SendEmail()
    {
        string senderEmail = emailInputField.text;
        string messageBody = messageInputField.text;

        try
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(senderEmail); // Gönderen e-posta adresi
            mail.To.Add("bilgehand2014@gmail.com"); // Alıcı e-posta adresi
            mail.Subject = "Kullanıcı Raporu";
            mail.Body = $"Gönderen: {senderEmail}\nMesaj:\n{messageBody}";

            SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
            smtpServer.Port = 587;
            smtpServer.Credentials = new NetworkCredential("bilgehand2014@gmail.com", "hqrgkgrxvrxovjuu") as ICredentialsByHost;
            smtpServer.EnableSsl = true;

            smtpServer.Send(mail);
            Debug.Log("Rapor gönderildi.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("E-posta gönderme hatası: " + ex.Message);
        }
    }
}