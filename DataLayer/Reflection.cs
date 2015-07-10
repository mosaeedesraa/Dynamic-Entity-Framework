using DataLayer.IDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// Eng : Mohammad Alsaid 

namespace DataLayer
{
    public class Reflection : IReflection
    {
        public void FillObjectWithProperty(ref object objectTo, string propertyName, object propertyValue)
        {
            Type tOb2 = objectTo.GetType();
            if (!String.IsNullOrEmpty(propertyValue.ToString()))
            {
                tOb2.GetProperty(propertyName).SetValue(objectTo, propertyValue);
            }
            
        }
    }
}
