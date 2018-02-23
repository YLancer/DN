using AssemblyCSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LitJson;
using UnityEngine.UI;

public class merberSubScript: MonoBehaviour
{
    public Image headIconImg;
    public Text nickname;
    public Text ID;
    private string headIcon;
    Texture2D texture2D;
    public void setValue(ClubAccount vo)
    {
        headIcon = vo.headicon;
        nickname.text = vo.nickname;
        ID.text = vo.uuid.ToString();
        //clubname.text = vo.clubname;
        //membercount.text = vo.membercount.ToString();
        //roomcount.text = vo.roomcount.ToString();
        StartCoroutine(LoadImg());
    }

    private IEnumerator LoadImg()
    {
        //开始下载图片
        if (headIcon != null && headIcon != "")
        {
            if (headIcon.IndexOf("http") == -1)
            {
                headIconImg.sprite = GlobalDataScript.getInstance().headSprite;
                yield break;
            }
            if (FileIO.wwwSpriteImage.ContainsKey(headIcon))
            {
                headIconImg.sprite = FileIO.wwwSpriteImage[headIcon];
                yield break;
            }

            WWW www = new WWW(headIcon);
            yield return www;
            //下载完成，保存图片到路径filePath
            try
            {
                texture2D = www.texture;
                byte[] bytes = texture2D.EncodeToPNG();
                //将图片赋给场景上的Sprite
                Sprite tempSp = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0, 0));
                headIconImg.sprite = tempSp;
                FileIO.wwwSpriteImage.Add(headIcon, tempSp);
            }
            catch (Exception e)
            {

                MyDebug.Log("LoadImg" + e.Message);
            }
        }
    }
}

