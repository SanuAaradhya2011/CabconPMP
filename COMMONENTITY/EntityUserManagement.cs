using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COMMONENTITY
{
   public class EntityUserManagement
    {

        string loginuserID;
        string loginpassword;
        string loginType;
        byte loginTypeIndex;
        string loginReserved1;
        string loginReserved2;
        string loginReserved3;
        string loginReserved4;
        string loginReserved5;

        public string LoginuserID
        {
            get { return loginuserID; }
            set { loginuserID = value; }
        }
        public string Loginpassword
        {
            get { return loginpassword; }
            set { loginpassword = value; }
        }   
        public string LogType
        {
            get { return loginType; }
            set { loginType = value; }
        }
        public byte LoginTypeIndex
        {
            get { return loginTypeIndex; }
            set { loginTypeIndex = value; }
        }
        public string LoginReserved1
        {
            get { return loginReserved1; }
            set { loginReserved1 = value; }
        }
        public string LoginReserved2
        {
            get { return loginReserved2; }
            set { loginReserved2 = value; }
        }
        public string LoginReserved3
        {
            get { return loginReserved3; }
            set { loginReserved3 = value; }
        }
        public string LoginReserved4
        {
            get { return loginReserved4; }
            set { loginReserved4 = value; }
        }
        public string LoginReserved5
        {
            get { return loginReserved5; }
            set { loginReserved5 = value; }
        }
    }
}
