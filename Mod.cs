using BoneLib;
using Desktop_MP;
using Jevil;
using Jevil.Spawning;
using MelonLoader;
using SLZ.Marrow.Pool;
using UnityEngine;

[assembly: MelonInfo(typeof(Mod), "Desktop MP", "1.1.0", "Void Vapor Inc")]
[assembly: MelonGame("Stress Level Zero", "BONELAB")]
namespace Desktop_MP
{
    public class Mod : MelonMod
    {
        public static AssetPoolee dudePoolee;
        public static bool IsDudeThere => dudePoolee != null && dudePoolee.isActiveAndEnabled;
        public static MelonPreferences_Entry<bool> doVolumetrics;

        private const string dudeBarcode = "VoidVaporInc.DesktopMP.Spawnable.Dude";
        private static MelonPreferences_Category modCat;

        public static async void SpawnDude()
        {
            dudePoolee = await Barcodes.ToSpawnable(dudeBarcode).SpawnAsync(Player.playerHead.position, Quaternion.identity);
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.J) && IsDudeThere == false)
            {
                //spawn the dude breoijaboijergbjhearuhgv8934hjf893j48iwkejmdiskwoj4oiwejf (old joke)
                SpawnDude();
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                bool oldEntryValue = doVolumetrics.Value;
                doVolumetrics.Value = !oldEntryValue; // Toggles the entry boolean
                //iirc this doesn't work
            }
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
            modCat = MelonPreferences.CreateCategory("DesktopMP");
            doVolumetrics = modCat.CreateEntry<bool>("Do Volumetrics", false);
        }
    }
}
