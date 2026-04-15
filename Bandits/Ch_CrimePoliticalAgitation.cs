using Assets.Code;
using System.Collections.Generic;
using UnityEngine;

namespace Bandits_and_Crime
{
    public class Ch_CrimePoliticalAgitation : Challenge
    {
        public Pr_Crime crime;
        public Ch_CrimePoliticalAgitation(Pr_Crime crime, Location loc)
            : base(loc)
        {
            this.crime = crime;
        }

        public override string getName()
        {
            return "Crime: Sedition";
        }

        public override string getCastFlavour()
        {
            return "The Criminal Underworld in this location can be a useful tool in driving separatism.";
        }

        public override string getDesc()
        {
            return "Creates or boosts a political agitation modifier in this location.";
        }

        public override string getRestriction()
        {
            return "Requires Crime to be at least 200% and the location to be fully infiltrated. Not avaliable in society capitals.";
        }

        public override double getProfile()
        {
            return 10;
        }

        public override double getMenace()
        {
            return 15;
        }

        public override int getCompletionProfile()
        {
            return 10;
        }

        public override int getCompletionMenace()
        {
            return 15;
        }

        public override challengeStat getChallengeType()
        {
            return challengeStat.INTRIGUE;
        }

        public override double getProgressPerTurnInner(UA unit, List<ReasonMsg> msgs)
        {
            // No longer needs to call Math.Max.
            double progress = unit.getStatIntrigue();
            if (progress < 1)
            {
                progress = 1;
                msgs?.Add(new ReasonMsg("Base", progress));
            }
            else
            {
                // If might is 1 or greater, it states that the value is from might.
                msgs?.Add(new ReasonMsg("Stat: Intrigue", progress));
            }

            // The number was acquired only once, and used five times.
            return progress;
        }

        public override double getComplexity()
        {
            return 30;
        }

        public override bool validFor(UA ua)
        {
            return true;
        }

        public override Sprite getSprite()
        {
            return map.world.iconStore.agitate;
        }

        public override int isGoodTernary()
        {
            return -1;
        }

        public override void complete(UA u)
        {
            Pr_PoliticalAgitation politicalagitation = null;
            foreach (Property property in location.properties)
            {
                politicalagitation = property as Pr_PoliticalAgitation;
                if (politicalagitation != null)
                {
                    // This tells the code to exit the loop early. There's no point in continuing to loop over the properties after you've found what you need.
                    break;
                }
            }

            if (politicalagitation == null)
            {
                // If there is no instance of Pr_Stolen_Food at that location, the result will remain `null` after the loop goes through all properties, and we need to add a new one.
                politicalagitation = new Pr_PoliticalAgitation(location);
                politicalagitation.charge = 25;
                location.properties.Add(politicalagitation);
            }
            else
            {
                // If we did find an instance of stolen food, we need to add an influence to it, so that it shows up in the UI.
                politicalagitation.influences.Add(new ReasonMsg("Criminal Influence", 25));
            }
        }

        public override bool valid()
        {
            if (crime.charge >= 200 && base.location.settlement.infiltration >= 1)
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
            return new int[0];
        }
    }
}