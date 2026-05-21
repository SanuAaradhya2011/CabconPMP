using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;

namespace COMMONENTITY
{
    /// <summary>
    /// 1PH source number
    /// </summary>
    public enum MeterSrc1PHCharEnum
    {
        SGS = 4,
        KAYNES = 6,
    }
    /// <summary>
    /// 3PH source number
    /// </summary>
    public enum MeterSrc3PHCharEnum
    {
        SGS = 2,
        KAYNES = 4,
    }
    /// <summary>
    /// Possible Error States.
    /// </summary>
    public static class ErrorStateConstants
    {
        public const string StateAll = "All States";
        public const string StatePassed = "Passed";
        public const string StateInitial = "Initial State";
        public const string StateAnalysis = "Under Analysis";
        public const string StateProcessed = "Processed";
    }
    /// <summary>
    /// Meter PCBAID Source
    /// </summary>
    public static class PCBASourceNames
    {
        public const string PCBSourceAll = "ALL";
        public const string PCBSourceSGS = "SGS";
        public const string PCBSourceKAYNES = "KAYNES";
        public const string PCBSourceOTHER = "OTHER";
    }

    public static class FRSTableNames
    {
        public const string FTMaster = "[AUTOFRS].[dbo].[FTMaster]";
        public const string ExternalFTMaster_New = "[AUTOFRS].[dbo].[ExternalFTMaster_New]";
        public const string CALMaster = "[AUTOFRS].[dbo].[CALMaster]";
        public const string SerializationMaster = "[AUTOFRS].[dbo].[SerializationMaster]";
        public const string SealingMaster = "[AUTOFRS].[dbo].[SealingMaster]";
    }

    public static class FRSColumnNames
    {
        public const string StationID = "StationID";
        public const string PassCount = "PassCount";
        public const string FailCount = "FailCount";
    }

    public static class DateTimeFormats
    {
        public const string DATETIME_FORMAT_DISPLAY = "dd/MM/yyyy hh:mm:ss tt";
        public const string DATETIME_FORMAT_SQL = "MM/dd/yyyy hh:mm:ss tt";
        public const string DATETIME_FORMAT_LEGEND = "dd-MM-yy hh:mm tt";
    }

    /// <summary>
    /// Meter PCBAID Source
    /// </summary>
    public static class ProductionShifts
    {
        public const string Shift1 = "22:00 - 06:00";
        public const string Shift2 = "06:00 - 14:00";
        public const string Shift3 = "14:00 - 22:00";
    }

    /// <summary>
    /// Dictionary keys for SQL query parameter
    /// </summary>
    public static class ParamDictionaryKeys
    {
        public const string ProductionStage = "Production Stage";
        public const string MeterType = "Meter Type";
        public const string ErrorStatus = "Error Status";
        public const string SearchParams = "Search Criteria";
        public const string UniquePCBA = "Unique PCBA";
        public const string ErrorType = "Error Type";
        public const string ErrorDateType = "Error Date Type";
        public const string ErrorDateStart = "Start Date";
        public const string ErrorDateEnd = "End Date";
        public const string Customer = "Customer";
        public const string ProductionShift = "Production Shift";
        public const string PCBASource = "PCBA Source";
    }
    /// <summary>
    /// Miscellaneous functions for Rejection Module.
    /// </summary>
    public static class ErrorUtility
    {

        public const string KeywordDeleteEntry = "DELREC";
        public const string KeywordNewError = "NEWERR";

        public const string MeterType_ALL = "All Meters";

        public const string TestType_ALL = "All Stages";
        public const string TestType_Sealing = "Sealing";
        public const string TestType_Assembly = "Assembly";

        public const string SearchTypeALL = "ALL";

        /// <summary>
        /// Enum for Error Date Filter
        /// </summary>
        public enum ErrorDateType
        {
            OriginalErrorDate = 0,
            ActionDate = 1
        }

 		/// <summary>
        /// Convert string to Pascal case, optionally removing strings.
        /// </summary>
        /// <param name="textToChange"></param>
        /// <param name="bRemoveSpace"></param>
        /// <returns>Pascal case string</returns>
        public static string ConvertToPascalCase(string textToChange, bool bRemoveSpace)
        {
            System.Text.StringBuilder resultBuilder = new System.Text.StringBuilder();

            foreach (char c in textToChange)
            {
                if (!Char.IsLetterOrDigit(c))
                {
                    resultBuilder.Append(" ");
                }
                else
                {
                    resultBuilder.Append(c);
                }
            }

            string result = resultBuilder.ToString();

            result = result.ToLower();

            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;

            result = myTI.ToTitleCase(result);
            if (bRemoveSpace) result = result.Replace(" ", String.Empty);
            return result;
        }

        /// <summary>
        /// Get Work Shift Date
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string GetShiftFromDate(DateTime dateTime)
        {
            if (dateTime.TimeOfDay.Hours >= 22 && dateTime.TimeOfDay.Hours < 6)
                return ProductionShifts.Shift1;
            else if (dateTime.TimeOfDay.Hours >= 6 && dateTime.TimeOfDay.Hours < 14)
                return ProductionShifts.Shift2;
            else if (dateTime.TimeOfDay.Hours >= 14 && dateTime.TimeOfDay.Hours < 22)
                return ProductionShifts.Shift3;
            else
                return string.Empty;
        }

        /// <summary>
        /// Set PCB Source from PCBAID; 
        /// ***************************
        /// 1PH
        /// 1st char '4' = SGS
        /// 1st char '6' = Kaynes
        /// ***************************
        /// 3PH
        /// 6th char '2' = SGS
        /// 6th char '4' = Kaynes
        /// ***************************
        /// Smart Meter all SGS
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string GetSourceFromPCBAID_MeterType(string pcbID, string meterType)
        {
            try
            {
                if (meterType.Equals(StaticVariables.MeterType_1PH_SM) || meterType.Equals(StaticVariables.MeterType_3PH_SM))
                    return PCBASourceNames.PCBSourceSGS;
                else if (pcbID[0] == ((int)MeterSrc1PHCharEnum.SGS).ToString()[0])
                    return PCBASourceNames.PCBSourceSGS;
                else if (pcbID[0] == ((int)MeterSrc1PHCharEnum.KAYNES).ToString()[0])
                    return PCBASourceNames.PCBSourceKAYNES;
                else if (pcbID[5] == ((int)MeterSrc3PHCharEnum.SGS).ToString()[0])
                    return PCBASourceNames.PCBSourceSGS;
                else if (pcbID[5] == ((int)MeterSrc3PHCharEnum.KAYNES).ToString()[0])
                    return PCBASourceNames.PCBSourceKAYNES;
                else
                    return PCBASourceNames.PCBSourceOTHER;
            }
            catch (Exception ex)
            {
                return PCBASourceNames.PCBSourceOTHER;
            }

        }

        /// <summary>
        /// Insert spaces between Camel Case string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SplitStringCamelCase(string str)
        {
            string result = Regex.Replace(
                                        Regex.Replace(str, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"),
                                        @"(\p{Ll})(\P{Ll})",
                                        "$1 $2");
            return result;
        }

        /// <summary>
        /// Compress consecutive tabs & spaces to single space
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string NormalizeWhiteSpace(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            int current = 0;
            char[] output = new char[input.Length];
            bool skipped = false;

            foreach (char c in input.ToCharArray())
            {
                if (char.IsWhiteSpace(c))
                {
                    if (!skipped)
                    {
                        if (current > 0)
                            output[current++] = ' ';

                        skipped = true;
                    }
                }
                else
                {
                    skipped = false;
                    output[current++] = c;
                }
            }

            return new string(output, 0, current);
        }
    }

    public partial class Tuple<T1, T2, T3, T4, T5>
    {
        public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
        {
            this.Item1 = item1;
            this.Item2 = item2;
            this.Item3 = item3;
            this.Item4 = item4;
            this.Item5 = item5;
        }

        public T1 Item1 { get; set; }
        public T2 Item2 { get; set; }
        public T3 Item3 { get; set; }
        public T4 Item4 { get; set; }
        public T5 Item5 { get; set; }
    }

    public static partial class Tuple
    {
        public static Tuple<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(
            T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
        {
            return new Tuple<T1, T2, T3, T4, T5>(item1, item2, item3, item4, item5);
        }
    }


    public class MultiKeyDictionary<T1, T2, T3> : Dictionary<T1, Dictionary<T2, T3>>
    {
        new public Dictionary<T2, T3> this[T1 key]
        {
            get
            {
                if (!ContainsKey(key))
                    Add(key, new Dictionary<T2, T3>());

                Dictionary<T2, T3> returnObj;
                TryGetValue(key, out returnObj);

                return returnObj;
            }
        }
    }

    /// <summary>
    /// SQL column names for RejectionTable in database.
    /// </summary>
    public struct RejectionTable
    {
        public const string colIDReject = "IDReject";
        public const string colPCBAID = "PCBAID";
        public const string colCustomer = "Customer";
        public const string colMeterType = "MeterType";
        public const string colErrorDate = "ErrorDate";
        public const string colActionDate = "ActionDate";
        public const string colProductionStage = "ProductionStage";
        public const string colParameterName = "ParameterName";
        public const string colErrorState = "ErrorState";
        public const string colRejectionAction = "RejectionAction";
        public const string colProblemDescription = "ProblemDescription";
        public const string colRejectionCause = "RejectionCause";
        public const string colMoreInformation = "MoreInformation";
        public const string colIDError = "ErrorID";
        //public const string colHistoryStatus = "HistoryStatus";
        public const string colLoggedUserID = "LoggedUserID";
        public const string colWorkstationID = "WorkstationID";
        public const string colLastRepairID = "LastRepairID";
        public const string dbRejectionMasterTableName = "tabRejectionMaster";
        public const string colPCBSource = "Source";
        public const string colShiftEDate = "EDateShift";
        public const string colShiftADate = "ADateShift";
    }
    /// <summary>
    /// SQL column names for Error Types table in Database.
    /// </summary>
    public struct ErrorTypesTable
    {
        public const string colIdError = "IDError";
        public const string colErrorType = "ErrorType";
        public const string colErrorname = "ErrorName";
        public const string colErrorDescription = "ErrorDescription";
        public const string dbErrorTypeTableName = "tabErrorTypeMaster";
    }


}
