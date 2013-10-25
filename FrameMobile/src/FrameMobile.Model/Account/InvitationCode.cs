using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic.SqlGeneration.Schema;

namespace FrameMobile.Model.Account
{
    [Serializable]
    [SubSonicTableNameOverride("invitationcode")]
    public class InvitationCode : MySQLModelBase
    {
        [SubSonicStringLength(6)]
        public string Code { get; set; }
    }
}
