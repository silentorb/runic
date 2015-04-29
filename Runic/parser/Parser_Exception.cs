using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using runic.lexer;

namespace runic.parser
{
   public class Parser_Exception : Exception
   {
       public string filename;
       public Position position;

       public Parser_Exception(string message, string filename, Position position)
           :base(message)
       {
           this.filename = filename;
           this.position = position;
       }

       public Parser_Exception(string message, Position position)
           : base(message)
       {
           filename = position.meadow.filename;
           this.position = position;
       }
    }
}
