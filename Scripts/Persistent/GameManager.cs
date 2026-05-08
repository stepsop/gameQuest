using UnityEngine;
using System.Collections.Generic;

public sealed class GameManager : MonoBehaviour
{
    // Единственный "корневой" объект игры, живущий между сценами.
    // Держим здесь персистентные менеджеры, чтобы они:
    // - существовали с самого старта игры (даже если стартовая сцена их не содержит)
    // - не уничтожались при переходе сцен
    public static GameManager Instance { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        // Unity вызовет этот метод автоматически ДО загрузки первой сцены.
        // Это максимально безопасная точка для bootstrap'а: сцены ещё не трогались.
        if (Instance != null) return;

        // Создаём GameObject вручную, чтобы не зависеть от наличия "MainLogic" сцены
        // и от того, какие объекты расставлены в сценах.
        var go = new GameObject(nameof(GameManager));
        go.AddComponent<GameManager>();

        // Эти компоненты должны быть "глобальными" и сохраняться между сценами.
        // Важно: сами классы additionally защищены от дублей через Instance-check в Awake().
        go.AddComponent<PickupTracker>();
        go.AddComponent<InventoryManager>();
        
    }

    private void Awake()
    {
        // Классическая защита от дублей:
        // если по ошибке появится второй GameManager (например, кто-то положит его в сцену),
        // то новый уничтожится, а старый останется.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Ключевая строка: объект НЕ уничтожается при смене сцены.
        DontDestroyOnLoad(gameObject);
    }
}

