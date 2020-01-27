using log4net;
using log4net.Config;
using System;
using System.Linq.Expressions;
using System.Text;

namespace Kursach
{
    static class Logger
    {
        public static ILog Log { get; } = LogManager.GetLogger("LOGGER");

        static Logger()
        {
            XmlConfigurator.Configure();
        }

        public static string GetParamsNamesValues(params Expression<Func<object>>[] expressions)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < expressions.Length; i++)
            {
                var exp = expressions[i];
                string name = null;
                object value = exp.Compile().Invoke();

                if (exp.Body.NodeType == ExpressionType.Convert)
                {
                    UnaryExpression unaryExpression = exp.Body as UnaryExpression;

                    if (unaryExpression.Operand.NodeType == ExpressionType.Call)
                    {
                        name = (unaryExpression.Operand as MethodCallExpression).Method.Name;
                    }
                    else name = (unaryExpression.Operand as MemberExpression).Member.Name;
                }
                else if (exp.Body.NodeType == ExpressionType.MemberAccess)
                {
                    name = ((MemberExpression)exp.Body).Member.Name;
                }
                else name = exp.Body.ToString();

                sb.Append($"{name}={value}");

                if (i + 1 < expressions.Length)
                    sb.Append(", ");
            }

            return sb.ToString();
        }
    }
}
