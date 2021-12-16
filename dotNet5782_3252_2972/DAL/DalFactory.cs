using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public static class DalFactory
    {
        public static DalApi.IDal GetDal(string typeDl)
        {
            
            switch(typeDl)
            {
                case "List": return new DalObject.DalObject(); // DalApi.IDal.instance;
                // case "XML":return DLXML.instance;
                default: return null;
            }

        }
    }
}
