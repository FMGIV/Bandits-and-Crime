using Assets.Code;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bandits_and_Crime
{
    public class Ch_CrimeAcquireContaminant : Challenge
    {
        public Pr_Crime crime;
        public Ch_CrimeAcquireContaminant(Pr_Crime crime, Location loc)
            : base(loc)
        {
            this.crime = crime;
        }

        public override string getName()
        {
            return "Acquire Contaminant";
        }

        public override string getDesc()
        {
            return "Acquires a Contaminant, which can be used to start a plague in a human settlement. Reduces crime by 50%.";
        }

        public override string getCastFlavour()
        {
            return "Using connections in the local criminal underworld, we can obtain a Contaminant from the sewage in this location's sewers.";
        }

        public override double getProfile()
        {
            return 10;
        }

        public override string getRestriction()
        {
            return "Requires the crime modifier to be over 200% in this location and Infiltration to be at or over 50%";
        }

        public override double getMenace()
        {
            return 25;
        }

        public override int getCompletionProfile()
        {
            return 5;
        }

        public override challengeStat getChallengeType()
        {
            return challengeStat.INTRIGUE;
        }

        public override double getProgressPerTurnInner(UA unit, List<ReasonMsg> msgs)
        {
            msgs?.Add(new ReasonMsg("Stat: Intrigue", Math.Max(1, unit.getStatIntrigue())));
            return Math.Max(1, unit.getStatIntrigue());
        }

        public override double getComplexity()
        {
            return 25;
        }

        public override bool validFor(UA ua)
        {
            return true;
        }

        public override Sprite getSprite()
        {
            return map.world.iconStore.plague;
        }

        public override int isGoodTernary()
        {
            return -1;
        }

        public override void complete(UA u)
        {
           u.person.gainItem(new I_Crime_Contaminant(map, u.location));
           crime.influences.Add(new ReasonMsg("Criminal Casualties.", -50.0));
        }

        public override bool valid()
        {
            if (crime.charge >= 150 && base.location.settlement.infiltration >= 0.5)
            {
                return true;
            }
            return false;
        }

        public override int[] buildPositiveTags()
        {
            return new int[1]
            {
                Tags.DISCORD
            };
        }

        public override int[] buildNegativeTags()
        {
            return new int[1]
            {
                Tags.RELIGION
            };
        }
    }
}