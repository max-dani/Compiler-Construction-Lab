using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/*<<<<<<< HEAD
public enum Token_Class {other,String, Error,comment,Identifier, constant, reservedKeyword, semicolon, loop, two_operator, Operator, program, IF, dataType, Return, end, Else, Elseif, until, repeat, then };
=======*/
public enum Token_Class {While, MulOp , AddOp , AssigmentOp, Epsilon, LeftCurlyBracket , RightCurlyBracket ,comma,RightBracket,LeftBracket,String, Error,comment,Identifier, constant, reservedKeyword, semicolon, loop, two_operator, Operator, program, IF, dataType, Return, end, Else, Elseif, until, repeat, then, Or_Operator, And_Operator, other, GreaterThanOp, NotEqualOp, LessThanOp, IsEqualOp };
//>>>>>>> origin/master

namespace WindowsFormsApplication1
{
    public class TokenChecker
    {
        Dictionary<String, Token_Class> Classes;
        public void filldic()
        {
            Classes.Add("if", Token_Class.IF);
            Classes.Add(";", Token_Class.semicolon);
            Classes.Add("program", Token_Class.program);
            Classes.Add("+", Token_Class.AddOp);
            Classes.Add("-", Token_Class.AddOp);
            Classes.Add("*", Token_Class.MulOp);
            Classes.Add("/", Token_Class.MulOp);
            Classes.Add("int", Token_Class.dataType);
            Classes.Add("float", Token_Class.dataType);
            Classes.Add("string", Token_Class.dataType);
            Classes.Add("read", Token_Class.reservedKeyword);
            Classes.Add("write", Token_Class.reservedKeyword);
            Classes.Add("<", Token_Class.LessThanOp);
            Classes.Add(">", Token_Class.GreaterThanOp);
            Classes.Add("=", Token_Class.IsEqualOp);
            Classes.Add("<>", Token_Class.NotEqualOp);
            Classes.Add("||", Token_Class.Or_Operator);
            Classes.Add("&&", Token_Class.And_Operator);
            Classes.Add("repeat", Token_Class.repeat);
            Classes.Add("while", Token_Class.While);
            Classes.Add("until", Token_Class.until);
            Classes.Add("elseif", Token_Class.Elseif);
            Classes.Add("else", Token_Class.Else);
            Classes.Add("then", Token_Class.then);
            Classes.Add("return", Token_Class.Return);
            Classes.Add("end", Token_Class.end);
            Classes.Add("(", Token_Class.LeftBracket);
            Classes.Add(")", Token_Class.RightBracket);
            Classes.Add(",", Token_Class.comma);
            Classes.Add("{", Token_Class.LeftCurlyBracket);
            Classes.Add("}", Token_Class.RightCurlyBracket);
            Classes.Add("main", Token_Class.reservedKeyword);
            Classes.Add(":=", Token_Class.AssigmentOp);
        }
        public TokenChecker()
        {
            Classes = new Dictionary<string, Token_Class>();
            filldic();
        }
        public string check(string T)
        {

            T = T.ToLower();
            string type = "Error";
            if (Classes.ContainsKey(T)) { type = Classes[T].ToString(); }
            else
            {

                int i = 0;
                if (Helper.is_letter(T[i]))
                {
                    while (i < T.Length && (Helper.is_letter(T[i]) || Helper.is_digit(T[i])))
                    {

                        i++;
                    }
                    if (i == T.Length)
                        type = Token_Class.Identifier.ToString();
                    else i = 0;

                }
                else if (Helper.is_digit(T[i]))
                {
                    int dots = 0;
                    while (i < T.Length && (Helper.is_digit(T[i]) || (T[i] == '.' && dots == 0)))
                    {
                        if (T[i] == '.')
                            dots++;
                        ++i;

                    }
                    if (dots < 2 && i == T.Length) type = Token_Class.constant.ToString();

                }
                else if (i <= T.Length - 2 && Helper.is_two_operator(T[i], T[i + 1]))
                {

                    if (T[i] == '/' && T[i + 1] == '/')
                    {
                        type = Token_Class.comment.ToString();
                    }else if ( T[i] == '/' && T[i + 1] == '*')
                    {
                        if (T[T.Length - 2] == '*' && T[T.Length - 1] == '/')
                            type = Token_Class.comment.ToString();
                        else type = Token_Class.Error.ToString();

                    }
                    else
                    type = Token_Class.two_operator.ToString();
                }
                else if (Helper.is_one_operator(T[i]))
                {
                    //type = Type.Operator.ToString();
                    if (T[i] == '"')
                    {
                        if (T[T.Length - 1] == '"') type = Token_Class.String.ToString();

                    }
                    else type = Token_Class.Error.ToString();
                }
            }
                return type;
            }
        }
    }
