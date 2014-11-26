using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace FlappyBird
{
	public class Enemy
	{
		
		private static SpriteUV 	sprite;
		private static TextureInfo	textureInfoUp;
		private static TextureInfo	textureInfoDown;
		private static TextureInfo	textureInfoRight;
		private static TextureInfo	textureInfoLeft;
		private static bool			enemyAlive;
<<<<<<< HEAD
		private static float 		enemySpeed = 3.0f;
=======
		private static float 		enemySpeed = 0.5f;
>>>>>>> origin/master
		private float width;
		private float height;
		
		
		
		public Enemy ( Player player ,Scene scene)
		{
			textureInfoUp  = new TextureInfo("/Application/textures/zombie.png");
			textureInfoDown  = new TextureInfo("/Application/textures/zombiedown.png");
			textureInfoRight  = new TextureInfo("/Application/textures/zombieright.png");
			textureInfoLeft  = new TextureInfo("/Application/textures/zombieleft.png");
			sprite	 		= new SpriteUV();
			sprite 			= new SpriteUV(textureInfoDown);	
			sprite.Quad.S 	= textureInfoUp.TextureSizef;
			enemyAlive = true;
			
			// set enemy position
			sprite.Position = new Vector2 (50.0f,200.0f);
<<<<<<< HEAD
			
=======

>>>>>>> origin/master
			//Add to the current scene.
			scene.AddChild(sprite);
			
			// Get sprite bounds
			Bounds2 b = sprite.Quad.Bounds2();
			width = b.Point10.X;
			height = b.Point01.Y;
			
		}
		
		public void Dispose()
		{
			textureInfoUp.Dispose();
		}
	
		
<<<<<<< HEAD
		public void Update()
		{
			sprite.Position = new Vector2(sprite.Position.X - enemySpeed, sprite.Position.Y );
			
			if (sprite.Position.X < -width)
			{
				sprite.Position = new Vector2(Director.Instance.GL.Context.GetViewport().Width,
			                              Director.Instance.GL.Context.GetViewport().Height);
			
			}
			
=======
		public void Update(Player player )	
		{
			//player.Sprite.Position.X
			//sprite.Position = new Vector2(sprite.Position.X - enemySpeed, sprite.Position.Y );
			
			if (player.Sprite.Position.X > sprite.Position.X)
			{
				sprite.Position = new Vector2(sprite.Position.X + enemySpeed,sprite.Position.Y);
			
			}
			
			else if (player.Sprite.Position.X < sprite.Position.X)
			{
				sprite.Position = new Vector2(sprite.Position.X - enemySpeed,sprite.Position.Y);
			}
			
			if (player.Sprite.Position.Y > sprite.Position.Y)
			{
				sprite.Position = new Vector2(sprite.Position.X,sprite.Position.Y + enemySpeed);
			
			}
			
			else if (player.Sprite.Position.Y < sprite.Position.Y)
			{
				sprite.Position = new Vector2(sprite.Position.X ,sprite.Position.Y - enemySpeed);
			}
			
>>>>>>> origin/master
		}
		
		public bool HasCollidedWith(SpriteUV sprite)
		{
			Bounds2 zombie = sprite.GetlContentLocalBounds();
			sprite.GetContentWorldBounds(ref zombie);
			
			Bounds2 player = sprite.GetlContentLocalBounds();
			sprite.GetContentWorldBounds(ref player);
			
			if (player.Overlaps(zombie))
			{
				return true; 
			}
			
			
			else 
			{
				return false;
			}
		}
		
		
		
		
	}
}

