﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maker.RiseEngine.Core.Game.GameUtils
{
    public class WorldRender
    {

        GameScene G;
        SpriteBatch tSpriteBatch;
        SpriteBatch eSpriteBatch;


        public WorldRender(GameScene _WorldScene)
        {
            G = _WorldScene;
            tSpriteBatch = new SpriteBatch(Common.GraphicsDevice);
            eSpriteBatch = new SpriteBatch(Common.GraphicsDevice);
        }



        public void Draw(GameTime gameTime, bool Blur)
        {

            if (Blur)
            {
                tSpriteBatch.Begin();
                eSpriteBatch.Begin();

            }
            else
            {

                tSpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
                eSpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
            }

            for (int Ty = G.Camera.StartTile.Y; Ty <= G.Camera.EndTile.Y; Ty++)
            {
                for (int Tx = G.Camera.StartTile.X; Tx <= G.Camera.EndTile.X; Tx++)
                {
                    if (Tx >= 0 && Ty >= 0 && Tx < G.worldProperty.Size * 16 - 1 && Ty < G.worldProperty.Size * 16 - 1)
                    {

                        //Calcule des emplacements
                        Point CurrentLocation = new Point(Tx, Ty);
                        Point OnScreenLocation = new Point(
                            (Tx - G.Camera.StartTile.X) * G.Camera.Zoom + G.Camera.ScreenOrigine.X,
                             (Ty - G.Camera.StartTile.Y) * G.Camera.Zoom + G.Camera.ScreenOrigine.Y);

                        if (G.chunkManager.PrepareChunk(CurrentLocation.ToWorldLocation().chunk.X, CurrentLocation.ToWorldLocation().chunk.Y)) {

                            //Recuperation des arguments
                            GameObject.Event.GameObjectEventArgs e = G.eventsManager.GetEventArgs(CurrentLocation.ToWorldLocation(), OnScreenLocation);

                            //recuperation des objets
                            World.ObjTile T = G.chunkManager.GetTile(CurrentLocation);


                            //desin des objets


                            GameObjectsManager.GetGameObject<GameObject.ITile>(T.ID).OnDraw(e, tSpriteBatch, gameTime);



                            if (!(T.Entity == -1))
                            {
                                World.ObjEntity E = G.chunkManager.GetEntity(CurrentLocation);
                                GameObjectsManager.GetGameObject<GameObject.IEntity>(E.ID).OnDraw(e, eSpriteBatch, gameTime);

                                if (Config.Debug.WorldOverDraw && E.IsFocus)
                                {

                                    eSpriteBatch.DrawString(ContentEngine.SpriteFont("Engine", "Consolas_16pt"), $"ID : {E.ID}\nV : {E.Variant}", OnScreenLocation.ToVector2() + new Vector2(2, 2) + E.OnTileLocation, Color.Black);
                                    eSpriteBatch.DrawString(ContentEngine.SpriteFont("Engine", "Consolas_16pt"), $"ID : {E.ID}\nV : {E.Variant}", OnScreenLocation.ToVector2() + E.OnTileLocation, Color.White);

                                }

                            }


                        }



                    }




                }
            }
            tSpriteBatch.End();
            eSpriteBatch.End();

        }
    }
}
