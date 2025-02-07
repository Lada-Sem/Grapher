using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grapher.Utils
{
    public abstract class ParsTreeNode
    {
        public string Value { get; set; }
        public abstract double CalculateValue(double[] idsValue);
        public abstract double[] CalculateValue(string[] ids, double[][] idsValues);
        public ParsTreeNode Parent { get; set; }
        public string[] IDs { get; set; }
        protected ParsTreeNode(string value, string[] ids, ParsTreeNode parnet)
        {
            this.Value = value.Replace(" ", "");
            this.Parent = parnet;
            this.IDs = ids;
        }
    }
}