using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace FlappyBird
{
	public class Background
	{	
		//Private variables.
		private SpriteUV[,] 	sprites;
		private TextureInfo	textureInfo, textureInfo2, textureInfo3;
		
		//Public functions.
		public Background (Scene scene)
		{
			sprites	= new SpriteUV[3,3];
			
			textureInfo  		= new TextureInfo("/Application/textures/a1.png");
			textureInfo2  		= new TextureInfo("/Application/textures/a2.png");
			textureInfo3  		= new TextureInfo("/Application/textures/a4.png");
			
			for (int i = 0; i < 3; ++i)
			{
    			for (int j = 0; j < 3; ++j)
				{
					sprites[i,j] 			= new SpriteUV(textureInfo);
					sprites[i,j].Quad.S 		= textureInfo.TextureSizef;
					sprites[i,j].Position = new Vector2(1000.0f*i, 1000.0f*j);
				}
			}
			//Add to the current scene.
			foreach(SpriteUV sprite in sprites)
				scene.AddChild(sprite);
		}	
		
		
		public void Dispose()
		{
			textureInfo.Dispose();
		}
		
		public void NextMap(int level)
		{
			if (level == 2)
			{
				for (int i = 0; i < 3; ++i)
				{
	    			for (int j = 0; j < 3; ++j)
					{
						sprites[i,j].TextureInfo = textureInfo2;
					}
				}
			}
			else if (level == 3)
			{
				for (int i = 0; i < 3; ++i)
				{
	    			for (int j = 0; j < 3; ++j)
					{
						sprites[i,j].TextureInfo = textureInfo3;
					}
				}
			}
			
			
		}
		
		public void Update(Vector2 playerPos)
		{	
			//Only display squares within 200 of the player
			
					
			//sprites[0].Position = new Vector2(sprites[0].Position.X 0.5f, sprites[0].Position.Y);
			///sprites[1].Position = new Vector2(sprites[1].Position.X - 0.5f, sprites[1].Position.Y);
			//sprites[2].Position = new Vector2(sprites[2].Position.X - 0.5f, sprites[2].Position.Y);
			
			//Move the background.
			//Left
			//if(sprites[0].Position.X < -width)
			//	sprites[0].Position = new Vector2(sprites[2].Position.X+width, 0.0f);
			//else
			//	sprites[0].Position = new Vector2(sprites[0].Position.X-1, 0.0f);	
			
			//Middle
			//if(sprites[1].Position.X < -width)
			//	sprites[1].Position = new Vector2(sprites[0].Position.X+width, 0.0f);
			//else
			//	sprites[1].Position = new Vector2(sprites[1].Position.X-1, 0.0f);	
			
			//Right
			//if(sprites[2].Position.X < -width)
			//	sprites[2].Position = new Vector2(sprites[1].Position.X+width, 0.0f);
			//else
			//	sprites[2].Position = new Vector2(sprites[2].Position.X-1, 0.0f);	
		}
	}
}

