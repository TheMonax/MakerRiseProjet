﻿using Maker.RiseEngine.Core.Game.WorldDataStruct;
using Maker.RiseEngine.Core.GameObject.Event;
using Maker.RiseEngine.Core.Rendering.SpriteSheets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Maker.twiyol.GameObject.Entities
{
    public class Entity : IEntity
    {

        public string gameObjectName { get; set; }
        public string pluginName { get; set; }

        //Drawing Property
        public Rectangle DrawBox;
        public List<Sprite> Variant;

        //Determine la position de l'entiter sur la case
        public Vector2 SpriteLocation = new Vector2(0, -1f);

        public int MaxVariantCount { get; set; }

        public int MoveSpeed { get; set; } = 5;

        public int MaxLife { get; set; } = 20;

        public int MoveRunSpeed { get; set; } = 10;

        public bool canTakeDamage { get; set; } = false;

        public bool canBeKilled { get; set; } = false;

        public Entity(string[] _SpriteVariant, string _SpriteSheet, Vector2 _SpriteLocation)
        {
            Variant = new List<Rendering.SpriteSheets.Sprite>();

            foreach (string str in _SpriteVariant)
            {
                Variant.Add(GameObjectsManager.GetGameObject<Rendering.SpriteSheets.SpriteSheet>(_SpriteSheet.Split('.')[0], _SpriteSheet.Split('.')[1]).GetSprite(str));
            }
            SpriteLocation = _SpriteLocation;
            DrawBox = new Rectangle(Point.Zero, new Point(Variant[0].sprites[0].Width, Variant[0].sprites[0].Height));
            this.MaxVariantCount = Variant.Count - 1;
        }

        public void OnDraw(GameObjectEventArgs e, SpriteBatch spritebatch, GameTime gametime)
        {
            Variant[e.ParrentEntity.Variant].Draw(spritebatch, new Rectangle(
                   e.OnScreenLocation.X + (int)(e.World.Camera.Zoom * (this.SpriteLocation.X + e.ParrentEntity.OnTileLocationX)),
                   e.OnScreenLocation.Y + (int)(e.World.Camera.Zoom * (this.SpriteLocation.Y + +e.ParrentEntity.OnTileLocationY)),
                   this.DrawBox.Width * e.World.Camera.Zoom, this.DrawBox.Height * e.World.Camera.Zoom),Color.White, gametime);
        }

        public virtual void OnTick(GameObjectEventArgs e, GameTime gametime)
        {
            
        }

        public virtual void OnUpdate(GameObjectEventArgs e, KeyboardState keyboard, MouseState mouse, GameTime gametime)
        {

        }

        public void OnEntityInteract(GameObjectEventArgs eThisEntity, GameObjectEventArgs eInteratingEntity)
        {
            
        }

        public void OnGameObjectAdded()
        {
        }

        public float damage = 5;
        public float GetDamage(GameObjectEventArgs e)
        {
            return damage;
        }

        public float defence = 0;
        public float GetDefence(GameObjectEventArgs e)
        {
            return defence;
        }

        public void OnDamageTaken(GameObjectEventArgs e)
        {

        }

        public void OnEntityDestroy(GameObjectEventArgs e)
        {

        }

        public void OnEntityKilled(GameObjectEventArgs e, DataEntity entityKills)
        {
            e.World.EntityDataManager.RemoveEntityData(e.CurrentLocation);
        }
    }
}