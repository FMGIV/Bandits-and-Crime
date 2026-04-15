using UnityEngine;
using Assets.Code;
using System.Collections.Generic;
using System.Linq;

namespace Bandits_and_Crime
{
    public class I_Crime_Contaminant : Item
    {
        public List<Ritual> rituals = new List<Ritual>();
        public List<Ritual> nothing = new List<Ritual>();

        public bool doublesHeld = false;

        public I_Crime_Contaminant(Map map, Location loc)
            : base(map)
        {
            rituals.Add(new Rti_CrimeStartPlague(loc));
        }

        public override string getName()
        {
            return "Sewage Contaminant";
        }

        public override string getShortDesc()
        {
            return "A Contaminant from a sewage system that can be used to start a plague in a human settlement. (max of only one will have effect per agent)." + (doublesHeld ? " [DISABLED]" : "");
        }

        public override Sprite getIconFore()
        {
            return map.world.iconStore.i_poison;
        }

        public override int getLevel()
        {
            return 0;
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

                if (item is I_Crime_Contaminant)
                {
                    doublesHeld = true;
                }
            }
        }

        public override int getMorality()
        {
            return Item.MORALITY_EVIL;
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