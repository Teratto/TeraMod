using static HarmonyLib.CodeInstruction;

namespace DemoMod.Actions
{
    public class ATaxingAttack : AAttack
    {
        public int Tax = 1;
        // CardAction


        //internal static string glossary_item = "";

        //public override void Begin(G g, State s, Combat c)
        //{
        //    c.energy += 99;
        //    ModManifest.EventHub?.SignalEvent<Combat>("EWanderer.DemoMod.TestEvent", c);
        //}

        public override Icon? GetIcon(State s)
        {
            return new Icon((Spr)ModManifest.TaxesSprite.Id, damage, Colors.attack);
            // return new Icon(Spr.icons_ace, 42, Colors.attackFail);
        }

        public override List<Tooltip> GetTooltips(State s)
        {
            List<Tooltip> tooltips = new List<Tooltip>();
            tooltips.Add(new TTGlossary("Attack for {0}, and if the ship was hit, apply {1} tax.", new object[] { damage, Tax }));
            return tooltips;
        }
    }
}