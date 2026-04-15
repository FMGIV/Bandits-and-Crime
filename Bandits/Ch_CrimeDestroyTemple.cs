#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Shadows of Forbidden Gods\ShadowsOfForbiddenGods_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion
using Assets.Code;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bandits_and_Crime
{
    public class Ch_CrimeDestroyTemple : Challenge
    {
        public Pr_Crime crime;
        public Ch_CrimeDestroyTemple(Pr_Crime crime, Location loc)
            : base(loc)
        {
            this.crime = crime;
        }

        public override string getName()
        {
            return "Destroy Temple";
        }

        public override string getDesc()
        {
            return "Destroys the Temple in this location. Reduces crime by 100%.";
        }

        public override string getCastFlavour()
        {
            return "The criminals in this location can be manipulated to destroy the temple in this location. This isn't exactly subtle, and the local garrison is going to come down hard on the criminals responsible.";
        }

        public override double getProfile()
        {
            return 15;
        }

        public override string getRestriction()
        {
            return "Requires the crime modifier to be over 150% in this location.";
        }

        public override double getMenace()
        {
            return 15;
        }

        public override int getCompletionProfile()
        {
            return 15;
        }

        public override int getInherentDanger()
        {
            return 2;
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
            return map.world.iconStore.raid;
        }

        public override int isGoodTernary()
        {
            return -1;
        }

        public override void complete(UA u)
        {
           if (base.location.settlement is SettlementHuman settlementHuman)
            {
                foreach (Subsettlement sub in settlementHuman.subs)
                {
                    if (sub is Sub_Temple sub_Temple)
                    {
                        settlementHuman.subs.Remove(sub_Temple);
                        break;
                    }
                }
            }
            crime.influences.Add(new ReasonMsg("Garrison Crackdown.", -100.0));
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