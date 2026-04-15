using Assets.Code;

namespace Bandits_and_Crime
{
    public class T_Gang_Leader : Trait
    {
        public override string getName()
        {
            return "Gang Leader";
        }

        public override string getDesc()
        {
            return "When this trait is first acquired, gain a Bandit minion in all empty slots";
        }

        public override int getMaxLevel()
        {
            return 1;
        }

        public override void turnTick(Person p)
        {
            base.turnTick(p);
        }

        public override void onAcquire(Person person)
        {
            base.onAcquire(person);
            for (int i = 0; i < 3; i++)
            {
                if (person.unit != null && person.unit is UA uA && uA.minions[i] == null)
                {
                    M_Bandit m_Bandit = new M_Bandit(person.map);
                    uA.minions[i] = m_Bandit;
                }
            }
        }

        public override int[] getTags()
        {
            return new int[0];
        }
    }
}
