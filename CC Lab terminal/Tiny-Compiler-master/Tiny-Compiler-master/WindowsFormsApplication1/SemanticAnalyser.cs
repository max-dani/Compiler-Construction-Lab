using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public class value
    {
        public double val;
        public string stringVal;
        public string dataType;
        public string scope;
        public value()
        {
            val = Int32.MinValue;
            dataType = "";
            scope = "";
            stringVal = "";
        }
    };

    public class FunctionValue
    {
        public string datatype;
        public List<string> parmsDatatypes;
        public List<string> parmsNames;
        public int numOfParms;
        public double returnValue;
        public string returnStringVal;
        public FunctionValue()
        {
            datatype = "";
            numOfParms = 0;
            returnValue = Int32.MinValue;
            parmsDatatypes = new List<string>();
            parmsNames = new List<string>();
            returnStringVal = "";
        }
        
    }

    /*public class Node
    {
        public List<Node> children = new List<Node>();
        public int value = Int32.MinValue;
        public string datatype = "";
        public string Name;
        public Token token;
        public Node(string N)
        {
            this.Name = N;
        }
    }*/
    class SemanticAnalyser
    {
        private static bool lastCondition = true;
        private static bool stop = false;
        private static bool begin = false;
        private static int counter = 0;
        private static int curIf = 0;
        private static string currentScope;
        private static string curFunction;
        public static List<string> infix;
        public static List<string> parameters;
        public static List<string> parametersNames;
        public static List<KeyValuePair<string, int>> nestedVars = new List<KeyValuePair<string, int>>();
        public static List<KeyValuePair<Node, string> > DeclaredFunctions = new List<KeyValuePair<Node, string>>();
        public static Dictionary<KeyValuePair<string, string>, value> symbolTable = new Dictionary<KeyValuePair<string, string>, value>();
        public static List<string> error = new List<string>();
        public static Dictionary<string, FunctionValue> functionTable = new Dictionary<string, FunctionValue>();
        
        public static string GetConstDatatype(string constant)
        {
            string ret = "int";
            for(int i = 0; i < constant.Length; i++)
            {
                if (constant[i] == '.')
                    ret = "float";
            }
            return ret;
        }

        public static string GetExpDatatype(List <string> infix)
        {
            string datatype = "int";

            for(int i = 0; i < infix.Count; i++)
            {
                for(int j = 0; j < infix[i].Length; j++)
                {
                    if (infix[i][j] == '"')
                    {
                        datatype = "string";
                        return datatype;
                    }
                    else if(infix[i][j] == '.')
                    {
                        datatype = "float";
                        return datatype;
                    }
                }
            }
            
            return datatype;
        }

        public static void GetInfix(Node root, string datatype, bool go)
        {
            if (root != null && root.token != null)
            {
                if(root.token.lex == "identifier" && root.stringVal != "funCall" && go)
                {
                    double val = -1;
                    string strVal = "";
                    KeyValuePair<string, string> kvp;
                    //if (currentScope[0] != 'c' && currentScope[0] != 'r')
                        kvp = new KeyValuePair<string, string>(root.children[0].token.lex, currentScope);
                    //else
                       // kvp = new KeyValuePair<string, string>(root.children[0].token.lex, curFunction);
                    if (symbolTable.ContainsKey(kvp))
                    {
                        val = symbolTable[kvp].val;
                        strVal = symbolTable[kvp].stringVal;
                        //check the datatype mismatch error
                        //MessageBox.Show("id " + kvp.Key + " datatype: " + symbolTable[kvp].dataType + " exp datatype: " + datatype);
                        if (symbolTable[kvp].dataType != datatype && datatype != "")
                        {
                            //MessageBox.Show(datatype);
                            error.Add("Datatype mismatch");
                        }
                    } 
                    else 
                        error.Add("Variable " + root.children[0].token.lex + " was not declared");

                    if (val != Int32.MinValue)
                        infix.Add(val.ToString());
                    else if (strVal != "")
                        infix.Add(strVal);
                    else
                        error.Add("Variable " + root.children[0].token.lex + " was not initialized");
                    
                }
                else if(root.token.token_type == Token_Class.constant && go)
                {
                    string constDatatype = GetConstDatatype(root.token.lex);
                    if (constDatatype == "int" && datatype == "float")
                        infix.Add(root.token.lex);
                    else if (GetConstDatatype(root.token.lex) != datatype && datatype != "")
                        error.Add("Datatype mismatch");
       
                    else
                        infix.Add(root.token.lex);
                }
                else if(root.token.lex == "string" && go)
                {
                    //MessageBox.Show(datatype);
                    if (datatype != "string")
                        error.Add("Datatype mismatch");
                    else
                        infix.Add(root.children[0].token.lex);
                }
                else if ((root.token.lex == "+" || root.token.lex == "*" || root.token.lex == "-" || root.token.lex == "/" || root.token.lex == "(" || root.token.lex == ")") && go)
                    infix.Add(root.token.lex);
                else if(root.token.lex == "function call" && go)
                {
                    //MessageBox.Show(root.token.lex);
                    //corner case
                    go = false;
                    root.children[0].stringVal = "funCall";
                    if (!functionTable.ContainsKey(root.children[0].children[0].token.lex))
                        error.Add("Calling a function was not declared");
                    else
                    {
                        /////////////////////////////////////////////
                        if (functionTable.ContainsKey(root.children[0].children[0].token.lex))
                        {
                            string funName = root.children[0].children[0].token.lex;
                            List<string> temp = infix;
                            infix = new List<string>();
                            int numOfParms = FuncPart(root.children[1], funName);
                            infix = new List<string>();
                            int numOfFunParms = functionTable[funName].numOfParms;

                            //Calling a function with a wrong number of parameters error
                            if (numOfParms != numOfFunParms)
                            {
                                error.Add("Function " + root.children[0].children[0].token.lex + " called with wrong number of parameters.");

                            }
                            else
                            {
                                bool f = false;
                                Node node = new Node();
                                foreach (KeyValuePair<Node, string> item in DeclaredFunctions)
                                {
                                    if (item.Value == funName)
                                    {
                                        f = true;
                                        node = item.Key;
                                        break;
                                    }
                                }
                                if (f)
                                {
                                    currentScope = funName;
                                    TraverseTree(node);
                                    currentScope = "Main";
                                    if (node.children[2].value != Int32.MinValue)
                                        root.value = node.children[2].value;
                                    else if (node.children[2].stringVal != "")
                                        root.stringVal = node.children[2].stringVal;
                                    List<string> delVars = new List<string>();
                                    foreach (var item in symbolTable)
                                    {
                                        if(item.Key.Value == funName)
                                        {
                                            int cnt = 0;
                                            for(int i = 0; i < functionTable[funName].parmsNames.Count; i++)
                                            {
                                                if (functionTable[funName].parmsNames[i] != item.Key.Key)
                                                    cnt++;
                                            }
                                            if(cnt == functionTable[funName].numOfParms)
                                                delVars.Add(item.Key.Key);
                                        }
                                    }
                                    for(int i = 0; i < delVars.Count; i++)
                                    {
                                        symbolTable.Remove(new KeyValuePair<string, string>(delVars[i], funName));
                                    }
                                }
                            }
                            infix = temp;
                        }
                        /////////////////////////////////////////////

                        if (root.value != Int32.MinValue)
                            infix.Add(Convert.ToString(root.value));
                        else if (root.stringVal != "")
                            infix.Add(root.stringVal);
                        //int len = root.children[1].children.Count;
                        //GetInfix(root.children[1].children[len - 1], datatype);
                    }

                }
                else if(root.token.lex == ")" && !go)
                {
                    go = true;
                }
                
            }

            for (int i = 0; i < root.children.Count; i++)
            {
                GetInfix(root.children[i], datatype, go);
            }
        }

        public static void Expr(Node root)
        {
            infix = new List<string>();
            int oldErrors = error.Count;
            GetInfix(root, root.datatype, true);
            root.datatype = GetExpDatatype(infix);
            
            if (error.Count == oldErrors)
            {
                if(root.datatype == "int" || root.datatype == "" )
                {
                    
                    List<string> postfix = SemanticHelper.InfixToPostfix(infix);
                    int value = SemanticHelper.EvaluateExp(postfix);
                    root.value = value;
                    
                }
                else if(root.datatype == "float" )
                {
                    List<string> postfix = SemanticHelper.InfixToPostfix(infix);
                    double value = SemanticHelper.EvaluateFloatExp(postfix);
                    root.value = value;
                }
                else
                {
                    //MessageBox.Show(infix[0]);
                    root.stringVal = infix[0];
                }
            }
        }

        public static void DeclStmt(Node root)
        {
            string datatype = root.children[0].children[0].token.lex;
            for(int i = 1; i < root.children.Count; i++)
            {
                if(root.children[i] == null || root.children[i].token == null)
                {
                    continue;
                }
                if(root.children[i].token.lex == "identifier")
                {
                    KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(root.children[i].children[0].token.lex, currentScope);
                    
                    if(symbolTable.ContainsKey(kvp))
                    {
                        error.Add("Variable " + root.children[i].children[0].token.lex + " was declared before");
                        continue;
                    }
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    nestedVars.Add(new KeyValuePair<string, int>(root.children[i].children[0].token.lex, curIf));
                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    root.children[i].datatype = datatype;
                    root.children[i].children[0].datatype = datatype;
                    
                    //saving the declared variable in the symbole table
                    string id = root.children[i].children[0].token.lex;
                    value v = new value();
                    v.dataType = datatype;
                    v.scope = currentScope;
                    symbolTable[kvp] = v;

                    //if the declared variable was initialized
                    if (i+2 < root.children.Count && root.children[i+2].token != null)
                    {
                        if(root.children[i+2].token.lex == "Expression")
                        {
                            //calculating the value of the expression
                            root.children[i + 2].datatype = datatype;
                            Expr(root.children[i + 2]);
                            root.children[i].value = root.children[i + 2].value;
                            root.children[i].children[0].value = root.children[i + 2].value;
                            
                            //saving the value of declared variable in the symbol table
                            //v.dataType = root.children[i].children[0].datatype = datatype;
                            v.val = root.children[i].children[0].value = root.children[i + 2].value;
                            v.stringVal = root.children[i + 2].stringVal;                         
                            symbolTable[kvp] = v;
                            //MessageBox.Show(id + " , Value= " + v.val.ToString() + " , Data Type: " + v.dataType);
                        }
                    }
                }
                
            }
            
        }

        public static void AssStmt(Node root)
        {
            string id = root.children[0].children[0].token.lex;
            KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(root.children[0].children[0].token.lex, currentScope);
            if(symbolTable.ContainsKey(kvp))
            {
                root.children[2].datatype = symbolTable[kvp].dataType;
                Expr(root.children[2]);
                //root.children[0].datatype = symbolTable[id].dataType;
                //root.children[0].value = root.children[2].value;
                root.children[0].children[0].datatype = symbolTable[kvp].dataType;
                root.children[0].children[0].value = root.children[2].value;
                if(root.children[2].value != Int32.MinValue || root.children[2].stringVal != "")
                {
                    symbolTable[kvp].val = root.children[2].value;
                    symbolTable[kvp].stringVal = root.children[2].stringVal;
                }
            }
            else
                error.Add("Variable " + id + " was not declared");
        }

        public static void Cond(Node root)
        {
            Expr(root.children[0]);
            Expr(root.children[2]);
            string oper = root.children[1].children[0].token.lex;
            double exp1_val = root.children[0].value;
            double exp2_val = root.children[2].value;
            if(oper == ">")
            {
                if (exp1_val > exp2_val)
                    root.value = 1;
                else
                    root.value = 0;
            }
            else if(oper == "<")
            {
                if (exp1_val < exp2_val)
                    root.value = 1;
                else
                    root.value = 0;
            }
            else if(oper == "=")
            {
                if (exp1_val == exp2_val)
                    root.value = 1;
                else
                    root.value = 0;
            }
            else if(oper == "<>")
            {
                if (exp1_val != exp2_val)
                    root.value = 1;
                else
                    root.value = 0;
            }
        }

        public static void CondStmt(Node root)
        {
            Cond(root.children[0].children[0]);
            if (root.children[0].children[0].value == 1)
                root.value = 1;
            else
                root.value = 0;
            
        }

        public static void ParmList(Node root)
        {
            if(root != null || root.token != null)
            {
                if (root.token.lex == "Parameter")
                {
                    string datatype = root.children[0].children[0].token.lex;
                    string name = root.children[1].children[0].token.lex;
                    KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(name, currentScope);
                    value val = new value();
                    val.dataType = datatype;
                    symbolTable[kvp] = val;
                    parameters.Add(root.children[0].children[0].token.lex);
                    parametersNames.Add(root.children[1].children[0].token.lex);
                }
            }
            
            for(int i = 0; i < root.children.Count; i++)
            {
                ParmList(root.children[i]);
            }
        }

        public static double ReturnStmt(Node root)
        {
            //if(root.datatype != "string")
            Expr(root.children[1]);
            root.value = root.children[1].value;
            root.datatype = root.children[1].datatype;
            //MessageBox.Show(root.value.ToString() + root.datatype);
            return root.children[1].value;

        }

        public static string ReturnStr(Node root)
        {
            Expr(root.children[1]);
            root.stringVal = root.children[1].stringVal;
            root.datatype = root.children[1].datatype;
            //MessageBox.Show(root.value.ToString() + root.datatype);
            return root.children[1].stringVal;
        }

        public static void FuncStmt(Node root)
        {
            string datatype = root.children[0].children[0].children[0].token.lex;
            string name = root.children[0].children[1].children[0].children[0].token.lex;
            if(functionTable.ContainsKey(name))
            {
                error.Add("Function " + name + " was declared before");
            }
            else
            {
                root.children[0].children[1].children[0].children[0].datatype = datatype;
                FunctionValue val = new FunctionValue();
                val.datatype = datatype;
                if (root.children[0].children[3].token != null)
                {
                    parameters = new List<string>();
                    parametersNames = new List<string>();
                    ParmList(root.children[0].children[3]);
                    val.numOfParms = parameters.Count;
                    val.parmsDatatypes = parameters;
                    val.parmsNames = parametersNames;
                }
                functionTable[name] = val;
            }
        }

        public static int FuncPart(Node root, string funName)
        {
            int ret = 0;
            for(int i = 0; i < root.children.Count; i++)
            {
                if (root.children[i].token.lex == ",")
                    ret++;
            }
            
            if(ret+1 == functionTable[funName].numOfParms)
            {
                //calculate the value and datatypes of parameters
                int cnt = 0, cnt2 = 0;
                List<string> parmsDatatypes = functionTable[funName].parmsDatatypes;
                List<string> parmsNames = functionTable[funName].parmsNames;
                for (int i = 0; i < root.children.Count; i++)
                {
                    if (root.children[i].token.lex == "Expression")
                    {
                        Expr(root.children[i]);
                        if(root.children[i].datatype != parmsDatatypes[cnt++])
                        {
                            error.Add("Function " + funName + " called with wrong parameters datatype");
                            break;
                        }
                        else
                        {
                            string pName = parmsNames[cnt2++];
                            KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(pName, funName);
                            //MessageBox.Show(pName + " " + funName);
                            if (root.children[i].datatype == "string")
                                symbolTable[kvp].stringVal = root.children[i].stringVal;
                            else
                                symbolTable[kvp].val = root.children[i].value;
                        }
                    }
                        
                }
            }
            if (functionTable[funName].numOfParms == 0 && ret == 0)
                return 0;
            else
                return ret + 1;
        }

        public static void Write(Node root)
        {
            if(root != null && root.token != null)
            {
                if(root.token.lex == "identifier")
                {
                    KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(root.children[0].token.lex, currentScope);
                    if (!symbolTable.ContainsKey(kvp))
                    {
                        error.Add("Variable " + root.children[0].token.lex + " was not declared");
                    }
                }
            }

            for(int i = 0; i < root.children.Count; i++)
            {
                Write(root.children[i]);
            }
        }

        public static void TraverseTree(Node root)
        {
            if(root != null && root.token != null)
            {
                if (root.token.lex == "Declaration_Statement" && !stop && lastCondition && begin)
                    DeclStmt(root);
                else if (root.token.lex == "Assignment_Statement" && !stop && lastCondition && begin)
                    AssStmt(root);
                else if (root.token.lex == "Condition_Statement" && !stop && lastCondition && begin)
                {
                    //try
                    //currentScope = "cond " + (counter++).ToString();
                    //
                    //CondStmt(root);
                    if (root.value == 0)
                        lastCondition = false;
                    
                }
                else if ((root.token.lex == "elseif" || root.token.lex == "else") && lastCondition && begin)
                    stop = true;
                else if((root.token.lex == "elseif" || root.token.lex == "else") && !lastCondition && begin)
                {
                    stop = false;
                    lastCondition = true;
                }
                else if (root.token.lex == "end" && begin)
                {
                    stop = false;
                    lastCondition = true;
                    for(int i = 0; i < nestedVars.Count; i++)
                    {
                        if(curIf == nestedVars[i].Value)
                        {
                            symbolTable.Remove(new KeyValuePair<string, string>(nestedVars[i].Key, currentScope));
                        }
                    }
                    curIf--;
                    //currentScope = "Main";
                }
                else if(root.token.lex == "Program" && !stop && lastCondition)
                {
                    if (root.children[0].token.lex == "Main_Function")
                    {
                        currentScope = "Main";
                        curFunction = "Main";
                        begin = true;
                    }
                        
                    else
                    {
                        currentScope = root.children[0].children[0].children[1].children[0].children[0].token.lex;
                        curFunction = root.children[0].children[0].children[1].children[0].children[0].token.lex;
                    }
                        
                }
                else if(root.token.lex == "Function_Statement")
                {
                    KeyValuePair<Node, string> k = new KeyValuePair<Node, string>(root.children[1], root.children[0].children[1].children[0].children[0].token.lex);
                    DeclaredFunctions.Add(k);
                    FuncStmt(root);
                }
                else if(root.token.lex == "Return_Statement" && !stop && lastCondition && currentScope != "Main" && begin)
                {
                    double returnedValue = 0;
                    int oldErrors = error.Count;
                    returnedValue = ReturnStmt(root);
                    if(returnedValue != Int32.MinValue)
                        functionTable[currentScope].returnValue = returnedValue;
                    else if(oldErrors == error.Count)
                    {
                        string ret = ReturnStr(root);
                        if(functionTable.ContainsKey(currentScope)) 
                            functionTable[currentScope].returnStringVal = ret;
                    }
                }
                else if(root.token.lex == "function call" && begin)
                {
                    ///////////////////////////////////////////////
                    //if (functionTable.ContainsKey(root.children[0].children[0].token.lex))
                    //{
                    //    string funName = root.children[0].children[0].token.lex;
                    //    int numOfParms = FuncPart(root.children[1], funName);
                    //    int numOfFunParms = functionTable[funName].numOfParms;

                    //    //Calling a function with a wrong number of parameters error
                    //    if (numOfParms != numOfFunParms)
                    //    {
                    //        error.Add("Function " + root.children[0].children[0].token.lex + " called with wrong number of parameters.");

                    //    }
                    //    else
                    //    {
                    //        bool f = false;
                    //        Node node = new Node();
                    //        foreach (KeyValuePair<Node, string> item in DeclaredFunctions)
                    //        {
                    //            if (item.Value == funName)
                    //            {
                    //                f = true;
                    //                node = item.Key;
                    //                break;
                    //            }
                    //        }
                    //        if (f)
                    //        {
                    //            currentScope = funName;
                    //            TraverseTree(node);
                    //            currentScope = "Main";
                    //            if (node.children[2].value != Int32.MinValue)
                    //                root.value = node.children[2].value;
                    //            else if (node.children[2].stringVal != "")
                    //                root.stringVal = node.children[2].stringVal;
                    //        }
                    //    }
                    //}
                }
                else if (root.token.lex == "repeat" && !stop && lastCondition && begin)
                {
                    //try
                    curIf++;

                }
                else if (root.token.lex == "until" && !stop && lastCondition && begin)
                {
                    for (int i = 0; i < nestedVars.Count; i++)
                    {
                        if (curIf == nestedVars[i].Value)
                        {
                            symbolTable.Remove(new KeyValuePair<string, string>(nestedVars[i].Key, currentScope));
                        }
                    }
                    curIf--;
                }
                else if(root.token.lex == "if" && begin && !stop && lastCondition)
                {
                    curIf = curIf + 1;
                }
                else if(root.token.lex == "Write_Statement" && begin && !stop && lastCondition)
                {
                    //root.datatype = 
                    //Expr(root.children[1]);
                    Write(root.children[1]);
                }
            }

            for (int i = 0; i < root.children.Count; i++)
            {
                TraverseTree(root.children[i]);
            }
        }



        public static TreeNode PrintSemanticTree(Node root)
        {
            TreeNode tree = new TreeNode("Annotated Tree");
            TreeNode treeRoot = PrintAnnotatedTree(root);
            tree.Expand();
            if (treeRoot != null)
                tree.Nodes.Add(treeRoot);
            return tree;
        }
        static TreeNode PrintAnnotatedTree(Node root)
        {
            if (root == null || root.token == null)
                return null;

            TreeNode tree;
            if (root.value == Int32.MinValue && root.datatype == "")
                //tree = new TreeNode(root.Name);
                tree = new TreeNode(root.token.lex);
            else if (root.value != Int32.MinValue && root.datatype == "")
                //tree = new TreeNode(root.Name + " & its value is: " + root.value);
                tree = new TreeNode(root.token.lex + " & its value is: " + root.value);
            else if (root.value == Int32.MinValue && root.datatype != "")
                //tree = new TreeNode(root.Name + " & its datatype is: " + root.datatype);
                tree = new TreeNode(root.token.lex + " & its datatype is: " + root.datatype);
            else
                //tree = new TreeNode(root.Name + " & its value is: " + root.value + " & datatype is: " + root.datatype);
                tree = new TreeNode(root.token.lex + " & its value is: " + root.value + " & datatype is: " + root.datatype);
            tree.Expand();
            if (root.children.Count == 0)
                return tree;
            foreach (Node child in root.children)
            {
                if (child == null || child.token == null)
                    continue;
                tree.Nodes.Add(PrintAnnotatedTree(child));
            }
            return tree;
        }
    }
    
}
