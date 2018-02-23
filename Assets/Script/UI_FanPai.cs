using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//public class UI_FanPai : MonoBehaviour, IPointerDownHandler, IDragHandler
//{
//    Animator anim;
//    SpriteRenderer sprite;
//    public Sprite sp;
//    public MyDNScript myDN;
//    void Start()
//    {
//        anim=GetComponentInChildren <Animator>();
//        sprite = GetComponentInChildren<SpriteRenderer>();
//    }


////    bool isDown = false;//是否被按下
//    Vector2 downPoint;
//    /// <summary>
//    /// 光标按下时记录屏幕坐标
//    /// </summary>
//    public void OnPointerDown(PointerEventData eventData)
//    {
//        downPoint = eventData.position;
////        isDown = true;
//    }
//    /// <summary>
//    /// 滑动时计算偏移量
//    /// </summary>
//    public void OnDrag(PointerEventData eventData)
//    {
//        Vector2 offset = eventData.position - downPoint;

//        anim.SetBool("CuoPaiBool", offset.y>10);
//        Invoke("Close", 1.5f);
//    }
//    void Close()
//    {
//        this.gameObject.SetActive(false);
//        myDN.FanPai(0, 5);
//        sprite.sprite = sp;
//    }
//}

// Make by Dongdong

public class UI_FanPai : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Vector3 originalPos;
    void Start()
    {
        originalPos = transform.position;
        Invoke("Close", 3f);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        SetDraggedPosition(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        SetDraggedPosition(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SetDraggedPosition(eventData);
    }

    void SetDraggedPosition(PointerEventData eventData)
    {
        RectTransform rt = GetComponent<RectTransform>();

        Vector3 cursorPos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, eventData.pressEventCamera, out cursorPos))
        {
            rt.position = cursorPos;
        }
    }

    void Close()
    {
        GameObject parentGo = transform.parent.gameObject;
        if (parentGo.activeInHierarchy)
            parentGo.SetActive(false);
    }

    void OnDisable()
    {
        transform.position = originalPos;
    }
}