using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameMobile.Core
{
    public class ResultCode
    {
        public const int Successful = 0;
        public const int USER_NAME_IS_EMPTY = 1;
        public const int INVALID_CHARGE_CARD = 2;
        public const int BALANCE_IS_NOT_ENOUGH = 3;
        public const int ORDER_NOT_EXIST = 4;
        public const int User_Not_Exist = 5;
        public const int PHONE_ALREADY_USED = 6;
        public const int PHONE_ALREADY_BOUND = 7;
        public const int NO_RECORD = 8;
        public const int No_Record_Found = 993;
        public const int PROCESS_TIMEOUT = 994;
        public const int AMOUNT_INVALID = 995;
        public const int RQUEST_OUT_OF_DATE = 996;
        public const int Invalid_Parameter = 997;
        public const int ENCRYPTION_SIGN_INVALID = 998;
        public const int System_Error = 999;
    }
}
