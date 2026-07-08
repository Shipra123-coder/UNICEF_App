using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    public interface ISQLHelper : IDisposable
    {
        public Task<DataSet> ExecuteProcedure(string procedureName, IDictionary<string, object> Parameters);
        public Task<DataSet> ExecuteProcedure(string procedureName, params SqlParameter[] param);

        public Task<object> ExecuteProcedureScalarAsync(string procedureName, params SqlParameter[] param);

    }
}
