using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyDown.Models
{
    public class saves
    {
        public int score { get; set; }

         public override string ToString()
         {
             return $"({score})";
         }
    }
}
