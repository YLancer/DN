using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIButton_CuoPai : MonoBehaviour {
    //搓牌的按钮，需要出发的事件
    public GameObject target;
    public MyDNScript myDN;
	void OnEnable()
    {
        GetComponent<Button>().onClick.AddListener(delegate { FanPaiFun(); });
    }
    void FanPaiFun()
    {
        target.SetActive(true);
        this.gameObject.SetActive(false);
        Invoke("FanPaiDelay", 5f);
    }
    void FanPaiDelay()
    {
        myDN.FanPai(0);
    }
}
