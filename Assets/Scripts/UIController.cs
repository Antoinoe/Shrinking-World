using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[Serializable]
public enum MenuType
{
    START,
    PAUSE,
    GAMEOVER
}

[Serializable]
public class Menu
{
    public GameObject menuObject;
    public MenuType menuType;
}

public class UIController : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private List<Menu> menus;
    [SerializeField] private TextMeshProUGUI gameOverScoreText;
    [SerializeField] private TextMeshProUGUI inGameScoreText;

    private const string SCORE_TEXT_HEADER = "Score: ";

    public void DisplayCanvas(bool state)
    {
        if (!canvas)
            return;
        canvas.gameObject.SetActive(state);
    }

    public void OpenMenu(MenuType menu)
    {
        foreach (var m in menus)
        {
            m.menuObject.SetActive(m.menuType == menu);
        }
    }

    public void CloseMenu(MenuType menu)
    {
        menus.Where(x => x.menuType == menu).First().menuObject.SetActive(false);
    }

    public void CloseAllMenus()
    {
        foreach (var m in menus)
        {
            m.menuObject.SetActive(false);
        }
    }

    public void RedirectTo(string url)
    {
        Application.OpenURL(url);
    }

    private void Start()
    {
        GameManager.Instance.OnGameStarts.AddListener(() => CloseAllMenus());
        GameManager.Instance.OnGameOver.AddListener(() => OnGameOver());
        GameManager.Instance.OnPauseToggled.AddListener(() => TogglePauseMenu());
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGamePaused && GameManager.Instance.IsGameRunning)
            inGameScoreText.text = $"{SCORE_TEXT_HEADER}{(int)GameManager.Instance.Score}";
    }

    private void OnGameOver()
    {
        gameOverScoreText.text = $"{SCORE_TEXT_HEADER}{(int)GameManager.Instance.Score}";
        OpenMenu(MenuType.GAMEOVER);
    }

    private void TogglePauseMenu()
    {
        if (GameManager.Instance.IsGamePaused)
            OpenMenu(MenuType.PAUSE);
        else
            CloseMenu(MenuType.PAUSE);
    }
}
