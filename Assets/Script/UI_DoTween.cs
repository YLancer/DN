using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;//DOTween命名空间

public class UI_DoTween : MonoBehaviour
{
    public GameObject target;
    public GameObject targetT;
    Vector3 targetPos;
    Color colorTarget;
    Color colorTargetT;
    bool flag;
    void Start()
    {
        colorTarget = target.GetComponent<Image>().color;
        colorTargetT = targetT.GetComponent<Text>().color;
        targetPos = target.transform.position;
    }
    public void Fun()
    {
        target.SetActive(true);
        colorTarget.a = 1;
        colorTargetT.a = 1;
        target.GetComponent<Image>().color = colorTarget;
        target.transform.position = targetPos;
        target.transform.DOMoveY(300, 0.5f);
        flag = true;
    }
    void Update()
    {
        if (flag)
        {
            colorTarget.a -= Time.deltaTime;
            colorTargetT.a -= Time.deltaTime;
            target.GetComponent<Image>().color = colorTarget;
            targetT.GetComponent<Text>().color = colorTargetT;
            if (colorTarget.a <= 0 && colorTargetT.a<=0)
                flag = false;
        }
    }
}
