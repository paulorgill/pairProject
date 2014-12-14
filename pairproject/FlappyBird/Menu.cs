using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace FlappyBird
{
	public class Menu
	{
		//Private variables.
		public static SpriteUV 	sprite;
		private static TextureInfo	textureInfo;
		
		private static bool			alive;
		
		public bool Alive { get{return alive;} set{alive = value;} }
		
		//Accessors.
		//public SpriteUV Sprite { get{return sprite;} }
		
		//Public functions.
		public Menu (Scene scene)
		{
			textureInfo  = new TextureInfo("/Application/textures/menuScreen.png");
			
			sprite	 		= new SpriteUV();
			sprite 			= new SpriteUV(textureInfo);	
			sprite.Quad.S 	= textureInfo.TextureSizef;
			sprite.Position = new Vector2(0.0f,0.0f);
			//sprite.Pivot 	= new Vector2(0.5f,0.5f);
			
			
			//Add to the current scene.
			scene.AddChild(sprite);
		}
		
		public void Dispose()
		{
			textureInfo.Dispose();
		}
		
		public void Update(bool visible)
		{			
			
			
			sprite.Visible = visible;
			
		}	
		
		
		
		
		
	}
}
