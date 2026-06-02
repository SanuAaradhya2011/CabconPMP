using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ApplicationInterface.Form_Waitwindow
{
    public partial class frmsplash : Form
    {
        public frmsplash()
        {
            InitializeComponent();
        }

        private void frmsplash_Load(object sender, EventArgs e)
        {
            this.Opacity = 0.5;
        }
    }
}
