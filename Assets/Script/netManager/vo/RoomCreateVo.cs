using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	[Serializable]
	public class RoomCreateVo
	{
		public List<AvatarVO> playerList;
        //public string creditScore;

        public  bool hong;
		public int ma;
		public int roomId;
		public int roomType;//1转转；2、划水；3、长沙 ；4、广东
		/**局数**/
		public int roundNumber;
		public int currentRound;
		public bool sevenDouble;
		public int ziMo;//1：自摸胡；2、抢杠胡
		public int xiaYu;
		public string name;
		public bool addWordCard = true;
		public int magnification;
        public int gui;//鬼牌 0无鬼；1白板；2翻鬼
        public bool gangHu;//可抢杠胡
        public int guiPai=-1;
		public int guiPai2=-1;
		public bool gangHuQuanBao = false;
		public bool wuGuiX2 = false;
		public bool maGenDifen = false;
		public bool maGenGang = false;

		public int shangxiaFanType;
		public int diFen;
		public bool tongZhuang;
		public int pingHu;

		public bool keDianPao;//可点炮胡
		public bool lunZhuang;//轮庄或霸王庄

		//潮汕麻将
		//public bool gengZhuang = false;              //跟庄 //
		public bool genZhuang = false;              //跟庄
		public bool qiDui = false;
		public bool qiangGangHu = false;
		public bool pengpengHu = false;
		public bool qingYiSe = false;
		public bool gangShangKaiHua = false;         //杠上开花
		public bool haoHuaQiDui = false;             //豪华七对
		public bool shiSanYao = false;               //十三幺
		public bool tianDiHu = false;                //天地胡
		public bool shuangHaoHua = false;            //双豪华
		public bool quanFeng = false;                //全风
		public bool huaYaoJiu = false;               //花幺九
		public int fengDing = 9999;                  //封顶  默认100 不封顶
		public bool jiejiegao = false;                //节节高

		//新潮汕麻将
		public bool chongZhuang = false;
		public bool sanHaoHua = false;
		public bool shiBaLuoHan = false;
		public bool xiaoSanYuan = false;
		public bool xiaoSiXi = false;
		public bool daSanYuan = false;
		public bool daSiXi = false;
		public bool zhuaMa = false;
		public bool hunYiSe = false;

		//跑得快 gameType=1 .麻将全部为0
		public int gameType=0;
		public bool zhang16 = true;//16张
		public bool showPai = true;//显示牌
		public bool xian3 = true;//先出黑桃3
		//斗牛
		public bool qiang = false;
		public bool ming = false;
		public int mengs = 1;
		public bool AA = false;
		public int qiangForNiu = -1;
		public int zhuForNiu = 0;
		public bool niu7fan;
        public bool cuoPai=false;

        // Add by Shise
        public int ruleType;            // 创建房间游戏规则   1：看牌抢庄  2：闲家推注  3：牛牛换庄  4：轮流当庄  5：房主霸王庄  6：最大牌为庄
        public int playerAmounts;       // 房间可容纳人数 6：10；
        public int rules;               // 游戏模式 1：普通模式1   2：普通模式2   3：扫雷模式
        public int hogMultiples;        // 抢庄基础倍率   1：1倍   2：2倍   3：3倍   4：4倍
        public int multiplyingPower;    // 抢庄具体倍率   1：      2：      3：
        public bool niuNiuShangZhuang;  // 牛牛上庄
        public bool niuNiuHuanZhuang;   // 牛牛换庄
        public bool trusteeship;        // 托管
        public bool special;            // 特殊牌型
        public bool creditScoreIsOn;    // 信用分是否开启
        public int creditScoreMultiply; // 信用分倍率
        public int clubid = 0;
        // End

        //德州扑克
        public int initFen_dzpk;

		//添加金币场
		public bool goldType = false;
        public RoomCreateVo()
		{

		}
	}
}

