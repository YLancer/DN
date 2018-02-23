using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Menu : MonoBehaviour
{
    public Button uiButton_Menu;
    public GameObject target;
    bool flag;
    void OnEnable()
    {
        uiButton_Menu.onClick.AddListener(delegate { Fun(); });
    }
    void Start()
    {
        target.SetActive(false);
    }
    void Fun()
    {
        if(!target.activeSelf)
        target.SetActive(true);
        else
            target.SetActive(false);
    }
}
