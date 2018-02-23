using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
    [SerializeField]
    public class ClubStateVO
    {
        public string clubname;
        public int msgcode;
        public string msgstring;
        public int clubid;
        public int membercount;
        public int roomcount;
        public List<ClubRoomVO> listroom;
        public string icoimg;
        public int creditScore;
    }
}
