﻿using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Entities;

namespace TGC.MonoGame.TP.ConcreteEntities
{
    internal class Trench : StaticPhysicEntity<Sphere>
    {
        protected override Model Model() => TGCGame.content.M_Trench;
        protected override Texture2D[] Textures() => TGCGame.content.T_Trench;
        protected override Vector3 Scale => Vector3.One * DeathStar.trenchScale / 100f;
        protected override Sphere Shape => new Sphere(20f);
    }
}