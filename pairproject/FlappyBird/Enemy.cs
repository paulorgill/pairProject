using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace FlappyBird
{
	public class Enemy
	{
		
		private SpriteUV 	enemySprite;
		private TextureInfo	textureInfoUp;
		private TextureInfo	textureInfoDown;
		private TextureInfo	textureInfoRight;
		private TextureInfo	textureInfoLeft;
		private bool			enemyAlive;
		private float 		enemySpeed;
		private float width;
		private float height;
		
		public bool Alive { get{return enemyAlive;} set{enemyAlive = value;} }
					
		public Enemy (float startX, float startY, float speed, Player player ,Scene scene)
		{
			textureInfoUp  = new TextureInfo("/Application/textures/zombie.png");
			textureInfoDown  = new TextureInfo("/Application/textures/zombiedown.png");
			textureInfoRight  = new TextureInfo("/Application/textures/zombieright.png");
			textureInfoLeft  = new TextureInfo("/Application/textures/zombieleft.png");
			enemySprite	 		= new SpriteUV();
			enemySprite	 	= new SpriteUV(textureInfoDown);	
						
			enemySprite.Quad.S = textureInfoUp.TextureSizef;
			enemyAlive = true;
			enemySpeed = speed/10;
			// Get sprite bounds
			Bounds2 b = enemySprite.Quad.Bounds2();
			width = b.Point10.X;
			height = b.Point01.Y;
			
			// set enemy position
			enemySprite.Position = new Vector2 (startX, startY);
			enemySprite.CenterSprite(new Vector2(0.5f,0.5f));

			//Add to the current scene.
			scene.AddChild(enemySprite);
		}
		
		public void Dispose()
		{
			textureInfoUp.Dispose();
		}
	
		
		public void Update(Player player, Scene scene )	
		{
			if (Alive == true)
				{
			
					if (player.Alive == true)
						{
			
							if (player.Sprite.Position.X > enemySprite.Position.X)
							{
								enemySprite.Position = new Vector2(enemySprite.Position.X + enemySpeed,enemySprite.Position.Y);
								enemySprite.TextureInfo = textureInfoRight;
						
							}
					
					
								if(player.Sprite.Position.X < enemySprite.Position.X)
								{
								
						enemySprite.Position = new Vector2(enemySprite.Position.X - enemySpeed,enemySprite.Position.Y);
								enemySprite.TextureInfo = textureInfoLeft;
					
								}
					
					
					
			
					 		 if (player.Sprite.Position.Y > enemySprite.Position.Y)
							{
								enemySprite.Position = new Vector2(enemySprite.Position.X,enemySprite.Position.Y + enemySpeed);
								enemySprite.TextureInfo = textureInfoUp;
						
						
							}
					
					if (player.Sprite.Position.Y < enemySprite.Position.Y)
							{
								enemySprite.Position = new Vector2(enemySprite.Position.X ,enemySprite.Position.Y - enemySpeed);
								enemySprite.TextureInfo = textureInfoDown;
							}
					
							
					
							
					
					}
		}
			
			if (Alive == false )
			{
				//scene.AddChild(sprite);
				scene.RemoveChild(enemySprite,false );
				enemyAlive = false;
			}
		}
		
		
	
			public bool HasCollidedWithPlayer(SpriteUV sprite)
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
		
			public bool HasCollidedWithBullet(SpriteUV sprite)
			{
			Bounds2 enemy = sprite.GetlContentLocalBounds();
			enemySprite.GetContentWorldBounds(ref enemy );
			
			
			Bounds2 bullet = sprite.GetlContentLocalBounds();
			sprite.GetContentWorldBounds(ref bullet);
			
			if (bullet.Overlaps(enemy))
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

