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
		private TextureInfo	textureInfo, textureInfo2, textureInfo3, textureInfo4, textureInfo5, textureInfo6, textureInfo7, textureInfo8, textureInfo9, textureInfo10;
		
		//Public functions.
		public Background (Scene scene)
		{
			sprites	= new SpriteUV[2,2];
			
			textureInfo  		= new TextureInfo("/Application/textures/a1.png");
			textureInfo2  		= new TextureInfo("/Application/textures/a2.png");
			textureInfo3  		= new TextureInfo("/Application/textures/a3.png");
			textureInfo4  		= new TextureInfo("/Application/textures/a4.png");
			textureInfo5  		= new TextureInfo("/Application/textures/a5.png");
			textureInfo6  		= new TextureInfo("/Application/textures/a6.png");
			textureInfo7  		= new TextureInfo("/Application/textures/a7.png");
			textureInfo8  		= new TextureInfo("/Application/textures/a8.png");
			textureInfo9  		= new TextureInfo("/Application/textures/a9.png");
			textureInfo10  		= new TextureInfo("/Application/textures/a10.png");
			
			for (int i = 0; i < 2; ++i)
			{
    			for (int j = 0; j < 2; ++j)
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
			textureInfo2.Dispose();
			textureInfo3.Dispose();
			textureInfo4.Dispose();
			textureInfo5.Dispose();
			textureInfo6.Dispose();
			textureInfo7.Dispose();
			textureInfo8.Dispose();
			textureInfo9.Dispose();
			textureInfo10.Dispose();
			
		}
		
		public void NextMap(int level)
		{
			if (level == 1)
			{
				for (int i = 0; i < 2; ++i)
				{
	    			for (int j = 0; j < 2; ++j)
					{
						sprites[i,j].TextureInfo = textureInfo;
					}
				}
			}
			else if (level == 2)
			{
				for (int i = 0; i < 2; ++i)
				{
	    			for (int j = 0; j < 2; ++j)
					{
						sprites[i,j].TextureInfo = textureInfo2;
					}
				}
			}
			else if (level == 3)
			{
				for (int i = 0; i < 2; ++i)
				{
	    			for (int j = 0; j < 2; ++j)
					{
						sprites[i,j].TextureInfo = textureInfo3;
					}
				}
			}
			else if (level == 4)
			{
				for (int i = 0; i < 2; ++i)
				{
	    			for (int j = 0; j < 2; ++j)
					{
						sprites[i,j].TextureInfo = textureInfo4;
					}
				}
			}
			else if (level == 5)
			{
				for (int i = 0; i < 2; ++i)
				{
	    			for (int j = 0; j < 2; ++j)
					{
						sprites[i,j].TextureInfo = textureInfo5;
					}
				}
			}
			else if (level == 6)
			{
				for (int i = 0; i < 2; ++i)
				{
	    			for (int j = 0; j < 2; ++j)
					{
						sprites[i,j].TextureInfo = textureInfo6;
					}
				}
			}
			else if (level == 7)
			{
				for (int i = 0; i < 2; ++i)
				{
	    			for (int j = 0; j < 2; ++j)
					{
						sprites[i,j].TextureInfo = textureInfo7;
					}
				}
			}
			else if (level == 8)
			{
				for (int i = 0; i < 2; ++i)
				{
	    			for (int j = 0; j < 2; ++j)
					{
						sprites[i,j].TextureInfo = textureInfo8;
					}
				}
			}
			else if (level == 9)
			{
				for (int i = 0; i < 2; ++i)
				{
	    			for (int j = 0; j < 2; ++j)
					{
						sprites[i,j].TextureInfo = textureInfo9;
					}
				}
			}
			else if (level == 10)
			{
				for (int i = 0; i < 2; ++i)
				{
	    			for (int j = 0; j < 2; ++j)
					{
						sprites[i,j].TextureInfo = textureInfo10;
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

