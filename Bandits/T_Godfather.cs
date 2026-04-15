#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Shadows of Forbidden Gods\ShadowsOfForbiddenGods_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using Assets.Code;

namespace Bandits_and_Crime
{
    public class T_Godfather : Trait
    {
        public override string getName()
        {
            return "The Godfather";
        }

        public override string getDesc()
        {
            return "When this trait is first acquired, increase the crime modifier in location by 200% or create a new one at 100%";
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
            Pr_Crime crime = null;
            foreach (Property property in person.unit.location.properties)
            {
                crime = property as Pr_Crime;
                if (crime != null)
                {
                    // This tells the code to exit the loop early. There's no point in continuing to loop over the properties after you've found what you need.
                    break;
                }
            }

            if (crime == null)
            {
                // If there is no instance of Pr_Stolen_Food at that location, the result will remain `null` after the loop goes through all properties, and we need to add a new one.
                crime = new Pr_Crime(person.unit.location);
                crime.charge = 100.0;
                person.unit.location.properties.Add(crime);
            }
            else
            {
                // If we did find an instance of stolen food, we need to add an influence to it, so that it shows up in the UI.
                crime.influences.Add(new ReasonMsg("Godfather Trait", 200.0));
            }
        }

        public override int[] getTags()
        {
            return new int[1] { Tags.DISCORD };
        }
    }
}