using AssemblyCSharp;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomePanel_LeaveRoomFunc : MonoBehaviour
{
    public GameObject panelExitDialog = null;
    public Image dialog_fanhui;
    string dissoliveRoomType = "";
    public Button jiesanBtn;

    public bool canClickButtonFlag = false;
    public int type = 0;

    public void toExit()
    {
        if (panelExitDialog == null)
        {
            panelExitDialog = Instantiate(Resources.Load("Prefab/Panel_Exit")) as GameObject;
            panelExitDialog.transform.parent = gameObject.transform;
            panelExitDialog.transform.localScale = Vector3.one;
            //panelCreateDialog.transform.localPosition = new Vector3 (200f,150f);
            panelExitDialog.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
            panelExitDialog.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);

        }
    }

    public void toLeaveRoom()
    {
        dialog_fanhui.gameObject.SetActive(true);
    }

    //public void toJieSan()
    //{
    //    if (canClickButtonFlag)
    //    {
    //        dissoliveRoomType = "0";
    //        TipsManagerScript.getInstance().loadDialog("申请解散房间", "你确定要申请解散房间？", doDissoliveRoomRequest, cancle);
    //    }
    //    else
    //    {
    //        TipsManagerScript.getInstance().setTips("还没有开始游戏，不能申请退出房间");
    //    }
    //}

    //public void doDissoliveRoomRequest()
    //{
    //    DissoliveRoomRequestVo dissoliveRoomRequestVo = new DissoliveRoomRequestVo();
    //    dissoliveRoomRequestVo.roomId = GlobalDataScript.loginResponseData.roomId;
    //    dissoliveRoomRequestVo.type = dissoliveRoomType;
    //    string sendMsg = JsonMapper.ToJson(dissoliveRoomRequestVo);
    //    CustomSocket.getInstance().sendMsg(new DissoliveRoomRequest(sendMsg));
    //    GlobalDataScript.isonApplayExitRoomstatus = true;
    //}

    void Start()
    {
        //if (type == 1)
        //{

        // Remove&Add by Shise
        //    jiesanBtn.onClick.AddListener(delegate ()
        //    {
        //        this.toExit();
        //    });
        //}
        //else
        if (type == 2)
        {
            jiesanBtn.onClick.AddListener(delegate ()
            {
                this.toJieSan();
            });
        }
        //else if (type == 3)
        //{
        //    jiesanBtn.onClick.AddListener(delegate ()
        //    {
        //        this.toLeaveRoom();
        //    });
        //}

        // Add

        // End
    }

    public void toJieSan()
    {
        if (canClickButtonFlag)
        {
            dissoliveRoomType = "0";
            TipsManagerScript.getInstance().loadDialog("申请解散房间", "你确定要申请解散房间？", doDissoliveRoomRequest, cancle);
        }
        else
        {
            TipsManagerScript.getInstance().setTips("还没有开始游戏，不能申请退出房间");
        }
        
    }
    //// Remove by Shise
    //public void toExit() {        
    //    if (panelExitDialog == null)
    //    {
    //        panelExitDialog = Instantiate(Resources.Load("Prefab/Panel_Exit")) as GameObject;
    //        panelExitDialog.transform.parent = gameObject.transform;
    //        panelExitDialog.transform.localScale = Vector3.one;
    //        //panelCreateDialog.transform.localPosition = new Vector3 (200f,150f);
    //        panelExitDialog.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
    //        panelExitDialog.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);

    //    }
    //}
    //// End

    public void tuichu()
    {
        OutRoomRequestVo vo = new OutRoomRequestVo();
        vo.roomId = GlobalDataScript.roomVo.roomId;
        string sendMsg = JsonMapper.ToJson(vo);
        CustomSocket.getInstance().sendMsg(new OutRoomRequest(sendMsg));
    }

    private void cancle()
    {

    }
    public void quxiao()
    {
        dialog_fanhui.gameObject.SetActive(false);
    }

    public void doDissoliveRoomRequest()
    {
        DissoliveRoomRequestVo dissoliveRoomRequestVo = new DissoliveRoomRequestVo();
        dissoliveRoomRequestVo.roomId = GlobalDataScript.loginResponseData.roomId;
        dissoliveRoomRequestVo.type = dissoliveRoomType;
        string sendMsg = JsonMapper.ToJson(dissoliveRoomRequestVo);
        CustomSocket.getInstance().sendMsg(new DissoliveRoomRequest(sendMsg));
        GlobalDataScript.isonApplayExitRoomstatus = true;
    }
}
