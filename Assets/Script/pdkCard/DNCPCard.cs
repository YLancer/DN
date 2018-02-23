using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using AssemblyCSharp;

public class DNCPCard : MonoBehaviour
{
    public int cardPoint = -1;
    public Image pointImage;
    public Image kingPointImage;
    public Image typeImage;
    public Image centerImage;
    public Image kingCenterImage;
    public bool hasShow = false;
    public bool isSmall = false;

    public void setPoint(int _cardPoint)
    {

       
        
            GlobalDataScript.isDrag = false;
            if (_cardPoint == 52)
            {
                cardPoint = _cardPoint;
                
				typeImage.gameObject.SetActive (false);
				pointImage.gameObject.SetActive (false);
				centerImage.gameObject.SetActive (false);
				kingPointImage.gameObject.SetActive (true);
				kingCenterImage.gameObject.SetActive (true);
				
                kingCenterImage.sprite = Resources.Load("pdk/card/" + "20_1", typeof(Sprite)) as Sprite;
                kingPointImage.sprite = Resources.Load("pdk/card/" + "20_2", typeof(Sprite)) as Sprite;

            }
            else if (_cardPoint == 53)
            {
                cardPoint = _cardPoint;
               
				typeImage.gameObject.SetActive (false);
				pointImage.gameObject.SetActive (false);
				centerImage.gameObject.SetActive (false);
				kingPointImage.gameObject.SetActive (true);
				kingCenterImage.gameObject.SetActive (true);
				
                kingCenterImage.sprite = Resources.Load("pdk/card/" + "21_1", typeof(Sprite)) as Sprite;
                kingPointImage.sprite = Resources.Load("pdk/card/" + "21_2", typeof(Sprite)) as Sprite;
            }
            else if (_cardPoint >= 0)
            {
                int point = _cardPoint % 13 + 1;
                int type = _cardPoint / 13;
            typeImage.gameObject.SetActive(true);
            pointImage.gameObject.SetActive(true);
            centerImage.gameObject.SetActive(true);
            kingPointImage.gameObject.SetActive(false);
            kingCenterImage.gameObject.SetActive(false);
            cardPoint = _cardPoint;//设置所有牌指针
                typeImage.sprite = Resources.Load("pdk/card/type" + (3 - type), typeof(Sprite)) as Sprite;
                centerImage.sprite = Resources.Load("pdk/card/type" + (3 - type), typeof(Sprite)) as Sprite;

                if (type == 1 || type == 3)
                    pointImage.sprite = Resources.Load("pdk/card/b_" + point, typeof(Sprite)) as Sprite;
                else
                    pointImage.sprite = Resources.Load("pdk/card/r_" + point, typeof(Sprite)) as Sprite;
            }
            else if (_cardPoint == -1)
            {
                cardPoint = _cardPoint;
                return;
            }
        
       
    }
}

