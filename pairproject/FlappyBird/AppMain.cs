using System;
using System.Collections.Generic;
using System.Diagnostics;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Audio;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.HighLevel.UI;

namespace FlappyBird
{
	public class AppMain
	{
		private static Sce.PlayStation.HighLevel.GameEngine2D.Scene 	gameScene;
		private static Sce.PlayStation.HighLevel.UI.Scene 				uiScene;
		private static Sce.PlayStation.HighLevel.UI.Label				hudLabel, timerLabel, gunLabel, reloadLabel, enemiesLabel, gameOverLabel, respawnLabel;
		
		private static bool 		reloading = false, quitGame = false, nextLevelVoice = false, ismenu = true, isgame = false, inGameOver = false, newLevel = true;
		private static List<Enemy>  enemies;
		private static List<Bullet> bullets;
		private static Player		player;
		private static Background	background;
		private static Menu         menu;
		private static Sound		soundBullet, bgmusic, clickSound, nextLevelSound;
		private static SoundPlayer 	bulletPlayer, bgmusicPlayer, clickPlayer, nextLevelPlayer;
		private static float 		analogX, analogY, timeStamp, timeStamp2, timeStamp3, timeBetweenShots = 0.2f, reloadTime = 3.0f;
		private static Vector2 		playerRotation = new Vector2((0.0f),(0.0f)), playerMovement = new Vector2((0.0f),(0.0f)); 
		private static int			score = 0, lives = 3, level = 1, bulletsLeft = 20, enemiesRemaining = 0;
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
			hudLabel.Visible = false; 
			panel.AddChildLast(hudLabel);
			
			//Setup the Time label (time)
			timerLabel = new Sce.PlayStation.HighLevel.UI.Label();
			timerLabel.HorizontalAlignment = HorizontalAlignment.Right;
			timerLabel.VerticalAlignment = VerticalAlignment.Top;
			timerLabel.Width = panel.Width;
			timerLabel.SetPosition(0,0);
			timerLabel.Text = "Time Survived: 0 secs";
			timerLabel.Visible = false;
			panel.AddChildLast(timerLabel);
			
			//Setup the Gun label (time)
			gunLabel = new Sce.PlayStation.HighLevel.UI.Label();
			gunLabel.HorizontalAlignment = HorizontalAlignment.Center;
			gunLabel.VerticalAlignment = VerticalAlignment.Bottom;
			gunLabel.Width = panel.Width;
			gunLabel.SetPosition(0,0);
			gunLabel.Text = "Bullets Left: " + bulletsLeft;
			gunLabel.Visible = false; 
			panel.AddChildLast(gunLabel);
			
			//Setup the reload label (instructs user to reload)
			reloadLabel = new Sce.PlayStation.HighLevel.UI.Label();
			reloadLabel.HorizontalAlignment = HorizontalAlignment.Center;
			reloadLabel.VerticalAlignment = VerticalAlignment.Middle;
			reloadLabel.Width = panel.Width;
			reloadLabel.SetPosition(Director.Instance.GL.Context.GetViewport().Width/35, Director.Instance.GL.Context.GetViewport().Height/2.5f);
			reloadLabel.Text = "Press X to reload!";
			reloadLabel.Visible = false;
			panel.AddChildLast(reloadLabel);
			
			//Setup the gameover label 
			gameOverLabel = new Sce.PlayStation.HighLevel.UI.Label();
			gameOverLabel.HorizontalAlignment = HorizontalAlignment.Center;
			gameOverLabel.VerticalAlignment = VerticalAlignment.Middle;
			gameOverLabel.Width = panel.Width;
			gameOverLabel.SetPosition(Director.Instance.GL.Context.GetViewport().Width/5, Director.Instance.GL.Context.GetViewport().Height/2.5f);
			gameOverLabel.Text = "GAME OVER";
			gameOverLabel.Visible = false;
			panel.AddChildLast(gameOverLabel);
			
			//Setup the press to respawn label 
			respawnLabel = new Sce.PlayStation.HighLevel.UI.Label();
			respawnLabel.HorizontalAlignment = HorizontalAlignment.Center;
			respawnLabel.VerticalAlignment = VerticalAlignment.Middle;
			respawnLabel.Width = panel.Width;
			respawnLabel.SetPosition(Director.Instance.GL.Context.GetViewport().Width/35, Director.Instance.GL.Context.GetViewport().Height/5.5f);
			respawnLabel.Text = "Press triangle to respawn";
			respawnLabel.Visible = false;
			panel.AddChildLast(respawnLabel);
			
			//Setup the enemies left label
			enemiesLabel = new Sce.PlayStation.HighLevel.UI.Label();
			enemiesLabel.HorizontalAlignment = HorizontalAlignment.Left;
			enemiesLabel.VerticalAlignment = VerticalAlignment.Bottom;
			enemiesLabel.Width = panel.Width;
			enemiesLabel.SetPosition(0.0f, Director.Instance.GL.Context.GetViewport().Height/1.05f);
			enemiesLabel.Text = "Enemies left: " + enemiesRemaining;
			enemiesLabel.Visible = false;
			panel.AddChildLast(enemiesLabel);
			
			//Add the panel to the UISystem
			uiScene.RootWidget.AddChildLast(panel);
			UISystem.SetScene(uiScene);
				
			//Create the background.
			background = new Background(gameScene);
			
			//Create the player
			player = new Player(gameScene);
			
			//Create the enemy list and spawn one at (0,0)
			enemies = new List<Enemy>();
								
			//Create the bullet
			bullets = new List<Bullet>();
			Bullet bullet = new Bullet(gameScene);
			bullets.Add(bullet);
				
			//Create the timer
			seconds = new Timer();
			
			//Create sounds and their respective soundplayers
			soundBullet = new Sound("/Application/sounds/shoot2.wav");
			bulletPlayer = soundBullet.CreatePlayer();
			bgmusic = new Sound("/Application/sounds/bgm.wav");
			bgmusicPlayer = bgmusic.CreatePlayer();
			clickSound = new Sound("/Application/sounds/click.wav");
			clickPlayer = clickSound.CreatePlayer();
			nextLevelSound = new Sound("/Application/sounds/chaching.wav");
			nextLevelPlayer = nextLevelSound.CreatePlayer();
			
			//Create menu screen
			if (ismenu == true )
			{
				menu = new Menu(gameScene);
			}
						
			//Run the scene.
			bgmusicPlayer.Play(); //Start the BGM
			Director.Instance.RunWithScene(gameScene, true);
		}
				
		public static void ResetGame(int finalScore) //Resets the game
		{
			reloading = false;
			quitGame = false;
			nextLevelVoice = false;
			ismenu = true;
			isgame = false;
			inGameOver = false;
			newLevel = true;
			score = 0;
			lives = 3;
			level = 1;
			bulletsLeft = 20;
			enemiesRemaining = 0;
			timeStamp = 0;
			timeStamp2 = 0;
			timeStamp3 = 0;
			gameOverLabel.Visible =true;
			background.NextMap(level);
			gameOverLabel.Text = "GAME OVER       Score: " + finalScore.ToString() + "";
			bgmusicPlayer.Play();
			menu.Update(true);
		}
				
		public static void Update()
		{
			//Determine whether the player tapped the screen
			var touches = Touch.GetData(0);
			GamePadData data = GamePad.GetData(0);
			
			// Starts the game once the player has tapped the screen
			if (ismenu == true)
			{
				if (touches.Count > 0 )
				{
					ismenu = false ;
					isgame = true;	
					inGameOver = false;
					
					seconds.Reset();	
				}
			}
			
			// Removes the menu from the screen
			if (isgame == true)
			{
				menu.Update(false);
					
				//Makes the UI visible to the player
				timerLabel.Visible = true;
				hudLabel.Visible = true; 
				gunLabel.Visible = true;
				enemiesLabel.Visible = true;
				gameOverLabel.Visible = false;
				
				if (newLevel) // if new level is true
				{
					for(int i = 0; i < 5 * level; i++) //5 more enemies each level
					{
						if (seconds.Seconds() >= timeStamp2)
						{
							Random r = new Random();
							bool spawn = false; //If random coords are near the player pick another set
							while(spawn == false)
							{
								float tempX = r.Next(1,2000); //Random coords between 1 and 1500
								float tempY = r.Next(1,2000);
								float tempSpeed = r.Next(5,20); //Vary the speed (divide by 10 in the enemy method)
								Vector2 spawnPoint = new Vector2(tempX,tempY);
								if((Vector2.Distance(player.GetPos(), spawnPoint) > 250) && (Vector2.Distance(player.GetPos(), spawnPoint) < 1000)) // Can't spawn on the player or too far
								{
									Enemy enemy = new Enemy(spawnPoint.X, spawnPoint.Y, tempSpeed, player, gameScene);
									enemies.Add(enemy); //Spawn enemy at random coords
									enemiesRemaining++;
									spawn = true; //Exit the loop
								}
							}
							timeStamp2 = (float)seconds.Seconds() + 0.05f; 
						} //If you create new instances too close in time, they will produce the same 
						//series of random numbers as the random generator is seeded from the system clock.
					}
				}
				
				timerLabel.Visible = true;
				hudLabel.Visible = true; 
				gunLabel.Visible = true;
								
				// creates the game over screen
				if (lives ==0 )
				{
					inGameOver = true;
					
					if (inGameOver == true)
					{
						//ismenu = false ;
						//isgame = false;
						//gameOverScreen = new GameOver(player.Sprite.Position.X, player.Sprite.Position.Y,gameScene);
						
						timerLabel.Visible = false;
						hudLabel.Visible = false; 
						gunLabel.Visible = false;
						enemiesLabel.Visible = false;
						respawnLabel.Visible = false;
						
						for (int i = enemies.Count - 1; i >= 0; i--)
						{
							enemies[i].Alive = false;
							enemies[i].Update(player, gameScene);
							enemies.RemoveAt(i);
						}
						
						player.Alive = true;
						player.SetPlayer(50.0f, 50.0f);
						ResetGame(score);		
						isgame=false;
					}	
				}
					//ismenu = false ;
					//isgame = true;
					//quitGame = true; 
								
				if (enemiesRemaining == level*5)
					newLevel = false; //Stop spawning enemies
				
				if (enemiesRemaining == 0 && (!nextLevelVoice) && (lives != 0) && (!newLevel)) //All enemies are dead
				{
					nextLevelVoice = true;
					nextLevelPlayer.Play();
					reloadLabel.Text = "Prepare for the next level!";
					reloadLabel.Visible = true;
					timeStamp3 = (float)seconds.Seconds() + 2f;
				}
													
				if ((seconds.Seconds() >= timeStamp3) && nextLevelVoice) 
				{
					level++; //Next level
					reloadLabel.Visible = false;
					newLevel = true;
					nextLevelVoice = false;
					background.NextMap(level); //New map texture
				}
				
				//Move the player using basic boolean logic
				if (Input2.GamePad0.Up.Down)
					analogY = -1.0f;
				else
					analogY = 0.0f;
							
				if (Input2.GamePad0.Left.Down)
					analogX = -1.0f;
				else
					analogX = 0.0f;
							
				if (Input2.GamePad0.Right.Down)
					analogX = 1.0f;
						
				if (Input2.GamePad0.Down.Down)
					analogY = 1.0f;
							
				if (Input2.GamePad0.Triangle.Down && inGameOver == false && player.Alive == false) //Respawnbutton
				{
					player.Alive = true;
					respawnLabel.Visible = false;
					player.Sprite.Visible = true; 
				}
										
				if (Input2.GamePad0.R.Down)
				{
					if(bulletsLeft> 0 && (!reloading)) //Can't shoot whilst reloading
					{
						if (seconds.Seconds() >= timeStamp) //For automatic firing the fire rate is set by ensuring that
						{									//the time difference between the previous shot and this is equal					
							Bullet bullet = new Bullet(gameScene); //to the hard coded fire rate.
							bullet.Fire(player.GetX(), player.GetY(), player.GetAngle());
							bulletPlayer.Play();
							bullets.Add(bullet);
							bulletsLeft = bulletsLeft - 1;
							timeStamp = (float)seconds.Seconds() + timeBetweenShots; //Set the time for next shot
						}
					}
					else
					{
						if (!reloading)
						{
						reloadLabel.Text = "Press X to reload!";
						reloadLabel.Visible = true;
						}
					}
				}
				
				if (Input2.GamePad0.Cross.Down) //Reload button
				{
					reloadLabel.Visible = true;
					reloading = true;
					reloadLabel.Text = "Reloading...";
					timeStamp = (float)seconds.Seconds() + reloadTime; //Set the time for when reload is done
				}
				
				if (reloading)
				{
					if (seconds.Seconds() >= timeStamp) //When elapsed time reaches the set reload finish time
					{
						reloadLabel.Text = "Press X to reload!";
						reloadLabel.Visible = false;
						reloading = false;
						clickPlayer.Play();
						bulletsLeft = 20; //Reload magazine
					}
				}
			
				//Rotate according to the right analog stick, or if it's not moving, then according the the left stick.
				//So basically if you are not pointing the player in any direction with the right stick he is going to point in the walking direction
				//or if both sticks are not moving, then use the analogX and analogY values(D-PAD movement)
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
					player.UpdateUsingANALOG(playerMovement, playerRotation, gameScene);
				else
					player.UpdateUsingDPAD(analogX, analogY, playerRotation, gameScene);
							
				//Move the bullet if its being fired, if not nothing happens
				for (int i = bullets.Count - 1; i >= 0; i--) 
					bullets[i].Update(gameScene);
								
				//Update all enemies in the list
				for (int i = enemies.Count - 1; i >= 0; i--) 
					enemies[i].Update(player, gameScene);
								
				if(player.Alive) 
				{
					//Camera. Focus on player. Don't let the camera show any off map area. If the player walks near the edge
					//leave the edge of the camera on the edge of the map but let the player walk to the actual map edge.
					//If the player isn't within screenwidth/2 or screen height/2 of a edge of the map then center on the
					//player.
					if ((player.GetX() < Director.Instance.GL.Context.GetViewport().Width*0.5f) || (player.GetX() > 2000f - Director.Instance.GL.Context.GetViewport().Width*0.5f) ||
					    (player.GetY() < Director.Instance.GL.Context.GetViewport().Height*0.5f) || (player.GetY() > 2000f - Director.Instance.GL.Context.GetViewport().Height*0.5f))
					{
						if (player.GetX() < Director.Instance.GL.Context.GetViewport().Width*0.5f) //Near left side
							gameScene.Camera2D.SetViewY(new Vector2(0.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f), new Vector2(Director.Instance.GL.Context.GetViewport().Width*0.5f, player.GetY()));
						if (player.GetX() > 2000f - Director.Instance.GL.Context.GetViewport().Width*0.5f) //Near right side
							gameScene.Camera2D.SetViewY(new Vector2(0.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f), new Vector2(2000f - Director.Instance.GL.Context.GetViewport().Width*0.5f, player.GetY()));
						if (player.GetY() < Director.Instance.GL.Context.GetViewport().Height*0.5f) //Near bottom side
							gameScene.Camera2D.SetViewY(new Vector2(0.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f), new Vector2(player.GetX(), Director.Instance.GL.Context.GetViewport().Height*0.5f));
						if (player.GetY() > 2000.0f - Director.Instance.GL.Context.GetViewport().Height*0.5f) //Near top side
							gameScene.Camera2D.SetViewY(new Vector2(0.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f), 
							                            new Vector2(player.GetX(), 2000.0f - Director.Instance.GL.Context.GetViewport().Height*0.5f));
						if ((player.GetX() < Director.Instance.GL.Context.GetViewport().Width*0.5f) && (player.GetY() < Director.Instance.GL.Context.GetViewport().Height*0.5f)) //Near bottom left corner
							gameScene.Camera2D.SetViewY(new Vector2(0.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f),
							                            new Vector2(Director.Instance.GL.Context.GetViewport().Width*0.5f, Director.Instance.GL.Context.GetViewport().Height*0.5f));
						if ((player.GetX() < Director.Instance.GL.Context.GetViewport().Width*0.5f) && (player.GetY() > 2000.0f - Director.Instance.GL.Context.GetViewport().Height*0.5f)) //Near top left corner
							gameScene.Camera2D.SetViewY(new Vector2(0.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f),
							                            new Vector2(Director.Instance.GL.Context.GetViewport().Width*0.5f, 2000.0f - Director.Instance.GL.Context.GetViewport().Height*0.5f));
						if ((player.GetX() > 2000.0f - Director.Instance.GL.Context.GetViewport().Width*0.5f) && (player.GetY() > 2000.0f - Director.Instance.GL.Context.GetViewport().Height*0.5f)) //Near top right corner
							gameScene.Camera2D.SetViewY(new Vector2(0.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f),
							                            new Vector2(2000.0f - Director.Instance.GL.Context.GetViewport().Width*0.5f, 2000.0f - Director.Instance.GL.Context.GetViewport().Height*0.5f));
						if ((player.GetX() > 2000.0f -  Director.Instance.GL.Context.GetViewport().Width*0.5f) && (player.GetY() < Director.Instance.GL.Context.GetViewport().Height*0.5f)) //Near bottom right corner
							gameScene.Camera2D.SetViewY(new Vector2(0.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f),
							                            new Vector2(Director.Instance.GL.Context.GetViewport().Width*0.5f, Director.Instance.GL.Context.GetViewport().Height*0.5f));
					}
					else
						gameScene.Camera2D.SetViewY(new Vector2(0.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f), player.GetPos()); //Player not near an edge
						
					//Create map boundaries
					if (player.GetX() < 0.0f) //Left side boundary
						player.SetPlayer(0.0f, player.GetY());
					if (player.GetX() > 2000.0f) //Right side boundary
						player.SetPlayer(2000.0f, player.GetY());
					if (player.GetY() < 0.0f) //Bottom side boundary
						player.SetPlayer(player.GetX(), 0.0f);
					if (player.GetY() > 2000.0f) //Top side boundary
						player.SetPlayer(player.GetX(), 2000.0f);
						
					for (int i = enemies.Count - 1; i >= 0; i--) //Check all enemies for collisions with player or bullet
					{
						if (enemies[i].Alive ) 
						{
							if (enemies[i].HasCollidedWithPlayer (player.Sprite) == true) //Colision between player and enemies
							{
								if (lives>0)
									lives = lives - 1;
								player.Alive = false; 
								respawnLabel.Visible = true;
							}
							for (int j = bullets.Count - 1; j >= 0; j--) 
							{
								if (enemies[i].HasCollidedWithBullet (bullets[j].Sprite) == true)
								{
									score = score + 1;
									enemies[i].Alive = false; //Remove the enemy from the scene 
									enemies[i].Update(player, gameScene);
									enemies.RemoveAt(i); //Remove the enemy from the list
									enemiesRemaining--;
									//bullets[j].ResetBullet(-500,-500);
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
				enemiesLabel.Text = "Enemies left: " + enemiesRemaining;
			}
		}
	}
}
