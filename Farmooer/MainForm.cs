using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Natsuhime;
using System.Net;
using Newtonsoft.Json;
using Natsuhime.Farmooer.Entities;

namespace Natsuhime.Farmooer
{
    public partial class MainForm : Form
    {
        NewHttper httper;
        CookieContainer cookie;
        StatusForm sf;
        CurrentStatus cs;
        List<int> harvestList;

        EnumOperation currentOperation;
        public MainForm()
        {
            InitializeComponent();
            cookie = new CookieContainer();

            httper = new NewHttper();
            httper.Cookie = cookie;
            httper.Charset = "UTF-8";
            httper.RequestDataCompleted += new NewHttper.RequestDataCompletedEventHandler(httper_RequestDataCompleted);
            httper.RequestStringCompleted += new NewHttper.RequestStringCompleteEventHandler(httper_RequestStringCompleted);

            sf = new StatusForm();
            harvestList = new List<int>();
            this.currentOperation = EnumOperation.None;
        }
        void ShowMessage(string str)
        {
            textBox2.Text += str + Environment.NewLine;
        }


        void wbMain_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (checkBox1.Checked)
            {
                DisplayForm d1 = new DisplayForm(e.Url + Environment.NewLine + this.wbMain.DocumentText);
                d1.Text = e.Url.ToString();
                d1.Show();


                DisplayForm d2 = new DisplayForm(e.Url + Environment.NewLine + this.wbMain.Document.Cookie);
                d2.Text = e.Url.ToString();
                d2.Show();
            }
            string url = string.Empty;

            if (e.Url.AbsolutePath == "/api.php" && this.currentOperation == EnumOperation.CheckFarmStatus)
            {
                RefeshCurrentStatusCompleted();
            }
            else if (e.Url.AbsolutePath == "/api.php" && this.currentOperation == EnumOperation.Harvest)
            {
                HarvestCompleted();
            }
            else if (e.Url.AbsolutePath == "/home/userapp.php" && this.currentOperation == EnumOperation.InitApp)
            {
                InitAppCompleted();
            }
            else if (this.currentOperation == EnumOperation.PreHarvest)
            {
                Harvest2();
            }
            else
            {
                return;
            }
        }


        void BeginInitApp()
        {
            this.currentOperation = EnumOperation.InitApp;
            ShowMessage("开始引导应用...");
            wbMain.Navigate("http://u.discuz.net/home/userapp.php?id=1021978");
        }
        void InitAppCompleted()
        {
            this.currentOperation = EnumOperation.None;
            ShowMessage("引导应用完成!");
            BeginRefeshCurrentStatus();
        }

        void BeginRefeshCurrentStatus()
        {
            this.currentOperation = EnumOperation.CheckFarmStatus;
            ShowMessage("开始刷新数据...");
            string url = string.Format(
                "http://my.hf.fminutes.com/api.php?mod=user&act=run&farmKey={0}&farmTime={1}&inuId=",
                textBox1.Text,
                UnixStamp()
                );
            wbMain.Navigate(url);
            textBox2.Text += "正在获取状态数据..." + Environment.NewLine;
        }
        private void RefeshCurrentStatusCompleted()
        {
            this.currentOperation = EnumOperation.None;
            ShowMessage("刷新数据完成!");
            cs = GetCurrentStatus(this.wbMain.DocumentText);
            UpdateStatusForm();

            ShowMessage("开始检查收获...");
            for (int i = 0; i < this.cs.farmlandStatus.Length; i++)
            {
                if (cs.farmlandStatus[i].b == 6)
                {
                    this.harvestList.Add(i);
                }
            }
            if (this.harvestList.Count > 0)
            {
                ShowMessage(this.harvestList.Count + "块等待收获!");
                BeginHarvest(this.harvestList[0]);
            }
            else
            {
                ShowMessage("没有需要收获!");
            }
        }

        void BeginHarvest(int placeid)
        {
            this.currentOperation = EnumOperation.PreHarvest;
            string url = string.Format(
                "http://my.hf.fminutes.com/api.php?mod=farmlandstatus&act=harvest&farmKey={0}&farmTime={1}&inuId=",
                textBox1.Text,
                UnixStamp()
                );
            StringBuilder sbHtml = new StringBuilder();
            sbHtml.Append(string.Format("<form action=\"{0}\" method=\"post\">", url));
            sbHtml.Append(string.Format("<input id=\"ownerId\" name=\"ownerId\" value=\"{0}\" type=\"text\" />", cs.user.uId));
            sbHtml.Append(string.Format("<input id=\"place\" name=\"place\" value=\"{0}\" type=\"text\" />", placeid));
            sbHtml.Append("<input id=\"sub\" name=\"sub\" type=\"submit\" />");
            sbHtml.Append("</form>");
            this.wbMain.DocumentText = sbHtml.ToString();

            ShowMessage("准备收获第" + placeid + "块...");
        }
        void Harvest2()
        {
            this.currentOperation = EnumOperation.Harvest;
            HtmlElement htmlbtnSub = this.wbMain.Document.Body.All["sub"];
            htmlbtnSub.InvokeMember("click");
            ShowMessage("正在收获...");
        }
        void HarvestCompleted()
        {
            this.currentOperation = EnumOperation.None;
            HarvestInfo hi = null;
            try
            {
                hi = (HarvestInfo)JavaScriptConvert.DeserializeObject(this.wbMain.DocumentText, typeof(HarvestInfo));
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }
            if (hi != null)
            {
                ShowMessage("[farmlandIndex]" + hi.farmlandIndex + ":[code]" + hi.code + ":[poptype]" + hi.poptype + ":[direction]" + hi.direction + ":[harvest]" + hi.harvest);
                ShowMessage("完毕!");
            }
            else
            {
                ShowMessage("意外完毕!" + this.wbMain.DocumentText);
            }
            this.harvestList.RemoveAt(0);

            if (this.harvestList.Count > 0)
            {
                BeginHarvest(this.harvestList[0]);
            }
            else
            {
                ShowMessage("所有完毕!");
            }
        }



        void UpdateStatusForm()
        {
            sf.InitStatusData(cs);
            ShowMessage("刷新状态窗体完成!");
        }

        private void SetHttperCookieFromWB(string[] wbCookie, string domain)
        {
            foreach (string str in wbCookie)
            {
                string[] nameValue = str.Split('=');
                Cookie ck = new Cookie(nameValue[0].Trim(), nameValue[1].Trim());
                ck.Domain = domain;
                httper.Cookie = new CookieContainer();
                httper.Cookie.Add(ck);
            }
        }
        void httper_RequestStringCompleted(object sender, RequestStringCompletedEventArgs e)
        {

        }
        void httper_RequestDataCompleted(object sender, RequestDataCompletedEventArgs e)
        {
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            BeginInitApp();
            /*
            wbMain.Navigate(
                string.Format(
                "http://apps.manyou.com/1021978/?my_uchId=996169&my_sId=1000174&my_prefix=http://u.discuz.net/home/&my_suffix=/&my_current=http://u.discuz.net/home/userapp.php%3Fid%3D1021978%26my_suffix%3DLw%253D%253D&my_extra=&my_ts=1246092185&my_appVersion=0&my_sig=5d9e3200bfeaf781742a33df4bd2a4f4",
                UnixStamp()
                )
                );
             */
        }
        private void btnShowStatusForm_Click(object sender, EventArgs e)
        {
            this.UpdateStatusForm();
            sf.Location = new Point(this.Location.X, this.Location.Y + this.Height);
            sf.Show();
            sf.Focus();
        }
        private void btnDebug_Click(object sender, EventArgs e)
        {
            InputForm ipt = new InputForm();
            if (ipt.ShowDialog() == DialogResult.OK)
            {
                this.wbMain.Navigate(ipt.InputString);
            }
            //httper.Url = string.Format(
            //    "http://my.hf.fminutes.com/api.php?mod=user&act=run&farmKey={0}&farmTime={1}&inuId=",
            //    textBox1.Text,
            //    UnixStamp()
            //    );
            //httper.RequestStringAsync(EnumRequestMethod.GET);
        }
        private void btnTest_Click(object sender, EventArgs e)
        {
            //Dictionary<string, int> aaa = new Dictionary<string, int>();
            //aaa.Add("cc", 111);
            //aaa.Add("ccc", 11111);
            //string a = JavaScriptConvert.SerializeObject(aaa);

            //a = "[]";
            //aaa = (Dictionary<string, int>)JavaScriptConvert.DeserializeObject(a, typeof(Dictionary<string, int>));
            BeginRefeshCurrentStatus();
        }

        private UInt32 UnixStamp()
        {
            DateTime timeStamp = new DateTime(1970, 1, 1);  //得到1970年的时间戳
            long a = (DateTime.UtcNow.Ticks - timeStamp.Ticks) / 10000000;  //注意这里有时区问题，用now就要减掉8个小时

            TimeSpan ts = DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return Convert.ToUInt32(ts.TotalSeconds);
        }

        static CurrentStatus GetCurrentStatus(string returnMsg)
        {
            CurrentStatus cs;
            string a = returnMsg.Replace("\"1\":", "\"a\":").Replace("\"2\":", "\"b\":").Replace("\"3\":", "\"c\":").Replace("\"4\":", "\"d\":").Replace("\\u", "\\\\u").Replace("\"n\":[]", "\"n\":{}").Replace("\"p\":[]", "\"p\":{}");
            object aa = JavaScriptConvert.DeserializeObject(a, typeof(CurrentStatus));

            cs = aa as CurrentStatus;
            return cs;
        }
    }
}
