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
		
		private static Bullet		bullet;
		private static Player			player;
		private static Background	background;
		private static bool				North, South, East, West;
		
				
		public static void Main (string[] args)
		{
			Initialize();
			
			//Game loop
			bool quitGame = false;
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
			scoreLabel.SetPosition(
				Director.Instance.GL.Context.GetViewport().Width/2 - scoreLabel.Width/2,
				Director.Instance.GL.Context.GetViewport().Height*0.1f - scoreLabel.Height/2);
			scoreLabel.Text = "0";
			panel.AddChildLast(scoreLabel);
			uiScene.RootWidget.AddChildLast(panel);
			UISystem.SetScene(uiScene);
			
			//Create the background.
			background = new Background(gameScene);
			
			//Create the flappy douche
			player = new Player(gameScene);
			
			
			//bullet = new Bullet( 50.0f, gameScene);	
			
			
			//Run the scene.
			Director.Instance.RunWithScene(gameScene, true);
		}
		
		public static void Update()
		{
			//Determine whether the player tapped the screen
			var touches = Touch.GetData(0);
			GamePadData data = GamePad.GetData(0);
			
			
			//Move the player
			if (Input2.GamePad0.Up.Down)
				North = true;
			else
				North = false;
			
			if (Input2.GamePad0.Left.Down)
				West = true;
			else
				West = false;
			
			if (Input2.GamePad0.Right.Down)
				East = true;
			else
				East = false;
			
			if (Input2.GamePad0.Down.Down)
				South = true;
			else
				South = false;
				
			player.Update(North, East, South, West);
			
			
//			Vector2 direction = targetPosition - currentPosition;
//			direction.Normalize();
//			float rotationInRadians = (float)Math.Atan2((double)direction.Y, (double)direction.X) + MathHelper.PiOver2;
			
			gameScene.Camera2D.SetViewY(new Vector2(0.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f), player.GetPos());
			
			//Update the bird.
			//player.Update(0.0f);
			
			if(player.Alive)
			{
				//Move the background.
				background.Update(0.0f);
							
				//Update the obstacles.
				//bullet.Update(0.0f);
			
			}
		}
		
	}
}
