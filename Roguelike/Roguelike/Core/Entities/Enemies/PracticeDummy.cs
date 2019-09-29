using System;
using OpenTK.Graphics;
using Roguelike.Core.Stats;

namespace Roguelike.Core.Entities
{
    public class PracticeDummy : Entity
    {
        public PracticeDummy(Level parent)
            : base(parent)
        {
            EntityType = EntityTypes.Enemy;
            ForegroundColor = Color4.Tan;
            Token = '±';
            IsSolid = true;

            statsPackage = new StatsPackage(this)
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
