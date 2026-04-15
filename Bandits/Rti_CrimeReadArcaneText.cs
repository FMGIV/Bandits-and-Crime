using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code;

namespace Bandits_and_Crime
{
    public class Rti_CrimeReadArcaneText : Ritual
    {

        public Rti_CrimeReadArcaneText(Location loc)
            : base(loc)
        {
        }

        public override double getMenace()
        {
            return 0;
        }

        public override challengeStat getChallengeType()
        {
            return challengeStat.LORE;
        }

        public override string getName()
        {
            return "Read Arcane Text";
        }

        public override string getCastFlavour()
        {
            return "The forces which flow through this world can be understood, can be controlled, if one learns their ways. In these texts we can learn what those who came before us had pieced together of the world's underlying nature, but other ways, far more unsavoury, also exist to gain insights into the arcane.";
        }

        public override double getProfile()
        {
            return 0;
        }

        public override int getCompletionMenace()
        {
            return 0;
        }

        public override int getCompletionProfile()
        {
            return 0;
        }

        public override double getProgressPerTurnInner(UA unit, List<ReasonMsg> msgs)
        {
            msgs?.Add(new ReasonMsg("Lore", unit.getStatLore()));
            return Math.Max(1, unit.getStatLore());
        }

        public override double getComplexity()
        {
            return map.param.ch_learnsecret_complexity;
        }

        public override Sprite getSprite()
        {
            return map.world.iconStore.arcaneSecret;
        }

        public override int isGoodTernary()
        {
            return 0;
        }

        public override bool validFor(UA ua)
        {
            return true;
        }

        public override void complete(UA u)
        {
            T_ArcaneKnowledge t_ArcaneKnowledge = null;
            foreach (Trait trait in u.person.traits)
            {
                if (trait is T_ArcaneKnowledge t_ArcaneKnowledge2)
                {
                    t_ArcaneKnowledge = t_ArcaneKnowledge2;
                    t_ArcaneKnowledge.level++;
                }
            }

            if (t_ArcaneKnowledge == null)
            {
                t_ArcaneKnowledge = new T_ArcaneKnowledge();
                u.person.receiveTrait(t_ArcaneKnowledge);
            }
            for (int i = 0; i < u.person.items.Length; i++)
            {
                if (u.person.items[i] is I_Crime_Arcane_Texts)
                {
                    u.person.items[i] = null;
                    break;
                }
            }
        }

        public override bool valid()
        {
            return true;
        }

        public override string getDesc()
        {
            return "Learns the Arcane Secret in these texts, removing it and increasing the user's <b>Arcane Knowledge</b>.";
        }

        public override double getUtility(UA ua, List<ReasonMsg> msgs)
        {
            double utility = base.getUtility(ua, msgs);
            if (ua.isCommandable())
            {
                return utility;
            }

            if (ua.getGeomancyMastery() == map.param.ch_learnsecret_parameterValue2)
            {
                return 0.0;
            }

            double num = map.param.ch_studyMagicAversion;
            utility += num;
            msgs?.Add(new ReasonMsg("Magic is dangerous", num));
            if (ua is UAG_Mage)
            {
                double num2 = map.overmind.magicalArmsRace - (double)ua.getGeomancyMastery() * map.param.ch_learnsecret_parameterValue3;
                if (num2 > 0.0)
                {
                    num = num2 * map.overmind.magicalArmsRace * (double)map.param.ch_studyMagicFromArmsRace;
                    utility += num;
                    msgs?.Add(new ReasonMsg("Magical Arms Race", num));
                }
            }

            return utility;
        }

        public override int[] buildPositiveTags()
        {
            return new int[1] { Tags.AMBITION };
        }

        public override int[] buildNegativeTags()
        {
            return new int[0];
        }
    }
}