using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DropdownFunc : MonoBehaviour
{
    public GameObject backGround;
    public Toggle[] toggles;
    public Text lablel;
    private Button btn_Drog;
    public Button btn_Drog_Up;
    public Button btn_Drog_Down;

    public void Start()
    {
        btn_Drog = this.GetComponentInChildren<Button>();
        toggles = this.GetComponentsInChildren<Toggle>();
        foreach (var toggle in toggles)
        {
            toggle.onValueChanged.AddListener(delegate(bool isOn)
            {
                TogglesFunc(isOn, toggle);
            });
        }
        backGround.SetActive(false);
    }

    private void TogglesFunc(bool isOn, Toggle toggle)
    {
        if (isOn)
        {
            lablel.text = toggle.GetComponentInChildren<Text>().text;
            backGround.SetActive(false);
            btn_Drog_Down.gameObject.SetActive(false);
            btn_Drog_Up.gameObject.SetActive(true);
        }
        else
        {
            backGround.SetActive(true);
            btn_Drog_Down.gameObject.SetActive(true);
            btn_Drog_Up.gameObject.SetActive(false);

        }
    }
}
