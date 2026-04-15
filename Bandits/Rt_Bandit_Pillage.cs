using Assets.Code;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bandits_and_Crime
{
    public class Rt_Bandit_Pillage : Ritual
    {
        public Rt_Bandit_Pillage(Location loc)
            : base(loc)
        {
        }

        public override string getName()
        {
            return "Pillage";
        }

        public override string getCastFlavour()
        {
            return "In times of danger the people flee to the fortress cities, and the army defends their local duke or duchess, leaving the surrounding areas ripe for the picking by marauding Bandits.";
        }

        public override string getDesc()
        {
            return "The Bandit pillages this location, causing devastation and steals some gold from the location.";
        }

        public override string getRestriction()
        {
            return "Requires a minor human settlement.";
        }

        public override double getProfile()
        {
            return map.param.ch_raidperiphery_aiProfile;
        }

        public override double getMenace()
        {
            return 30.0 * (1.0 - base.location.settlement.shadow * 2.0);
        }

        public override Sprite getSprite()
        {
            return map.world.iconStore.raid;
        }

        public override challengeStat getChallengeType()
        {
            return challengeStat.MIGHT;
        }

        public override bool validFor(UA ua)
        {
            return ua.location.settlement is Set_MinorHuman && 
                (ua.person.shadow < 0.5 
                || (!location.soc.isDark() && (!(location.soc is HolyOrder order) 
                || !order.worshipsThePlayer)));
        }

        /*
The location must have a settlement of Type Set_MinorHuman
AND The bandit's shadow must be less than 0.5
OR The locations Society must not be dark
AND The location's society must not be a Holy Order OR if it is, it must not worship the player. - Ilikegoodfood
        /*
        public override bool valid()
        {
            return true; 
        }
        */
        public override int isGoodTernary()
        {
            return -1;
        }

        public override int getCompletionMenace()
        {
            return map.param.ch_raidPeripheryMenaceGain;
        }

        public override int getCompletionProfile()
        {
            return map.param.ch_raidPeripheryProfileGain;
        }

        public override double getProgressPerTurnInner(UA unit, List<ReasonMsg> msgs)
        {
            msgs?.Add(new ReasonMsg("Stat: Might", Math.Max(1, unit.getStatMight())));
            return Math.Max(1, unit.getStatMight());
        }

        public override double getComplexity()
        {
            return Math.Max(10.0, base.location.settlement.defences / 2.0);
        }

        public override void complete(UA u)
        {
            int goldtotake = (int)(location.person().gold * ((Eleven.random.Next(100) + 1) / 100.0));
            Property.addToPropertySingleShot(u.getName(), Property.standardProperties.DEVASTATION, map.param.ch_raidperiphery_parameterValue2, base.location);
            u.person.addGold(goldtotake);
            location.person().addGold(-goldtotake);
        }

        public override int[] buildPositiveTags()
        {
            return new int[2]
            {
            Tags.CRUEL,
            Tags.COMBAT
            };
        }

        public override int[] buildNegativeTags()
        {
            return new int[0];
        }
    }
}
