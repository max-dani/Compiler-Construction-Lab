namespace WindowsFormsApplication1
{
    public class Token
    {
        public string lex;
        public Token_Class token_type;
        public Token(string lex , Token_Class token_type)
        {
            this.lex = lex;
            this.token_type = token_type;
        }
    }
}