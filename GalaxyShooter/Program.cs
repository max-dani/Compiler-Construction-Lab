using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GalaxyShooterDSL
{
	class Program
	{
		static void Main(string[] args)
		{
			Game game = new Game();
			game.Start();
		}
	}

	// Represents the main game mechanics
	public class Game
	{
		private Ship player;
		private List<Bullet> bullets;
		private List<Enemy> enemies;
		private List<EnemyBullet> enemyBullets;
		private int score;
		private bool isRunning;
		private const int screenWidth = 50;
		private const int screenHeight = 20;

		// Speed modifiers
		private double bulletSpeedModifier = 1.0;
		private double enemySpeedModifier = 1.0;
		private double gameSpeedModifier = 1.0;

		public Game()
		{
			player = new Ship(screenWidth / 2, screenHeight - 2); // Initialize player at the center bottom
			bullets = new List<Bullet>();
			enemies = new List<Enemy>();
			enemyBullets = new List<EnemyBullet>();
			score = 0;
			isRunning = true;
			Console.SetWindowSize(screenWidth, screenHeight);
			Console.CursorVisible = false;
		}

		public void Start()
		{
			// Add some initial enemies
			AddEnemies(5);

			while (isRunning)
			{
				DrawScreen();
				HandleInput();
				UpdateGame();
				Thread.Sleep((int)(50 / gameSpeedModifier)); // Adjust game speed based on modifier
			}

			// Game Over
			Console.Clear();
			Console.SetCursorPosition(screenWidth / 2 - 4, screenHeight / 2);
			Console.WriteLine("GAME OVER");
			Console.SetCursorPosition(screenWidth / 2 - 7, screenHeight / 2 + 1);
			Console.WriteLine("Final Score: " + score);
			Console.ReadKey();
		}

		// Draw all game elements
		private void DrawScreen()
		{
			Console.Clear();
			// Draw player
			Console.SetCursorPosition(player.X, player.Y);
			Console.Write("A");

			// Draw player bullets
			foreach (var bullet in bullets)
			{
				Console.SetCursorPosition(bullet.X, bullet.Y);
				Console.Write("|");
			}

			// Draw enemy bullets
			foreach (var bullet in enemyBullets)
			{
				Console.SetCursorPosition(bullet.X, bullet.Y);
				Console.Write("!");
			}

			// Draw enemies
			foreach (var enemy in enemies)
			{
				Console.SetCursorPosition(enemy.X, enemy.Y);
				Console.Write("0_0");
			}

			// Display score
			Console.SetCursorPosition(0, 0);
			Console.WriteLine($"Score: {score}");

			// Display remaining lives
			Console.SetCursorPosition(0, 1);
			Console.WriteLine($"Lives: {player.Lives}");
		}

		// Handle player input for moving and shooting
		private void HandleInput()
		{
			if (Console.KeyAvailable)
			{
				var key = Console.ReadKey(intercept: true).Key;

				if (key == ConsoleKey.A && player.X > 0) // Move left
				{
					player.X--;
				}
				else if (key == ConsoleKey.D && player.X < screenWidth - 1) // Move right
				{
					player.X++;
				}
				else if (key == ConsoleKey.Spacebar) // Shoot
				{
					bullets.Add(new Bullet(player.X, player.Y - 1));
				}
			}
		}

		// Update game logic
		private void UpdateGame()
		{
			// Move player bullets
			for (int i = bullets.Count - 1; i >= 0; i--)
			{
				var bullet = bullets[i];
				bullet.Y -= (int)(1 * bulletSpeedModifier); // Move bullets based on speed modifier

				// Remove bullet if it's out of bounds
				if (bullet.Y < 0)
				{
					bullets.RemoveAt(i);
					continue;
				}

				// Check for collisions with enemies
				for (int j = enemies.Count - 1; j >= 0; j--)
				{
					var enemy = enemies[j];
					if (bullet.X == enemy.X && bullet.Y == enemy.Y)
					{
						// Enemy hit
						bullets.RemoveAt(i);
						enemies.RemoveAt(j);
						score += 10; // Increase score
						AdjustGameSpeed(); // Increase game speed as score increases
						break;
					}
				}
			}

			// Move enemies left or right
			foreach (var enemy in enemies)
			{
				// Move enemies horizontally
				enemy.X += enemy.Direction * (int)(1 * enemySpeedModifier); // Direction determines movement

				// Bounce enemies off the screen edges
				if (enemy.X <= 0 || enemy.X >= screenWidth - 1)
				{
					enemy.Direction *= -1; // Change direction when hitting the wall
				}

				// Make the enemy shoot at intervals
				if (new Random().Next(0, 20) == 0)
				{
					enemyBullets.Add(new EnemyBullet(enemy.X, enemy.Y + 1));
				}
			}

			// Move enemy bullets
			for (int i = enemyBullets.Count - 1; i >= 0; i--)
			{
				var bullet = enemyBullets[i];
				bullet.Y += 1; // Move bullets downward

				// Remove bullet if it's out of bounds
				if (bullet.Y >= screenHeight)
				{
					enemyBullets.RemoveAt(i);
					continue;
				}

				// Check for collision with the player
				if (bullet.X == player.X && bullet.Y == player.Y)
				{
					player.Lives--; // Decrease player's life
					enemyBullets.RemoveAt(i);

					// If player has no lives left, end game
					if (player.Lives <= 0)
					{
						isRunning = false;
					}
				}
			}

			// Spawn new enemies randomly
			if (new Random().Next(0, 20) == 0)
			{
				AddEnemies(1);
			}
		}

		// Adjust the speed of the game as the score increases
		private void AdjustGameSpeed()
		{
			if (score % 50 == 0) // Every 50 points, increase the speed
			{
				bulletSpeedModifier += 0.1; // Increase bullet speed
				enemySpeedModifier += 0.1; // Increase enemy speed
				gameSpeedModifier += 0.1; // Make the game loop run faster
			}
		}

		// Add enemies at the top
		private void AddEnemies(int count)
		{
			for (int i = 0; i < count; i++)
			{
				var enemyX = new Random().Next(1, screenWidth - 1);
				enemies.Add(new Enemy(enemyX, 0)); // Add new enemies at the top
			}
		}
	}

	// Represents the player's ship
	public class Ship
	{
		public int X { get; set; }
		public int Y { get; set; }
		public int Lives { get; set; }

		public Ship(int x, int y)
		{
			X = x;
			Y = y;
			Lives = 3; // Default lives
		}
	}

	// Represents a bullet shot by the player
	public class Bullet
	{
		public int X { get; set; }
		public int Y { get; set; }

		public Bullet(int x, int y)
		{
			X = x;
			Y = y;
		}
	}

	// Represents a bullet shot by an enemy
	public class EnemyBullet
	{
		public int X { get; set; }
		public int Y { get; set; }

		public EnemyBullet(int x, int y)
		{
			X = x;
			Y = y;
		}
	}

	// Represents an enemy plane
	public class Enemy
	{
		public int X { get; set; }
		public int Y { get; set; }
		public int Direction { get; set; } // Direction controls left or right movement

		public Enemy(int x, int y)
		{
			X = x;
			Y = y;
			Direction = new Random().Next(0, 2) == 0 ? 1 : -1; // Randomize initial direction (left or right)
		}
	}
}