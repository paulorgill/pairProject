using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace FlappyBird
{
	public class Player
	{
		//Private variables.
		private static SpriteUV 	sprite;
		private static TextureInfo	textureInfo;
		private static TextureInfo	textureInfo1;
		
		private static bool			alive;
		private static float 		speed = 3.0f;
		public float 				angle = 0.0f;
		
		public bool Alive { get{return alive;} set{alive = value;} }
		
		//Accessors.
		//public SpriteUV Sprite { get{return sprite;} }
		
		//Public functions.
		public Player (Scene scene)
		{
			textureInfo  = new TextureInfo("/Application/textures/playertest2.png");
					
			sprite	 		= new SpriteUV();
			sprite 			= new SpriteUV(textureInfo);	
			sprite.Quad.S 	= textureInfo.TextureSizef;
			sprite.Position = new Vector2(100.0f,100.0f);
			sprite.Pivot 	= new Vector2(0.5f,0.5f);
			alive = true;
			
			//Add to the current scene.
			scene.AddChild(sprite);
		}
			
		public void Dispose()
		{
			textureInfo.Dispose();
		}
		
//		public void Update(float deltaTime)
//		{			
//			//adjust the push
//			if(up)
//			{
//				//sprite.Rotate(0.008f);
//				//if( (sprite.Position.Y-yPositionBeforePush) < pushAmount)
//				
//				
//					sprite.Position = new Vector2(sprite.Position.X ,sprite.Position.Y + 3f);
//				//else
//					up = false;
//			}
//			
//			if(down)
//			{
//				//sprite.Rotate(0.008f);
//				//if( (sprite.Position.Y-yPositionBeforePush) < pushAmount)
//					sprite.Position = new Vector2(sprite.Position.X ,sprite.Position.Y - 3f);
//				//else
//					down = false;
//			}
//			
//			if(left)
//			{
//				//sprite.Rotate(0.008f);
//				//if( (sprite.Position.Y-yPositionBeforePush) < pushAmount)
//					sprite.Position = new Vector2(sprite.Position.X -3 ,sprite.Position.Y);
//				//else
//					left = false;
//			}
//			
//			if(right)
//			{
//				sprite.Position = new Vector2 (sprite.Position.X + 3, sprite.Position.Y );
//				right = false; 
//			}
//			
//			
//		
//		}	
		
		public void Update(bool n, bool e, bool s, bool w)
		{
			if (n == true)
			{
				sprite.Position = new Vector2(sprite.Position.X, sprite.Position.Y + speed);
			}
			else
			{
				sprite.Position = new Vector2(sprite.Position.X, sprite.Position.Y);
			}
			if (s == true)	
			{
				sprite.Position = new Vector2(sprite.Position.X, sprite.Position.Y - speed);
			}
			else
			{
				sprite.Position = new Vector2(sprite.Position.X, sprite.Position.Y);
			}
			if (w == true)
			{
				sprite.Position = new Vector2(sprite.Position.X - speed, sprite.Position.Y);
			}
			else
			{
				sprite.Position = new Vector2(sprite.Position.X, sprite.Position.Y);
			}
			if (e == true)
			{
				sprite.Position = new Vector2(sprite.Position.X + speed, sprite.Position.Y);
			}
			else
			{
				sprite.Position = new Vector2(sprite.Position.X, sprite.Position.Y);
			
			}
		}
		
		public Vector2 GetPos()
		{
			return sprite.Position;
		}
		
		public float GetX()
		{
			return sprite.Position.X;
		}
		
		public float GetY()
		{
			return sprite.Position.Y;
		}
		
		
		
//		public void TappedUp()
//		{
//			if(!up)
//			{
//				up = true;
//				//yPositionBeforePush = sprite.Position.Y;
//			}
//		}
//		
//		public void TappedDown()
//		{
//			if(!down)
//			{
//				down = true;
//				//yPositionBeforePush = sprite.Position.Y;
//			}
//		}
//		
//		public void Tappedleft()
//		{
//			if(!left)
//			{
//				left= true;
//				//yPositionBeforePush = sprite.Position.Y;
//			}
//		}
//		
//		public void TappedRight()
//		{
//			if(!right)
//			{
//				right = true;
//				//yPositionBeforePush = sprite.Position.Y;
//			}
//		}
	}
}

