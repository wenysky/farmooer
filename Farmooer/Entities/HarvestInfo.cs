using System;
using System.Collections.Generic;
using System.Text;

namespace Natsuhime.Farmooer.Entities
{
    public class HarvestInfo
    {
        public int farmlandIndex { get; set; }
        public int code { get; set; }
        public int poptype { get; set; }
        public string direction { get; set; }
        public int harvest { get; set; }
        public string exp { get; set; }
        public bool levelUp { get; set; }
        public HarvestStatus status { get; set; }
    }

    public class HarvestStatus
    {
        public int cId { get; set; }
        public int cropStatus { get; set; }
        public int oldweed { get; set; }
        public int oldpest { get; set; }
        public int oldhumidity { get; set; }
        public int weed { get; set; }
        public int pest { get; set; }
        public int humidity { get; set; }
        public int health { get; set; }
        public int harvestTimes { get; set; }
        public int output { get; set; }
        public int min { get; set; }
        public int leavings { get; set; }
        public int[] thief { get; set; }
        public int fertilize { get; set; }
        public int[] action { get; set; }
        public int plantTime { get; set; }
        public int updateTime { get; set; }
        public int pId { get; set; }
        public int nph { get; set; }
        public int mph { get; set; }
    }
}
