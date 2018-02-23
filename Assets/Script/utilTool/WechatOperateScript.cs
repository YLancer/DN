using UnityEngine;
using System.Collections;
using cn.sharesdk.unity3d;
using AssemblyCSharp;
using System.IO;
using UnityEngine.UI;
using System;
using LitJson;

/**
 * 微信操作
 */
public class WechatOperateScript : MonoBehaviour
{
    public ShareSDK shareSdk;
    private string picPath;
    private Text Debug_text = null;
    //
    void Start()
    {
        Debug_text = GameObject.Find("debug").GetComponent<Text>();
        Debug_text.text = ("开始微信登录");
        if (shareSdk != null)
        {
            shareSdk.showUserHandler = getUserInforCallback;
            shareSdk.shareHandler = onShareCallBack;
            shareSdk.authHandler = authResultHandler;
        }
    }


    void Update()
    {

    }



    public void cancelAuth()
    {
        shareSdk.CancelAuthorize(PlatformType.WeChat);
    }
    /**
	 * 登录，提供给button使用
	 * 
	 */
    public void login()
    {

        shareSdk.GetUserInfo(PlatformType.WeChat);
        //shareSdk.Authorize (PlatformType.WeChat);


    }

    /**
	 * 获取微信个人信息成功回调,登录
	 *
	 */
    public void authResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable data)
    {
        //TipsManagerScript.getInstance ().setTips ("获取个人信息成功");
        Debug_text.text = ("获取微信个人信息成功回调,登录 ");
        //TipsManagerScript.getInstance ().setTipsBig (data.toJson());
#if UNITY_IOS
			shareSdk.GetUserInfo(PlatformType.WeChat);
			return;
#else
        data = shareSdk.GetAuthInfo(PlatformType.WeChat);
#endif
        if (data != null)
        {

            LoginVo loginvo = new LoginVo();
            try
            {

                MyDebug.Log(data.toJson());
                //TipsManagerScript.getInstance ().setTipsBig (data.toJson());

                loginvo.openId = (string)data["openid"];
                loginvo.nickName = (string)data["nickname"];
                loginvo.headIcon = (string)data["icon"];
                loginvo.unionid = (string)data["unionid"];
                loginvo.province = (string)data["province"];
                loginvo.city = (string)data["city"];
                string sex = data["gender"].ToString();
                loginvo.sex = int.Parse(sex);
                loginvo.IP = GlobalDataScript.getInstance().getIpAddress();
                String msg = JsonMapper.ToJson(loginvo);

                PlayerPrefs.SetString("loginInfo", msg);

                CustomSocket.getInstance().sendMsg(new LoginRequest(msg));

                /*
				GlobalDataScript.loginVo = loginvo;
				GlobalDataScript.loginResponseData = new AvatarVO ();
				GlobalDataScript.loginResponseData.account = new Account ();
				GlobalDataScript.loginResponseData.account.city = loginvo.city;
				GlobalDataScript.loginResponseData.account.openid = loginvo.openId;
				MyDebug.Log(" loginvo.nickName:"+loginvo.nickName);
				GlobalDataScript.loginResponseData.account.nickname = loginvo.nickName;
				GlobalDataScript.loginResponseData.account.headicon = loginvo.headIcon;
				GlobalDataScript.loginResponseData.account.unionid = loginvo.city;
				GlobalDataScript.loginResponseData.account.sex = loginvo.sex;
				GlobalDataScript.loginResponseData.IP = loginvo.IP;

				//加载头像
				if(GlobalDataScript.loginVo.sex == 2){
					GlobalDataScript.getInstance().headSprite = Resources.Load ("xianlai/public_ui/head_img_female", typeof(Sprite)) as Sprite;
				}else {
					GlobalDataScript.getInstance().headSprite = Resources.Load ("xianlai/public_ui/head_img_male", typeof(Sprite)) as Sprite;
				}
				*/
            }
            catch (Exception e)
            {
                MyDebug.Log("微信接口有变动！" + e.Message);
                TipsManagerScript.getInstance().setTips("微信接口有变动！" + e.Message);
                return;
            }
        }
        else
        {
            TipsManagerScript.getInstance().setTips("微信登录失败");
        }




    }


    /**
	 * 获取微信个人信息成功回调,登录
	 *
	 */
    public void getUserInforCallback(int reqID, ResponseState state, PlatformType type, Hashtable data)
    {
        //TipsManagerScript.getInstance ().setTips ("获取个人信息成功");
        Debug_text.text = ("获取个人信息");
        shareSdk.CancelAuthorize(PlatformType.WeChat);
        if (data != null)
        {
            MyDebug.Log(data.toJson());
            LoginVo loginvo = new LoginVo();
            try
            {

                loginvo.openId = (string)data["openid"];
                loginvo.nickName = (string)data["nickname"];
                loginvo.headIcon = (string)data["headimgurl"];
                loginvo.unionid = (string)data["unionid"];
                loginvo.province = (string)data["province"];
                loginvo.city = (string)data["city"];
                string sex = data["sex"].ToString();
                loginvo.sex = int.Parse(sex);
                loginvo.IP = GlobalDataScript.getInstance().getIpAddress();
                String msg = JsonMapper.ToJson(loginvo);

                PlayerPrefs.SetString("loginInfo", msg);

                CustomSocket.getInstance().sendMsg(new LoginRequest(msg));


                /*
				GlobalDataScript.loginVo = loginvo;
				GlobalDataScript.loginResponseData = new AvatarVO ();
				GlobalDataScript.loginResponseData.account = new Account ();
				GlobalDataScript.loginResponseData.account.city = loginvo.city;
				GlobalDataScript.loginResponseData.account.openid = loginvo.openId;
				MyDebug.Log(" loginvo.nickName:"+loginvo.nickName);
				GlobalDataScript.loginResponseData.account.nickname = loginvo.nickName;
				GlobalDataScript.loginResponseData.account.headicon = loginvo.headIcon;
				GlobalDataScript.loginResponseData.account.unionid = loginvo.city;
				GlobalDataScript.loginResponseData.account.sex = loginvo.sex;
				GlobalDataScript.loginResponseData.IP = loginvo.IP;
				*/

            }
            catch (Exception e)
            {
                MyDebug.Log("微信接口有变动！" + e.Message);
                TipsManagerScript.getInstance().setTips("请先打开你的微信客户端");
                return;
            }
        }
        else
        {
            TipsManagerScript.getInstance().setTips("微信登录失败");
        }


    }


    /***
	 * 分享战绩成功回调
	 */
    public void onShareCallBack(int reqID, ResponseState state, PlatformType type, Hashtable result)
    {
        if (state == ResponseState.Success)
        {
            TipsManagerScript.getInstance().setTips("分享成功");
            if (GlobalDataScript.isShare)
            {
                CustomSocket.getInstance().sendMsg(new ShareRequest());
                GlobalDataScript.isShare = false;
            }

        }
        else if (state == ResponseState.Fail)
        {
            MyDebug.Log("shar fail :" + result["error_msg"]);
        }
    }

    /**
	 * 分享战绩、战绩
	 */
    public void shareAchievementToWeChat(PlatformType platformType)
    {
        StartCoroutine(GetCapture(platformType));
    }

    /**
	 * 执行分享到朋友圈的操作
	 */
    private void shareAchievement(PlatformType platformType)
    {
        ShareContent customizeShareParams = new ShareContent();
        customizeShareParams.SetText("");
        customizeShareParams.SetImagePath(picPath);
        customizeShareParams.SetShareType(ContentType.Image);
        customizeShareParams.SetObjectID("");
        customizeShareParams.SetShareContentCustomize(platformType, customizeShareParams);
        shareSdk.ShareContent(platformType, customizeShareParams);
    }

    /**
	 * 截屏
	 * 
	 * 
	 */
    private IEnumerator GetCapture(PlatformType platformType)
    {
        yield return new WaitForEndOfFrame();
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)

            picPath = Application.persistentDataPath;

        else if (Application.platform == RuntimePlatform.WindowsPlayer)

            picPath = Application.dataPath;

        else if (Application.platform == RuntimePlatform.WindowsEditor)

        {
            picPath = Application.dataPath;
            picPath = picPath.Replace("/Assets", null);
        }

        picPath = picPath + "/screencapture.png";

        MyDebug.Log("picPath:" + picPath);

        int width = Screen.width;
        int height = Screen.height;
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0, true);
        tex.Apply();
        byte[] imagebytes = tex.EncodeToPNG();//转化为png图
        tex.Compress(false);//对屏幕缓存进行压缩
        MyDebug.Log("imagebytes:" + imagebytes);
        if (File.Exists(picPath))
        {
            File.Delete(picPath);
        }
        File.WriteAllBytes(picPath, imagebytes);//存储png图
        Destroy(tex);
        shareAchievement(platformType);
    }




    public void inviteFriend()
    {
        //if(GlobalDataScript.roomVo != null){
        //	RoomCreateVo roomvo = GlobalDataScript.roomVo;
        //	GlobalDataScript.totalTimes = roomvo.roundNumber;
        //	GlobalDataScript.surplusTimes = roomvo.roundNumber;
        string str = "快来和我一起牛起来吧！"
            + "房间类型："
        + (GlobalDataScript.roomVo.gameType == 4 ? "德州扑克" :
        GlobalDataScript.roomVo.gameType == 1 ? "跑得快" :
        (GlobalDataScript.roomVo.ruleType == 1 ? "看牌抢庄" :
        GlobalDataScript.roomVo.ruleType == 3 ? "牛牛换庄" :
        GlobalDataScript.roomVo.ruleType == 4 ? "轮流当庄" :
        GlobalDataScript.roomVo.ruleType == 5 ? "房主霸王庄" : "最大牌为庄"))
        + "\n房间号：" + GlobalDataScript.roomVo.roomId;
        string title = "金牛王——绝无外挂公平公正的游戏！";

        //         string title;
        //if (roomvo.goldType) {
        //	title ="【" + Application.productName + "】" + "  房间号：训练场";
        //} else {
        //	title ="【" + Application.productName + "】" + "  房间号："+roomvo.roomId;
        //}
        ShareContent customizeShareParams = new ShareContent();
        customizeShareParams.SetTitle(title);
        customizeShareParams.SetText(str);
        //配置下载地址
        customizeShareParams.SetUrl("https://dafuvip.com/bY73q2");
        //配置分享logo
        // customizeShareParams.SetImageUrl(APIS.baseUrl + "/download/logo.png");
        customizeShareParams.SetShareType(ContentType.Webpage);//ContentType.Webpage
                                                               //customizeShareParams.SetObjectID("");
        shareSdk.ShowShareContentEditor(PlatformType.WeChat, customizeShareParams);
        //}
    }


    public void shareHaoYou(PlatformType platformtype)
    {
        string title = Application.productName + "";
        string str = "金牛王——绝无外挂公平公正的游戏！";
        ShareContent customizeShareParams = new ShareContent();
        customizeShareParams.SetTitle(title);
        customizeShareParams.SetText(str);
        //配置下载地址
        //customizeShareParams.SetUrl("https://dafuvip.com/bY73q2");
        //customizeShareParams.SetUrl(APIS.baseUrl + "/download/index.html");
        //配置分享logo
        //customizeShareParams.SetImageUrl(APIS.baseUrl + "/download/feed.png");
        customizeShareParams.SetShareType(ContentType.Webpage);
        //customizeShareParams.SetObjectID("");
        shareSdk.ShowShareContentEditor(platformtype, customizeShareParams);
    }


    private void testLogin()
    {
        CustomSocket.getInstance().sendMsg(new LoginRequest(null));
    }

}
