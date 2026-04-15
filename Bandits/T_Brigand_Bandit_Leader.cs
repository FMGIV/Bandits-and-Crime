#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Shadows of Forbidden Gods\ShadowsOfForbiddenGods_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion
using Assets.Code;

namespace Bandits_and_Crime
{
    public class T_Brigand_Bandit_Leader : Trait
    {
        public override string getName()
        {
            return "Bandit Leader";
        }

        public override string getDesc()
        {
            return "When this trait is first acquired, increase the Banditry modifier in location by 200% or create a new one at 150%";
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
            Pr_New_Banditry banditry = null;
            foreach (Property property in person.unit.location.properties)
            {
                banditry = property as Pr_New_Banditry;
                if (banditry != null)
                {
                    // This tells the code to exit the loop early. There's no point in continuing to loop over the properties after you've found what you need.
                    break;
                }
            }

            if (banditry == null)
            {
                // If there is no instance of Pr_Stolen_Food at that location, the result will remain `null` after the loop goes through all properties, and we need to add a new one.
                banditry = new Pr_New_Banditry(person.unit.location);
                banditry.charge = 150.0;
                person.unit.location.properties.Add(banditry);
            }
            else
            {
                // If we did find an instance of stolen food, we need to add an influence to it, so that it shows up in the UI.
                banditry.influences.Add(new ReasonMsg("Bandit Leader Trait", 200.0));
            }
        }

        public override int[] getTags()
        {
            return new int[1] { Tags.DISCORD };
        }
    }
}