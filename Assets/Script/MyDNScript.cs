using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AssemblyCSharp;
using DG.Tweening;
using UnityEngine.UI;
using LitJson;
using UnityEngine.SceneManagement;

public class MyDNScript : MonoBehaviour
{

    public List<PlayerItemScript> playerItems;
    //所有玩家
    public List<AvatarVO> avatarList;

    private biaoqingScript bqScript;
    private GameObject bqObj;

    /**更否申请退出房间申请**/
    // Change by Shise
    private bool canClickButtonFlag = true;
    // End

    //解散房间
    public Image dialog_fanhui;
    public Text dialog_fanhui_text;

    public Text versionText;
    public Text Number;
    public Text roomRemark;
    public Text goldText;
    public GameObject gold;

    public Button readyBtn;
    public Button inviteFriendButton;

    private int bankerId;
    private int curDirIndex;
    private int currentIndex = -1;
    //当前出牌人
    private string curDirString = "B";
    //当前的方向字符串

    private bool isFirstOpen = true;

    public pdkButtonActionScript btnActionScript;

    private List<List<int>> mineList;

    public List<Transform> handTransformList;
    public List<Transform> ThrowTransformList;

    public List<List<GameObject>> handerCardList;    // 手牌

    private float timer = 0;
    private bool isGameStart = false;

    private int[] lastCard;
    //上家、或上上家最后出的牌

    // Use this for initialization

    public GameObject panel_Qiang;
    public GameObject panel_Zhus;

    // Add by Shise
    public GameObject panel_Zhus_Small;
    public GameObject panel_Zhus_Medium;
    public GameObject panel_Zhus_Big;
    public List<int> qiangBank;     // 存储最终抢庄的玩家下标
    // End

    public GameObject panel_Niu;    //显示有牛无牛按钮界面
    public GameObject panel_RankNiu;
    public GameObject panel_Tips;

    private bool hasQiang = false;
    private bool hasZhu = false;
    private int hasZhus = 0;
    private int hasQiangs = 0;

    public GameObject Btn_cheat;
    public GameObject cheatPanel;
    public GameObject cheatContentParent;
    private List<GameObject> cheatObjList;
    private List<int> cheatSendList;

    //搓牌按钮
    public Button cuoPai_Button;
    public GameObject cuoPaiPanel;
    public DNCPCard cuoPaiCardPoint;
    //金币UI
    public GameObject gold_Sprite;
    GameObject gold_SpriteClone;
    List<GameObject> listGold = new List<GameObject>();
    //是否结束
    bool isFinish;

    List<int> myCardList = new List<int>();
    private void Awake()
    {
        cuoPai_Button.onClick.AddListener(OnCuoPaiClick);
    }
    void Start()
    {
        // Add by Shise   进入GAMEDN播放音乐
        if (gameObject.activeSelf == true)
        {
            SoundCtrl.getInstance().playBGM(4);
        }
        // End
        init();
        if (GlobalDataScript.reEnterRoomData != null)
        { //断线重连进入房间
            GlobalDataScript.loginResponseData.roomId = GlobalDataScript.reEnterRoomData.roomId;
            reEnterRoom();
        }
        else if (GlobalDataScript.roomJoinResponseData != null)
        {//进入他人房间
            joinToRoom(GlobalDataScript.roomJoinResponseData.playerList);
            GlobalDataScript.loginResponseData.stateForNiu = 0;
            SetTip();
        }
        else
        {//创建房间
            createRoomAddAvatarVO(GlobalDataScript.loginResponseData);
            GlobalDataScript.loginResponseData.stateForNiu = 0;
            SetTip();
        }

        showFangzhu();
        loadBiaoQing();

        if (GlobalDataScript.loginResponseData.account.isCheat == 1)
        {
            Btn_cheat.SetActive(true);
        }
        else
        {
            Btn_cheat.SetActive(false);
        }
        cheatPanel.SetActive(false);


        if (GlobalDataScript.roomVo.multiplyingPower == 1)
        {
            panel_Zhus = panel_Zhus_Small;
        }
        else if (GlobalDataScript.roomVo.multiplyingPower == 2)
        {
            panel_Zhus = panel_Zhus_Medium;
        }
        else if (GlobalDataScript.roomVo.multiplyingPower == 3)
        {
            panel_Zhus = panel_Zhus_Big;
        }
    }

    public void init()
    {
        GlobalDataScript.isonLoginPage = false;
        addListener();
        versionText.text = "V" + Application.version;
    }
    public void OnCuoPaiClick()
    {
        GameObject go = handerCardList[0][4];
        if (go == null)
        {
            return;
        }
        else
        {
            cuoPaiPanel.SetActive(true);
            cuoPaiCardPoint.setPoint(go.GetComponent<pdkCardScript>().getPoint());
            Debug.Log("错牌最后一张牌" + go.GetComponent<pdkCardScript>().getPoint());
            Invoke("FanPaiDelay", 5f);
        }// End mark

        //if (myCardList.Count<5)
        //{
        //    return;
        //}
        //cuoPaiPanel.SetActive(true);
        ////this.gameObject.SetActive(false);
        //Debug.Log("错牌最后一张牌" + myCardList[myCardList.Count - 1].ToString());
        //cuoPaiCardPoint.setPoint(myCardList[myCardList.Count - 1],3);
        //Invoke("FanPaiDelay", 5f);
    }
    void FanPaiDelay()
    {
        cuoPaiPanel.SetActive(false);
        FanPai(0);
    }
    public void showFangzhu()//考虑放到
    {
        //显示房主
        int myIndex = getMyIndexFromList();
        if (myIndex == 0)
        {
            playerItems[0].showFangZhu();

        }
        else if (myIndex >= 1 && myIndex < GlobalDataScript.roomVo.playerAmounts)
        {
            playerItems[playerItems.Count - myIndex].showFangZhu();
            //print(playerItems.Count);
            //print(playerItems.Count - myIndex);
        }
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            timer = -1;
        }
        /**
		if (isGameStart && currentIndex >= 0) {
			playerItems [currentIndex].showClockText (Math.Floor (timer) + "");
		}
		*/
        panel_Tips.GetComponent<TipsScript_DN>().setTime((int)Math.Floor(timer));
        if ((int)Math.Floor(timer) == 0)
        {
            if (!hasQiang && GlobalDataScript.roomVo.qiang)
            {
                qiangZhuang(0);   // todo  mark
            }
            else if (GlobalDataScript.mainUuid != GlobalDataScript.loginResponseData.account.uuid && !hasZhu)
                xiaZhu(1);
            else if (GlobalDataScript.loginResponseData.stateForNiu == 7)
            {
                // Remove by Shise
                // hasNiu(GlobalDataScript.loginResponseData.niu > 0);
                // Add
                //showCards();
                // End
            }
        }
        if (inviteFriendButton.isActiveAndEnabled)
        {
            if (avatarList != null && avatarList.Count == GlobalDataScript.roomVo.playerAmounts)    //5
                inviteFriendButton.gameObject.SetActive(true);
        }

        if (isFinish)
        {
            //GoldMove();
        }
    }

    void OnGUI()
    {
        /*
		Event e = Event.current;
		if (e.isMouse && (e.clickCount == 2)) {
			MyDebug.Log ("用户双击了鼠标");
			if (handerCardList != null && handerCardList.Count > 0) {
				cardReset ();
				isCanChu ();
			}
		}*/
    }

    public void addListener()
    {
        SocketEventHandle.getInstance().outRoomCallback += outRoomCallbak;
        SocketEventHandle.getInstance().otherUserJointRoomCallBack += otherUserJointRoom;
        SocketEventHandle.getInstance().messageBoxNotice += messageBoxNotice;
        SocketEventHandle.getInstance().gameReadyNotice += gameReadyNotice;
        SocketEventHandle.getInstance().StartGameNotice += startGame;
        SocketEventHandle.getInstance().PDK_putOutCardCallBack += putOutCardCallback;
        SocketEventHandle.getInstance().serviceErrorNotice += serviceErrorNotice;
        SocketEventHandle.getInstance().PDK_ChiBuQiCallBack += ChiBuQiCallBack;
        SocketEventHandle.getInstance().returnGameResponse += returnGameResponse;
        SocketEventHandle.getInstance().dissoliveRoomResponse += dissoliveRoomResponse;
        SocketEventHandle.getInstance().messageBoxNotice += messageBoxNotice;
        SocketEventHandle.getInstance().offlineNotice += offlineNotice;
        SocketEventHandle.getInstance().onlineNotice += onlineNotice;
        SocketEventHandle.getInstance().HupaiCallBack += hupaiCallBack;
        SocketEventHandle.getInstance().FinalGameOverCallBack += finalGameOverCallBack;
        CommonEvent.getInstance().closeGamePanel += exitOrDissoliveRoom;
        SocketEventHandle.getInstance().micInputNoticeOther += micInputNotice;

        SocketEventHandle.getInstance().DN_qiangResponse += qiangResponse_DN;
        SocketEventHandle.getInstance().DN_zhuangResponse += zhuangResponse_DN;
        SocketEventHandle.getInstance().DN_xiaZhuResponse += xiaZhuResponse_DN;
        SocketEventHandle.getInstance().DN_niuResponse += niuResponse_DN;
        SocketEventHandle.getInstance().DN_niuNoticeResponse += niuNoticeResponse_DN;
    }

    private void removeListener()
    {
        SocketEventHandle.getInstance().outRoomCallback -= outRoomCallbak;
        SocketEventHandle.getInstance().otherUserJointRoomCallBack -= otherUserJointRoom;
        SocketEventHandle.getInstance().messageBoxNotice -= messageBoxNotice;
        SocketEventHandle.getInstance().gameReadyNotice -= gameReadyNotice;
        SocketEventHandle.getInstance().StartGameNotice -= startGame;
        SocketEventHandle.getInstance().PDK_putOutCardCallBack -= putOutCardCallback;
        SocketEventHandle.getInstance().serviceErrorNotice -= serviceErrorNotice;
        SocketEventHandle.getInstance().PDK_ChiBuQiCallBack -= ChiBuQiCallBack;
        SocketEventHandle.getInstance().returnGameResponse -= returnGameResponse;
        SocketEventHandle.getInstance().dissoliveRoomResponse -= dissoliveRoomResponse;
        SocketEventHandle.getInstance().messageBoxNotice -= messageBoxNotice;
        SocketEventHandle.getInstance().offlineNotice -= offlineNotice;
        SocketEventHandle.getInstance().onlineNotice -= onlineNotice;
        SocketEventHandle.getInstance().HupaiCallBack -= hupaiCallBack;
        SocketEventHandle.getInstance().FinalGameOverCallBack -= finalGameOverCallBack;
        CommonEvent.getInstance().closeGamePanel -= exitOrDissoliveRoom;
        SocketEventHandle.getInstance().micInputNoticeOther -= micInputNotice;

        SocketEventHandle.getInstance().DN_qiangResponse -= qiangResponse_DN;
        SocketEventHandle.getInstance().DN_zhuangResponse -= zhuangResponse_DN;
        SocketEventHandle.getInstance().DN_xiaZhuResponse -= xiaZhuResponse_DN;
        SocketEventHandle.getInstance().DN_niuResponse -= niuResponse_DN;
        SocketEventHandle.getInstance().DN_niuNoticeResponse -= niuNoticeResponse_DN;
    }

    public void micInputNotice(int sendUUid)
    {
        if (sendUUid > 0)
        {
            for (int i = 0; i < playerItems.Count; i++)
            {
                if (playerItems[i].getUuid() != -1)
                {
                    if (sendUUid == playerItems[i].getUuid())
                    {
                        playerItems[i].showChatAction();
                    }
                }
            }
        }
    }

    public void messageBoxNotice(ClientResponse response)
    {
        string[] arr = response.message.Split(new char[1] { '|' });
        if (int.Parse(arr[0]) == 1)
        {
            int uuid = int.Parse(arr[2]);
            int myIndex = getMyIndexFromList();
            int curAvaIndex = getIndex(uuid);
            int seatIndex = curAvaIndex - myIndex;
            if (seatIndex < 0)
            {
                // Change by Shise
                seatIndex = GlobalDataScript.roomVo.playerAmounts + seatIndex;
                // End
            }

            int code = int.Parse(arr[1]);
            //传输性别  大于3000为女
            if (code > 3000)
            {
                code = code - 3000;
                playerItems[seatIndex].showChatMessage(code);
                SoundCtrl.getInstance().playMessageBoxSound(code, 0, 3);
            }
            else
            {
                code = code - 1000;
                playerItems[seatIndex].showChatMessage(code);
                SoundCtrl.getInstance().playMessageBoxSound(code, 1, 3);
            }
        }
        else if (int.Parse(arr[0]) == 2)
        {
            int uuid = int.Parse(arr[2]);
            int myIndex = getMyIndexFromList();
            int curAvaIndex = getIndex(uuid);
            int seatIndex = curAvaIndex - myIndex;
            if (seatIndex < 0)
            {
                // Change by Shise
                seatIndex = GlobalDataScript.roomVo.playerAmounts + seatIndex;
                // End
            }
            playerItems[seatIndex].showChatMessage(arr[1]);
        }
        else if (int.Parse(arr[0]) == 3)
        {
            int uuid = int.Parse(arr[2]);
            int myIndex = getMyIndexFromList();
            int curAvaIndex = getIndex(uuid);
            int seatIndex = curAvaIndex - myIndex;
            if (seatIndex < 0)
            {
                // Change by Shise
                seatIndex = GlobalDataScript.roomVo.playerAmounts + seatIndex;
                // End
            }
            int code = int.Parse(arr[1]);
            int sex = 0;
            //传输性别  大于3000为女
            if (code > 3000)
            {
                code = code - 3000;
                sex = 0;
            }
            else
            {
                code = code - 1000;
                sex = 1;
            }
            if (!(GlobalDataScript.roomVo.gameType == 3))
                SoundCtrl.getInstance().playMessageBoxSound(code, sex, 2);
            playerItems[seatIndex].showBiaoQing(bqScript.getBiaoqing(code - 1));//int.Parse (arr [1])
        }
        else if (int.Parse(arr[0]) == 4)
        {
            int srcuuid = int.Parse(arr[2]);
            int destuuid = int.Parse(arr[3]);
            int myIndex = getMyIndexFromList();
            int srcAvaIndex = getIndex(srcuuid);
            int destAvaIndex = getIndex(destuuid);

            int destSeatIndex = destAvaIndex - myIndex;
            if (destSeatIndex < 0)
            {
                // Change by Shise
                destSeatIndex = GlobalDataScript.roomVo.playerAmounts + destSeatIndex;
                // End
            }
            int srcSeatIndex = srcAvaIndex - myIndex;
            if (srcSeatIndex < 0)
            {
                // Change by Shise
                srcSeatIndex = GlobalDataScript.roomVo.playerAmounts + srcSeatIndex;
                // End
            }
            //XXX
            playerItems[destSeatIndex].showMfbq(srcSeatIndex, destSeatIndex, arr[1]);
        }


    }
    private void finalGameOverCallBack(ClientResponse response)
    {
        timer = 0;
        //GlobalDataScript.surplusTimes = 0; //0829@##
        GlobalDataScript.finalGameEndVo = JsonMapper.ToObject<FinalGameEndVo>(response.message);
        Invoke("DNGameOver", 3);
    }

    private void DNGameOver()
    {
        GameObject obj = PrefabManage.loadPerfab("Prefab/Panel_Game_Over");
        obj.GetComponent<GameOverScript>().setDisplayContent_pdk(1, avatarList, null, null, 3);
        GlobalDataScript.singalGameOverList.Add(obj);
        avatarList[bankerId].main = false;
    }

    /**
	 * 赢牌请求回调
	 */

    public bool reEnterFirst = true;
    int minIndex = 0;
    int maxIndex = 0;
    private void hupaiCallBack(ClientResponse response)
    {
        currentIndex = -1;
        timer = -1;

        GlobalDataScript.hupaiResponseVo = new HupaiResponseVo();
        GlobalDataScript.hupaiResponseVo = JsonMapper.ToObject<HupaiResponseVo>(response.message);
        if (reEnterFirst)
        {
            reEnterFirst = false;
            if (GlobalDataScript.reEnterRoomData != null && GlobalDataScript.loginResponseData.stateForNiu < 2)
            {
                return;
            }
        }

        GlobalDataScript.loginResponseData.stateForNiu = 8;
        SetTip();

        if (GlobalDataScript.hupaiResponseVo == null)
            return;


        for (int i = 0; i < GlobalDataScript.count_Players_DN; i++)
        {
            int fen = avatarList[i].fen;
            int index = getIndexByDir(getDirection(i));

            if (fen < 0)
                minIndex = index;
            else if (fen > 0)
                maxIndex = index;

            fen += int.Parse(playerItems[index].scoreText.text);
            avatarList[i].scores = fen;
            playerItems[index].scoreText.text = fen + "";

            avatarList[i].fen = 0;//0830 @##
        }
        if (GlobalDataScript.goldType)
            GlobalDataScript.playCountForGoldType++;
        if (GlobalDataScript.hupaiResponseVo.type == "0")
        {


            isFinish = true;

            Invoke("openGameOverPanelSignal", 3);
            // Add by Duyao
            //for (int i = 0; i < playerItems.Count; i++)
            //{
            //    if (playerItems[i].getCurZhu() > 0)
            //    {
            //        GameObject obj = Instantiate(Resources.Load("prefab/dzpk/winZhuCoinMove")) as GameObject;
            //        obj.transform.SetParent(gameObject.transform);
            //        obj.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
            //        obj.transform.localPosition = getZhuCoinPosition(i);
            //        Vector3 dest = new Vector3(0, 148, 0);
            //        StartCoroutine(ZhuCoinMoveToPosition(obj, dest));
            //    }
            //}
            int bankerindex = getIndexByDir(getDirection(bankerId));
            Vector3 bankerpos = playerItems[bankerindex].transform.localPosition;
            foreach (HupaiResponseItem item in GlobalDataScript.hupaiResponseVo.avatarList)
            {
                int itemseat = getIndexByDir(getDirection(getIndex(item.uuid)));
                if (itemseat != bankerindex)
                {
                    Vector3 itempos = playerItems[itemseat].transform.localPosition; ;
                    if (item.totalScore > 0)
                    {
                        dnGoldMove(bankerpos, itempos);
                    }
                    else
                    {
                        dnGoldMove(itempos, bankerpos);
                    }
                }
            }

        }
        else
        {
            Invoke("openGameOverPanelSignal", 1);
        }
    }
    IEnumerator ZhuCoinMoveToPosition(GameObject obj, Vector3 dest)
    {
        while (obj.transform.localPosition != dest)
        {
            obj.transform.localPosition = Vector3.MoveTowards(obj.transform.localPosition, dest, 700 * Time.deltaTime);
            yield return 0;
        }
        Destroy(obj);
    }
    private Vector3 getZhuCoinPosition(int i)
    {
        Vector3 vec = new Vector3(0, 0, 0);
        switch (i)
        {
            case 0:
                vec = new Vector3(-100, -155, 0);
                break;
            case 1:
                vec = new Vector3(250, -155, 0);
                break;
            case 2:
                vec = new Vector3(415, -115, 0);
                break;
            case 3:
                vec = new Vector3(450, 50, 0);
                break;
            case 4:
                vec = new Vector3(320, 145, 0);
                break;
            case 5:
                vec = new Vector3(90, 145, 0);
                break;
            case 6:
                vec = new Vector3(-330, 145, 0);
                break;
            case 7:
                vec = new Vector3(-445, 20, 0);
                break;
            case 8:
                vec = new Vector3(-395, -130, 0);
                break;
            case 9:
                vec = new Vector3(-295, -155, 0);
                break;
            default:
                break;
        }
        return vec;
    }


    private void openGameOverPanelSignal()
    {//单局结算
        // mark need move
        cuoPai_Button.gameObject.SetActive(false);
        
        myCardList.Clear();
        if (GlobalDataScript.goldType)
        {
            int gold = 10;
            if (GlobalDataScript.roomVo.gameType == 3)
                gold = 5;
            gold = int.Parse(goldText.text) - gold;
            goldText.text = gold + "";
        }
        if (GlobalDataScript.hupaiResponseVo.type == "0")
        {
            //GameObject obj = PrefabManage.loadPerfab("Prefab/Panel_DN_Current_Item");
            //obj.GetComponent<DNOverScript>().setUI(gameObject);
           
        }
        else
        {
            /**
			setAllPlayerDNImgVisbleToFalse();

			//GlobalDataScript.singalGameOver = PrefabManage.loadPerfab("prefab/Panel_Game_Over");


			getDirection (bankerId);
			playerItems [curDirIndex].setbankImgEnable (false);
			if (handerCardList != null && handerCardList.Count > 0 && handerCardList [0].Count > 0) {
				for (int i = 0; i < handerCardList.Count; i++) {
					List<GameObject> hands = handerCardList [i];
					for (int j = 0; j < hands.Count; j++) {
						//hands [j].GetComponent<pdkCardScript> ().onDragSelect -= dragSelect;
						//hands [j].GetComponent<pdkCardScript> ().onCardSelect -= cardSelect;
						Destroy (hands [j]);
					}
				}
			}

			initPanel ();
			*/
            //GameObject obj = PrefabManage.loadPerfab("Prefab/Panel_Game_Over");
            //obj.GetComponent<GameOverScript>().setDisplayContent_pdk(1, avatarList, null, null, 3);
            //GlobalDataScript.singalGameOverList.Add(obj);
            //avatarList[bankerId].main = false;
        }
        setAllPlayerDNImgVisbleToFalse();
    }


    private void dnGoldMove(Vector3 forv3, Vector3 tov3)
    {
        //GameObject objgold = Instantiate(Resources.Load("Prefab/dn/dngold")) as GameObject;
        GameObject objgold = PrefabUtils.AddChild(gameObject, Resources.Load("Prefab/dngold") as GameObject);
        dnGold hd = objgold.GetComponent<dnGold>();
        hd.move(forv3, tov3);
    }
    private Vector3 getPlayerPosition(int i)
    {      // L-
        Vector3 vec = new Vector3(0, 0, 0);
        switch (i)
        {
            case 0:
                vec = new Vector3(-145, -235, 0);

                break;
            case 1:
                vec = new Vector3(205, -235, 0);
                break;
            case 2:
                vec = new Vector3(500, -210, 0);
                break;
            case 3:
                vec = new Vector3(590, 70, 0);
                break;
            case 4:
                vec = new Vector3(380, 265, 0);
                break;
            case 5:
                vec = new Vector3(100, 265, 0);
                break;
            case 6:
                vec = new Vector3(-445, 265, 0);
                break;
            case 7:
                vec = new Vector3(-590, 70, 0);
                break;
            case 8:
                vec = new Vector3(-580, -205, 0);
                break;
            case 9:
                vec = new Vector3(-390, -240, 0);
                break;
            default:
                break;
        }
        return vec;
    }



    IEnumerator winZhuMoveToPlayer(GameObject obj, Vector3 dest, int seat)
    {
        while (obj.transform.localPosition != dest)
        {
            obj.transform.localPosition = Vector3.MoveTowards(obj.transform.localPosition, dest, 900 * Time.deltaTime);
            yield return 0;
        }
        Destroy(obj);
    }
    // End


    public void setAllPlayerDNImgVisbleToFalse()
    {
        initPanel();

        //if (GlobalDataScript.surplusTimes == 0)
        //{
        //    GlobalDataScript.hupaiResponseVo.type = "1";
        //    openGameOverPanelSignal();
        //}
    }

    void initPanel()
    {
        clean();
        readyBtn.gameObject.SetActive(true);

    }

    /// <summary>
    /// 清理桌面
    /// </summary>
    public void clean()
    {
        //cleanArrayList (handerCardList);

        //去除打出的牌
        /**
		for (int i = 0; i < 5; i++) {
			cleanChuPai (i);
		}
		*/

        if (mineList != null)
        {
            mineList.Clear();
        }

    }

    private void cleanArrayList(List<List<GameObject>> list)
    {
        if (list != null)
        {
            while (list.Count > 0)
            {
                List<GameObject> tempList = list[0];
                list.RemoveAt(0);
                cleanList(tempList);
            }
        }
    }

    private void cleanList(List<GameObject> tempList)
    {
        if (tempList != null)
        {
            while (tempList.Count > 0)
            {
                GameObject temp = tempList[0];
                tempList.RemoveAt(0);
                GameObject.Destroy(temp);
            }
        }
    }
    /**用户离线回调**/
    public void offlineNotice(ClientResponse response)
    {
        int uuid = int.Parse(response.message);
        int index = getIndex(uuid);
        string dirstr = getDirection(index);
        switch (dirstr)
        {
            case DirectionEnum.Bottom:
                playerItems[0].GetComponent<PlayerItemScript>().setPlayerOffline();
                break;
            case DirectionEnum.Right:
                playerItems[1].GetComponent<PlayerItemScript>().setPlayerOffline();
                break;
            case DirectionEnum.TopRight:
                playerItems[2].GetComponent<PlayerItemScript>().setPlayerOffline();
                break;
            case DirectionEnum.TopLeft:
                playerItems[3].GetComponent<PlayerItemScript>().setPlayerOffline();
                break;
            case DirectionEnum.Left:
                playerItems[4].GetComponent<PlayerItemScript>().setPlayerOffline();
                break;
            case DirectionEnum.LeftBottom:
                playerItems[5].GetComponent<PlayerItemScript>().setPlayerOffline();
                break;
            case DirectionEnum.RightBottom:
                playerItems[6].GetComponent<PlayerItemScript>().setPlayerOffline();
                break;
            case DirectionEnum.MoreOne:
                playerItems[7].GetComponent<PlayerItemScript>().setPlayerOffline();
                break;
            case DirectionEnum.MoreTwo:
                playerItems[8].GetComponent<PlayerItemScript>().setPlayerOffline();
                break;
            case DirectionEnum.Top:
                playerItems[9].GetComponent<PlayerItemScript>().setPlayerOffline();
                break;
        }

        //申请解散房间过程中，有人掉线，直接不能解散房间
        /**
		if (GlobalDataScript.isonApplayExitRoomstatus) {
			if (dissoDialog != null) {
				dissoDialog.GetComponent<VoteScript> ().removeListener ();
				Destroy (dissoDialog.GetComponent<VoteScript> ());
				Destroy (dissoDialog);
			}
			TipsManagerScript.getInstance ().setTips ("由于" + avatarList [index].account.nickname + "离线，系统不能解散房间。");

		}
		*/
    }

    /**用户上线提醒**/
    public void onlineNotice(ClientResponse response)
    {
        int uuid = int.Parse(response.message);
        int index = getIndex(uuid);
        string dirstr = getDirection(index);
        switch (dirstr)
        {
            case DirectionEnum.Bottom:
                playerItems[0].GetComponent<PlayerItemScript>().setPlayerOffline();
                break;
            case DirectionEnum.Right:
                playerItems[1].GetComponent<PlayerItemScript>().setPlayerOffline();
                break;
            case DirectionEnum.TopRight:
                playerItems[2].GetComponent<PlayerItemScript>().setPlayerOffline();
                break;
            case DirectionEnum.TopLeft:
                playerItems[3].GetComponent<PlayerItemScript>().setPlayerOffline();
                break;
            case DirectionEnum.Left:
                playerItems[4].GetComponent<PlayerItemScript>().setPlayerOffline();
                break;
            case DirectionEnum.LeftBottom:
                playerItems[5].GetComponent<PlayerItemScript>().setPlayerOffline();
                break;
            case DirectionEnum.RightBottom:
                playerItems[6].GetComponent<PlayerItemScript>().setPlayerOffline();
                break;
            case DirectionEnum.MoreOne:
                playerItems[7].GetComponent<PlayerItemScript>().setPlayerOffline();
                break;
            case DirectionEnum.MoreTwo:
                playerItems[8].GetComponent<PlayerItemScript>().setPlayerOffline();
                break;
            case DirectionEnum.Top:
                playerItems[9].GetComponent<PlayerItemScript>().setPlayerOffline();
                break;
        }
    }
    /***
	 * 申请解散房间回调
	 */
    GameObject dissoDialog;
    private string dissoliveRoomType = "0";

    public void dissoliveRoomResponse(ClientResponse response)
    {
        MyDebug.Log("dissoliveRoomResponse" + response.message);
        DissoliveRoomResponseVo dissoliveRoomResponseVo = JsonMapper.ToObject<DissoliveRoomResponseVo>(response.message);
        string plyerName = dissoliveRoomResponseVo.accountName;
        if (dissoliveRoomResponseVo.type == "0")
        {
            GlobalDataScript.isonApplayExitRoomstatus = true;
            dissoliveRoomType = "1";
            dissoDialog = PrefabManage.loadPerfab("Prefab/Panel_Apply_Exit");
            dissoDialog.GetComponent<VoteScript>().iniUI(plyerName, avatarList);
        }
        else if (dissoliveRoomResponseVo.type == "3")
        {

            GlobalDataScript.isonApplayExitRoomstatus = false;
            if (dissoDialog != null)
            {
                GlobalDataScript.isOverByPlayer = true;
                dissoDialog.GetComponent<VoteScript>().removeListener();
                Destroy(dissoDialog.GetComponent<VoteScript>());
                Destroy(dissoDialog);
            }
        }
    }

    /// <summary>
    /// 重新开始计时
    /// </summary>
    void UpdateTimeReStart()
    {
        // Change by Shise  timer = 20
        timer = 5;
        // End
    }

    public void returnGameResponse(ClientResponse response)
    {
        JsonData json = JsonMapper.ToObject(response.message);
        int gameRound = int.Parse(json["gameRound"].ToString());
        GlobalDataScript.surplusTimes = gameRound;
        if (GlobalDataScript.goldType)
        {
            GlobalDataScript.playCountForGoldType = GlobalDataScript.roomVo.roundNumber - gameRound;
            Number.text = GlobalDataScript.playCountForGoldType + "";
        }
        else
            Number.text = GlobalDataScript.surplusTimes + "/" + GlobalDataScript.roomVo.roundNumber;

        if (GlobalDataScript.loginResponseData.stateForNiu > 2)
        {
            for (int i = 0; i < GlobalDataScript.count_Players_DN; i++)
            {
                AvatarVO avatarVO = avatarList[i];
                if (avatarVO.qiangForNiu > -1)
                {
                    hasQiangs++;
                }
            }
            if (hasQiangs < GlobalDataScript.count_Players_DN)
            {
            }

            for (int i = 0; i < GlobalDataScript.count_Players_DN; i++)
            {
                AvatarVO avatarVO = avatarList[i];
                if (avatarVO.zhuForNiu > 0)
                {
                    playerItems[getIndexByDir(getDirection(i))].setZhuImg(avatarVO.zhuForNiu);
                    hasZhus++;
                }
            }
        }


        if (hasZhus == GlobalDataScript.count_Players_DN - 1)
        {
            if (GlobalDataScript.loginResponseData.stateForNiu == 6)
            {
                avatarList[getMyIndexFromList()].stateForNiu = 7;
                GlobalDataScript.loginResponseData.stateForNiu = 7;
                SetTip();
            }
        }
        if (GlobalDataScript.loginResponseData.stateForNiu == 3 && GlobalDataScript.loginResponseData.qiangForNiu > -1)
        {
            GlobalDataScript.loginResponseData.stateForNiu = 4;
        }

        bool isNewTurn = (bool)json["isNewTurn"];
        if (!isNewTurn)
        {//不是新的一轮必须显示打牌数据
            if (GlobalDataScript.loginResponseData.stateForNiu == 2)
            {
                float time = GlobalDataScript.roomVo.ming ? 0.7f : 0f;
                Invoke("ShowQiang", time);
                Invoke("ShowQiang", time);
            }
            if (GlobalDataScript.loginResponseData.stateForNiu == 3)
            {
                panel_Qiang.gameObject.SetActive(true);
                panel_Zhus.gameObject.SetActive(false);
            }
            else if (GlobalDataScript.loginResponseData.stateForNiu == 5)
            {
                panel_Qiang.gameObject.SetActive(false);
                panel_Zhus.gameObject.SetActive(true);
            }
            else if (GlobalDataScript.loginResponseData.stateForNiu == 6)
            {

            }
            else if (GlobalDataScript.loginResponseData.stateForNiu >= 7)
            {

                panel_Qiang.gameObject.SetActive(false);
                panel_Zhus.gameObject.SetActive(false);

                FanPai(0);

                for (int i = 0; i < GlobalDataScript.count_Players_DN; i++)
                {
                    AvatarVO avatarVO = avatarList[i];
                    int selfIndex = getIndexByDir(getDirection(i));
                    if (avatarVO.noticedForNiu)
                    {
                        if (selfIndex != 0)
                        {
                            FanPai(selfIndex);
                            panel_Niu.gameObject.SetActive(true);
                            // Change by Shise   timer = 11;
                            timer = 5;
                            // End mark
                        }
                        playerItems[selfIndex].setNiuImg(avatarVO.niu);
                        if (selfIndex == 0)
                            panel_Niu.gameObject.SetActive(false);
                    }
                    else
                    {
                        if (selfIndex == 0)
                        {
                            FanPai(selfIndex);
                            panel_Niu.gameObject.SetActive(true);
                            // Change by Shise  timer = 11;
                            timer = 5;
                            // End
                        }
                    }
                    if (GlobalDataScript.loginResponseData.stateForNiu == 8)
                    {
                        panel_Niu.gameObject.SetActive(false);
                        timer = -1;
                    }
                }
            }
        }
        else
        {
            lastCard = null;
        }

    }

    public void displayChupai(string chupai)
    {
        try
        {
            string[] chupaiArray = chupai.Split(';');
            for (int k = 0; k < chupaiArray.Length; k++)
            {
                string str = chupaiArray[k];
                if (str == null || str.Length == 0)
                    continue;
                int avatarIndex = int.Parse(str.Split(':')[0]);
                string chuStr = str.Split(':')[1];

                int outIndex = getIndexByDir(getDirection(avatarIndex));

                //去除上轮打出的牌
                cleanChuPai(outIndex);

                //显示打出的牌
                string[] card = chuStr.Split(',');
                if (card.Length == 1 && int.Parse(card[0]) == -1)
                {
                    playerItems[outIndex].showYaoBuQi(true);
                }
                else
                {
                    List<int> pai = new List<int>();
                    int count = 0;
                    for (int i = 0; i < card.Length; i++)
                    {
                        GameObject gob = Instantiate(Resources.Load("prefab/pdk/HandCard_s")) as GameObject;
                        if (gob != null)
                        {
                            gob.transform.SetParent(ThrowTransformList[outIndex]);
                            gob.GetComponent<pdkCardScript>().setPoint(int.Parse(card[i]));
                            gob.transform.localScale = new Vector3(1f, 1f, 1);
                            gob.transform.localPosition = new Vector3(-200 + count * 25, 0, 0);
                            count++;
                            pai.Add(int.Parse(card[i]));
                        }
                    }

                    lastCard = ToArray(pai);
                }
            }
        }
        catch (Exception ex)
        {
            MyDebug.Log(ex.ToString());
        }

    }

    public void ChiBuQiCallBack(ClientResponse response)
    {
        cleanClock();
        UpdateTimeReStart();
        JsonData json = JsonMapper.ToObject(response.message);
        int type = (int)json["type"];
        if (type == 0)
        {//吃不起
            int AvatarIndex = (int)json["AvatarIndex"];
            int outIndex = getIndexByDir(getDirection(AvatarIndex));

            //去除上轮打出的牌
            cleanChuPai(outIndex, true);

            if (outIndex == 0)
            {//自己
                currentIndex = 1;
            }
            else
            {//别人
                if (outIndex == 2)
                {//上家
                    currentIndex = 0;
                }
                else if (outIndex == 1)
                {//下家
                    currentIndex = 2;
                }
            }

            cleanChuPai(currentIndex);
        }
        else if (type == 1)
        {//接上家 继续出牌的提示
            btnActionScript.showBtn();

            //去除上轮打出的牌
            cleanChuPai(0);

            TiShi();//智能自动弹出能出的牌
        }
        else if (type == 2)
        {//都吃不起，开始新一轮出牌的提示
            lastCard = null;

            int AvatarIndex = (int)json["AvatarIndex"];
            int outIndex = getIndexByDir(getDirection(AvatarIndex));

            if (outIndex == 0)
            {
                btnActionScript.showBtn();
            }

            //去除上轮打出的牌
            for (int i = 0; i < 3; i++)
            {
                cleanChuPai(i);
            }
        }
    }

    public void serviceErrorNotice(ClientResponse response)
    {
        TipsManagerScript.getInstance().setTips(response.message);
    }

    public void TiShi()
    {
        List<int> pai = new List<int>();
        for (int i = handerCardList[0].Count - 1; i >= 0; i--)
        {
            GameObject gob = handerCardList[0][i];
            if (gob != null)
            {
                pai.Add(gob.GetComponent<pdkCardScript>().getPoint());
            }
        }

        if (lastCard == null || lastCard.Length == 0)
        {//庄家第一手出牌

        }
        else
        {//吃上家或上上家的牌
            showTiShi(lastCard, ToArray(pai));
        }
    }

    private bool isClickTiShi = false;
    private int tishiIndex = 0;
    List<int[]> result = new List<int[]>();

    public void showTiShi(int[] chuCard_old, int[] myCardArray_old)
    {
        cardReset();//牌全部先复位
        int[] chuCard = new int[chuCard_old.Length];
        for (int i = 0; i < chuCard_old.Length; i++)
        {
            chuCard[i] = chuCard_old[i] % 13;
        }
        // 从小到大排序
        Array.Sort(chuCard);

        int[] myCardArray = new int[myCardArray_old.Length];
        for (int i = 0; i < myCardArray_old.Length; i++)
        {
            myCardArray[i] = myCardArray_old[i] % 13;
        }

        // 从小到大排序
        Array.Sort(myCardArray);

        if (!isClickTiShi)
        {
            result = returnResult(chuCard, myCardArray);//返回所有大于当前出牌的链表
            tishiIndex = 0;
            isClickTiShi = true;
        }

        if (result == null || result.Count == 0)
            return;

        pdkCardType pct = new pdkCardType();
        pdkCardType.CARDTYPE type1 = pct.getType(chuCard_old);

        if (type1 == pdkCardType.CARDTYPE.c1
            || type1 == pdkCardType.CARDTYPE.c2
            || type1 == pdkCardType.CARDTYPE.c4)
        {//单牌、双牌、炸弹

            int[] re = result[tishiIndex];
            int count = 0;
            for (int i = handerCardList[0].Count - 1; i >= 0; i--)
            {
                GameObject gob = handerCardList[0][i];
                if (gob != null)
                {
                    pdkCardScript pcs = gob.GetComponent<pdkCardScript>();
                    if (pcs.getPoint() % 13 == re[1])
                    {
                        gob.transform.localPosition = new Vector3(gob.transform.localPosition.x, 40);
                        pcs.selected = true;
                        count++;
                        if (count >= re[0])
                            break;
                    }
                }
            }
        }
        else if (type1 == pdkCardType.CARDTYPE.c311)
        {//3带11
            int[] re = result[tishiIndex];

            int count = 0;
            for (int i = handerCardList[0].Count - 1; i >= 0; i--)
            {
                GameObject gob = handerCardList[0][i];
                if (gob != null)
                {
                    pdkCardScript pcs = gob.GetComponent<pdkCardScript>();
                    if (pcs.getPoint() % 13 == re[1])
                    {
                        gob.transform.localPosition = new Vector3(gob.transform.localPosition.x, 40);
                        pcs.selected = true;
                        count++;
                        if (count >= re[0])
                            break;
                    }
                }
            }
            if (re[0] == 3)
            {
                //要2个杂牌起立
                count = 0;
                for (int i = handerCardList[0].Count - 1; i >= 0; i--)
                {
                    GameObject gob = handerCardList[0][i];
                    if (gob != null)
                    {
                        pdkCardScript pcs = gob.GetComponent<pdkCardScript>();
                        if (!pcs.selected)
                        {
                            gob.transform.localPosition = new Vector3(gob.transform.localPosition.x, 40);
                            pcs.selected = true;
                            count++;
                            if (count >= 2)
                                break;
                        }
                    }
                }
            }
        }
        else if (type1 == pdkCardType.CARDTYPE.c12345)
        {//顺子
            int[] re = result[tishiIndex];
            if (re[0] == 4)
            {//炸弹
                int count = 0;
                for (int i = handerCardList[0].Count - 1; i >= 0; i--)
                {
                    GameObject gob = handerCardList[0][i];
                    if (gob != null)
                    {
                        pdkCardScript pcs = gob.GetComponent<pdkCardScript>();
                        if (pcs.getPoint() % 13 == re[0])
                        {
                            gob.transform.localPosition = new Vector3(gob.transform.localPosition.x, 40);
                            pcs.selected = true;
                            count++;
                            if (count >= re[0])
                                break;
                        }
                    }
                }
            }
            else
            {//顺子
                for (int j = 1; j < re.Length; j++)
                {
                    for (int i = handerCardList[0].Count - 1; i >= 0; i--)
                    {
                        GameObject gob = handerCardList[0][i];
                        if (gob != null)
                        {
                            pdkCardScript pcs = gob.GetComponent<pdkCardScript>();
                            if (pcs.getPoint() % 13 == re[j])
                            {
                                gob.transform.localPosition = new Vector3(gob.transform.localPosition.x, 40);
                                pcs.selected = true;
                                break;
                            }
                        }
                    }
                }
            }
        }
        else if (type1 == pdkCardType.CARDTYPE.c112233)
        {//连对
            int[] re = result[tishiIndex];

            if (re[0] == 4 && re.Length == 2)
            {//炸弹
                int count = 0;
                for (int i = handerCardList[0].Count - 1; i >= 0; i--)
                {
                    GameObject gob = handerCardList[0][i];
                    if (gob != null)
                    {
                        pdkCardScript pcs = gob.GetComponent<pdkCardScript>();
                        if (pcs.getPoint() % 13 == re[0])
                        {
                            gob.transform.localPosition = new Vector3(gob.transform.localPosition.x, 40);
                            pcs.selected = true;
                            count++;
                            if (count >= re[0])
                                break;
                        }
                    }
                }
            }
            else
            {//连对
                for (int j = 1; j < re.Length; j++)
                {
                    int count = 0;
                    for (int i = handerCardList[0].Count - 1; i >= 0; i--)
                    {
                        GameObject gob = handerCardList[0][i];
                        if (gob != null)
                        {
                            pdkCardScript pcs = gob.GetComponent<pdkCardScript>();
                            if (pcs.getPoint() % 13 == re[j])
                            {
                                gob.transform.localPosition = new Vector3(gob.transform.localPosition.x, 40);
                                pcs.selected = true;
                                if (++count >= 2)
                                    break;
                            }
                        }
                    }
                }
            }
        }
        else if (type1 == pdkCardType.CARDTYPE.c1112223456)
        {//飞机带翅膀
            int[] re = result[tishiIndex];
            if (re[0] == 4 && re.Length == 2)
            {//炸弹
                int count = 0;
                for (int i = handerCardList[0].Count - 1; i >= 0; i--)
                {
                    GameObject gob = handerCardList[0][i];
                    if (gob != null)
                    {
                        pdkCardScript pcs = gob.GetComponent<pdkCardScript>();
                        if (pcs.getPoint() % 13 == re[0])
                        {
                            gob.transform.localPosition = new Vector3(gob.transform.localPosition.x, 40);
                            pcs.selected = true;
                            count++;
                            if (count >= re[0])
                                break;
                        }
                    }
                }
            }
            else
            {//飞机
                for (int j = 1; j < re.Length; j++)
                {
                    int count = 0;
                    for (int i = handerCardList[0].Count - 1; i >= 0; i--)
                    {
                        GameObject gob = handerCardList[0][i];
                        if (gob != null)
                        {
                            pdkCardScript pcs = gob.GetComponent<pdkCardScript>();
                            if (pcs.getPoint() % 13 == re[j])
                            {
                                gob.transform.localPosition = new Vector3(gob.transform.localPosition.x, 40);
                                pcs.selected = true;
                                if (++count >= 3)
                                    break;
                            }
                        }
                    }
                }

                //起立几个杂牌
                int counts = 0;
                for (int i = handerCardList[0].Count - 1; i >= 0; i--)
                {
                    GameObject gob = handerCardList[0][i];
                    if (gob != null)
                    {
                        pdkCardScript pcs = gob.GetComponent<pdkCardScript>();
                        if (!pcs.selected)
                        {
                            gob.transform.localPosition = new Vector3(gob.transform.localPosition.x, 40);
                            pcs.selected = true;
                            if (++counts >= (re.Length - 1) * 2)
                                break;
                        }
                    }
                }
            }
        }

        isCanChu();

        tishiIndex++;
        if (tishiIndex >= result.Count)
            tishiIndex = 0;
    }

    //返回所有可以大过 当前所出牌的链表
    public List<int[]> returnResult(int[] chuCard, int[] myCardArray)
    {
        List<int[]> result = new List<int[]>();

        pdkCardType pct = new pdkCardType();
        pdkCardType.CARDTYPE type1 = pct.getType(chuCard);

        List<int>[] a = pct.getCount(chuCard);
        List<int>[] b = pct.getCount(myCardArray);

        int length = myCardArray.Length;

        if (type1 == pdkCardType.CARDTYPE.c1)
        {//单牌

            //单张
            for (int k = 0; k < b[0].Count; k++)
            {
                int[] re = new int[2];
                if (b[0][k] > chuCard[0])
                {
                    re[1] = b[0][k];
                    re[0] = 1;
                    result.Add(re);
                }
            }

            //双张
            for (int k = 0; k < b[1].Count; k++)
            {
                int[] re = new int[2];
                if (b[1][k] > chuCard[0])
                {
                    re[1] = b[1][k];
                    re[0] = 1;
                    result.Add(re);
                }
            }

            //三张
            for (int k = 0; k < b[2].Count; k++)
            {
                int[] re = new int[2];
                if (b[2][k] > chuCard[0])
                {
                    re[1] = b[2][k];
                    re[0] = 1;
                    result.Add(re);
                }
            }

            //4张
            for (int k = 0; k < b[3].Count; k++)
            {
                int[] re = new int[2];
                if (b[3][k] > chuCard[0])
                {
                    re[1] = b[3][k];
                    re[0] = 1;
                    result.Add(re);
                }
            }

            //炸弹
            for (int k = 0; k < b[3].Count; k++)
            {
                int[] re = new int[2];
                re[1] = b[3][k];
                re[0] = 4;
                result.Add(re);
            }
        }
        else if (type1 == pdkCardType.CARDTYPE.c2)
        {// 对子
         //双张
            for (int k = 0; k < b[1].Count; k++)
            {
                int[] re = new int[2];
                if (b[1][k] > chuCard[0])
                {
                    re[1] = b[1][k];
                    re[0] = 2;
                    result.Add(re);
                }
            }

            //三张
            for (int k = 0; k < b[2].Count; k++)
            {
                int[] re = new int[2];
                if (b[2][k] > chuCard[0])
                {
                    re[1] = b[2][k];
                    re[0] = 2;
                    result.Add(re);
                }
            }

            //4张
            for (int k = 0; k < b[3].Count; k++)
            {
                int[] re = new int[2];
                if (b[3][k] > chuCard[0])
                {
                    re[1] = b[3][k];
                    re[0] = 2;
                    result.Add(re);
                }
            }
            //炸弹
            for (int k = 0; k < b[3].Count; k++)
            {
                int[] re = new int[2];
                re[1] = b[3][k];
                re[0] = 4;
                result.Add(re);
            }
        }
        else if (type1 == pdkCardType.CARDTYPE.c112233)
        {//连对

            //循环遍历玩家手牌 是否有大于出牌的
            for (int i = (int)a[1][0] + 1; i < 13; i++)
            {
                int findCount = 0;
                int[] re = new int[a[1].Count + 1];
                re[0] = a[1].Count;
                for (int j = 0; j < chuCard.Length / 2; j++)
                {
                    //遍历2张的。
                    for (int k = 0; k < b[1].Count; k++)
                    {
                        if ((int)b[1][k] != 12 && (int)b[1][k] == i + j)
                        {
                            findCount++;
                            re[findCount] = b[1][k];
                            break;
                        }
                    }
                    //遍历3张的。
                    for (int k = 0; k < b[2].Count; k++)
                    {
                        if ((int)b[2][k] != 12 && (int)b[2][k] == i + j)
                        {
                            findCount++;
                            re[findCount] = b[2][k];
                            break;
                        }
                    }
                    //遍历4张的。
                    for (int k = 0; k < b[3].Count; k++)
                    {
                        if ((int)b[3][k] != 12 && (int)b[3][k] == i + j)
                        {
                            findCount++;
                            re[findCount] = b[3][k];
                            break;
                        }
                    }
                }

                if (findCount >= chuCard.Length / 2)
                {
                    result.Add(re);
                }
            }

            //炸弹
            for (int k = 0; k < b[3].Count; k++)
            {
                int[] re = new int[2];
                re[1] = b[3][k];
                re[0] = 4;
                result.Add(re);
            }

        }
        else if (type1 == pdkCardType.CARDTYPE.c311)
        {//三带2

            //三张
            for (int k = 0; k < b[2].Count; k++)
            {
                int[] re = new int[2];
                if (b[2][k] > a[2][0])
                {
                    re[1] = b[2][k];
                    re[0] = 3;
                    result.Add(re);
                }
            }

            //4张
            for (int k = 0; k < b[3].Count; k++)
            {
                int[] re = new int[2];
                if (b[3][k] > a[2][0])
                {
                    re[1] = b[3][k];
                    re[0] = 3;
                    result.Add(re);
                }
            }

            //炸弹
            for (int k = 0; k < b[3].Count; k++)
            {
                int[] re = new int[2];
                re[1] = b[3][k];
                re[0] = 4;
                result.Add(re);
            }

        }
        else if (type1 == pdkCardType.CARDTYPE.c1112223456)
        {//飞机带翅膀

            //循环遍历玩家手牌 是否有大于出牌的
            for (int i = (int)a[2][0] + 1; i < 13; i++)
            {
                int findCount = 0;
                int[] re = new int[a[2].Count + 1];
                re[0] = a[2].Count;
                for (int j = 0; j < chuCard.Length / 5; j++)
                {
                    //遍历3张的。
                    for (int k = 0; k < b[2].Count; k++)
                    {
                        if ((int)b[2][k] != 12 && (int)b[2][k] == i + j)
                        {
                            findCount++;
                            re[findCount] = b[2][k];
                            break;
                        }
                    }
                    //遍历4张的。
                    for (int k = 0; k < b[3].Count; k++)
                    {
                        if ((int)b[3][k] != 12 && (int)b[3][k] == i + j)
                        {
                            findCount++;
                            re[findCount] = b[3][k];
                            break;
                        }
                    }
                }

                if (findCount >= chuCard.Length / 5)
                {
                    result.Add(re);
                }

            }

            //炸弹
            for (int k = 0; k < b[3].Count; k++)
            {
                int[] re = new int[2];
                re[1] = b[3][k];
                re[0] = 4;
                result.Add(re);
            }
        }
        else if (type1 == pdkCardType.CARDTYPE.c12345)
        {//顺子

            //循环遍历玩家手牌 是否有大于出牌的
            for (int i = chuCard[0] + 1; i < 13; i++)
            {
                int[] re = new int[chuCard.Length + 1];
                re[0] = chuCard.Length;
                int findCount = 0;
                for (int j = 0; j < chuCard.Length; j++)
                {
                    for (int k = 0; k < length; k++)
                    {
                        if (myCardArray[k] != 12 && myCardArray[k] == i + j)
                        {
                            findCount++;
                            re[findCount] = myCardArray[k];
                            break;
                        }
                    }
                }

                if (findCount >= chuCard.Length)
                {
                    result.Add(re);
                }
            }

            //炸弹
            for (int k = 0; k < b[3].Count; k++)
            {
                int[] re = new int[2];
                re[1] = b[3][k];
                re[0] = 4;
                result.Add(re);
            }
        }
        else if (type1 == pdkCardType.CARDTYPE.c4)
        {//炸弹

            //炸弹
            for (int k = 0; k < b[3].Count; k++)
            {
                int[] re = new int[2];
                if (b[3][k] > chuCard[0])
                {
                    re[1] = b[3][k];
                    re[0] = 4;
                    result.Add(re);
                }
            }
        }


        return result;
    }

    //自己出牌
    public void putOutCard()
    {
        pdkCardVO cardvo = new pdkCardVO();

        List<int> pai = new List<int>();
        for (int i = 0; i < handerCardList[0].Count; i++)
        {
            GameObject gob = handerCardList[0][i];
            if (gob != null)
            {
                if (gob.GetComponent<pdkCardScript>().selected)
                {
                    pai.Add(gob.GetComponent<pdkCardScript>().getPoint());
                    handerCardList[0].Remove(gob);
                    Destroy(gob);
                    i--;
                }
            }
        }

        int outIndex = 0;
        //去除上轮打出的牌
        cleanChuPai(0);
        //显示打出的牌
        List<int> card = pai;
        int count = 0;
        for (int i = 0; i < card.Count; i++)
        {
            GameObject gob = Instantiate(Resources.Load("prefab/pdk/HandCard_s")) as GameObject;
            if (gob != null)
            {
                gob.transform.SetParent(ThrowTransformList[outIndex]);
                gob.GetComponent<pdkCardScript>().setPoint(card[i]);
                gob.transform.localScale = new Vector3(1f, 1f, 1);
                gob.transform.localPosition = new Vector3(-200 + count * 25, 0, 0);
                count++;
            }
        }

        if (pai.Count == 0)
            return;

        SetPosition();

        cardvo.card = ToArray(pai);
        CustomSocket.getInstance().sendMsg(new PDK_PutOutCardRequest(cardvo));
        btnActionScript.cleanBtnShow();

        isClickTiShi = false;
    }

    public void cleanClock()
    {
        foreach (PlayerItemScript pis in playerItems)
        {
            pis.showClock(false);
        }
    }

    /// <summary>
    /// 接收到其它人的出牌通知
    /// </summary>
    /// <param name="response">Response.</param>
    public void putOutCardCallback(ClientResponse response)
    {
        cleanClock();
        UpdateTimeReStart();
        putOutCardVO cardvo = JsonMapper.ToObject<putOutCardVO>(response.message);
        int outIndex = getIndexByDir(getDirection(cardvo.curAvatarIndex));

        //如果是自己出牌，去掉自己的手牌
        if (outIndex == 0)
        {
            currentIndex = 1;
        }
        else
        {//别人出牌
         //去除上轮打出的牌
            cleanChuPai(outIndex);
            //显示打出的牌
            List<int> card = cardvo.card;
            lastCard = ToArray(card);
            int count = 0;
            for (int i = 0; i < card.Count; i++)
            {
                GameObject gob = Instantiate(Resources.Load("prefab/pdk/HandCard_s")) as GameObject;
                if (gob != null)
                {
                    gob.transform.SetParent(ThrowTransformList[outIndex]);
                    gob.GetComponent<pdkCardScript>().setPoint(card[i]);
                    gob.transform.localScale = new Vector3(1f, 1f, 1);
                    gob.transform.localPosition = new Vector3(-200 + count * 25, 0, 0);
                    count++;
                }
            }

            int oldCount = int.Parse(playerItems[outIndex].paiCountText.text);
            playerItems[outIndex].showPaiCountText(oldCount - card.Count);

            if (outIndex == 2)
            {//上家出牌
                currentIndex = 0;
            }
            else if (outIndex == 1)
            {//下家出牌
                currentIndex = 2;
            }
        }

        //去除下家上轮打出的牌
        cleanChuPai(currentIndex);
    }

    //去除打出的牌
    public void cleanChuPai(int outIndex, bool Yaobuqi = false)
    {
        for (int i = 0; i < ThrowTransformList[outIndex].childCount; i++)
        {
            GameObject go = ThrowTransformList[outIndex].GetChild(i).gameObject;
            Destroy(go);
        }
        playerItems[outIndex].showYaoBuQi(Yaobuqi);
    }

    public void SetPosition()//设置位置
    {
        int count = handerCardList[0].Count;
        int startX = -370 + (16 - count) / 2 * 50;

        for (int i = 0; i < count; i++)
        {
            handerCardList[0][i].transform.localPosition = new Vector3(startX + i * 50f, 0); //从左到右依次对齐
        }
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    /// <param name="response">Response.</param>
    public void startGame(ClientResponse response)
    {
        hasQiang = hasZhu = false;
        hasZhus = hasQiangs = 0;
        GlobalDataScript.loginResponseData.stateForNiu = 2;
        SetTip();

        GlobalDataScript.roomAvatarVoList = avatarList;
        StartGameVO sgvo = JsonMapper.ToObject<StartGameVO>(response.message);
        GlobalDataScript.count_Players_DN = avatarList.Count;
        GlobalDataScript.roomVo.guiPai = sgvo.gui;
        GlobalDataScript.roomVo.guiPai2 = sgvo.gui2;

        cleanGameplayUI();

        //开始游戏后不显示
        MyDebug.Log("startGame");
        GlobalDataScript.surplusTimes--;
        if (GlobalDataScript.goldType)
        {
            Number.text = GlobalDataScript.playCountForGoldType + "";
        }
        else
            Number.text = GlobalDataScript.surplusTimes + "/" + GlobalDataScript.roomVo.roundNumber;

        GlobalDataScript.finalGameEndVo = null;
        initArrayList();

        isFirstOpen = false;
        GlobalDataScript.isOverByPlayer = false;
        int selfIndex = getMyIndexFromList();
        for (int i = 0; i < GlobalDataScript.count_Players_DN; i++)       // GlobalDataScript.count_Players_DN  玩家的总数
        {
            if (selfIndex == i)
            {
                mineList.Add(sgvo.paiArray[0]);
                print(" startGame ++++++++++++++++++++ " + mineList.Count);       // -lan
            }
            else
            {

                mineList.Add(new List<int>());
            }
        }
        //mineList = sgvo.paiArray;

        setAllPlayerReadImgVisbleToFalse();
        DisplayHandCard();         //todo
        float time = GlobalDataScript.roomVo.ming ? 1.0f : 0.5f;
        Invoke("ShowQiang", time);

        if (curDirString == DirectionEnum.Bottom)
        {  //如果是庄家并且是在bottom位置 先出牌  跑得快  //斗牛  不是庄家要下注
            btnActionScript.showBtn();
            GlobalDataScript.isDrag = true;
        }
        else
        {
            GlobalDataScript.isDrag = false;
        }

        isGameStart = true;
        currentIndex = curDirIndex;
    }

    private void ShowQiang()
    {
        if (GlobalDataScript.roomVo.qiang)
        {
            // Add by Shise
            SoundCtrl.getInstance().playSoundByActionButton(9);
            // End
            panel_Qiang.SetActive(true);
            GlobalDataScript.loginResponseData.stateForNiu = 3;
            SetTip();
        }
    }

    public void qiangZhuang(int isQiang)   // 0为不抢庄    1为抢庄  mark
    {
        int index = getMyIndexFromList();  //自身 int i
        //playerItems[index].bianKuang.SetActive(true);
        //int chooseHogMultiple = 1;
        SoundCtrl.getInstance().playSoundByActionButton(1);
        hasQiang = true;
        timer = -1;
        CustomSocket sok = CustomSocket.getInstance();
        //if (isQiang != 0)
        //    chooseHogMultiple = isQiang;
        sok.sendMsg(new QiangZhuangRequest(isQiang/* == 0 ? 0 : 1*/));
    }

    public void qiangResponse_DN(ClientResponse response)
    {
        int UUID = Convert.ToInt32(response.message);
        hasQiangs++;
        if (UUID == GlobalDataScript.loginResponseData.account.uuid)
        {
            panel_Qiang.SetActive(false);
            GlobalDataScript.loginResponseData.stateForNiu = 4;
            SetTip();
        }
    }

    public void zhuangResponse_DN(ClientResponse response)
    {
        // Add by Shise
        string bankerMessage;    // 关于庄回调的信息
        string qiangBankStr;
        string[] qiangBankStrs;
        // banker mark
        // response.message  发送格式为string形的庄家下标 + “,” +  string 的抢庄玩家下标的列表  例：4,[3,5];
        bankerMessage = response.message;
        // Change by Shise    bankerId = Convert.ToInt32(response.message);
        bankerId = Convert.ToInt32(bankerMessage.Split(',')[0]);
        curDirString = getDirection(bankerId);
        //string 的抢庄玩家下标的列表再次拆分  例：[0,1]  去除中括号
        qiangBankStr = bankerMessage.Remove(0, 2);
        qiangBankStrs = qiangBankStr.Replace("[", "").Replace("]", "").Split(',');
        for (int i = 0; i < qiangBankStrs.Length; i++)
        {
            qiangBank.Add(Convert.ToInt32(i));
            print("<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<" + bankerMessage);
        }
        float time = 0;
        time = !GlobalDataScript.roomVo.qiang && GlobalDataScript.roomVo.ming ? 4.0f : 3.5f;
        //StartCoroutine(BianKuangGlint());
        if(GlobalDataScript.roomVo.qiang)
        {
            for (int index = 0; index < playerItems.Count; index++)
            {
                playerItems[index].bianKuang.SetActive(true);
               
            }
            SoundCtrl.getInstance().playSoundByActionButton(12);
        }
        StartCoroutine(BankerBianKuangGlint());
        Invoke("SetBanker", time);
    }

    public IEnumerator BianKuangGlint()
    {
        for (int i = 0; i < qiangBank.Count; i++)
        {
            print("<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<" + qiangBank[i]);
            playerItems[i].bianKuang.SetActive(true);
            yield return null;
        }

    }

    public IEnumerator BankerBianKuangGlint()
    {
        yield return new WaitForSeconds(2);
        playerItems[bankerId].bianKuang.SetActive(true);
        playerItems[bankerId].bianKuangAni.SetBool("IsBanker", true);
    }

    private void SetBanker()
    {
        // Add by Shise
        for (int index = 0; index < playerItems.Count; index++)
        {
            playerItems[index].bianKuang.SetActive(false);
            playerItems[index].bianKuangAni.SetBool("IsBanker", false);
        }
        // End
        avatarList[bankerId].main = true;
        GlobalDataScript.mainUuid = avatarList[bankerId].account.uuid;
        if (GlobalDataScript.mainUuid == GlobalDataScript.loginResponseData.account.uuid)
        {
            GlobalDataScript.loginResponseData.stateForNiu = 6;
            SetTip();
        }
        else
        {
            GlobalDataScript.loginResponseData.stateForNiu = 5;
            SetTip();
        }
        int bankerInedx = getIndexByDir(getDirection(bankerId));
        playerItems[bankerInedx].setbankImgEnable(true);
        if (bankerInedx != 0)
        {
            // Add by Shise
            SoundCtrl.getInstance().playSoundByActionButton(8);
            // End
            panel_Zhus.SetActive(true);
        }
    }
    public void xiaZhu(int zhu)
    {
        // Change by Shise
        SoundCtrl.getInstance().playSoundByActionButton(10);
        // SoundCtrl.getInstance().playSoundByActionButton(1);
        // End
        hasZhu = true;
        timer = -1;
        CustomSocket sok = CustomSocket.getInstance();
        sok.sendMsg(new XiaZhuRequest(zhu));

    }
    public void xiaZhuResponse_DN(ClientResponse response)
    {
        // mark
        hasZhus++;
        string[] strs = response.message.Split(new char[1] { ':' });  // Response.message返回 uuid:下注倍数
        int UUID = Convert.ToInt32(strs[0]);
        if (UUID == GlobalDataScript.loginResponseData.account.uuid)
        {
            panel_Zhus.SetActive(false);
            GlobalDataScript.loginResponseData.stateForNiu = 7;
            SetTip();
        }

        if (hasZhus == (GlobalDataScript.count_Players_DN - 1) && GlobalDataScript.mainUuid == GlobalDataScript.loginResponseData.account.uuid)
        {
            GlobalDataScript.loginResponseData.stateForNiu = 7;
            SetTip();
        }

        if (hasZhus == (GlobalDataScript.count_Players_DN - 1))
        {
            float time = GlobalDataScript.roomVo.ming ? 0.5f : 1f;
            Invoke("NiuCountDown", time);
        }

        int index = getIndex(UUID);
        index = getIndexByDir(getDirection(index));
        playerItems[index].setZhuImg(Convert.ToInt32(strs[1]));

        //-lan
        Invoke("isShowCuoPai_Button", 0.1f);
    }
    private void NiuCountDown()
    {
        // Change by Shise  timer = 11;
        timer = 5;
        // End
    }
    private void showCards(int index)
    {
        foreach (GameObject go in handerCardList[index])
        {
            go.GetComponent<pdkCardScript>().show();
        }
    }

    public void niuResponse_DN(ClientResponse response)
    {
        string[] infos = response.message.Split(new char[1] { ';' });
        DealNiuAndFen(infos);
        panel_Niu.SetActive(true);
        SetCardPoint();

        //mark
        // Change by Dongdong
        // if (!GlobalDataScript.roomVo.cuoPai)
        FanPai(0);
        // End

    }

    public void DealNiuAndFen(string[] Niu_Pai)
    {
        int index = getMyIndexFromList();
        for (int i = 0; i < GlobalDataScript.count_Players_DN; i++)
        {
            string[] niu = Niu_Pai[i].Split(':');
            string[] pai = Niu_Pai[i + GlobalDataScript.count_Players_DN].Split(':');
            AvatarVO ava = avatarList[i];
            ava.niu = Convert.ToInt32(niu[0]);
            ava.fen = Convert.ToInt32(niu[1]);
            if (i == index)
            {
                GlobalDataScript.loginResponseData.niu = ava.niu;
                GlobalDataScript.loginResponseData.fen = ava.fen;
            }
            ava.paiArray = new int[1][];
            ava.paiArray[0] = new int[54];
            /**
			for (int j = 0; j < 54; j++) {
				ava.paiArray [0] [j] = 0;
			}
			*/
            for (int j = 0; j < pai.Length; j++)
            {
                int card = Convert.ToInt32(pai[j]);
                ava.paiArray[0][card] = 1;
            }
        }
    }
    private void SetCardPoint()
    {
        for (int i = 0; i < GlobalDataScript.count_Players_DN; i++)
        {
            SetCardPointAtIndex(i);
        }

    }

    private void SetCardPointAtIndex(int index)              //根据下标放置牌位置   -lan  翻牌
    {
        int[] pais = avatarList[index].paiArray[0];
        int pos = getIndexByDir(getDirection(index));
        int myIndex = getMyIndexFromList();
        int count = 0;
        for (int i = 0; i < 54; i++)
        {
            if (pais[i] == 1)
            {
                print("+++SetCardPointAtIndex +++ " + index + " ------------" + i + "|||||||||||||" + myIndex + "---" + index);
                GameObject go = handerCardList[pos][count];
                go.GetComponent<pdkCardScript>().setPoint(i, 3);
                count++;
                if (myIndex == index)
                {
                    myCardList.Add(i);
                }
            }
        }
        print("++++SetCardPointAtIndex++" + myCardList.Count);
        //cuopaiCard.GetComponent<pdkCardScript>().setPoint(mineList[0][1], 3);              //-lan    显示自己家翻的那张牌
    }
    public void hasNiu(bool isNiu)
    {
        SoundCtrl.getInstance().playSoundByActionButton(1);
        CustomSocket sok = CustomSocket.getInstance();
        sok.sendMsg(new NiuNoticeRequest());
        //SoundCtrl.getInstance ().playSoundByNiu (GlobalDataScript.loginResponseData.niu,GlobalDataScript.loginResponseData.account.sex);//防止重复播放 让服务端返回时统一播放
        timer = -1;
        if (GlobalDataScript.roomVo.cuoPai == true)
        {

            GameObject go = handerCardList[0][4];
            bool hasShow = go.GetComponent<pdkCardScript>().hasShow;
            //if(!go.GetComponent<pdkCardScript> ().hasShow)
            StartCoroutine(FanPai(1, go));
            go.GetComponent<pdkCardScript>().hasShow = true;

        }



    }

    // Remove by Shise
    //else
    //{
    //    if (GlobalDataScript.loginResponseData.niu > 0)
    //    {
    //      //  TipsManagerScript.getInstance().setTips("你的牌有牛哦");
    //    }
    //    else
    //    {
    //        CustomSocket sok = CustomSocket.getInstance();
    //        sok.sendMsg(new NiuNoticeRequest());
    //        //SoundCtrl.getInstance ().playSoundByNiu (GlobalDataScript.loginResponseData.niu,GlobalDataScript.loginResponseData.account.sex);
    //        timer = -1;
    //    }
    //}
    // End

    public void niuNoticeResponse_DN(ClientResponse response)
    {
        int index = Convert.ToInt32(response.message);
        AvatarVO ava = avatarList[index];
        int sex = ava.account.sex;
        index = getIndexByDir(getDirection(index));
        if (index == 0)
            panel_Niu.SetActive(false);

        // Change by Dongdong
        else
        {
            FanPai(index);
        }
        // End

        playerItems[index].setNiuImg(ava.niu);
        SoundCtrl.getInstance().playSoundByNiu(ava.niu, sex);
    }

    private void initArrayList()
    {
        mineList = new List<List<int>>();
        handerCardList = new List<List<GameObject>>();
        for (int i = 0; i < GlobalDataScript.roomVo.playerAmounts; i++)
        {
            handerCardList.Add(new List<GameObject>());
        }
    }

    /**
	private void ShowCards(){
		int selfIndex = getMyIndexFromList ();
		ShowCards_DN_Self (selfIndex);
		ShowCards_DN_Others (selfIndex);
	}
	 
	private void ShowCards_DN_Self(int index){
		int count = 0;
		for (int i = 0; i < 54; i++) {
			int point = i;
			if (mineList [index] [point] == 1) {
				GameObject gob = null;
				if (GlobalDataScript.roomVo.ming) {
					
					if (count < 5 - GlobalDataScript.roomVo.mengs) {
						if (point > 51) {
							gob = Instantiate (Resources.Load ("prefab/pdk/KingCard")) as GameObject;
						}else
							gob = Instantiate (Resources.Load ("prefab/pdk/HandCard")) as GameObject;
						gob.GetComponent<pdkCardScript> ().hasShow = true;
					} else {
						if (point > 51) {
							gob = Instantiate (Resources.Load ("prefab/pdk/KingCard_Back")) as GameObject;
						}else
							gob = Instantiate (Resources.Load ("prefab/pdk/HandCard_Back")) as GameObject;
						gob.GetComponent<pdkCardScript> ().hasShow = false;
					}
				} else {
					if (point > 51) {
						gob = Instantiate (Resources.Load ("prefab/pdk/KingCard_Back")) as GameObject;
					}else
						gob = Instantiate (Resources.Load ("prefab/pdk/HandCard_Back")) as GameObject;
					gob.GetComponent<pdkCardScript> ().hasShow = false;
				}

				if (gob != null) {
					//gob.GetComponent<pdkCardScript> ().onDragSelect += dragSelect;
					//gob.GetComponent<pdkCardScript> ().onCardSelect += cardSelect;
					gob.transform.SetParent (handTransformList [0]);
					gob.GetComponent<pdkCardScript> ().setPoint (point,3);
					//gob.transform.localPosition = new Vector3 (-370 + count * 50f, 0, 0);
					handerCardList [0].Add (gob);
					count++;
				}
			}
		}

	}
	private void ShowCards_DN_Others(int index){
		for (int i = 0; i < mineList.Count; i++) {
			if (i == index)
				continue;
			int curIndex = getIndexByDir (getDirection (i));

			for (int k = 0; k < 54; k++) {
				
				int point = k;
				if (mineList [i] [point] == 1) {
					GameObject gob = null;
					if(point > 51) 
						gob = Instantiate (Resources.Load ("prefab/pdk/KingCard_s_Back")) as GameObject;
					else
						gob = Instantiate (Resources.Load ("prefab/pdk/HandCard_s_Back")) as GameObject;
						gob.GetComponent<pdkCardScript> ().hasShow = false;
						if (gob != null) {
							//gob.GetComponent<pdkCardScript> ().onDragSelect += dragSelect;
							//gob.GetComponent<pdkCardScript> ().onCardSelect += cardSelect;
							gob.transform.SetParent (handTransformList [curIndex]);
							gob.GetComponent<pdkCardScript> ().setPoint (point,3);
							//gob.transform.localPosition = new Vector3 (-370 + count * 50f, 0, 0);
							handerCardList [curIndex].Add (gob);
					}
				}
			}
			
		}
	}
	*/
    private void DisplayHandCard(bool hasShowed = false)
    {
        for (int i = 0; i < avatarList.Count; i++)
        {
            DisplayHandCardAtIndex(i, hasShowed);
        }
    }

    private void DisplayHandCardAtIndex(int index, bool hasShowed)
    {
        int count = 0;
        GameObject gob = null;
        int pos = getIndexByDir(getDirection(index));
        for (int i = 0; i < 54; i++)
        {
            if (mineList[index].Count == 0)
                break;
            if (mineList[index][i] == 1)
            {
                print(" ++++++++++++++ DisplayHandCardAtIndex ++++++++++++++++++ " + index + "----- " + i);
                if (pos == 0)                       // 自己
                {
                    gob = Instantiate(Resources.Load("prefab/dn/HandCard_DN")) as GameObject;
                    gob.GetComponent<pdkCardScript>().isSmall = false;
                }
                else
                {
                    gob = Instantiate(Resources.Load("prefab/dn/HandCard_s_DN")) as GameObject;
                    gob.GetComponent<pdkCardScript>().isSmall = true;
                }
                if (gob != null)
                {
                    //gob.GetComponent<pdkCardScript> ().onDragSelect += dragSelect;
                    //gob.GetComponent<pdkCardScript> ().onCardSelect += cardSelect;
                    gob.transform.SetParent(handTransformList[pos]);
                    gob.transform.localScale = Vector3.one;
                    gob.GetComponent<pdkCardScript>().setPoint(i, 3);
                    if (hasShowed)
                    {
                        gob.GetComponent<pdkCardScript>().show();
                        gob.GetComponent<pdkCardScript>().hasShow = true;
                    }
                    handerCardList[pos].Add(gob);
                    count++;
                }
            }
        }
        //cuopaiCard.GetComponent<pdkCardScript>().setPoint(myCardList[3], 3);
        while (count < 5)
        {
            if (pos == 0)
            {
                gob = Instantiate(Resources.Load("prefab/dn/HandCard_DN")) as GameObject;
                gob.GetComponent<pdkCardScript>().isSmall = false;
            }
            else
            {
                gob = Instantiate(Resources.Load("prefab/dn/HandCard_s_DN")) as GameObject;
                gob.GetComponent<pdkCardScript>().isSmall = true;
            }
            if (gob != null)
            {
                //gob.GetComponent<pdkCardScript> ().onDragSelect += dragSelect;
                //gob.GetComponent<pdkCardScript> ().onCardSelect += cardSelect;
                gob.transform.SetParent(handTransformList[pos]);
                gob.transform.localScale = Vector3.one;

                gob.GetComponent<pdkCardScript>().setPoint(-1, 3);
                /**
				if (hasShowed) {
					gob.GetComponent<pdkCardScript> ().ShowUp();
					gob.GetComponent<pdkCardScript> ().hasShow = true;
				}
				*/
                handerCardList[pos].Add(gob);
                count++;
            }
        }
        if (!hasShowed)
            Invoke("FanPaiSelf", 0.2f);
    }

    void isShowCuoPai_Button()    // 是否调用显示搓牌按钮   翻牌
    {
        if (GlobalDataScript.roomVo.cuoPai == true)
        {
            cuoPai_Button.gameObject.SetActive(true);
        }
    }

    // by Dongdong
    /// <summary>
    /// 是否搓牌
    /// </summary>
    void FanPaiSelf()
    {
        // mark
        if (GlobalDataScript.roomVo.cuoPai == false)
        {
            FanPai(0);    //todo 
        }
    }
    public void FanPai(int pos)
    {
        // mark
        //float i = 0;
        //int start = 0;
        //int end = 5;
        //if (GlobalDataScript.roomVo.ming)
        //{
        //    if (pos == 0)
        //    {
        //        GameObject go = handerCardList[pos][0];
        //        if (go.GetComponent<pdkCardScript>().hasShow)
        //        {
        //            start = 5 - GlobalDataScript.roomVo.mengs;
        //        }
        //        else
        //        {
        //            if (!GlobalDataScript.roomVo.cuoPai)
        //                end = 5 - GlobalDataScript.roomVo.mengs;
        //            else
        //                end = 5;
        //        }
        //    }
        //}
        float i = 0;
        int end = 5;
        int start = 0;
        if (GlobalDataScript.roomVo.ming)
        {
            if (pos == 0)
            {
                GameObject go = handerCardList[pos][0];    //todo 
                if (go.GetComponent<pdkCardScript>().hasShow)
                {
                    start = 5 - GlobalDataScript.roomVo.mengs;
                    print(" ++++++++++++ fanpai start+++++++++++" + start + " -----" + GlobalDataScript.roomVo.mengs);
                }
                else
                {
                    end = 5 - GlobalDataScript.roomVo.mengs;
                    print(" ++++++++++++ fanpai end+++++++++++" + end);
                }
            }
        }

        for (int k = start; k < end; k++)
        {
            GameObject go = handerCardList[pos][k];
            bool hasShow = go.GetComponent<pdkCardScript>().hasShow;
            //if(!go.GetComponent<pdkCardScript> ().hasShow)
            StartCoroutine(FanPai(i, go));
            go.GetComponent<pdkCardScript>().hasShow = true;
            i += pos == 0 ? 0.3f : 0.1f;
        }
    }


    IEnumerator FanPai(float time, GameObject go)
    {
        yield return new WaitForSeconds(time);
        if (go.GetComponent<pdkCardScript>().cardPoint != -1)
        {
            go.GetComponent<pdkCardScript>().startPlay();
        }
    }

    public void dragSelect(GameObject gameobject)
    {
        for (int i = 0; i < handerCardList[0].Count; i++)
        {
            GameObject obj = handerCardList[0][i];
            if (obj != null)
            {
                if (obj.GetComponent<pdkCardScript>().dragFlag)
                {
                    pdkCardScript script = obj.transform.GetComponent<pdkCardScript>();
                    if (script.selected == false)
                    {
                        obj.transform.localPosition = new Vector3(obj.transform.localPosition.x, 40);
                        script.selected = true;
                    }
                    else
                    {
                        obj.transform.localPosition = new Vector3(obj.transform.localPosition.x, 0);
                        script.selected = false;
                    }
                    obj.GetComponent<pdkCardScript>().dragFlag = false;
                }
            }
        }

        isCanChu();
    }

    public void cardSelect(GameObject obj)
    {
        if (obj != null)
        {
            pdkCardScript script = obj.transform.GetComponent<pdkCardScript>();
            if (script.selected == false)
            {
                obj.transform.localPosition = new Vector3(obj.transform.localPosition.x, 40);
                script.selected = true;
            }
            else
            {
                obj.transform.localPosition = new Vector3(obj.transform.localPosition.x, 0);
                script.selected = false;
            }
        }

        isCanChu();
    }

    public void isCanChu()
    {
        List<int> pai = new List<int>();
        for (int i = 0; i < handerCardList[0].Count; i++)
        {
            GameObject gob = handerCardList[0][i];
            if (gob != null)
            {
                if (gob.GetComponent<pdkCardScript>().selected)
                {
                    pai.Add(gob.GetComponent<pdkCardScript>().getPoint());
                }
            }
        }

        int[] card = ToArray(pai);

        pdkCardType pct = new pdkCardType();
        if (lastCard == null || lastCard.Length == 0)
        {//庄家第一手出牌
            pdkCardType.CARDTYPE type = pct.getType(card);
            if (type == pdkCardType.CARDTYPE.c0)
            {
                if (btnActionScript.tishiBtn.active == true)
                {
                    btnActionScript.showBtn(2);
                }
            }
            else if (type == pdkCardType.CARDTYPE.c3
              || type == pdkCardType.CARDTYPE.c31
              || type == pdkCardType.CARDTYPE.c111222N)
            {

                if (btnActionScript.tishiBtn.active == true)
                {
                    if (handerCardList[0].Count == pai.Count)//最后一手牌
                        btnActionScript.showBtn(3);
                    else
                        btnActionScript.showBtn(2);
                }

            }
            else
            {
                if (btnActionScript.tishiBtn.active == true)
                {
                    if (GlobalDataScript.roomVo.xian3)
                    {
                        bool isFind3 = false;
                        foreach (int p in card)
                        {
                            if (p == 0)
                                isFind3 = true;
                        }

                        if (GlobalDataScript.roomVo.zhang16)
                        {
                            if (handerCardList[0].Count == 16)
                            {
                                if (!isFind3)
                                {
                                    btnActionScript.showBtn(2);
                                    return;
                                }
                            }
                        }
                        else
                        {
                            if (handerCardList[0].Count == 15)
                            {
                                if (!isFind3)
                                {
                                    btnActionScript.showBtn(2);
                                    return;
                                }
                            }
                        }
                        btnActionScript.showBtn(3);
                    }
                    else
                    {
                        btnActionScript.showBtn(3);
                    }
                }
            }
        }
        else if (pct.compareType(lastCard, card))
        {
            if (btnActionScript.tishiBtn.active == true)
            {
                btnActionScript.showBtn(3);
            }
        }
        else
        {
            if (btnActionScript.tishiBtn.active == true)
            {
                btnActionScript.showBtn();
            }
        }
    }

    //*
    //所有牌复位
    //*
    public void cardReset()
    {
        for (int i = 0; i < handerCardList[0].Count; i++)
        {
            if (handerCardList[0][i] == null)
            {
                handerCardList[0].RemoveAt(i);
                i--;
            }
            else
            {
                handerCardList[0][i].transform.localPosition = new Vector3(handerCardList[0][i].transform.localPosition.x, 0);
                handerCardList[0][i].transform.GetComponent<pdkCardScript>().selected = false;
                handerCardList[0][i].transform.GetComponent<pdkCardScript>().dragFlag = false;
            }
        }
    }

    private void cleanGameplayUI()
    {
        canClickButtonFlag = true;
        inviteFriendButton.transform.gameObject.SetActive(false);
    }

    private void setAllPlayerReadImgVisbleToFalse()  // change 人数
    {
        for (int i = 0; i < GlobalDataScript.roomVo.playerAmounts; i++)
        {
            playerItems[i].readyImg.enabled = false;
        }
    }

    public void gameReadyNotice(ClientResponse response)   // change 人数
    {
        JsonData json = JsonMapper.ToObject(response.message);
        int avatarIndex = Int32.Parse(json["avatarIndex"].ToString());
        int myIndex = getMyIndexFromList();
        int seatIndex = avatarIndex - myIndex;
        if (seatIndex < 0)
        {
            // Change by Shise
            seatIndex = GlobalDataScript.roomVo.playerAmounts - 1 + seatIndex;
            // End
        }

        //自己
        if (avatarIndex == myIndex)
        {
            markselfReadyGame();
        }

        this.playerItems[seatIndex].readyImg.enabled = true;
        avatarList[avatarIndex].isReady = true;

        string readyUid = json["readyUid"].ToString();
        //20170310 增加一句结算时 某玩家断线后重连 看不到他人准备 
        for (int i = 0; i < avatarList.Count; i++)
        {
            if (readyUid.IndexOf(avatarList[i].account.uuid.ToString()) != -1)
            {
                avatarList[i].isReady = true;
                setSeat(avatarList[i]);
            }
        }

        SoundCtrl.getInstance().playSoundByActionButton(3);
    }

    /*显示自己准备*/
    public void markselfReadyGame()
    {
        playerItems[0].readyImg.enabled = true;
        readyBtn.gameObject.SetActive(false);
    }

    /**
    *准备游戏
	*/
    public void readyGame()
    {
        if (handerCardList != null && handerCardList.Count > 0 && handerCardList[0].Count > 0)
        {
            cleanArrayList(handerCardList);
            /**
			for (int i = 0; i < handerCardList.Count; i++) {
				List<GameObject> hands = handerCardList [i];
				for (int j = 0; j < hands.Count; j++) {
					//hands [j].GetComponent<pdkCardScript> ().onDragSelect -= dragSelect;
					//hands [j].GetComponent<pdkCardScript> ().onCardSelect -= cardSelect;
					Destroy (hands [j]);
				}
			}
			*/
        }
        getDirection(bankerId);
        if (avatarList[bankerId] != null)
            avatarList[bankerId].main = false;
        playerItems[curDirIndex].setbankImgEnable(false);
        for (int i = 0; i < playerItems.Count; i++)
        {
            playerItems[i].setDNHide();
        }

        GlobalDataScript.loginResponseData.stateForNiu = 1;
        SetTip();
        CustomSocket.getInstance().sendMsg(new GameReadyRequest());
    }


    private void SetTip()
    {
        int tip = GlobalDataScript.loginResponseData.stateForNiu;
        if (tip == 2)
        {
            cleanGameplayUI();//断线重连
        }
        if (tip == 3 || tip == 5)
            UpdateTimeReStart();
        if (tip != 3 && tip != 5 && tip != 7)
            timer = -1;

        if (tip == 7)
        {
            panel_Tips.GetComponent<TipsScript_DN>().setTip(-1);
            return;
        }
        panel_Tips.GetComponent<TipsScript_DN>().setTip(tip);
    }
    public void setRoomRemark()
    {
        RoomCreateVo roomvo = GlobalDataScript.roomVo;
        if (GlobalDataScript.goldType)
        {
            GlobalDataScript.surplusTimes = 10000;
        }
        else
        {
            GlobalDataScript.totalTimes = roomvo.roundNumber;
            GlobalDataScript.surplusTimes = roomvo.roundNumber;
        }
        if (GlobalDataScript.goldType)
        {
            gold.SetActive(true);
            goldText.text = GlobalDataScript.loginResponseData.account.gold + "";
        }
        string roomInfo = "";

        // Change by Shise
        if (!GlobalDataScript.goldType)
        {
            roomInfo += "房间号:" + roomvo.roomId + "\n";
            roomInfo += "局数:" + GlobalDataScript.roomVo.roundNumber;
        }
        //else {
        //roomInfo += "房间号<练习场>";
        ////Number.gameObject.SetActive (false);
        //}
        //if (!GlobalDataScript.goldType) {
        //	if(GlobalDataScript.roomVo.AA)
        //		roomInfo += "、AA制";
        //	else
        //		roomInfo += "、房主付费";
        //}
        //if(GlobalDataScript.roomVo.ming)
        //	roomInfo += "、明牌";
        //else
        //	roomInfo += "、暗牌";
        //if (GlobalDataScript.roomVo.ming) {
        //	roomInfo += "<蒙" + GlobalDataScript.roomVo.mengs + "张>";
        //}
        //if(GlobalDataScript.roomVo.qiang)
        //	roomInfo += "、抢庄";
        //else
        //	roomInfo += "、轮庄";
        //roomInfo += "、底分："+Convert.ToString (GlobalDataScript.roomVo.diFen)+"分";
        // End

        roomRemark.text = roomInfo;

    }

    // Add by Shise
    // mark public void
    // End

    public void otherUserJointRoom(ClientResponse response)
    {
        AvatarVO avatar = JsonMapper.ToObject<AvatarVO>(response.message);
        addAvatarVOToList(avatar);
    }

    public void outRoomCallbak(ClientResponse response)
    {
        OutRoomResponseVo responseMsg = JsonMapper.ToObject<OutRoomResponseVo>(response.message);
        if (responseMsg.status_code == "0")
        {
            if (responseMsg.type == "0")
            {

                int uuid = responseMsg.uuid;
                if (uuid != GlobalDataScript.loginResponseData.account.uuid)
                {
                    int index = getIndex(uuid);
                    avatarList.RemoveAt(index);
                    index = getIndexByDir(getDirection(index));
                    if (handerCardList != null)
                        cleanList(handerCardList[index]);

                    for (int i = 0; i < playerItems.Count; i++)
                    {
                        playerItems[i].setAvatarVo(null);
                        playerItems[i].gameObject.SetActive(false);
                    }

                    if (avatarList != null)
                    {
                        for (int i = 0; i < avatarList.Count; i++)
                        {
                            setSeat(avatarList[i]);
                        }
                    }
                }
                else
                {
                    exitOrDissoliveRoom();
                }

            }
            else
            {
                exitOrDissoliveRoom();
            }

        }
        else
        {
            TipsManagerScript.getInstance().setTips("退出房间失败：" + responseMsg.error);
        }
    }

    public void exitOrDissoliveRoom()
    {
        GlobalDataScript.loginResponseData.resetData();//复位房间数据
        GlobalDataScript.loginResponseData.roomId = 0;//复位房间数据
        GlobalDataScript.roomJoinResponseData = null;//复位房间数据
        GlobalDataScript.roomVo.roomId = 0;
        GlobalDataScript.soundToggle = true;
        clean();
        removeListener();


        if (GlobalDataScript.homePanel != null)
        {
            GlobalDataScript.homePanel.SetActive(true);
            GlobalDataScript.homePanel.transform.SetSiblingIndex(1);
        }
        else
        {
            GlobalDataScript.homePanel = PrefabManage.loadPerfab("Prefab/Panel_Home");
            GlobalDataScript.homePanel.transform.SetSiblingIndex(1);
        }

        while (playerItems.Count > 0)
        {
            PlayerItemScript item = playerItems[0];
            playerItems.RemoveAt(0);
            item.clean();
            Destroy(item.gameObject);
            Destroy(item);
        }
        Destroy(this);
        Destroy(gameObject);

    }

    public void joinToRoom(List<AvatarVO> avatars)
    {
        avatarList = avatars;
        for (int i = 0; i < avatars.Count; i++)
        {
            setSeat(avatars[i]);
        }
        setRoomRemark();
    }

    public void createRoomAddAvatarVO(AvatarVO avatar)
    {
        avatar.scores = 0;
        addAvatarVOToList(avatar);
        setRoomRemark();
        SetTip();
    }

    private void addAvatarVOToList(AvatarVO avatar)   // todo
    {
        if (avatarList == null)
        {
            avatarList = new List<AvatarVO>();
        }
        avatarList.Add(avatar);
        setSeat(avatar);
    }

    /*************************断线重连*********************************/
    public void reEnterRoom()
    {
        if (GlobalDataScript.reEnterRoomData != null)
        {
            //显示房间基本信息
            GlobalDataScript.roomVo.roundNumber = GlobalDataScript.reEnterRoomData.roundNumber;
            GlobalDataScript.roomVo.roomId = GlobalDataScript.reEnterRoomData.roomId;
            GlobalDataScript.roomVo.gameType = GlobalDataScript.reEnterRoomData.gameType;
            GlobalDataScript.roomVo.qiang = GlobalDataScript.reEnterRoomData.qiang;
            GlobalDataScript.roomVo.diFen = GlobalDataScript.reEnterRoomData.diFen;
            GlobalDataScript.roomVo.ming = GlobalDataScript.reEnterRoomData.ming;
            GlobalDataScript.roomVo.mengs = GlobalDataScript.reEnterRoomData.mengs;
            GlobalDataScript.roomVo.AA = GlobalDataScript.reEnterRoomData.AA;
            GlobalDataScript.roomVo.goldType = GlobalDataScript.reEnterRoomData.goldType;
            GlobalDataScript.roomVo.playerAmounts= GlobalDataScript.reEnterRoomData.playerAmounts;
            setRoomRemark();

            //cheat
            if (GlobalDataScript.loginResponseData.account.isCheat == 1)
            {
                Btn_cheat.SetActive(true);
            }
            else
            {
                Btn_cheat.SetActive(false);
            }
            cheatPanel.SetActive(false);

            //确定自己和其它玩家
            avatarList = GlobalDataScript.reEnterRoomData.playerList;
            GlobalDataScript.loginResponseData = avatarList[getMyIndexFromList()];
            GlobalDataScript.count_Players_DN = avatarList.Count;
            GlobalDataScript.roomAvatarVoList = GlobalDataScript.reEnterRoomData.playerList;
            for (int i = 0; i < avatarList.Count; i++)
            {
                AvatarVO ava = avatarList[i];
                setSeat(ava);

                if (ava.main)
                    bankerId = i;
                if (ava.main && GlobalDataScript.loginResponseData.stateForNiu <= 1)
                {
                    GlobalDataScript.mainUuid = ava.account.uuid;
                    playerItems[getIndexByDir(getDirection(bankerId))].setbankImgEnable(false);//新一局游戏还没开始庄家是上一局的庄家不显示，因已经准备好，只是别人还没准备好
                }
                if (ava.isReady && ava.stateForNiu == 1)
                    playerItems[getIndexByDir(getDirection(i))].readyImg.enabled = true;
            }

            //确定自己基本状态
            hasZhus = hasQiangs = 0;
            if (GlobalDataScript.loginResponseData.stateForNiu > 0)
            {
                readyBtn.gameObject.SetActive(false);
            }
            if (GlobalDataScript.reEnterRoomData.qiangForNiu > -1)
                hasQiang = true;
            if (GlobalDataScript.reEnterRoomData.zhuForNiu > 0)
                hasZhu = true;
            SetTip();

            //
            int[][] selfPaiArray = GlobalDataScript.loginResponseData.paiArray;
            if (selfPaiArray == null || selfPaiArray.Length == 0)
            {//游戏还没有开始

            }
            else
            {//牌局已开始
                isGameStart = true;
                //if(GlobalDataScript.loginResponseData.stateForNiu > 1)
                //setAllPlayerReadImgVisbleToFalse ();
                cleanGameplayUI();
                initArrayList();

                if (GlobalDataScript.reEnterRoomData != null && GlobalDataScript.loginResponseData.stateForNiu < 2)
                    return;
                int[][] pais = new int[GlobalDataScript.count_Players_DN][];
                for (int i = 0; i < GlobalDataScript.count_Players_DN; i++)
                {
                    pais[i] = GlobalDataScript.reEnterRoomData.playerList[i].paiArray[0];
                }
                //显示打牌数据
                mineList = ToList(pais);
                DisplayHandCard();//GlobalDataScript.loginResponseData.stateForNiu >= 7
            }
        }

    }

    /// <summary>
    /// 设置当前角色的座位
    /// </summary>
    /// <param name="avatar">Avatar.</param>
    private void setSeat(AvatarVO avatar)
    {
        //游戏结束后用的数据，勿删！！！

        //GlobalDataScript.palyerBaseInfo.Add (avatar.account.uuid, avatar.account);

        if (avatar.account.uuid == GlobalDataScript.loginResponseData.account.uuid)
        {
            playerItems[0].gameObject.SetActive(true);
            playerItems[0].setAvatarVo(avatar);
        }
        else
        {
            int myIndex = getMyIndexFromList();
            int curAvaIndex = avatarList.IndexOf(avatar);
            int seatIndex = curAvaIndex - myIndex;
            if (seatIndex < 0)
            {
                if (GlobalDataScript.roomVo.gameType == 0)
                    seatIndex = 4 + seatIndex;
                else if (GlobalDataScript.roomVo.gameType == 1)
                    seatIndex = 3 + seatIndex;
                else if (GlobalDataScript.roomVo.gameType == 3)
                {//斗牛  最多五个 5个轮回才能回到正确的位置   人数  Change by Shise
                    seatIndex = GlobalDataScript.roomVo.playerAmounts + seatIndex;
                    // End
                }

            }
            playerItems[seatIndex].gameObject.SetActive(true);
            playerItems[seatIndex].setAvatarVo(avatar);
        }

    }

    /// <summary>
    /// Gets the index by dir.
    /// </summary>
    /// <returns>The index by dir.</returns>
    /// <param name="dir">Dir.</param> 
    private int getIndexByDir(string dir)    // mark   位置
    {
        int result = 0;
        switch (dir)
        {

            case DirectionEnum.Bottom: //下
                result = 0;
                break;
            case DirectionEnum.Right: //右
                result = 1;
                break;
            case DirectionEnum.TopRight://上右
                result = 2;
                break;
            case DirectionEnum.TopLeft://上左
                result = 3;
                break;
            case DirectionEnum.Left: //左
                result = 4;
                break;
            // Add by Shise
            case DirectionEnum.LeftBottom:
                result = 5;
                break;
            case DirectionEnum.RightBottom:
                result = 6;
                break;
            case DirectionEnum.MoreOne:
                result = 7;
                break;
            case DirectionEnum.MoreTwo:
                result = 8;
                break;
            case DirectionEnum.Top:
                result = 9;
                break;
                // End-
        }

        return result;
    }

    public int getIndex(int uuid)
    {
        if (avatarList != null)
        {
            for (int i = 0; i < avatarList.Count; i++)
            {
                if (avatarList[i].account != null)
                {
                    if (avatarList[i].account.uuid == uuid)
                    {
                        return i;
                    }
                }
            }
        }
        return 0;
    }


    public int getMyIndexFromList()     // 根据已经坐下的位置进行判断自己的座位下标
    {
        if (avatarList != null)
        {
            for (int i = 0; i < avatarList.Count; i++)
            {
                if (avatarList[i].account.uuid == GlobalDataScript.loginResponseData.account.uuid
                    || avatarList[i].account.openid == GlobalDataScript.loginResponseData.account.openid)
                {
                    GlobalDataScript.loginResponseData.account.uuid = avatarList[i].account.uuid;
                    MyDebug.Log("数据正常返回" + i);
                    return i;
                }

            }
        }

        MyDebug.Log("数据异常返回0");
        return 0;
    }

    /// 根据一个人在数组里的索引，得到这个人所在的方位，L-左，T-上,R-右，B-下（自己的方位永远都是在下方）
    private String getDirection(int avatarIndex)    //  座位位置
    {
        String result = DirectionEnum.Bottom;
        int myselfIndex = getMyIndexFromList();
        if (myselfIndex == avatarIndex)
        {
            MyDebug.Log("getDirection == B");
            curDirIndex = 0;
            return result;
        }
        //从自己开始计算，下一位的索引
        for (int i = 0; i < GlobalDataScript.roomVo.playerAmounts; i++)
        {
            myselfIndex++;
            if (myselfIndex >= GlobalDataScript.roomVo.playerAmounts)
            {
                myselfIndex = 0;
            }
            if (myselfIndex == avatarIndex)
            {
                if (i == 0)
                {
                    MyDebug.Log("getDirection == R");
                    curDirIndex = 1;
                    return DirectionEnum.Right;
                }
                else if (i == 1)
                {
                    MyDebug.Log("getDirection == TR");
                    curDirIndex = 2;
                    return DirectionEnum.TopRight;
                }
                else if (i == 2)
                {
                    MyDebug.Log("getDirection == TL");
                    curDirIndex = 3;
                    return DirectionEnum.TopLeft;
                }
                else if (i == 3)
                {
                    MyDebug.Log("getDirection == L");
                    curDirIndex = 4;
                    return DirectionEnum.Left;
                }
                // Add by Shise
                else if (i == 4)
                {
                    curDirIndex = 5;
                    MyDebug.Log("getDirection == LB");
                    return DirectionEnum.LeftBottom;
                }
                else if (i == 5)
                {
                    curDirIndex = 6;
                    MyDebug.Log("getDirection == RB");
                    return DirectionEnum.RightBottom;
                }
                else if (i == 6)
                {
                    curDirIndex = 7;
                    MyDebug.Log("getDirection == MO");
                    return DirectionEnum.MoreOne;
                }
                else if (i == 7)
                {
                    curDirIndex = 8;
                    MyDebug.Log("getDirection == MT");
                    return DirectionEnum.MoreTwo;
                }
                else if (i == 8)
                {
                    curDirIndex = 9;
                    MyDebug.Log("getDirection == T");
                    return DirectionEnum.Top;
                }
                // End
            }
        }
        MyDebug.Log("getDirection == B");
        curDirIndex = 0;
        return DirectionEnum.Bottom;
    }


    void loadBiaoQing()
    {
        bqObj = Instantiate(Resources.Load("Prefab/Panel_biaoqing")) as GameObject;
        bqObj.transform.parent = this.gameObject.transform;
        bqObj.transform.localPosition = new Vector3(0, 1600, 1);
        // Add by Shise
        bqObj.transform.localScale = new Vector3(1, 1, 1);
        // End
        bqScript = bqObj.GetComponent<biaoqingScript>();
    }

    public biaoqingScript getBqScript()
    {
        return bqScript;
    }

    /*************************桌面常用按钮*********************************/
    /**
	* 邀请好友
	*/
    public void inviteFriend()
    {
        SoundCtrl.getInstance().playSoundByActionButton(1);
        GlobalDataScript.getInstance().wechatOperate.inviteFriend();
    }

    /**
    * 打开聊天窗口
    */
    public void openChatBox()
    {
        SoundCtrl.getInstance().playSoundByActionButton(1);
        PrefabManage.loadPerfab("Prefab/Panel_chatbox_dn");
    }

    // Change by Shise
    /**
    * 打开设置对话框
    */
    public void openGameSettingDialog()
    {
        SoundCtrl.getInstance().playSoundByActionButton(1);
        loadPerfab("Prefab/Panel_Setting");

        SettingScript ss = panelCreateDialog.GetComponent<SettingScript>();
        if (canClickButtonFlag)
        {
            ss.canClickButtonFlag = canClickButtonFlag;
            ss.jiesanBtn.GetComponentInChildren<Text>().text = "申请解散房间";
            ss.type = 2;
        }
        else
        {
            if (0 == getMyIndexFromList())
            { //我是房主
                ss.canClickButtonFlag = canClickButtonFlag;
                ss.jiesanBtn.GetComponentInChildren<Text>().text = "解散房间";
                ss.type = 3;
                ss.dialog_fanhui = dialog_fanhui;
                dialog_fanhui_text.text = "亲，确定要解散房间吗?";
            }
            else
            {
                ss.canClickButtonFlag = canClickButtonFlag;
                ss.jiesanBtn.GetComponentInChildren<Text>().text = "离开房间";
                ss.type = 3;
                ss.dialog_fanhui = dialog_fanhui;
                dialog_fanhui_text.text = "亲，确定要离开房间吗?";
            }
        }
    }

    public void tuichu()
    {
        OutRoomRequestVo vo = new OutRoomRequestVo();
        vo.roomId = GlobalDataScript.roomVo.roomId;
        string sendMsg = JsonMapper.ToJson(vo);

        CustomSocket.getInstance().sendMsg(new OutRoomRequest(sendMsg));

        dialog_fanhui.gameObject.SetActive(false);

        // Add by Shise   播放Panel_HOME的音乐
        SoundCtrl.getInstance().playBGM(2);
        // End


    }

    public void quxiao()
    {
        dialog_fanhui.gameObject.SetActive(false);
    }


    public GameObject panelCreateDialog;
    //界面上打开的dialog
    private void loadPerfab(string perfabName)
    {
        panelCreateDialog = Instantiate(Resources.Load(perfabName)) as GameObject;
        panelCreateDialog.transform.parent = gameObject.transform;
        panelCreateDialog.transform.localScale = Vector3.one;
        //panelCreateDialog.transform.localPosition = new Vector3 (200f,150f);
        panelCreateDialog.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
        panelCreateDialog.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
    }

    public void myselfSoundActionPlay()
    {
        playerItems[0].showChatAction();
    }

    private List<List<int>> ToList(int[][] param)
    {
        List<List<int>> returnData = new List<List<int>>();
        for (int i = 0; i < param.Length; i++)
        {
            List<int> temp = new List<int>();
            for (int j = 0; j < param[i].Length; j++)
            {
                temp.Add(param[i][j]);
            }
            returnData.Add(temp);
        }
        return returnData;
    }

    private int[] ToArray(List<int> param)
    {
        int[] returnData = new int[param.Count];
        for (int i = 0; i < param.Count; i++)
        {
            returnData[i] = param[i];
        }
        return returnData;
    }

    //cheat
    public void closeCheatPanel()
    {
        cheatPanel.SetActive(false);
        if (cheatObjList != null)
        {
            foreach (GameObject obj in cheatObjList)
            {
                Destroy(obj);
            }
            cheatObjList.Clear();
        }
    }


    public void clickBtnCheat()
    {
        cheatPanel.SetActive(true);
        cheatObjList = new List<GameObject>();

        for (int i = 0; i < 54; i++)
        {
            GameObject obj = Instantiate(Resources.Load("prefab/dn/HandCard_dn_cheat")) as GameObject;
            if (obj != null)
            {
                obj.transform.SetParent(cheatContentParent.transform);
                obj.transform.localScale = new Vector3(1, 1, 1);
                obj.GetComponent<cheatDnScript>().setPoint(i, 3);
                obj.GetComponent<cheatDnScript>().onCardSelect += cheatCardSelect;
                cheatObjList.Add(obj); ;
            }
        }

        cheatSendList = new List<int>();
    }

    public void cheatCardSelect(GameObject obj)
    {
        if (cheatSendList.Count < 5)
        {
            int cardPoint = obj.GetComponent<cheatDnScript>().getPoint();
            cheatSendList.Add(cardPoint);
            if (cheatSendList.Count == 5)
            {
                CustomSocket.getInstance().sendMsg(new CheatDnRequest(cheatSendList));
                closeCheatPanel();
            }
        }
    }

    //mark
    /// <summary>
    /// 金币移动
    /// </summary>
    //public void GoldMove()
    //{
    //    Vector3 currentPos = playerItems[minIndex].gameObject.transform.position;
    //    Vector3 targetPos = playerItems[maxIndex].gameObject.transform.position;

    //    for (int i = 0; i < GlobalDataScript.roomVo.playerAmounts; i++)
    //    {
    //        gold_SpriteClone = Instantiate(gold_Sprite);
    //        listGold.Add(gold_SpriteClone);
    //        listGold[i].transform.position = Vector3.MoveTowards(currentPos, targetPos, 300);
    //    }

    //    //延时调用销毁方法
    //    Invoke("DestoryFun", 5f);
    //}

    void DestoryFun()
    {
        for (int i = 0; i < listGold.Count; i++)
        {
            Destroy(listGold[i]);
        }

        isFinish = false;
    }
}

