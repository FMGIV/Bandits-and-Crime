using UnityEngine;
using Assets.Code;
using System.Collections.Generic;

namespace Bandits_and_Crime
{
    public class I_Crime_Arcane_Texts : Item
    {
        public List<Ritual> rituals = new List<Ritual>();
        public List<Ritual> nothing = new List<Ritual>();

        public bool doublesHeld = false;

        public I_Crime_Arcane_Texts(Map map, Location loc)
            : base(map)
        {
            rituals.Add(new Rti_CrimeReadArcaneText(loc));
        }

        public override string getName()
        {
            return "Arcane Texts";
        }

        public override string getShortDesc()
        {
            return "These texts have knowledge of ancient magics, which may allow those skilled in the magical arts to learn new capabilities.";
        }

        public override Sprite getIconFore()
        {
            return map.world.iconStore.library;
        }

        public override void turnTick(Person owner)
        {
            base.turnTick(owner);
            doublesHeld = false;
            foreach (Item item in owner.items)
            {
                if (item == this)
                {
                    break;
                }

                if (item is I_Crime_Arcane_Texts)
                {
                    doublesHeld = true;
                }
            }
        }

        public override int getLevel()
        {
            return Item.LEVEL_RARE;
        }

        public override int getMorality()
        {
            return Item.MORALITY_NEUTRAL;
        }
        public override List<Ritual> getRituals(UA ua)
        {
            if (!doublesHeld)
            {
                return rituals;
            }
            return nothing;
        }
    }
}