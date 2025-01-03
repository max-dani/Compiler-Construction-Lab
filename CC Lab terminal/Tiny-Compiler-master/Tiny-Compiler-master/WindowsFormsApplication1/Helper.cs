using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public static class Helper
    {


        public static bool is_digit(char c)
        {
            return (c >= '0' && c <= '9');
        }

        public static bool is_letter(char c)
        {
            return ((c >= 'a' && c <= 'z')|| (c >= 'A' && c <= 'Z')||c=='_');
        }

        public static bool is_one_operator(char c)
        {
            return (c == '{' || c == '}' || c == '/' || c == '*' || c == '-' || c =='+'||c == '!' || c == '@' ||  c == '$' || c == '%' || c == '^'
              || c == '\'' || c == '"' || c == '[' || c == ']' || c == ',' || c == '&' || c == '(' || c == ')' || c == '~' || c == '>' || c == '<' || c == '=' || c == '='|| c == ';');
        }

        public static bool is_two_operator(char c1, char c2)
        {
            String s = "";
            s += c1;
            s += c2;
            return (s == "<>" || s == "&&" || s == "||" || s == "==" || s == ":=" || s == "!="||s=="//"||s=="/*" || s == "*/");
        }


    }
}
