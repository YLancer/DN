using AssemblyCSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class JoinClubScript:MonoBehaviour
{
    public Button ok;
    public Button no;
    public InputField clubid;

    private void Awake()
    {
        ok.onClick.AddListener(onOKclick);
        no.onClick.AddListener(onONclick);
    }
    private void onOKclick()
    {
        string club = clubid.text;
        if (club == "")
        {
            TipsManagerScript.getInstance().setTipsClub("请输入俱乐部ID");
        }
        else
        {
            ClientRequest cr = new ClientRequest();
            cr.headCode = APIS.JOIN_CLUB_REQUEST;
            cr.messageContent = "{'clubname':" + club + "}"; ;
            CustomSocket.getInstance().sendMsg(cr);
        }
    }
    private void onONclick()
    {
        this.gameObject.SetActive(false);
    }
}

