namespace Grapher.Utils
{
    public class Factor : ParsTreeNode
    {
        public enum FactorExpansion
        {
            Number,//1,2,3,etc
            Function,//sin,cos,etc
            MinuFactor,//-x,-15,-sin,-(x+1),etc
            WrappedExpression,//(expression)
            ID//x
        }
        public static bool IsFactor(string factor, string[] ids)
        {
            double tst;
            if (double.TryParse(factor, out tst))
                return true;
            else if (factor.StartsWith("(") && factor.EndsWith(")") && Expression.IsExpression(factor.Substring(1, factor.Length - 2), ids))
                return true;
            else if (factor.StartsWith("-") && Factor.IsFactor(factor.Substring(1, factor.Length - 1), ids))
                return true;
            else if (Function.IsFunction(factor, ids))
                return true;
            else if (IsID(factor, ids))
                return true;
            else { return false; }
        }
        private static bool IsID(string id, string[] ids)
        {
            foreach (string s in ids)
            {
                if (id == s)
                {
                    return true;
                }

            }
            return false;
        }
        public FactorExpansion Expansion { get; set; }
        public Function Function { get; set; }
        public Expression WrappedExpression { get; set; }
        public Factor InnerFactor;
        public Factor(string factor, string[] ids, ParsTreeNode parent)
            : base(factor, ids, parent)
        {

            this.Value = factor;
            double value;
            if (double.TryParse(factor, out value))
            {

                this.Expansion = FactorExpansion.Number;
            }
            else
            {
                if (factor.StartsWith("(") && factor.EndsWith(")"))
                {
                    this.Expansion = FactorExpansion.WrappedExpression;
                    this.WrappedExpression = new Expression(factor.Substring(1, factor.Length - 2), ids, this);
                }
                else if (Function.IsFunction(factor, ids))
                {
                    this.Expansion = FactorExpansion.Function;
                    this.Function = new Function(factor, ids, this);

                }
                else if (factor.StartsWith("-"))
                {
                    this.Expansion = FactorExpansion.MinuFactor;
                    this.InnerFactor = new Factor(factor.Substring(1, factor.Length - 1), ids, this);
                }
                else
                {
                    this.Expansion = FactorExpansion.ID;
                }
            }

        }
        public override double CalculateValue(double[] idsValue)
        {
            if (Expansion == FactorExpansion.Number)
            {
                return (double.Parse(this.Value));
            }
            else if (Expansion == FactorExpansion.WrappedExpression)
            {
                return (WrappedExpression.CalculateValue(idsValue));
            }
            else if (Expansion == FactorExpansion.Function)
            {
                return (this.Function.CalculateValue(idsValue));
            }
            else if (Expansion == FactorExpansion.MinuFactor)
            {
                return -this.InnerFactor.CalculateValue(idsValue);
            }
            else
            {
                //ID
                int idIndex = -1;
                for (int i = 0; i < IDs.Length; i++)
                    if (IDs[i] == this.Value)
                    {
                        idIndex = i;
                        break;
                    }
                return idsValue[idIndex];

            }
        }
        public override double[] CalculateValue(string[] ids, double[][] idsValues)
        {
            if (Expansion == FactorExpansion.Number)
            {
                double[] ret = new double[idsValues.Length];
                double value = double.Parse(this.Value);
                for (int i = 0; i < ret.Length; i++)
                    ret[i] = value;
                return ret;
            }
            else if (Expansion == FactorExpansion.WrappedExpression)
            {
                return (WrappedExpression.CalculateValue(ids, idsValues));
            }
            else if (Expansion == FactorExpansion.Function)
            {
                return (this.Function.CalculateValue(ids, idsValues));
            }
            else if (Expansion == FactorExpansion.MinuFactor)
            {
                double[] result = this.InnerFactor.CalculateValue(ids, idsValues);
                for (int i = 0; i < result.Length; i++)
                    result[i] = -result[i];
                return result;
            }
            else
            {
                //ID
                int idIndex = -1;
                for (int i = 0; i < ids.Length; i++)
                    if (ids[i] == this.Value)
                    {
                        idIndex = i;
                        break;
                    }
                return idsValues[idIndex];
            }
        }
    }
}