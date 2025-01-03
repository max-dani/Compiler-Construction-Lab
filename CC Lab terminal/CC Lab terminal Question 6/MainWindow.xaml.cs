using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace CC_Lab_terminal_Question_6
{
	public partial class MainWindow : Window
	{
		private List<string> invalidUsernames = new List<string>();
		private readonly UsernameValidator validator = new UsernameValidator();
		private readonly PasswordGenerator passwordGenerator = new PasswordGenerator();

		public MainWindow()
		{
			InitializeComponent();
		}

		private void ValidateButton_Click(object sender, RoutedEventArgs e)
		{
			string input = UsernamesInput.Text.Trim();
			ProcessUsernames(input);
		}

		private void ProcessUsernames(string input)
		{
			var usernames = input.Split(new[] { ',', '\n' }, StringSplitOptions.RemoveEmptyEntries)
								.Select(u => u.Trim())
								.ToList();

			ResultsTextBox.Document.Blocks.Clear();
			invalidUsernames.Clear();

			int validCount = 0;
			foreach (var username in usernames)
			{
				var validationResult = validator.ValidateUsername(username);
				if (validationResult.IsValid)
				{
					validCount++;
					AddValidUsernameResult(username, validationResult);
				}
				else
				{
					invalidUsernames.Add(username);
					AddInvalidUsernameResult(username, validationResult.ErrorMessage);
				}
			}

			AddSummary(usernames.Count, validCount);
		}

		private void AddValidUsernameResult(string username, ValidationResult result)
		{
			var password = passwordGenerator.GeneratePassword();
			var strength = passwordGenerator.CheckPasswordStrength(password);

			var paragraph = new Paragraph();
			paragraph.Inlines.Add(new Bold(new Run($"{username} - Valid\n")));
			paragraph.Inlines.Add(new Run($"Letters: {result.TotalLetters} (Uppercase: {result.UppercaseCount}, " +
										$"Lowercase: {result.LowercaseCount}), Digits: {result.DigitCount}, " +
										$"Underscores: {result.UnderscoreCount}\n"));
			paragraph.Inlines.Add(new Run($"Generated Password: {password} (Strength: {strength})\n\n"));

			ResultsTextBox.Document.Blocks.Add(paragraph);
		}

		private void AddInvalidUsernameResult(string username, string error)
		{
			var paragraph = new Paragraph();
			paragraph.Inlines.Add(new Bold(new Run($"{username} - Invalid\n")));
			paragraph.Inlines.Add(new Run($"Error: {error}\n\n"));
			ResultsTextBox.Document.Blocks.Add(paragraph);
		}

		private void AddSummary(int total, int validCount)
		{
			var paragraph = new Paragraph();
			paragraph.Inlines.Add(new Bold(new Run("Summary:\n")));
			paragraph.Inlines.Add(new Run($"Total Usernames: {total}\n"));
			paragraph.Inlines.Add(new Run($"Valid Usernames: {validCount}\n"));
			paragraph.Inlines.Add(new Run($"Invalid Usernames: {invalidUsernames.Count}\n\n"));
			ResultsTextBox.Document.Blocks.Add(paragraph);
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				TextRange range = new TextRange(ResultsTextBox.Document.ContentStart,
											  ResultsTextBox.Document.ContentEnd);
				using (FileStream fs = new FileStream("UserDetails.txt", FileMode.Append))
				{
					range.Save(fs, DataFormats.Text);
				}
				MessageBox.Show("Results saved to UserDetails.txt", "Success");
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error saving file: {ex.Message}", "Error");
			}
		}

		private void RetryButton_Click(object sender, RoutedEventArgs e)
		{
			if (invalidUsernames.Count == 0)
			{
				MessageBox.Show("No invalid usernames to retry.", "Information");
				return;
			}

			UsernamesInput.Text = string.Join(",", invalidUsernames);
		}
	}

	public class UsernameValidator
	{
		private readonly Regex usernameRegex = new Regex(@"^[a-zA-Z][a-zA-Z0-9_]{4,14}$");

		public ValidationResult ValidateUsername(string username)
		{
			var result = new ValidationResult();

			if (string.IsNullOrWhiteSpace(username))
			{
				result.ErrorMessage = "Username cannot be empty";
				return result;
			}

			if (username.Length < 5 || username.Length > 15)
			{
				result.ErrorMessage = "Username length must be between 5 and 15 characters";
				return result;
			}

			if (!char.IsLetter(username[0]))
			{
				result.ErrorMessage = "Username must start with a letter";
				return result;
			}

			if (!usernameRegex.IsMatch(username))
			{
				result.ErrorMessage = "Username can only contain letters, numbers, and underscores";
				return result;
			}

			// Count character types
			result.IsValid = true;
			result.UppercaseCount = username.Count(char.IsUpper);
			result.LowercaseCount = username.Count(char.IsLower);
			result.DigitCount = username.Count(char.IsDigit);
			result.UnderscoreCount = username.Count(c => c == '_');
			result.TotalLetters = result.UppercaseCount + result.LowercaseCount;

			return result;
		}
	}

	public class ValidationResult
	{
		public bool IsValid { get; set; }
		public string ErrorMessage { get; set; }
		public int UppercaseCount { get; set; }
		public int LowercaseCount { get; set; }
		public int DigitCount { get; set; }
		public int UnderscoreCount { get; set; }
		public int TotalLetters { get; set; }
	}

	public class PasswordGenerator
	{
		private readonly Random random = new Random();
		private const string UppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		private const string LowercaseChars = "abcdefghijklmnopqrstuvwxyz";
		private const string DigitChars = "0123456789";
		private const string SpecialChars = "!@#$%^&*";

		public string GeneratePassword()
		{
			var password = new List<char>();

			// Add required characters
			password.AddRange(GetRandomChars(UppercaseChars, 2));
			password.AddRange(GetRandomChars(LowercaseChars, 2));
			password.AddRange(GetRandomChars(DigitChars, 2));
			password.AddRange(GetRandomChars(SpecialChars, 2));

			// Fill remaining characters
			string allChars = UppercaseChars + LowercaseChars + DigitChars + SpecialChars;
			password.AddRange(GetRandomChars(allChars, 4));

			// Shuffle the password
			return new string(password.OrderBy(x => random.Next()).ToArray());
		}

		private IEnumerable<char> GetRandomChars(string source, int count)
		{
			return Enumerable.Range(0, count)
						   .Select(_ => source[random.Next(source.Length)]);
		}

		public string CheckPasswordStrength(string password)
		{
			int score = 0;

			if (password.Length >= 12) score++;
			if (password.Any(char.IsUpper)) score++;
			if (password.Any(char.IsLower)) score++;
			if (password.Any(char.IsDigit)) score++;
			if (password.Any(c => SpecialChars.Contains(c))) score++;

			return score switch
			{
				5 => "Strong",
				3 or 4 => "Medium",
				_ => "Weak"
			};
		}
	}
}