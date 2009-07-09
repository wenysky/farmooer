using System;
using System.Collections.Generic;
using System.Text;

namespace Natsuhime.Farmooer.Entities
{
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
        /// <summary>
        /// 被偷窃信息"n":{"1031769":1,"295854688":1}
        /// </summary>
        public Dictionary<string, int> n { get; set; }
        public int o { get; set; }
        public Dictionary<string, int> p { get; set; }
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
}
