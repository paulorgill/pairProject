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
			
		private static bool			alive;
		private static float 		speed = 3.5f;
		private static float 		rotationAngle = 0.0f, movementAngle = 0.0f;
		private static Vector2 		movementVector;
		
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
			sprite.Position = new Vector2(500.0f,500.0f);
			sprite.CenterSprite(new Vector2(0.5f,0.5f));
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
		public void UpdateUsingANALOG(Vector2 playerMovement, Vector2 playerRotation, Scene scene)
		{
			rotationAngle = -(float)FMath.Atan2(playerRotation.X, playerRotation.Y); //Math to find the angle
			sprite.Angle = rotationAngle-45.555f; //-45 because of how the player sprite is loaded
			
			movementAngle = -(float)FMath.Atan2(playerMovement.X, playerMovement.Y); //Has to be different to rotation
			movementVector = Vector2FromAngle(movementAngle-91.1f,true);			 //since player can move and rotate
			//Move the sprite along the movement vector								 //at once.
			sprite.Position = new Vector2(sprite.Position.X+(movementVector.X*speed), sprite.Position.Y+(movementVector.Y*speed));
			
			if (Alive == false )
			{
				//scene.AddChild(sprite);
				//scene.RemoveChild(sprite,false );
				//Player.alive = false; 
			}
			
			//else 
			//{//
			//	scene.AddChild(sprite)
			//}
			
		}
		
		public void SetPlayer(float x, float y) //Sets the player to the entered coords
		{
			sprite.Position = new Vector2(x, y);
		}
		
		public void UpdateUsingDPAD(float playerDirectionX, float playerDirectionY, Vector2 playerRotation, Scene scene)
		{
			rotationAngle = -(float)FMath.Atan2(playerRotation.X, playerRotation.Y);
			sprite.Angle = rotationAngle-45.555f; //Same as before
						
			if (playerDirectionY == -1.0f) //North
			{
				sprite.Position = new Vector2(sprite.Position.X, sprite.Position.Y + speed);
			}
			else
			{
				sprite.Position = new Vector2(sprite.Position.X, sprite.Position.Y);
			}
			if (playerDirectionY == 1.0f)	//South
			{
				sprite.Position = new Vector2(sprite.Position.X, sprite.Position.Y - speed);
			}
			else
			{
				sprite.Position = new Vector2(sprite.Position.X, sprite.Position.Y);
			}
			if (playerDirectionX == -1.0f) //West
			{
				sprite.Position = new Vector2(sprite.Position.X - speed, sprite.Position.Y);
			}
			else
			{
				sprite.Position = new Vector2(sprite.Position.X, sprite.Position.Y);
			}
			if (playerDirectionX == 1.0f) //East
			{
				sprite.Position = new Vector2(sprite.Position.X + speed, sprite.Position.Y);
			}
			else
			{
				sprite.Position = new Vector2(sprite.Position.X, sprite.Position.Y);
			}
								
//			if (n == true)
//			{
//				sprite.Position = new Vector2(sprite.Position.X, sprite.Position.Y + speed);
//			}
//			else
//			{
//				sprite.Position = new Vector2(sprite.Position.X, sprite.Position.Y);
//			}
//			if (s == true)	
//			{
//				sprite.Position = new Vector2(sprite.Position.X, sprite.Position.Y - speed);
//			}
//			else
//			{
//				sprite.Position = new Vector2(sprite.Position.X, sprite.Position.Y);
//			}
//			if (w == true)
//			{
//				sprite.Position = new Vector2(sprite.Position.X - speed, sprite.Position.Y);
//			}
//			else
//			{
//				sprite.Position = new Vector2(sprite.Position.X, sprite.Position.Y);
//			}
//			if (e == true)
//			{
//				sprite.Position = new Vector2(sprite.Position.X + speed, sprite.Position.Y);
//			}
//			else
//			{
//				sprite.Position = new Vector2(sprite.Position.X, sprite.Position.Y);
//			}
			
			if (Alive == false )
			{
				float tempX;
				float tempY;
				if (sprite.Position.Y > 500.0f)
				{
					tempY = (50.0f);
					
				}
				else
				{
					tempY = (1000.0f);
				}
				if (sprite.Position.X > 500.0f)
				{
					tempX = (50.0f);
				}
				else
				{
					tempX = (1000.0f);
				}
												
				sprite.Position = new Vector2(tempX, tempY);

				//sprite.Position = new Vector2(100.0f, 100.0f);
				//scene.RemoveChild(sprite,false );
				Player.alive = false; 
				sprite.Visible = false;
			}
			
			if (Alive == true )
			{
				sprite.Visible = true;
				
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
		
		public float GetAngle()
		{
			return sprite.Angle;
		}
		
		public SpriteUV Sprite
		{
			get
			{
				return sprite;
			}
		}
		
		public static Vector2 Vector2FromAngle(float angle, bool normalize = true)
		{
		    Vector2 vector = new Vector2((float)FMath.Cos(angle), (float)FMath.Sin(angle));
		    if (vector != Vector2.Zero && normalize)
		        vector.Normalize();
		    return vector;
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

