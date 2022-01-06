using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
{
    public static class DalFactory
    {
        public static DalApi.IDal GetDal(string typeDl)
        {
            
            switch(typeDl)
            {
                case "List": return DalObject.DalObject.GetInstance(); // DalApi.IDal.instance;
                case "XML": return DalXml.DalXml.GetInstance();
                default: return null;
            }

        }
    }
}
