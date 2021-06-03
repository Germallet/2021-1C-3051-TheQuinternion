﻿using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Drawers;
using TGC.MonoGame.TP.Entities;

namespace TGC.MonoGame.TP.ConcreteEntities
{
    internal class Trench_Corner : StaticPhysicEntity
    {
        protected override Drawer Drawer() => TGCGame.content.D_Trench_Corner;
        protected override Vector3 Scale => Vector3.One * DeathStar.trenchScale;
        protected override TypedIndex Shape => TGCGame.content.Sh_Trench_Corner;
    }
}