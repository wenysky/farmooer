﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Natsuhime.Common;

namespace Natsuhime.Farmooer
{
    public partial class StatusForm : Form
    {
        public StatusForm()
        {
            InitializeComponent();
        }

        public StatusForm(CurrentStatus status)
            : this()
        {
            this.InitStatusData(status);
        }
        private void StatusForm_Load(object sender, EventArgs e)
        {
        }


        public void InitStatusData(CurrentStatus status)
        {
            this.lblServerTime.Text = Utils.UnixTimestampToDateTime(status.serverTime.time).ToString("yy-MM-dd HH:mm:ss");
            this.lblWeather.Text = Utils.UnicodeCharToChineseChar(status.weather.weatherDesc) + "[" + status.weather.weatherId + "]";

            this.lblUID.Text = status.user.uId;
            this.lblUserName.Text = status.user.userName;
            this.lblMoney.Text = status.user.money.ToString();
            this.lblExp.Text = status.user.exp.ToString();
            this.pbx.ImageLocation = status.user.headPic;

            this.cmbbxFarmList.DataSource = status.farmlandStatus;
            this.cmbbxFarmList.DisplayMember = "a";            
        }

        private void cmbbxFarmList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmbbx = (ComboBox)sender;
            FarmlandStatus fs = (FarmlandStatus)cmbbx.SelectedItem;
            this.lbla.Text = fs.a.ToString();
            this.lblb.Text = fs.b.ToString();
            this.lblc.Text = fs.c.ToString();
            this.lbld.Text = fs.d.ToString();
            this.lble.Text = fs.e.ToString();
            this.lblf.Text = fs.f.ToString();
            this.lblg.Text = fs.g.ToString();
            this.lblh.Text = fs.h.ToString();
            this.lbli.Text = fs.i.ToString();
            this.lblj.Text = fs.j.ToString();
            this.lblk.Text = fs.k.ToString();
            this.lbll.Text = fs.l.ToString();
            this.lblm.Text = fs.m.ToString();
            this.lbln.Text = fs.n.ToString();
            this.lblo.Text = fs.o.ToString();
            this.lblp.Text = fs.p.ToString();
            this.lblq.Text = Utils.UnixTimestampToDateTime(fs.q).ToString("MM-dd HH:mm:ss");
            this.lblr.Text = Utils.UnixTimestampToDateTime(fs.r).ToString("MM-dd HH:mm:ss");
            this.lbls.Text = fs.s.ToString();
            this.lblt.Text = fs.t.ToString();
            this.lblu.Text = fs.u.ToString();
        }
    }
}
