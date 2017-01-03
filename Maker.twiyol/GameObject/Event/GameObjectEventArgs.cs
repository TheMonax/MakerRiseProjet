﻿using Maker.twiyol.Game.GameUtils;
using Microsoft.Xna.Framework;

namespace Maker.twiyol.GameObject.Event
{
    public class GameObjectEventArgs
    {

        public Game.WorldDataStruct.DataEntity ParrentEntity;
        public Game.WorldDataStruct.DataTile ParrentTile;

        public WorldLocation CurrentLocation;
        public Game.GameScene World;

        public Point OnScreenLocation;

    }
}
