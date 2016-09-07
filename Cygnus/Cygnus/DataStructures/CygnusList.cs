using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.Expressions;
using  Cygnus.Extensions;
namespace Cygnus.DataStructures
{
    public class CygnusList : CygnusObject, IIndexable
    {
        public override CygnusType type
        {
            get
            {
                return CygnusType.List;
            }
        }
        public List<CygnusObject> list { get; private set; }

        public CygnusInteger Length
        {
            get
            {
                return new CygnusInteger(list.Count);
            }
        }

        public CygnusObject this[params CygnusObject[] indexes]
        {
            get
            {
                int index = (int)indexes.Single();
                return list[index];
            }

            set
            {
                int index = (int)indexes.Single();
                list[index] = value;
            }
        }

        public CygnusList(List<CygnusObject> list)
        {
            this.list = list;
        }
        public override void Display(Scope scope)
        {
            list.DisplayList(scope);
        }
    }
}
