using AssemblyCSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LitJson;
using UnityEngine.UI;

public class HomeClubScript : MonoBehaviour
{
    public string cantCreate;
    public GameObject panelCreateClub;
    public GameObject panelJoinClub;
    public Button openClubbtn;
    public Button closeClubbtn;
    public Button createClubshowbtn;
    public Button joinClubshowbtn;
    public Button joinClubbtn;
    public Button joinClubBackbtn;
    public InputField joinClubID;
    public Button createClub;
    public Button createClubBack;
    public InputField createclubname;
    public Button quitClubshowbtn;
    public GameObject panelClubHome;
    public GameObject clubState;
    public GameObject createPanel;
    public GameObject joinPanel;
    public GameObject cretaejoinbuttonPanel;
    public Button showbtn;
    private bool isshowcretaejoinbuttonPanel= false;
    public Button menberBtn;
    public GridLayoutGroup menberListGrid;
    public GridLayoutGroup agreeListGrid;
    public GridLayoutGroup roomListGrid;
    public GameObject menberListobj;//全部成员
    public GameObject merbersub;
    public GameObject agreeListPanel;
    public GameObject agreesub;
    public GameObject clubroomSub;
    private bool ismerberlistshow = false;
    private bool isagreePanelShow = false;
    private bool isinClub = false;//是否在俱乐部中
    public Button showAgreeListbtn;
    public Text clubnametxt;
    public Text clubIDtxt;
    public Button createClubRoombtn;
    public Button reshBtn;
    public Text mycreditScoretxt;
    public Button quitClubBtn;

    public void Awake()
    {
        openClubbtn.onClick.AddListener(OpenClub);
        closeClubbtn.onClick.AddListener(CloseClub);
        createClubshowbtn.onClick.AddListener(ShowCreateClub);
        joinClubshowbtn.onClick.AddListener(ShowJoinClub);
        showbtn.onClick.AddListener(showbtnOnClick);
        menberBtn.onClick.AddListener(showmenberList);
        createClub.onClick.AddListener(createClubOnClick);
        createClubBack.onClick.AddListener(createClubBackOnClick);
        joinClubbtn.onClick.AddListener(joinClubbtnOnClick);
        joinClubBackbtn.onClick.AddListener(joinClubBackOnClick);
        showAgreeListbtn.onClick.AddListener(showagreeList);
        createClubRoombtn.onClick.AddListener(createclubRoomDialog);
        reshBtn.onClick.AddListener(reshOnClick);
        quitClubBtn.onClick.AddListener(quitClub);

    }

    public void Start()
    {

        SocketEventHandle.getInstance().CreateClubCallBack += OnCreateClubCallBack;
        SocketEventHandle.getInstance().JoinClubCallBack += OnJoinClubCallBack;
        SocketEventHandle.getInstance().GetApplyClubCallBack += OnGetApplyClubCallBack;
        SocketEventHandle.getInstance().GetAllClubCallBack += OnGetAllClubCallBack;
        SocketEventHandle.getInstance().AgreeClubCallBack += OnAgreeClubCallBack;
        SocketEventHandle.getInstance().ClubStateCallBack += OnClubStateCallBack;
        SocketEventHandle.getInstance().QuitClubCallBack += OnQuitClubCallBack;
        //SocketEventHandle.getInstance().JoinRoomCallBack += onJoinRoomCallBack;
    }

   



    private void createClubOnClick()
    {
        string club = createclubname.text;
        if (club == "")
        {
            TipsManagerScript.getInstance().setTips("请输入俱乐部名称");
        }
        else
        {
            ClientRequest cr = new ClientRequest();
            cr.headCode = APIS.CREATE_CLUB_REQUEST;
            cr.messageContent = "{'clubname':'" + club + "'}";
            CustomSocket.getInstance().sendMsg(cr);
        }
    }
    //加入俱乐部
    private void joinClubbtnOnClick()
    {
        string clubid = joinClubID.text;
        if (clubid == "")
        {
            TipsManagerScript.getInstance().setTips("请输入俱乐部ID");
        }
        else
        {
            ClientRequest cr = new ClientRequest();
            cr.headCode = APIS.JOIN_CLUB_REQUEST;
            cr.messageContent = "{'clubid':" + clubid + "}"; ;
            CustomSocket.getInstance().sendMsg(cr);
        }
    }
    private void createClubBackOnClick()
    {
        createPanel.SetActive(false);
        clubStateShow();

    }
    private void joinClubBackOnClick()
    {
        joinPanel.SetActive(false);
        clubStateShow();

    }
    //显示成员列表
    private void showmenberList()
    {
        if(ismerberlistshow)
        {
            ismerberlistshow = false;
            menberListobj.SetActive(false);
        }
        else
        {
            ClientRequest cr = new ClientRequest();
            cr.headCode = APIS.GETALL_CLUB_REQUEST;
            cr.messageContent = "";
            CustomSocket.getInstance().sendMsg(cr);
            
        }
        
        
    }
    //显示申请人
    private void showagreeList()
    {
        if (isagreePanelShow)
        {
            isagreePanelShow = false;
            agreeListPanel.SetActive(false);
        }
        else
        {
            ClientRequest cr = new ClientRequest();
            cr.headCode = APIS.GETAPPLY_CLUB_REQUEST;
            cr.messageContent = "";
            CustomSocket.getInstance().sendMsg(cr);

        }
    }

    private void quitClub()
    {
        ClientRequest cr = new ClientRequest();
        cr.headCode = APIS.QUIT_CLUB_REQUEST;
        cr.messageContent = "";
        CustomSocket.getInstance().sendMsg(cr);
    }
    private void showbtnOnClick()
    {
        if(isshowcretaejoinbuttonPanel)
        {
            isshowcretaejoinbuttonPanel = false;
            cretaejoinbuttonPanel.SetActive(isshowcretaejoinbuttonPanel);
        }
        else
        {
            isshowcretaejoinbuttonPanel = true;
            cretaejoinbuttonPanel.SetActive(isshowcretaejoinbuttonPanel);
        }
    }
    private void ShowCreateClub()
    {
        createPanel.SetActive(true);
        joinPanel.SetActive(false);
        isshowcretaejoinbuttonPanel = false;
        cretaejoinbuttonPanel.SetActive(false);
        clubState.SetActive(false);

    }
    private void ShowJoinClub()
    {
        joinPanel.SetActive(true);
        createPanel.SetActive(false);
        cretaejoinbuttonPanel.SetActive(false);
        isshowcretaejoinbuttonPanel = false;
        clubState.SetActive(false);
    }

    private void clubStateShow()
    {
        clubState.SetActive(isinClub);
    }
    private void OpenClub()
    {
        //获取俱乐部状态
        ClientRequest cr = new ClientRequest();
        cr.headCode = APIS.CLUBSTATE_REQUEST;
        cr.messageContent = "";
        CustomSocket.getInstance().sendMsg(cr);
        panelClubHome.SetActive(true);
        addListener();
        //mycreditScore.text = GlobalDataScript.loginResponseData.account.creditscore.ToString();
    }

    private void addListener()
    {
        SocketEventHandle.getInstance().JoinRoomCallBack += onJoinRoomCallBack;
        SocketEventHandle.getInstance().serviceErrorNotice += serviceResponse;
    }

    private void removeListener()
    {
        SocketEventHandle.getInstance().JoinRoomCallBack -= onJoinRoomCallBack;
        SocketEventHandle.getInstance().serviceErrorNotice -= serviceResponse;
    }
    public void serviceResponse(ClientResponse response)
    {
        //watingPanel.gameObject.SetActive(false);
        TipsManagerScript.getInstance().setTips(response.message);
    }
    private void CloseClub()
    {
        panelClubHome.SetActive(false);
        removeListener();
    }
    public void OnCreateClubCallBack(ClientResponse response)
    {
        CreateClubVo createClubVo = JsonMapper.ToObject<CreateClubVo>(response.message);
        // 如果服务器信息 msgcode 为0，则提示信息 msgstring
        if (createClubVo.msgcode == 0)
        {
            TipsManagerScript.getInstance().setTips("创建成功");
            ClientRequest cr = new ClientRequest();
            cr.headCode = APIS.CLUBSTATE_REQUEST;
            cr.messageContent = "";
            CustomSocket.getInstance().sendMsg(cr);
            createPanel.SetActive(false);
        }
        else
        {
            TipsManagerScript.getInstance().setTips(createClubVo.msgstring);
        }
               
    }
    public void OnJoinClubCallBack(ClientResponse response)
    {
        CreateClubVo createClubVo = JsonMapper.ToObject<CreateClubVo>(response.message);
        // 如果服务器信息 msgcode 为0，则提示信息 msgstring
        if (createClubVo.msgcode != 0)
        {
            TipsManagerScript.getInstance().setTips(createClubVo.msgstring);
        }
        else
        {
            TipsManagerScript.getInstance().setTips("申请成功");
        }

            
    }
    public void OnGetApplyClubCallBack(ClientResponse response)
    {
        ClubAccountVO vo = JsonMapper.ToObject<ClubAccountVO>(response.message);
        if (vo.msgcode != 0)
        {
            TipsManagerScript.getInstance().setTips(vo.msgstring);
        }
        else
        {
            isagreePanelShow = true;
            agreeListPanel.SetActive(true);
            PrefabUtils.ClearChild(agreeListGrid);
            foreach (ClubAccount ac in vo.listaccount)
            {
                GameObject child = PrefabUtils.AddChild(agreeListGrid, agreesub);
                agreeSubScript cs = child.GetComponent<agreeSubScript>();
                cs.setValue(ac);
                cs.agreebtn.onClick.AddListener(() =>
                {
                    ClientRequest cr = new ClientRequest();
                    cr.headCode = APIS.AGREE_CLUB_REQUEST;
                    cr.messageContent = "{'agree':1,'uuid':" + ac.uuid + "}";
                    CustomSocket.getInstance().sendMsg(cr);
                });
                cs.nobtn.onClick.AddListener(() =>
                {
                    ClientRequest cr = new ClientRequest();
                    cr.headCode = APIS.AGREE_CLUB_REQUEST;
                    cr.messageContent = "{'agree':0,'uuid':" + ac.uuid + "}";
                    CustomSocket.getInstance().sendMsg(cr);
                });

            }


        }
    }
    public void OnGetAllClubCallBack(ClientResponse response)
    {
        ClubAccountVO vo= JsonMapper.ToObject<ClubAccountVO>(response.message);
        if (vo.msgcode != 0)
        {
            TipsManagerScript.getInstance().setTips(vo.msgstring);
        }
        else
        {
            ismerberlistshow = true;
            menberListobj.SetActive(true);
            PrefabUtils.ClearChild(menberListGrid);
            foreach (ClubAccount ac in vo.listaccount)
            {
                GameObject child = PrefabUtils.AddChild(menberListGrid, merbersub);
                merberSubScript cs = child.GetComponent<merberSubScript>();
                cs.setValue(ac);

            }
            
            
        }

    }
    public void OnAgreeClubCallBack(ClientResponse response)
    {
        ClubMsgVO vo = JsonMapper.ToObject<ClubMsgVO>(response.message);
        if (vo.msgcode != 0)
        {
            TipsManagerScript.getInstance().setTips(vo.msgstring);
        }
        else
        {
            TipsManagerScript.getInstance().setTips("操作成功！");
            ClientRequest cr = new ClientRequest();
            cr.headCode = APIS.GETAPPLY_CLUB_REQUEST;
            cr.messageContent = "";
            CustomSocket.getInstance().sendMsg(cr);
        }

    }
    public void OnClubStateCallBack(ClientResponse response)
    {
        ClubStateVO vo= JsonMapper.ToObject<ClubStateVO>(response.message);
        if(vo.msgcode!=0)
        {
            isinClub = false;
            clubState.SetActive(false);
            GlobalDataScript.clubid = 0;
            clubnametxt.text = "";
            mycreditScoretxt.text = "";
            clubIDtxt.text ="";
        }
        else
        {
            isinClub = true;
            clubState.SetActive(true);
            clubnametxt.text = vo.clubname;
            GlobalDataScript.clubid = vo.clubid;
            GlobalDataScript.mycreditScore = vo.creditScore;
            mycreditScoretxt.text="当前信用分:"+ vo.creditScore;
            clubIDtxt.text ="俱乐部ID：" +vo.clubid.ToString();
            ClubStateScript cs = clubState.GetComponent<ClubStateScript>();
            cs.setValue(vo);
            addRoomList(vo.listroom);
        }
    }
    public void OnQuitClubCallBack(ClientResponse response)
    {
        ClubMsgVO vo = JsonMapper.ToObject<ClubMsgVO>(response.message);
        if (vo.msgcode != 0)
        {
            TipsManagerScript.getInstance().setTips(vo.msgstring);
        }
        else
        {
            TipsManagerScript.getInstance().setTips("退出成功！");
            ClientRequest cr = new ClientRequest();
            cr.headCode = APIS.CLUBSTATE_REQUEST;
            cr.messageContent = "";
            CustomSocket.getInstance().sendMsg(cr);
        }
    }
    public void addRoomList(List<ClubRoomVO> list)
    {
        PrefabUtils.ClearChild(roomListGrid);
        foreach (ClubRoomVO ac in list)
        {
            GameObject child = PrefabUtils.AddChild(roomListGrid, clubroomSub);
            clubRoomSubScript cs = child.GetComponent<clubRoomSubScript>();
            cs.setValue(ac);

        }
    }
    public void createclubRoomDialog()
    {
        SoundCtrl.getInstance().playSoundByActionButton(1);
        if (GlobalDataScript.roomVo == null || GlobalDataScript.roomVo.roomId == 0)
        {
            GlobalDataScript.goldType = false;
            loadPerfab("Prefab/Panel_CreateClubRomm_Dialog");

        }
        else
        {
            TipsManagerScript.getInstance().setTips("当前正在房间状态，无法加入新的房间");
        }
    }

    private void loadPerfab(string perfabName)
    {
        panelCreateClub = Instantiate(Resources.Load(perfabName)) as GameObject;
        panelCreateClub.transform.parent = gameObject.transform;
        panelCreateClub.transform.localScale = Vector3.one;
        panelCreateClub.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
        panelCreateClub.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
    }

    public void reshOnClick()
    {
        ClientRequest cr = new ClientRequest();
        cr.headCode = APIS.CLUBSTATE_REQUEST;
        cr.messageContent = "";
        CustomSocket.getInstance().sendMsg(cr);
       
    }

    public void onJoinRoomCallBack(ClientResponse response)
    {
        //watingPanel.gameObject.SetActive(false);
        MyDebug.Log(response);
        if (response.status == 1)
        {
            GlobalDataScript.roomJoinResponseData = JsonMapper.ToObject<RoomCreateVo>(response.message);

            GlobalDataScript.roomVo = GlobalDataScript.roomJoinResponseData;
            /*
			GlobalDataScript.roomVo.addWordCard = GlobalDataScript.roomJoinResponseData.addWordCard;
            GlobalDataScript.roomVo.hong = GlobalDataScript.roomJoinResponseData.hong;
            GlobalDataScript.roomVo.ma = GlobalDataScript.roomJoinResponseData.ma;
            GlobalDataScript.roomVo.name = GlobalDataScript.roomJoinResponseData.name;
            GlobalDataScript.roomVo.roomId = GlobalDataScript.roomJoinResponseData.roomId;
            GlobalDataScript.roomVo.roomType = GlobalDataScript.roomJoinResponseData.roomType;
			GlobalDataScript.roomVo.goldType = GlobalDataScript.roomJoinResponseData.goldType;
            GlobalDataScript.roomVo.roundNumber = GlobalDataScript.roomJoinResponseData.roundNumber;
            GlobalDataScript.roomVo.sevenDouble = GlobalDataScript.roomJoinResponseData.sevenDouble;
            GlobalDataScript.roomVo.xiaYu = GlobalDataScript.roomJoinResponseData.xiaYu;
            GlobalDataScript.roomVo.ziMo = GlobalDataScript.roomJoinResponseData.ziMo;
            GlobalDataScript.roomVo.gui = GlobalDataScript.roomJoinResponseData.gui;
            GlobalDataScript.roomVo.gangHu = GlobalDataScript.roomJoinResponseData.gangHu;
			GlobalDataScript.roomVo.gangHuQuanBao = GlobalDataScript.roomJoinResponseData.gangHuQuanBao;
			GlobalDataScript.roomVo.wuGuiX2 = GlobalDataScript.roomJoinResponseData.wuGuiX2;
            GlobalDataScript.roomVo.guiPai = GlobalDataScript.roomJoinResponseData.guiPai;
			GlobalDataScript.roomVo.shangxiaFanType = GlobalDataScript.roomJoinResponseData.shangxiaFanType;
			GlobalDataScript.roomVo.diFen = GlobalDataScript.roomJoinResponseData.diFen;
			GlobalDataScript.roomVo.tongZhuang = GlobalDataScript.roomJoinResponseData.tongZhuang;
			GlobalDataScript.roomVo.pingHu = GlobalDataScript.roomJoinResponseData.pingHu;
			GlobalDataScript.roomVo.keDianPao = GlobalDataScript.roomJoinResponseData.keDianPao;
			GlobalDataScript.roomVo.lunZhuang = GlobalDataScript.roomJoinResponseData.lunZhuang;
			GlobalDataScript.roomVo.gameType = GlobalDataScript.roomJoinResponseData.gameType;
			GlobalDataScript.roomVo.zhang16 = GlobalDataScript.roomJoinResponseData.zhang16;
			GlobalDataScript.roomVo.showPai = GlobalDataScript.roomJoinResponseData.showPai;
			GlobalDataScript.roomVo.xian3 = GlobalDataScript.roomJoinResponseData.xian3;
			GlobalDataScript.roomVo.qiang = GlobalDataScript.roomJoinResponseData.qiang;
			GlobalDataScript.roomVo.ming = GlobalDataScript.roomJoinResponseData.ming;
			GlobalDataScript.roomVo.mengs = GlobalDataScript.roomJoinResponseData.mengs;
			GlobalDataScript.roomVo.AA = GlobalDataScript.roomJoinResponseData.AA;
			GlobalDataScript.goldType = GlobalDataScript.roomJoinResponseData.goldType;
			*/


            GlobalDataScript.surplusTimes = GlobalDataScript.roomJoinResponseData.roundNumber;
            GlobalDataScript.loginResponseData.roomId = GlobalDataScript.roomJoinResponseData.roomId;

            GlobalDataScript.reEnterRoomData = null;

            GlobalDataScript.loadTime = GlobalDataScript.GetTimeStamp();

            if (GlobalDataScript.roomVo.gameType == 0)//(int)GameType.GameType_PK_PDK
                GlobalDataScript.gamePlayPanel = PrefabManage.loadPerfab("Prefab/Panel_GamePlay");
            else if (GlobalDataScript.roomVo.gameType == 1)//(int)GameType.GameType_PK_PDK
                GlobalDataScript.gamePlayPanel = PrefabManage.loadPerfab("Prefab/Panel_GamePDK");
            else if (GlobalDataScript.roomVo.gameType == 3 && GlobalDataScript.roomVo.playerAmounts == 10)//(int)GameType.GameType_PK_DN
            {
                GlobalDataScript.gamePlayPanel = PrefabManage.loadPerfab("Prefab/Panel_GameDN");
                //  SoundCtrl.getInstance().playBGM(3);
            }
            else if (GlobalDataScript.roomVo.gameType == 3 && GlobalDataScript.roomVo.playerAmounts == 6)
            {
                GlobalDataScript.gamePlayPanel = PrefabManage.loadPerfab("Prefab/Panel_GameDN_6");
                //  SoundCtrl.getInstance().playBGM(3);
            }
            else if (GlobalDataScript.roomVo.gameType == 4)
            {
                GlobalDataScript.gamePlayPanel = PrefabManage.loadPerfab("Prefab/Panel_GameDZPK");
            }
            //GlobalDataScript.gamePlayPanel = PrefabManage.loadPerfab(GlobalDataScript.playObject);
            //SocketEventHandle.getInstance().gameReadyNotice += GlobalDataScript.gamePlayPanel.GetComponent<MyMahjongScript>().gameReadyNotice;
            //GlobalDataScript.gamePlayPanel.GetComponent<MyMahjongScript>().joinToRoom(GlobalDataScript.roomJoinResponseData.playerList);

           
        }
        else
        {
            
            //TipsManagerScript.getInstance();
            //watingPanel.gameObject.SetActive(true);
            //watingPanel.gameObject.transform.gameObject.SetActive(true);
            //watingPanel.gameObject.transform.FindChild("tip3/Text").GetComponent<Text>().text = response.message;


            TipsManagerScript.getInstance().setTips(response.message);
            
            GlobalDataScript.homePanel.GetComponent<HomePanelScript>().openEnterRoomDialog();

        }

    }

}
