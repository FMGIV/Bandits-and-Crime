using Assets.Code;
using UnityEngine;

namespace Bandits_and_Crime
{
    public class UAE_Abs_Lawbreaker : UAE_Abstraction
    {
        public UAE_Abs_Lawbreaker(Map map)
            : base(map, -1)
        {
            base.map = map;
        }

        public override string getName()
        {
            return "The Lawbreaker";
        }

        public override string getDesc()
        {
            return "The Lawbreaker specializes in increasing and using crime for distraction and prosperity reduction. He can start new crime modifiers in Infiltrated Cities or along trade routes, or create organized dissent against the local ruler. (Unrest does wonders for crime, like banditry.)";
        }

        public override string getFlavour()
        {
            return "Money begets Power, and Power isn't always acquired fairly. The Lawbreaker knows how to use the poor and downtrodden to take money away from those that would consider themselves powerful.";
        }

        public override string getRestrictions()
        {
            return "Requires a City";
        }

        public override Sprite getBackground()
        {
            return map.world.iconStore.standardBack;
        }

        public override Sprite getForeground()
        {
            return EventManager.getImg("BanditsandCrime.Lawbreaker.png");
        }

        public override int getStatMight()
        {
            return 2;
        }

        public override int getStatLore()
        {
            return 1;
        }

        public override int getStatIntrigue()
        {
            return 4;
        }

        public override int getStatCommand()
        {
            return 2;
        }

        public override bool validTarget(Location loc)
        {
            if (World.allowAllAgents)
            {
                return true;
            }

            if (map.overmind.nEnthralled >= map.overmind.getAgentCap())
            {
                return false;
            }

            return loc.settlement is Set_City;
        }

        public override void createAgent(Location target)
        {
            if (map.overmind.god is God_Eternity god_Eternity)
            {
                god_Eternity.agentBuffer.Add(this);
            }

            UAE_Lawbreaker Lawbreaker = new UAE_Lawbreaker(target, target.soc as Society);
            Lawbreaker.person.stat_might = getStatMight();
            Lawbreaker.person.stat_lore = getStatLore();
            Lawbreaker.person.stat_intrigue = getStatIntrigue();
            Lawbreaker.person.stat_command = getStatCommand();
            map.units.Add(Lawbreaker);
            target.units.Add(Lawbreaker);
            map.overmind.agents.Add(Lawbreaker);
            map.overmind.availableEnthrallments--;
            Lawbreaker.person.shadow = 1.0;
            Lawbreaker.person.skillPoints++;
            Lawbreaker.person.state = Person.personState.enthralled;
            Lawbreaker.person.hates.Clear();
            Lawbreaker.person.extremeHates.Clear();
            Lawbreaker.person.likes.Clear();
            Lawbreaker.person.extremeLikes.Clear();

            if (!map.automatic)
            {
                GraphicalMap.selectedUnit = Lawbreaker;
                map.world.prefabStore.popAgentLevelUp(Lawbreaker);
                GraphicalMap.panTo(target.hex);
            }

            target.map.overmind.agentsUnique.Remove(this);
        }
    }
}
