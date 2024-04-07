using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Objects
{
    public class PagedDto<T> where T : new()
    {
        public IReadOnlyList<T> Data { get; }
        public PagedDto(int total, IReadOnlyList<T> data)
        {
            TotalRecords = total;
            Data = data;
        }

        public int TotalRecords { get; set; }
    }
}
