using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

// Make by Shise

namespace AssemblyCSharp
{
    /// <summary>
    /// 创建俱乐部请求
    /// </summary>
    public class CreateClubRequest : ClientRequest
    {
        public CreateClubRequest(CreateClubVo clubvo)
        {
            headCode = APIS.CREATE_CLUB_REQUEST;
            messageContent = JsonMapper.ToJson(clubvo);
        }
    }
}
