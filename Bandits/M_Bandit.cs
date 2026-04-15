using Assets.Code;
using UnityEngine;

namespace Bandits_and_Crime
{
    public class M_Bandit : Minion
    {
        public M_Bandit(Map map)
            : base(map)
        {
        }

        public override string getName()
        {
            return "Bandit";
        }

        public override Sprite getIcon()
        {
            return map.world.iconStore.banditry;
        }

        public override Sprite getIconBack()
        {
            return map.world.textureStore.clear;
        }

        public override int getCommandCost()
        {
            return 1;
        }

        public override int getAttack()
        {
            return 2;
        }

        public override int getMaxDefence()
        {
            return 1;
        }

        public override int getGoldCost()
        {
            return 10;
        }

        public override int getMaxHP()
        {
            return 2;
        }

        public override int[] getTags()
        {
            return new int[0];
        }

        public override Minion getClone()
        {
            return new M_Goblin(map);
        }
    }
}
