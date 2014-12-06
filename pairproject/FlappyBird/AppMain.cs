using System;
using System.Collections.Generic;
using System.Diagnostics;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.HighLevel.UI;

namespace FlappyBird
{
	public class AppMain
	{
		private static Sce.PlayStation.HighLevel.GameEngine2D.Scene 	gameScene;
		private static Sce.PlayStation.HighLevel.UI.Scene 				uiScene;
		private static Sce.PlayStation.HighLevel.UI.Label				hudLabel, timerLabel, gunLabel;
		
		private static bool 		quitGame = false;
		private static List<Enemy>  enemies;
		private static List<Bullet> bullets;
		private static Player		player;
		private static Background	background;
		private static float 		analogX, analogY, timeStamp, timeBetweenShots = 0.3f;
		private static Vector2 		playerRotation = new Vector2((0.0f),(0.0f)), playerMovement = new Vector2((0.0f),(0.0f)); 
		private static int			score = 0, lives = 3, level = 1, bulletsLeft = 16;
		private static Timer		seconds;
							
		public static void Main (string[] args)
		{
			Initialize();
			
			//Game loop
			while (!quitGame) 
			{
				Update ();
				
				Director.Instance.Update();
				Director.Instance.Render();
				UISystem.Render();
				
				Director.Instance.GL.Context.SwapBuffers();
				Director.Instance.PostSwap();
			}
			
			//Clean up after ourselves.
			player.Dispose();
						
			background.Dispose();
			
			foreach(Enemy enemys in enemies)
				enemys.Dispose();
			
			foreach(Bullet bullet in bullets)
				bullet.Dispose();
			
			Director.Terminate ();
		}

		public static void Initialize ()
		{
			//Set up director and UISystem.
			Director.Initialize ();
			UISystem.Initialize(Director.Instance.GL.Context);
			
			//Set game scene
			gameScene = new Sce.PlayStation.HighLevel.GameEngine2D.Scene();
			gameScene.Camera.SetViewFromViewport();
			
			//Set the ui scene.
			uiScene = new Sce.PlayStation.HighLevel.UI.Scene();
			Panel panel  = new Panel();
			panel.Width  = Director.Instance.GL.Context.GetViewport().Width;
			panel.Height = Director.Instance.GL.Context.GetViewport().Height;
			
			//Setup the HUD label (score, lives, level)
			hudLabel = new Sce.PlayStation.HighLevel.UI.Label();
			hudLabel.HorizontalAlignment = HorizontalAlignment.Left;
			hudLabel.VerticalAlignment = VerticalAlignment.Top;
			hudLabel.Width = panel.Width;
			hudLabel.SetPosition(0,0);
			//hudLabel.TextTrimming = TextTrimming.None;
			hudLabel.Text = "Score: " + score + " 		Lives: " + lives + " 		Level: " + level;
			panel.AddChildLast(hudLabel);
			
			//Setup the Time label (time)
			timerLabel = new Sce.PlayStation.HighLevel.UI.Label();
			timerLabel.HorizontalAlignment = HorizontalAlignment.Right;
			timerLabel.VerticalAlignment = VerticalAlignment.Top;
			timerLabel.Width = panel.Width;
			timerLabel.SetPosition(0,0);
			timerLabel.Text = "Time Survived: 0 secs";
			panel.AddChildLast(timerLabel);
			
			//Setup the Gun label (time)
			gunLabel = new Sce.PlayStation.HighLevel.UI.Label();
			gunLabel.HorizontalAlignment = HorizontalAlignment.Center;
			gunLabel.VerticalAlignment = VerticalAlignment.Bottom;
			gunLabel.Width = panel.Width;
			gunLabel.SetPosition(0,0);
			gunLabel.Text = "Bullets Left: " + bulletsLeft;
			panel.AddChildLast(gunLabel);
			
			//Add the panel to the UISystem
			uiScene.RootWidget.AddChildLast(panel);
			UISystem.SetScene(uiScene);
			
			//Create the background.
			background = new Background(gameScene);
			
			//Create the player
			player = new Player(gameScene);
			
			//Create the enemy list and spawn one at (0,0)
			enemies = new List<Enemy>();
			Enemy enemy = new Enemy(0.0f, 0.0f, player, gameScene);
			enemies.Add(enemy);
					
			//Create the bullet
			bullets = new List<Bullet>();
			Bullet bullet = new Bullet(gameScene);
			bullets.Add(bullet);
				
			//Create the timer
			seconds = new Timer();
				
			//Run the scene.
			Director.Instance.RunWithScene(gameScene, true);
		}
		
		public static void Update()
		{
			//Determine whether the player tapped the screen
			var touches = Touch.GetData(0);
			GamePadData data = GamePad.GetData(0);
						
			//Move the player using basic boolean logic
			if (Input2.GamePad0.Up.Down)
			{
				analogY = -1.0f;
			}
				else
			{
				analogY = 0.0f;
			}
			
			if (Input2.GamePad0.Left.Down)
			{
				analogX = -1.0f;
			}
				else
			{
				analogX = 0.0f;
			}
			
			if (Input2.GamePad0.Right.Down)
				analogX = 1.0f;
					
			if (Input2.GamePad0.Down.Down)
				analogY = 1.0f;
			
			if (Input2.GamePad0.Square.Down) //Spawn enemies ('A' on keyboard)
			{
				if (enemies.Count < 15) //Max number of enemies
				{
					Random r = new Random();
					bool spawn = false; //If random coords are near the player pick another set
					while(spawn == false)
					{
						float tempX = r.Next(1,1500); //Random coords between 1 and 1500
						float tempY = r.Next(1,1500);
						Vector2 spawnPoint = new Vector2(tempX,tempY);
						if(Vector2.Distance(player.GetPos(), spawnPoint) > 200) // Can't spawn on the player
						{
							Enemy enemy = new Enemy(spawnPoint.X, spawnPoint.Y, player, gameScene);
							enemies.Add(enemy); //Spawn enemy at random coords
							spawn = true; //Exit the loop
						}
					}
				}
			}
			
			if (Input2.GamePad0.R.Down)
			{
				if (seconds.Seconds() >= timeStamp) //For automatic firing the fire rate is set by ensuring that
				{									//the time difference between the previous shot and this is equal					
					Bullet bullet = new Bullet(gameScene); //to the hard coded fire rate.
					bullet.Fire(player.GetX(), player.GetY(), player.GetAngle());
					bullets.Add(bullet);
					bulletsLeft = bulletsLeft - 1;
					timeStamp = (float)seconds.Seconds() + timeBetweenShots;
				}
			}
		
			//rotate according to the right analog stick, or if it's not moving, then according the the left stick
			//so basically if you are not pointing the player in any direction with the right stick he is going to point in the walking direction
			//or if both sticks are not moving,then use the analogX and analogY values(d-pad movement)
			if (data.AnalogRightX > 0.2f || data.AnalogRightX < -0.2f || data.AnalogRightY > 0.2f || data.AnalogRightY < -0.2f) 
			{
				var angleInRadians = FMath.Atan2 (-data.AnalogRightX, -data.AnalogRightY);
				var angleInRadians2 = FMath.Atan2 (-data.AnalogLeftX, -data.AnalogLeftY);
				playerRotation = new Vector2 (FMath.Cos (angleInRadians), FMath.Sin (angleInRadians));
				playerMovement = new Vector2 (FMath.Cos (angleInRadians2), FMath.Sin (angleInRadians2));
			} 
			else if (data.AnalogLeftX > 0.2f || data.AnalogLeftX < -0.2f || data.AnalogLeftY > 0.2f || data.AnalogLeftY < -0.2f) 
			{
				var angleInRadians = FMath.Atan2 (-data.AnalogLeftX, -data.AnalogLeftY);
				playerRotation = new Vector2 (FMath.Cos (angleInRadians), FMath.Sin (angleInRadians));
				playerMovement = playerRotation;
			} 
			else if(analogX != 0.0f || analogY != 0.0f)
			{
				var angleInRadians = FMath.Atan2 (-analogX, -analogY);
				playerRotation = new Vector2 (FMath.Cos (angleInRadians), FMath.Sin (angleInRadians));
			}
						
			//North, east, south, west are changed via the d-pad and playerrotation via the sticks
			if (Input2.GamePad0.AnalogLeft.Length() > 0.1f)
			{
				player.UpdateUsingANALOG(playerMovement, playerRotation, gameScene);
			}
			else
				player.UpdateUsingDPAD(analogX, analogY, playerRotation, gameScene);
						
			//Move the bullet if its being fired, if not nothing happens
			for (int i = bullets.Count - 1; i >= 0; i--) 
			{
				bullets[i].Update(gameScene);
			}
			
			//Update all enemies in the list
			for (int i = enemies.Count - 1; i >= 0; i--) 
			{
				enemies[i].Update(player, gameScene);
			}
			
			if (player.Alive == true) //Constantly focus the camera on the players coordinates
			{	
				gameScene.Camera2D.SetViewY(new Vector2(0.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f), player.GetPos());
			}
			
			if(player.Alive)
			{
				for (int i = enemies.Count - 1; i >= 0; i--) 
				{
					if (enemies[i].Alive ) //Check all enemies for collisions with player or bullet
					{
						if (enemies[i].HasCollidedWithPlayer (player.Sprite) == true)
						{
							lives = lives - 1;
							player.Alive = false; 
						}
						for (int j = bullets.Count - 1; j >= 0; j--) 
						{
							if (enemies[i].HasCollidedWithBullet (bullets[j].Sprite) == true)
							{
								score = score + 1;
								enemies[i].Alive = false; //Remove the enemy from the scene 
								enemies[i].Update(player, gameScene);
								enemies.RemoveAt(i); //Remove the enemy from the list
								bullets[j].ResetBullet(-500,-500);
							}
							Vector2 bulletPosition = bullets[j].GetPos(); 			
							if(bulletPosition.X == -500 && bulletPosition.Y == -500)
							{
								bullets.RemoveAt(j);
							}
						}
					}
				}
			}
					
			//Update the UI
			hudLabel.Text = "Score: " + score + " 		Lives: " + lives + " 		Level: " + level;
			timerLabel.Text = "Time Survived: " + (int)seconds.Seconds() + " secs";
			gunLabel.Text = "Bullets Left: " + bulletsLeft;
		}
		
	}
}
