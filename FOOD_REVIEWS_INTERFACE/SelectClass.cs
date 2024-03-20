using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOOD_REVIEWS_INTERFACE
{
    class SelectClass
    {
        string tableName { set; get; }
        List<string> columns { get; set; }

        SelectClass(string tableName, List<string> columns)
        {
            this.tableName = tableName;
            this.columns = columns;
        }
    }
}
