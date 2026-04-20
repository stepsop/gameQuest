using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    [Header("UI")]
    public GameObject pauseMenuPanel;
    public Button menuButton;
    private PlayerInputActions input;

    private void Awake()
    {
        input = new PlayerInputActions();
        input.Enable();
    }

    private void Start()
    {
        pauseMenuPanel.SetActive(false);
        menuButton.gameObject.SetActive(false);
        menuButton.onClick.AddListener(ToggleMenu);
    }

    private void Update()
    {
        if (input.Player.Menu.WasPressedThisFrame())
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        bool isOpen = !pauseMenuPanel.activeSelf;
        pauseMenuPanel.SetActive(isOpen);
        GameState.IsMenuOpen = isOpen;
    }

    public void CloseMenu()
    {
        pauseMenuPanel.SetActive(false);
        GameState.IsMenuOpen = false;
    }

    public void SetMenuButtonActive(bool active)
    {
        menuButton.gameObject.SetActive(active);
    }
}