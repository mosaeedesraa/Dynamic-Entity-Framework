using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.IDataLayer
{
    public interface ITables<T>
    {
       List<T> Select(string StoredProcedureName, ArrayList ParametersValues, string[] ParametersNames);
       List<T> AllData();
        bool Execute(string StoredProcedureName, ArrayList parametersValues, string[] parametersNames);
        T FirstOrDefault();
        T LastOrDefault();
        T Find(Int32 ID);
        decimal Max(string ColumnName);
        decimal Min(string ColumnName);
        decimal Sum(string ColumnName);
        Int32 Count();
        List<T> OrderBy();
        bool Insert(T param);
        List<T> OrderByDescending();
        List<T> Contains(string ColumnName, string Value);
        bool Delete(Int32 ID);
    }
}
