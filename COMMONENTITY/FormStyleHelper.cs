using System;
using System.Drawing;
using System.Windows.Forms;

namespace COMMONENTITY
{
    public static class FormStyleHelper
    {
        // Modern 2026 Light Enterprise Palette
        public static readonly Color PrimaryAccent = Color.FromArgb(37, 99, 235);     // Modern Royal Blue #2563EB
        public static readonly Color PrimaryHover = Color.FromArgb(29, 78, 216);      // #1D4ED8
        public static readonly Color PrimaryActive = Color.FromArgb(30, 58, 138);     // #1E3A8A
        
        public static readonly Color SecondaryAccent = Color.FromArgb(243, 244, 246);  // Light Gray #F3F4F6
        public static readonly Color SecondaryHover = Color.FromArgb(229, 231, 235);   // #E5E7EB
        public static readonly Color SecondaryText = Color.FromArgb(75, 85, 99);        // Gray Text #4B5563

        public static readonly Color BgColor = Color.FromArgb(249, 250, 251);          // Off-white background #F9FAFB
        public static readonly Color CardBgColor = Color.White;
        public static readonly Color BorderColor = Color.FromArgb(229, 231, 235);      // Clean borders #E5E7EB
        public static readonly Color TextColor = Color.FromArgb(17, 24, 39);           // Charcoal body text #111827

        // Enterprise Typography
        public static readonly Font HeaderFont = new Font("Segoe UI", 12F, FontStyle.Bold);
        public static readonly Font SubHeaderFont = new Font("Segoe UI", 10F, FontStyle.Bold);
        public static readonly Font ControlFont = new Font("Segoe UI", 9.5F, FontStyle.Regular);
        public static readonly Font ControlBoldFont = new Font("Segoe UI", 9.5F, FontStyle.Bold);

        /// <summary>
        /// Recursively modernizes the visual styling of a Form and all its nested child controls.
        /// </summary>
        public static void Apply(Form form)
        {
            if (form == null) return;

            // Enable double buffering programmatically to prevent any rendering flicker
            try
            {
                var dBufProp = typeof(Form).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                dBufProp?.SetValue(form, true, null);
            }
            catch { }

            form.BackColor = BgColor;
            form.Font = ControlFont;
            form.ForeColor = TextColor;

            ApplyToControls(form.Controls);
        }

        /// <summary>
        /// Recursively modernizes the visual styling of a generic Control, UserControl, or container.
        /// </summary>
        public static void Apply(Control control)
        {
            if (control == null) return;

            if (control is Form frm)
            {
                Apply(frm);
                return;
            }

            control.BackColor = BgColor;
            control.Font = ControlFont;
            control.ForeColor = TextColor;

            ApplyToControl(control);
            ApplyToControls(control.Controls);
        }

        private static void ApplyToControls(Control.ControlCollection controls)
        {
            foreach (Control ctrl in controls)
            {
                ApplyToControl(ctrl);
                if (ctrl.HasChildren && !(ctrl is DataGridView))
                {
                    ApplyToControls(ctrl.Controls);
                }
            }
        }

        public static void ApplyToControl(Control ctrl)
        {
            if (ctrl == null) return;

            // Enforce modern typography except for special containers
            if (!(ctrl is GroupBox) && !(ctrl is TabControl) && !(ctrl is Button))
            {
                ctrl.Font = ControlFont;
            }

            if (ctrl is Button btn)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.Font = ControlBoldFont;
                btn.Cursor = Cursors.Hand;
                btn.FlatAppearance.BorderSize = 1;
                btn.Height = Math.Max(btn.Height, 32);

                // Classify buttons: Primary actions get the premium royal blue accent
                string txtLower = btn.Text.ToLower();
                bool isPrimary = txtLower.Contains("login") || 
                                 txtLower.Contains("save") || 
                                 txtLower.Contains("ok") || 
                                 txtLower.Contains("submit") || 
                                 txtLower.Contains("run") ||
                                 txtLower.Contains("import") ||
                                 txtLower.Contains("export") ||
                                 txtLower.Contains("add") ||
                                 txtLower.Contains("update") ||
                                 txtLower.Contains("sync") ||
                                 btn.BackColor == Color.Blue;

                if (isPrimary)
                {
                    btn.BackColor = PrimaryAccent;
                    btn.ForeColor = Color.White;
                    btn.FlatAppearance.BorderColor = PrimaryAccent;
                    btn.FlatAppearance.MouseOverBackColor = PrimaryHover;
                    btn.FlatAppearance.MouseDownBackColor = PrimaryActive;
                }
                else
                {
                    btn.BackColor = SecondaryAccent;
                    btn.ForeColor = TextColor;
                    btn.FlatAppearance.BorderColor = BorderColor;
                    btn.FlatAppearance.MouseOverBackColor = SecondaryHover;
                    btn.FlatAppearance.MouseDownBackColor = BorderColor;
                }
            }
            else if (ctrl is TextBox txt)
            {
                txt.BorderStyle = BorderStyle.FixedSingle;
                txt.Font = ControlFont;
                txt.BackColor = Color.White;
                txt.ForeColor = TextColor;
            }
            else if (ctrl is MaskedTextBox mxt)
            {
                mxt.BorderStyle = BorderStyle.FixedSingle;
                mxt.Font = ControlFont;
                mxt.BackColor = Color.White;
                mxt.ForeColor = TextColor;
            }
            else if (ctrl is ComboBox cmb)
            {
                cmb.FlatStyle = FlatStyle.Flat;
                cmb.Font = ControlFont;
                cmb.BackColor = Color.White;
                cmb.ForeColor = TextColor;
            }
            else if (ctrl is Label lbl)
            {
                string nameLower = lbl.Name.ToLower();
                if (lbl.Font.Size >= 12 || nameLower.Contains("title") || nameLower.Contains("header"))
                {
                    lbl.Font = HeaderFont;
                    lbl.ForeColor = PrimaryActive;
                }
                else if (lbl.Font.Bold || nameLower.Contains("bold") || nameLower.Contains("subtitle"))
                {
                    lbl.Font = SubHeaderFont;
                    lbl.ForeColor = PrimaryActive;
                }
                else
                {
                    lbl.Font = ControlFont;
                    lbl.ForeColor = TextColor;
                }
            }
            else if (ctrl is GroupBox grp)
            {
                grp.FlatStyle = FlatStyle.Flat;
                grp.Font = SubHeaderFont;
                grp.ForeColor = PrimaryActive;
                grp.BackColor = BgColor;
            }
            else if (ctrl is Panel pnl)
            {
                string pnlName = pnl.Name.ToLower();
                if (pnlName.Contains("card") || pnlName.Contains("container") || pnlName.Contains("logincontrol"))
                {
                    pnl.BackColor = CardBgColor;
                    pnl.BorderStyle = BorderStyle.FixedSingle;
                }
            }
            else if (ctrl is TabControl tab)
            {
                // Normalize tab control visual appearance
                tab.Appearance = TabAppearance.Normal;
                tab.DrawMode = TabDrawMode.OwnerDrawFixed;
                tab.ItemSize = new Size(110, 32);
                tab.SizeMode = TabSizeMode.Fixed;
                
                // Attach safe double-draw protection
                tab.DrawItem -= TabControl_DrawItem;
                tab.DrawItem += TabControl_DrawItem;

                // Set parent form backcolor to BgColor for all tab pages
                foreach (TabPage page in tab.TabPages)
                {
                    page.BackColor = CardBgColor;
                    page.ForeColor = TextColor;
                }
            }
            else if (ctrl is DataGridView dgv)
            {
                dgv.BackgroundColor = CardBgColor;
                dgv.GridColor = BorderColor;
                dgv.BorderStyle = BorderStyle.None;
                dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                dgv.RowHeadersVisible = false;
                dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgv.MultiSelect = false;
                dgv.AllowUserToResizeRows = false;
                dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 250, 251);

                dgv.EnableHeadersVisualStyles = false;

                // Modern column headers styling
                dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(243, 244, 246);
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = SecondaryText;
                dgv.ColumnHeadersDefaultCellStyle.Font = ControlBoldFont;
                dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(243, 244, 246);
                dgv.ColumnHeadersHeight = 32;

                // Modern rows styling
                dgv.DefaultCellStyle.BackColor = Color.White;
                dgv.DefaultCellStyle.ForeColor = TextColor;
                dgv.DefaultCellStyle.Font = ControlFont;
                dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(239, 246, 255);
                dgv.DefaultCellStyle.SelectionForeColor = PrimaryAccent;
                dgv.RowTemplate.Height = 28;
            }
            else if (ctrl is ListBox lst)
            {
                lst.BorderStyle = BorderStyle.FixedSingle;
                lst.Font = ControlFont;
                lst.BackColor = Color.White;
                lst.ForeColor = TextColor;
            }
            else if (ctrl is MenuStrip ms)
            {
                ms.BackColor = CardBgColor;
                ms.ForeColor = TextColor;
                ms.Font = ControlBoldFont;
                ms.Padding = new Padding(6, 4, 0, 4);
                ms.Renderer = new ToolStripProfessionalRenderer(new ModernColorTable());
            }
            else if (ctrl is ToolStrip ts)
            {
                ts.BackColor = CardBgColor;
                ts.ForeColor = TextColor;
                ts.Font = ControlFont;
                ts.Padding = new Padding(6, 6, 6, 6);
                ts.GripStyle = ToolStripGripStyle.Hidden;
                ts.Renderer = new ToolStripProfessionalRenderer(new ModernColorTable());
            }
            else if (ctrl is StatusStrip ss)
            {
                ss.BackColor = SecondaryAccent;
                ss.ForeColor = SecondaryText;
                ss.Font = ControlFont;
                ss.Padding = new Padding(1, 0, 14, 0);
            }
        }

        private static void TabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (!(sender is TabControl tab) || e.Index < 0 || e.Index >= tab.TabCount) return;

            Graphics g = e.Graphics;
            TabPage page = tab.TabPages[e.Index];
            Rectangle rect = tab.GetTabRect(e.Index);

            // Antialiasing for high-DPI text rendering
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            bool isSelected = (tab.SelectedIndex == e.Index);

            if (isSelected)
            {
                // Active tab background
                g.FillRectangle(new SolidBrush(Color.White), rect);

                // Modern 3px bottom active indicator
                using (Pen accentPen = new Pen(PrimaryAccent, 3))
                {
                    g.DrawLine(accentPen, rect.Left + 2, rect.Bottom - 1, rect.Right - 2, rect.Bottom - 1);
                }
            }
            else
            {
                // Inactive tab background
                g.FillRectangle(new SolidBrush(BgColor), rect);
            }

            // Render modern flat tab label
            TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine;
            Font textFont = isSelected ? ControlBoldFont : ControlFont;
            Color textColor = isSelected ? PrimaryAccent : SecondaryText;

            TextRenderer.DrawText(g, page.Text, textFont, rect, textColor, flags);
        }
    }

    /// <summary>
    /// Custom color table to eliminate default visual gradients and render flat controls.
    /// </summary>
    public class ModernColorTable : ProfessionalColorTable
    {
        public override Color ToolStripDropDownBackground => Color.White;
        public override Color ImageMarginGradientBegin => Color.White;
        public override Color ImageMarginGradientMiddle => Color.White;
        public override Color ImageMarginGradientEnd => Color.White;
        public override Color MenuBorder => Color.FromArgb(229, 231, 235);
        public override Color MenuItemBorder => Color.Transparent;
        public override Color MenuItemSelected => Color.FromArgb(239, 246, 255);
        public override Color MenuItemSelectedGradientBegin => Color.FromArgb(239, 246, 255);
        public override Color MenuItemSelectedGradientEnd => Color.FromArgb(239, 246, 255);
        public override Color MenuItemPressedGradientBegin => Color.FromArgb(219, 234, 254);
        public override Color MenuItemPressedGradientEnd => Color.FromArgb(219, 234, 254);
        public override Color ToolStripBorder => Color.FromArgb(229, 231, 235);
        public override Color ToolStripGradientBegin => Color.White;
        public override Color ToolStripGradientMiddle => Color.White;
        public override Color ToolStripGradientEnd => Color.White;
    }
}
