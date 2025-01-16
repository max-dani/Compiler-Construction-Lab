using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;


namespace CCLab14
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private Parser parser;
		private SymbolTable symbolTable;

		public MainWindow()
		{
			InitializeComponent();
			parser = new Parser();
			symbolTable = new SymbolTable();
		}

		private void CompileButton_Click(object sender, RoutedEventArgs e)
		{
			string sourceCode = inputTextBox.Text;
			try
			{
				parser.Parse(sourceCode);
				DisplayResults();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Compilation Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void DisplayResults()
		{
			outputTextBox.Text = "Compilation Successful!\n\n";
			outputTextBox.Text += "Symbol Table:\n";
			foreach (var entry in symbolTable.Entries)
			{
				outputTextBox.Text += $"{entry.Key}: {entry.Value.Type}\n";
			}
		}
	}

	public class SymbolTable
	{
		public Dictionary<string, SymbolInfo> Entries { get; private set; }

		public SymbolTable()
		{
			Entries = new Dictionary<string, SymbolInfo>();
		}

		public void AddSymbol(string identifier, SymbolType type)
		{
			if (Entries.ContainsKey(identifier))
			{
				throw new Exception($"Duplicate symbol: {identifier}");
			}
			Entries[identifier] = new SymbolInfo { Type = type };
		}

		public SymbolInfo GetSymbol(string identifier)
		{
			if (!Entries.ContainsKey(identifier))
			{
				throw new Exception($"Undefined symbol: {identifier}");
			}
			return Entries[identifier];
		}
	}

	public class SymbolInfo
	{
		public SymbolType Type { get; set; }
	}

	public enum SymbolType
	{
		Integer,
		Float,
		String,
		Boolean
	}

	public class Parser
	{
		private SymbolTable symbolTable;
		private List<string> tokens;
		private int currentTokenIndex;

		public Parser()
		{
			symbolTable = new SymbolTable();
		}

		public void Parse(string sourceCode)
		{
			// Tokenization (simplified)
			tokens = Tokenize(sourceCode);
			currentTokenIndex = 0;

			// Start parsing
			ParseProgram();
		}

		private List<string> Tokenize(string sourceCode)
		{
			// Very basic tokenization - replace with a more robust lexer in a real implementation
			return new List<string>(sourceCode.Split(new[] { ' ', '\t', '\n', '\r' },
				StringSplitOptions.RemoveEmptyEntries));
		}

		private void ParseProgram()
		{
			while (currentTokenIndex < tokens.Count)
			{
				ParseStatement();
			}
		}

		private void ParseStatement()
		{
			string currentToken = tokens[currentTokenIndex];

			// Simple declaration parsing
			if (IsValidType(currentToken))
			{
				SymbolType type = ParseType(currentToken);
				currentTokenIndex++;

				if (currentTokenIndex >= tokens.Count)
					throw new Exception("Unexpected end of input");

				string identifier = tokens[currentTokenIndex];
				symbolTable.AddSymbol(identifier, type);
				currentTokenIndex++;
			}
			else
			{
				throw new Exception($"Unexpected token: {currentToken}");
			}
		}

		private bool IsValidType(string token)
		{
			return token == "int" || token == "float" ||
				   token == "string" || token == "bool";
		}

		private SymbolType ParseType(string token)
		{
			switch (token)
			{
				case "int": return SymbolType.Integer;
				case "float": return SymbolType.Float;
				case "string": return SymbolType.String;
				case "bool": return SymbolType.Boolean;
				default: throw new Exception($"Unknown type: {token}");
			}
		}
	}

}