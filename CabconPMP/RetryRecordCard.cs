using System;
using System.Drawing;
using System.Windows.Forms;

namespace CabconPMP
{
    /// <summary>
    /// A designer-friendly card control that displays a single bench retry record.
    /// Drop this control on any WinForms surface and set its properties at design time
    /// or call <see cref="BindRecord"/> at runtime to populate it from a data object.
    /// </summary>
    public partial class RetryRecordCard : UserControl
    {
        // ─── Appearance constants (override in designer via properties) ───────────

        private static readonly Color PassPillBack    = Color.FromArgb(220, 252, 231);
        private static readonly Color PassPillFore    = Color.FromArgb( 21, 128,  61);
        private static readonly Color FailPillBack    = Color.FromArgb(254, 226, 226);
        private static readonly Color FailPillFore    = Color.FromArgb(185,  28,  28);
        private static readonly Color PositionFore    = Color.FromArgb( 30,  64, 175);
        private static readonly Color PcbaFore        = Color.FromArgb( 17,  24,  39);
        private static readonly Color SubtextFore     = Color.FromArgb(100, 100, 100);
        private static readonly Color SelectedBack    = Color.FromArgb(224, 242, 254);
        private static readonly Color DefaultBack     = Color.White;

        // ─── Events ───────────────────────────────────────────────────────────────

        /// <summary>Raised when the user clicks the Retry button.</summary>
        public event EventHandler<RetryCardEventArgs> RetryClicked;

        /// <summary>Raised when the user clicks the See Details button.</summary>
        public event EventHandler<RetryCardEventArgs> DetailsClicked;

        // ─── Designer-visible properties ─────────────────────────────────────────

        private int    _position;
        private string _pcbaId          = "1234567";
        private string _calibrationResult = "Pass";
        private int    _threadId        = 2001;
        private string _detailText      = "No details available.";
        private int    _retryCount      = 0;
        private bool   _isSelected      = false;

        /// <summary>Board position number shown in the card header.</summary>
        public int Position
        {
            get => _position;
            set { _position = value; UpdateDisplay(); }
        }

        /// <summary>PCBA identifier string.</summary>
        public string PcbaId
        {
            get => _pcbaId;
            set { _pcbaId = value; UpdateDisplay(); }
        }

        /// <summary>"Pass" or "Fail".  Controls pill colour and Retry button visibility.</summary>
        public string CalibrationResult
        {
            get => _calibrationResult;
            set { _calibrationResult = value; UpdateDisplay(); }
        }

        /// <summary>Thread ID shown in the subtitle.</summary>
        public int ThreadId
        {
            get => _threadId;
            set { _threadId = value; UpdateDisplay(); }
        }

        /// <summary>Full detail text surfaced in the See Details dialog.</summary>
        public string DetailText
        {
            get => _detailText;
            set { _detailText = value; }
        }

        /// <summary>
        /// Number of retries already attempted.
        /// When ≥ 3 the Retry button is disabled.
        /// </summary>
        public int RetryCount
        {
            get => _retryCount;
            set { _retryCount = value; UpdateDisplay(); }
        }

        /// <summary>Highlights the card in the selection colour when true.</summary>
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                BackColor = value ? SelectedBack : DefaultBack;
            }
        }

        // ─── Construction ─────────────────────────────────────────────────────────

        public RetryRecordCard()
        {
            InitializeComponent();
            WireClickThrough(this);
            UpdateDisplay();
        }

        // ─── Public helpers ───────────────────────────────────────────────────────

        /// <summary>
        /// Convenience method: populate all properties from a plain data bag in one call.
        /// Pass <c>retryCount</c> to carry the existing retry state across re-renders.
        /// </summary>
        public void BindRecord(
            int    position,
            string pcbaId,
            string calibrationResult,
            int    threadId,
            string detailText,
            int    retryCount = 0)
        {
            _position          = position;
            _pcbaId            = pcbaId;
            _calibrationResult = calibrationResult;
            _threadId          = threadId;
            _detailText        = detailText;
            _retryCount        = retryCount;

            UpdateDisplay();
        }

        // ─── Private helpers ──────────────────────────────────────────────────────

        private bool IsPass =>
            string.Equals(_calibrationResult, "Pass", StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Refreshes every child control from the current property values.
        /// Called automatically when any property setter runs.
        /// </summary>
        private void UpdateDisplay()
        {
            // Position label
            lblPosition.Text = $"Position {_position}";

            // PCBA label
            lblPcbaId.Text = string.IsNullOrWhiteSpace(_pcbaId) ? "<n/a>" : _pcbaId;

            // Port / Thread sub-labels
            lblPort.Text   = $"Port: {_pcbaId}";
            lblThread.Text = $"Thread: {_threadId}";

            // Status pill
            pnlStatusPill.BackColor = IsPass ? PassPillBack : FailPillBack;
            lblCalibration.ForeColor = IsPass ? PassPillFore : FailPillFore;
            lblCalibration.Text      = $"Status: {_calibrationResult}";

            // Retry button  — only visible for failed records
            btnRetry.Visible = !IsPass;
            if (!IsPass)
            {
                bool canRetry     = _retryCount < 3;
                btnRetry.Enabled  = canRetry;
                btnRetry.Text     = canRetry ? "Retry" : "Retry (max)";
            }
        }

        /// <summary>
        /// Makes every child forward Click events to the card's own Click event
        /// so consumers can attach a single handler on the card itself.
        /// </summary>
        private void WireClickThrough(Control root)
        {
            foreach (Control c in root.Controls)
            {
                // Buttons handle their own clicks — don't steal them
                if (c is Button) { WireClickThrough(c); continue; }

                c.Cursor = Cursors.Hand;
                c.Click += (s, e) => OnClick(e);
                WireClickThrough(c);
            }
        }

        // ─── Button handlers ──────────────────────────────────────────────────────

        private void btnDetails_Click(object sender, EventArgs e)
        {
            DetailsClicked?.Invoke(this, new RetryCardEventArgs(_position, _pcbaId, _detailText));

            // Built-in fallback: show details dialog if nobody handles the event
            if (DetailsClicked == null)
            {
                ShowDetailsDialog();
            }
        }

        private void btnRetry_Click(object sender, EventArgs e)
        {
            if (_retryCount >= 3) return;

            _retryCount++;
            UpdateDisplay();

            RetryClicked?.Invoke(this, new RetryCardEventArgs(_position, _pcbaId, _detailText));
        }

        private void ShowDetailsDialog()
        {
            using (Form dlg = new Form())
            {
                dlg.Text = $"Details — Position {_position}";
                dlg.Size = new Size(600, 400);
                dlg.StartPosition = FormStartPosition.CenterParent;

                var txt = new TextBox
                {
                    Multiline   = true,
                    Dock        = DockStyle.Fill,
                    ReadOnly    = true,
                    ScrollBars  = ScrollBars.Both,
                    Text        = string.IsNullOrWhiteSpace(_detailText)
                                    ? "No details available."
                                    : _detailText
                };

                dlg.Controls.Add(txt);
                dlg.ShowDialog(this);
            }
        }
    }

    // ─── Event args ───────────────────────────────────────────────────────────────

    /// <summary>Data carried by <see cref="RetryRecordCard.RetryClicked"/> and
    /// <see cref="RetryRecordCard.DetailsClicked"/>.</summary>
    public sealed class RetryCardEventArgs : EventArgs
    {
        public int    Position   { get; }
        public string PcbaId     { get; }
        public string DetailText { get; }

        public RetryCardEventArgs(int position, string pcbaId, string detailText)
        {
            Position   = position;
            PcbaId     = pcbaId;
            DetailText = detailText;
        }
    }
}
