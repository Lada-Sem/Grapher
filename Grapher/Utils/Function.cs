namespace Grapher.Utils
{
    public class Function : ParsTreeNode
    {
        public enum FunctionEnum
        {
            Sinh, Sin, Cosh, Cos, Tanh, Tan, Coth, Cot, Sich, Sic, Csch, Csc, E, Log, Ln, Sqrt
        }
        public static bool IsFunction(string function, string[] ids)
        {
            foreach (string func in Enum.GetNames(typeof(FunctionEnum)))
            {
                if (function.ToLower().StartsWith(func.ToLower()))
                {
                    return Term.IsTerm(function.Substring(func.Length), ids);
                }
            }
            return false;
        }
        public FunctionEnum Func { get; set; }
        public Term Term;

        public Function(string function, string[] ids, ParsTreeNode parent)
            : base(function, ids, parent)
        {

            foreach (string func in Enum.GetNames(typeof(FunctionEnum)))
            {
                if (function.ToLower().StartsWith(func.ToLower()))
                {

                    Func = (FunctionEnum)Enum.Parse(typeof(FunctionEnum), func);
                    this.Term = new Term(function.Substring(func.Length), ids, this);
                    break;
                }
            }
        }
        public override double CalculateValue(double[] idsValue)
        {
            double termValue = this.Term.CalculateValue(idsValue);
            double ret = 0;
            switch (Func)
            {
                case FunctionEnum.Sin:
                    ret = Math.Sin(termValue * Math.PI / 180);
                    break;
                case FunctionEnum.Cos:
                    ret = Math.Sin(termValue * Math.PI / 180);
                    break;
                case FunctionEnum.Tan:
                    ret = Math.Tan(termValue * Math.PI / 180);
                    break;
                case FunctionEnum.Sinh:
                    ret = Math.Sinh(termValue * Math.PI / 180);
                    break;
                case FunctionEnum.Cosh:
                    ret = Math.Cosh(termValue * Math.PI / 180);
                    break;
                case FunctionEnum.Tanh:
                    ret = Math.Tanh(termValue * Math.PI / 180);
                    break;
                case FunctionEnum.Csc:
                    ret = (1 / Math.Sin(termValue * Math.PI / 180));
                    break;
                case FunctionEnum.Sic:
                    ret = (1 / Math.Cos(termValue * Math.PI / 180));
                    break;
                case FunctionEnum.Cot:
                    ret = (1 / Math.Tan(termValue * Math.PI / 180));
                    break;
                case FunctionEnum.Csch:
                    ret = (1 / Math.Sinh(termValue * Math.PI / 180));
                    break;
                case FunctionEnum.Sich:
                    ret = (1 / Math.Cosh(termValue * Math.PI / 180));
                    break;
                case FunctionEnum.Coth:
                    ret = (1 / Math.Tanh(termValue * Math.PI / 180));
                    break;
                case FunctionEnum.E:
                    ret = (Math.Exp(termValue));
                    break;
                case FunctionEnum.Log:
                    ret = (Math.Log10(termValue));
                    break;
                case FunctionEnum.Ln:
                    ret = (Math.Log(termValue, Math.E));
                    break;
                case FunctionEnum.Sqrt:
                    ret = (Math.Sqrt(termValue));
                    break;
            }
            return ret;

        }
        public override double[] CalculateValue(string[] ids, double[][] idsValues)
        {
            double[] termValue = this.Term.CalculateValue(ids, idsValues);
            double[] ret = new double[termValue.Length];
            switch (Func)
            {
                case FunctionEnum.Sin:
                    for (int i = 0; i < ret.Length; i++)
                        ret[i] = Math.Sin(termValue[i] * Math.PI / 180);
                    break;
                case FunctionEnum.Cos:
                    for (int i = 0; i < ret.Length; i++)
                        ret[i] = Math.Cos(termValue[i] * Math.PI / 180);
                    break;
                case FunctionEnum.Tan:
                    for (int i = 0; i < ret.Length; i++)
                        ret[i] = Math.Tanh(termValue[i] * Math.PI / 180);
                    break;
                case FunctionEnum.Sinh:
                    for (int i = 0; i < ret.Length; i++)
                        ret[i] = Math.Sinh(termValue[i] * Math.PI / 180);
                    break;
                case FunctionEnum.Cosh:
                    for (int i = 0; i < ret.Length; i++)
                        ret[i] = Math.Cosh(termValue[i] * Math.PI / 180);
                    break;
                case FunctionEnum.Tanh:
                    for (int i = 0; i < ret.Length; i++)
                        ret[i] = Math.Tanh(termValue[i] * Math.PI / 180);
                    break;
                case FunctionEnum.Csc:
                    for (int i = 0; i < ret.Length; i++)
                        ret[i] = (1 / Math.Sin(termValue[i] * Math.PI / 180));
                    break;
                case FunctionEnum.Sic:
                    for (int i = 0; i < ret.Length; i++)
                        ret[i] = (1 / Math.Cos(termValue[i] * Math.PI / 180));
                    break;
                case FunctionEnum.Cot:
                    for (int i = 0; i < ret.Length; i++)
                        ret[i] = (1 / Math.Tan(termValue[i] * Math.PI / 180));
                    break;
                case FunctionEnum.Csch:
                    for (int i = 0; i < ret.Length; i++)
                        ret[i] = (1 / Math.Sinh(termValue[i] * Math.PI / 180));
                    break;
                case FunctionEnum.Sich:
                    for (int i = 0; i < ret.Length; i++)
                        ret[i] = (1 / Math.Cosh(termValue[i] * Math.PI / 180));
                    break;
                case FunctionEnum.Coth:
                    for (int i = 0; i < ret.Length; i++)
                        ret[i] = (1 / Math.Tanh(termValue[i] * Math.PI / 180));
                    break;
                case FunctionEnum.E:
                    for (int i = 0; i < ret.Length; i++)
                        ret[i] = (Math.Exp(termValue[i]));
                    break;
                case FunctionEnum.Log:
                    for (int i = 0; i < ret.Length; i++)
                        ret[i] = (Math.Log10(termValue[i]));
                    break;
                case FunctionEnum.Ln:
                    for (int i = 0; i < ret.Length; i++)
                        ret[i] = (Math.Log(Math.E, termValue[i]));
                    break;
            }
            return ret;

        }
    }
}