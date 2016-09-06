using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.Expressions;
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

        public CygnusClass(string ClassName, Scope ClassScope)
        {
            this.ClassName = ClassName;
            this.ClassScope = ClassScope;
            this.ClassScope.SetVariable("this", this);
        }
        public CygnusClass InitClass(Expression[] Values, Scope scope)
        {
            var newScope = ClassScope.Clone();
            var newclass = new CygnusClass(ClassName, newScope);
            var parameters = new ConstantExpression[] { new ConstantExpression(newclass) }.Concat(Values).ToArray();

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
                return ClassScope.GetFunction(field);
            }
            else
            {
                return (ClassScope.GetVariable(field) as ConstantExpression).Value;
            }
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
