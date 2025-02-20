using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Menu
{
    START,
    PAUSE,
    GAMEOVER
}
public class UIController : MonoBehaviour
{
    [SerializeField] private Dictionary<GameObject,Menu> menus;

    public void OpenMenu(Menu menu)
    {
        foreach (var m in menus)
        {
            m.Key.SetActive(m.Value == menu);
        }
    }

    public void CloseMenu(Menu menu)
    {
        menus.Where(x => x.Value == menu).First().Key.SetActive(false);
    }

    public void CloseAllMenus()
    {
        foreach (var m in menus)
        {
            m.Key.SetActive(false);
        }
    }
}
