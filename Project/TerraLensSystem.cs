using Terraria.ModLoader;
using Terraria.UI;
using Terraria;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TerraLens.Project.UI;
using TerraLens.Project.Config;
using TerraLens.Project.Profiling;
using TerraLens.Project.Profiling.ModProfilers;
using Microsoft.Xna.Framework.Input; // For Keys enum

namespace TerraLens.Project
{
    public class TerraLensSystem : ModSystem
    {
        private UserInterface userInterface;
        private TerraLensUI terraLensUI;
        private ModKeybind toggleOverlayKeybind;

        public override void Load()
        {
            if (!Main.dedServ)
            {
                // Initialize configuration
                TerraLensConfig.Instance = ModContent.GetInstance<TerraLensConfig>();

                // Initialize UI
                terraLensUI = new TerraLensUI();
                userInterface = new UserInterface();
                userInterface.SetState(terraLensUI);

                // Register keybind for toggling overlay, default "Insert"
                toggleOverlayKeybind = KeybindLoader.RegisterKeybind(Mod, "Toggle Overlay", Keys.Insert);
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (TerraLensConfig.Instance.ShowOverlay)
            {
                userInterface?.Update(gameTime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int index = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (index != -1)
            {
                layers.Insert(index, new LegacyGameInterfaceLayer(
                    "TerraLens: Profiler UI",
                    delegate
                    {
                        if (TerraLensConfig.Instance.ShowOverlay)
                        {
                            userInterface.Draw(Main.spriteBatch, new GameTime());
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }

        public override void PostUpdateEverything()
        {
            if (toggleOverlayKeybind.JustPressed)
            {
                TerraLensConfig.Instance.ShowOverlay = !TerraLensConfig.Instance.ShowOverlay;
                TerraLensConfig.Instance.Save(); // Save the updated config
            }
        }

        public override void PostUpdateNPCs()
        {
            if (TerraLensConfig.Instance != null && TerraLensConfig.Instance.CollectModEntities)
            {
                ModEntityProfiler.UpdateNPCs();
            }
        }

        public override void PostUpdateProjectiles()
        {
            if (TerraLensConfig.Instance != null && TerraLensConfig.Instance.CollectModEntities)
            {
                ModEntityProfiler.UpdateProjectiles();
            }
        }

        public override void Unload()
        {
            // Clean up references to prevent memory leaks
            userInterface?.SetState(null);
            terraLensUI = null;
            userInterface = null;
            TerraLensConfig.Instance = null;
        }
    }
}
