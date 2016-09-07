using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.Expressions;
using Cygnus.Errors;
namespace Cygnus.DataStructures
{
    public class CygnusClass : CygnusObject, IDotAccessible
    {
        public override CygnusType type
        {
            get
            {
                return new CygnusType("Class", 12);
            }
        }
        public Scope ClassScope { get; protected set; }//class has its own scope
        public string ClassName { get; private set; }
        public CygnusClass Parent { get; private set; }
        public string ParentName { get; private set; }
        public CygnusClass(string ClassName, Scope ClassScope, CygnusClass Parent = null)
        {
            this.ClassName = ClassName;
            this.ClassScope = ClassScope;
            this.Parent = Parent;
            this.ClassScope.SetVariable("this", this);
            this.ClassScope.SetVariable("base", this.Parent);
        }
        public CygnusClass InitClass(Expression[] Values, Scope scope)
        {
            var newScope = ClassScope.Clone();
            var newclass = new CygnusClass(ClassName, newScope, Parent);
            var parameters = new ConstantExpression[] { new ConstantExpression(newclass) }.Concat(Values.Select(i => i.Eval(scope))).ToArray();
            newScope.GetFunction("__init__")
                .Update(parameters, newScope).Eval(newScope);
            return newclass;
        }
        public override void Display(Scope scope)
        {
            Console.WriteLine(ClassScope);
        }

        public Expression GetByDot(string field, bool IsMethod)
        {
            if (IsMethod)
            {
                FunctionExpression func;
                if (TryGetFunctionByDot(field, out func))
                {
                    return func;
                }
                else throw new NotDefinedException(field);
            }
            else
            {
                //  return (ClassScope.GetVariable(field) as ConstantExpression).Value;
                Expression value;
                if (TryGetFieldByDot(field, out value))
                {
                    return (value as ConstantExpression).Value;
                }
                else throw new NotDefinedException(field);
            }
        }
        private bool TryGetFunctionByDot(string field, out FunctionExpression func)
        {
            var Current = this;
            while (Current != null)
            {
                if (Current.ClassScope.TryGetFunction(field, out func))
                {
                    return true;
                }
                else
                    Current = Current.Parent;
            }
            func = null;
            return false;
        }
        private bool TryGetFieldByDot(string field, out Expression value)
        {
            var Current = this;
            while (Current != null)
            {
                if (Current.ClassScope.TryGetVariable(field, out value))
                {
                    return true;
                }
                else
                    Current = Current.Parent;
            }
            value = null;
            return false;
        }
        public void SetByDot(CygnusObject obj, string field, bool IsMethod)
        {
            if (IsMethod)
                throw new NotImplementedException();
            else
            {
                ClassScope.SetVariable(field, obj);
            }
        }
    }
}
