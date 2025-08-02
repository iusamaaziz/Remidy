using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remidy.Data
{
    public interface ILookup
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
