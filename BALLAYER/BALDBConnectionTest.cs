using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DALLAYER;
using System.IO;
using COMMONENTITY;
namespace BALLAYER
{
    public class BALDBConnectionTest
    {
       ConnectionClass objcnn = new ConnectionClass();
    
       public bool IsDBConnected()
       {
           return objcnn.OpenConn(); 
       }
       public bool IsDBFileExist()
       {
           if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Configuration\\" + StaticVariables.DataBaseConnectionFile + ".xml")) return true;
           else return false;
       }
    }
    
}
 
