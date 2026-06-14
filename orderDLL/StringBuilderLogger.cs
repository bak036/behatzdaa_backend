using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderDll
{
    public class StringBuilderLogger
    {

        StringBuilder sb = new StringBuilder();
        public void AppendLine(string txt)
        {
            sb.AppendLine($"{DateTime.Now:g}: {txt}");
        }

        public override string ToString()
        {
            return sb.ToString();
        }

        public StringBuilder ConvertToStringBuilder()
        {
            return sb;
        }

    }
}
