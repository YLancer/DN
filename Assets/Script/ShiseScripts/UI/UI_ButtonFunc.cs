using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Make by Shise

public class UI_ButtonFunc : MonoBehaviour
{
    public GameObject[] needHideThings;     // 要隐藏的东西
    public GameObject[] needAppearThings;   // 要显示的东西

    /// <summary>
    /// 隐藏东西的方法
    /// </summary>
    public void HideThingsFunc()
    {
        if(needHideThings.Length!=0)
        {
            foreach (var thing in needHideThings)
            {
                thing.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 显示隐藏东西的方法
    /// </summary>
    public void AppearThingsFunc()
    {
        foreach (var thing in needAppearThings)
        {
            thing.SetActive(true);
        }
    }
}
