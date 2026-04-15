using Assets.Code;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bandits_and_Crime
{
    public class Rt_BanditArmy_AbandonBandits : Ritual
    {
        public UM_BanditArmy uM_BanditArmy;

        public Rt_BanditArmy_AbandonBandits(Location location, UM_BanditArmy uM_BanditArmy)
        : base(location)
        {
            this.uM_BanditArmy = uM_BanditArmy;
        }

        public override string getName()
        {
            return "Abandon Bandit Army";
        }

        public override string getDesc()
        {
            return "Leaves the Bandit Army behind, and returns this character to their agent state. Does not disband the Bandit Army.";
        }

        public override string getCastFlavour()
        {
            return "There is a time and a place for all things, and while the rampaging bandits is a powerful tool, The Brigand has better uses elsewhere right now.";
        }

        public override string getRestriction()
        {
            return "No army must be currently attacking the Bandits.";
        }

        public override double getProfile()
        {
            return 20.0;
        }

        public override double getMenace()
        {
            return 0.0;
        }

        public override Sprite getSprite()
        {
            return map.world.iconStore.banditry;
        }

        public override challengeStat getChallengeType()
        {
            return challengeStat.COMMAND;
        }

        public override bool validFor(UA ua)
        {
            return true;
        }

        public override bool validFor(UM ua)
        {
            foreach (Unit unit in map.units)
            {
                if (unit.task is Task_AttackArmy task_AttackArmy && task_AttackArmy.other == ua)
                {
                    return false;
                }
            }

            if (ua.task is Task_InBattle)
            {
                return false;
            }

            return base.validFor(ua);
        }

        public override bool valid()
        {
            return true;
        }

        public override int isGoodTernary()
        {
            return -1;
        }

        public override int getCompletionMenace()
        {
            return 5;
        }

        public override int getCompletionProfile()
        {
            return 10;
        }

        public override double getProgressPerTurnInner(UA unit, List<ReasonMsg> msgs)
        {
            int num = 0;
            num += unit.getStatCommand();
            msgs?.Add(new ReasonMsg("Stat: Command", unit.getStatCommand()));
            if (num < 1)
            {
                num++;
                msgs?.Add(new ReasonMsg("Base", 1.0));
            }

            return num;
        }

        public override double getComplexity()
        {
            return 1.0;
        }

        public override void complete(UA u)
        {
            throw new NotImplementedException();
        }

        public override void complete(UM u)
        {
            if (uM_BanditArmy == null || uM_BanditArmy.subsumedUnit == null)
            {
                return;
            }

            if (GraphicalMap.selectedUnit == u)
            {
                GraphicalMap.selectedUnit = uM_BanditArmy.subsumedUnit;
            }

            //map.units.Add(uM_BanditArmy.subsumedUnit);
            uM_BanditArmy.subsumedUnit.isDead = false;
            uM_BanditArmy.subsumedUnit.location = u.location;
            u.location.units.Add(uM_BanditArmy.subsumedUnit);
            uM_BanditArmy.subsumedUnit.person = u.person;
            uM_BanditArmy.subsumedUnit.addMenace(uM_BanditArmy.menace);
            u.person.unit = uM_BanditArmy.subsumedUnit;
            u.person = null;
            map.overmind.agents.Remove(uM_BanditArmy);

            //location.units.Add(uM_BanditArmy.subsumedUnit);
            map.units.Add(uM_BanditArmy.subsumedUnit);
            map.overmind.agents.Add(uM_BanditArmy.subsumedUnit);

            if (GraphicalMap.selectedUnit == uM_BanditArmy.subsumedUnit)
            {
                GraphicalMap.selectedUnit = uM_BanditArmy.subsumedUnit;
            }

            uM_BanditArmy.subsumedUnit = null;
            uM_BanditArmy.rituals.Remove(this);
        }

        public override int[] buildPositiveTags()
        {
            return new int[0];
        }

        public override int[] buildNegativeTags()
        {
            return new int[0];
        }
    }
}
