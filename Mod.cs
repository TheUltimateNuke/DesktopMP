using MelonLoader;
using UnityEngine;
using Jevil.Spawning;
using Jevil;
using BoneLib;
using Desktop_MP;
using SLZ.Marrow.Data;

[assembly: MelonInfo(typeof(Mod), "Desktop MP", "1.1.0", "Void Vapor Inc")]
[assembly: MelonGame("Stress Level Zero", "BONELAB")]
namespace Desktop_MP
{
    public class Mod : MelonMod
    {
        static public bool isDudeThere;
        public Spawnable dudeSpawnable;
        private MelonPreferences_Category modcat;
        static public MelonPreferences_Entry<bool> dovolumetrics;
        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.J) && isDudeThere == false)
            {
                isDudeThere = true;
                //spawn the dude breoijaboijergbjhearuhgv8934hjf893j48iwkejmdiskwoj4oiwejf (old joke)
                Barcodes.ToSpawnable("VoidVaporInc.DesktopMP.Spawnable.Dude").Spawn(Player.playerHead.position, Quaternion.identity, true);
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                bool oldEntryValue = dovolumetrics.Value;
                dovolumetrics.Value = !oldEntryValue; // Toggles the entry boolean
                //iirc this doesn't work
            }
        }
        public void falseDude(LevelInfo levelInfo)
        {
            isDudeThere = false;
        }
        public override void OnApplicationStart()
        {
            FieldInjector.SerialisationHandler.Inject<PickupSoundSystem>();
            FieldInjector.SerialisationHandler.Inject<PickupSystem1>();
            FieldInjector.SerialisationHandler.Inject<FPSController>();
            FieldInjector.SerialisationHandler.Inject<ObjectInteractable>();
            FieldInjector.SerialisationHandler.Inject<ObjectActivator1>();
            FieldInjector.SerialisationHandler.Inject<FootstepSound1>();
        }
        public override void OnInitializeMelon()
        {
            modcat = MelonPreferences.CreateCategory("DesktopMP");
            dovolumetrics = modcat.CreateEntry<bool>("Do Volumetrics", false);
            Hooking.OnLevelInitialized += falseDude;
        }
    }
}
