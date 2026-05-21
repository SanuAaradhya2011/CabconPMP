using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using BALLAYER;
using System.Threading;
using System.Threading.Tasks;
using COMMONENTITY;
using System.Diagnostics;
using System.IO.Pipes;
using System.Windows.Forms.DataVisualization.Charting;
using System.Globalization;

namespace LGPMPREJECTIONTOOL
{

    public partial class frmDisplayChart : Form
    {
        private static readonly string[] STATIONID_ASSY = { "INCCUPC049", "INCCUPC054", "INCCUPC1270", "INCCUPC130", "INCCUPC208", "INCCUPC035", "INCCUPC069", "INCCUPC555", "INCCUPC300", "INCCUPC552", "INCCUPC553", "INCCUPC507", "INCCUPC570", "INCCUPC575", "INCCUPC576", "INCCUPC577", "INCCUPC578" };
        private static readonly string[] STATIONID_FT = { "INCCUPC553", "INCCUPC049", "INCCUPC054", "INCCUPC1270", "INCCUPC130", "INCCUPC208", "INCCUPC035", "INCCUPC069", "INCCUPC555", "INCCUPC300", "INCCUPC552", "INCCUPC507", "INCCUPC570", "INCCUPC575", "INCCUPC576", "INCCUPC577", "INCCUPC578", "INCCUPC049", "INCCUPC054", "INCCUPC1270", "INCCUPC205", "INCCUPC206", "INCCUPC130", "INCCUPC208", "INCCUPC035", "INCCUPC069", "INCCUPC555", "INCCUPC300", "INCCUPC552", "INCCUPC553", "INCCUPC207", "INCCUPC507", "INCCPC208", "INCCUPC210", "INCCU1PC16", "INCCU1PC17", "INCCU1PC18", "INCCU1PC19", "INCCUPC059", "INCCUPC570", "INCCUPC575", "INCCUPC576", "INCCUPC577", "INCCUPC578", "INCCUPC579", "INCCUPC580", "INCCUPC581", "INCCUPC583" };
        private static readonly string[] STATIONID_CAL = { "CALN-1", "INCCUPC304", "CALN-4", "CALN-8", "INCCUPC65", "INCCUPC216", "CALN-6", "INCCUPC066", "INCCUPC145", "INCCUPC092", "INCCUPC68", "INCCUPC730", "CALN-5", "INCCUPC011" };
        private static readonly string[] STATIONID_SER = { "INCCUPC085", "INCCUPC093", "INCCUPC094", "INCCUPC505", "INCCUPC103", "INCCU1PC11", "INCCUPC105", "INCCUPC204", "INCCUPC004", "INCCUPC021", "INCCUPC202", "INCCUPC203", "INCCUPC580", "INCCUPC002", "INCCUPC571", "INCCUPC572", "INCCUPC585", "INCCUPC218", "INCCUPC212", "INCCUPC214" };
        private static readonly string[] STATIONID_SEAL = { "INCCUPC346", "INCCUB30", "INCCUPC087", "INCCUNB14", "INCCUPC211", "INCCUPC551", "INCCUPC120", "INCCUPC166", "INCCUPC505", "INCCUPC090", "INCCUPC103", "INCCUPC122", "INCCUPC129", "INCCUPC161", "INCCUPC1PC09", "INCCUPC406", "INCCUPC128" };
        private static readonly string[] STATIONID_PACK = { "INCCUPC346", "INCCUNB30", "INCCUPC087", "INCCUNB14", "INCCUPC211", "INCCUPC213", "INCCUPC551", "INCCUPC120", "INCCUPC166", "INCCUPC505", "INCCUPC090", "INCCUPC103", "INCCUPC122", "INCCUPC129", "INCCUPC161", "INCCUPC1PC09", "INCCUPC406", "INCCUPC128", "INCCUPC215" };
        private static frmDisplayChart openForm = null;
        private static Dictionary<string, string> param;
        private static string query = "";
        private string DBAUTOFRS = "AUTOFRS";
        private string DBPMP = "PMPDB";
        bool bInProcess = false;
        private const float CZoomScale = 1.1f;
        PausableTimer timerReject;
        PausableTimer timerProd;
        Series seriesCumPercent;
        Series seriesCount;
        Series seriesValPercent;
        Series seriesFTPass;
        Series seriesFTFail;
        Series seriesFTExtPass;
        Series seriesFTExtFail;
        Series seriesCalPass;
        Series seriesCalFail;
        Series seriesSerPass;
        Series seriesSerFail;
        Series seriesSealPass;
        Series seriesSealFail;
        Series seriesY;
        Series seriesY2;
        Legend legendReject;
        Legend legendProd;
        DataTable[] dataTableGridSourceFT = {null, null};
        DataTable[] dataTableGridSourceFTExt= {null, null};
        DataTable[] dataTableGridSourceCal= {null, null};
        DataTable[] dataTableGridSourceSerial= {null, null};
        DataTable[] dataTableGridSourceSeal= {null, null};
        DataTable datatableGridSourceReject= null;
        MultiKeyDictionary<string, string, COMMONENTITY.Tuple<List<string>, List<string>, List<string>, List<string>, List<string>>> dataDict = new MultiKeyDictionary<string, string, COMMONENTITY.Tuple<List<string>, List<string>, List<string>, List<string>, List<string>>>();
        CancellationTokenSource token = new CancellationTokenSource();//   CancellationTokenSource token = new CancellationTokenSource();

        public frmDisplayChart(Dictionary<string, string> chartparam, string rowFilterQuery)
        {
            InitializeComponent();
            param = chartparam;
            query = rowFilterQuery;
            cmbRefreshTime.SelectedItem = "60";
            cmbShowProdMin.SelectedItem = "30";
            cmbShowRejectMin.SelectedItem = "30";
            timerReject = new PausableTimer(30*1000*60);
            timerProd = new PausableTimer(30*1000*60);
            timerReject.Elapsed += timerReject_Tick;
            timerProd.Elapsed += timerProd_Tick;
            timerReject.Start();
            seriesCount = chart2.Series["SeriesCount"];
            seriesCumPercent = chart2.Series["SeriesCumPercent"];
            seriesValPercent = chart2.Series["SeriesValPercent"];
            seriesFTPass = chart2.Series["SeriesFTPass"];
            seriesFTFail = chart2.Series["SeriesFTFail"];
            seriesFTExtPass = chart2.Series["SeriesFTExtPass"];
            seriesFTExtFail = chart2.Series["SeriesFTExtFail"];
            seriesCalPass = chart2.Series["SeriesCalPass"];
            seriesCalFail = chart2.Series["SeriesCalFail"];
            seriesSerPass = chart2.Series["SeriesSerPass"];
            seriesSerFail = chart2.Series["SeriesSerFail"];
            seriesSealPass = chart2.Series["SeriesSealPass"];
            seriesSealFail = chart2.Series["SeriesSealFail"];
            legendReject = chart2.Legends[0];
            legendProd = chart2.Legends[1];
            seriesCount.ToolTip = "##AXISLABEL Count = #VALY";
            seriesCumPercent.ToolTip = "Cumulative Percentage = #VALY";
            chart2.MouseWheel += new MouseEventHandler(chart2_MouseWheel);
            dgvData.MouseWheel+=new MouseEventHandler(dgvData_MouseWheel);
            dtpStart.Value = DateTime.Now.AddDays(-1);
            dtpEnd.Value = DateTime.Now;
            rdbFT.Checked = true;
            rdbAUTOFRS.Checked = true;
            btnShowProdData.Text = "Display Production Line Data";
            SetProdPanelVisible(false);
            SetProductionSeries(false);
        }

        private void SetProdPanelVisible(bool state)
        {
            panelProd.Enabled = state;
            if (state)
            {
                chart2.ChartAreas[0].AxisY.Title = "Pass Count";
                chart2.ChartAreas[0].AxisY2.Title = "Fail Count";
                chart2.ChartAreas[0].AxisX.IsLabelAutoFit = true;
            }
            else
            {
                chart2.ChartAreas[0].AxisY.Title = "Count";
                chart2.ChartAreas[0].AxisY2.Title = "Percentage";
                chart2.ChartAreas[0].AxisX.IsLabelAutoFit = false;
            }
            legendReject.Enabled = !state;
            chart2.Titles[0].Visible = !state;
            legendProd.Enabled = state;
            chart2.Titles[1].Visible = state;
        }

        void dgvData_MouseWheel(object sender, MouseEventArgs e)
        {
            int currentIndex = this.dgvData.FirstDisplayedScrollingColumnIndex;
            int scrollLines = SystemInformation.MouseWheelScrollLines;

            if (e.Delta > 0)
              {
                  this.dgvData.FirstDisplayedScrollingColumnIndex = Math.Max(1, currentIndex - scrollLines);
            }
            else if (e.Delta < 0)
            {
                if (this.dgvData.ColumnCount > (currentIndex + scrollLines))
                    this.dgvData.FirstDisplayedScrollingColumnIndex = currentIndex + scrollLines;
            }
        }

        private void chart2_MouseWheel(object sender, MouseEventArgs e)
        {
            try
            {
                Axis xAxis = chart2.ChartAreas[0].AxisX;
                double xMin = xAxis.ScaleView.ViewMinimum;
                double xMax = xAxis.ScaleView.ViewMaximum;
                double xPixelPos = xAxis.PixelPositionToValue(e.Location.X);

                if (e.Delta < 0)
                {
                    double xStartPos = Math.Max(xPixelPos - (xPixelPos - xMin) * CZoomScale, 0);
                    double xEndPos = Math.Min(xStartPos + (xMax - xMin) * CZoomScale, xAxis.Maximum);

                    if (xStartPos >= xAxis.Minimum && xEndPos <= xAxis.Maximum)
                        xAxis.ScaleView.Zoom(xStartPos, xEndPos);

                }
                else if (e.Delta > 0)
                {
                    // Scrolled up, meaning zoom in
                    double xStartPos = Math.Max(xPixelPos - (xPixelPos - xMin) / CZoomScale, 0);
                    double xEndPos = Math.Min(xStartPos + (xMax - xMin) / CZoomScale, xAxis.Maximum);

                    xStartPos = Math.Ceiling(xStartPos);
                    xEndPos = Math.Floor(xEndPos);
                    if (xStartPos >= xAxis.Minimum && xEndPos <= xAxis.Maximum)
                        xAxis.ScaleView.Zoom(xStartPos, xEndPos);
                }
            }
            catch { }
        }

        /// <summary>
        /// Constructor function for ErrorList Form, always maintains only one instance
        /// </summary>
        /// <param name="objetyusermgt"></param>
        /// <returns></returns>
        public static frmDisplayChart GetInstance(Dictionary<string, string> chartparam, string rowFilterQuery)
        {
            if (openForm == null)
            {
                if (chartparam == null)
                {
                    return null;
                }
                openForm = new frmDisplayChart(chartparam, rowFilterQuery);
                openForm.FormClosing += (s, e) => 
                { 
                    openForm.token.Cancel(); 
                    openForm = null; 
                };
            }
            else
            {
                param = chartparam;
                query = rowFilterQuery;
            }

            return openForm;
        }

        private void frmDisplayChart_VisibleChanged(object sender, EventArgs e)
        {
            if (openForm.Visible)
            {
                timer1.Start();
            }
            else
            {
                timer1.Stop();
            }
        }

        public static void ShowInstance(Dictionary<string, string> chartparam, string rowFilterQuery)
        {
            GetInstance(chartparam, rowFilterQuery).Show();
            openForm.BringToFront();

            openForm.RefreshAllData();
        }

        private void SetControlState(bool state)
        {
            refreshToolStripMenuItem.Enabled = state;
            cmbRefreshTime.Enabled = state;
            progressBar1.Style = state ? ProgressBarStyle.Continuous : ProgressBarStyle.Marquee;
            panelDuration.Enabled = state;
            btnShowProdData.Enabled = state;
            chkChartLabel.Enabled = state;
        }

        private void GetProductionData(int idx)
        {
            try
            {
                BALErrorOperations balerrorobj = new BALErrorOperations();
                string DB = idx == 0 ? DBAUTOFRS : DBPMP;
                foreach (Control rdb in panelRdbStages.Controls)
                {
                    if (token.IsCancellationRequested) return;
                    DataTable dataTableGridSourceProd = null;
                    string tableName = "";
                    string procedure = "";
                    switch (rdb.Name)
                    {
                        case "rdbFT":
                            tableName = idx == 0 ? FRSTableNames.FTMaster : StaticVariables.TestType_FT;
                            procedure = StaticVariables.TestType_FT;
                            dataTableGridSourceFT[idx] = new DataTable();
                            dataTableGridSourceProd = dataTableGridSourceFT[idx];
                            break;
                        case "rdbFTExt":
                            tableName = idx == 0 ? FRSTableNames.ExternalFTMaster_New : string.Empty;
                            procedure = FRSTableNames.ExternalFTMaster_New;
                            dataTableGridSourceFTExt[idx] = new DataTable();
                            dataTableGridSourceProd = dataTableGridSourceFTExt[idx];
                            break;
                        case "rdbCal":
                            tableName = idx == 0 ? FRSTableNames.CALMaster : StaticVariables.TestType_Cal;
                            procedure = StaticVariables.TestType_Cal;
                            dataTableGridSourceCal[idx] = new DataTable();
                            dataTableGridSourceProd = dataTableGridSourceCal[idx];
                            break;
                        case "rdbSer":
                            tableName = idx == 0 ? FRSTableNames.SerializationMaster : StaticVariables.TestType_SR;
                            procedure = StaticVariables.TestType_SR;
                            dataTableGridSourceSerial[idx] = new DataTable();
                            dataTableGridSourceProd = dataTableGridSourceSerial[idx];
                            break;
                        case "rdbSeal":
                            tableName = idx == 0 ? FRSTableNames.SealingMaster : string.Empty;
                            procedure = FRSTableNames.SealingMaster;
                            dataTableGridSourceSeal[idx] = new DataTable();
                            dataTableGridSourceProd = dataTableGridSourceSeal[idx];
                            break;
                        default:
                            tableName = string.Empty;
                            break;
                    }

                    if (string.IsNullOrEmpty(tableName)) continue;

                    string startDateTime = dtpStart.Value.ToString(DateTimeFormats.DATETIME_FORMAT_SQL.Replace("ss", "00"), CultureInfo.InvariantCulture);
                    string endDateTime = dtpEnd.Value.ToString(DateTimeFormats.DATETIME_FORMAT_SQL.Replace("ss", "59"), CultureInfo.InvariantCulture);

                    if (token.IsCancellationRequested) return;
                    DataTable tableProductionData = idx == 0 ? balerrorobj.GetProductionDataAutoFRS(tableName, startDateTime, endDateTime)
                                                            : balerrorobj.GetProductionDataPMPDB(tableName, startDateTime, endDateTime)
                                                            ;
                    if (token.IsCancellationRequested) return;

                    dataTableGridSourceProd.Columns.Add(new DataColumn("Header", typeof(string)) { AllowDBNull = false, DefaultValue = string.Empty });
                    dataTableGridSourceProd.Rows.Add("Total");
                    dataTableGridSourceProd.Rows.Add("Machine Name");
                    dataTableGridSourceProd.Rows.Add("Pass");
                    dataTableGridSourceProd.Rows.Add("Fail");
                    dataTableGridSourceProd.Rows.Add("Total/MC");

                    List<string> listStationID = new List<string>();
                    List<string> listFailCount = new List<string>();
                    List<string> listPassCount = new List<string>();
                    List<string> item4 = null;
                    List<string> item5 = null;
                    int rowIdx = 1;
                    for (int count = 0; count < tableProductionData.Rows.Count; count++)
                    {
                        string stationID = tableProductionData.Rows[count][FRSColumnNames.StationID].ToString().Trim();
                        bool bIsValidID = false;
                        bIsValidID =   STATIONID_ASSY.Any(stationID.Contains)
                                    || STATIONID_FT.Any(stationID.Contains)
                                    || STATIONID_CAL.Any(stationID.Contains)
                                    || STATIONID_SER.Any(stationID.Contains)
                                    || STATIONID_SEAL.Any(stationID.Contains)
                                    || STATIONID_PACK.Any(stationID.Contains)
                                    ;
                        if (!bIsValidID) continue;

                        string colName = string.Format("D{0}", rowIdx++);
                        if (!dataTableGridSourceProd.Columns.Contains(colName))
                        {
                            dataTableGridSourceProd.Columns.Add(new DataColumn(colName, typeof(string)) { AllowDBNull = false, DefaultValue = string.Empty });
                        }
                        if (rowIdx == 2)
                        {
                            dataTableGridSourceProd.Columns.Add(new DataColumn(string.Format("D{0}", rowIdx), typeof(string)) { AllowDBNull = false, DefaultValue = string.Empty });
                        }

                        dataTableGridSourceProd.Rows[1][colName] = tableProductionData.Rows[count][FRSColumnNames.StationID].ToString();
                        dataTableGridSourceProd.Rows[2][colName] = tableProductionData.Rows[count][FRSColumnNames.PassCount].ToString();
                        dataTableGridSourceProd.Rows[3][colName] = tableProductionData.Rows[count][FRSColumnNames.FailCount].ToString();
                        dataTableGridSourceProd.Rows[4][colName] = ((int)tableProductionData.Rows[count][FRSColumnNames.FailCount] + (int)tableProductionData.Rows[count][FRSColumnNames.PassCount]).ToString();


                        listStationID.Add(tableProductionData.Rows[count][FRSColumnNames.StationID].ToString());
                        listPassCount.Add(tableProductionData.Rows[count][FRSColumnNames.PassCount].ToString());
                        listFailCount.Add(tableProductionData.Rows[count][FRSColumnNames.FailCount].ToString());
                    }
                    dataDict[DB][procedure] =COMMONENTITY.Tuple.Create(listStationID, listPassCount, listFailCount, item4, item5);

                    if (tableProductionData.Rows.Count > 0)
                    {
                        dataTableGridSourceProd.Rows[0]["D1"] = "Pass: " + tableProductionData.Compute("Sum(" + FRSColumnNames.PassCount + ")", string.Empty);
                        dataTableGridSourceProd.Rows[0]["D2"] = "Fail: " + tableProductionData.Compute("Sum(" + FRSColumnNames.FailCount + ")", string.Empty);
                    }
                    else
                    {
                        dataTableGridSourceProd.Columns.Add(new DataColumn("D1", typeof(string)) { AllowDBNull = false, DefaultValue = string.Empty });
                        dataTableGridSourceProd.Columns.Add(new DataColumn("D2", typeof(string)) { AllowDBNull = false, DefaultValue = string.Empty });
                        dataTableGridSourceProd.Rows[0]["D1"] = "Pass: " + 0;
                        dataTableGridSourceProd.Rows[0]["D2"] = "Fail: " + 0;
                    }
                    dataTableGridSourceProd.AcceptChanges();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void SetProductionData()
        {
            if (dataDict.Count == 0) return;
            string db = rdbAUTOFRS.Checked ? DBAUTOFRS : DBPMP;

            if (dataDict[RejectionTable.dbRejectionMasterTableName][RejectionTable.dbRejectionMasterTableName].Item1.Count > 0)
            {
                seriesCount.Points.DataBindXY(dataDict[RejectionTable.dbRejectionMasterTableName][RejectionTable.dbRejectionMasterTableName].Item1
                                                , dataDict[RejectionTable.dbRejectionMasterTableName][RejectionTable.dbRejectionMasterTableName].Item2);
                seriesCumPercent.Points.DataBindY(dataDict[RejectionTable.dbRejectionMasterTableName][RejectionTable.dbRejectionMasterTableName].Item3);
                seriesValPercent.Points.DataBindY(dataDict[RejectionTable.dbRejectionMasterTableName][RejectionTable.dbRejectionMasterTableName].Item4);
            }
            if (dataDict.ContainsKey(db) && dataDict[db].ContainsKey(StaticVariables.TestType_FT))
            {
                seriesFTPass.Points.DataBindXY(dataDict[db][StaticVariables.TestType_FT].Item1, dataDict[db][StaticVariables.TestType_FT].Item2);
                seriesFTFail.Points.DataBindY(dataDict[db][StaticVariables.TestType_FT].Item3);
            }
            if (dataDict.ContainsKey(db) && dataDict[db].ContainsKey(StaticVariables.TestType_Cal))
            {
                seriesCalPass.Points.DataBindXY(dataDict[db][StaticVariables.TestType_Cal].Item1, dataDict[db][StaticVariables.TestType_Cal].Item2);
                seriesCalFail.Points.DataBindY(dataDict[db][StaticVariables.TestType_Cal].Item3);
            }
            if (dataDict.ContainsKey(db) && dataDict[db].ContainsKey(StaticVariables.TestType_SR))
            {
                seriesSerPass.Points.DataBindXY(dataDict[db][StaticVariables.TestType_SR].Item1, dataDict[db][StaticVariables.TestType_SR].Item2);
                seriesSerFail.Points.DataBindY(dataDict[db][StaticVariables.TestType_SR].Item3);
            }
            if (db == DBAUTOFRS)
            {
                if (dataDict.ContainsKey(db) && dataDict[db].ContainsKey(FRSTableNames.ExternalFTMaster_New))
                {
                    seriesFTExtPass.Points.DataBindXY(dataDict[db][FRSTableNames.ExternalFTMaster_New].Item1, dataDict[db][FRSTableNames.ExternalFTMaster_New].Item2);
                    seriesFTExtFail.Points.DataBindY(dataDict[db][FRSTableNames.ExternalFTMaster_New].Item3);
                }
                if (dataDict.ContainsKey(db) && dataDict[db].ContainsKey(FRSTableNames.SealingMaster))
                {
                    seriesSealPass.Points.DataBindXY(dataDict[db][FRSTableNames.SealingMaster].Item1, dataDict[db][FRSTableNames.SealingMaster].Item2);
                    seriesSealFail.Points.DataBindY(dataDict[db][FRSTableNames.SealingMaster].Item3);
                }
            }
        }

        private void DisplayMSChartReject()
        {
            BALErrorOperations balerrorobj = new BALErrorOperations();
            datatableGridSourceReject = new DataTable();
            try
            {
                float TOTAL = 0f;

                if (token.IsCancellationRequested) return;
                DataTable tableRejectionMaster = balerrorobj.GetRejectionMasterTable(param);
                if (token.IsCancellationRequested) return;

                DataColumn col = new DataColumn(RejectionTable.colPCBSource, typeof(string));
                tableRejectionMaster.Columns.Add(col);
                foreach (DataRow row in tableRejectionMaster.Rows)
                {
                    row[RejectionTable.colPCBSource] = ErrorUtility.GetSourceFromPCBAID_MeterType(row[RejectionTable.colPCBAID].ToString(), row[RejectionTable.colMeterType].ToString());
                }

                col = new DataColumn(param[ParamDictionaryKeys.ErrorDateType] == RejectionTable.colErrorDate ? RejectionTable.colShiftEDate : RejectionTable.colShiftADate, typeof(string));
                tableRejectionMaster.Columns.Add(col);
                foreach (DataRow row in tableRejectionMaster.Rows)
                {
                    row[col.ColumnName] = ErrorUtility.GetShiftFromDate((DateTime)row[param[ParamDictionaryKeys.ErrorDateType]]);
                }
                tableRejectionMaster.AcceptChanges();

                tableRejectionMaster.DefaultView.RowFilter = query;
                tableRejectionMaster = tableRejectionMaster.DefaultView.ToTable();
                var errorQuery = from row in tableRejectionMaster.AsEnumerable()
                                 group row by row.Field<string>((ErrorTypesTable.colErrorname)) into error
                                 orderby error.Key
                                 select new
                                 {
                                     ErrorType = error.Key,
                                     CountOfErrors = error.Count()
                                 };
                var dict = errorQuery.ToDictionary(t => t.ErrorType, t => t.CountOfErrors);
                var sorteddict = dict.OrderByDescending(i => i.Value).ToDictionary(i => i.Key, i => i.Value);

                string initialzoom = "";
                bool bUseInitialZoom = true;

                TOTAL = sorteddict.Sum(x => x.Value);

                datatableGridSourceReject.Columns.Add(new DataColumn("Header", typeof(string)) { AllowDBNull = false, DefaultValue = string.Empty });
                datatableGridSourceReject.Rows.Add("Total");
                datatableGridSourceReject.Rows.Add("No. #");
                datatableGridSourceReject.Rows.Add("Type");
                datatableGridSourceReject.Rows.Add("Count");
                datatableGridSourceReject.Rows.Add("Percent");
                datatableGridSourceReject.Rows.Add("Cum.%");

                List<string> listErrorName = new List<string>();
                List<string> listErrorCount = new List<string>();
                List<string> listCumPercent = new List<string>();
                List<string> listValPercent = new List<string>();
                List<string> listIdx = new List<string>();

                for (int idx = 0; idx < sorteddict.Count; idx++)
                {
                    string colName = string.Format("D{0}", idx + 1);
                    datatableGridSourceReject.Columns.Add(new DataColumn(colName, typeof(string)) { AllowDBNull = false, DefaultValue = string.Empty });

                    float percent = 0;
                    float subtotal = 0;
                    for (int item = 0; item <= idx; item++)
                    {
                        subtotal += sorteddict.ElementAt(item).Value;
                    }
                    percent = subtotal / TOTAL * 100;


                    if (percent >= 80.00 && bUseInitialZoom && idx > 25)
                    {
                        initialzoom = string.Format("initialRange: [0, {0}],", idx + 1);
                        bUseInitialZoom = false;
                    }

                    listIdx.Add((idx + 1).ToString());
                    listErrorName.Add(sorteddict.ElementAt(idx).Key.Replace('_', ' '));
                    listErrorCount.Add(sorteddict.ElementAt(idx).Value.ToString("0.00"));
                    listCumPercent.Add(percent.ToString());
                    listValPercent.Add((sorteddict.ElementAt(idx).Value / TOTAL * 100).ToString("0.00"));

                    datatableGridSourceReject.Rows[1][colName] = string.Format("#{0}", idx + 1);
                    datatableGridSourceReject.Rows[2][colName] = listErrorName[idx];
                    datatableGridSourceReject.Rows[3][colName] = listErrorCount[idx];
                    datatableGridSourceReject.Rows[4][colName] = listValPercent[idx];
                    datatableGridSourceReject.Rows[5][colName] = listCumPercent[idx];
                }
                datatableGridSourceReject.Rows[0]["D1"] = TOTAL.ToString();
                datatableGridSourceReject.AcceptChanges();


                dataDict[RejectionTable.dbRejectionMasterTableName][RejectionTable.dbRejectionMasterTableName] = COMMONENTITY.Tuple.Create(listIdx, listErrorCount, listCumPercent, listValPercent, listErrorName);
            }
            catch(Exception ex)
            {
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshAllData();
        }

        private void RefreshAllData()
        {
            token = new CancellationTokenSource();
            if (bInProcess) { MessageBox.Show("Ongoing operation!"); return; }
            Task.Factory.StartNew(() =>
            {
                PausableTimer currentTimer = null;
                try
                {
                    bInProcess = true;
                    TimeSpan span = new DateTime(dtpEnd.Value.Year, dtpEnd.Value.Month, dtpEnd.Value.Day, dtpEnd.Value.Hour, dtpEnd.Value.Minute, 59)
                                        - new DateTime(dtpStart.Value.Year, dtpStart.Value.Month, dtpStart.Value.Day, dtpStart.Value.Hour, dtpStart.Value.Minute, 00);
                    Invoke(new Action(() =>
                    {
                        if (timerReject.Enabled) currentTimer = timerReject;
                        if (timerProd.Enabled) currentTimer = timerProd;
                        currentTimer.Pause();
                        dtpEnd.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 59);
                        dtpStart.Value = dtpEnd.Value - span;
                        SetControlState(false);

                        seriesCount.Points.Clear();
                        seriesCumPercent.Points.Clear();
                        seriesValPercent.Points.Clear();
                    }));
                    span = DateTime.ParseExact(param[ParamDictionaryKeys.ErrorDateEnd], DateTimeFormats.DATETIME_FORMAT_SQL, CultureInfo.InvariantCulture) - DateTime.ParseExact(param[ParamDictionaryKeys.ErrorDateStart], DateTimeFormats.DATETIME_FORMAT_SQL, CultureInfo.InvariantCulture);

                    param[ParamDictionaryKeys.ErrorDateEnd] = DateTime.Now.ToString(DateTimeFormats.DATETIME_FORMAT_SQL, CultureInfo.InvariantCulture);
                    param[ParamDictionaryKeys.ErrorDateStart] = (DateTime.Now - span).ToString(DateTimeFormats.DATETIME_FORMAT_SQL, CultureInfo.InvariantCulture);

                    token = new CancellationTokenSource();
                    Parallel.Invoke(DisplayMSChartReject, () => GetProductionData(0), () => GetProductionData(1));   //, () => GetProductionData(1)
                }
                finally
                {
                    if (!token.IsCancellationRequested)
                    {
                        Invoke(new Action(() =>
                        {
                            SetControlState(true);
                            txtDataTime.Text = DateTime.Now.ToString(DateTimeFormats.DATETIME_FORMAT_DISPLAY, CultureInfo.InvariantCulture);
                            legendReject.Title = DateTime.ParseExact(param[ParamDictionaryKeys.ErrorDateStart], DateTimeFormats.DATETIME_FORMAT_SQL, CultureInfo.InvariantCulture).ToString(DateTimeFormats.DATETIME_FORMAT_LEGEND, CultureInfo.InvariantCulture)
                                                        + " ▬ " +
                                                       DateTime.ParseExact(param[ParamDictionaryKeys.ErrorDateEnd], DateTimeFormats.DATETIME_FORMAT_SQL, CultureInfo.InvariantCulture).ToString(DateTimeFormats.DATETIME_FORMAT_LEGEND, CultureInfo.InvariantCulture)
                                                       ;
                            legendProd.Title = dtpStart.Value.ToString(DateTimeFormats.DATETIME_FORMAT_LEGEND) + " ▬ " + dtpEnd.Value.ToString(DateTimeFormats.DATETIME_FORMAT_LEGEND);
                            SetProductionData();
                            SetProductionSeries(panelProd.Enabled);
                            currentTimer.Resume();
                        }));
                        bInProcess = false;
                    }
                }
            });
        }

        private void cmbRefreshTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            timer1.Interval = int.Parse(cmbRefreshTime.SelectedItem.ToString()) * 60000;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            RefreshAllData();
        }



        private void chart2_MouseMove(object sender, MouseEventArgs e)
        {
            chart2.ChartAreas[0].CursorX.SetCursorPixelPosition(new Point(e.X, e.Y), true);

            if (seriesCount.Enabled) { seriesY = seriesCount; seriesY2 = seriesCumPercent; }
            else if (rdbFT.Checked) { seriesY = seriesFTPass; seriesY2 = seriesFTFail; }
            else if (rdbFTExt.Checked) { seriesY = seriesFTExtPass; seriesY2 = seriesFTExtFail; }
            else if (rdbCal.Checked) { seriesY = seriesCalPass; seriesY2 = seriesCalFail; }
            else if (rdbSer.Checked) { seriesY = seriesSerPass; seriesY2 = seriesSerFail; }
            else if (rdbSeal.Checked) { seriesY = seriesSealPass; seriesY2 = seriesSealFail; }

            int XIndex = (int)chart2.ChartAreas[0].CursorX.Position - 1;
            var result  = chart2.HitTest(e.X, e.Y);

            if (XIndex > -1 && XIndex < seriesY.Points.Count)
            {
                seriesY.Points[XIndex].XValue.ToString();

                var thisPt = new PointF((float)seriesY.Points[XIndex].XValue,
                                    (float)seriesY.Points[XIndex].YValues[0]);

                var ta = new CalloutAnnotation();
                ta.AnchorDataPoint = seriesY.Points[XIndex];
                double min = chart2.ChartAreas[0].AxisX.ScaleView.ViewMinimum;
                double max = chart2.ChartAreas[0].AxisX.ScaleView.ViewMaximum;
                //ta.X = (XIndex <= chart2.ChartAreas[0].AxisX.ScaleView. / 2) ? XIndex + seriesCount.Points.Count / 10 : XIndex - seriesCount.Points.Count / 10;
                //ta.Y = thisPt.Y + 1;
                ta.X = XIndex <= (min + max) / 2 ? XIndex + (max - min) / 10 : XIndex - (max - min) / 4;
                ta.Y = (seriesY.Points.FindMaxByValue().YValues[0] + seriesY.Points.FindMinByValue().YValues[0]) / 2;
                if (!seriesCount.Enabled)
                {
                    ta.Text = string.Format("[{0}]\nPass: {1}\nFail: {2}\nTotal: {3}"
                                               ,seriesY.Points[XIndex].AxisLabel
                                               ,seriesY.Points[XIndex].YValues[0]
                                               ,seriesY2.Points[XIndex].YValues[0]
                                               ,seriesY.Points[XIndex].YValues[0] + seriesY2.Points[XIndex].YValues[0]
                                               );
                }
                else
                {
                    ta.Text = string.Format("[{3}]\n#{4}\nCount = {0}\nCumulative % = {1:0.00}%\nValue % = {2:0.00}%"
                                               , seriesCount.Points[XIndex].YValues[0]
                                               , seriesCumPercent.Points[XIndex].YValues[0]
                                               , seriesValPercent.Points[XIndex].YValues[0]
                                               , seriesValPercent.Points[XIndex].AxisLabel
                                               , XIndex + 1
                                               );
                }
                ta.ForeColor = Color.Red;
                ta.Font = new Font("Tahoma", 10, FontStyle.Bold);
                ta.IsMultiline = true;
                ta.CalloutStyle = CalloutStyle.RoundedRectangle;
                ta.SmartLabelStyle = new AnnotationSmartLabelStyle()
                {
                    Enabled = true,
                    IsMarkerOverlappingAllowed = false,
                    IsOverlappedHidden = false,
                    MaxMovingDistance = 100,
                    MinMovingDistance = 10,
                    MovingDirection = LabelAlignmentStyles.Bottom
                                        | LabelAlignmentStyles.Center
                                        | LabelAlignmentStyles.BottomLeft
                                        | LabelAlignmentStyles.BottomRight
                                        | LabelAlignmentStyles.Left
                                        | LabelAlignmentStyles.Right
                                        | LabelAlignmentStyles.Top
                                        | LabelAlignmentStyles.TopLeft
                                        | LabelAlignmentStyles.TopLeft
                                        | LabelAlignmentStyles.TopRight
                };

                if (chart2.Annotations.Count > 0)
                    chart2.Annotations[0] = ta;
                else
                    chart2.Annotations.Add(ta);
                chart2.Invalidate();
            }
        }

        private void chkChartLabel_CheckedChanged(object sender, EventArgs e)
        {
            seriesCount.IsValueShownAsLabel = chkChartLabel.Checked;
            seriesCumPercent.IsValueShownAsLabel = chkChartLabel.Checked;
            seriesFTPass.IsValueShownAsLabel = chkChartLabel.Checked;
            seriesFTFail.IsValueShownAsLabel = chkChartLabel.Checked;
            seriesFTExtPass.IsValueShownAsLabel = chkChartLabel.Checked;
            seriesFTExtFail.IsValueShownAsLabel = chkChartLabel.Checked;
            seriesCalPass.IsValueShownAsLabel = chkChartLabel.Checked;
            seriesCalFail.IsValueShownAsLabel = chkChartLabel.Checked;
            seriesSerPass.IsValueShownAsLabel = chkChartLabel.Checked;
            seriesSerFail.IsValueShownAsLabel = chkChartLabel.Checked;
            seriesSealPass.IsValueShownAsLabel = chkChartLabel.Checked;
            seriesSealFail.IsValueShownAsLabel = chkChartLabel.Checked;
        }

        private void btnShowProdData_Click(object sender, EventArgs e)
        {
            if (btnShowProdData.Text.IndexOf("Production", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                btnShowProdData.Text = "Display Rejection Data";
                SetProdPanelVisible(true);

                SetProductionSeries(true);
            }
            else
            {
                btnShowProdData.Text = "Display Production Line Data";
                SetProdPanelVisible(false);

                SetProductionSeries(false);
            }
        }

        private void SetProductionSeries(bool bProductionIsVisible)
        {
            seriesCount.Enabled = false;
            seriesCumPercent.Enabled = false;
            seriesValPercent.Enabled = false;
            seriesFTPass.Enabled  = false;
            seriesFTFail.Enabled  = false;
            seriesFTExtPass.Enabled = false;
            seriesFTExtFail.Enabled = false;
            seriesCalPass.Enabled  = false;
            seriesCalFail.Enabled  = false;
            seriesSerPass.Enabled  = false;
            seriesSerFail.Enabled  = false;
            seriesSealPass.Enabled  = false;
            seriesSealFail.Enabled = false;

            seriesCount.Enabled = !bProductionIsVisible;
            seriesCumPercent.Enabled = !bProductionIsVisible;
            seriesFTPass.Enabled = rdbFT.Checked && bProductionIsVisible;
            seriesFTFail.Enabled = rdbFT.Checked && bProductionIsVisible;
            seriesFTExtPass.Enabled = rdbFTExt.Checked && bProductionIsVisible;
            seriesFTExtFail.Enabled = rdbFTExt.Checked && bProductionIsVisible;
            seriesCalPass.Enabled = rdbCal.Checked && bProductionIsVisible;
            seriesCalFail.Enabled = rdbCal.Checked && bProductionIsVisible;
            seriesSerPass.Enabled = rdbSer.Checked && bProductionIsVisible;
            seriesSerFail.Enabled = rdbSer.Checked && bProductionIsVisible;
            seriesSealPass.Enabled = rdbSeal.Checked && bProductionIsVisible;
            seriesSealFail.Enabled = rdbSeal.Checked && bProductionIsVisible;

            chart2.ChartAreas[0].AxisY2.MajorGrid.Enabled = !bProductionIsVisible;
            chart2.ChartAreas[0].AxisY.MajorGrid.Enabled = bProductionIsVisible;
            chart2.ChartAreas[0].RecalculateAxesScale();
            chart2.ChartAreas[0].AxisX.ScaleView.ZoomReset(0);
            chart2.Invalidate();

            if (bProductionIsVisible)
            {
                int idx = rdbAUTOFRS.Checked ? 0 : 1;
                if (rdbFT.Checked && bProductionIsVisible) dgvData.DataSource = dataTableGridSourceFT[idx];
                else if (rdbFTExt.Checked && bProductionIsVisible) dgvData.DataSource = dataTableGridSourceFTExt[idx];
                else if (rdbCal.Checked && bProductionIsVisible) dgvData.DataSource = dataTableGridSourceCal[idx];
                else if (rdbSer.Checked && bProductionIsVisible) dgvData.DataSource = dataTableGridSourceSerial[idx];
                else if (rdbSeal.Checked && bProductionIsVisible) dgvData.DataSource = dataTableGridSourceSeal[idx];
                else dgvData.DataSource = null;

            }
            else
            {
                dgvData.DataSource = datatableGridSourceReject;
            }
            if (dgvData.DataSource != null)
            {
                dgvData.Rows[2].Height = 50;
                dgvData.Rows[1].DefaultCellStyle.BackColor = Color.PaleGreen;
                dgvData.Columns[0].DefaultCellStyle.BackColor = Color.PaleGreen;
                dgvData.Columns[0].Frozen = true;
            }
        }

        private void rdbFT_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdb = sender as RadioButton;
            SetProductionSeries(true);
        }

        private void rdbAUTOFRS_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbFTExt.Checked || rdbSeal.Checked)
            {
                rdbFT.Checked = true;
            }
            else
            {
                SetProductionSeries(true);
            }
            rdbFTExt.Enabled = rdbAUTOFRS.Checked;
            rdbSeal.Enabled = rdbAUTOFRS.Checked;
            SetProductionData();
        }

        private void timerReject_Tick(object sender, EventArgs e)
        {
            if (this.IsHandleCreated)
            {
                Invoke(new Action(() =>
                    {
                        if (btnShowProdData.Text.IndexOf("Production", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            btnShowProdData.PerformClick();
                        }
                        timerReject.Stop();
                        timerProd.Start();
                    }));
            }
        }

        private void timerProd_Tick(object sender, EventArgs e)
        {
            Invoke(new Action(() =>
                {
                    if (btnShowProdData.Text.IndexOf("Rejection", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        btnShowProdData.PerformClick();
                    }
                    timerProd.Stop();
                    timerReject.Start();
                }));
        }

        private void cmbShowProdMin_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox cmb = sender as ComboBox;

            timerReject.Stop();
            timerProd.Stop();

            timerReject.Interval = int.Parse(cmbShowRejectMin.SelectedItem.ToString()) * 1000 * 60;
            timerProd.Interval = int.Parse(cmbShowProdMin.SelectedItem.ToString()) * 1000 * 60;

            if (timerReject.Interval > timerProd.Interval) timerProd.Start();
            else timerReject.Start();
        }
    }
}
