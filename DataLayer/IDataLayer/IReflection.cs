using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.IDataLayer
{
    public interface IReflection
    {
        void FillObjectWithProperty(ref object objectTo, string propertyName, object propertyValue);
    }
}
