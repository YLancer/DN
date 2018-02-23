using System;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
    [SerializeField]
    public class ClubAccountVO
    {
       
        public int msgcode;
        public string msgstring;
        public List<ClubAccount> listaccount;

    }

    public class ClubAccount
    {
        public int id;

        public int uuid;

        public string openid;

        public string nickname;

        public string headicon;

        public int creditscore;
    }

}

