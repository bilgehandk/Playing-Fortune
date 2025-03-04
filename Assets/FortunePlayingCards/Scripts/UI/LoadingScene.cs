using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    public Image loadingBarFill;  // Yükleme barı referansı
    public float loadDuration = 3.5f;  // Yükleme süresi (saniye)

    private void Awake()
    {
        // Sahne geçişlerinde LoadingScene objesinin yok edilmemesi için
        DontDestroyOnLoad(gameObject);

        // Eğer loadingBarFill referansı atanmadıysa hata mesajı döndür
        if (loadingBarFill == null)
        {
            Debug.LogError("Loading bar fill is not assigned!");
        }
    }

    private void Start()
    {
        // Yükleme işlemini başlat
        StartCoroutine(LoadAsyncScene());
    }

    // Asenkron sahne yükleme işlemi
    private IEnumerator LoadAsyncScene()
    {
        // Yükleme süresi hesaplaması
        float elapsedTime = 0f;

        // Yükleme çubuğunu güncelle
        while (elapsedTime < loadDuration)
        {
            elapsedTime += Time.deltaTime;  // Geçen süreyi güncelle
            float progress = Mathf.Clamp01(elapsedTime / loadDuration);  // İlerleme çubuğu oranını hesapla
            UpdateProgress(progress);  // İlerleme çubuğunu güncelle

            yield return null;  // Bir sonraki frame'e geç
        }

        // Yükleme süresi tamamlandığında, %100 dolu olan çubuğu göster
        UpdateProgress(1f);

        // Yükleme işlemi tamamlandığında GameScene'i aç
        GameManager.Instance.LoadScene(SceneType.Eleven);
        CardDeckNew.Instance.InstantiateDeck();
    }

    // Yükleme barını güncelleyen metod
    public void UpdateProgress(float progress)
    {
        if (loadingBarFill != null)
        {
            loadingBarFill.fillAmount = progress;
        }
        else
        {
            Debug.LogError("Loading bar fill is not assigned in the Inspector.");
        }
    }
}