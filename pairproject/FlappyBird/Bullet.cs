using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace FlappyBird
{
	public class Bullet
	{
		const float kGap = 200.0f;
		
		//Private variables.
		private SpriteUV 	sprite;
		private TextureInfo	textureInfoTop;
		
		private float		width;
		private float		height;
		
		//Accessors.
		//public SpriteUV SpriteTop 	 { get{return sprites[0];} }
		//public SpriteUV SpriteBottom { get{return sprites[1];} }
		
		//Public functions.
		public Bullet (float startX, Scene scene)
		{
			textureInfoTop     = new TextureInfo("/Application/textures/toppipe.png");
			
			
			
			
			//Top
			sprite			= new SpriteUV(textureInfoTop);	
			sprite.Quad.S 	= textureInfoTop.TextureSizef;
			//Add to the current scene.
			//scene.AddChild(sprites[0]);
			
			
				
			//Add to the current scene.
			scene.AddChild(sprite);
			
			//Get sprite bounds.
			Bounds2 b = sprite.Quad.Bounds2();
			width  = b.Point10.X;
			height = b.Point01.Y;
			
			//Position pipes.
			sprite.Position = new Vector2(startX, 50.0f);
			
			
		}
		
		public bool drawbullet()
		{
			
			
			return false;
		}
		
		//public float GetY()
		//{
			
		//	return sprite.Position.Y;
		//}
		
		
		public void Dispose()
		{
			textureInfoTop.Dispose();
			
		}
		
		public void Update(float deltaTime)
		{			
			
			var touches = Touch.GetData(0);
			GamePadData data = GamePad.GetData(0);
			
			if (Input2.GamePad0.Cross.Down)
			{
				
			}
			
			
			sprite.Position = new Vector2(sprite.Position.X - 3, sprite.Position.Y);
			
			//If off the left of the viewport, loop them around.
			if(sprite.Position.X < -width)
			{
				sprite.Position = new Vector2(50.0f, 50.0f);
			}		
		}
		
		private float RandomPosition()
		{
			Random rand = new Random();
			float randomPosition = (float)rand.NextDouble();
			randomPosition += 0.45f;
			
			if(randomPosition > 1.0f)
				randomPosition = 0.9f;
		
			return randomPosition;
		}
		
		public bool HasCollidedWith(SpriteUV sprite)
		{
			return false;
		}
	}
}

