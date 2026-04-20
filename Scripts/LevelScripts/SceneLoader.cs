using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    // Singleton — один SceneLoader на всю игру
    public static SceneLoader Instance { get; private set; }

    [Header("Fade эффект")]
    // Чёрный Image который перекрывает экран при переходе
    [SerializeField] private Image fadeImage;

    // Сколько секунд длится затемнение
    [SerializeField] private float fadeDuration = 0.5f;

    private void Awake()
    {
        // Если SceneLoader уже существует — уничтожаем новый
        // Это нужно потому что DontDestroyOnLoad сохраняет объект
        // и при загрузке новой сцены может появиться второй
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Этот объект не уничтожается при смене сцены
        // Именно поэтому fade-эффект работает плавно между сценами
        DontDestroyOnLoad(gameObject);
    }

    // Главный метод — вызывается из SceneTransition
    // sceneName — это название файла сцены без расширения
    public void LoadScene(string sceneName)
    {
        // Запускаем корутину — она делает:
        // затемнение → загрузка сцены → осветление
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        // Блокируем управление пока идёт переход
        GameState.IsTransitioning = true;

        // Шаг 1 — плавно затемняем экран
        yield return StartCoroutine(Fade(0f, 1f));

        // Шаг 2 — загружаем новую сцену
        // LoadSceneMode.Single — выгружает старую сцену и загружает новую
        SceneManager.LoadScene(sceneName);

        // Шаг 3 — ждём один кадр чтобы сцена успела загрузиться
        yield return null;

        // Шаг 4 — говорим камере найти новый Floor
        Camera.main.GetComponent<CameraFollow>().FindBounds();

        // Шаг 5 — плавно осветляем экран
        yield return StartCoroutine(Fade(1f, 0f));

        // Разблокируем управление
        GameState.IsTransitioning = false;
    }

    // Корутина плавного изменения прозрачности
    // from — начальная прозрачность (0 = прозрачный, 1 = чёрный)
    // to — конечная прозрачность
    private IEnumerator Fade(float from, float to)
    {
        float elapsed = 0f;
        Color color = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            // Lerp плавно меняет значение от from до to
            // elapsed / fadeDuration — это прогресс от 0 до 1
            float alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);

            // Ждём следующий кадр
            yield return null;
        }

        // Гарантируем точное конечное значение
        fadeImage.color = new Color(color.r, color.g, color.b, to);
    }
}