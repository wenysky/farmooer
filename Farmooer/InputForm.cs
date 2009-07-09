using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Natsuhime.Farmooer
{
    public partial class InputForm : Form
    {
        public string InputString { get; set; }
        public InputForm()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.InputString = tbxInput.Text;
            DialogResult = DialogResult.OK;
        }
    }
}
