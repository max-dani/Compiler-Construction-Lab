Explanation:

Source Code: The user provides the source code in a C-like language.
Lexical Analyzer: It tokenizes the source code into tokens (keywords, identifiers, literals, operators, etc.).
Syntax Analyzer: The tokens are passed to the syntax analyzer, which builds the Abstract Syntax Tree (AST) by checking the grammar of the source code.
Semantic Analyzer: The AST is passed to the semantic analyzer, which performs type checking, variable scope resolution, and other semantic checks.
Error Handling: If any errors are detected at any stage, the respective error message is generated and shown to the user.
Generate Code: Once all analyses are complete, the code is translated into the target language or intermediate code.
