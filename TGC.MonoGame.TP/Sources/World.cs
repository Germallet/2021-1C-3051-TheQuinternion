﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using TGC.MonoGame.TP.ConcreteEntities;
using TGC.MonoGame.TP.Drawers;
using TGC.MonoGame.TP.Entities;

namespace TGC.MonoGame.TP
{
    internal class World
    {
        int RandomNumber = 0;
        Random Random = new Random();

        private readonly List<Entity> pendingEntities = new List<Entity>();
        private readonly List<Entity> entities = new List<Entity>();
        private readonly List<Entity> removedEntities = new List<Entity>();
        internal XWing xwing;

        internal void Register(Entity entity) => pendingEntities.Add(entity);

        internal void Unregister(Entity entity) => removedEntities.Add(entity);

        internal void Initialize()
        {
            new DeathStar().Create();
            xwing = new XWing();
            xwing.Instantiate(new Vector3(50f, 0f, 0f));
        }

        internal void Update(double elapsedTime)
        {
            RandomNumber = Random.Next(0, 1000);
            if (RandomNumber == 1)
                new TIE().Instantiate(new Vector3((float)Random.Next(-2000, 2000), 0f, 0f));

            pendingEntities.ForEach(entity => entities.Add(entity));
            pendingEntities.Clear();
            removedEntities.ForEach(entity => entities.Remove(entity));
            removedEntities.Clear();
            entities.ForEach(entity => entity?.Update(elapsedTime));
        }

        internal void Draw()
        {
            // E_BasicShader
            /*TGCGame.content.E_BasicShader.Parameters["View"].SetValue(TGCGame.camera.View);
            TGCGame.content.E_BasicShader.Parameters["Projection"].SetValue(TGCGame.camera.Projection);*/

            // E_BlinnPhong
            TGCGame.content.E_BlinnPhong.Parameters["ambientColor"].SetValue(new Vector3(1f, 1f, 1f));
            TGCGame.content.E_BlinnPhong.Parameters["diffuseColor"].SetValue(new Vector3(1f, 1f, 1f));
            TGCGame.content.E_BlinnPhong.Parameters["specularColor"].SetValue(new Vector3(1f, 1f, 1f));

            TGCGame.content.E_BlinnPhong.Parameters["KAmbient"].SetValue(0.7f);
            TGCGame.content.E_BlinnPhong.Parameters["KDiffuse"].SetValue(0.8f);
            TGCGame.content.E_BlinnPhong.Parameters["KSpecular"].SetValue(0.1f);
            TGCGame.content.E_BlinnPhong.Parameters["shininess"].SetValue(16.0f);

            TGCGame.content.E_BlinnPhong.Parameters["ViewProjection"].SetValue(TGCGame.camera.View * TGCGame.camera.Projection);
            TGCGame.content.E_BlinnPhong.Parameters["lightPosition"].SetValue(new Vector3(100000f, 80000f, 50000f));
            TGCGame.content.E_BlinnPhong.Parameters["eyePosition"].SetValue(TGCGame.camera.position);

            // E_LaserShader
            TGCGame.content.E_LaserShader.Parameters["View"].SetValue(TGCGame.camera.View);
            TGCGame.content.E_LaserShader.Parameters["Projection"].SetValue(TGCGame.camera.Projection);

            // Draw
            entities.ForEach(entity => entity?.Draw());
        }

        internal void InstantiateLaser(Vector3 position, Vector3 forward, Quaternion orientation)
        {
            new Laser().Instantiate(position - forward * 25f, orientation);
        }
    }
}