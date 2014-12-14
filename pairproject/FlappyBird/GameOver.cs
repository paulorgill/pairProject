using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace FlappyBird
{
	public class GameOver
	{
		//Private variables.
		public static SpriteUV 	gameoversprite;
		private static TextureInfo	textureInfo;
		
		private static bool			alive;
		
		public bool Alive { get{return alive;} set{alive = value;} }
		
		//Accessors.
		//public SpriteUV Sprite { get{return sprite;} }
		
		//Public functions.
		public GameOver (float startX, float startY,Scene scene)
		{
			
			textureInfo  = new TextureInfo("/Application/textures/menuScreen.png");
			
			gameoversprite	 		= new SpriteUV();
			gameoversprite 			= new SpriteUV(textureInfo);	
			gameoversprite.Quad.S 	= textureInfo.TextureSizef;
			gameoversprite.Position = new Vector2(Director.Instance.GL.Context.GetViewport().Width, Director.Instance.GL.Context.GetViewport().Height/1.05f);
			//sprite.Pivot 	= new Vector2(0.5f,0.5f);
			
			
			//Add to the current scene.
			scene.AddChild(gameoversprite);
		}
		
		public void Dispose()
		{
			textureInfo.Dispose();
		}
		
		public void Update(float startx, float starty, float deltaTime)
		{			
			
			
			//gameoversprite.Visible = true;
			gameoversprite.Position = new Vector2(startx, starty);

		}	
		
		public void Hide()
		{			
			
			
			gameoversprite.Visible = false;
			
		}	
		
		
		
		
	}
}
