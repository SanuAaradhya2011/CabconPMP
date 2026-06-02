using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities
{
    public class DisplayParameterList
    {
        public Dictionary<string, int> GetDisplayParameterList()
        {
            Dictionary<string, int> dictionaryDisplayList = new Dictionary<string, int>();
            int ItemIDX = 1;
            dictionaryDisplayList.Add("Cummulative Active Energy", ItemIDX++);//1
            dictionaryDisplayList.Add("Rate 1 Active Energy", ItemIDX++);//2
            dictionaryDisplayList.Add("Rate 2 Active Energy", ItemIDX++);//3
            dictionaryDisplayList.Add("Rate 3 Active Energy", ItemIDX++);//4
            dictionaryDisplayList.Add("Rate 4 Active Energy", ItemIDX++);//5
            dictionaryDisplayList.Add("Rate 5 Active Energy", ItemIDX++);//6
            dictionaryDisplayList.Add("Rate 6 Active Energy", ItemIDX++);//7
            dictionaryDisplayList.Add("Active Rate", ItemIDX++);//8
            dictionaryDisplayList.Add("Instant Voltage", ItemIDX++);//9
            dictionaryDisplayList.Add("Phase Active Power", ItemIDX++);//10
            dictionaryDisplayList.Add("Neutral Active Power", ItemIDX++);//11
            dictionaryDisplayList.Add("High Resolution Active Energy", ItemIDX++);//12
            dictionaryDisplayList.Add("Demand Reset Counter", ItemIDX++);//13
            dictionaryDisplayList.Add("Fraud Reset Counter", ItemIDX++);//14
            dictionaryDisplayList.Add("Time", ItemIDX++);//15
            dictionaryDisplayList.Add("Date", ItemIDX++);//16
            dictionaryDisplayList.Add("Blank Test", ItemIDX++);//17
            dictionaryDisplayList.Add("All Segement Test", ItemIDX++);//18
            dictionaryDisplayList.Add("Odd Segement Test", ItemIDX++);//19
            dictionaryDisplayList.Add("Even Segement Test", ItemIDX++);//20

            dictionaryDisplayList.Add("Instant Phase Current", ItemIDX++);//21
            dictionaryDisplayList.Add("Instant Neutral Current", ItemIDX++);//22
            dictionaryDisplayList.Add("Present Active MD", ItemIDX++);//23
            
            dictionaryDisplayList.Add("Present Month Consumption", ItemIDX++);//24
            dictionaryDisplayList.Add("Instant PF", ItemIDX++);//25
            dictionaryDisplayList.Add("Instant Frequency", ItemIDX++);//26
            dictionaryDisplayList.Add("AC Magnet Field Count", ItemIDX++);//27
            dictionaryDisplayList.Add("Main Battery Voltage", ItemIDX++);//28
            dictionaryDisplayList.Add("RTC Battery Voltage", ItemIDX++);//29
            dictionaryDisplayList.Add("TLV Voltage", ItemIDX++);//30
            dictionaryDisplayList.Add("Billing Active Power", ItemIDX++);//31
            dictionaryDisplayList.Add("Billing Active Energy", ItemIDX++);//32
            dictionaryDisplayList.Add("Billing Power-On Minutes", ItemIDX++);//33
            dictionaryDisplayList.Add("Cummulative Power-On Minutes", ItemIDX++);//34
            //dictionaryDisplayList.Add("CUM POWEROFF MINUTES", ItemIDX++);//35
            dictionaryDisplayList.Add("Billing Average PF", ItemIDX++);//36
            dictionaryDisplayList.Add("Meter ID", ItemIDX++);//37
            dictionaryDisplayList.Add("Meter ID LSB", ItemIDX++);//38
            dictionaryDisplayList.Add("RATE 1 Acitve MD", ItemIDX++);//39
            dictionaryDisplayList.Add("RATE 2 Acitve MD", ItemIDX++);//40
            dictionaryDisplayList.Add("RATE 3 Acitve MD", ItemIDX++);//41
            dictionaryDisplayList.Add("RATE 4 Acitve MD", ItemIDX++);//42
            dictionaryDisplayList.Add("RATE 5 Acitve MD", ItemIDX++);//43
            dictionaryDisplayList.Add("RATE 6 Acitve MD", ItemIDX++);//44

            dictionaryDisplayList.Add("Voltage Comp Counts", ItemIDX++);//45
            dictionaryDisplayList.Add("Current Comp Counts", ItemIDX++);//46
            dictionaryDisplayList.Add("Power-Fail Comp Counts", ItemIDX++);//47
            dictionaryDisplayList.Add("Transaction Comp Counts", ItemIDX++);//48
            dictionaryDisplayList.Add("Other Comp Counts", ItemIDX++);//49
            dictionaryDisplayList.Add("Non-Rollover Comp Counts", ItemIDX++);//50
            dictionaryDisplayList.Add("Connect-Disconnect Comp Counts", ItemIDX++);//51

 
            dictionaryDisplayList.Add("Total Tamper Conts", ItemIDX++);//52
            dictionaryDisplayList.Add("ABC String", ItemIDX++);//53
            
            dictionaryDisplayList.Add("Signed PF", ItemIDX++);//54
            dictionaryDisplayList.Add("Present Average PF", ItemIDX++);//55
            dictionaryDisplayList.Add("Last Bill Date", ItemIDX++);//56
            dictionaryDisplayList.Add("Last Bill Time", ItemIDX++);//57
            dictionaryDisplayList.Add("Cummulative Apparent Energy", ItemIDX++);//58
            dictionaryDisplayList.Add("Cummulative Reactive Energy-Lag", ItemIDX++);//59
            dictionaryDisplayList.Add("Cummulative Reactive Energy-Lead", ItemIDX++);//60
            dictionaryDisplayList.Add("Instant Apparent Power", ItemIDX++);//61
            dictionaryDisplayList.Add("Instant Reactive Power", ItemIDX++);//62
            dictionaryDisplayList.Add("High Resolution Apparent Energy", ItemIDX++);//63
            dictionaryDisplayList.Add("High Resolution Reactive Energy-Lag", ItemIDX++);//64
            dictionaryDisplayList.Add("High Resolution Reactive Energy-Lead", ItemIDX++);//65
            dictionaryDisplayList.Add("Present Apparent MD", ItemIDX++);//66
            dictionaryDisplayList.Add("Billing Apparent Power", ItemIDX++);//67
            dictionaryDisplayList.Add("Billing Apparent Energy", ItemIDX++);//68
            
            dictionaryDisplayList.Add("Case Tamper First Occurrance", ItemIDX++);//69
            dictionaryDisplayList.Add("Active Instant Current", ItemIDX++);//70
            dictionaryDisplayList.Add("Active Instant Power", ItemIDX++);//71
            dictionaryDisplayList.Add("Apparent Rate", ItemIDX++);//72
            dictionaryDisplayList.Add("Rate 1 Apparent MD", ItemIDX++);//73
            dictionaryDisplayList.Add("Rate 2 Apparent MD", ItemIDX++);//74
            dictionaryDisplayList.Add("Rate 3 Apparent MD", ItemIDX++);//75
            dictionaryDisplayList.Add("Rate 4 Apparent MD", ItemIDX++);//76
            dictionaryDisplayList.Add("Rate 5 Apparent MD", ItemIDX++);//77
            dictionaryDisplayList.Add("Rate 6 Apparent MD", ItemIDX++);//78
            dictionaryDisplayList.Add("Rate 1 Apparent Energy", ItemIDX++);//79
            dictionaryDisplayList.Add("Rate 2 Apparent Energy", ItemIDX++);//80
            dictionaryDisplayList.Add("Rate 3 Apparent Energy", ItemIDX++);//81
            dictionaryDisplayList.Add("Rate 4 Apparent Energy", ItemIDX++);//82
            dictionaryDisplayList.Add("Rate 5 Apparent Energy", ItemIDX++);//83
            dictionaryDisplayList.Add("Rate 6 Apparent Energy", ItemIDX++);//84
            dictionaryDisplayList.Add("Active Tariff Price", ItemIDX++);//85
            dictionaryDisplayList.Add("Billing Reactive Energy-Lag", ItemIDX++);//86
            dictionaryDisplayList.Add("Billing Reactive Energy-Lead", ItemIDX++);//87

            dictionaryDisplayList.Add("Voltage Comp Latest Event", ItemIDX++);//88
            dictionaryDisplayList.Add("Current Comp Latest Event", ItemIDX++);//89
            dictionaryDisplayList.Add("Power-Fail Comp Latest Event", ItemIDX++);//90
            dictionaryDisplayList.Add("Transaction Comp Latest Event", ItemIDX++);//91
            dictionaryDisplayList.Add("Other Comp Latest Event", ItemIDX++);//92
            dictionaryDisplayList.Add("Non-Rollover Comp Latest Event", ItemIDX++);//93
            dictionaryDisplayList.Add("Connect-Disconnect Comp Latest Event", ItemIDX++);//94

            dictionaryDisplayList.Add("Comms Remove Tamper First Occurrance", ItemIDX++);//95
            dictionaryDisplayList.Add("Relay Malfunction Tamper First Occurrance", ItemIDX++);//96
            dictionaryDisplayList.Add("RS 485 Address", ItemIDX++);//97
            
 
            return dictionaryDisplayList;
      }

        public Dictionary<string, int> GetDisplayParameterList_MicroStarDLMS()
        {
            Dictionary<string, int> dictionaryDisplayList = new Dictionary<string, int>();
            int ItemIDX = 1;
            dictionaryDisplayList.Add("Cummulative Active Energy", ItemIDX++);//1
            dictionaryDisplayList.Add("Rate 1 Active Energy", ItemIDX++);//2
            dictionaryDisplayList.Add("Rate 2 Active Energy", ItemIDX++);//3
            dictionaryDisplayList.Add("Rate 3 Active Energy", ItemIDX++);//4
            dictionaryDisplayList.Add("Rate 4 Active Energy", ItemIDX++);//5
            dictionaryDisplayList.Add("Rate 5 Active Energy", ItemIDX++);//6
            dictionaryDisplayList.Add("Rate 6 Active Energy", ItemIDX++);//7
            dictionaryDisplayList.Add("Active Rate", ItemIDX++);//8
            dictionaryDisplayList.Add("Instant Voltage", ItemIDX++);//9
            dictionaryDisplayList.Add("Phase Active Power", ItemIDX++);//10
            dictionaryDisplayList.Add("Neutral Active Power", ItemIDX++);//11
            dictionaryDisplayList.Add("High Resolution Active Energy", ItemIDX++);//12
            dictionaryDisplayList.Add("Demand Reset Counter", ItemIDX++);//13
            dictionaryDisplayList.Add("Fraud Reset Counter", ItemIDX++);//14
            dictionaryDisplayList.Add("Time", ItemIDX++);//15
            dictionaryDisplayList.Add("Date", ItemIDX++);//16
            dictionaryDisplayList.Add("Blank Test", ItemIDX++);//17
            dictionaryDisplayList.Add("All Segement Test", ItemIDX++);//18
            dictionaryDisplayList.Add("Odd Segement Test", ItemIDX++);//19
            dictionaryDisplayList.Add("Even Segement Test", ItemIDX++);//20

            dictionaryDisplayList.Add("Instant Phase Current", ItemIDX++);//21
            dictionaryDisplayList.Add("Instant Neutral Current", ItemIDX++);//22
            dictionaryDisplayList.Add("Present Active MD", ItemIDX++);//23

            dictionaryDisplayList.Add("Present Month Consumption", ItemIDX++);//24
            dictionaryDisplayList.Add("Instant PF", ItemIDX++);//25
            dictionaryDisplayList.Add("Instant Frequency", ItemIDX++);//26
            dictionaryDisplayList.Add("AC Magnet Field Count", ItemIDX++);//27
            dictionaryDisplayList.Add("Main Battery Voltage", ItemIDX++);//28
            dictionaryDisplayList.Add("RTC Battery Voltage", ItemIDX++);//29
            dictionaryDisplayList.Add("TLV Voltage", ItemIDX++);//30
            dictionaryDisplayList.Add("Billing Active Power", ItemIDX++);//31
            dictionaryDisplayList.Add("Billing Active Energy", ItemIDX++);//32
            dictionaryDisplayList.Add("Billing Power-On Minutes", ItemIDX++);//33
            dictionaryDisplayList.Add("Cummulative Power-On Minutes", ItemIDX++);//34
            //dictionaryDisplayList.Add("CUM POWEROFF MINUTES", ItemIDX++);//35  ----Removed----------
            dictionaryDisplayList.Add("Billing Average PF", ItemIDX++);//35
            dictionaryDisplayList.Add("Meter ID", ItemIDX++);//36
            dictionaryDisplayList.Add("Meter ID LSB", ItemIDX++);//37
            dictionaryDisplayList.Add("RATE 1 Acitve MD", ItemIDX++);//38
            dictionaryDisplayList.Add("RATE 2 Acitve MD", ItemIDX++);//39
            dictionaryDisplayList.Add("RATE 3 Acitve MD", ItemIDX++);//40
            dictionaryDisplayList.Add("RATE 4 Acitve MD", ItemIDX++);//41
            dictionaryDisplayList.Add("RATE 5 Acitve MD", ItemIDX++);//42
            dictionaryDisplayList.Add("RATE 6 Acitve MD", ItemIDX++);//43

            dictionaryDisplayList.Add("Voltage Comp Counts", ItemIDX++);//44
            dictionaryDisplayList.Add("Current Comp Counts", ItemIDX++);//45
            dictionaryDisplayList.Add("Power-Fail Comp Counts", ItemIDX++);//46
            dictionaryDisplayList.Add("Transaction Comp Counts", ItemIDX++);//47
            dictionaryDisplayList.Add("Other Comp Counts", ItemIDX++);//48
            dictionaryDisplayList.Add("Non-Rollover Comp Counts", ItemIDX++);//49
            dictionaryDisplayList.Add("Connect-Disconnect Comp Counts", ItemIDX++);//50


            dictionaryDisplayList.Add("Total Tamper Conts", ItemIDX++);//51
            dictionaryDisplayList.Add("ABC String", ItemIDX++);//52

            dictionaryDisplayList.Add("Signed PF", ItemIDX++);//53
            dictionaryDisplayList.Add("Present Average PF", ItemIDX++);//54
            dictionaryDisplayList.Add("Last Bill Date", ItemIDX++);//55
            dictionaryDisplayList.Add("Last Bill Time", ItemIDX++);//56
            dictionaryDisplayList.Add("Cummulative Apparent Energy", ItemIDX++);//57
            dictionaryDisplayList.Add("Cummulative Reactive Energy-Lag", ItemIDX++);//58
            dictionaryDisplayList.Add("Cummulative Reactive Energy-Lead", ItemIDX++);//59
            dictionaryDisplayList.Add("Instant Apparent Power", ItemIDX++);//60
            dictionaryDisplayList.Add("Instant Reactive Power", ItemIDX++);//61
            dictionaryDisplayList.Add("High Resolution Apparent Energy", ItemIDX++);//62
            dictionaryDisplayList.Add("High Resolution Reactive Energy-Lag", ItemIDX++);//63
            dictionaryDisplayList.Add("High Resolution Reactive Energy-Lead", ItemIDX++);//64
            dictionaryDisplayList.Add("Present Apparent MD", ItemIDX++);//65
            dictionaryDisplayList.Add("Billing Apparent Power", ItemIDX++);//66
            dictionaryDisplayList.Add("Billing Apparent Energy", ItemIDX++);//67

            dictionaryDisplayList.Add("Case Tamper First Occurrance", ItemIDX++);//68
            dictionaryDisplayList.Add("Active Instant Current", ItemIDX++);//69
            dictionaryDisplayList.Add("Active Instant Power", ItemIDX++);//70
            dictionaryDisplayList.Add("Apparent Rate", ItemIDX++);//71
            dictionaryDisplayList.Add("Rate 1 Apparent MD", ItemIDX++);//72
            dictionaryDisplayList.Add("Rate 2 Apparent MD", ItemIDX++);//73
            dictionaryDisplayList.Add("Rate 3 Apparent MD", ItemIDX++);//74
            dictionaryDisplayList.Add("Rate 4 Apparent MD", ItemIDX++);//75
            dictionaryDisplayList.Add("Rate 5 Apparent MD", ItemIDX++);//76
            dictionaryDisplayList.Add("Rate 6 Apparent MD", ItemIDX++);//77
            dictionaryDisplayList.Add("Rate 1 Apparent Energy", ItemIDX++);//78
            dictionaryDisplayList.Add("Rate 2 Apparent Energy", ItemIDX++);//79
            dictionaryDisplayList.Add("Rate 3 Apparent Energy", ItemIDX++);//80
            dictionaryDisplayList.Add("Rate 4 Apparent Energy", ItemIDX++);//81
            dictionaryDisplayList.Add("Rate 5 Apparent Energy", ItemIDX++);//82
            dictionaryDisplayList.Add("Rate 6 Apparent Energy", ItemIDX++);//83
            dictionaryDisplayList.Add("Active Tariff Price", ItemIDX++);//84
            dictionaryDisplayList.Add("Billing Reactive Energy-Lag", ItemIDX++);//85
            dictionaryDisplayList.Add("Billing Reactive Energy-Lead", ItemIDX++);//86

            dictionaryDisplayList.Add("Voltage Comp Latest Event", ItemIDX++);//87
            dictionaryDisplayList.Add("Current Comp Latest Event", ItemIDX++);//88
            dictionaryDisplayList.Add("Power-Fail Comp Latest Event", ItemIDX++);//89
            dictionaryDisplayList.Add("Transaction Comp Latest Event", ItemIDX++);//90
            dictionaryDisplayList.Add("Other Comp Latest Event", ItemIDX++);//91
            dictionaryDisplayList.Add("Non-Rollover Comp Latest Event", ItemIDX++);//92
            dictionaryDisplayList.Add("Connect-Disconnect Comp Latest Event", ItemIDX++);//93

            dictionaryDisplayList.Add("Comms Remove Tamper First Occurrance", ItemIDX++);//94
            dictionaryDisplayList.Add("Relay Malfunction Tamper First Occurrance", ItemIDX++);//95

            dictionaryDisplayList.Add("Apparent Billing Rate 1 Energy", ItemIDX++);//96
            dictionaryDisplayList.Add("Apparent Billing Rate 2 Energy", ItemIDX++);//97
            dictionaryDisplayList.Add("Apparent Billing Rate 3 Energy", ItemIDX++);//98
            dictionaryDisplayList.Add("Apparent Billing Rate 4 Energy", ItemIDX++);//99
            dictionaryDisplayList.Add("Apparent Billing Rate 5 Energy", ItemIDX++);//100
            dictionaryDisplayList.Add("Apparent Billing Rate 6 Energy", ItemIDX++);//101

            dictionaryDisplayList.Add("Billing Power Off Hours", ItemIDX++);//102
            dictionaryDisplayList.Add("Total Power Off Hours", ItemIDX++);//103  
            dictionaryDisplayList.Add("Magnet Counts", ItemIDX++);//104

            dictionaryDisplayList.Add("Cumulative MD Active", ItemIDX++);//105
            dictionaryDisplayList.Add("Cumulative MD Apparent", ItemIDX++);//106  
            dictionaryDisplayList.Add("Latest Tamper Occ-Res Details", ItemIDX++);//107

             dictionaryDisplayList.Add("Billing kWh TOD1", ItemIDX++);//108
             dictionaryDisplayList.Add("Billing kWh TOD2", ItemIDX++);//109
             dictionaryDisplayList.Add("Billing kWh TOD3", ItemIDX++);//110
             dictionaryDisplayList.Add("Billing kWh TOD4", ItemIDX++);//111
             dictionaryDisplayList.Add("Billing kWh TOD5", ItemIDX++);//112
             dictionaryDisplayList.Add("Billing kWh TOD6", ItemIDX++);//113


             dictionaryDisplayList.Add("Billing MD KVA TOD1", ItemIDX++);//114
             dictionaryDisplayList.Add("Billing MD KVA TOD2", ItemIDX++);//115
             dictionaryDisplayList.Add("Billing MD KVA TOD3", ItemIDX++);//116
             dictionaryDisplayList.Add("Billing MD KVA TOD4", ItemIDX++);//117
             dictionaryDisplayList.Add("Billing MD KVA TOD5", ItemIDX++);//118
             dictionaryDisplayList.Add("Billing MD KVA TOD6", ItemIDX++);//119
          
             dictionaryDisplayList.Add("Present Month Power on-Hours", ItemIDX++);//120
             dictionaryDisplayList.Add("Power off-Hours Since Last Reset", ItemIDX++);//121
             dictionaryDisplayList.Add("Metering Current", ItemIDX++);//122
             dictionaryDisplayList.Add("Billing Power on-Hours", ItemIDX++);//123
             dictionaryDisplayList.Add("First Tamper Occurrence", ItemIDX++);//124

             dictionaryDisplayList.Add("L2L Billing kWh", ItemIDX++);//125
             dictionaryDisplayList.Add("L2L Billing kVAh", ItemIDX++);//126
             dictionaryDisplayList.Add("L2L Billing APF", ItemIDX++);//127
             dictionaryDisplayList.Add("L2L Billing kW", ItemIDX++);//128

             dictionaryDisplayList.Add("Cum Active Export Energy", ItemIDX++);//129
             dictionaryDisplayList.Add("High Resolution Active Export Lead", ItemIDX++);//130

             dictionaryDisplayList.Add("Present Reactive Lag MD", ItemIDX++); //131
             dictionaryDisplayList.Add("Present Reactive Lead MD", ItemIDX++); //132 
            
             return dictionaryDisplayList;
        }

        public Dictionary<string, int> GetDisplayParameterList_MicroStarDLMS128()
        {
            Dictionary<string, int> dictionaryDisplayList = new Dictionary<string, int>();
            int ItemIDX = 1;
            dictionaryDisplayList.Add("Cummulative Active Energy", ItemIDX++);//1
            dictionaryDisplayList.Add("Dynamic TOD Active Energy", ItemIDX++);//2
            //changes for VIM128K
            //dictionaryDisplayList.Add("Rate 2 Active Energy", ItemIDX++)//3; 
            ItemIDX++;
            //dictionaryDisplayList.Add("Rate 3 Active Energy", ItemIDX++);//4
            ItemIDX++;
            //dictionaryDisplayList.Add("Rate 4 Active Energy", ItemIDX++);//5
            ItemIDX++;
            //dictionaryDisplayList.Add("Rate 5 Active Energy", ItemIDX++);//6
            ItemIDX++;
            //dictionaryDisplayList.Add("Rate 6 Active Energy", ItemIDX++);//7
            ItemIDX++;

            dictionaryDisplayList.Add("Active Rate", ItemIDX++);//8
            dictionaryDisplayList.Add("Instant Voltage", ItemIDX++);//9
            dictionaryDisplayList.Add("Phase Active Power", ItemIDX++);//10
            dictionaryDisplayList.Add("Neutral Active Power", ItemIDX++);//11
            dictionaryDisplayList.Add("High Resolution Active Energy", ItemIDX++);//12
            dictionaryDisplayList.Add("Demand Reset Counter", ItemIDX++);//13
            dictionaryDisplayList.Add("Fraud Reset Counter", ItemIDX++);//14
            dictionaryDisplayList.Add("Time", ItemIDX++);//15
            dictionaryDisplayList.Add("Date", ItemIDX++);//16
            dictionaryDisplayList.Add("Blank Test", ItemIDX++);//17
            dictionaryDisplayList.Add("All Segement Test", ItemIDX++);//18
            dictionaryDisplayList.Add("Odd Segement Test", ItemIDX++);//19
            dictionaryDisplayList.Add("Even Segement Test", ItemIDX++);//20

            dictionaryDisplayList.Add("Instant Phase Current", ItemIDX++);//21
            dictionaryDisplayList.Add("Instant Neutral Current", ItemIDX++);//22
            dictionaryDisplayList.Add("Present Active MD", ItemIDX++);//23



            //dictionaryDisplayList.Add("Present Month Consumption", ItemIDX++);//24
            ItemIDX++;
            //dictionaryDisplayList.Add("Instant PF", ItemIDX++);//25
            ItemIDX++;
            dictionaryDisplayList.Add("Instant Frequency", ItemIDX++);//26
            //dictionaryDisplayList.Add("AC Magnet Field Count", ItemIDX++);//27
            ItemIDX++;
            dictionaryDisplayList.Add("Metering Power", ItemIDX++);//28

            dictionaryDisplayList.Add("Battery Status", ItemIDX++);//29
            //dictionaryDisplayList.Add("TLV Voltage", ItemIDX++);//30
            ItemIDX++;
            dictionaryDisplayList.Add("Billing Active Power", ItemIDX++);//31
            dictionaryDisplayList.Add("Billing Active Energy", ItemIDX++);//32
            dictionaryDisplayList.Add("Billing Power-On Minutes", ItemIDX++);//33
            dictionaryDisplayList.Add("Cummulative Power-On Minutes", ItemIDX++);//34
            //dictionaryDisplayList.Add("CUM POWEROFF MINUTES", ItemIDX++);//35  ----Removed----------
            dictionaryDisplayList.Add("Billing Average PF", ItemIDX++);//35
            dictionaryDisplayList.Add("Meter ID", ItemIDX++);//36
            dictionaryDisplayList.Add("Meter ID LSB", ItemIDX++);//37
            dictionaryDisplayList.Add("Dynamic TOD Active MD", ItemIDX++);//38
            //dictionaryDisplayList.Add("RATE 2 Acitve MD", ItemIDX++);//39
            ItemIDX++;
            //dictionaryDisplayList.Add("RATE 3 Acitve MD", ItemIDX++);//40
            ItemIDX++;
            //dictionaryDisplayList.Add("RATE 4 Acitve MD", ItemIDX++);//41
            ItemIDX++;
            //dictionaryDisplayList.Add("RATE 5 Acitve MD", ItemIDX++);//42
            ItemIDX++;
            //dictionaryDisplayList.Add("RATE 6 Acitve MD", ItemIDX++);//43
            ItemIDX++;

            //dictionaryDisplayList.Add("Voltage Comp Counts", ItemIDX++);//44
            ItemIDX++;
            dictionaryDisplayList.Add("Current Comp Count", ItemIDX++);//45
            dictionaryDisplayList.Add("Power-Fail Comp Count", ItemIDX++);//46
            dictionaryDisplayList.Add("Transaction Comp Count", ItemIDX++);//47
            dictionaryDisplayList.Add("Other Comp Count", ItemIDX++);//48
            dictionaryDisplayList.Add("Non-Rollover Comp Count", ItemIDX++);//49
            //dictionaryDisplayList.Add("Connect-Disconnect Comp Counts", ItemIDX++);//50
            ItemIDX++;


            dictionaryDisplayList.Add("Total Tamper Count with Latest Occurence", ItemIDX++);//51
            dictionaryDisplayList.Add("ABC String Numeric", ItemIDX++);//52

            dictionaryDisplayList.Add("Signed PF", ItemIDX++);//53
            dictionaryDisplayList.Add("Present Average PF", ItemIDX++);//54
            dictionaryDisplayList.Add("Last Bill Date", ItemIDX++);//55
            dictionaryDisplayList.Add("Last Bill Time", ItemIDX++);//56
            dictionaryDisplayList.Add("Cummulative Apparent Energy", ItemIDX++);//57
            dictionaryDisplayList.Add("Cummulative Reactive Energy-Lag", ItemIDX++);//58
            dictionaryDisplayList.Add("Cummulative Reactive Energy-Lead", ItemIDX++);//59
            dictionaryDisplayList.Add("Instant Apparent Power", ItemIDX++);//60
            dictionaryDisplayList.Add("Instant Reactive Power", ItemIDX++);//61
            dictionaryDisplayList.Add("High Resolution Apparent Energy", ItemIDX++);//62
            dictionaryDisplayList.Add("High Resolution Reactive Energy-Lag", ItemIDX++);//63
            dictionaryDisplayList.Add("High Resolution Reactive Energy-Lead", ItemIDX++);//64
            dictionaryDisplayList.Add("Present Apparent MD", ItemIDX++);//65
            dictionaryDisplayList.Add("Billing Apparent Power", ItemIDX++);//66
            dictionaryDisplayList.Add("Billing Apparent Energy", ItemIDX++);//67

            dictionaryDisplayList.Add("Case Tamper First Occurrance", ItemIDX++);//68

            //dictionaryDisplayList.Add("Active Instant Current", ItemIDX++);//69
            ItemIDX++;
            //dictionaryDisplayList.Add("Active Instant Power", ItemIDX++);//70
            ItemIDX++;
            //dictionaryDisplayList.Add("Apparent Rate", ItemIDX++);//71
            ItemIDX++;
            dictionaryDisplayList.Add("Dynamic TOD Apparent MD", ItemIDX++);//72
            //dictionaryDisplayList.Add("Rate 2 Apparent MD", ItemIDX++);//73
            ItemIDX++;
            //dictionaryDisplayList.Add("Rate 3 Apparent MD", ItemIDX++);//74
            ItemIDX++;
            //dictionaryDisplayList.Add("Rate 4 Apparent MD", ItemIDX++);//75
            ItemIDX++;
            //dictionaryDisplayList.Add("Rate 5 Apparent MD", ItemIDX++);//76
            ItemIDX++;
            //dictionaryDisplayList.Add("Rate 6 Apparent MD", ItemIDX++);//77
            ItemIDX++;
            dictionaryDisplayList.Add("Dynamic TOD Apparent Energy", ItemIDX++);//78
            //dictionaryDisplayList.Add("Rate 2 Apparent Energy", ItemIDX++);//79
            ItemIDX++;
            //dictionaryDisplayList.Add("Rate 3 Apparent Energy", ItemIDX++);//80
            ItemIDX++;
            //dictionaryDisplayList.Add("Rate 4 Apparent Energy", ItemIDX++);//81
            ItemIDX++;
            //dictionaryDisplayList.Add("Rate 5 Apparent Energy", ItemIDX++);//82
            ItemIDX++;
            //dictionaryDisplayList.Add("Rate 6 Apparent Energy", ItemIDX++);//83
            ItemIDX++;
            //dictionaryDisplayList.Add("Active Tariff Price", ItemIDX++);//84
            ItemIDX++;
            dictionaryDisplayList.Add("Billing Reactive Energy-Lag", ItemIDX++);//85
            dictionaryDisplayList.Add("Billing Reactive Energy-Lead", ItemIDX++);//86

            //dictionaryDisplayList.Add("Voltage Comp Latest Event", ItemIDX++);//87
            ItemIDX++;
            dictionaryDisplayList.Add("Current Comp Latest Event", ItemIDX++);//88
            dictionaryDisplayList.Add("Power-Fail Comp Latest Event", ItemIDX++);//89
            dictionaryDisplayList.Add("Transaction Comp Latest Event", ItemIDX++);//90
            dictionaryDisplayList.Add("Other Comp Latest Event", ItemIDX++);//91
            dictionaryDisplayList.Add("Non-Rollover Comp Latest Event", ItemIDX++);//92
            //dictionaryDisplayList.Add("Connect-Disconnect Comp Latest Event", ItemIDX++);//93
            ItemIDX++;

            //dictionaryDisplayList.Add("Comms Remove Tamper First Occurrance", ItemIDX++);//94
            ItemIDX++;
            //dictionaryDisplayList.Add("Relay Malfunction Tamper First Occurrance", ItemIDX++);//95
            ItemIDX++;

            dictionaryDisplayList.Add("Dynamic Rate Billing kVAh", ItemIDX++);//96
            //dictionaryDisplayList.Add("Apparent Billing Rate 2 Energy", ItemIDX++);//97
            ItemIDX++;
            //dictionaryDisplayList.Add("Apparent Billing Rate 3 Energy", ItemIDX++);//98
            ItemIDX++;
            //dictionaryDisplayList.Add("Apparent Billing Rate 4 Energy", ItemIDX++);//99
            ItemIDX++;
           // dictionaryDisplayList.Add("Apparent Billing Rate 5 Energy", ItemIDX++);//100
            ItemIDX++;
           // dictionaryDisplayList.Add("Apparent Billing Rate 6 Energy", ItemIDX++);//101
            ItemIDX++;

            dictionaryDisplayList.Add("Billing Power Off Hours", ItemIDX++);//102
            dictionaryDisplayList.Add("Total Power Off Hours", ItemIDX++);//103  
            dictionaryDisplayList.Add("Magnet Counts", ItemIDX++);//104

            dictionaryDisplayList.Add("Cumulative MD Active", ItemIDX++);//105
            dictionaryDisplayList.Add("Cumulative MD Apparent", ItemIDX++);//106  
            dictionaryDisplayList.Add("Latest Tamper Occ-Res Details", ItemIDX++);//107

            dictionaryDisplayList.Add("Dynamic Rate Billing kWh ", ItemIDX++);//108
            //dictionaryDisplayList.Add("Billing kWh TOD2", ItemIDX++);//109
            ItemIDX++;
            //dictionaryDisplayList.Add("Billing kWh TOD3", ItemIDX++);//110
            ItemIDX++;
            //dictionaryDisplayList.Add("Billing kWh TOD4", ItemIDX++);//111
            ItemIDX++;
            //dictionaryDisplayList.Add("Billing kWh TOD5", ItemIDX++);//112
            ItemIDX++;
            //dictionaryDisplayList.Add("Billing kWh TOD6", ItemIDX++);//113
            ItemIDX++;


            dictionaryDisplayList.Add("Dynamic Billing MD KVA", ItemIDX++);//114
            //dictionaryDisplayList.Add("Billing MD KVA TOD2", ItemIDX++);//115
            ItemIDX++;
            //dictionaryDisplayList.Add("Billing MD KVA TOD3", ItemIDX++);//116
            ItemIDX++;
            //dictionaryDisplayList.Add("Billing MD KVA TOD4", ItemIDX++);//117
            ItemIDX++;
            //dictionaryDisplayList.Add("Billing MD KVA TOD5", ItemIDX++);//118
            ItemIDX++;
            //dictionaryDisplayList.Add("Billing MD KVA TOD6", ItemIDX++);//119
            ItemIDX++;

            //dictionaryDisplayList.Add("Present Month Power on-Hours", ItemIDX++);//120
            ItemIDX++;
            dictionaryDisplayList.Add("Power off-Hours Since Last Reset", ItemIDX++);//121
            dictionaryDisplayList.Add("Metering Current", ItemIDX++);//122
            //dictionaryDisplayList.Add("Billing Power on-Hours", ItemIDX++);//123
            ItemIDX++;
            dictionaryDisplayList.Add("First Tamper Occurrence", ItemIDX++);//124

            dictionaryDisplayList.Add("L2L Billing kWh", ItemIDX++);//125
            dictionaryDisplayList.Add("L2L Billing kVAh", ItemIDX++);//126

            dictionaryDisplayList.Add("L2L Billing APF", ItemIDX++);//127
            dictionaryDisplayList.Add("L2L Billing kW", ItemIDX++);//128

            //dictionaryDisplayList.Add("Cum Active Export Energy", ItemIDX++);//129
            ItemIDX++;
            //dictionaryDisplayList.Add("High Resolution Active Export Lead", ItemIDX++);//130
            ItemIDX++;
            dictionaryDisplayList.Add("Temperature", ItemIDX++);//131
            dictionaryDisplayList.Add("ABC String AlphaNumeric", ItemIDX++);//132
            dictionaryDisplayList.Add("Neutral Disturbance Count with Latest Occurence", ItemIDX++);//133
            dictionaryDisplayList.Add("Earth Count with Latest Occurence", ItemIDX++);//134
            dictionaryDisplayList.Add("Reverse Count with Latest Occurence", ItemIDX++);//135
            dictionaryDisplayList.Add("SWT Count with Latest Occurence", ItemIDX++);//136
            dictionaryDisplayList.Add("Readout Count", ItemIDX++);//137
            dictionaryDisplayList.Add("Anomaly Indicator", ItemIDX++);//138
            dictionaryDisplayList.Add("Running Tamper Status", ItemIDX++);//139
            ItemIDX++;//140
            dictionaryDisplayList.Add("Dynamic Billing MDKW,", ItemIDX++);//141
            ItemIDX++;//142
            ItemIDX++;//143
            ItemIDX++;//144
            ItemIDX++;//145
            ItemIDX++;//146
            dictionaryDisplayList.Add("Cumulative Fraud Energy", ItemIDX++);//147

            dictionaryDisplayList.Add("Cumulative Export Energy", ItemIDX++); //148
            dictionaryDisplayList.Add("Bill KWH Export", ItemIDX++); //149
            dictionaryDisplayList.Add("MD Present Active Export", ItemIDX++); //150
            dictionaryDisplayList.Add("Bill KW Export", ItemIDX++); //151
            dictionaryDisplayList.Add("Dial Test Active Export", ItemIDX++); //152
            dictionaryDisplayList.Add("Cum Apparent Export Energy", ItemIDX++); //153
            dictionaryDisplayList.Add("Dial Test Apparent Export", ItemIDX++); //154
            dictionaryDisplayList.Add("Pmax Present Apparent Export", ItemIDX++); //155
            dictionaryDisplayList.Add("Bill Kvah Export", ItemIDX++); //156
            dictionaryDisplayList.Add("Bill Kva Export", ItemIDX++); //157
            dictionaryDisplayList.Add("Cum Reactive Energy Lag Export", ItemIDX++); //158
            dictionaryDisplayList.Add("Cum Reactive Energy Lead Export", ItemIDX++); //159
            dictionaryDisplayList.Add("High Resolution Reactive Lag Export", ItemIDX++); //160
            dictionaryDisplayList.Add("High Resolution Reactive Lead Export", ItemIDX++); //161
            dictionaryDisplayList.Add("Billing kVArh Lag Export", ItemIDX++); //162
            dictionaryDisplayList.Add("Billing kVArh Lead Export", ItemIDX++); //163
            dictionaryDisplayList.Add("MD KW - Max of history 1 and history 2", ItemIDX++); //164
            dictionaryDisplayList.Add("Dynamic TOD Active Export Energy", ItemIDX++); //165


            return dictionaryDisplayList;
        }

        public Dictionary<int, string> GetTamperCodeParameterList()
        {
            Dictionary<int, string> dictionaryEventCodeList = new Dictionary<int, string>();
          
            dictionaryEventCodeList.Add(1,"R Phase PT Link Missing (Missing Potential) Occurrence") ;
            dictionaryEventCodeList.Add(2, "R Phase PT Link Missing (Missing Potential) Restoration");
            dictionaryEventCodeList.Add(3, "Y Phase PT Link Missing (Missing Potential) Occurrence ");
            dictionaryEventCodeList.Add(4, "Y Phase PT Link Missing (Missing Potential) Restoration ");
            dictionaryEventCodeList.Add(5, "B Phase PT Link Missing (Missing Potential) Occurrence ");
            dictionaryEventCodeList.Add(6, "B Phase PT Link Missing (Missing Potential) Restoration");
            dictionaryEventCodeList.Add(7, "Over Voltage Occurrence");
            dictionaryEventCodeList.Add(8, "Over Voltage Restoration");
            dictionaryEventCodeList.Add(9, "Low Voltage Occurrence");
            dictionaryEventCodeList.Add(10, "Low Voltage Restoration");
            dictionaryEventCodeList.Add(11, "Voltage Unbalance Occurrence");
            dictionaryEventCodeList.Add(12, "Voltage Unbalance Restoration");
            dictionaryEventCodeList.Add(49, "Invalid Voltage Occurrence");
            dictionaryEventCodeList.Add(50, "Invalid Voltagee Restoration");
            dictionaryEventCodeList.Add(51, "Phase R CT Reverse Occurrence");
            dictionaryEventCodeList.Add(52, "Phase R CT Reverse Restoration");
            dictionaryEventCodeList.Add(53, "Phase Y CT Reverse Occurrence");
            dictionaryEventCodeList.Add(54, "Phase Y CT Reverse Restoration");
            dictionaryEventCodeList.Add(55, "Phase B CT Reverse Occurrence");
            dictionaryEventCodeList.Add(56, "Phase B CT Reverse Restoration");
            dictionaryEventCodeList.Add(57, "Phase R CT Open Occurrence");
            dictionaryEventCodeList.Add(58, "Phase R CT Open Restoration");
            dictionaryEventCodeList.Add(59, "Phase Y CT Open Occurrence");
            dictionaryEventCodeList.Add(60, "Phase Y CT Open Restoration");
            dictionaryEventCodeList.Add(61, "Phase B CT Open Occurrence");
            dictionaryEventCodeList.Add(62, "Phase B CT Open Restoration");
            dictionaryEventCodeList.Add(63, "Current Unbalance Occurrence");
            dictionaryEventCodeList.Add(64, "Current Unbalance Restoration");
            dictionaryEventCodeList.Add(65, "CT Bypass Occurrence");
            dictionaryEventCodeList.Add(66, "CT Bypass Restoration");
            dictionaryEventCodeList.Add(67, "Over Current Occurrence");
            dictionaryEventCodeList.Add(68, "Over Current Restoration");
            dictionaryEventCodeList.Add(69, "Earth Tamper Occurrence");
            dictionaryEventCodeList.Add(70, "Earth Tamper Restoration");
            dictionaryEventCodeList.Add(91, "Over Load Occurance");
            dictionaryEventCodeList.Add(92, "Over Load Restoration");
            dictionaryEventCodeList.Add(101,"Power Failure  Occurrence") ;
            dictionaryEventCodeList.Add(102, "Power Failure Restoration");
            dictionaryEventCodeList.Add(151, "RTC Change");
            dictionaryEventCodeList.Add(152, "DIP Change");
            dictionaryEventCodeList.Add(153, "SIP Change)");
            dictionaryEventCodeList.Add(154, "BILL Date Change");
            dictionaryEventCodeList.Add(155, "TOU Change");
            dictionaryEventCodeList.Add(156, "RS 485 device address");
            dictionaryEventCodeList.Add(157, "New Firmware Activated");
            dictionaryEventCodeList.Add(158, "Load Limit (Kw) set"); // code 188 is also added for kVAh selection for compatebeity
            dictionaryEventCodeList.Add(159, "Billing Reset/Load Limit Function-Enabled");
            dictionaryEventCodeList.Add(160, "Load Limit Function-Disabled");
            dictionaryEventCodeList.Add(161, "LLS Secret (MR)Change");
            dictionaryEventCodeList.Add(162, "HLS key (US)Change");
            dictionaryEventCodeList.Add(163, "HLS key (FW)Change");
            dictionaryEventCodeList.Add(164, "Global key (encryption and authentication) change");
            dictionaryEventCodeList.Add(165, "ESWF change");
            dictionaryEventCodeList.Add(166, "MD Reset");
            dictionaryEventCodeList.Add(167, "Metering Mode");
           
            dictionaryEventCodeList.Add(169, "Image Activation Single action schedule");
            dictionaryEventCodeList.Add(177, "Configuration changed Forward only mode");
            dictionaryEventCodeList.Add(178, "Configuration changed to import and export mode");
             
            dictionaryEventCodeList.Add(192, "Display Parameters");
            dictionaryEventCodeList.Add(193, "LS capture parameter");
            dictionaryEventCodeList.Add(194, "Bill Cycle Changed ");
            dictionaryEventCodeList.Add(195, "Price Table");
            dictionaryEventCodeList.Add(196, "Load Control Parameter");
            dictionaryEventCodeList.Add(197, "Firmware Upgrade");
            dictionaryEventCodeList.Add(198, "Tamper Configuration Changed");
            dictionaryEventCodeList.Add(199, "SR Configuration Change");
            dictionaryEventCodeList.Add(201, "Influence of Permanent Magnet or AC DC Electromagnet Occurrence");
            dictionaryEventCodeList.Add(202, "Influence of Permanent Magnet or AC DC Electromagnet Restoration");
            dictionaryEventCodeList.Add(203, "Neutral Disturbance  HF And DC Occurrence");
            dictionaryEventCodeList.Add(204, "Neutral Disturbance  HF And DC Restoration");
            dictionaryEventCodeList.Add(205, "Very Low PF Occurrence");
            dictionaryEventCodeList.Add(206, "Very Low PF Restoration");
            dictionaryEventCodeList.Add(207, "SWT Occurrence");
            dictionaryEventCodeList.Add(208, "SWT Restoration");
            dictionaryEventCodeList.Add(209, "Plug in module removal Occurrence");
            dictionaryEventCodeList.Add(210, "Plug in module removal Restoration");
            dictionaryEventCodeList.Add(211, "Configuration changed to post paid mode");
            dictionaryEventCodeList.Add(212, "Configuration changed to pre paid mode");
            dictionaryEventCodeList.Add(213, "Configuration changed to forward only mode");
            dictionaryEventCodeList.Add(214, "Configuration changed to import and export mode");
            dictionaryEventCodeList.Add(215, "Over Load Occurrence");
            dictionaryEventCodeList.Add(216, "Over Load Restoration");
            dictionaryEventCodeList.Add(247, "2Pn Tamper Occurrence");
            dictionaryEventCodeList.Add(248, "2Pn Tamper Restoration");
            dictionaryEventCodeList.Add(249, "ESD Occurrence");      
            dictionaryEventCodeList.Add(251, "Meter Cover Opening Occurrence");
            dictionaryEventCodeList.Add(295, "RTC Bad Store Occurrence");
            dictionaryEventCodeList.Add(296, "Relay Malfunctioning Occurrence");
            dictionaryEventCodeList.Add(297, "COMS Card Removal Occurence");

            dictionaryEventCodeList.Add(301, "Relay Dis-Connected");
            dictionaryEventCodeList.Add(302, "Relay Connected");    
        
            dictionaryEventCodeList.Add(188, "Kvah selection logging"); // code 158 is also added for kVAh selection for compatebeity
            dictionaryEventCodeList.Add(189, "Billing transaction logging");
            dictionaryEventCodeList.Add(190, "CT ratio programming");

            dictionaryEventCodeList.Add(243, "Low Supply  Voltage Occurrence");
            dictionaryEventCodeList.Add(244, "Low Supply  Voltage Restoration");

            dictionaryEventCodeList.Add(245, "Phase in Neutral Occurrence");
            dictionaryEventCodeList.Add(246, "Phase in Neutral Restoration");
            //dictionaryEventCodeList.Add(251, "Cover Open");
            //dictionaryEventCodeList.Add(301, "Relay Disconnected");
            //dictionaryEventCodeList.Add(302, "Relay Connected");

           

            dictionaryEventCodeList.Add(701, "High Neutral Current Occurrence");
            dictionaryEventCodeList.Add(702, "High Neutral Current Restoration");
            dictionaryEventCodeList.Add(703, "Current Mis-match Occurrence");
            dictionaryEventCodeList.Add(704, "Current Mis-match Restoration");

            dictionaryEventCodeList.Add(751, "Last token recharge amount Prepaid mode");
            dictionaryEventCodeList.Add(752, "Last token recharge time Prepaid mode");
            dictionaryEventCodeList.Add(753, "Total time last recharge Prepaid mode");
            dictionaryEventCodeList.Add(754, "Current balance amount Prepaid mode");
            dictionaryEventCodeList.Add(755, "Current balance time Prepaid mode");
            dictionaryEventCodeList.Add(756, "Digital Output operation");
            dictionaryEventCodeList.Add(757, "Demand method configuration");
            dictionaryEventCodeList.Add(758, "Event threshold config change");
            dictionaryEventCodeList.Add(759, "Event threshold persistence time change");
            dictionaryEventCodeList.Add(760, "Display parameter change");
            dictionaryEventCodeList.Add(761, "LS parameter StoreID");
            dictionaryEventCodeList.Add(762, "Optical port lock");
            dictionaryEventCodeList.Add(763, "Optical port Unlock");
            dictionaryEventCodeList.Add(764, "RJ port lock");
            dictionaryEventCodeList.Add(765, "RJ port Unlock");
            dictionaryEventCodeList.Add(766, "Special Day");
            dictionaryEventCodeList.Add(767, "Event Enable/Disable Configuration");
            dictionaryEventCodeList.Add(768, "Load control parameter");
            dictionaryEventCodeList.Add(769, "ARM button Enable");
            dictionaryEventCodeList.Add(770, "ARM button Disable");
            dictionaryEventCodeList.Add(771, "FS mode lock");
            dictionaryEventCodeList.Add(772, "FS mode Unlock");
            dictionaryEventCodeList.Add(801, "ESD Tamper Occurrence");
            dictionaryEventCodeList.Add(802, "ESD Tamper Restoration");

            dictionaryEventCodeList.Add(803, "Abnormal Power-Off Occurrence");
            dictionaryEventCodeList.Add(804, "Abnormal Power-Off Restoration");

            dictionaryEventCodeList.Add(805, "Invalid Phase Association Occurrence");
            dictionaryEventCodeList.Add(806, "Invalid Phase Association Restoration");

            dictionaryEventCodeList.Add(951, "Temperature Rise Occurence");
            dictionaryEventCodeList.Add(952, "Temperature Rise Restoration");

            dictionaryEventCodeList.Add(959,"%THDV R- Phase Tamper Occurance");
            dictionaryEventCodeList.Add(960,"%THDV R- Phase Tamper Restoration");	
            dictionaryEventCodeList.Add(961,"%THDV Y- Phase Tamper Occurance");	
            dictionaryEventCodeList.Add(962,"%THDV Y- Phase Tamper Restoration");
            dictionaryEventCodeList.Add(963,"%THDV B- Phase Tamper Occurance");
            dictionaryEventCodeList.Add(964,"%THDV B- Phase Tamper Restoration");
            dictionaryEventCodeList.Add(965,"%THDI R- Phase Tamper Occurance");
            dictionaryEventCodeList.Add(966,"%THDI R- Phase Tamper Restoration");
            dictionaryEventCodeList.Add(967,"%THDI Y- Phase Tamper Occurance");
            dictionaryEventCodeList.Add(968,"%THDI Y- Phase Tamper Restoration");
            dictionaryEventCodeList.Add(969,"%THDI B- Phase Tamper Occurance");
            dictionaryEventCodeList.Add(970,"%THDI B- Phase Tamper Restoration");	

            dictionaryEventCodeList.Add(1001, "Digital input 2 Set");
            dictionaryEventCodeList.Add(1002, "Digital input 2 Reset");
            dictionaryEventCodeList.Add(1003, "Digital input 3 Set");
            dictionaryEventCodeList.Add(1004, "Digital input 3 Reset");
            dictionaryEventCodeList.Add(1005, "Digital input 4 Set");
            dictionaryEventCodeList.Add(1006, "Digital input 4 Reset");
            dictionaryEventCodeList.Add(1007, "Digital input 5 Set");
            dictionaryEventCodeList.Add(1008, "Digital input 5 Reset");
            dictionaryEventCodeList.Add(1009, "Digital input 6 Set");
            dictionaryEventCodeList.Add(1010, "Digital input 6 Reset");



            
           


            return dictionaryEventCodeList;
        }

        public Dictionary<string, string> GetXMLDisplayParaList()
        {
            Dictionary<string, string> XMLDisplayParaList = new Dictionary<string, string>();
            XMLDisplayParaList.Add(StaticConstantsCommon.DemandIP, "Demand IP");            
            XMLDisplayParaList.Add(StaticConstantsCommon.BillingDateTime, "Billing Date Time");            
            XMLDisplayParaList.Add(StaticConstantsCommon.LoadLimit, "Load Limit");
            XMLDisplayParaList.Add(StaticConstantsCommon.MeteringMode, "Metering Mode");
            XMLDisplayParaList.Add(StaticConstantsCommon.PaymentMode, "Payment Mode");
            XMLDisplayParaList.Add(StaticConstantsCommon.EventStatusWordFilter, "ESWF Config");
            XMLDisplayParaList.Add(StaticConstantsCommon.DisplayAutoScroll, "Auto Scroll");
            XMLDisplayParaList.Add(StaticConstantsCommon.DisplayPushButton, "Push Button");
            XMLDisplayParaList.Add(StaticConstantsCommon.DisplayHRmode, "HR Display");
            XMLDisplayParaList.Add(StaticConstantsCommon.OpticalPortLocking, "Optical Port Locking");
            XMLDisplayParaList.Add(StaticConstantsCommon.RJPortLocking, "RJ Port Locking");
            XMLDisplayParaList.Add(StaticConstantsCommon.EventLog, "Event Log");
            XMLDisplayParaList.Add(StaticConstantsCommon.LoadControl, "Load Control");
            XMLDisplayParaList.Add(StaticConstantsCommon.ARMButton, "ARM Button");
            XMLDisplayParaList.Add(StaticConstantsCommon.TouParaName, "Time of Use");
            XMLDisplayParaList.Add(StaticConstantsCommon.TamperThresholdParaName, "Tamper Thresholds");
            XMLDisplayParaList.Add(StaticConstantsCommon.CaptureObjects, "LS Capture object");
            XMLDisplayParaList.Add(StaticConstantsCommon.SurveyIP, "Survey IP");
            XMLDisplayParaList.Add(StaticConstantsCommon.DemandMethod, "Demand Method");
            XMLDisplayParaList.Add(StaticConstantsCommon.RS485DeviceAddress, "RS485 Device Address");
            return XMLDisplayParaList;
        }

        public Dictionary<string, int> ReadoutParameters_1PHIEC()
        {
            Dictionary<string, int> dictionaryReadoutList = new Dictionary<string, int>();

            dictionaryReadoutList.Add("Meter ID", 2);
            dictionaryReadoutList.Add("Software Version", 29);
            dictionaryReadoutList.Add("Meter Manufacturing Date-Time", 70);
            dictionaryReadoutList.Add("Time and date", 16);
            dictionaryReadoutList.Add("Instantaneous Voltage", 13);
            dictionaryReadoutList.Add("Instantaneous Phase Current", 46);
            dictionaryReadoutList.Add("Instantaneous Neutral Current", 47);
            dictionaryReadoutList.Add("Instantaneous PF", 48);
            dictionaryReadoutList.Add("Instantaneous PF with sign", 61);
            dictionaryReadoutList.Add("Present Month Average PF", 62);
            dictionaryReadoutList.Add("Instantaneous Phase Power", 14);
            dictionaryReadoutList.Add("Instantaneous Neutral Power", 15);
            dictionaryReadoutList.Add("Instantaneous Apparent Power", 64);
            dictionaryReadoutList.Add("Instantaneous Reactive Power", 65);
            dictionaryReadoutList.Add("Power fail count", 20);
            dictionaryReadoutList.Add("Total Active energy", 3);
            dictionaryReadoutList.Add("Active Energy rate 1", 4);
            dictionaryReadoutList.Add("Active Energy rate 2", 5);
            dictionaryReadoutList.Add("Active Energy rate 3", 6);
            dictionaryReadoutList.Add("Active Energy rate 4", 7);
            dictionaryReadoutList.Add("Active Energy rate 5", 8);
            dictionaryReadoutList.Add("Active Energy rate 6", 9);
            dictionaryReadoutList.Add("Active Energy Rate 7", 108);
            dictionaryReadoutList.Add("Active Energy Rate 8", 109);
            dictionaryReadoutList.Add("Apparent Energy rate 1", 74);
            dictionaryReadoutList.Add("Apparent Energy rate 2", 75);
            dictionaryReadoutList.Add("Apparent Energy rate 3", 76);
            dictionaryReadoutList.Add("Apparent Energy rate 4", 77);
            dictionaryReadoutList.Add("Apparent Energy rate 5", 78);
            dictionaryReadoutList.Add("Apparent Energy rate 6", 79);
            dictionaryReadoutList.Add("Apparent Energy rate 7", 114);
            dictionaryReadoutList.Add("Apparent Energy rate 8", 115);
            dictionaryReadoutList.Add("Total Apparent Energy", 66);
            dictionaryReadoutList.Add("Total Reactive Energy", 67);
            dictionaryReadoutList.Add("Maximum demand Active present Slot", 10);
            dictionaryReadoutList.Add("Maximum demand apparent present Slot", 68);
            dictionaryReadoutList.Add("Legal Energy", 30);
            dictionaryReadoutList.Add("Fraud Energy", 31);
            dictionaryReadoutList.Add("Total hours on power", 25);
            dictionaryReadoutList.Add("Total minutes on power", 26);
            dictionaryReadoutList.Add("Number of reverse current Events", 21);
            dictionaryReadoutList.Add("Reverse Load tamper Events", 37);
            dictionaryReadoutList.Add("Number of Earthed load Events", 22);
            dictionaryReadoutList.Add("Earth Tamper Events", 35);
            dictionaryReadoutList.Add("Number of Magnet fraud Events", 23);
            dictionaryReadoutList.Add("Magnet tamper Events", 27);
            dictionaryReadoutList.Add("Number of Case tamper Events", 34);
            dictionaryReadoutList.Add("Case Tamper first occurence", 28);
            dictionaryReadoutList.Add("Number of Neutral Disturbance Events", 33);
            dictionaryReadoutList.Add("Neutral Disturbance tamper Events", 59);
            dictionaryReadoutList.Add("Number of Single Wire Tamper Events", 32);
            dictionaryReadoutList.Add("Single Wire tamper Events", 36);
            dictionaryReadoutList.Add("Total no. of tamper counts", 58);
            dictionaryReadoutList.Add("Transaction Events Log", 72);
            dictionaryReadoutList.Add("Total Transaction Events counts", 73);
            dictionaryReadoutList.Add("Tamper reset count", 24);
            dictionaryReadoutList.Add("Active Energy stored values", 11);
            dictionaryReadoutList.Add("Active Billing Rate 1 Energy", 39);
            dictionaryReadoutList.Add("Active Billing Rate 2 Energy", 40);
            dictionaryReadoutList.Add("Active Billing Rate 3 Energy", 41);
            dictionaryReadoutList.Add("Active Billing Rate 4 Energy", 42);
            dictionaryReadoutList.Add("Active Billing Rate 5 Energy", 43);
            dictionaryReadoutList.Add("Active Billing Rate 6 Energy", 44);
            dictionaryReadoutList.Add("Active Billing Rate 7 Energy", 110);
            dictionaryReadoutList.Add("Active Billing Rate 8 Energy", 111);
            dictionaryReadoutList.Add("Apparent Billing Rate 1 Energy", 80);
            dictionaryReadoutList.Add("Apparent Billing Rate 2 Energy", 81);
            dictionaryReadoutList.Add("Apparent Billing Rate 3 Energy", 82);
            dictionaryReadoutList.Add("Apparent Billing Rate 4 Energy", 83);
            dictionaryReadoutList.Add("Apparent Billing Rate 5 Energy", 84);
            dictionaryReadoutList.Add("Apparent Billing Rate 6 Energy", 85);
            dictionaryReadoutList.Add("Apparent Billing Rate 7 Energy", 116);
            dictionaryReadoutList.Add("Apparent Billing Rate 8 Energy", 117);
            dictionaryReadoutList.Add("Billing Average PF", 45);
            dictionaryReadoutList.Add("Active Demand stored values", 12);
            dictionaryReadoutList.Add("Apparent (Billing) Energy Stored Values", 69);
            dictionaryReadoutList.Add("Apparent Demand stored values", 71);
            dictionaryReadoutList.Add("Billing Power On Hours", 38);
            dictionaryReadoutList.Add("Billing Power On Minutes", 123);//---Moved here from position 123
            dictionaryReadoutList.Add("Max Demand reset count", 17);
            dictionaryReadoutList.Add("MD rate 1", 49);
            dictionaryReadoutList.Add("MD rate 2", 50);
            dictionaryReadoutList.Add("MD rate 3", 51);
            dictionaryReadoutList.Add("MD rate 4", 52);
            dictionaryReadoutList.Add("MD rate 5", 53);
            dictionaryReadoutList.Add("MD rate 6", 54);
            dictionaryReadoutList.Add("MD Rate 7", 112);
            dictionaryReadoutList.Add("MD Rate 8", 113);
            dictionaryReadoutList.Add("Authenticated Billing Code-1", 60);
            dictionaryReadoutList.Add("Authenticated Billing Code-2", 101);
            dictionaryReadoutList.Add("ABC AlphaNumeric", 107);//------Single code will support all its readout History and same will be manage by FW
            dictionaryReadoutList.Add("Programmed bill day/time", 63);
            dictionaryReadoutList.Add("Successful data download counter", 57);
            dictionaryReadoutList.Add("Status information", 19);
            dictionaryReadoutList.Add("Battery voltage", 18);
            dictionaryReadoutList.Add("RTC Battery Voltage", 55);
            dictionaryReadoutList.Add("Vreg voltage", 56);
            dictionaryReadoutList.Add("Cumulative Active MD", 86);
            dictionaryReadoutList.Add("Total Power Off Hours", 87);
            dictionaryReadoutList.Add("Billing Power Off Hours", 88);
            dictionaryReadoutList.Add("Power Off Hours Since Last Reset", 89);
            dictionaryReadoutList.Add("Power OFF-ON Events", 90);
            dictionaryReadoutList.Add("Total single wire tamper duration", 91);
            dictionaryReadoutList.Add("Total magnet tamper duration", 92);
            dictionaryReadoutList.Add("Total ND tamper duration", 93);
            dictionaryReadoutList.Add("Total reverse tamper duration", 94);
            dictionaryReadoutList.Add("Total earth tamper duration", 95);
            //-------------------96 is missing from Base code----------------
            dictionaryReadoutList.Add("Over Load Tamper Count", 97);
            dictionaryReadoutList.Add("Over Load Tamper History", 98);
            dictionaryReadoutList.Add("Low Voltage Tamper Count", 99);
            dictionaryReadoutList.Add("Low Voltage Tamper History", 100);
            // dictionaryReadoutList.Add("Authenticated Billing Code-2",101); Moved above with Billing Code-1
            dictionaryReadoutList.Add("ESD Tamper Events", 102);
            dictionaryReadoutList.Add("ESD Tamper Count", 103);
            dictionaryReadoutList.Add("Line Frequency", 104);
            dictionaryReadoutList.Add("ESD Tamper Duration", 105);
            dictionaryReadoutList.Add("Annual MD History", 106);
            dictionaryReadoutList.Add("Abnormal Power Off Event Counts", 125); //---------Abnormal Counts
            dictionaryReadoutList.Add("Abnormal Power Off Event Log", 126); //---------Abnormal Log
            // dictionaryReadoutList.Add("ABC AlphaNumeric", 107);// Moved above with Billing Code-1

            //--------------------------Parameters With ID 108 to 117 are moved up with respected parameters---------------
            //dictionaryReadoutList.Add("Active Energy Rate 7", 108);
            //dictionaryReadoutList.Add("Active Energy Rate 8", 109);
            //dictionaryReadoutList.Add("Active Billing Rate 1 Energy", 110);
            //dictionaryReadoutList.Add("Active Billing Rate 2 Energy", 111);
            //dictionaryReadoutList.Add("MD Rate 7", 112);
            //dictionaryReadoutList.Add("MD Rate 8", 113);
            //dictionaryReadoutList.Add("Apparent Energy rate 7", 114);
            //dictionaryReadoutList.Add("Apparent Energy rate 8", 115);
            //dictionaryReadoutList.Add("Apparent Billing Rate 7 Energy", 116);
            //dictionaryReadoutList.Add("Apparent Billing Rate 8 Energy", 117);
            //---------------------------------------------------------------------------------------------------------------
            dictionaryReadoutList.Add("Low PF Tamper Count", 118);
            dictionaryReadoutList.Add("Low PF Tamper Events", 119);
            dictionaryReadoutList.Add("High Temperature Count", 120);
            dictionaryReadoutList.Add("High Temperature Events", 121);
            dictionaryReadoutList.Add("Drill Tamper Events", 122);
            //dictionaryReadoutList.Add("Billing Power On Minutes", 123);;//Moved above with Power ON Hours
            dictionaryReadoutList.Add("Error code", 1);
            dictionaryReadoutList.Add("Error code-1", 124); //---------Meter Battery Status Code 
            //dictionaryReadoutList.Add("Abnormal Power-OFF Counts", 125); //---------Abnormal Counts //Moved above with Tamper Logs
            // dictionaryReadoutList.Add("Abnormal Power-OFF Log", 126); //---------Abnormal Log //Moved above with Tamper Logs
            return dictionaryReadoutList;
        }

        public Dictionary<string, int> PushButtonParameters_1PHIEC()
        {
            Dictionary<string, int> dictionaryPushParaList = new Dictionary<string, int>();
           
            dictionaryPushParaList.Add("LCD Segments On", 24);
            dictionaryPushParaList.Add("LCD Segments Off", 23);     
            dictionaryPushParaList.Add("Meter ID/Meter ID MSB",43 );
            dictionaryPushParaList.Add("Meter ID LSB", 44);       
            dictionaryPushParaList.Add("Time", 21);
            dictionaryPushParaList.Add("Date", 22);
            dictionaryPushParaList.Add("Instant Voltage",15 );
            dictionaryPushParaList.Add("Phase current", 28);
            dictionaryPushParaList.Add("Neutral current", 29);
            dictionaryPushParaList.Add("Line Frequency", 34);
            dictionaryPushParaList.Add("Instantaneous PF", 33);
            dictionaryPushParaList.Add("Instantaneous PF with sign", 58);
            dictionaryPushParaList.Add("Instant Metering Active Power",16 );
            dictionaryPushParaList.Add("Instant Neutral Active Power", 17);
            dictionaryPushParaList.Add("Inst. Apparent power", 64);
            dictionaryPushParaList.Add("Inst. Reactive power", 65);
            dictionaryPushParaList.Add("Active Rate", 14);
            dictionaryPushParaList.Add("Present Month Average PF", 59);
            dictionaryPushParaList.Add("Dial Test For Active Energy", 18);
            dictionaryPushParaList.Add("Dial Test For Apparent Energy", 66);
            dictionaryPushParaList.Add("Dial Test For Reactive Energy", 67);
            dictionaryPushParaList.Add("Cumulative Apparent Energy", 62);
            dictionaryPushParaList.Add("Cumulative Reactive Energy", 63);
            dictionaryPushParaList.Add("Cumulative Active Energy", 1);
            dictionaryPushParaList.Add("Active Energy Rate 1", 2);
            dictionaryPushParaList.Add("Active Energy Rate 2", 3);
            dictionaryPushParaList.Add("Active Energy Rate 3", 4);
            dictionaryPushParaList.Add("Active Energy Rate 4", 5);
            dictionaryPushParaList.Add("Active Energy Rate 5", 6);
            dictionaryPushParaList.Add("Active Energy Rate 6", 7);
            dictionaryPushParaList.Add("Active Energy Rate 7", 108);
            dictionaryPushParaList.Add("Active Energy Rate 8", 109);
            dictionaryPushParaList.Add("Apparent Energy rate 1", 73);
            dictionaryPushParaList.Add("Apparent Energy rate 2", 74);
            dictionaryPushParaList.Add("Apparent Energy rate 3", 75);
            dictionaryPushParaList.Add("Apparent Energy rate 4", 76);
            dictionaryPushParaList.Add("Apparent Energy rate 5", 77);
            dictionaryPushParaList.Add("Apparent Energy rate 6", 78);
            dictionaryPushParaList.Add("Apparent Energy rate 7", 112);
            dictionaryPushParaList.Add("Apparent Energy rate 8", 113);
            dictionaryPushParaList.Add("Active Billing Rate 1 Energy", 95);
            dictionaryPushParaList.Add("Active Billing Rate 2 Energy", 96);
            dictionaryPushParaList.Add("Active Billing Rate 3 Energy", 97);
            dictionaryPushParaList.Add("Active Billing Rate 4 Energy", 98);
            dictionaryPushParaList.Add("Active Billing Rate 5 Energy", 99);
            dictionaryPushParaList.Add("Active Billing Rate 6 Energy", 100);
            dictionaryPushParaList.Add("Active Billing Rate 7 Energy", 116);
            dictionaryPushParaList.Add("Active Billing Rate 8 Energy", 117);
            dictionaryPushParaList.Add("Apparent Billing Rate 1 Energy", 79);
            dictionaryPushParaList.Add("Apparent Billing Rate 2 Energy", 80);
            dictionaryPushParaList.Add("Apparent Billing Rate 3 Energy", 81);
            dictionaryPushParaList.Add("Apparent Billing Rate 4 Energy", 82);
            dictionaryPushParaList.Add("Apparent Billing Rate 5 Energy", 83);
            dictionaryPushParaList.Add("Apparent Billing Rate 6 Energy", 84);
            dictionaryPushParaList.Add("Apparent Billing Rate 7 Energy", 114);
            dictionaryPushParaList.Add("Apparent Billing Rate 8 Energy", 115);
            dictionaryPushParaList.Add("Max Demand Active (present)", 30);
            dictionaryPushParaList.Add("Max Demand Apparent (present)", 68);
            dictionaryPushParaList.Add("Billing Active Energy", 40);
            dictionaryPushParaList.Add("Billing apparent MD", 69);
            dictionaryPushParaList.Add("Billing apparent Energy", 70);
            dictionaryPushParaList.Add("Billing MD", 39);
            dictionaryPushParaList.Add("Billing Power on Hours", 41);
            dictionaryPushParaList.Add("Billing Average PF", 42);
            dictionaryPushParaList.Add("Rate 1 MD", 45);
            dictionaryPushParaList.Add("Rate 2 MD", 46);
            dictionaryPushParaList.Add("Rate 3 MD", 47);
            dictionaryPushParaList.Add("Rate 4 MD", 48);
            dictionaryPushParaList.Add("Rate 5 MD", 49);
            dictionaryPushParaList.Add("Rate 6 MD", 50);
            dictionaryPushParaList.Add("Rate 7 MD", 110);
            dictionaryPushParaList.Add("Rate 8 MD", 111);
            dictionaryPushParaList.Add("Demand reset counter", 19);
            dictionaryPushParaList.Add("Fraud Reset Counter", 20);
            dictionaryPushParaList.Add("Earth Tamper Count", 51);
            dictionaryPushParaList.Add("Reverse Tamper Count", 52);
            dictionaryPushParaList.Add("Magnet Tamper Count", 53);
            dictionaryPushParaList.Add("Single Wire Tamper Count", 54);
            dictionaryPushParaList.Add("Neutral Disturbance tamper count", 57);
            dictionaryPushParaList.Add("Case Tamper First occurence", 72);
            dictionaryPushParaList.Add("Total tamper counts", 55);
            dictionaryPushParaList.Add("Total transaction Event Counter", 71);
            dictionaryPushParaList.Add("Authenticated Billing Code-1", 56);
            dictionaryPushParaList.Add("Authenticated Billing Code-2", 102);
            dictionaryPushParaList.Add("Last bill date", 60);
            dictionaryPushParaList.Add("Last bill time", 61);          
            dictionaryPushParaList.Add("Instantaneous Maximum Power", 36);
            dictionaryPushParaList.Add("RTC Battery Voltage", 37);
            dictionaryPushParaList.Add("TLV Voltage", 38);
            dictionaryPushParaList.Add("AC Magnetic Field", 35);      
            dictionaryPushParaList.Add("Cumulative Active MD", 85);
            dictionaryPushParaList.Add("Last To Last Billing Active Energy", 86);
            dictionaryPushParaList.Add("Last To Last Billing Apparent Energy", 87);
            dictionaryPushParaList.Add("Last To Last Billing Avg. PF", 88);
            dictionaryPushParaList.Add("Last To Last Billing MD (kW)", 89);
            dictionaryPushParaList.Add("Total Power Off Hours", 90);
            dictionaryPushParaList.Add("Billing Power Off Hours",91 );
            dictionaryPushParaList.Add("Power Off Hours Since Last Reset", 92);
            dictionaryPushParaList.Add("Total Hours On Power", 93);
            dictionaryPushParaList.Add("Latest Tamper Occ/Rest",94 );          
            
            dictionaryPushParaList.Add("Tamper Status",101);
            //----Code 102 is for ABC Bill-2 Encryption and is along with bill -1 in above code
            dictionaryPushParaList.Add("Fraud Energy", 103);
            dictionaryPushParaList.Add("ESD Tamper Count", 104);
            dictionaryPushParaList.Add("Present Annual MD", 105);
            dictionaryPushParaList.Add("Biiling Annual MD", 106);
            dictionaryPushParaList.Add("Present Month Power ON Hours", 107);
            dictionaryPushParaList.Add("Metering Current", 119);

            //----------------Parameters With ID 108 to 117 are moved up with respected parameters------------------------
            //dictionaryPushParaList.Add("Active Energy Rate 1", 108);
            //dictionaryPushParaList.Add("Active Energy Rate 2", 109);
            //dictionaryPushParaList.Add("Rate 7 MD", 110);
            //dictionaryPushParaList.Add("Rate 8 MD", 111);
            //dictionaryPushParaList.Add("Apparent Energy rate 7", 112);
            //dictionaryPushParaList.Add("Apparent Energy rate 8", 113);
            //dictionaryPushParaList.Add("Apparent Billing Rate 7 Energy", 114);
            //dictionaryPushParaList.Add("Apparent Billing Rate 8 Energy", 115);
            //dictionaryPushParaList.Add("Active Billing Rate 7 Energy", 116);
            //dictionaryPushParaList.Add("Active Billing Rate 8 Energy", 117);
            //---------------------------------------------------------------------------------------------------------------
            dictionaryPushParaList.Add("Temperature", 118);
            dictionaryPushParaList.Add("First Tamper Occ", 120);


            return dictionaryPushParaList;
        }

        public Dictionary<string, int> DisplayParameters_1PHFalcon2()
        {
            Dictionary<string, int> dictionaryPushParaList = new Dictionary<string, int>();

            dictionaryPushParaList.Add("BLANK_TEST",                                             1);
            dictionaryPushParaList.Add("ALL_SEG_TEST",                                           2);
            dictionaryPushParaList.Add("ODD_SEGMENTS",                                           3);
            dictionaryPushParaList.Add("EVEN_SEGMENTS",	                                         4);
            dictionaryPushParaList.Add("TIME",				                                     5);
            dictionaryPushParaList.Add("DATE",				                                     6);
            dictionaryPushParaList.Add("METER_ID",			                                     7);
            dictionaryPushParaList.Add("METER_ID_LSB",                                           8);
            dictionaryPushParaList.Add("VOLT",				                                     9);
            dictionaryPushParaList.Add("MAIN_BAT_VOLT",		                                    10);
            dictionaryPushParaList.Add("RTC_BAT_VOLT",		                                    11);
            dictionaryPushParaList.Add("ACTIVE_CURRENT",		                                12);
            dictionaryPushParaList.Add("PHASE_CURRENT",		                                    13);
            dictionaryPushParaList.Add("NEUTRAL_CURRENT",		                                14);
            dictionaryPushParaList.Add("INST_SIGNED_PF",		                                15);
            dictionaryPushParaList.Add("PRESENT_AVERAGE_PF",	                                16);
            dictionaryPushParaList.Add("FREQUENCY",			                                    17);
            dictionaryPushParaList.Add("CUM_POWERON_HOURS",	                                    18);
            dictionaryPushParaList.Add("POWER_ACTIVE",		                                    19);
            dictionaryPushParaList.Add("PHASE_POWER_ACTIVE",	                                20);
            dictionaryPushParaList.Add("NEUTRAL_POWER_ACTIVE",                                  21);
            dictionaryPushParaList.Add("APPARENT_POWER",		                                22);
            dictionaryPushParaList.Add("CUM_ACTIVE_ENERGY_IMPORT",                              23);
            dictionaryPushParaList.Add("CUM_ACTIVE_ENERGY_EXPORT",                              24);
            dictionaryPushParaList.Add("CUM_APPARENT_ENERGY_IMPORT",                            25);
            dictionaryPushParaList.Add("CUM_APPARENT_ENERGY_EXPORT",                            26);
            dictionaryPushParaList.Add("HIGH_RESOLUTION_ACTIVE_IMPORT",                         27);
            dictionaryPushParaList.Add("HIGH_RESOLUTION_ACTIVE_EXPORT",                         28);
            dictionaryPushParaList.Add("HIGH_RESOLUTION_APPARENT_IMPORT",                       29);
            dictionaryPushParaList.Add("HIGH_RESOLUTION_APPARENT_EXPORT",                       30);
            dictionaryPushParaList.Add("PRESENT_MONTH_CONSUMPTION_KWH",                         31);
            dictionaryPushParaList.Add("PRESENT_MONTH_CONSUMPTION_KVAH",                        32);
            dictionaryPushParaList.Add("PRESENT_MONTH_MD_KW",	                                33);
            dictionaryPushParaList.Add("PRESENT_MONTH_MD_KVA",                                  34);
            dictionaryPushParaList.Add("TOD_KWH",				                                35);
            dictionaryPushParaList.Add("TOD_KVAH",			                                    36);
            dictionaryPushParaList.Add("TOD_MD_KW",			                                    37);
            dictionaryPushParaList.Add("TOD_MD_KVA",			                                38);	
            dictionaryPushParaList.Add("BILL_KWH_IMPORT",		                                39);
            dictionaryPushParaList.Add("BILL_KVAH_IMPORT",	                                    40);
            dictionaryPushParaList.Add("BILL_KWH_EXPORT",		                                41);
            dictionaryPushParaList.Add("BILL_KVAH_EXPORT",	                                    42);
            dictionaryPushParaList.Add("BILL_KW",				                                43);
            dictionaryPushParaList.Add("BILL_KVA",			                                    44);
            dictionaryPushParaList.Add("BILLING_AVERAGE_PF",	                                45);
            dictionaryPushParaList.Add("BILLING_POWERON_HOURS",                                 46);
            dictionaryPushParaList.Add("BILL_TOD_KWH",		                                    47);
            dictionaryPushParaList.Add("BILL_TOD_KVAH",		                                    48);
            dictionaryPushParaList.Add("BILL_TOD_MD_KW",		                                49);
            dictionaryPushParaList.Add("BILL_TOD_MD_KVA",		                                50);
            dictionaryPushParaList.Add("LAST_BILL_DATE",		                                51);
            dictionaryPushParaList.Add("LAST_BILL_TIME",		                                52);
            dictionaryPushParaList.Add("CONNECT_COUNT_WITH_DATETIME",	                        53);
            dictionaryPushParaList.Add("DISCONNECT_COUNT_WITH_DATETIME",                        54);
            dictionaryPushParaList.Add("TOTAL_TAMPER_COUNTS",	                                55);
            dictionaryPushParaList.Add("LOAD_RELAY_STATUS", 	                                56);
            dictionaryPushParaList.Add("DEMAND_RESET_COUNT", 	                                57);
            dictionaryPushParaList.Add("LOAD_LIMIT_KW",							                58);
            dictionaryPushParaList.Add("METERING_MODE",							                59);
            dictionaryPushParaList.Add("EARTH_LOAD_COUNT_WITH_DATETIME",                        60);
            dictionaryPushParaList.Add("CURRENT_REV_COUNT_WITH_DATETIME",   		            61);
            dictionaryPushParaList.Add("OVER_CURRENT_COUNT_WITH_DATETIME",  		            62);
            dictionaryPushParaList.Add("OVER_LOAD_COUNT_WITH_DATETIME",         	            63);
            dictionaryPushParaList.Add("CUM_ND_COUNT_WITH_DATETIME",	                        64);
            dictionaryPushParaList.Add("LOW_VOLTAGE_COUNT_WITH_DATETIME",   		            65);
            dictionaryPushParaList.Add("OVER_VOLTAGE_COUNT_WITH_DATETIME",			            66);
            dictionaryPushParaList.Add("CUM_SW_COUNT_WITH_DATETIME",	                        67);
            dictionaryPushParaList.Add("CUM_MAGNET_COUNT_WITH_DATETIME",                        68);
            dictionaryPushParaList.Add("CUM_ESD_COUNT_WITH_DATETIME",	                        69);
            dictionaryPushParaList.Add("TEMP_RISE_COUNT_WITH_DATETIME",        	                70);
            dictionaryPushParaList.Add("CURRENT_MISMATCH_COUNT_WITH_DATETIME",	                71);
            dictionaryPushParaList.Add("POWER_FAIL_COUNT_WITH_DATETIME",					    72);
            dictionaryPushParaList.Add("COMMS_TAMPER_COUNT_WITH_DATETIME",       	            73);
            dictionaryPushParaList.Add("COVER_OPEN_COUNT_FIRST_OCCURANCE_DATETIME",             74);
            dictionaryPushParaList.Add("REASON_FOR_DISCONNECTION",						        75);
            dictionaryPushParaList.Add("TEMPERATURE",										    76);
            dictionaryPushParaList.Add("ACTIVE_RATE",											77);
            dictionaryPushParaList.Add("APPARENT_RATE",										  	78);												
            dictionaryPushParaList.Add("COMP1_LATEST",											79);
            dictionaryPushParaList.Add("COMP2_LATEST",											80);
            dictionaryPushParaList.Add("COMP3_LATEST",											81);
            dictionaryPushParaList.Add("COMP4_LATEST",											82);
            dictionaryPushParaList.Add("COMP5_LATEST",											83);
            dictionaryPushParaList.Add("COMP6_LATEST",											84);
            dictionaryPushParaList.Add("COMP7_LATEST",											85);
            dictionaryPushParaList.Add("COMP8_LATEST",											86);
            dictionaryPushParaList.Add("COMP1_COUNTS",											87);
            dictionaryPushParaList.Add("COMP2_COUNTS",											88);
            dictionaryPushParaList.Add("COMP3_COUNTS",											89);
            dictionaryPushParaList.Add("COMP4_COUNTS",											90);
            dictionaryPushParaList.Add("COMP5_COUNTS",											91);
            dictionaryPushParaList.Add("COMP6_COUNTS",											92);
            dictionaryPushParaList.Add("COMP7_COUNTS",											93);
            dictionaryPushParaList.Add("COMP8_COUNTS",											94);
            dictionaryPushParaList.Add("TAMPER_STATUS",                                         95);

            dictionaryPushParaList.Add("CUM_MD_KW",                                             96);
            dictionaryPushParaList.Add("CUM_MD_KVA",                                            97);
            dictionaryPushParaList.Add("LATEST_TAMPER_OCC",                                     98);
            dictionaryPushParaList.Add("LATEST_TAMPER_RES",                                     99);
            dictionaryPushParaList.Add("CASE_OPEN_FLAG_STATUS",                                100);

           return dictionaryPushParaList;
        }
      
        public Dictionary<string, int> HRDisplayParameters_1PHFalcon2()
        {
            Dictionary<string, int> dictionaryHRParaList = new Dictionary<string, int>();
            dictionaryHRParaList.Add("HIGH_RESOLUTION_ACTIVE_IMPORT", 27);
            dictionaryHRParaList.Add("HIGH_RESOLUTION_ACTIVE_EXPORT", 28);
            dictionaryHRParaList.Add("HIGH_RESOLUTION_APPARENT_IMPORT", 29);
            dictionaryHRParaList.Add("HIGH_RESOLUTION_APPARENT_EXPORT", 30);
            return dictionaryHRParaList;
        }



        #region tampernamelist3PhaseSmartMeter

        public string[] tampernamelist3PhaseSmartMeter = new string[]{
                                                    "F",                // BIT 23           
                                                    "E",                       // BIT 22
                                                    "D",                           // BIT 21
                                                    "C",                           // BIT 20
                                                    "B",                           // BIT 19
                                                    "A",                              // BIT 18
                                                    "O",                         // BIT 17
                                                    "N",                         // BIT 16
                                                    "M",                         // BIT 15
                                                    "L",                     // BIT 14
                                                    "K",                     // BIT 13
                                                    "J",                     // BIT 12
                                                    "I",                     // BIT 11
                                                    "H",                    // BIT 10
                                                    "G",                    // BIT 9
                                                    "S",                             // BIT 8
                                                    "R",                            // BIT 7
                                                    "Q",                             // BIT 6
                                                    "P",                                 // BIT 5  
                                                    "X",                           // BIT 4    
                                                    "Y",                     // BIT 3
                                                    "Z",                         // BIT 2
                                                    "W",                                // BIT 1
                                                    "T"                                     // BIT 0                                                    
                                                };


        #endregion


        #region tampernamelist3PhaseNonSmart

        public string[] tampernamelist3PhaseNonSmart = new string[]{
                                                 "F",
                                                  "", //Unused
                                                 "E",                      
                                                 "D",                         
                                                 "C",                           
                                                 "B",                          
                                                 "A",                          
                                                 "O", 
                                                 "N", 
                                                 "M", 
                                                 "L", 
                                                 "K", 
                                                 "J", 
                                                 "I",
                                                 "H",
                                                 "G",
                                                 "S",
                                                 "R",
                                                 "Q",
                                                 "P",  
                                                 "T",  
                                                 "U",
                                                 "V",
                                                 "W" };

        #endregion
        
        public Dictionary<string, string> FillEventStatusSmartMeter()
        {
            Dictionary<string, string> tampaerstatus = new Dictionary<string, string>();
            
            tampaerstatus.Add("External Magnetic Field", "F/ ");                // BIT 23           
            tampaerstatus.Add("Front Cover Open", "E/ ");                       // BIT 22
            tampaerstatus.Add("C Ph Missing", "D/ ");                           // BIT 21
            tampaerstatus.Add("B Ph Missing", "C/ ");                           // BIT 20
            tampaerstatus.Add("A Ph Missing", "B/ ");                           // BIT 19
            tampaerstatus.Add("CT Bypass", "A/ ");                              // BIT 18
            tampaerstatus.Add("C Ph CT Open", "O/ ");                         // BIT 17
            tampaerstatus.Add("B Ph CT Open", "N/ ");                         // BIT 16
            tampaerstatus.Add("A Ph CT Open", "M/ ");                         // BIT 15
            tampaerstatus.Add("C Ph CT Reversal", "L/ ");                     // BIT 14
            tampaerstatus.Add("B Ph CT Reversal", "K/ ");                     // BIT 13
            tampaerstatus.Add("A Ph CT Reversal", "J/ ");                     // BIT 12
            tampaerstatus.Add("Neutral Disturbance", "I/ ");                     // BIT 11
            tampaerstatus.Add("Current Imbalance", "H/ ");                    // BIT 10
            tampaerstatus.Add("Voltage Imbalance", "G/ ");                    // BIT 9
            tampaerstatus.Add("OverVoltage", "S/ ");                             // BIT 8
            tampaerstatus.Add("UnderVoltage", "R/ ");                            // BIT 7
            tampaerstatus.Add("OverCurrent", "Q/ ");                             // BIT 6
            tampaerstatus.Add("Low PF", "P/ ");                                 // BIT 5  
            tampaerstatus.Add("Comm Removed", "X/ ");                           // BIT 4    
            tampaerstatus.Add("Relay Malfunction", "Y/ ");                     // BIT 3
            tampaerstatus.Add("Relay Connect", "Z/ ");                         // BIT 2
            tampaerstatus.Add("OverLoad", "W/ ");                                // BIT 1
            tampaerstatus.Add("2PN", "T ");                                     // BIT 0   
           
            return tampaerstatus;
        }

        public Dictionary<string, string> FillEventStatusNonSmartMeter()
        {
            Dictionary<string, string> tampaerstatus = new Dictionary<string, string>();           
            tampaerstatus.Add("External Magnetic Field", "F/ ");                // BIT 23
            tampaerstatus.Add(" ", " ");                                        // BIT 22 - Unused
            tampaerstatus.Add("Front Cover Open", "E/ ");                       // BIT 21
            tampaerstatus.Add("C Ph Missing", "D/ ");                           // BIT 20
            tampaerstatus.Add("B Ph Missing", "C/ ");                           // BIT 19
            tampaerstatus.Add("A Ph Missing", "B/ ");                           // BIT 18
            tampaerstatus.Add("CT Bypass", "A/ ");                              // BIT 17
            tampaerstatus.Add("C Ph CT Open", "O/ ");                         // BIT 16
            tampaerstatus.Add("B Ph CT Open", "N/ ");                         // BIT 15
            tampaerstatus.Add("A Ph CT Open", "M/ ");                         // BIT 14
            tampaerstatus.Add("C Ph CT Reversal", "L/ ");                     // BIT 13
            tampaerstatus.Add("B Ph CT Reversal", "K/ ");                     // BIT 12
            tampaerstatus.Add("A Ph CT Reversal", "J/ ");                     // BIT 11
            tampaerstatus.Add("Neutral Disturbance", "I/ ");                     // BIT 10
            tampaerstatus.Add("Current Imbalance", "H/ ");                    // BIT 9
            tampaerstatus.Add("Voltage Imbalance", "G/ ");                    // BIT 8
            tampaerstatus.Add("OverVoltage", "S/ ");                             // BIT 7
            tampaerstatus.Add("UnderVoltage", "R/ ");                            // BIT 6
            tampaerstatus.Add("OverCurrent", "Q/ ");                             // BIT 5
            tampaerstatus.Add("Low PF", "P/ ");                                 // BIT 4    
            tampaerstatus.Add("2PN", "T/ ");                                     // BIT 3    
            tampaerstatus.Add("Invalid Phase Sequence", "U/ ");                 // BIT 2
            tampaerstatus.Add("Invalid Voltage", "V/ ");                         // BIT 1
            tampaerstatus.Add("OverLoad", "W/ ");                                // BIT 0

            return tampaerstatus;

        }

    }
}
