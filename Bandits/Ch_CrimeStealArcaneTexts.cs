using Assets.Code;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bandits_and_Crime
{
    public class Ch_CrimeStealArcaneTexts : Challenge
    {
        public Pr_Crime crime;
        public Ch_CrimeStealArcaneTexts(Pr_Crime crime, Location loc)
            : base(loc)
        {
            this.crime = crime;
        }

        public override string getName()
        {
            return "Steal Arcane Texts";
        }

        public override string getDesc()
        {
            return "Acquires Stolen Arcane Texts, which can be used to increase the user's <b>Arcane Knowledge</b>. Won't start the Magical Arms Race.";
        }

        public override string getCastFlavour()
        {
            return "Using connections in the local criminal underworld, knowledge can be taken from the unworthy straight to our agent's hands.";
        }

        public override double getProfile()
        {
            return 10;
        }

        public override string getRestriction()
        {
            return "Requires the crime modifier to be over 150% in this location.";
        }

        public override double getMenace()
        {
            return 10;
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
            return 20;
        }

        public override bool validFor(UA ua)
        {
            return true;
        }

        public override Sprite getSprite()
        {
            return map.world.iconStore.library;
        }

        public override int isGoodTernary()
        {
            return -1;
        }

        public override void complete(UA u)
        {
            u.person.gainItem(new I_Crime_Arcane_Texts(map, u.location));
            crime.influences.Add(new ReasonMsg("Criminal Arrests.", -25.0));
        }

        public override bool valid()
        {
            if (crime.charge >= 150)
            {
                return true;
            }
            return false;
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