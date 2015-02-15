﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Engine.Game.Stats;

namespace Roguelike.Engine.Game.Entities
{
    public class PracticeDummy : Entity
    {
        public PracticeDummy(Level parent)
            : base(parent)
        {
            this.EntityType = EntityTypes.Enemy;
            this.ForegroundColor = Color.Tan;
            this.Token = '±';
            this.IsSolid = true;

            this.statsPackage = new StatsPackage(this)
                {
                    UnitName = "Training Dummy",

                    AttackPower = new Stat() { BaseValue = 0.0 },
                    PhysicalAvoidance = new Stat() { BaseValue = 0.0 },
                    PhysicalReduction = new Stat() { BaseValue = 0.0 },
                    Health = 500000
                };
        }

        public override void OnInteract(Entity entity)
        {

        }

        public override void UpdateStep()
        {
            base.UpdateStep();
        }
    }
}
