using System;
using System.Collections.Generic;
using System.Text;

namespace Natsuhime.Farmooer.Entities
{
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
}
