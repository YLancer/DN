using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class dnGold : MonoBehaviour
{

    public Image img;

    public void move(Vector3 forv3, Vector3 tov3)
    {
        gameObject.transform.localPosition = forv3;
        //img.sprite = Resources.Load("hd/hdimage" + hdindex.ToString(), typeof(Sprite)) as Sprite;
        img.gameObject.SetActive(true);
        SoundCtrl.getInstance().playSoundByActionButton(11);
        gameObject.transform.DOLocalMove(tov3, 1).OnComplete(() =>
        {
            img.gameObject.SetActive(false);
            //Game.SoundManager.PlayHuDong(hdindex);



            //showMood = false;
            gameObject.gameObject.SetActive(false);
            //Destroy(go);
            Destroy(gameObject);



        });
    }
    //public  Image[] dnGolds;

    //public void move(Vector3 forv3, Vector3 tov3)
    //{
    //    gameObject.transform.localPosition = forv3;
    //    gameObject.transform.LookAt(tov3);
    //    //img.sprite = Resources.Load("hd/hdimage" + hdindex.ToString(), typeof(Sprite)) as Sprite;
    //    foreach (var gold in dnGolds)
    //    {
    //        gold.gameObject.SetActive(true);
    //    }
    //    SoundCtrl.getInstance().playSoundByActionButton(11);
    //    gameObject.transform.DOLocalMove(tov3, 1).OnComplete(() =>
    //    {
    //        foreach (var gold in dnGolds)
    //        {
    //            gold.gameObject.SetActive(false);
    //        }
    //        //Game.SoundManager.PlayHuDong(hdindex);



    //        //showMood = false;
    //        gameObject.SetActive(false);
    //        //Destroy(go);
    //        Destroy(gameObject);

    //    });
    //}

    public void setImg(string Imgname)
    {
        img.sprite = Resources.Load("hd/hdimage" + Imgname, typeof(Sprite)) as Sprite;
    }
}
