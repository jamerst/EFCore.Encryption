using System.Linq.Expressions;
using System.Reflection;

using Microsoft.AspNetCore.OData.Query.Expressions;

namespace EFCore.Encryption.OData;
public class HashedValueFilterBinder : FilterBinder
{
    internal static List<Type> HashTypes = new List<Type>();

    public override Expression BindSingleValueFunctionCallNode(Microsoft.OData.UriParser.SingleValueFunctionCallNode node, QueryBinderContext context)
    {
        if (node.Name == nameof(IHashedValue<object>.HashEquals).ToLower())
        {
            Expression[] arguments = BindArguments(node.Parameters, context);

            Type? type = HashTypes.FirstOrDefault(t => arguments.First().Type.IsAssignableFrom(t));
            if (type != default)
            {
                MethodInfo methodInfo = type.GetMethod(nameof(IHashedValue<object>.HashEquals))!;

                return Expression.Call(arguments.First(), methodInfo, arguments.Skip(1));
            }
        }

        return base.BindSingleValueFunctionCallNode(node, context);
    }
}
