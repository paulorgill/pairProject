using System;
using System.Collections.Generic;

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
		private static Sce.PlayStation.HighLevel.UI.Label				scoreLabel;
		
		private static bool North, South, East, West, firing = false, quitGame = false;
		private static Bullet		bullet;
		private static Player		player;
		private static Background	background;
		private static Enemy		enemy;
		private static float 		analogX, analogY;
		private static Vector2 		playerRotation = new Vector2((0.0f),(0.0f));
				
		public static void Main (string[] args)
		{
			Initialize();
			
			//Game loop
			//bool quitGame = false;
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
			//foreach(Obstacle obstacle in obstacles)
				//obstacle.Dispose();
			background.Dispose();
			
			enemy.Dispose();
			
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
			scoreLabel = new Sce.PlayStation.HighLevel.UI.Label();
			scoreLabel.HorizontalAlignment = HorizontalAlignment.Center;
			scoreLabel.VerticalAlignment = VerticalAlignment.Top;
			scoreLabel.Width = panel.Width;
			scoreLabel.SetPosition(0,Director.Instance.GL.Context.GetViewport().Height*0.1f - scoreLabel.Height/2);
			scoreLabel.TextTrimming = TextTrimming.None;
			scoreLabel.Text = "WATCH OUT FOR THE ZOMBIE!";
			panel.AddChildLast(scoreLabel);
			uiScene.RootWidget.AddChildLast(panel);
			UISystem.SetScene(uiScene);
			
			//Create the background.
			background = new Background(gameScene);
			
			//Create the player
			player = new Player(gameScene);
			
			//Create the enemy
			enemy = new Enemy( player, gameScene);
			
			//Create the bullet
			bullet = new Bullet(gameScene);
			
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
				North = true;
				analogY = 1.0f;
			}
				else
			{
				North = false;
				analogY = 0.0f;
			}
			
			if (Input2.GamePad0.Left.Down)
			{
				West = true;
				analogX = -1.0f;
			}
				else
			{
				West = false;
				analogX = 0.0f;
			}
			
			if (Input2.GamePad0.Right.Down)
			{
				East = true;
				analogX = 1.0f;
			}
				else
			{
				East = false;
				//analogX = 0.0f;
			}
			
			if (Input2.GamePad0.Down.Down)
			{
				South = true;
				analogY = -1.0f;
			}
				else
			{
				South = false;
				//analogY = 0.0f;
			}
			
			if (Input2.GamePad0.Square.Down)
			{
				if (!firing) //Enable firing to make the bullet fire immediately once pressed instead of released.
				{			 //Whilst it is firing you cannot fire until the firing process is complete
					bullet.Fire(player.GetX(), player.GetY(), player.GetAngle());
					firing = true;
				}
			}
			
			Vector2 bulletPos = bullet.GetPos(); 			//Once the bullet has finished firing or hits something it
			if(bulletPos.X == -500 && bulletPos.Y == -500)	//goes off-screen to (-500,-500) and the player can fire
				firing = false;								//once again.
			    			
			//rotate according to the right analog stick, or if it's not moving, then according the the left stick
			//so basically if you are not pointing the player in any direction with the right stick he is going to point in the walking direction
			//or if both sticks are not moving,then use the analogX and analogY values(d-pad movement)
			if (data.AnalogRightX > 0.2f || data.AnalogRightX < -0.2f || data.AnalogRightY > 0.2f || data.AnalogRightY < -0.2f) 
			{
				var angleInRadians = FMath.Atan2 (-data.AnalogRightX, -data.AnalogRightY);
				playerRotation = new Vector2 (FMath.Cos (angleInRadians), FMath.Sin (angleInRadians));
			} 
			else if (data.AnalogLeftX > 0.2f || data.AnalogLeftX < -0.2f || data.AnalogLeftY > 0.2f || data.AnalogLeftY < -0.2f) 
			{
				var angleInRadians = FMath.Atan2 (-data.AnalogLeftX, -data.AnalogLeftY);
				playerRotation = new Vector2 (FMath.Cos (angleInRadians), FMath.Sin (angleInRadians));
			} 
			else if(analogX != 0.0f || analogY != 0.0f)
			{
				var angleInRadians = FMath.Atan2 (-analogX, -analogY);
				playerRotation = new Vector2 (FMath.Cos (angleInRadians), FMath.Sin (angleInRadians));
			}
						
			//North, east, south, west are changed via the d-pad and playerrotation via the sticks
			player.Update(North, East, South, West, playerRotation, gameScene);
						
			//Move the bullet if its being fired, if not do nothing
			bullet.Update();
						
			enemy.Update(enemy, player, gameScene);
			
			if (player.Alive == true)
			{	//Constantly focus the camera on the players coordinates
				gameScene.Camera2D.SetViewY(new Vector2(0.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f), player.GetPos());
			}
			
			if(player.Alive)
			{
				//Move the background.
				//background.Update(0.0f);
				if (enemy.Alive )
				{
					if (enemy.HasCollidedWithPlayer (player.Sprite) == true)
					{
						scoreLabel.Text = "1";
						player.Alive = false; 
					}
					
					if (enemy.HasCollidedWithBullet (bullet.Sprite) == true)
					{
						scoreLabel.Text = "1";
						enemy.Alive = false; 
					}
				}
			}
		}
		
	}
}
