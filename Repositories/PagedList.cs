using System;
using System.Collections.Generic;

namespace aspdotnet_managesys.Repositories
{
    public class PagedList<T> where T : class
    {
        public IList<T> Content { get; set; }

        public Boolean Last { get; set; }

        public int TotalElements { get; set; }

        public int TotalPages { get; set; }

        public int Size { get; set; }

        public int Page { get; set; }

        public int NumberOfElements { get; set; }
    }
}