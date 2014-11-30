using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace FlappyBird
{
	public class Enemy
	{
		
		private static SpriteUV 	enemySprite;
		private static TextureInfo	textureInfoUp;
		private static TextureInfo	textureInfoDown;
		private static TextureInfo	textureInfoRight;
		private static TextureInfo	textureInfoLeft;
		private static bool			enemyAlive;
		private static float 		enemySpeed = 0.5f;
		private float width;
		private float height;
		
		
		
		public Enemy ( Player player ,Scene scene)
		{
			textureInfoUp  = new TextureInfo("/Application/textures/zombie.png");
			textureInfoDown  = new TextureInfo("/Application/textures/zombiedown.png");
			textureInfoRight  = new TextureInfo("/Application/textures/zombieright.png");
			textureInfoLeft  = new TextureInfo("/Application/textures/zombieleft.png");
			enemySprite	 		= new SpriteUV();
			enemySprite	 	= new SpriteUV(textureInfoDown);	
			
			enemySprite.Quad.S = textureInfoUp.TextureSizef;
			enemyAlive = true;
			
			// Get sprite bounds
			Bounds2 b = enemySprite.Quad.Bounds2();
			width = b.Point10.X;
			height = b.Point01.Y;
			
			// set enemy position
			enemySprite.Position = new Vector2 (50.0f,200.0f);

			//Add to the current scene.
			scene.AddChild(enemySprite);
		}
		
		public void Dispose()
		{
			textureInfoUp.Dispose();
		}
	
		
		public void Update(Player player )	
		{
			if (player.Alive == true)
		{
			
			if (player.Sprite.Position.X > enemySprite.Position.X)
			{
				enemySprite.Position = new Vector2(enemySprite.Position.X + enemySpeed,enemySprite.Position.Y);
			
			}
			
			else if (player.Sprite.Position.X < enemySprite.Position.X)
			{
				enemySprite.Position = new Vector2(enemySprite.Position.X - enemySpeed,enemySprite.Position.Y);
			}
			
			if (player.Sprite.Position.Y > enemySprite.Position.Y)
			{
				enemySprite.Position = new Vector2(enemySprite.Position.X,enemySprite.Position.Y + enemySpeed);
			
			}
			
			else if (player.Sprite.Position.Y < enemySprite.Position.Y)
			{
				enemySprite.Position = new Vector2(enemySprite.Position.X ,enemySprite.Position.Y - enemySpeed);
			}
		
		}
		}
		
	
			public bool HasCollidedWith(SpriteUV sprite)
		{
			Bounds2 enemy = sprite.GetlContentLocalBounds();
			enemySprite.GetContentWorldBounds(ref enemy );
			
			
			
			Bounds2 player = sprite.GetlContentLocalBounds();
			sprite.GetContentWorldBounds(ref player);
			
			if (player.Overlaps(enemy))
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

