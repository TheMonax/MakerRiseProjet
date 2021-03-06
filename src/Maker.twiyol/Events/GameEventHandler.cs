﻿using Maker.Twiyol.Game;
using Maker.Twiyol.Game.WorldDataStruct;
using Maker.Twiyol.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maker.Twiyol.Events
{
    public static class GameEventHandler
    {
        public static event EventHandler OnWorldGeneratingBegin;
        public static event EventHandler OnWorldGeneratingEnd;
        public static void RaiseOnWorldGeneratingEnd(object sender, DataWorld world, WorldGenerator worldGenerator) {
            OnWorldGeneratingEnd?.Invoke(sender, new WorldEventArgs(world, worldGenerator));
        }
        public static void RaiseOnWorldGeneratingBegin(object sender, DataWorld world, WorldGenerator worldGenerator)
        {
            OnWorldGeneratingBegin?.Invoke(sender, new WorldEventArgs(world, worldGenerator));
        }

    }
}
