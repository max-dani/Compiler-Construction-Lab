using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public class Scanner
    {



        public static List<int> NewLine = new List<int>();
        public List<String> scan(String source_code)
        {
            NewLine.Add(-1);
            List<String> tokens = new List<String>();

            for (int i = 0; i < source_code.Length; i++)
            {
                int cur = i;
                String lex = "";

                if (Helper.is_letter(source_code[cur]))
                {
                    while (cur < source_code.Length && (Helper.is_letter(source_code[cur]) || Helper.is_digit(source_code[cur])))
                    {
                        lex += source_code[cur];
                        cur++;
                    }
                    tokens.Add(lex);
                    i = cur - 1;
                }
                else if (Helper.is_digit(source_code[cur]))
                {
                    int dots = 0;
                    while (cur < source_code.Length && (Helper.is_digit(source_code[cur]) || source_code[cur] == '.' || Helper.is_letter(source_code[cur])))
                    {
                        if (source_code[cur] == '.')
                            dots++;
                        lex += source_code[cur];
                        cur++;
                    }
                    tokens.Add(lex);
                    i = cur - 1;
                }
                else if (cur != source_code.Length - 1 && Helper.is_two_operator(source_code[cur], source_code[cur + 1]))
                {
                    lex += source_code[cur];
                    lex += source_code[cur + 1];

                    if (source_code[cur] == '/' && source_code[cur + 1] == '/')
                    {
                        cur = cur + 2;
                        while (cur < source_code.Length && source_code[cur] != '\n')
                        {
                            lex += source_code[cur];
                            ++cur;
                            //  MessageBox.Show(lex.ToString());
                        }
                    }
                    else if (source_code[cur] == '/' && source_code[cur + 1] == '*')
                    {

                        cur = cur + 2;
                        while (cur < source_code.Length - 1 && source_code[cur] != '*' && source_code[cur + 1] != '/')
                        {
                            lex += source_code[cur];
                            cur += 1;
                            //  MessageBox.Show(lex.ToString());
                        }

                        if (cur < source_code.Length - 1 && source_code[cur] == '*' && source_code[cur + 1] == '/')
                        {
                            lex += source_code[cur];
                            lex += source_code[cur + 1];
                            cur++;
                        }

                    }
                    else
                        cur++;
                   // MessageBox.Show(lex.ToString());
                    i = cur ;
                    tokens.Add(lex);
                }
                else if (Helper.is_one_operator(source_code[cur]))
                {
                    lex += source_code[cur];


                    if (source_code[cur] == '"')
                    {

                        //string tmp ="";
                        //  tmp += source_code[cur];
                        ++cur;
                        while (cur < source_code.Length && source_code[cur] != '"' && source_code[cur] != '\n' && source_code[cur] != ';')
                        {

                            lex += source_code[cur];
                            ++cur;

                        }
                        if (source_code[cur] == '"') lex += source_code[cur];
                    }
                    i = cur;
                    tokens.Add(lex);

                }
                else
                {
                    cur = i;
                    if (source_code[cur] == ' ' || source_code[cur] == '\r' || source_code[cur] == '\n')
                    {
                        if (source_code[cur] == '\n')
                            NewLine.Add(tokens.Count-1);
                            continue;
                    }
                    //  else tokens.Add(source_code[cur].ToString());
                    //  MessageBox.Show(source_code.Length.ToString());

                    while (cur < source_code.Length && source_code[cur] != ' ' && source_code[cur] != '\r' && source_code[cur] != '\n') { lex += source_code[cur]; ++cur; }
                    tokens.Add(lex);
                    i = cur - 1;
                }

            }
            NewLine.Add(tokens.Count-1);
            return tokens;
        }

    }
}