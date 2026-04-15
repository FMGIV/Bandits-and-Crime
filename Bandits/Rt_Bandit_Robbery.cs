#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Shadows of Forbidden Gods\ShadowsOfForbiddenGods_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code;

namespace Bandits_and_Crime
{
    public class Rt_Bandit_Robbery : Ritual
    {
        public UA Target;

        public Rt_Bandit_Robbery(Location loc, UA Target)
            : base(loc) 
        {
            this.Target = Target;
        }

        public override string getName()
        {
            return "Bandit Robbery";
        }

        public override string getDesc()
        {
            return "Steal random amount of gold from " + Target.getName() + ". Causes the hero to dislike the bandit.";
        }

        public override string getRestriction()
        {
            return "Requires a Hero with over 1 Gold";
        }

        public override string getCastFlavour()
        {
            return "What's yours is mine.";
        }

        public override double getProfile()
        {
            return 10.0;
        }

        public override double getMenace()
        {
            return 10.0;
        }

        public override challengeStat getChallengeType()
        {
            return challengeStat.MIGHT;
        }

        public override double getProgressPerTurnInner(UA unit, List<ReasonMsg> msgs)
        {
            msgs?.Add(new ReasonMsg("Base Progress", Math.Max(1, 1)));
            return 1;
        }

        public override double getComplexity()
        {
            return 1;
        }

        public override int getCompletionMenace()
        {
            return 10;
        }

        public override int getCompletionProfile()
        {
            return 10;
        }

        public override bool validFor(UA ua)
        {
            return Target.getStatMight() < ua.getStatMight()
              && (ua.person.shadow < 0.5
                || !Target.isCommandable()
                || (Target.society != null && (!Target.society.isDark()
                || !(Target.society is HolyOrder order)
                || !order.worshipsThePlayer)));
        }

        public override Sprite getSprite()
        {
            return map.world.iconStore.i_concealedDagger;
        }

        public override int isGoodTernary()
        {
            return -1;
        }

        public override void complete(UA u)
        {
            int goldtotake = (int)(Target.person.gold * (Eleven.random.Next(100) + 1) / 100.0);
            goldtotake = Math.Max(1, goldtotake);
            u.person.addGold(goldtotake);
            Target.person.addGold(-goldtotake);
            Target.person.decreasePreference(u.person.index + 10000);
        }

        public override bool valid()
        {
            if (Target.person.gold > 0) { return true; }
            else return false;
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