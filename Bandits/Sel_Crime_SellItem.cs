using System.Collections.Generic;
using Assets.Code;

namespace Bandits_and_Crime
{
    public class Sel_Crime_SellItem : SelectClickReceiver
    {
        public Map map;

        public Unit hero;

        public List<Item> options;

        public Sel_Crime_SellItem(Map map, Unit target, List<Item> opts)
        {
            this.map = map;
            hero = target;
            options = opts;
        }

        public void cancelled()
        {
        }

        public void selectableClicked(string text, int index)
        {
            switch (index)
            {
                case 0:
                    hero.person.items[0] = null;
                    hero.person.gold += options[0].getLevel() * 25;
                    break;
                case 1:
                    hero.person.items[1] = null;
                    hero.person.gold += options[1].getLevel() * 25;
                    break;
                case 2:
                    hero.person.items[2] = null;
                    hero.person.gold += options[2].getLevel() * 25;
                    break;
            }
        }
    }
}