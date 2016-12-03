﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maker.RiseEngine.Core.World.WorldObj
{

    public enum chunkStatutList { Done, onDecoration, needDecoration}

    [Serializable]
    public class ObjChunk
    {
        public Dictionary<int, ObjEntity> Entities = new Dictionary<int, ObjEntity>();
        public ObjTile[,] Tiles = new ObjTile[16, 16];


        public chunkStatutList chunkStatut = chunkStatutList.needDecoration; //cette valeur est passé à true quand le chunk a été décoré.

        public void AddEntity(ObjEntity _Entity, Point Location)
        {

                int EntityID = (Location.Y * 16) + Location.X;
                this.Entities.Add(EntityID, _Entity);
                this.Tiles[Location.X, Location.Y].Entity = EntityID;
            

            
        }
    }
}
