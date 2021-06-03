﻿using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using System;
using TGC.MonoGame.TP.Drawers;
using TGC.MonoGame.TP.Physics;

namespace TGC.MonoGame.TP.ConcreteEntities
{
    internal class Turret : BaseTurret
    {
        private readonly TurretDrawer turretDrawer = new TurretDrawer(TGCGame.content.M_Turret, TGCGame.content.T_Turret);
        protected override Drawer Drawer() => turretDrawer;
        protected override TypedIndex Shape => TGCGame.content.SH_Turret;
        protected override Vector3 Scale => Vector3.One * DeathStar.trenchScale;
        private float headAngle = 0f, cannonsAngle = 0f;
        private double idleTime = 0;

        protected override float MaxRange => 1000f;
        private const float maxRotation = 0.2f;
        private const float minIdleTime = 1000f;
        private const float precition = (float)Math.PI / 4;

        private Quaternion headRotation = Quaternion.Identity, cannonsRotation = Quaternion.Identity;

        private readonly Vector3 cannonsOffset = new Vector3(0f, 2.8911f, 0f) * 10f;

        private Matrix HeadWorldMatrix()
            => Matrix.CreateScale(Scale) * Matrix.CreateFromQuaternion(headRotation) * Matrix.CreateTranslation(Position);

        private Matrix CannonsWorldMatrix()
            => Matrix.CreateScale(Scale) * Matrix.CreateFromQuaternion(cannonsRotation) * Matrix.CreateTranslation(Position + cannonsOffset);
        
        internal override void Update(double elapsedTime)
        {
            Vector3 difference = TGCGame.world.xwing.Position() - (Position + cannonsOffset);
            float distance = difference.Length();

            if (IsInRange(distance))
            {
                PhysicUtils.DirectionToEuler(difference, distance, out float objectiveHeadAngle, out float objectiveCannonsAngle);

                float differenceHead = objectiveHeadAngle - headAngle;
                float differenceCannonsAngle = objectiveCannonsAngle - cannonsAngle;

                headAngle += (differenceHead > 0 ? 1 : -1) * (float)Math.Min(Math.Abs(differenceHead), maxRotation * elapsedTime);
                cannonsAngle += (differenceCannonsAngle > 0 ? 1 : -1) * (float)Math.Min(Math.Abs(differenceCannonsAngle), maxRotation * elapsedTime);

                headRotation = Quaternion.CreateFromAxisAngle(Vector3.Up, headAngle);
                cannonsRotation = headRotation * Quaternion.CreateFromAxisAngle(Vector3.Left, cannonsAngle);

                differenceHead = Math.Abs(objectiveHeadAngle - headAngle);
                differenceCannonsAngle = Math.Abs(objectiveCannonsAngle - cannonsAngle);
                if (differenceHead < precition && differenceCannonsAngle < precition && idleTime > minIdleTime)
                {
                    Fire();
                    idleTime = 0;
                }
                else
                    idleTime += elapsedTime;
            }
            else
                idleTime += elapsedTime;
        }

        internal override void Draw()
        {
            turretDrawer.HeadWorldMatrix = HeadWorldMatrix();
            turretDrawer.CannonsWorldMatrix = CannonsWorldMatrix();
            base.Draw();
        }

        private void Fire()
        {
            new Laser().Instantiate(Position + cannonsOffset - PhysicUtils.Left(cannonsRotation) * 2f - PhysicUtils.Forward(cannonsRotation) * 25f, cannonsRotation);
            new Laser().Instantiate(Position + cannonsOffset + PhysicUtils.Left(cannonsRotation) * 2f - PhysicUtils.Forward(cannonsRotation) * 25f, cannonsRotation);
        }
    }
}