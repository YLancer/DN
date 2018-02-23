using System;

namespace AssemblyCSharp
{
    public class HogMultipleRequest : ClientRequest
    {
        public HogMultipleRequest(int chooseHogMultiple)
        {
           // headCode = APIS.QIANGBEI_DN_REQUEST;
            if (GlobalDataScript.loginResponseData != null)
            {
                messageContent = "" + chooseHogMultiple;//GlobalDataScript.loginResponseData.account.uuid +
            }
        }
    }
}

