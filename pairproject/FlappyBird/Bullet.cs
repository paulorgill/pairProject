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
		public Texture2D texture;
		private SpriteUV 	sprite;
		private TextureInfo	textureInfo;
		private Vector2 direction, origin;
		public bool isVisible;
		float speed=12.0f;
		
		public Bullet(Scene scene)
		{
			textureInfo  = new TextureInfo("/Application/textures/bullet.png");
					
			sprite	 		= new SpriteUV();
			sprite 			= new SpriteUV(textureInfo);	
			sprite.Quad.S 	= textureInfo.TextureSizef/4;
			sprite.Position = new Vector2(-500, -500);
			sprite.CenterSprite(new Vector2(0.5f,0.5f));
			scene.AddChild(sprite);
			sprite.Visible = false;
		}
		
		public void Dispose()
		{
			textureInfo.Dispose();
		}
		
		public void ResetBullet(float x, float y) //Resets the bullet to the entered coords
		{
			sprite.Position = new Vector2(x, y);
			sprite.Visible = false;
		}
		
		public void Fire(float x, float y, float angle) //Resets the bullet to the players gun.
		{												//Works out the vector from the angle of the player.
			ResetBullet(x,y);							//Sets the bullet direction to that vector (update method moves it)
			sprite.Visible = true;						//Rotates the bullet to the appropiate angle.
			origin = new Vector2(x,y);		
			direction = Vector2FromAngle(angle-45.55f,true);
			//sprite.RotateTo(direction);
			sprite.RotationNormalize = direction;
		}
		
		public static Vector2 Vector2FromAngle(float angle, bool normalize = true)
		{
		    Vector2 vector = new Vector2((float)FMath.Cos(angle), (float)FMath.Sin(angle));
		    if (vector != Vector2.Zero && normalize) //Basic math to find out the vector from the angle
		        vector.Normalize();
		    return vector;
		}
		
//		public void Fire(Vector2 theStartPosition, Vector2 theSpeed, Vector2 theDirection)
//        {
//            position = theStartPosition;            
//            mStartPosition = theStartPosition;
//            mSpeed = theSpeed;
//            mDirection = theDirection;
//            Visible = true;
//        }
		
		public void Update(Scene scene)
		{
				sprite.Position = new Vector2(sprite.Position.X+direction.X*speed, sprite.Position.Y+direction.Y*speed);
				if(Vector2.Distance(sprite.Position,origin) > 350) //If its travelled 300 squares reset it
				{
					ResetBullet(-500, -500);
					sprite.Visible = false;
					scene.RemoveChild(sprite,false );
				}
				
			//sprite.Position = new Vector2(x+=speed, y);
			//Vector2 direction = Rotate(angle);
			//sprite.Position.X -= speed;
		}
		
		public Vector2 GetPos()
		{
			return sprite.Position;
		}
		
		public SpriteUV Sprite
		{
			get
			{
				return sprite;
			}
		}
			
			
//		const float kGap = 200.0f;
//		
//		//Private variables.
//		private SpriteUV 	sprite;
//		private TextureInfo	textureInfoTop;
//		
//		private float		width;
//		private float		height;
//		
//		//Accessors.
//		//public SpriteUV SpriteTop 	 { get{return sprites[0];} }
//		//public SpriteUV SpriteBottom { get{return sprites[1];} }
//		
//		//Public functions.
//		public Bullet (float startX, Scene scene)
//		{
//			textureInfoTop     = new TextureInfo("/Application/textures/toppipe.png");
//			
//			
//			
//			
//			//Top
//			sprite			= new SpriteUV(textureInfoTop);	
//			sprite.Quad.S 	= textureInfoTop.TextureSizef;
//			//Add to the current scene.
//			//scene.AddChild(sprites[0]);
//			
//			
//				
//			//Add to the current scene.
//			scene.AddChild(sprite);
//			
//			//Get sprite bounds.
//			Bounds2 b = sprite.Quad.Bounds2();
//			width  = b.Point10.X;
//			height = b.Point01.Y;
//			
//			//Position pipes.
//			sprite.Position = new Vector2(startX, 50.0f);
//			
//			
//		}
//		
//		public bool drawbullet()
//		{
//			
//			
//			return false;
//		}
//		
//		//public float GetY()
//		//{
//			
//		//	return sprite.Position.Y;
//		//}
//		
//		
//		public void Dispose()
//		{
//			textureInfoTop.Dispose();
//			
//		}
//		
//		public void Update(float deltaTime)
//		{			
//			
//			var touches = Touch.GetData(0);
//			GamePadData data = GamePad.GetData(0);
//			
//			if (Input2.GamePad0.Cross.Down)
//			{
//				
//			}
//			
//			
//			sprite.Position = new Vector2(sprite.Position.X - 3, sprite.Position.Y);
//			
//			//If off the left of the viewport, loop them around.
//			if(sprite.Position.X < -width)
//			{
//				sprite.Position = new Vector2(50.0f, 50.0f);
//			}		
//		}
//		
//		private float RandomPosition()
//		{
//			Random rand = new Random();
//			float randomPosition = (float)rand.NextDouble();
//			randomPosition += 0.45f;
//			
//			if(randomPosition > 1.0f)
//				randomPosition = 0.9f;
//		
//			return randomPosition;
//		}
//		
//		public bool HasCollidedWith(SpriteUV sprite)
//		{
//			return false;
//		}
	}
}

