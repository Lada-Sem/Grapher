namespace Grapher.Utils
{
    public class Term : ParsTreeNode
    {
        public enum TermExpansion
        {
            TermMulFactor,
            TermDivFactor,
            TermPowFactor,
            Factor
        }
        public static bool IsTerm(string term, string[] ids)
        {
            int oprIndx = -1;
            int brackets = 0;
            for (int i = term.Length - 1; i > 0; i--)
            {
                if ((term[i] == '*' || term[i] == '/' || term[i] == '^') && (brackets == 0))
                {
                    oprIndx = i;
                    break;
                }
                else if (term[i] == ')') brackets++;
                else if (term[i] == '(') brackets--;
            }
            if (oprIndx > 0)
            {

                string subterm, factor;
                subterm = term.Substring(0, oprIndx);
                factor = term.Substring(oprIndx + 1);
                return Term.IsTerm(subterm, ids) && Factor.IsFactor(factor, ids);
            }
            else
            {
                return Factor.IsFactor(term, ids);
            }

        }
        public TermExpansion Expansion { get; set; }
        public Term SubTerm { get; set; }
        public Factor Factor { get; set; }
        public Term(string term, string[] ids, ParsTreeNode parent)
            : base(term, ids, parent)
        {

            this.Value = term;
            int oprIndx = -1;
            int brackets = 0;
            for (int i = term.Length - 1; i > 0; i--)
            {
                if ((term[i] == '*' || term[i] == '/' || term[i] == '^') && (brackets == 0))
                {
                    oprIndx = i;
                    break;
                }
                else if (term[i] == ')') brackets++;
                else if (term[i] == '(') brackets--;
            }
            if (oprIndx > 0)
            {

                string subterm, factor;
                char opr = term[oprIndx];
                subterm = term.Substring(0, oprIndx);
                factor = term.Substring(oprIndx + 1);
                this.Factor = new Factor(factor, ids, this);
                this.SubTerm = new Term(subterm, ids, this);
                if (opr == '*')
                {
                    this.Expansion = TermExpansion.TermMulFactor;
                }
                else if (opr == '/')
                {
                    this.Expansion = TermExpansion.TermDivFactor;
                }
                else
                {
                    this.Expansion = TermExpansion.TermPowFactor;
                }
            }
            else
            {
                this.Expansion = TermExpansion.Factor;
                this.Factor = new Factor(term, ids, this);
            }

        }
        public override double CalculateValue(double[] idsValue)
        {
            if (Expansion == TermExpansion.TermDivFactor)
            {
                return (this.SubTerm.CalculateValue(idsValue) / this.Factor.CalculateValue(idsValue));
            }
            else if (Expansion == TermExpansion.TermMulFactor)
            {
                return (this.SubTerm.CalculateValue(idsValue) * this.Factor.CalculateValue(idsValue));
            }
            else if (Expansion == TermExpansion.TermPowFactor)
            {
                return (Math.Pow(this.SubTerm.CalculateValue(idsValue), this.Factor.CalculateValue(idsValue)));
            }
            else
                return (this.Factor.CalculateValue(idsValue));
        }
        public override double[] CalculateValue(string[] ids, double[][] idsValues)
        {
            double[] ret = new double[idsValues.Length];
            if (Expansion == TermExpansion.TermDivFactor)
            {
                double[] subtermValues = this.SubTerm.CalculateValue(ids, idsValues);
                double[] factorValues = this.Factor.CalculateValue(ids, idsValues);
                for (int i = 0; i < ret.Length; i++)
                    ret[i] = subtermValues[i] / factorValues[i];
                return ret;
            }
            else if (Expansion == TermExpansion.TermMulFactor)
            {
                double[] subtermValues = this.SubTerm.CalculateValue(ids, idsValues);
                double[] factorValues = this.Factor.CalculateValue(ids, idsValues);
                for (int i = 0; i < ret.Length; i++)
                    ret[i] = subtermValues[i] * factorValues[i];
                return ret;
            }
            else if (Expansion == TermExpansion.TermPowFactor)
            {
                double[] subtermValues = this.SubTerm.CalculateValue(ids, idsValues);
                double[] factorValues = this.Factor.CalculateValue(ids, idsValues);
                for (int i = 0; i < ret.Length; i++)
                    ret[i] = Math.Pow(subtermValues[i], factorValues[i]);
                return ret;
            }
            else
                return (this.Factor.CalculateValue(ids, idsValues));
        }
    }
}