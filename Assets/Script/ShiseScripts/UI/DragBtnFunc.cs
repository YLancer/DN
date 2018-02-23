using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragBtnFunc : MonoBehaviour
{
    public GameObject dragDownBtn;
    public GameObject dragUpBtn;
    public GameObject[] hideBtn;


    public void  DragDownFunc()
    {
        dragDownBtn.SetActive(false);
        dragUpBtn.SetActive(true);
        for (int i = 0; i < hideBtn.Length; i++)
        {
            hideBtn[i].SetActive(true);
        }
    }

    public void DragUpFunc()
    {
        dragDownBtn.SetActive(true);
        dragUpBtn.SetActive(false);
        for (int i = 0; i < hideBtn.Length; i++)
        {
            hideBtn[i].SetActive(false);
        }
    }
}
