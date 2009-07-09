using System;
using System.Collections.Generic;
using System.Text;

namespace Natsuhime.Farmooer.Entities
{
    public class FarmlandStatus
    {
        /// <summary>
        /// 作物(2白萝;3红萝;4玉米)
        /// </summary>
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
        /// <summary>
        /// 健康度?(不健康0;健康1)
        /// </summary>
        public int h { get; set; }
        public int i { get; set; }
        public int j { get; set; }
        /// <summary>
        /// 产量
        /// </summary>
        public int k { get; set; }
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
        /// <summary>
        /// 灾害状态("p":{"1247125439":[3]}  应该是时间和id)
        /// </summary>
        public Dictionary<string, int[]> p { get; set; }
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
