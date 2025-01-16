namespace CCLab08
{
	internal class Program
	{
		class CVariableDFA
		{
			// DFA States
			enum State
			{
				q0, // Initial state
				q1, // Accepting state
				q2  // Dead state
			}

			static void Main(string[] args)
			{
				Console.WriteLine("Enter a variable name to validate:");
				string input = Console.ReadLine();

				if (IsValidVariableName(input))
				{
					Console.WriteLine($"'{input}' is a valid C variable name.");
				}
				else
				{
					Console.WriteLine($"'{input}' is not a valid C variable name.");
				}
			}

			static bool IsValidVariableName(string input)
			{
				// Initial state
				State currentState = State.q0;

				foreach (char c in input)
				{
					switch (currentState)
					{
						case State.q0:
							if (IsLetter(c) || c == '_')
								currentState = State.q1;
							else
								currentState = State.q2;
							break;

						case State.q1:
							if (IsLetter(c) || IsDigit(c) || c == '_')
								currentState = State.q1;
							else
								currentState = State.q2;
							break;

						case State.q2:
							// Stay in the dead state
							return false;
					}
				}

				// Accept only if we end in the accepting state
				return currentState == State.q1;
			}

			static bool IsLetter(char c)
			{
				return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
			}

			static bool IsDigit(char c)
			{
				return (c >= '0' && c <= '9');
			}
		}
	}
}
