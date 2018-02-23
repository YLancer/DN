using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;
using System;
using LitJson;
using UnityEngine.SceneManagement;


public class CrateRoomSettingScript : MonoBehaviour
{

    public GameObject panelZhuanzhuanSetting;
    public GameObject panelChangshaSetting;
    public GameObject panelHuashuiSetting;
    public GameObject panelGuangDongSetting;
    public GameObject panelGanZhouSetting;
    public GameObject panelRuiJinSetting;
    public GameObject panelHongZhongSetting;
    public GameObject panelChaoShanSetting;
    public GameObject panelPdkSetting;
    public GameObject panelDevoloping;
    public GameObject panelDnSetting;
    public GameObject panelDzpkSetting;

    public GameObject Button_zhuanzhuan1;
    public GameObject Button_zhuanzhuan;
    public GameObject Button_huashui1;
    public GameObject Button_huashui;

    public GameObject Button_changsha1;
    public GameObject Button_changsha;

    public GameObject Btn_zhuanZ_liang;
    public GameObject Btn_zhuanZ_dark;
    public GameObject Btn_huaS_liang;
    public GameObject Btn_huaS_dark;
    public GameObject Btn_run_liang;
    public GameObject Btn_run_dark;
    public GameObject Btn_ganzhou_liang;
    public GameObject Btn_ganzhou_dark;
    public GameObject Btn_ruijin_liang;
    public GameObject Btn_ruijin_dark;
    public GameObject Btn_hongzhong_liang;
    public GameObject Btn_hongzhong_dark;
    public GameObject Btn_chaoshan_liang;
    public GameObject Btn_chaoshan_dark;
    public GameObject Btn_pdk_liang;
    public GameObject Btn_pdk_dark;
    public GameObject Btn_dn_liang;
    public GameObject Btn_dn_dark;
    public GameObject Btn_dzpk_liang;
    public GameObject Btn_dzpk_dark;

    public GameObject Btn_sangong_liang;
    public GameObject Btn_sangong_dark;
    public GameObject Btn_mushi_liang;
    public GameObject Btn_mushi_dark;
    public GameObject Btn_ddz_liang;
    public GameObject Btn_ddz_dark;

    // Add by Shise
    public GameObject PanelKpqzSetting;   // 看牌抢庄
    public GameObject PanelXjtzSetting;   // 闲家推注
    public GameObject PanelLldzSetting;   // 轮流当庄
    public GameObject PanelNnhzSetting;   // 牛牛换庄
    public GameObject PanelFzbwzSetting;  // 房主霸王庄
    public GameObject PanelZdpwzSetting;  // 最大牌为庄

    public GameObject Btn_kpqz_light;
    public GameObject Btn_kpqz_dark;
    // public GameObject Btn_xjtz_light;
    // public GameObject Btn_xjtz_dark;
    public GameObject Btn_lldz_light;
    public GameObject Btn_lldz_dark;
    public GameObject Btn_nnhz_light;
    public GameObject Btn_nnhz_dark;
    public GameObject Btn_fzbwz_light;
    public GameObject Btn_fzbwz_dark;
    public GameObject Btn_zdpwz_light;
    public GameObject Btn_zdpwz_dark;

    // End

    public Image watingPanel;

    public Image button_Create_Sure;


    public List<Toggle> zhuanzhuanRoomCards;//转转麻将房卡数
    public List<Toggle> changshaRoomCards;//长沙麻将房卡数
    public List<Toggle> huashuiRoomCards;//划水麻将房卡数
    public List<Toggle> guangdongRoomCards;//广东麻将房卡数
    public List<Toggle> ganzhouRoomCards;//赣州麻将房卡数
    public List<Toggle> ruijinRoomCards;//瑞金麻将房卡数

    public List<Toggle> xinchaoshanGameRule;//潮汕麻将规则


    public List<Toggle> zhuanzhuanGameRule;//转转麻将玩法
    public List<Toggle> changshaGameRule;//长沙麻将玩法
    public List<Toggle> huashuiGameRule;//划水麻将玩法
    public List<Toggle> guangdongGameRule;//广东麻将玩法
    public List<Toggle> ganzhouGameRule;//赣州麻将玩法
    public List<Toggle> ruijinGameRule;//瑞金麻将玩法
    public List<Toggle> pdkGameRule;//跑得快玩法
    public List<Toggle> dnGameRule;//斗牛玩法
    public List<Toggle> dzpkGameRule;//德州扑克玩法

    // Add by Shise
    public List<Toggle> kpqzGameRule;   // 看牌抢庄规则
    public List<Toggle> xjtzGameRule;   // 闲家推注规则
    public List<Toggle> nnhzGameRule;   // 牛牛换庄规则
    public List<Toggle> lldzGameRule;   // 轮流当庄规则
    public List<Toggle> fzbwzGameRule;  // 房主霸王庄规则
    public List<Toggle> zdpwzGameRule;  // 最大牌为庄规则
    // End

    public List<Toggle> zhuanzhuanZhuama;//转转麻将抓码个数
    public List<Toggle> changshaZhuama;//长沙麻将抓码个数
    public List<Toggle> huashuixiayu;//划水麻将下鱼条数
    public List<Toggle> guangdongZhuama;//广东麻将抓码个数

    public List<Toggle> guangdongGui;//广东麻将鬼牌

    private int roomCardCount;//房卡数
    private GameObject gameSence;
    private RoomCreateVo sendVo;//创建房间的信息

    public Button createRoom;


    public GameObject[] fangka;

    private string userDefaultSet = null;
    public bool isclubroom;

    void Start()
    {

        PlayerPrefs.DeleteAll();
        PlayerPrefs.DeleteKey("userDefaultGameType");
        PlayerPrefs.DeleteKey("userDefaultSet_ZhuangZhuang");
        PlayerPrefs.DeleteKey("userDefaultSet_HuaShui");
        PlayerPrefs.DeleteKey("userDefaultSet_ChangSha");
        PlayerPrefs.DeleteKey("userDefaultSet_GuangDong");
        PlayerPrefs.DeleteKey("userDefaultSet_GanZhou");
        PlayerPrefs.DeleteKey("userDefaultSet_RuiJin");
        PlayerPrefs.DeleteKey("userDefaultSet_DN");
        PlayerPrefs.DeleteKey("userDefaultSet_GanZhou");

        // Add by Shise
        PlayerPrefs.DeleteKey("userDefaultSet_KPQZ");
        PlayerPrefs.DeleteKey("userDefaultSet_XJTZ");
        PlayerPrefs.DeleteKey("userDefaultSet_LLDZ");
        PlayerPrefs.DeleteKey("userDefaultSet_NNHZ");
        PlayerPrefs.DeleteKey("userDefaultSet_FZBWZ");
        PlayerPrefs.DeleteKey("userDefaultSet_ZDPWZ");
        // End


        if (GlobalDataScript.configTemp.pay == 0)
        {
            foreach (GameObject go in fangka)
            {
                go.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject go in fangka)
            {
                go.SetActive(false);
            }
        }

        openDefaultSetingPanel();//打开默认房间设置

        SocketEventHandle.getInstance().CreateRoomCallBack += onCreateRoomCallback;
        if (SocketEventHandle.getInstance().serviceErrorNotice != null)
        {
            SocketEventHandle.getInstance().serviceErrorNotice = null;
        }
        SocketEventHandle.getInstance().serviceErrorNotice += serviceResponse;
    }




    public void serviceResponse(ClientResponse response)
    {
        watingPanel.gameObject.SetActive(false);
        TipsManagerScript.getInstance().setTips(response.message);
    }


    public void cancle()
    {
        SoundCtrl.getInstance().playSoundByActionButton(1);
        watingPanel.gameObject.SetActive(false);


    }
    /***
	 * 打开转转麻将设置面板
	 */
    public void openSetingPanel_ZhuangZhuang()
    {

        SoundCtrl.getInstance().playSoundByActionButton(1);
        //GlobalDataScript.userGameType = GameType.GameType_MJ_ZhuangZhuang;
        GlobalDataScript.userGameType = GameType.GameType_MJ_YiChun;
        loadDefaultSet(GlobalDataScript.userGameType);
        setGameObjectActive(GlobalDataScript.userGameType);

        createRoom.onClick.RemoveAllListeners();
        createRoom.onClick.AddListener(delegate ()
        {
            this.createZhuanzhuanRoom();
        });
    }

    /***
	 * 打开长沙麻将设置面板
	 */
    public void openSetingPanel_ChangSha()
    {
        SoundCtrl.getInstance().playSoundByActionButton(1);
        GlobalDataScript.userGameType = GameType.GameType_MJ_ChangSha;
        loadDefaultSet(GlobalDataScript.userGameType);
        setGameObjectActive(GlobalDataScript.userGameType);

        createRoom.onClick.RemoveAllListeners();
        createRoom.onClick.AddListener(delegate ()
        {
            this.createChangshaRoom();
        });
    }

    /***
	 * 打开划水麻将设置面板
	 */
    public void openSetingPanel_HuaShui()
    {
        SoundCtrl.getInstance().playSoundByActionButton(1);
        GlobalDataScript.userGameType = GameType.GameType_MJ_HuaShui;
        loadDefaultSet(GlobalDataScript.userGameType);
        setGameObjectActive(GlobalDataScript.userGameType);

        createRoom.onClick.RemoveAllListeners();
        createRoom.onClick.AddListener(delegate ()
        {
            this.createHuashuiRoom();
        });
    }

    /***
	 * 打开红中麻将设置面板
	 */
    public void openSetingPanel_HongZhong()
    {
        SoundCtrl.getInstance().playSoundByActionButton(1);
        GlobalDataScript.userGameType = GameType.GameType_MJ_HongZhong;
        loadDefaultSet(GlobalDataScript.userGameType);
        setGameObjectActive(GlobalDataScript.userGameType);

        createRoom.onClick.RemoveAllListeners();
        createRoom.onClick.AddListener(delegate ()
        {
            this.createHuashuiRoom();
        });
    }

    /***
	 * 打开广东麻将设置面板
	 */
    public void openSetingPanel_GuangDong()
    {

        SoundCtrl.getInstance().playSoundByActionButton(1);
        GlobalDataScript.userGameType = GameType.GameType_MJ_GuangDong;
        loadDefaultSet(GlobalDataScript.userGameType);
        setGameObjectActive(GlobalDataScript.userGameType);

        createRoom.onClick.RemoveAllListeners();
        createRoom.onClick.AddListener(delegate ()
        {
            this.createGuangDongRoom();
        });
    }

    /***
	 * 打开赣州麻将设置面板
	 */
    public void openSetingPanel_GanZhou()
    {
        //TipsManagerScript.getInstance ().setTips ("开发中");
        SoundCtrl.getInstance().playSoundByActionButton(1);
        //return;

        GlobalDataScript.userGameType = GameType.GameType_MJ_GanZhou;
        loadDefaultSet(GlobalDataScript.userGameType);
        setGameObjectActive(GlobalDataScript.userGameType);

        createRoom.onClick.RemoveAllListeners();
        createRoom.onClick.AddListener(delegate ()
        {
            this.createGanZhouRoom();
        });
    }

    /***
	 * 打开瑞金麻将设置面板
	 */
    public void openSetingPanel_ChaoShan()
    {
        SoundCtrl.getInstance().playSoundByActionButton(1);
        GlobalDataScript.userGameType = GameType.GameType_MJ_ChaoShan;
        setGameObjectActive(GlobalDataScript.userGameType);
        loadDefaultSet(GlobalDataScript.userGameType);
        createRoom.onClick.RemoveAllListeners();
        createRoom.onClick.AddListener(delegate ()
        {
            this.createxinChaoShanRoom();
        });
    }



    /***
	 * 打开瑞金麻将设置面板
	 */
    public void openSetingPanel_RuiJin()
    {
        SoundCtrl.getInstance().playSoundByActionButton(1);
        GlobalDataScript.userGameType = GameType.GameType_MJ_RuiJin;
        setGameObjectActive(GlobalDataScript.userGameType);
        loadDefaultSet(GlobalDataScript.userGameType);
        createRoom.onClick.RemoveAllListeners();
        createRoom.onClick.AddListener(delegate ()
        {
            this.createRuiJinRoom();
        });
    }


    /***
	 * 打开跑得快设置面板
	 */
    public void openSetingPanel_PDK()
    {
        /**
		TipsManagerScript.getInstance ().setTips ("开发中");
		SoundCtrl.getInstance().playSoundByActionButton(1);
		return;
		*/

        SoundCtrl.getInstance().playSoundByActionButton(1);
        GlobalDataScript.userGameType = GameType.GameType_PK_PDK;
        loadDefaultSet(GlobalDataScript.userGameType);
        setGameObjectActive(GlobalDataScript.userGameType);

        createRoom.onClick.RemoveAllListeners();
        createRoom.onClick.AddListener(delegate ()
        {
            this.createPDKRoom();
        });
    }

    /***
	 * 打开斗牛设置面板
	 */
    public void openSetingPanel_DN()
    {
        SoundCtrl.getInstance().playSoundByActionButton(1);
        GlobalDataScript.userGameType = GameType.GameType_PK_DN;
        loadDefaultSet(GlobalDataScript.userGameType);
        setGameObjectActive(GlobalDataScript.userGameType);

        createRoom.onClick.RemoveAllListeners();
        createRoom.onClick.AddListener(delegate ()
        {
            this.createDNRoom();
        });
    }

    /**
	 * 打开德州扑克设置面板
	 */
    public void openSetingPanel_DZPK()
    {
        SoundCtrl.getInstance().playSoundByActionButton(1);
        GlobalDataScript.userGameType = GameType.GameType_PK_DZPK;
        loadDefaultSet(GlobalDataScript.userGameType);
        setGameObjectActive(GlobalDataScript.userGameType);

        createRoom.onClick.RemoveAllListeners();
        createRoom.onClick.AddListener(delegate ()
        {
            this.createDZPKRoom();
        });
    }

    // Add by Shise
    public void OpenSettingPanel_KPQZ()
    {
        SoundCtrl.getInstance().playSoundByActionButton(1);
        GlobalDataScript.userGameType = GameType.GameType_PK_KPQZ;
        loadDefaultSet(GlobalDataScript.userGameType);
        setGameObjectActive(GlobalDataScript.userGameType);

        createRoom.onClick.RemoveAllListeners();
        createRoom.onClick.AddListener(delegate () { this.CreateKPQZRoom(); });
    }

    public void OpenSettingPanel_XJTZ()
    {
        SoundCtrl.getInstance().playSoundByActionButton(1);
        GlobalDataScript.userGameType = GameType.GameType_PK_XJTZ;
        loadDefaultSet(GlobalDataScript.userGameType);
        setGameObjectActive(GlobalDataScript.userGameType);

        createRoom.onClick.RemoveAllListeners();
        createRoom.onClick.AddListener(delegate () { this.CreateXJTZRoom(); });
    }

    public void OpenSettingPanel_LLDZ()
    {
        SoundCtrl.getInstance().playSoundByActionButton(1);
        GlobalDataScript.userGameType = GameType.GameType_PK_LLDZ;
        loadDefaultSet(GlobalDataScript.userGameType);
        setGameObjectActive(GlobalDataScript.userGameType);

        createRoom.onClick.RemoveAllListeners();
        createRoom.onClick.AddListener(delegate () { this.CreateLLDZRoom(); });
    }

    public void OpenSettingPanel_NNHZ()
    {
        SoundCtrl.getInstance().playSoundByActionButton(1);
        GlobalDataScript.userGameType = GameType.GameType_PK_NNHZ;
        loadDefaultSet(GlobalDataScript.userGameType);
        setGameObjectActive(GlobalDataScript.userGameType);

        createRoom.onClick.RemoveAllListeners();
        createRoom.onClick.AddListener(delegate () { this.CreateNNHZRoom(); });
    }

    public void OpenSettingPanel_FZBWZ()
    {
        SoundCtrl.getInstance().playSoundByActionButton(1);
        GlobalDataScript.userGameType = GameType.GameType_PK_FZBWZ;
        loadDefaultSet(GlobalDataScript.userGameType);
        setGameObjectActive(GlobalDataScript.userGameType);

        createRoom.onClick.RemoveAllListeners();
        createRoom.onClick.AddListener(delegate () { this.CreateFZBWZRoom(); });
    }

    public void OpenSettingPanel_ZDPWZ()
    {
        SoundCtrl.getInstance().playSoundByActionButton(1);
        GlobalDataScript.userGameType = GameType.GameType_PK_ZDPWZ;
        loadDefaultSet(GlobalDataScript.userGameType);
        setGameObjectActive(GlobalDataScript.userGameType);

        createRoom.onClick.RemoveAllListeners();
        createRoom.onClick.AddListener(delegate () { this.CreateZDPWZRoom(); });
    }

    // End

    public void Button_down()
    {
        SoundCtrl.getInstance().playSoundByActionButton(1);
        Application.OpenURL("http://a.app.qq.com/o/simple.jsp?pkgname=com.pengyoupdk.poker");

    }

    public void openDeveloping()
    {
        SoundCtrl.getInstance().playSoundByActionButton(1);
        GlobalDataScript.userGameType = GameType.GameType_MJ_Developing;
        loadDefaultSet(GlobalDataScript.userGameType);
        setGameObjectActive(GlobalDataScript.userGameType);
    }

    public void closeDialog()
    {

        SoundCtrl.getInstance().playSoundByActionButton(1);
        MyDebug.Log("closeDialog");
        SocketEventHandle.getInstance().CreateRoomCallBack -= onCreateRoomCallback;
        SocketEventHandle.getInstance().serviceErrorNotice -= serviceResponse;
        Destroy(this);
        Destroy(gameObject);
    }

    private void ReqForCreateRoom(string msg)
    {
        int gold = 10;
        if (GlobalDataScript.userGameType == GameType.GameType_PK_DN)
        {
            gold = 5;
        }
        if (GlobalDataScript.goldType)
        {
            if (GlobalDataScript.loginResponseData.account.gold >= gold)
            {
                watingPanel.gameObject.SetActive(true);
                CustomSocket.getInstance().sendMsg(new CreateRoomRequest(msg));
                userDefaultSet = msg;
            }
            else
            {
                TipsManagerScript.getInstance().setTips("你的金币不足，不能匹配到训练场");
            }

        }
        else
        {
            if (GlobalDataScript.loginResponseData.account.roomcard > 0)
            {
                watingPanel.gameObject.SetActive(true);
                CustomSocket.getInstance().sendMsg(new CreateRoomRequest(msg));
                userDefaultSet = msg;
            }
            else
            {
                TipsManagerScript.getInstance().setTips("你的房卡数量不足，不能创建房间");
            }
        }
    }

    /**
	 * 创建转转麻将房间
	 */
    public void createZhuanzhuanRoom()
    {
        SoundCtrl.getInstance().playSoundByActionButton(1);
        int roundNumber = 4;//房卡数量
                            //		bool isZimo=false;//自摸
                            //		bool hasHong=false;//红中赖子
                            //		bool isSevenDoube =false;//七小对
                            //bool isGang = false;
                            //		int maCount = 0;
        for (int i = 0; i < zhuanzhuanRoomCards.Count; i++)
        {
            Toggle item = zhuanzhuanRoomCards[i];
            if (item.isOn)
            {
                if (i == 0)
                {
                    roundNumber = 8;
                }
                else if (i == 1)
                {
                    roundNumber = 16;
                }
                break;
            }
        }

        //		if (zhuanzhuanGameRule [0].isOn) {
        //			isZimo = true;
        //		}

        //if (zhuanzhuanGameRule [1].isOn) {
        //	isGang = true;
        //}

        //		if (zhuanzhuanGameRule [2].isOn) {
        //			hasHong = true;
        //		}

        //		if (zhuanzhuanGameRule [3].isOn) {
        //			isSevenDoube = true;
        //		}


        //		for (int i = 0; i < zhuanzhuanZhuama.Count; i++) {
        //			if (zhuanzhuanZhuama [i].isOn) {
        //				maCount = 2 * (i + 1);
        //				break;
        //			}
        //		}
        sendVo = new RoomCreateVo();
        //sendVo.ma = maCount;
        sendVo.roundNumber = roundNumber;
        //		sendVo.ziMo = isZimo?1:0;
        //		sendVo.hong = hasHong;
        //		sendVo.sevenDouble = isSevenDoube;
        //sendVo.roomType = (int)GameType.GameType_MJ_ZhuangZhuang;

        sendVo.roomType = (int)GameType.GameType_MJ_YiChun;

        string sendmsgstr = JsonMapper.ToJson(sendVo);
        ReqForCreateRoom(sendmsgstr);
    }

    /**
	 * 创建广东麻将房间
	 */
    public void createGuangDongRoom()
    {
        SoundCtrl.getInstance().playSoundByActionButton(1);
        int roundNumber = 8;//房卡数量
        bool isGangHu = false;//自摸
        bool hasHong = false;//红中赖子
        bool isSevenDoube = false;//七小对
        bool isFengpai = false;//风牌
        int gui = 0; //鬼牌
        bool gangHuQuanBao = false;
        bool wuGuiX2 = false;

        int maCount = 0;
        bool maGenDiFen = false;
        bool maGenGang = false;
        for (int i = 0; i < guangdongRoomCards.Count; i++)
        {
            Toggle item = guangdongRoomCards[i];
            if (item.isOn)
            {
                if (i == 0)
                {
                    roundNumber = 8;
                }
                else if (i == 1)
                {
                    roundNumber = 16;
                }
                break;
            }
        }

        if (guangdongGameRule[0].isOn)
        {
            isSevenDoube = true;
        }

        if (guangdongGameRule[1].isOn)
        {
            isFengpai = true;
        }

        if (guangdongGameRule[2].isOn)
        {
            isGangHu = true;
        }

        if (guangdongGameRule[3].isOn)
        {
            gangHuQuanBao = true;
        }

        if (guangdongGameRule[4].isOn)
        {
            wuGuiX2 = true;
        }


        //        for (int i = 0; i < guangdongZhuama.Count; i++)
        //        {
        //            if (guangdongZhuama[i].isOn)
        //            {
        //                maCount = 2 * (i + 1);
        //                break;
        //            }
        //        }

        if (guangdongZhuama[0].isOn)
        {
            maCount = 2;
        }
        else if (guangdongZhuama[1].isOn)
        {
            maCount = 4;
        }
        else if (guangdongZhuama[2].isOn)
        {
            maCount = 6;
        }
        else if (guangdongZhuama[5].isOn)
        {
            maCount = 8;
        }


        if (maCount > 0)
        {
            if (guangdongZhuama[3].isOn)
                maGenDiFen = true;
            if (guangdongZhuama[4].isOn)
                maGenGang = true;
        }

        if (guangdongGui[0].isOn)
        {
            gui = 0;
        }
        else if (guangdongGui[1].isOn)
        {
            gui = 1;
        }
        else
        {
            if (guangdongGui[3].isOn)
                gui = 3; //双鬼
            else
                gui = 2;
        }

        sendVo = new RoomCreateVo();
        sendVo.ma = maCount;
        sendVo.roundNumber = roundNumber;
        sendVo.ziMo = 1;
        sendVo.hong = hasHong;
        sendVo.addWordCard = isFengpai;
        sendVo.sevenDouble = isSevenDoube;
        sendVo.gui = gui;
        sendVo.gangHu = isGangHu;
        sendVo.gangHuQuanBao = gangHuQuanBao;
        sendVo.wuGuiX2 = wuGuiX2;
        sendVo.maGenDifen = maGenDiFen;
        sendVo.maGenGang = maGenGang;

        sendVo.roomType = (int)GameType.GameType_MJ_GuangDong;
        sendVo.goldType = GlobalDataScript.goldType;
        string sendmsgstr = JsonMapper.ToJson(sendVo);
        ReqForCreateRoom(sendmsgstr);
    }

    /**
	 * 创建赣州麻将房间
	 */
    public void createGanZhouRoom()
    {
        SoundCtrl.getInstance().playSoundByActionButton(1);

        sendVo = new RoomCreateVo();
        if (ganzhouRoomCards[0].isOn)
        {
            sendVo.roundNumber = 8;
        }
        else
        {
            sendVo.roundNumber = 16;
        }

        if (ganzhouGameRule[0].isOn)
        {
            sendVo.shangxiaFanType = 1;
        }
        else
        {
            sendVo.shangxiaFanType = 2;
        }

        if (ganzhouGameRule[2].isOn)
        {
            sendVo.diFen = 1;
        }
        else
        {
            sendVo.diFen = 2;
        }

        if (ganzhouGameRule[4].isOn)
        {
            sendVo.tongZhuang = true;
        }
        else
        {
            sendVo.tongZhuang = false;
        }

        if (ganzhouGameRule[6].isOn)
        {
            sendVo.pingHu = 1;
        }
        else if (ganzhouGameRule[7].isOn)
        {
            sendVo.pingHu = 2;
        }
        else
        {
            sendVo.pingHu = 3;
        }


        sendVo.roomType = (int)GameType.GameType_MJ_GanZhou;
        string sendmsgstr = JsonMapper.ToJson(sendVo);
        ReqForCreateRoom(sendmsgstr);
    }

    /**
	 * 创建xin潮汕麻将房间
	 */
    public void createxinChaoShanRoom()
    {
        SoundCtrl.getInstance().playSoundByActionButton(1);
        int gui = 0; //鬼牌
        int maCount = 0;
        sendVo = new RoomCreateVo();         //设置局数  24局 8局 16局


        if (xinchaoshanGameRule[0].isOn)
        {
            sendVo.roundNumber = 8;
        }
        else if (xinchaoshanGameRule[1].isOn)
        {
            sendVo.roundNumber = 16;
        }
        else if (xinchaoshanGameRule[2].isOn)
        {
            sendVo.roundNumber = 24;
        }


        if (xinchaoshanGameRule[3].isOn)        //玩法设置  
        {
            sendVo.gangHu = true;
        }

        if (xinchaoshanGameRule[4].isOn)
        {
            sendVo.genZhuang = true;
        }
        if (xinchaoshanGameRule[5].isOn)
        {
            sendVo.chongZhuang = true;
        }

        if (xinchaoshanGameRule[6].isOn)
        {
            sendVo.pengpengHu = true;
        }

        if (xinchaoshanGameRule[7].isOn)
        {
            sendVo.qiDui = true;
        }

        if (xinchaoshanGameRule[8].isOn)
        {
            sendVo.qiangGangHu = true;
        }

        if (xinchaoshanGameRule[9].isOn)
        {
            sendVo.hunYiSe = true;
        }

        if (xinchaoshanGameRule[10].isOn)
        {
            sendVo.qingYiSe = true;
        }
        if (xinchaoshanGameRule[11].isOn)
        {
            sendVo.gangShangKaiHua = true;
        }

        if (xinchaoshanGameRule[12].isOn)
        {
            sendVo.haoHuaQiDui = true;
        }

        if (xinchaoshanGameRule[13].isOn)
        {
            sendVo.shiSanYao = true;
        }
        if (xinchaoshanGameRule[14].isOn)
        {
            sendVo.tianDiHu = true;
        }
        if (xinchaoshanGameRule[15].isOn)
        {
            sendVo.shuangHaoHua = true;
        }
        if (xinchaoshanGameRule[16].isOn)
        {
            sendVo.sanHaoHua = true;
        }
        if (xinchaoshanGameRule[17].isOn)
        {
            sendVo.shiBaLuoHan = true;
        }
        if (xinchaoshanGameRule[18].isOn)
        {
            sendVo.xiaoSanYuan = true;
        }
        if (xinchaoshanGameRule[19].isOn)
        {
            sendVo.xiaoSiXi = true;
        }
        if (xinchaoshanGameRule[20].isOn)
        {
            sendVo.daSanYuan = true;
        }
        if (xinchaoshanGameRule[21].isOn)
        {
            sendVo.daSiXi = true;
        }
        if (xinchaoshanGameRule[22].isOn)
        {
            sendVo.huaYaoJiu = true;
        }



        if (xinchaoshanGameRule[23].isOn)
        {
            sendVo.fengDing = 5;
        }
        else if (xinchaoshanGameRule[24].isOn)
        {
            sendVo.fengDing = 10;
        }
        else
        {
            sendVo.fengDing = 99999;
        }

        if (xinchaoshanGameRule[31].isOn)
            maCount = 2;
        else if (xinchaoshanGameRule[32].isOn)
            maCount = 4;
        else if (xinchaoshanGameRule[33].isOn)
            maCount = 6;
        else if (xinchaoshanGameRule[30].isOn)
            maCount = 0;

        //if (xinchaoshanZhuama[1].isOn)
        //  sendVo.zhuaMa = true;

        sendVo.ma = maCount;

        if (maCount > 0)
        {
            if (xinchaoshanGameRule[34].isOn)
                sendVo.maGenDifen = true;
            if (xinchaoshanGameRule[35].isOn)
                sendVo.maGenGang = true;
        }

        if (xinchaoshanGameRule[26].isOn)
        {
            gui = 0;
        }
        else if (xinchaoshanGameRule[27].isOn)
        {
            gui = 1;
        }
        else
        {
            if (xinchaoshanGameRule[29].isOn)
                gui = 3; //双鬼
            else
                gui = 2;
        }
        sendVo.gui = gui;

        if (xinchaoshanGameRule[36].isOn)
        {
            sendVo.AA = false;
        }
        else
        {
            sendVo.AA = true;
        }

        sendVo.roomType = 10;
        string sendmsgstr = JsonMapper.ToJson(sendVo);
        if (GlobalDataScript.loginResponseData.account.roomcard > 0)
        {
            watingPanel.gameObject.SetActive(true);

            CustomSocket.getInstance().sendMsg(new CreateRoomRequest(sendmsgstr));
            userDefaultSet = sendmsgstr;
        }
        else
        {
            TipsManagerScript.getInstance().setTips("你的房卡数量不足，不能创建房间");
        }
    }


    /**
	 * 创建瑞金麻将房间
	 */
    public void createRuiJinRoom()
    {
        SoundCtrl.getInstance().playSoundByActionButton(1);

        sendVo = new RoomCreateVo();
        if (ruijinRoomCards[0].isOn)
        {
            sendVo.roundNumber = 4;
        }
        else if (ruijinRoomCards[1].isOn)
        {
            sendVo.roundNumber = 8;
        }
        else
        {
            sendVo.roundNumber = 16;
        }

        if (ruijinGameRule[0].isOn)
        {
            sendVo.keDianPao = true;
        }
        else
        {
            sendVo.keDianPao = false;
        }

        if (ruijinGameRule[1].isOn)
        {
            sendVo.diFen = 1;
        }
        else if (ruijinGameRule[2].isOn)
        {
            sendVo.diFen = 2;
        }
        else
        {
            sendVo.diFen = 5;
        }

        if (ruijinGameRule[4].isOn)
        {
            sendVo.tongZhuang = true;
        }
        else
        {
            sendVo.tongZhuang = false;
        }

        if (ruijinGameRule[6].isOn)
        {
            sendVo.lunZhuang = true;
        }
        else
        {
            sendVo.lunZhuang = false;
        }

        sendVo.roomType = (int)GameType.GameType_MJ_RuiJin;
        string sendmsgstr = JsonMapper.ToJson(sendVo);
        ReqForCreateRoom(sendmsgstr);
    }

    /**
	 * 创建跑得快房间
	 */
    public void createPDKRoom()
    {
        sendVo = new RoomCreateVo();

        if (pdkGameRule[0].isOn)
            sendVo.roundNumber = 10;
        else
            sendVo.roundNumber = 20;

        if (pdkGameRule[2].isOn)
            sendVo.zhang16 = true;
        else
            sendVo.zhang16 = false;

        if (pdkGameRule[4].isOn)
            sendVo.showPai = true;
        else
            sendVo.showPai = false;

        if (pdkGameRule[6].isOn)
            sendVo.xian3 = true;
        else
            sendVo.xian3 = false;

        sendVo.gameType = 1;  //1   (int)GameType.GameType_PK_PDK
        string sendmsgstr = JsonMapper.ToJson(sendVo);
        ReqForCreateRoom(sendmsgstr);
    }

    /**
	 * 创建斗牛房间
	 */
    public void createDNRoom()
    {
        sendVo = new RoomCreateVo();
        sendVo.roundNumber = dnGameRule[0].isOn ? 10 : 20;
        sendVo.qiang = dnGameRule[3].isOn;
        if (dnGameRule[4].isOn)
            sendVo.diFen = 1;
        else if (dnGameRule[5].isOn)
            sendVo.diFen = 2;
        else if (dnGameRule[6].isOn)
            sendVo.diFen = 3;
        sendVo.ming = dnGameRule[7].isOn;
        sendVo.mengs = dnGameRule[9].isOn ? 1 : 2;
        sendVo.AA = dnGameRule[11].isOn;
        sendVo.niu7fan = dnGameRule[13].isOn;

        sendVo.cuoPai = dnGameRule[15].isOn;
        if (GlobalDataScript.IsClub == false)
        {
            sendVo.clubid = 0;
        }

        sendVo.gameType = 3;//斗牛  3  (int)GameType.GameType_PK_DN

        string sendmsgstr = JsonMapper.ToJson(sendVo);

        ReqForCreateRoom(sendmsgstr);
    }

    // Add by Shise
    public void CreateKPQZRoom()
    {
        sendVo = new RoomCreateVo();
        if (isclubroom)
        {
            int clubid = GlobalDataScript.clubid;
            if (clubid == 0)
            {
                TipsManagerScript.getInstance().setTips("你不在俱乐部中无法创建俱乐部房间");
                return;
            }
            sendVo.clubid = clubid;
        }

        // 抢庄
        sendVo.qiang = true;
        // 看牌抢庄
        sendVo.ruleType = 1;
        //mark
        sendVo.diFen = 1;
        sendVo.ming = true;
        sendVo.mengs = 1;

        sendVo.qiangForNiu = -1;
        sendVo.zhuForNiu = 0;
        // 人数
        sendVo.playerAmounts = kpqzGameRule[0].isOn ? 6 : 10;
        // 局数
        sendVo.roundNumber = kpqzGameRule[2].isOn ? 10 : 20;
        // 支付类型
        sendVo.AA = kpqzGameRule[4].isOn;
        // 游戏模式
        // 三元表达式嵌套，表示toggle单选最终值   1 普通模式1  2 普通模式2  3 扫雷模式
        sendVo.rules = kpqzGameRule[6].isOn ? 1 : (kpqzGameRule[7].isOn ? 2 : 3);
        // 抢庄基本倍率
        sendVo.hogMultiples = kpqzGameRule[9].isOn ? 1 : (kpqzGameRule[10].isOn ? 2 : (kpqzGameRule[11].isOn ? 3 : 4));
        // 抢庄详细倍率
        sendVo.multiplyingPower = kpqzGameRule[13].isOn ? 1 : 0;

        // 搓牌
        sendVo.cuoPai = kpqzGameRule[15].isOn;
        //// 特殊牌型
        //sendVo.special = kpqzGameRule[16].isOn;
        // 牛7翻倍
        sendVo.niu7fan = kpqzGameRule[16].isOn;
        // 托管
        sendVo.trusteeship = kpqzGameRule[14].isOn;

        // 使用斗牛的Panel
        sendVo.gameType = 3;
        if (this.name == "Panel_CreateClubRomm_Dialog(Clone)")
        {
            sendVo.creditScoreIsOn = kpqzGameRule[17].isOn;
            sendVo.creditScoreMultiply = int.Parse(kpqzGameRule[17].gameObject.GetComponentInChildren<Text>().text.Split(' ')[1]);
        }

        //俱乐部房间测试用
        //sendVo.clubid = 100023;
        //sendVo.creditScoreMultiply = 1;
        //sendVo.creditScore = kpqzGameRule[17].GetComponentInChildren<Text>().text;
        //print(sendVo.creditScore);
        string sendMagStr = JsonMapper.ToJson(sendVo);
        ReqForCreateRoom(sendMagStr);
    }

    public void CreateXJTZRoom()
    {
        sendVo = new RoomCreateVo();
        if (isclubroom)
        {
            int clubid = GlobalDataScript.clubid;
            if (clubid == 0)
            {
                TipsManagerScript.getInstance().setTips("你不在俱乐部中无法创建俱乐部房间");
                return;
            }
            sendVo.clubid = clubid;
        }
        sendVo.ruleType = 2;
        sendVo.diFen = 1;
        sendVo.ming = true;
        sendVo.mengs = 1;

        sendVo.qiangForNiu = -1;
        sendVo.zhuForNiu = 0;
        // 人数
        sendVo.playerAmounts = xjtzGameRule[0].isOn ? 6 : 10;
        // 局数
        sendVo.roundNumber = xjtzGameRule[2].isOn ? 10 : 20;
        // 支付类型
        sendVo.AA = xjtzGameRule[4].isOn;
        // 游戏模式   1：明牌模式  2：牛牛上庄
        sendVo.rules = xjtzGameRule[6].isOn ? 1 : 2;
        if (sendVo.rules == 1) sendVo.ming = true;
        if (sendVo.rules == 2) sendVo.niuNiuShangZhuang = true;
        // 抢庄基本倍率   三元表达式嵌套，表示toggle单选最终值
        sendVo.hogMultiples = xjtzGameRule[8].isOn ? 1 : (xjtzGameRule[9].isOn ? 2 : (xjtzGameRule[10].isOn ? 3 : 4));
        // 抢庄详细倍率   三元表达式嵌套，表示toggle单选最终值   1：5  2：10  3：20
        sendVo.multiplyingPower = xjtzGameRule[12].isOn ? 1 : (xjtzGameRule[13].isOn ? 2 : 3);

        // 搓牌
        sendVo.cuoPai = xjtzGameRule[15].isOn;
        //// 特殊牌型
        //sendVo.special = xjtzGameRule[16].isOn;
        // 牛7翻倍
        sendVo.niu7fan = xjtzGameRule[16].isOn;
        // 托管
        sendVo.trusteeship = xjtzGameRule[17].isOn;
        // 抢庄
        sendVo.qiang = true;
        // 底分    三元表达式嵌套，表示toggle单选最终值   1:1/2  2:2/4  3：4/8   4：5/10
        sendVo.diFen = xjtzGameRule[18].isOn ? 1 : xjtzGameRule[19].isOn ? 2 : xjtzGameRule[20].isOn ? 3 : 4;

        // 使用斗牛的Panel    
        sendVo.gameType = 3;

        string sendMagStr = JsonMapper.ToJson(sendVo);
        ReqForCreateRoom(sendMagStr);
    }

    public void CreateNNHZRoom()
    {
        sendVo = new RoomCreateVo();
        if (isclubroom)
        {
            int clubid = GlobalDataScript.clubid;
            if (clubid == 0)
            {
                TipsManagerScript.getInstance().setTips("你不在俱乐部中无法创建俱乐部房间");
                return;
            }
            sendVo.clubid = clubid;
        }
        sendVo.ruleType = 3;
        //mark
        sendVo.ming = true;
        sendVo.mengs = 1;
        sendVo.zhuForNiu = 0;

        sendVo.qiangForNiu = -1;
        sendVo.diFen = 1;
        // 人数
        sendVo.playerAmounts = nnhzGameRule[0].isOn ? 6 : 10;
        // 局数
        sendVo.roundNumber = nnhzGameRule[2].isOn ? 10 : 20;
        // 支付类型
        sendVo.AA = nnhzGameRule[4].isOn;
        // 游戏模式   1：普通模式   2：扫雷模式
        sendVo.rules = nnhzGameRule[6].isOn ? 1 : 2;
        // 抢庄基本倍率
        sendVo.hogMultiples = 1;
        // 抢庄详细倍率   1：小倍   2：中倍   3：大倍
        sendVo.multiplyingPower = nnhzGameRule[9].isOn ? 1 : nnhzGameRule[10].isOn ? 2 : 3;

        // 搓牌
        sendVo.cuoPai = nnhzGameRule[12].isOn;
        //// 特殊牌型
        //sendVo.special = nnhzGameRule[13].isOn;
        // 牛7翻倍
        sendVo.niu7fan = nnhzGameRule[13].isOn;
        // 托管
        sendVo.trusteeship = nnhzGameRule[14].isOn;
        // 牛牛换庄
        sendVo.niuNiuHuanZhuang = true;

        // 使用斗牛的Panel        
        sendVo.gameType = 3;

        if (this.name == "Panel_CreateClubRomm_Dialog(Clone)")
        {
            sendVo.creditScoreIsOn = nnhzGameRule[15].isOn;
            sendVo.creditScoreMultiply = int.Parse(nnhzGameRule[15].gameObject.GetComponentInChildren<Text>().text.Split(' ')[1]);
        }

        string sendMagStr = JsonMapper.ToJson(sendVo);
        ReqForCreateRoom(sendMagStr);
    }

    public void CreateLLDZRoom()
    {
        sendVo = new RoomCreateVo();
        if (isclubroom)
        {
            int clubid = GlobalDataScript.clubid;
            if (clubid == 0)
            {
                TipsManagerScript.getInstance().setTips("你不在俱乐部中无法创建俱乐部房间");
                return;
            }
            sendVo.clubid = clubid;
        }
        sendVo.ruleType = 4;
        sendVo.ming = true;
        sendVo.mengs = 1;
        sendVo.zhuForNiu = 0;

        sendVo.qiangForNiu = -1;
        sendVo.diFen = 1;
        // 人数
        sendVo.playerAmounts = lldzGameRule[0].isOn ? 6 : 10;
        // 局数
        sendVo.roundNumber = lldzGameRule[2].isOn ? 10 : 20;
        // 支付类型
        sendVo.AA = lldzGameRule[4].isOn;
        // 游戏模式
        // 三元表达式嵌套，表示toggle单选最终值   1：明牌下注   2：暗牌下注   3：扫雷模式
        sendVo.rules = lldzGameRule[6].isOn ? 1 : (lldzGameRule[7].isOn ? 2 : 3);
        if (sendVo.rules == 1) sendVo.ming = true;
        else if (sendVo.rules == 2) sendVo.ming = false;
        // 抢庄基本倍率
        sendVo.hogMultiples = 1;
        // 抢庄详细倍率
        sendVo.multiplyingPower = lldzGameRule[10].isOn ? 1 : lldzGameRule[11].isOn ? 2 : 3;

        // 搓牌
        sendVo.cuoPai = lldzGameRule[13].isOn;
        //// 特殊牌型
        //sendVo.special = lldzGameRule[14].isOn;
        // 牛7翻倍
        sendVo.niu7fan = lldzGameRule[14].isOn;
        // 托管
        sendVo.trusteeship = lldzGameRule[15].isOn;
        // 抢庄
        sendVo.qiang = false;

        // 使用斗牛的Panel    
        sendVo.gameType = 3;
        if (this.name == "Panel_CreateClubRomm_Dialog(Clone)")
        {
            sendVo.creditScoreIsOn = lldzGameRule[16].isOn;
            sendVo.creditScoreMultiply = int.Parse(lldzGameRule[16].gameObject.GetComponentInChildren<Text>().text.Split(' ')[1]);
        }

        string sendMagStr = JsonMapper.ToJson(sendVo);
        ReqForCreateRoom(sendMagStr);
    }

    public void CreateFZBWZRoom()
    {
        sendVo = new RoomCreateVo();
        if (isclubroom)
        {
            int clubid = GlobalDataScript.clubid;
            if (clubid == 0)
            {
                TipsManagerScript.getInstance().setTips("你不在俱乐部中无法创建俱乐部房间");
                return;
            }
            sendVo.clubid = clubid;
        }
        sendVo.ruleType = 5;
        sendVo.ming = true;
        sendVo.mengs = 1;
        sendVo.zhuForNiu = 0;

        sendVo.qiangForNiu = -1;
        sendVo.diFen = 1;
        sendVo.ruleType = 1;
        // 人数
        sendVo.playerAmounts = fzbwzGameRule[0].isOn ? 6 : 10;
        // 局数
        sendVo.roundNumber = fzbwzGameRule[2].isOn ? 10 : 20;
        // 支付类型
        sendVo.AA = fzbwzGameRule[4].isOn;
        // 游戏模式
        sendVo.rules = fzbwzGameRule[6].isOn ? 1 : 2;
        // 抢庄基本倍率
        sendVo.hogMultiples = 1;
        // 抢庄详细倍率   1：小倍   2：中倍   3：大倍
        sendVo.multiplyingPower = fzbwzGameRule[9].isOn ? 1 : fzbwzGameRule[10].isOn ? 2 : 3;

        // 搓牌
        sendVo.cuoPai = fzbwzGameRule[12].isOn;
        //// 特殊牌型
        //sendVo.special = fzbwzGameRule[13].isOn;
        // 牛7翻倍
        sendVo.niu7fan = fzbwzGameRule[13].isOn;
        // 托管
        sendVo.trusteeship = fzbwzGameRule[14].isOn;
        // 抢庄
        sendVo.qiang = false;

        // 使用斗牛的Panel    
        sendVo.gameType = 3;
        if (this.name == "Panel_CreateClubRomm_Dialog(Clone)")
        {
            sendVo.creditScoreIsOn = fzbwzGameRule[15].isOn;
            sendVo.creditScoreMultiply = int.Parse(fzbwzGameRule[15].gameObject.GetComponentInChildren<Text>().text.Split(' ')[1]);
        }

        string sendMagStr = JsonMapper.ToJson(sendVo);
        ReqForCreateRoom(sendMagStr);
    }

    public void CreateZDPWZRoom()
    {
        sendVo = new RoomCreateVo();
        if (isclubroom)
        {
            int clubid = GlobalDataScript.clubid;
            if (clubid == 0)
            {
                TipsManagerScript.getInstance().setTips("你不在俱乐部中无法创建俱乐部房间");
                return;
            }
            sendVo.clubid = clubid;
        }

        sendVo.ruleType = 6;
        sendVo.ming = true;
        sendVo.mengs = 1;
        sendVo.zhuForNiu = 0;

        sendVo.qiangForNiu = -1;
        sendVo.diFen = 1;
        // 人数
        sendVo.playerAmounts = zdpwzGameRule[0].isOn ? 6 : 10;
        // 局数
        sendVo.roundNumber = zdpwzGameRule[2].isOn ? 10 : 20;
        // 支付类型
        sendVo.AA = zdpwzGameRule[4].isOn;
        // 游戏模式
        // 三元表达式嵌套，表示toggle单选最终值   1：明牌下注   2：暗牌下注   3：扫雷模式
        sendVo.rules = zdpwzGameRule[6].isOn ? 1 : (zdpwzGameRule[7].isOn ? 2 : 3);
        if (sendVo.rules == 1) sendVo.ming = true;
        else if (sendVo.rules == 2) sendVo.ming = false;
        // 抢庄基本倍率
        sendVo.hogMultiples = 1;
        // 抢庄详细倍率   1：小倍   2：中倍   3：大倍
        sendVo.multiplyingPower = zdpwzGameRule[10].isOn ? 1 : zdpwzGameRule[11].isOn ? 2 : 3;

        // 搓牌
        sendVo.cuoPai = zdpwzGameRule[13].isOn;
        //// 特殊牌型
        //sendVo.special = zdpwzGameRule[14].isOn;
        // 牛7翻倍
        sendVo.niu7fan = zdpwzGameRule[14].isOn;
        // 托管
        sendVo.trusteeship = zdpwzGameRule[15].isOn;
        // 抢庄
        sendVo.qiang = false;

        // sendVo.creditScoreIsOn = zdpwzGameRule[17].isOn;
        // sendVo.creditScoreMultiply = zdpwzGameRule[18].isOn ? 1 : 0;

        // 使用斗牛的Panel    
        sendVo.gameType = 3;
        if (this.name == "Panel_CreateClubRomm_Dialog(Clone)")
        {
            sendVo.creditScoreIsOn = zdpwzGameRule[16].isOn;
            sendVo.creditScoreMultiply = int.Parse(zdpwzGameRule[16].gameObject.GetComponentInChildren<Text>().text.Split(' ')[1]);
        }
        string sendMagStr = JsonMapper.ToJson(sendVo);
        ReqForCreateRoom(sendMagStr);
    }

    // End


    /**
	 * 创建德州扑克房间
	 */
    public void createDZPKRoom()
    {
        sendVo = new RoomCreateVo();
        if (dzpkGameRule[0].isOn)
        {
            sendVo.roundNumber = 8;
        }
        else if (dzpkGameRule[1].isOn)
        {
            sendVo.roundNumber = 16;
        }
        else if (dzpkGameRule[2].isOn)
        {
            sendVo.roundNumber = 24;
        }

        if (dzpkGameRule[3].isOn)
        {
            sendVo.initFen_dzpk = 200;
        }
        else if (dzpkGameRule[4].isOn)
        {
            sendVo.initFen_dzpk = 500;
        }
        else if (dzpkGameRule[5].isOn)
        {
            sendVo.initFen_dzpk = 1000;
        }
        else if (dzpkGameRule[6].isOn)
        {
            sendVo.initFen_dzpk = 2000;
        }
        else if (dzpkGameRule[7].isOn)
        {
            sendVo.initFen_dzpk = 5000;
        }

        sendVo.gameType = 4;//德州扑克  3  (int)GameType.GameType_PK_DN

        string sendmsgstr = JsonMapper.ToJson(sendVo);

        ReqForCreateRoom(sendmsgstr);
    }


    /**
	 * 创建长沙麻将房间
	 */
    public void createChangshaRoom()
    {
        SoundCtrl.getInstance().playSoundByActionButton(1);
        int roundNumber = 4;//房卡数量
        bool isZimo = false;//自摸
        int maCount = 0;
        for (int i = 0; i < changshaRoomCards.Count; i++)
        {
            Toggle item = changshaRoomCards[i];
            if (item.isOn)
            {
                if (i == 0)
                {
                    roundNumber = 8;
                }
                else if (i == 1)
                {
                    roundNumber = 16;
                }
                break;
            }
        }
        if (changshaGameRule[0].isOn)
        {
            isZimo = true;
        }

        for (int i = 0; i < changshaZhuama.Count; i++)
        {
            if (changshaZhuama[i].isOn)
            {
                maCount = 2 * (i + 1);
                break;
            }
        }

        sendVo = new RoomCreateVo();
        sendVo.ma = maCount;
        sendVo.roundNumber = roundNumber;
        sendVo.ziMo = isZimo ? 1 : 0;
        sendVo.roomType = (int)GameType.GameType_MJ_ChangSha;
        string sendmsgstr = JsonMapper.ToJson(sendVo);
        ReqForCreateRoom(sendmsgstr);

    }

    /**
	 * 创建划水麻将房间
	 */
    public void createHuashuiRoom()
    {
        SoundCtrl.getInstance().playSoundByActionButton(1);
        int roundNumber = 4;//房卡数量
        bool isZimo = false;//自摸
        bool isFengpai = false;//七小对
        int maCount = 0;
        for (int i = 0; i < huashuiRoomCards.Count; i++)
        {
            Toggle item = huashuiRoomCards[i];
            if (item.isOn)
            {
                if (i == 0)
                {
                    roundNumber = 8;
                }
                else if (i == 1)
                {
                    roundNumber = 16;
                }
                break;
            }
        }
        if (huashuiGameRule[0].isOn)
        {
            isFengpai = true;
        }
        if (huashuiGameRule[1].isOn)
        {
            isZimo = true;
        }


        for (int i = 0; i < huashuixiayu.Count; i++)
        {
            if (huashuixiayu[i].isOn)
            {
                maCount = 2 * (i + 1) + i;
                break;
            }
        }

        sendVo = new RoomCreateVo();
        sendVo.xiaYu = maCount;
        sendVo.roundNumber = roundNumber;
        sendVo.ziMo = isZimo ? 1 : 0;
        sendVo.roomType = (int)GameType.GameType_MJ_HuaShui;
        sendVo.addWordCard = isFengpai;
        sendVo.sevenDouble = true;
        string sendmsgstr = JsonMapper.ToJson(sendVo);
        ReqForCreateRoom(sendmsgstr);
    }

    /**
	 * 创建红中麻将房间
	 */
    public void createHongZhongRoom()
    {
        SoundCtrl.getInstance().playSoundByActionButton(1);
        int roundNumber = 4;//房卡数量
        bool isZimo = false;//自摸
        bool isFengpai = false;//七小对
        int maCount = 0;
        for (int i = 0; i < huashuiRoomCards.Count; i++)
        {
            Toggle item = huashuiRoomCards[i];
            if (item.isOn)
            {
                if (i == 0)
                {
                    roundNumber = 8;
                }
                else if (i == 1)
                {
                    roundNumber = 16;
                }
                break;
            }
        }
        if (huashuiGameRule[0].isOn)
        {
            isFengpai = true;
        }
        if (huashuiGameRule[1].isOn)
        {
            isZimo = true;
        }


        for (int i = 0; i < huashuixiayu.Count; i++)
        {
            if (huashuixiayu[i].isOn)
            {
                maCount = 2 * (i + 1) + i;
                break;
            }
        }

        sendVo = new RoomCreateVo();
        sendVo.hong = true;
        sendVo.xiaYu = maCount;
        sendVo.roundNumber = roundNumber;
        sendVo.ziMo = isZimo ? 1 : 0;
        sendVo.roomType = (int)GameType.GameType_MJ_HongZhong;
        sendVo.addWordCard = isFengpai;
        sendVo.sevenDouble = true;
        string sendmsgstr = JsonMapper.ToJson(sendVo);
        ReqForCreateRoom(sendmsgstr);
    }

    //	public void toggleHongClick(){
    //
    //		if (zhuanzhuanGameRule [2].isOn) {
    //			zhuanzhuanGameRule [0].isOn = true;
    //		}
    //	}
    //
    //	public void toggleQiangGangHuClick(){
    //		if (zhuanzhuanGameRule [1].isOn) {
    //			zhuanzhuanGameRule [2].isOn = false;
    //		}
    //	}

    public void onCreateRoomCallback(ClientResponse response)
    {
        if (watingPanel != null)
        {
            watingPanel.gameObject.SetActive(false);
        }
        MyDebug.Log(response.message);
        if (response.status == 1)
        {
            //RoomCreateResponseVo responseVO = JsonMapper.ToObject<RoomCreateResponseVo> (response.message);
            int roomid = Int32.Parse(response.message);
            sendVo.roomId = roomid;
            GlobalDataScript.roomVo = sendVo;
            GlobalDataScript.loginResponseData.roomId = roomid;
            //GlobalDataScript.loginResponseData.isReady = true;
            if (GlobalDataScript.roomVo.gameType == 0)//(int)GameType.GameType_PK_PDK
                GlobalDataScript.loginResponseData.main = true;
            GlobalDataScript.loginResponseData.isOnLine = true;
            GlobalDataScript.reEnterRoomData = null;
            //SceneManager.LoadSceneAsync(1);


            saveDefaultSet(GlobalDataScript.userGameType, userDefaultSet);

            if (GlobalDataScript.roomVo.gameType == 0)
            {//(int)GameType.GameType_PK_PDK
                GlobalDataScript.gamePlayPanel = PrefabManage.loadPerfab("Prefab/Panel_GamePlay");
            }
            else if (GlobalDataScript.roomVo.gameType == 1)
            { //(int)GameType.GameType_PK_PDK
                GlobalDataScript.gamePlayPanel = PrefabManage.loadPerfab("Prefab/Panel_GamePDK");
            }
            else if (GlobalDataScript.roomVo.gameType == 3 && GlobalDataScript.roomVo.playerAmounts == 10)
            {//(int)GameType.GameType_PK_DN
                GlobalDataScript.gamePlayPanel = PrefabManage.loadPerfab("Prefab/Panel_GameDN");
                // SoundCtrl.getInstance().playBGM(3);
            }
            else if (GlobalDataScript.roomVo.gameType == 3 && GlobalDataScript.roomVo.playerAmounts == 6)
            {//(int)GameType.GameType_PK_DN
                GlobalDataScript.gamePlayPanel = PrefabManage.loadPerfab("Prefab/Panel_GameDN_6");
                //  SoundCtrl.getInstance().playBGM(3);
            }
            else if (GlobalDataScript.roomVo.gameType == 4)
            {
                GlobalDataScript.gamePlayPanel = PrefabManage.loadPerfab("Prefab/Panel_GameDZPK");
            }
            closeDialog();

        }
        else
        {
            TipsManagerScript.getInstance().setTips(response.message);
        }
    }

    public void MingChanged()
    {
        dnGameRule[9].gameObject.SetActive(dnGameRule[7].isOn);
        dnGameRule[10].gameObject.SetActive(dnGameRule[7].isOn);
    }

    public void fanguiChanged()
    {
        if (guangdongGui[2].isOn)
        {
            guangdongGui[3].enabled = true;
        }
        else
        {
            guangdongGui[3].isOn = false;
            guangdongGui[3].enabled = false;
        }
    }

    public void maChanged()
    {
        if (guangdongZhuama[0].isOn || guangdongZhuama[1].isOn || guangdongZhuama[2].isOn)
        {
            guangdongZhuama[3].enabled = true;
            guangdongZhuama[4].enabled = true;
        }
        else
        {
            guangdongZhuama[3].isOn = false;
            guangdongZhuama[4].isOn = false;
            guangdongZhuama[3].enabled = false;
            guangdongZhuama[4].enabled = false;
        }
    }

    public void saveDefaultSet(GameType gt, string userSet)
    {//保存设置
        if (gt == GameType.GameType_NULL) return;
        PlayerPrefs.SetInt("userDefaultGameType", (int)gt);
        if (userSet != null)
        {
            switch (gt)
            {
                //case GameType.GameType_MJ_ZhuangZhuang:
                case GameType.GameType_MJ_YiChun:
                    PlayerPrefs.SetString("userDefaultSet_ZhuangZhuang", userSet);
                    break;
                case GameType.GameType_MJ_HuaShui:
                    PlayerPrefs.SetString("userDefaultSet_HuaShui", userSet);
                    break;
                case GameType.GameType_MJ_ChangSha:
                    PlayerPrefs.SetString("userDefaultSet_ChangSha", userSet);
                    break;
                case GameType.GameType_MJ_GuangDong:
                    PlayerPrefs.SetString("userDefaultSet_GuangDong", userSet);
                    break;
                case GameType.GameType_MJ_GanZhou:
                    PlayerPrefs.SetString("userDefaultSet_GanZhou", userSet);
                    break;
                case GameType.GameType_MJ_RuiJin:
                    PlayerPrefs.SetString("userDefaultSet_RuiJin", userSet);
                    break;
                case GameType.GameType_MJ_ChaoShan:
                    PlayerPrefs.SetString("userDefaultSet_ChaoShan", userSet);
                    break;
                case GameType.GameType_PK_PDK:
                    PlayerPrefs.SetString("userDefaultSet_GanZhou", userSet);
                    break;
                case GameType.GameType_PK_DN:
                    PlayerPrefs.SetString("userDefaultSet_DN", userSet);
                    break;

                // Add by Shise
                case GameType.GameType_PK_KPQZ:
                    PlayerPrefs.SetString("userDefaultSet_DN", userSet);
                    break;
                case GameType.GameType_PK_XJTZ:
                    PlayerPrefs.SetString("userDefaultSet_DN", userSet);
                    break;
                case GameType.GameType_PK_LLDZ:
                    PlayerPrefs.SetString("userDefaultSet_DN", userSet);
                    break;
                case GameType.GameType_PK_NNHZ:
                    PlayerPrefs.SetString("userDefaultSet_DN", userSet);
                    break;
                case GameType.GameType_PK_FZBWZ:
                    PlayerPrefs.SetString("userDefaultSet_DN", userSet);
                    break;
                case GameType.GameType_PK_ZDPWZ:
                    PlayerPrefs.SetString("userDefaultSet_DN", userSet);
                    break;
                //case GameType.GameType_PK_KPQZ:
                //    PlayerPrefs.SetString("userDefaultSet_KPQZ", userSet);
                //    break;
                //case GameType.GameType_PK_XJTZ:
                //    PlayerPrefs.SetString("userDefaultSet_XJTZ", userSet);
                //    break;
                //case GameType.GameType_PK_LLDZ:
                //    PlayerPrefs.SetString("userDefaultSet_LLDZ", userSet);
                //    break;
                //case GameType.GameType_PK_NNHZ:
                //    PlayerPrefs.SetString("userDefaultSet_NNHZ", userSet);
                //    break;
                //case GameType.GameType_PK_FZBWZ:
                //    PlayerPrefs.SetString("userDefaultSet_FZBWZ", userSet);
                //    break;
                //case GameType.GameType_PK_ZDPWZ:
                //    PlayerPrefs.SetString("userDefaultSet_ZDPWZ", userSet);
                //    break;
                // End

                default:
                    break;
            }
        }
    }
    public void loadDefaultSet(GameType gt)
    {
        if (gt == GameType.GameType_NULL) return;
        switch (gt)
        {
            case GameType.GameType_MJ_Developing:
                break;
            //case GameType.GameType_MJ_ZhuangZhuang:
            case GameType.GameType_MJ_YiChun:
                loadSet_ZhuangZhuang();
                break;
            case GameType.GameType_MJ_HuaShui:
                loadSet_HuaShui();
                break;
            case GameType.GameType_MJ_ChangSha:
                loadSet_ChangSha();
                break;
            case GameType.GameType_MJ_GuangDong:
                loadSet_GuangDong();
                break;
            case GameType.GameType_MJ_GanZhou:
                loadSet_GanZhou();
                break;
            case GameType.GameType_MJ_RuiJin:
                loadSet_RuiJin();
                break;
            case GameType.GameType_MJ_HongZhong:
                loadSet_HongZhong();
                break;
            case GameType.GameType_MJ_ChaoShan:
                loadSet_ChaoShan();
                break;
            case GameType.GameType_PK_PDK:
                loadSet_PDK();
                break;
            case GameType.GameType_PK_DN:
                loadSet_DN();
                break;
            case GameType.GameType_PK_DZPK:
                loadSet_DZPK();
                break;

            // Add by Shise
            case GameType.GameType_PK_KPQZ:
                LoadSet_KPQZ();
                break;
            case GameType.GameType_PK_XJTZ:
                LoadSet_XJTZ();
                break;
            case GameType.GameType_PK_LLDZ:
                LoadSet_LLDZ();
                break;
            case GameType.GameType_PK_NNHZ:
                LoadSet_NNHZ();
                break;
            case GameType.GameType_PK_FZBWZ:
                LoadSet_FZBWZ();
                break;
            case GameType.GameType_PK_ZDPWZ:
                LoadSet_ZDPWZ();
                break;
            // End

            default:
                break;
        }
    }

    // Add by Shise
    private void LoadSet_ZDPWZ()
    {
        if (PlayerPrefs.HasKey("userDefaultSet_ZDPWZ"))
        {
            userDefaultSet = PlayerPrefs.GetString("userDefaultSet_ZDPWZ");
            //if (userDefaultSet != null)
            //{
            //RoomCreateVo roomVo = JsonMapper.ToObject<RoomCreateVo>(userDefaultSet);

            //ganzhouRoomCards[0].isOn = 8 == roomVo.roundNumber;
            //ganzhouRoomCards[1].isOn = 16 == roomVo.roundNumber;

            //ganzhouGameRule[0].isOn = 1 == roomVo.shangxiaFanType;
            //ganzhouGameRule[1].isOn = 2 == roomVo.shangxiaFanType;

            //ganzhouGameRule[2].isOn = 1 == roomVo.diFen;
            //ganzhouGameRule[3].isOn = 2 == roomVo.diFen;

            //ganzhouGameRule[4].isOn = roomVo.tongZhuang;
            //ganzhouGameRule[5].isOn = !roomVo.tongZhuang;

            //ganzhouGameRule[6].isOn = 1 == roomVo.pingHu;
            //ganzhouGameRule[7].isOn = 2 == roomVo.pingHu;
            //ganzhouGameRule[8].isOn = 3 == roomVo.pingHu;
            //}
        }
    }

    private void LoadSet_FZBWZ()
    {

    }

    private void LoadSet_NNHZ()
    {
        if (PlayerPrefs.HasKey("userDefaultSet_NNHZ"))
        {
            userDefaultSet = PlayerPrefs.GetString("userDefaultSet_NNHZ");
        }
    }

    private void LoadSet_LLDZ()
    {
        if (PlayerPrefs.HasKey("userDefaultSet_LLDZ"))
        {
            userDefaultSet = PlayerPrefs.GetString("userDefaultSet_LLDZ");
        }
    }

    private void LoadSet_XJTZ()
    {
        if (PlayerPrefs.HasKey("userDefaultSet_XJTZ"))
        {
            userDefaultSet = PlayerPrefs.GetString("userDefaultSet_XJTZ");
        }
    }

    private void LoadSet_KPQZ()
    {
        if (PlayerPrefs.HasKey("userDefaultSet_KPQZ"))
        {
            userDefaultSet = PlayerPrefs.GetString("userDefaultSet_KPQZ");
        }
    }
    // End

    public void loadSet_ZhuangZhuang()
    {

    }
    public void loadSet_HuaShui()
    {

    }
    public void loadSet_ChangSha()
    {

    }
    public void loadSet_GuangDong()
    {

    }
    public void loadSet_HongZhong()
    {

    }
    public void loadSet_GanZhou()
    {
        if (PlayerPrefs.HasKey("userDefaultSet_GanZhou"))
        {
            userDefaultSet = PlayerPrefs.GetString("userDefaultSet_GanZhou");
            if (userDefaultSet != null)
            {
                RoomCreateVo roomVo = JsonMapper.ToObject<RoomCreateVo>(userDefaultSet);

                ganzhouRoomCards[0].isOn = 8 == roomVo.roundNumber;
                ganzhouRoomCards[1].isOn = 16 == roomVo.roundNumber;

                ganzhouGameRule[0].isOn = 1 == roomVo.shangxiaFanType;
                ganzhouGameRule[1].isOn = 2 == roomVo.shangxiaFanType;

                ganzhouGameRule[2].isOn = 1 == roomVo.diFen;
                ganzhouGameRule[3].isOn = 2 == roomVo.diFen;

                ganzhouGameRule[4].isOn = roomVo.tongZhuang;
                ganzhouGameRule[5].isOn = !roomVo.tongZhuang;

                ganzhouGameRule[6].isOn = 1 == roomVo.pingHu;
                ganzhouGameRule[7].isOn = 2 == roomVo.pingHu;
                ganzhouGameRule[8].isOn = 3 == roomVo.pingHu;
            }
        }
    }
    public void loadSet_RuiJin()
    {
        if (PlayerPrefs.HasKey("userDefaultSet_RuiJin"))
        {
            userDefaultSet = PlayerPrefs.GetString("userDefaultSet_RuiJin");
            if (userDefaultSet != null)
            {
                RoomCreateVo roomVo = JsonMapper.ToObject<RoomCreateVo>(userDefaultSet);

                ruijinRoomCards[0].isOn = 4 == roomVo.roundNumber;
                ruijinRoomCards[1].isOn = 8 == roomVo.roundNumber;
                ruijinRoomCards[2].isOn = 16 == roomVo.roundNumber;

                ruijinGameRule[0].isOn = roomVo.keDianPao;

                ruijinGameRule[1].isOn = 1 == roomVo.diFen;
                ruijinGameRule[2].isOn = 2 == roomVo.diFen;
                ruijinGameRule[3].isOn = 5 == roomVo.diFen;

                ruijinGameRule[4].isOn = roomVo.tongZhuang;
                ruijinGameRule[5].isOn = !roomVo.tongZhuang;
                ruijinGameRule[6].isOn = roomVo.lunZhuang;
                ruijinGameRule[7].isOn = !roomVo.lunZhuang;
            }
        }
    }

    public void loadSet_ChaoShan()
    {
        if (PlayerPrefs.HasKey("userDefaultSet_ChaoShan"))
        {
            userDefaultSet = PlayerPrefs.GetString("userDefaultSet_ChaoShan");
            if (userDefaultSet != null)
            {
                RoomCreateVo roomVo = JsonMapper.ToObject<RoomCreateVo>(userDefaultSet);

                xinchaoshanGameRule[0].isOn = 8 == roomVo.roundNumber;
                xinchaoshanGameRule[1].isOn = 16 == roomVo.roundNumber;
                xinchaoshanGameRule[2].isOn = 24 == roomVo.roundNumber;

                xinchaoshanGameRule[3].isOn = roomVo.gangHu;
                xinchaoshanGameRule[4].isOn = roomVo.genZhuang;
                xinchaoshanGameRule[5].isOn = roomVo.chongZhuang;
                xinchaoshanGameRule[6].isOn = roomVo.pengpengHu;
                xinchaoshanGameRule[7].isOn = roomVo.qiDui;
                xinchaoshanGameRule[8].isOn = roomVo.qiangGangHu;
                xinchaoshanGameRule[9].isOn = roomVo.hunYiSe;
                xinchaoshanGameRule[10].isOn = roomVo.qingYiSe;
                xinchaoshanGameRule[11].isOn = roomVo.gangShangKaiHua;
                xinchaoshanGameRule[12].isOn = roomVo.haoHuaQiDui;
                xinchaoshanGameRule[13].isOn = roomVo.shiSanYao;
                xinchaoshanGameRule[14].isOn = roomVo.tianDiHu;
                xinchaoshanGameRule[15].isOn = roomVo.shuangHaoHua;
                xinchaoshanGameRule[16].isOn = roomVo.sanHaoHua;
                xinchaoshanGameRule[17].isOn = roomVo.shiBaLuoHan;
                xinchaoshanGameRule[18].isOn = roomVo.xiaoSanYuan;
                xinchaoshanGameRule[19].isOn = roomVo.xiaoSiXi;
                xinchaoshanGameRule[20].isOn = roomVo.daSanYuan;
                xinchaoshanGameRule[21].isOn = roomVo.daSiXi;
                xinchaoshanGameRule[22].isOn = roomVo.huaYaoJiu;
                xinchaoshanGameRule[23].isOn = 5 == roomVo.fengDing;
                xinchaoshanGameRule[24].isOn = 10 == roomVo.fengDing;
                xinchaoshanGameRule[25].isOn = 99999 == roomVo.fengDing;

                xinchaoshanGameRule[26].isOn = 0 == roomVo.gui;
                xinchaoshanGameRule[27].isOn = 1 == roomVo.gui;
                xinchaoshanGameRule[28].isOn = (2 == roomVo.gui || 3 == roomVo.gui);
                xinchaoshanGameRule[29].isOn = 3 == roomVo.gui;

                xinchaoshanGameRule[30].isOn = 0 == roomVo.ma;
                xinchaoshanGameRule[31].isOn = 2 == roomVo.ma;
                xinchaoshanGameRule[32].isOn = 4 == roomVo.ma;
                xinchaoshanGameRule[33].isOn = 6 == roomVo.ma;
                if (roomVo.ma > 0)
                {
                    xinchaoshanGameRule[34].isOn = roomVo.maGenDifen;
                    xinchaoshanGameRule[35].isOn = roomVo.maGenGang;
                }
                else
                {
                    xinchaoshanGameRule[34].isOn = false;
                    xinchaoshanGameRule[35].isOn = false;
                }

                xinchaoshanGameRule[36].isOn = !roomVo.AA;
                xinchaoshanGameRule[37].isOn = roomVo.AA;

            }
        }
    }
    public void loadSet_PDK()
    {

    }
    public void loadSet_DN()
    {
        if (PlayerPrefs.HasKey("userDefaultSet_DN"))
        {
            userDefaultSet = PlayerPrefs.GetString("userDefaultSet_DN");
            if (userDefaultSet != null)
            {
                RoomCreateVo roomVo = JsonMapper.ToObject<RoomCreateVo>(userDefaultSet);
                dnGameRule[0].isOn = 10 == roomVo.roundNumber;
                dnGameRule[1].isOn = 20 == roomVo.roundNumber;
                dnGameRule[2].isOn = !roomVo.qiang;
                dnGameRule[3].isOn = roomVo.qiang;
                dnGameRule[4].isOn = 1 == roomVo.diFen;
                dnGameRule[5].isOn = 2 == roomVo.diFen;
                dnGameRule[6].isOn = 3 == roomVo.diFen;
                dnGameRule[7].isOn = roomVo.ming;
                dnGameRule[8].isOn = !roomVo.ming;
                dnGameRule[9].isOn = 1 == roomVo.mengs;
                dnGameRule[10].isOn = 2 == roomVo.mengs;
                dnGameRule[11].isOn = roomVo.AA;
                dnGameRule[12].isOn = !roomVo.AA;
                dnGameRule[14].isOn = roomVo.cuoPai;
                dnGameRule[15].isOn = !roomVo.cuoPai;
            }
            MingChanged();
        }
    }

    public void loadSet_DZPK()
    {

    }

    public void openDefaultSetingPanel()
    {

        // Change by Shise
        //GlobalDataScript.userGameType = GameType.GameType_PK_PDK;//GameType_PK_PDK  GameType_MJ_ChaoShan
        GlobalDataScript.userGameType = GameType.GameType_PK_KPQZ;
        // End

        GlobalDataScript.goldType = false;
        GameType gameType = GlobalDataScript.userGameType;
        if (PlayerPrefs.HasKey("userDefaultGameType"))
        {
            gameType = (GameType)PlayerPrefs.GetInt("userDefaultGameType");
        }
        switch (gameType)
        {
            case GameType.GameType_MJ_Developing:
                openDeveloping();
                break;
            //case GameType.GameType_MJ_ZhuangZhuang:
            case GameType.GameType_MJ_YiChun:
                openSetingPanel_ZhuangZhuang();
                break;
            case GameType.GameType_MJ_HuaShui:
                openSetingPanel_HuaShui();
                break;
            case GameType.GameType_MJ_ChangSha:
                openSetingPanel_ChangSha();
                break;
            case GameType.GameType_MJ_GuangDong:
                openSetingPanel_GuangDong();
                break;
            case GameType.GameType_MJ_GanZhou:
                openSetingPanel_GanZhou();
                break;
            case GameType.GameType_MJ_RuiJin:
                openSetingPanel_RuiJin();
                break;
            case GameType.GameType_MJ_ChaoShan:
                openSetingPanel_ChaoShan();
                break;
            case GameType.GameType_PK_PDK:
                openSetingPanel_PDK();
                break;
            case GameType.GameType_PK_DN:
                openSetingPanel_DN();
                break;
            case GameType.GameType_PK_DZPK:
                openSetingPanel_DZPK();
                break;

            // Add by Shise
            case GameType.GameType_PK_KPQZ:
                OpenSettingPanel_KPQZ();
                break;
            case GameType.GameType_PK_XJTZ:
                OpenSettingPanel_XJTZ();
                break;
            case GameType.GameType_PK_LLDZ:
                OpenSettingPanel_LLDZ();
                break;
            case GameType.GameType_PK_NNHZ:
                OpenSettingPanel_NNHZ();
                break;
            case GameType.GameType_PK_FZBWZ:
                OpenSettingPanel_FZBWZ();
                break;
            case GameType.GameType_PK_ZDPWZ:
                OpenSettingPanel_ZDPWZ();
                break;
            // End

            default:
                break;
        }
    }

    public void setGameObjectActive(GameType gt)
    {//设置游戏物体显示的切换
     //		Btn_zhuanZ_liang.SetActive(GameType.GameType_MJ_ZhuangZhuang == gt);//GameType.GameType_MJ_ZhuangZhuang == gt
     //		Btn_zhuanZ_dark.SetActive(GameType.GameType_MJ_ZhuangZhuang != gt);//GameType.GameType_MJ_ZhuangZhuang != gt
     //Btn_zhuanZ_liang.SetActive(GameType.GameType_MJ_YiChun == gt);
     //Btn_zhuanZ_dark.SetActive(GameType.GameType_MJ_YiChun != gt);
     ////		Btn_huaS_liang.SetActive(false);//GameType.GameType_MJ_HuaShui == gt
     ////		Btn_huaS_dark.SetActive(false);//GameType.GameType_MJ_HuaShui != gt
     //Btn_huaS_liang.SetActive(GameType.GameType_MJ_HuaShui == gt);//GameType.GameType_MJ_HuaShui == gt
     //Btn_huaS_dark.SetActive(GameType.GameType_MJ_HuaShui != gt);//GameType.GameType_MJ_HuaShui != gt
     //Btn_ganzhou_liang.SetActive(GameType.GameType_MJ_GanZhou == gt);//
     //Btn_ganzhou_dark.SetActive(GameType.GameType_MJ_GanZhou != gt);//
     //Btn_ruijin_liang.SetActive(GameType.GameType_MJ_RuiJin == gt);
     //Btn_ruijin_dark.SetActive(GameType.GameType_MJ_RuiJin != gt);
     //Btn_hongzhong_liang.SetActive(GameType.GameType_MJ_HongZhong == gt);
     //Btn_hongzhong_dark.SetActive(GameType.GameType_MJ_HongZhong != gt);
     //Btn_run_liang.SetActive(GameType.GameType_MJ_GuangDong == gt);
     //Btn_run_dark.SetActive(GameType.GameType_MJ_GuangDong != gt);
        Btn_pdk_liang.SetActive(GameType.GameType_PK_PDK == gt);//GameType.GameType_PK_PDK == gt
        Btn_pdk_dark.SetActive(GameType.GameType_PK_PDK != gt);//GameType.GameType_PK_PDK != gt

        //Btn_dn_liang.SetActive(GameType.GameType_PK_DN == gt);
        //Btn_dn_dark.SetActive(GameType.GameType_PK_DN != gt);
        //Btn_chaoshan_liang.SetActive(GameType.GameType_MJ_ChaoShan == gt);
        //Btn_chaoshan_dark.SetActive(GameType.GameType_MJ_ChaoShan != gt);
        Btn_dzpk_liang.SetActive(GameType.GameType_PK_DZPK == gt);
        Btn_dzpk_dark.SetActive(GameType.GameType_PK_DZPK != gt);

        // Add by Shise
        Btn_kpqz_light.SetActive(GameType.GameType_PK_KPQZ == gt);
        Btn_kpqz_dark.SetActive(GameType.GameType_PK_KPQZ != gt);
        // Btn_xjtz_light.SetActive(GameType.GameType_PK_XJTZ == gt);
        // Btn_xjtz_dark.SetActive(GameType.GameType_PK_XJTZ != gt);
        Btn_lldz_light.SetActive(GameType.GameType_PK_LLDZ == gt);
        Btn_lldz_dark.SetActive(GameType.GameType_PK_LLDZ != gt);
        Btn_nnhz_light.SetActive(GameType.GameType_PK_NNHZ == gt);
        Btn_nnhz_dark.SetActive(GameType.GameType_PK_NNHZ != gt);
        Btn_fzbwz_light.SetActive(GameType.GameType_PK_FZBWZ == gt);
        Btn_fzbwz_dark.SetActive(GameType.GameType_PK_FZBWZ != gt);
        Btn_zdpwz_light.SetActive(GameType.GameType_PK_ZDPWZ == gt);
        Btn_zdpwz_dark.SetActive(GameType.GameType_PK_ZDPWZ != gt);
        // End

        //		if (GlobalDataScript.configTemp.pay == 1) {
        //			Btn_ganzhou_liang.SetActive (false);
        //			Btn_ganzhou_dark.SetActive (true);//true
        //		}
        if (GameType.GameType_MJ_Developing == gt)
        {
            createRoom.gameObject.SetActive(false);
        }
        else
        {
            createRoom.gameObject.SetActive(true);
        }


        //		panelZhuanzhuanSetting.SetActive(GameType.GameType_MJ_ZhuangZhuang == gt);
        // panelZhuanzhuanSetting.SetActive(GameType.GameType_MJ_YiChun == gt);
        // panelHuashuiSetting.SetActive(GameType.GameType_MJ_HuaShui == gt);
        // panelChangshaSetting.SetActive(GameType.GameType_MJ_ChangSha == gt);
        // panelGuangDongSetting.SetActive(GameType.GameType_MJ_GuangDong == gt);
        // panelGanZhouSetting.SetActive(GameType.GameType_MJ_GanZhou == gt);
        // panelRuiJinSetting.SetActive(GameType.GameType_MJ_RuiJin == gt);
        // panelHongZhongSetting.SetActive(GameType.GameType_MJ_HongZhong == gt);
        // panelChaoShanSetting.SetActive(GameType.GameType_MJ_ChaoShan == gt);
        panelPdkSetting.SetActive(GameType.GameType_PK_PDK == gt);
        // panelDevoloping.SetActive(GameType.GameType_MJ_Developing == gt);
        // panelDnSetting.SetActive(GameType.GameType_PK_DN == gt);
        panelDzpkSetting.SetActive(GameType.GameType_PK_DZPK == gt);

        // Add by Shise
        PanelKpqzSetting.SetActive(GameType.GameType_PK_KPQZ == gt);
        // PanelXjtzSetting.SetActive(GameType.GameType_PK_XJTZ == gt);
        PanelLldzSetting.SetActive(GameType.GameType_PK_LLDZ == gt);
        PanelNnhzSetting.SetActive(GameType.GameType_PK_NNHZ == gt);
        PanelFzbwzSetting.SetActive(GameType.GameType_PK_FZBWZ == gt);
        PanelZdpwzSetting.SetActive(GameType.GameType_PK_ZDPWZ == gt);
        // End
    }
}
