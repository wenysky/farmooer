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

namespace Natsuhime.Farmooer
{
    public partial class MainForm : Form
    {
        NewHttper httper;
        CookieContainer cookie;
        StatusForm sf;
        CurrentStatus cs;
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
            sf.Show();
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

            if (e.Url.AbsolutePath == "/api.php")
            {
                RefeshCurrentStatusCompleted();
                return; 

                //url = string.Format(
                //    "http://my.hf.fminutes.com/api.php?mod=user&act=run&farmKey={0}&farmTime={1}&inuId=",
                //    textBox1.Text,
                //    UnixStamp()
                //    );
                //httper.Url = url;
                //SetHttperCookieFromWB(wbMain.Document.Cookie.Split(';'), wbMain.Document.Domain);
                //httper.RequestStringAsync(EnumRequestMethod.GET);
            }
            else if (e.Url.AbsolutePath == "")
            {
            }
            else if (e.Url.AbsolutePath == "/home/userapp.php")
            {
                BeginRefeshCurrentStatus();
                return;
            }
            else
            {
                return;
            }
            if (url != string.Empty)
            {
                wbMain.Navigate(url);
            }
        }

        private void RefeshCurrentStatusCompleted()
        {
            cs = GetCurrentStatus(this.wbMain.DocumentText);
            UpdateStatusForm();
            textBox2.Text += "获取状态数据完成" + Environment.NewLine;
        }

        void BeginRefeshCurrentStatus()
        {
            string url = string.Format(
                "http://my.hf.fminutes.com/api.php?mod=user&act=run&farmKey={0}&farmTime={1}&inuId=",
                textBox1.Text,
                UnixStamp()
                );
            wbMain.Navigate(url);
            textBox2.Text += "正在获取状态数据..."+Environment.NewLine;
        }

        void UpdateStatusForm()
        {
            sf.InitStatusData(cs);
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
            wbMain.Navigate("http://u.discuz.net/home/userapp.php?id=1021978");
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
            sf.Show();
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
            string a = returnMsg.Replace("\"1\":", "\"a\":").Replace("\"2\":", "\"b\":").Replace("\"3\":", "\"c\":").Replace("\"4\":", "\"d\":").Replace("\\u", "\\\\u");
            object aa = JavaScriptConvert.DeserializeObject(a, typeof(CurrentStatus));

            cs = aa as CurrentStatus;
            return cs;
        }
    }

    public class CurrentStatus
    {
        public FarmlandStatus[] farmlandStatus { get; set; }
        public FarmItems items { get; set; }
        public int exp { get; set; }
        public Weather weather { get; set; }
        public ServerTime serverTime { get; set; }
        public User user { get; set; }
        public int a { get; set; }
        public int b { get; set; }
        public int c { get; set; }
    }

    public class FarmlandStatus
    {
        public int a { get; set; }
        /// <summary>
        /// 成长阶段(6:已成熟;7:已收获;0:空地)
        /// </summary>
        public int b { get; set; }
        public int c { get; set; }
        public int d { get; set; }
        public int e { get; set; }
        public int f { get; set; }
        public int g { get; set; }
        public int h { get; set; }
        public int i { get; set; }
        public int j { get; set; }
        /// <summary>
        /// 产量
        /// </summary>
        public int k { get; set; }
        /// <summary>
        /// 健康度
        /// </summary>
        public int l { get; set; }
        /// <summary>
        /// 产量
        /// </summary>
        public int m { get; set; }
        public int[] n { get; set; }
        public int o { get; set; }
        public int[] p { get; set; }
        /// <summary>
        /// 成熟时间
        /// </summary>
        public long q { get; set; }
        /// <summary>
        /// 成熟时间?
        /// </summary>
        public long r { get; set; }
        public int s { get; set; }
        public int t { get; set; }
        public int u { get; set; }
    }


    public class FarmItems
    {
        public FarmItem a { get; set; }
        public FarmItem b { get; set; }
        public FarmItem2 c { get; set; }
        public FarmItem2 d { get; set; }
    }

    public class FarmItem
    {
        public int itemId { get; set; }
    }
    public class FarmItem2
    {
        public string itemId { get; set; }
    }

    public class Weather
    {
        public int weatherId { get; set; }
        public string weatherDesc { get; set; }
    }

    public class ServerTime
    {
        public long time { get; set; }
    }

    public class User
    {
        public string uId { get; set; }
        public string userName { get; set; }
        public int money { get; set; }
        public string FB { get; set; }
        public string headPic { get; set; }
        public int exp { get; set; }
    }
}
