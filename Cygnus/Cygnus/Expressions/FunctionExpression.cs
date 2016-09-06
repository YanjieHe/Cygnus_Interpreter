using System.Collections.ObjectModel;
using System.Collections.Generic;
using Cygnus.Errors;
using Cygnus.Extensions;
using Cygnus.DataStructures;
namespace Cygnus.Expressions
{
    public class FunctionExpression : Expression
    {
        public ReadOnlyCollection<ParameterExpression> Arguments { get; private set; }
        public Expression Body { get; private set; }
        public string Name { get; private set; }
        public Scope funcScope { get; private set; }
        public CygnusClass InClass { get; set; }
        public FunctionExpression(string Name, Expression Body, Scope scope, IList<ParameterExpression> Arguments, CygnusClass InClass = null)
        {
            this.Name = Name;
            this.Body = Body;
            this.Arguments = new ReadOnlyCollection<ParameterExpression>(Arguments);
            this.funcScope = scope;
            this.InClass = InClass;
        }
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.Function;
            }
        }
        public override string ToString()
        {
            return "(Function)";
        }
        public override Expression Eval(Scope scope)
        {
            //return Body.Eval(scope);
            return Body.Eval(funcScope);
        }
        public FunctionExpression Update(Expression[] Values, Scope scope)
        {
            Scope newScope = new Scope(funcScope.Parent);
            //if (InClass == null || (Arguments.Count == 0) || (Arguments[0].Name != "this"))
            //{
                (Arguments.Count == Values.Length)
                .OrThrows<ParameterException>("Wrong number parameters for function {0}", Name);
                for (int i = 0; i < Arguments.Count; i++)
                    newScope.SetVariable(Arguments[i].Name, Values[i].Eval(scope));
            //}
            //else
            //{
            //    (Arguments.Count == Values.Length + 1)
            //  .OrThrows<ParameterException>("Wrong number parameters for function {0}", Name);
            //    newScope.SetVariable("this", InClass);
            //    for (int i = 1; i < Arguments.Count; i++)
            //        newScope.SetVariable(Arguments[i].Name, Values[i - 1].Eval(scope));
            //}
            return new FunctionExpression(Name, Body, newScope, Arguments);
        }
    }
}
