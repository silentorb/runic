using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace runic.lexer
{
   public class Meadow
   {
       public string content;
       public string filename;

       public Meadow(string content, string filename)
       {
           this.content = content;
           this.filename = filename;
       }
   }
}
