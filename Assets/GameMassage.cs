using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMassage : MonoBehaviour
{
    public Text ruleName;   // 玩法类型
    public Text playRule;   // 模式类型
    public Text baseScore;  // 底分
    public Text payRule;    // 支付类型
    public Text special;    // 特殊玩法

    public void Start()
    {
        if (GlobalDataScript.roomVo.ruleType == 1 || GlobalDataScript.roomVo.ruleType == 4 || GlobalDataScript.roomVo.ruleType == 5)
        {
            playRule.text = GlobalDataScript.roomVo.rules == 1
                ? "普通模式" : "扫雷模式";
            baseScore.text = GlobalDataScript.roomVo.diFen.ToString() + " 分";
            payRule.text = GlobalDataScript.roomVo.AA == true
                ? "AA制" : "房主支付";
            special.text = (GlobalDataScript.roomVo.multiplyingPower == 1
                ? "小倍(2,3,4,5)" : GlobalDataScript.roomVo.multiplyingPower == 2
                ? "中倍(6,9,12,15)" : "大倍(5,10,20,30)")
                + (GlobalDataScript.roomVo.roundNumber == 10 ?
                "10局" : "20局");
            if (GlobalDataScript.roomVo.ruleType == 1)
                ruleName.text = "看牌抢庄";
            else if (GlobalDataScript.roomVo.ruleType == 4)
                ruleName.text = "牛牛换庄";
            else if (GlobalDataScript.roomVo.ruleType == 5)
                ruleName.text = "房主霸王庄";

        }
        if (GlobalDataScript.roomVo.ruleType == 3 || GlobalDataScript.roomVo.ruleType == 6)
        {
            playRule.text = GlobalDataScript.roomVo.rules == 1
                ? "明牌模式" : GlobalDataScript.roomVo.rules == 2
                ? "暗牌模式" : "扫雷模式";
            baseScore.text = GlobalDataScript.roomVo.diFen.ToString() + " 分";
            payRule.text = GlobalDataScript.roomVo.AA == true
                ? "AA制" : "房主支付";
            special.text = (GlobalDataScript.roomVo.multiplyingPower == 1
                ? "小倍(2,3,4,5)" : GlobalDataScript.roomVo.multiplyingPower == 2
                ? "中倍(6,9,12,15)" : "大倍(5,10,20,30)")
                + (GlobalDataScript.roomVo.roundNumber == 10 ?
                "10局" : "20局");
            if (GlobalDataScript.roomVo.ruleType == 3)
                ruleName.text = "轮流当庄";
            else if (GlobalDataScript.roomVo.ruleType == 6)
                ruleName.text = "最大牌为庄";
        }
    }
}
