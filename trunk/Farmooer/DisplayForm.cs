using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Farmooer
{
    public partial class DisplayForm : Form
    {
        public DisplayForm(string msg)
        {
            InitializeComponent();
            this.textBox1.Text = msg;
        }
    }
}
