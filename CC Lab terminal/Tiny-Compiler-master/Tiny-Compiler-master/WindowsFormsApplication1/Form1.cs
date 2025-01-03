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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
           
        }
        List<string> GetLines(string Jaconcode)
        {

            List<string> Lines = new List<string>();

            string tmp = "";

            for (int i = 0; i < Jaconcode.Length; i++)
            {
                if (Jaconcode[i] == '\n' || Jaconcode[i] == '\r') continue;
                tmp += Jaconcode[i];

                if (Jaconcode[i] == ';')
                {
                    Lines.Add(tmp);
                    tmp = "";
                }
            }

            return Lines;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SyntaxAnalyser.ErrorList.Clear();
            Scanner.NewLine.Clear();
            // just for test 
            // you shoud display two lists (compile.tokens ,compile.types)
            string code = JaconCode.Text;
            Compiler compile = new Compiler(code);
           

            int numberOfElement = compile.tokens.Count();

            DataTable dt = new DataTable();
            dt.Columns.Add("Lexems");
            dt.Columns.Add("Type");

            DataTable dt2 = new DataTable();
            dt2.Columns.Add("Lexems");
  
            List<string> Errors = new List<string>();
            
            for (int i = 0; i < numberOfElement; ++i) {
               // MessageBox.Show(compile.tokens[i].ToString());
               // MessageBox.Show(compile.types[i].ToString());
                if (compile.types[i].ToString() == "Error")
                {
                    dt2.Rows.Add(compile.tokens[i].ToString());
                }
                else
                dt.Rows.Add(compile.tokens[i].ToString(), compile.types[i].ToString());
           }
            dataGridView.DataSource = dt;
            dgv.DataSource = dt2;
            // Parsing Process 
            //treeView1.Nodes.Add(SyntaxAnalyser.PrintParseTree(compile.root));
            SemanticAnalyser.TraverseTree(compile.root);
            treeView1.Nodes.Add(SemanticAnalyser.PrintSemanticTree(compile.root));

            ////////////////////////////////////////////////try///////////////////////////////////////////////////////////////
            DataTable symTable = new DataTable();
            symTable.Columns.Add("Identifier");
            symTable.Columns.Add("Value");
            symTable.Columns.Add("Datatype");
            symTable.Columns.Add("Scope");
            foreach (var item in SemanticAnalyser.symbolTable)
            {
                //MessageBox.Show(item.Key.Key + " , Value= " + item.Value.val.ToString() + " , Data Type: " + item.Value.dataType + " , Scope: " + item.Key.Value + ", String Value= " + item.Value.stringVal);
                string val = "";
                if (item.Value.val != Int32.MinValue)
                    val = item.Value.val.ToString();
                else
                    val = item.Value.stringVal;
                symTable.Rows.Add(item.Key.Key, val, item.Value.dataType, item.Key.Value);
            }
            dataGridView2.DataSource = symTable;

            DataTable funTable = new DataTable();
            funTable.Columns.Add("Identifier");
            funTable.Columns.Add("Datatype");
            funTable.Columns.Add("Num of Parameters");
            funTable.Columns.Add("Parameters");
            funTable.Columns.Add("Returned Value");
            foreach (var item in SemanticAnalyser.functionTable)
            {
                string parms = "";
                string names = "";
                string ret = "";
                //if (item.Value.parmsDatatypes.Count == 0)
                    //MessageBox.Show(item.Key + " , Datatype: " + item.Value.datatype + " , Returned Value= " + item.Value.returnValue);
                //else
                //{
                    for (int i = 0; i < item.Value.parmsDatatypes.Count; i++)
                    {
                        parms += item.Value.parmsDatatypes[i] + ", ";
                        names += item.Value.parmsNames[i] + ", ";
                    }
                    //MessageBox.Show(item.Key + " , Datatype: " + item.Value.datatype + ", num of parms= " + item.Value.parmsDatatypes.Count.ToString() + ", parms: " + parms + " Parmas Names: " + names + " , Returned Value= " + item.Value.returnValue);
                //}
                if (item.Value.returnValue != Int32.MinValue)
                    ret = item.Value.returnValue.ToString();
                else
                    ret = item.Value.returnStringVal;
                funTable.Rows.Add(item.Key, item.Value.datatype, item.Value.numOfParms, parms, ret);
            }
            dataGridView3.DataSource = funTable;

            DataTable er = new DataTable();
            er.Columns.Add("Error");
            foreach (var item in SemanticAnalyser.error)
            {
                //MessageBox.Show(item);
                er.Rows.Add(item);
            }
            dataGridView4.DataSource = er;
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            DataTable ParserErrors = new DataTable();
            ParserErrors.Columns.Add("Line");
            for (int i=0; i<SyntaxAnalyser.ErrorList.Count; i++)
            {
                ParserErrors.Rows.Add(SyntaxAnalyser.ErrorList[i]);
            }
            dataGridView1.DataSource = ParserErrors;
        }
    }
}
