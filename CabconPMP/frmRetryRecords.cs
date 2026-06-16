using COMMONENTITY;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace CabconPMP
{
    public partial class frmRetryRecords : Form
    {
        private string _currentOperationName;
        private const string BenchResponseJson = @"[
  { ""position"": 1, ""PCBAID"": ""1234567"", ""calibrationResult"": ""Pass"", ""threadID"": 2001 },
  { ""position"": 2, ""PCBAID"": ""1234568"", ""calibrationResult"": ""Pass"", ""threadID"": 2002 },
  { ""position"": 3, ""PCBAID"": ""1234569"", ""calibrationResult"": ""Pass"", ""threadID"": 2003 },
  { ""position"": 4, ""PCBAID"": ""1234522"", ""calibrationResult"": ""Pass"", ""threadID"": 2004 },
  { ""position"": 5, ""PCBAID"": ""1234511"", ""calibrationResult"": ""Pass"", ""threadID"": 2005 },
  { ""position"": 6, ""PCBAID"": ""1234406"", ""calibrationResult"": ""Pass"", ""threadID"": 2006 },
  { ""position"": 7, ""PCBAID"": ""1234407"", ""calibrationResult"": ""Fail"", ""threadID"": 2007 },
  { ""position"": 8, ""PCBAID"": ""1234408"", ""calibrationResult"": ""Pass"", ""threadID"": 2008 },
  { ""position"": 9, ""PCBAID"": ""1234409"", ""calibrationResult"": ""Pass"", ""threadID"": 2009 },
  { ""position"": 10, ""PCBAID"": ""1234410"", ""calibrationResult"": ""Pass"", ""threadID"": 2010 },
  { ""position"": 11, ""PCBAID"": ""1234411"", ""calibrationResult"": ""Pass"", ""threadID"": 2011 },
  { ""position"": 12, ""PCBAID"": ""1234412"", ""calibrationResult"": ""Pass"", ""threadID"": 2012 },
  { ""position"": 13, ""PCBAID"": ""1234413"", ""calibrationResult"": ""Pass"", ""threadID"": 2013 },
  { ""position"": 14, ""PCBAID"": ""1234414"", ""calibrationResult"": ""Fail"", ""threadID"": 2014 },
  { ""position"": 15, ""PCBAID"": ""1234415"", ""calibrationResult"": ""Pass"", ""threadID"": 2015 },
  { ""position"": 16, ""PCBAID"": ""1234416"", ""calibrationResult"": ""Pass"", ""threadID"": 2016 },
  { ""position"": 17, ""PCBAID"": ""1234417"", ""calibrationResult"": ""Pass"", ""threadID"": 2017 },
  { ""position"": 18, ""PCBAID"": ""1234418"", ""calibrationResult"": ""Pass"", ""threadID"": 2018 },
  { ""position"": 19, ""PCBAID"": ""1234419"", ""calibrationResult"": ""Pass"", ""threadID"": 2019 },
  { ""position"": 20, ""PCBAID"": ""1234420"", ""calibrationResult"": ""Pass"", ""threadID"": 2020 },
  { ""position"": 21, ""PCBAID"": ""1234421"", ""calibrationResult"": ""Fail"", ""threadID"": 2021 },
  { ""position"": 22, ""PCBAID"": ""1234422"", ""calibrationResult"": ""Pass"", ""threadID"": 2022 },
  { ""position"": 23, ""PCBAID"": ""1234423"", ""calibrationResult"": ""Pass"", ""threadID"": 2023 },
  { ""position"": 24, ""PCBAID"": ""1234424"", ""calibrationResult"": ""Pass"", ""threadID"": 2024 },
  { ""position"": 25, ""PCBAID"": ""1234425"", ""calibrationResult"": ""Pass"", ""threadID"": 2025 },
  { ""position"": 26, ""PCBAID"": ""1234426"", ""calibrationResult"": ""Pass"", ""threadID"": 2026 },
  { ""position"": 27, ""PCBAID"": ""1234427"", ""calibrationResult"": ""Pass"", ""threadID"": 2027 },
  { ""position"": 28, ""PCBAID"": ""1234428"", ""calibrationResult"": ""Fail"", ""threadID"": 2028 },
  { ""position"": 29, ""PCBAID"": ""1234429"", ""calibrationResult"": ""Pass"", ""threadID"": 2029 },
  { ""position"": 30, ""PCBAID"": ""1234430"", ""calibrationResult"": ""Pass"", ""threadID"": 2030 },
  { ""position"": 31, ""PCBAID"": ""1234431"", ""calibrationResult"": ""Pass"", ""threadID"": 2031 },
  { ""position"": 32, ""PCBAID"": ""1234432"", ""calibrationResult"": ""Pass"", ""threadID"": 2032 },
  { ""position"": 33, ""PCBAID"": ""1234433"", ""calibrationResult"": ""Pass"", ""threadID"": 2033 },
  { ""position"": 34, ""PCBAID"": ""1234434"", ""calibrationResult"": ""Pass"", ""threadID"": 2034 },
  { ""position"": 35, ""PCBAID"": ""1234435"", ""calibrationResult"": ""Fail"", ""threadID"": 2035 },
  { ""position"": 36, ""PCBAID"": ""1234436"", ""calibrationResult"": ""Pass"", ""threadID"": 2036 },
  { ""position"": 37, ""PCBAID"": ""1234437"", ""calibrationResult"": ""Pass"", ""threadID"": 2037 },
  { ""position"": 38, ""PCBAID"": ""1234438"", ""calibrationResult"": ""Pass"", ""threadID"": 2038 },
  { ""position"": 39, ""PCBAID"": ""1234439"", ""calibrationResult"": ""Pass"", ""threadID"": 2039 },
  { ""position"": 40, ""PCBAID"": ""1234440"", ""calibrationResult"": ""Pass"", ""threadID"": 2040 },
  { ""position"": 41, ""PCBAID"": ""1234441"", ""calibrationResult"": ""Pass"", ""threadID"": 2041 },
  { ""position"": 42, ""PCBAID"": ""1234442"", ""calibrationResult"": ""Fail"", ""threadID"": 2042 },
  { ""position"": 43, ""PCBAID"": ""1234443"", ""calibrationResult"": ""Pass"", ""threadID"": 2043 },
  { ""position"": 44, ""PCBAID"": ""1234444"", ""calibrationResult"": ""Pass"", ""threadID"": 2044 },
  { ""position"": 45, ""PCBAID"": ""1234445"", ""calibrationResult"": ""Pass"", ""threadID"": 2045 },
  { ""position"": 46, ""PCBAID"": ""1234446"", ""calibrationResult"": ""Pass"", ""threadID"": 2046 },
  { ""position"": 47, ""PCBAID"": ""1234447"", ""calibrationResult"": ""Pass"", ""threadID"": 2047 },
  { ""position"": 48, ""PCBAID"": ""1234448"", ""calibrationResult"": ""Pass"", ""threadID"": 2048 }
]";

        private readonly List<RetryBenchRecord> _records = new List<RetryBenchRecord>();

        private class RetryBenchRecord
        {
            public int Position { get; set; }
            public string PCBAID { get; set; }
            public string CalibrationResult { get; set; }
            public int ThreadID { get; set; }
            public string DetailRecord { get; set; }
            public int RetryCount { get; set; }
            public string FailureReason { get; set; }

            public string GetDetailText()
            {
                return string.IsNullOrWhiteSpace(DetailRecord) ? "No details available." : DetailRecord;
            }
        }

        public frmRetryRecords()
        {
            InitializeComponent();
            FormStyleHelper.Apply(this);
            LoadBenchRecords();
        }

        public void SetOperationName(string name)
        {
            _currentOperationName = string.IsNullOrWhiteSpace(name) ? "<none>" : name;
            if (lblOperationName != null)
            {
                lblOperationName.Text = string.Format(CultureInfo.InvariantCulture, "Method: {0}", _currentOperationName);
            }
        }

        public void ClearCards()
        {
            _records.Clear();
            flpCards.Controls.Clear();
            lblSummary.Text = "No cards loaded.";
            ClearDetailView();
        }

        public void BindResults(IEnumerable<RetryPortExecutionResult> results)
        {
            if (IsDisposed)
            {
                return;
            }

            if (InvokeRequired)
            {
                BeginInvoke(new Action<IEnumerable<RetryPortExecutionResult>>(BindResults), results);
                return;
            }

            if (results == null)
            {
                LoadBenchRecords();
                return;
            }

            _records.Clear();
            _records.AddRange(results.Select((result, index) => new RetryBenchRecord
            {
                Position = index + 1,
                PCBAID = string.IsNullOrWhiteSpace(result.PortName)
                    ? string.Format(System.Globalization.CultureInfo.InvariantCulture, "Port {0}", index + 1)
                    : result.PortName,
                CalibrationResult = result.CompletedSuccessfully ? "Pass" : "Fail",
                ThreadID = result.ManagedThreadId,
                DetailRecord = BuildRuntimeDetail(result)
            }));

            RenderCards();
        }

        private void LoadBenchRecords()
        {
            _records.Clear();

            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                object parsed = serializer.DeserializeObject(BenchResponseJson);
                IEnumerable records = parsed as IEnumerable;
                if (records != null)
                {
                    foreach (object item in records)
                    {
                        IDictionary dictionary = item as IDictionary;
                        if (dictionary == null)
                        {
                            continue;
                        }

                        _records.Add(new RetryBenchRecord
                        {
                            Position = ReadInt(dictionary, "position", "Position"),
                            PCBAID = ReadString(dictionary, "PCBAID", "pcbaid"),
                            CalibrationResult = ReadString(dictionary, "calibrationResult", "CalibrationResult"),
                            ThreadID = ReadInt(dictionary, "threadID", "ThreadID"),
                            DetailRecord = "Bench response card loaded from hardcoded JSON."
                        });
                    }
                }

                _records.Sort((left, right) => left.Position.CompareTo(right.Position));
            }
            catch
            {
                _records.AddRange(CreateFallbackBenchRecords());
            }

            RenderCards();
        }

        private List<RetryBenchRecord> CreateFallbackBenchRecords()
        {
            List<RetryBenchRecord> records = new List<RetryBenchRecord>();
            string[] firstFive = new[] { "1234567", "1234568", "1234569", "1234522", "1234511" };

            for (int i = 1; i <= 48; i++)
            {
                string pcbaid = i <= firstFive.Length
                    ? firstFive[i - 1]
                    : string.Format(System.Globalization.CultureInfo.InvariantCulture, "12344{0:00}", i);

                records.Add(new RetryBenchRecord
                {
                    Position = i,
                    PCBAID = pcbaid,
                    CalibrationResult = i % 7 == 0 ? "Fail" : "Pass",
                    ThreadID = 2000 + i,
                    DetailRecord = "Fallback bench record."
                });
            }

            return records;
        }

        private static string ReadString(IDictionary dictionary, params string[] keys)
        {
            foreach (string key in keys)
            {
                if (dictionary.Contains(key))
                {
                    object value = dictionary[key];
                    if (value != null)
                    {
                        return Convert.ToString(value, CultureInfo.InvariantCulture);
                    }
                }
            }

            return string.Empty;
        }

        private static int ReadInt(IDictionary dictionary, params string[] keys)
        {
            foreach (string key in keys)
            {
                if (dictionary.Contains(key))
                {
                    object value = dictionary[key];
                    if (value != null)
                    {
                        if (value is int)
                        {
                            return (int)value;
                        }

                        int parsedValue;
                        if (int.TryParse(Convert.ToString(value, CultureInfo.InvariantCulture), NumberStyles.Integer, CultureInfo.InvariantCulture, out parsedValue))
                        {
                            return parsedValue;
                        }
                    }
                }
            }

            return 0;
        }

        private void RenderCards()
        {
            flpCards.SuspendLayout();
            try
            {
                flpCards.Controls.Clear();

                foreach (RetryBenchRecord record in _records.OrderBy(item => item.Position))
                {
                    flpCards.Controls.Add(CreateCard(record));
                }
            }
            finally
            {
                flpCards.ResumeLayout(true);
            }

            lblSummary.Text = string.Format(
                System.Globalization.CultureInfo.InvariantCulture,
                "Loaded {0} card(s). Click a card to inspect the thread record.",
                _records.Count);

            if (_records.Count > 0)
            {
                SelectRecord(_records[0]);
            }
            else
            {
                ClearDetailView();
            }
        }

        private RetryRecordCard CreateCard(RetryBenchRecord record)
        {
            // Template-based cloning: create a fresh instance and copy
            // designer-set appearance properties from the hidden templateCard.
            RetryRecordCard card = null;

            if (this.templateCard != null)
            {
                card = (RetryRecordCard)Activator.CreateInstance(this.templateCard.GetType());

                // copy common appearance/layout properties so cloned card matches the template
                card.Size = this.templateCard.Size;
                card.Padding = this.templateCard.Padding;
                card.Margin = this.templateCard.Margin;
                card.BackColor = this.templateCard.BackColor;
                card.BorderStyle = this.templateCard.BorderStyle;
                card.Cursor = this.templateCard.Cursor;
                card.Font = this.templateCard.Font;
            }
            else
            {
                // fallback
                card = new RetryRecordCard();
            }

            card.Tag = record;

            card.BindRecord(
                position: record.Position,
                pcbaId: record.PCBAID ?? string.Empty,
                calibrationResult: record.CalibrationResult,
                threadId: record.ThreadID,
                detailText: record.GetDetailText(),
                retryCount: record.RetryCount);

            // clicking the card selects it
            card.Click += (s, e) => SelectRecord(record);

            // Retry button -> propagate to host and update retry count on model
            card.RetryClicked += (s, e) =>
            {
                record.RetryCount = card.RetryCount;
                OnRetryRequested(record);
            };

            // Details button -> show detail dialog
            card.DetailsClicked += (s, e) => ShowDetails(record);

            return card;
        }

        private void AttachCardClick(Control control, EventHandler handler)
        {
            control.Click += handler;

            foreach (Control child in control.Controls)
            {
                AttachCardClick(child, handler);
            }
        }

        private void Card_Click(object sender, EventArgs e)
        {
            Control control = sender as Control;
            while (control != null && !(control is RetryRecordCard && control.Tag is RetryBenchRecord))
            {
                control = control.Parent;
            }

            RetryRecordCard selectedCard = control as RetryRecordCard;
            RetryBenchRecord record = selectedCard != null ? selectedCard.Tag as RetryBenchRecord : null;
            if (record == null)
            {
                return;
            }

            SelectRecord(record);
        }

        // Event invoked when user requests a retry for a specific record
        // Parameters: position, current operation name (method)
        public event Action<int, string> RetryRequested;

        private void OnRetryRequested(RetryBenchRecord record)
        {
            RetryRequested?.Invoke(record.Position, _currentOperationName);
            // increment local retry count for UI feedback
            record.RetryCount++;
            RenderCards();
        }

        private void ShowDetails(RetryBenchRecord record)
        {
            // open a simple dialog showing details
            using (Form dlg = new Form())
            {
                dlg.Text = string.Format(CultureInfo.InvariantCulture, "Details - Position {0}", record.Position);
                dlg.Size = new Size(600, 400);
                TextBox txt = new TextBox { Multiline = true, Dock = DockStyle.Fill, ReadOnly = true, ScrollBars = ScrollBars.Both, Text = record.GetDetailText() };
                dlg.Controls.Add(txt);
                dlg.ShowDialog(this);
            }
        }

        private void SelectRecord(RetryBenchRecord record)
        {
            foreach (RetryRecordCard card in flpCards.Controls.OfType<RetryRecordCard>())
            {
                card.IsSelected = false;
            }

            RetryRecordCard selected = flpCards.Controls
                .OfType<RetryRecordCard>()
                .FirstOrDefault(c => ReferenceEquals(c.Tag, record));

            if (selected != null)
                selected.IsSelected = true;

            lblSelectedPositionValue.Text = record.Position.ToString(System.Globalization.CultureInfo.InvariantCulture);
            lblSelectedPcbaValue.Text = string.IsNullOrWhiteSpace(record.PCBAID) ? "<n/a>" : record.PCBAID;
            lblSelectedCalibrationValue.Text = string.IsNullOrWhiteSpace(record.CalibrationResult) ? "<n/a>" : record.CalibrationResult;
            lblSelectedThreadValue.Text = record.ThreadID.ToString(System.Globalization.CultureInfo.InvariantCulture);
            txtDetail.Text = record.GetDetailText();
        }

        private void ClearDetailView()
        {
            lblSelectedPositionValue.Text = "-";
            lblSelectedPcbaValue.Text = "-";
            lblSelectedCalibrationValue.Text = "-";
            lblSelectedThreadValue.Text = "-";
            txtDetail.Clear();
        }

        private string BuildRuntimeDetail(RetryPortExecutionResult result)
        {
            if (result == null)
            {
                return string.Empty;
            }
            string activities = result.Activities == null || result.Activities.Count == 0
                ? "No activities captured."
                : string.Join(Environment.NewLine, result.Activities);

            // Append per-method results (important for calibration failures)
            string methodDetails = string.Empty;
            if (result.MethodResults != null && result.MethodResults.Count > 0)
            {
                var lines = new List<string>();
                lines.Add("\r\nMethod Results:");
                foreach (var m in result.MethodResults)
                {
                    lines.Add(string.Format(CultureInfo.InvariantCulture, "- Method: {0}", string.IsNullOrWhiteSpace(m.MethodName) ? "<unknown>" : m.MethodName));
                    lines.Add(string.Format(CultureInfo.InvariantCulture, "  Status: {0}", string.IsNullOrWhiteSpace(m.Status) ? "<n/a>" : m.Status));
                    if (!string.IsNullOrWhiteSpace(m.ErrorMessage)) lines.Add(string.Format(CultureInfo.InvariantCulture, "  Error: {0}", m.ErrorMessage));
                    if (!string.IsNullOrWhiteSpace(m.ReturnValue)) lines.Add(string.Format(CultureInfo.InvariantCulture, "  Return: {0}", m.ReturnValue));
                    if (m.InputArguments != null && m.InputArguments.Count > 0)
                    {
                        lines.Add(string.Format(CultureInfo.InvariantCulture, "  Inputs: {0}", string.Join(", ", m.InputArguments)));
                    }
                    if (m.Activities != null && m.Activities.Count > 0)
                    {
                        lines.Add("  Activities:");
                        foreach (var a in m.Activities) lines.Add(string.Format(CultureInfo.InvariantCulture, "    {0}", a));
                    }
                }

                methodDetails = string.Join(Environment.NewLine, lines);
            }

            return string.Format(
                System.Globalization.CultureInfo.InvariantCulture,
                "Port: {0}\r\nStarted On: {1:yyyy-MM-dd HH:mm:ss}\r\nCompleted On: {2:yyyy-MM-dd HH:mm:ss}\r\nStatus: {3}\r\n\r\nActivities:\r\n{4}{5}",
                string.IsNullOrWhiteSpace(result.PortName) ? "<n/a>" : result.PortName,
                result.StartedOn,
                result.CompletedOn,
                string.IsNullOrWhiteSpace(result.Status) ? "<n/a>" : result.Status,
                activities,
                methodDetails);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadBenchRecords();
        }

        /// <summary>
        /// Remove any rendered cards that match the provided port name and re-render the UI.
        /// Port name comparison is case-insensitive and will match either exact PCBAID entries
        /// or the generated "Port N" labels used when PortName is empty.
        /// </summary>
        public void ClearCardsForPort(string portName)
        {
            if (string.IsNullOrWhiteSpace(portName)) return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(ClearCardsForPort), portName);
                return;
            }

            string normalized = portName.Trim();
            _records.RemoveAll(r =>
                string.Equals(r.PCBAID, normalized, StringComparison.OrdinalIgnoreCase)
                || string.Equals(r.PCBAID, string.Format(CultureInfo.InvariantCulture, "Port {0}", normalized), StringComparison.OrdinalIgnoreCase)
                || (r.PCBAID ?? string.Empty).IndexOf(normalized, StringComparison.OrdinalIgnoreCase) >= 0);

            RenderCards();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Hide();
        }
    }
}
