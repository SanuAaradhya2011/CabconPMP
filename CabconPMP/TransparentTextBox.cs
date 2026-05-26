using System;
using System.Drawing;
using System.Windows.Forms;

namespace CabconPMP
{
    public class TransparentTextBox : TextBox
    {
        public TransparentTextBox()
        {
            // Enable transparency and standard paint style overrides
            this.SetStyle(ControlStyles.SupportsTransparentBackColor |
                          ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.ResizeRedraw |
                          ControlStyles.UserPaint, true);

            this.BackColor = Color.Transparent;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x20; // WS_EX_TRANSPARENT
                return cp;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Enable high-quality ClearType text rendering
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            // Draw text aligned with standard WinForms text box bounds
            TextFormatFlags flags = TextFormatFlags.TextBoxControl | TextFormatFlags.VerticalCenter;
            Rectangle textRect = new Rectangle(2, 0, this.Width - 4, this.Height);

            if (this.PasswordChar != '\0')
            {
                string maskedText = new string(this.PasswordChar, this.Text.Length);
                TextRenderer.DrawText(e.Graphics, maskedText, this.Font, textRect, this.ForeColor, flags);
            }
            else
            {
                TextRenderer.DrawText(e.Graphics, this.Text, this.Font, textRect, this.ForeColor, flags);
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            this.Invalidate(); // Force repaint when user types
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            this.Invalidate(); // Force repaint when box is focused
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            this.Invalidate(); // Force repaint when focus is lost
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            this.Invalidate(); // Force repaint on keystrokes
        }
    }
}
