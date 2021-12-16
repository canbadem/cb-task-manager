using System;
using System.Collections.Generic;
using System.Text;

namespace CBTwelveInterface
{
    public class TwelveUserInfo
    {
        public bool IsImpersonating;
        public bool RequiresPasswordChange;
        public string ChangePasswordToken;
        public bool HasMissingInfo;
        public bool IsFirstLogin;
    }
}
