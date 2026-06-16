using ApplicationInterface;
using COMMONENTITY;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CabconPMP
{
    public partial class frmRetryLiveTracking : Form
    {
        public frmRetryLiveTracking()
        {
            InitializeComponent();
            COMMONENTITY.FormStyleHelper.Apply(this);
        }

        public void ResetLog()
        {
            if (IsDisposed)
            {
                return;
            }

            if (InvokeRequired)
            {
                BeginInvoke(new Action(ResetLog));
                return;
            }

            lvStatus.Items.Clear();
            lblStatus.Text = "Waiting for retry activity...";
        }

        public void AppendStatus(string message, bool isError)
        {
            if (IsDisposed)
            {
                return;
            }

            if (InvokeRequired)
            {
                BeginInvoke(new Action<string, bool>(AppendStatus), message, isError);
                return;
            }

            string safeMessage = string.IsNullOrWhiteSpace(message) ? string.Empty : message.Trim();
            string stamp = DateTime.Now.ToString("HH:mm:ss");
            ListViewItem item = new ListViewItem(stamp);
            item.SubItems.Add(safeMessage);
            item.SubItems.Add(isError ? "Error" : "Info");
            item.ForeColor = isError ? Color.FromArgb(185, 28, 28) : Color.FromArgb(22, 101, 52);
            lvStatus.Items.Add(item);
            if (lvStatus.Items.Count > 0)
            {
                lvStatus.EnsureVisible(lvStatus.Items.Count - 1);
            }

            lblStatus.Text = safeMessage;
            lblStatus.ForeColor = isError ? Color.FromArgb(185, 28, 28) : Color.FromArgb(22, 101, 52);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ResetLog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
