using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COMMONENTITY
{
    public class EntityAssociation
    {

            string communicationPortName;
            string communicationBaudrate;
            string communicationBaudIndex;
            string communicationParity;
            string communicationDataBits;
            string communicationStopBits;
            string communicationMode;

            public string CommunicationPortName
            {
                get { return communicationPortName; }
                set { communicationPortName = value; }
            }
            public string CommunicationBaudrate
            {
                get { return communicationBaudrate; }
                set { communicationBaudrate = value; }
            }
            public string CommunicationBaudIndex
            {
                get { return communicationBaudIndex; }
                set { communicationBaudIndex = value; }
            }
            public string CommunicationParity
            {
                get { return communicationParity; }
                set { communicationParity = value; }
            }
            public string CommunicationDataBits
            {
                get { return communicationDataBits; }
                set { communicationDataBits = value; }
            }
            public string CommunicationStopBits
            {
                get { return communicationStopBits; }
                set { communicationStopBits = value; }
            }
            public string CommunicationMode
            {
                get { return communicationMode; }
                set { communicationMode = value; }
            }         
    }
}
