using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Insql.Mappers
{
    public class InsqlEntityBuilder<TEntity> : InsqlEntityBuilder
        where TEntity : class, new()
    {
        public InsqlEntityBuilder() : base(typeof(TEntity))
        {
        }

        public InsqlEntityPropertyBuilder Property(Expression<Func<TEntity, object>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            var propertyInfo = this.GetProperty(expression) as PropertyInfo;

            if (propertyInfo == null)
            {
                throw new Exception($"{expression.ToString()} property not found!");
            }

            return this.Property(propertyInfo);
        }

        private MemberInfo GetProperty(LambdaExpression expression)
        {
            Expression expr = expression;

            for (; ; )
            {
                switch (expr.NodeType)
                {
                    case ExpressionType.Lambda:
                        expr = ((LambdaExpression)expr).Body;
                        break;
                    case ExpressionType.Convert:
                        expr = ((UnaryExpression)expr).Operand;
                        break;
                    case ExpressionType.MemberAccess:
                        MemberExpression memberExpression = (MemberExpression)expr;
                        MemberInfo mi = memberExpression.Member;
                        return mi;
                    default:
                        return null;
                }
            }
        }
    }
}
