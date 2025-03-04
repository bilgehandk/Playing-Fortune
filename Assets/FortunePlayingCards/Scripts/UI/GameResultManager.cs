using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Localization.Settings;

public class GameResultManager : MonoBehaviour
{
    public TMP_Text gameResultText;
    public Button closeButton;
    public Image loadingBar;
    public GameObject loadingPanel;
    public GameObject gameResultPanel;

    public static string response;

    private float loadingDuration = 2.5f; // Yüklenme süresi (saniye cinsinden)

    void Start()
    {
        loadingPanel.SetActive(true);
        gameResultPanel.SetActive(false);
        StartCoroutine(UpdateProgressBar());

        if (Application.internetReachability==new NetworkReachability())
        {
            GameManager.Instance.LoadScene(SceneType.InternetCheck);
        }
        else
        {
            if (AdsManager.Instance.IsInterstitialAdReady())
            {
                AdsManager.Instance._InterstitialAd.ShowAd();
            }
            if (string.IsNullOrEmpty(response))
            {
                loadingPanel.SetActive(false);
                gameResultPanel.SetActive(true);
                switch (LocalizationManager.Instance.GetCurrentLanguage())
                {
                    case SupportedLanguages.Arabic:
                        gameResultText.text = "حاول مرة أخرى!";
                        break;
                    case SupportedLanguages.English:
                        gameResultText.text = "Try again!";
                        break;
                    case SupportedLanguages.Italian:
                        gameResultText.text = "Riprova!";
                        break;
                    case SupportedLanguages.Portuguese:
                        gameResultText.text = "Tente novamente!";
                        break;
                    case SupportedLanguages.Spanish:
                        gameResultText.text = "¡Intenta de nuevo!";
                        break;
                    case SupportedLanguages.Turkish:
                        gameResultText.text = "Bir daha şansını dene!";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                loadingPanel.SetActive(false); // Yanıt yoksa loading panelini kapat
            }
            else
            {
                string gptResponse = null;
                string name;
                string userInfo;
                switch (LocalizationManager.Instance.GetCurrentLanguage())
                {
                    case SupportedLanguages.Arabic:
                        name = PlayerPrefs.GetString("name");
                        userInfo = "الاسم: " + name;
                        userInfo += ", الجنس: " + PlayerPrefs.GetString("gender");
                        userInfo += ", العمر: " + PlayerPrefs.GetString("age");
                        userInfo += ", الحالة الاجتماعية: " + PlayerPrefs.GetString("relationship");
                        gptResponse = $"هل يمكنك أن تتصرف كقارئ الطالع في لعبة قراءة الطالع بالورق الخاصة بي وتكتب لي قراءة قصيرة بناءً على معاني هذه الأوراق في قراءة الطالع {response} ومعلومات الشخص الذي تقرأ له الطالع {userInfo}؟ عند إعطاء الطالع، ترجم أسماء الأوراق إلى العربية. تأكد من أن الطالع لا يتجاوز 1000 حرف. انقل خصائص الأوراق مباشرة. لا تبدأ النص بقول مثل 'بالطبع، هذه هي خصائص الأوراق'، بل ابدأ بـ 'مرحبًا، عزيزي {name}' ثم أرسل الطالع.";
                        break;
                    case SupportedLanguages.English:
                        name = PlayerPrefs.GetString("name");
                        userInfo = "Name: " + name;
                        userInfo += ", Gender: " + PlayerPrefs.GetString("gender");
                        userInfo += ", Age: " + PlayerPrefs.GetString("age");
                        userInfo += ", Relationship: " + PlayerPrefs.GetString("relationship");
                        gptResponse =
                            $"Could you act like a fortune teller for my playing card fortune game and write a short fortune based on the meanings of these cards in playing card fortune-telling {response}, tailored to the information of the person whose fortune is being read {userInfo}? When giving the fortune, translate the names of the cards to English as well. Make sure it's no longer than 1000 characters. Directly convey the characteristics of the cards. Don't start the text by saying things like 'Of course, these are the features of the cards,' instead, begin with 'Hello, Dear Fortune Seeker {name}' and then send the fortune.";
                        break;
                
                    case SupportedLanguages.Italian:
                        name = PlayerPrefs.GetString("name");
                        userInfo = "Nome: " + name;
                        userInfo += ", Genere: " + PlayerPrefs.GetString("gender");
                        userInfo += ", Età: " + PlayerPrefs.GetString("age");
                        userInfo += ", Relazione: " + PlayerPrefs.GetString("relationship");
                        gptResponse = $"Potresti comportarti come un cartomante per il mio gioco di carte e scrivere una breve lettura basata sui significati di queste carte nella cartomanzia {response}, adattata alle informazioni della persona di cui stai leggendo il futuro {userInfo}? Quando dai il responso, traduci anche i nomi delle carte in italiano. Assicurati che non superi i 1000 caratteri. Riporta direttamente le caratteristiche delle carte. Non iniziare il testo dicendo cose come 'Certo, queste sono le caratteristiche delle carte,' ma inizia con 'Ciao, caro {name}' e poi invia la lettura.";
                        break;
                
                    case SupportedLanguages.Portuguese:
                        name = PlayerPrefs.GetString("name");
                        userInfo = "Nome: " + name;
                        userInfo += ", Gênero: " + PlayerPrefs.GetString("gender");
                        userInfo += ", Idade: " + PlayerPrefs.GetString("age");
                        userInfo += ", Relacionamento: " + PlayerPrefs.GetString("relationship");
                        gptResponse = $"Você poderia agir como um cartomante para o meu jogo de cartas e escrever uma breve leitura baseada nos significados dessas cartas na cartomancia {response}, adaptada às informações da pessoa cuja fortuna está sendo lida {userInfo}? Ao dar a leitura, traduza os nomes das cartas para o português também. Certifique-se de que não ultrapasse 1000 caracteres. Transmita diretamente as características das cartas. Não comece o texto dizendo coisas como 'Claro, essas são as características das cartas,' em vez disso, comece com 'Olá, Querido {name}' e depois envie a leitura.";
                        break;
                
                    case SupportedLanguages.Spanish:
                        name = PlayerPrefs.GetString("name");
                        userInfo = "Nombre: " + name;
                        userInfo += ", Género: " + PlayerPrefs.GetString("gender");
                        userInfo += ", Edad: " + PlayerPrefs.GetString("age");
                        userInfo += ", Relación: " + PlayerPrefs.GetString("relationship");
                        gptResponse = $"¿Podrías actuar como un adivino para mi juego de cartas y escribir una lectura corta basada en los significados de estas cartas en la adivinación con cartas {response}, adaptada a la información de la persona cuya fortuna se está leyendo {userInfo}? Al dar la lectura, traduce también los nombres de las cartas al español. Asegúrate de que no supere los 1000 caracteres. Transmite directamente las características de las cartas. No comiences el texto diciendo cosas como 'Por supuesto, estas son las características de las cartas,' en lugar de eso, comienza con 'Hola, Querido {name}' y luego envía la lectura.";
                        break;
                
                    case SupportedLanguages.Turkish:
                        name = PlayerPrefs.GetString("name");
                        userInfo = "İsmi: " + name;
                        userInfo += ", Cinsiyeti: " + PlayerPrefs.GetString("gender");
                        userInfo += ", Yaşı: " + PlayerPrefs.GetString("age");
                        userInfo += ", İlişki Durumu: " + PlayerPrefs.GetString("relationship");
                        gptResponse = $"İskambil falı oyunum için falcı gibi davranıp şu kartların iskambil falındaki anlamlarına göre {response} ve falına baktığın kişilerin bilgileri {userInfo} kişiye göre kısa bir fal yazar mısın? {response} kartların isimlerini de Türkçeye çevir misin falı söylerken? 1000 harfi geçmesin. Direk kart özelliklerini aktar. Tabii ki, işte bunlar kartların özellikleri falan diye başlama metine, 'Merhaba Sevgili Fal Baktıran {name}' diye başla ve falını gönder.";
                        break;
                
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                ChatGPTIntegration chatGPT = gameObject.AddComponent<ChatGPTIntegration>();
                StartCoroutine(chatGPT.GetChatGPTResponse(gptResponse, OnResponseReceived));
            }
        }
        

        closeButton.onClick.AddListener(CloseGameResult);
    }
    

    private IEnumerator UpdateProgressBar()
    {
        float elapsedTime = 0f;
        while (elapsedTime < loadingDuration)
        {
            elapsedTime += Time.deltaTime;
            loadingBar.fillAmount = Mathf.Clamp01(elapsedTime / loadingDuration);
            yield return null;
        }
        // Loading bar tamamlandıktan sonra yanıt alınabilir
    }

    void OnResponseReceived(string response)
    {
        loadingPanel.SetActive(false);
        gameResultPanel.SetActive(true);
        Debug.Log("ChatGPT Response: " + response);
        gameResultText.text = response;
    }

    public void CloseGameResult()
    {
        
        GameManager.Instance.LoadScene(SceneType.Entrance);
        
    }
}
