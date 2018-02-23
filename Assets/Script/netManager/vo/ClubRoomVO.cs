using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
    [Serializable]
    public class ClubRoomVO
    {
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
        public int clubid = 0;          //俱乐部ID
        public int playnum;             //当前玩家
        public bool isgame;          //是否开始游戏
        public bool qiang = false;
        public bool ming = false;
        public int mengs = 1;
        public bool AA = false;
        public int qiangForNiu = -1;
        public int zhuForNiu = 0;
        public bool niu7fan;
        public bool cuoPai = false;
        public int createuui;//创建者ID
        public string createname;
        public string createimg;
        public int roomID;
        public int roundNumber;
    }
}
