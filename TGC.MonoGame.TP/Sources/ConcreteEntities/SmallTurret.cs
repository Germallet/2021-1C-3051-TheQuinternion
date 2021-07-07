﻿using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using System;
using TGC.MonoGame.TP.Drawers;
using TGC.MonoGame.TP.Physics;
using TGC.MonoGame.TP.Scenes;

namespace TGC.MonoGame.TP.ConcreteEntities
{
    internal class SmallTurret : BaseTurret
    {
        internal const float scale = DeathStar.trenchScale * 0.9f;
        protected override Vector3 Scale => Vector3.One * scale;
        protected override Drawer Drawer() => TGCGame.content.D_SmallTurret;
        protected override TypedIndex Shape => TGCGame.content.SH_SmallTurret;

        protected override float MaxRange => 600f;
        protected override float MinIdleTime => 1000f;
        protected override Vector3 CannonsOffset => new Vector3(0f, 1.70945f, 0f) * 100 * scale;

        private const float rotationSpeed = 0.2f;
        private const float precitionYaw = (float)Math.PI / 30;
        private const float precitionPitch = (float)Math.PI / 10;

        private readonly bool rotated;
        internal SmallTurret(bool rotated) => this.rotated = rotated;

        private Vector3 left;
        protected override void OnInstantiate()
        {
            if (rotated)
                headAngle = MathHelper.PiOver2;
            left = PhysicUtils.Left(Rotation);
            base.OnInstantiate();
        }

        private Quaternion cannonsRotation = Quaternion.Identity;

        private Matrix CannonsWorldMatrix()
            => Matrix.CreateScale(Scale) * Matrix.CreateFromQuaternion(cannonsRotation) * Matrix.CreateTranslation(CannonsPosition);
                
        protected override void Aim(Vector3 difference, float yawDifference, float pitchDifference, double elapsedTime)
        {
            cannonsAngle += (pitchDifference > 0 ? 1 : -1) * (float)Math.Min(Math.Abs(pitchDifference), rotationSpeed * elapsedTime);
            if (!rotated)
                headAngle = difference.Z < 0 ? 0 : MathHelper.Pi;
            else
                headAngle = difference.X < 0 ? 0 : MathHelper.Pi;
            cannonsRotation = Rotation * Quaternion.CreateFromAxisAngle(Vector3.Up, headAngle) * Quaternion.CreateFromAxisAngle(Vector3.Left, cannonsAngle);
        }

        protected override bool IsAimed(float yawDifference, float pitchDifference) => (yawDifference < precitionYaw || yawDifference - MathHelper.Pi < precitionYaw) && pitchDifference < precitionPitch;

        protected override void Fire()
        {
            Vector3 forward = PhysicUtils.Forward(cannonsRotation);
            World.InstantiateLaser(CannonsPosition - left, forward, cannonsRotation, emitter);
            World.InstantiateLaser(CannonsPosition + left, forward, cannonsRotation, emitter);
        }

        internal override void Draw()
        {
            TGCGame.content.D_SmallTurret.CannonsWorldMatrix = CannonsWorldMatrix();
            base.Draw();
        }
    }
}