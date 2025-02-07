namespace Grapher.Utils
{
    public class Expression : ParsTreeNode
    {
        public enum ExpressionExpansion
        {
            ExpressionPlusTerm,
            ExpressionMinusTerm,
            Term
        }
        public static bool IsExpression(string expr, string[] ids)
        {
            expr = expr.Replace(" ", "");
            int oprIndx = -1;
            int brackets = 0;
            for (int i = expr.Length - 1; i > 0; i--)
            {
                if (((expr[i] == '-' && !IsOperator(expr[i - 1]))
                    || expr[i] == '+') && (brackets == 0))
                {
                    oprIndx = i;
                    break;
                }
                else if (expr[i] == ')') brackets++;
                else if (expr[i] == '(') brackets--;
            }
            if (oprIndx > 0)
            {
                string subExpr, term;
                subExpr = expr.Substring(0, oprIndx);
                term = expr.Substring(oprIndx + 1);
                return (Term.IsTerm(term, ids) && IsExpression(subExpr, ids));
            }
            else
            {
                return Term.IsTerm(expr, ids);
            }
        }
        static bool IsOperator(char c)
        {
            return c == '-' || c == '+' || c == '*' || c == '/' || c == '^';
        }
        public ExpressionExpansion Expansion { get; set; }
        public Term Term { get; set; }
        public Expression SubExpression { get; set; }
        public Expression(string expr, string[] ids, ParsTreeNode parent)
            : base(expr, ids, parent)
        {
            expr = expr.Replace(" ", "");
            int oprIndx = -1;
            int brackets = 0;
            for (int i = expr.Length - 1; i > 0; i--)
            {
                if (((expr[i] == '-' && !IsOperator(expr[i - 1]))
                    || expr[i] == '+') && (brackets == 0))
                {
                    oprIndx = i;
                    break;
                }
                else if (expr[i] == ')') brackets++;
                else if (expr[i] == '(') brackets--;
            }
            if (oprIndx > 0)
            {
                string subExpr, term;
                char opr;
                subExpr = expr.Substring(0, oprIndx);
                term = expr.Substring(oprIndx + 1);
                opr = expr[oprIndx];
                this.Term = new Term(term, ids, this);
                this.SubExpression = new Expression(subExpr, ids, this);
                if (opr == '-')
                {
                    Expansion = ExpressionExpansion.ExpressionMinusTerm;
                }
                else
                {
                    Expansion = ExpressionExpansion.ExpressionPlusTerm;
                }
            }
            else
            {
                Expansion = ExpressionExpansion.Term;
                this.Term = new Term(expr, ids, this);
            }
        }
        public override double CalculateValue(double[] idsValue)
        {
            if (Expansion == ExpressionExpansion.ExpressionMinusTerm)
            {
                return (this.SubExpression.CalculateValue(idsValue) - this.Term.CalculateValue(idsValue));
            }
            else if (Expansion == ExpressionExpansion.ExpressionPlusTerm)
            {
                return (this.SubExpression.CalculateValue(idsValue) + this.Term.CalculateValue(idsValue));
            }
            else
                return (this.Term.CalculateValue(idsValue));
        }
        public override double[] CalculateValue(string[] ids, double[][] idsValues)
        {
            double[] ret = new double[idsValues.Length];
            if (Expansion == ExpressionExpansion.ExpressionMinusTerm)
            {
                double[] subExprValues = this.SubExpression.CalculateValue(ids, idsValues);
                double[] termValues = this.Term.CalculateValue(ids, idsValues);
                for (int i = 0; i < ret.Length; i++)
                    ret[i] = subExprValues[i] - termValues[i];
                return ret;
            }
            else if (Expansion == ExpressionExpansion.ExpressionPlusTerm)
            {
                double[] subExprValues = this.SubExpression.CalculateValue(ids, idsValues);
                double[] termValues = this.Term.CalculateValue(ids, idsValues);
                for (int i = 0; i < ret.Length; i++)
                    ret[i] = subExprValues[i] + termValues[i];
                return ret;
            }
            else
                return (this.Term.CalculateValue(ids, idsValues));
        }

    }
}