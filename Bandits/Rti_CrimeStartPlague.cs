using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code;

namespace Bandits_and_Crime
{
    public class Rti_CrimeStartPlague : Ritual
    {

        public Rti_CrimeStartPlague(Location loc)
            : base(loc)
        {
        }

        public override double getMenace()
        {
            return 25;
        }

        public override challengeStat getChallengeType()
        {
            return challengeStat.INTRIGUE;
        }

        public override string getName()
        {
            return "Start Plague with Contaminant";
        }

        public override string getCastFlavour()
        {
            return "The Contaminant acquired from the sewers is a useful tool for poisining the local water supply.";
        }

        public override double getProfile()
        {
            return map.param.ch_startplague_aiProfile;
        }

        public override int getCompletionMenace()
        {
            return map.param.ch_startplague_completionMenace;
        }

        public override int getCompletionProfile()
        {
            return map.param.ch_startplague_completionProfile;
        }

        public override double getProgressPerTurnInner(UA unit, List<ReasonMsg> msgs)
        {
            msgs?.Add(new ReasonMsg("Stat: Intrigue", Math.Max(1, unit.getStatIntrigue())));
            return Math.Max(1, unit.getStatIntrigue());
        }

        public override double getComplexity()
        {
            return map.param.ch_startplague_complexity;
        }

        public override Sprite getSprite()
        {
            return map.world.iconStore.plague;
        }

        public override int isGoodTernary()
        {
            return -1;
        }

        public override bool validFor(UA ua)
        {
            foreach (Property property in ua.location.properties)
            {
                if (property is Pr_Plague)
                {
                    return false;
                }
            }
            return true;
        }

        public override void complete(UA u)
        {
            Property.addToPropertySingleShot("Contaminated Water", Property.standardProperties.PLAGUE, 30, u.location);
            for (int i = 0; i < u.person.items.Length; i++)
            {
                if (u.person.items[i] is I_Crime_Contaminant)
                {
                    u.person.items[i] = null;
                    break;
                }
            }
        }

        public override bool valid()
        {

            if (location.settlement is SettlementHuman && location.soc is Society)
            {
                return true;
            }

            return false;
        }

        public override string getDesc()
        {
            return "Starts a <b>plague</b> in this location. Plagues reduce prosperity and population of human settlements (including potentially wiping them out). They spread between adjacent locations if they grow enough, and can prove effective distractions as the human forces are busy combatting the disease.";
        }

        public override string getRestriction()
        {
            return "Requires a Human Settlement. Plague must not already be present.";
        }

        public override int[] buildPositiveTags()
        {
            return new int[2]
            {
                Tags.CRUEL,
                Tags.DISEASE
            };
        }

        public override int[] buildNegativeTags()
        {
            return new int[0];
        }
    }
}