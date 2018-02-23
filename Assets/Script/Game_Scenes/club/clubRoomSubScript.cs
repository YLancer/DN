using AssemblyCSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LitJson;
using UnityEngine.UI;

public class clubRoomSubScript : MonoBehaviour
{
    public Image headIconImg;
    public Text nickname;
    public Text ID;
    private string headIcon;
    Texture2D texture2D;
    public Text wanfa;
    public Text roomid;
    public Text playnum;
    public Image waite;
    public Image playing;
    public Text socre;
    public Text round;
    public Text pay;
    public Text guizhe;
    public Text teshu;
    public Button joinBtn;
    private bool isgame;
    private int clubroomid;

    private void Awake()
    {
        joinBtn.onClick.AddListener(joinBtnOnClick);
    }
    private void joinBtnOnClick()
    {
        if(isgame)
        {
            TipsManagerScript.getInstance().setTips("以开始游戏无法进入");
        }
        else
        {
            RoomJoinVo roomJoinVo = new RoomJoinVo();
            roomJoinVo.roomId =clubroomid;
            string sendMsg = JsonMapper.ToJson(roomJoinVo);
            //StartCoroutine(ConnectTime(4f, 1));
            CustomSocket sok = CustomSocket.getInstance();
            sok.sendMsg(new JoinRoomRequest(sendMsg));
        }
    }
    public void setValue(ClubRoomVO vo)
    {
        headIcon = vo.createimg;
        nickname.text = vo.createname;
        ID.text = vo.createuui.ToString();
        if(vo.ruleType==1)
        {
            wanfa.text = "看牌抢庄";
        }
        if (vo.ruleType == 2)
        {
            wanfa.text = "闲家推注";
        }
        if (vo.ruleType == 3)
        {
            wanfa.text = "牛牛换庄";
        }
        if (vo.ruleType == 4)
        {
            wanfa.text = "轮流当庄";
        }
        if (vo.ruleType == 5)
        {
            wanfa.text = "房主霸王庄";
        }
        if (vo.ruleType == 6)
        {
            wanfa.text = "最大牌为庄";
        }
        roomid.text = "房间号：" + vo.roomID.ToString();
        clubroomid = vo.roomID;
        isgame = vo.isgame;
        playnum.text = "人数：" +vo.playnum.ToString()+"/"+ vo.playerAmounts.ToString();
        socre.text = vo.creditScoreMultiply.ToString();
        if(vo.isgame)
        {
            waite.gameObject.SetActive(false);
            playing.gameObject.SetActive(true);
        }
        else
        {
            waite.gameObject.SetActive(true);
            playing.gameObject.SetActive(false);
        }
        round.text = vo.roundNumber.ToString();
        if(vo.rules==1)
        {
            guizhe.text = "普通模式1";
        }
        if (vo.rules == 2)
        {
            guizhe.text = "普通模式2";
        }
        if (vo.rules == 3)
        {
            guizhe.text = "扫雷模式";
        }
        string tsguize = "";
        if(vo.special)
        {
            tsguize += "特殊牌型";
        }
        if(vo.AA)
        {
            pay.text = "AA支付";
        }
        else
        {
            pay.text = "房主支付";
        }
        teshu.text = tsguize;
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

