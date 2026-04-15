using Assets.Code;
using DuloGames.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Bandits_and_Crime
{
    public class Ch_CrimeSellItem : Challenge
    {
        public UA seller;

        public Item Item1;
        public Item Item2;
        public Item Item3;

        public int Item1value = 0;
        public int Item2value = 0;
        public int Item3value = 0;

        public Pr_Crime crime;
        public Ch_CrimeSellItem(Pr_Crime crime, Location loc)
            : base(loc)
        {
            this.crime = crime;
        }

        public override string getName()
        {
            return "Sell Item on Black Market";
        }

        public override string getDesc()
        {
            return "Sells an item on the local black market. (Note that some items have no value to the black market.)";
        }

        public override string getCastFlavour()
        {
            return "One mans trash is another man's treasure. What is no longer useful can be sold off at the black market here.";
        }

        public override double getProfile()
        {
            return 0;
        }

        public override string getRestriction()
        {
            return "Requires the crime modifier to be over 100% in this location.";
        }

        public override double getMenace()
        {
            return 0;
        }

        public override int getCompletionProfile()
        {
            return 0;
        }

        public override challengeStat getChallengeType()
        {
            return challengeStat.OTHER;
        }

        public override double getProgressPerTurnInner(UA unit, List<ReasonMsg> msgs)
        {
            msgs?.Add(new ReasonMsg("Base", 1.0));
            return 1.0;
        }

        public override double getComplexity()
        {
            return 1;
        }

        public override bool validFor(UA ua)
        {
            if (ua == null)
            {
                return false;
            }
            for (int i = 0; i < ua.person.items.Length; i++)
            {
                if (ua.person.items[i] is Item)
                {
                    return true;
                }
            }
            return false;
        }

        public override Sprite getSprite()
        {
            return map.world.iconStore.shadowMarket;
        }

        public override int isGoodTernary()
        {
            return -1;
        }

        public override void complete(UA u)
        {
            if(u.person != null)
            {
                List<Item> list = new List<Item>();
                List<string> names = new List<string>();
                for (int i = 0; i < u.person.items.Length; i++)
                {
                    if (u.person.items[i] == null)
                    {
                        continue;
                    }
                    list.Add(u.person.items[i]);
                    names.Add(u.person.items[i].getName() + ": " + u.person.items[i].getLevel() * 25 + " gold.");
                }

                if (u.isCommandable())
                {
                    Sel_Crime_SellItem receiver = new Sel_Crime_SellItem(map, u, list);
                    map.world.ui.addBlocker(map.world.prefabStore.getScrollSetText(names, invertOrder: false, receiver, "Sell Item", "Select an item to sell from " + u.person.getName() + ".").gameObject);
                }
            }
        }

        public override bool valid()
        {
            if (crime.charge < 100)
            {
                return false;
            }

            return true;
        }

        public override int[] buildPositiveTags()
        {
            return new int[1]
            {
                Tags.GOLD
            };
        }

        public override int[] buildNegativeTags()
        {
            return new int[0];
        }
    }
}