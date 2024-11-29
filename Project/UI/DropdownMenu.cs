//using Terraria.UI;
//using Terraria.GameContent.UI.Elements;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using System.Collections.Generic;
//using System.Linq;

//namespace TerraLens.Project.UI
//{
//    public class DropdownMenu : UIElement
//    {
//        private UIPanel backgroundPanel;
//        private UIList modList;
//        private UIScrollbar scrollbar;
//        private List<UICheckbox> checkboxes = new List<UICheckbox>();
//        public Dictionary<string, bool> SelectedMods { get; private set; } = new Dictionary<string, bool>();

//        public DropdownMenu(List<string> mods, Vector2 position)
//        {
//            // Set the position of the dropdown menu
//            Left.Set(position.X, 0f);
//            Top.Set(position.Y, 0f);
//            Width.Set(200f, 0f); // Adjust width as needed
//            Height.Set(200f, 0f); // Adjust height as needed

//            // Initialize background panel
//            backgroundPanel = new UIPanel();
//            backgroundPanel.Width.Set(200f, 0f);
//            backgroundPanel.Height.Set(200f, 0f);
//            backgroundPanel.BackgroundColor = new Color(43, 43, 43) * 0.9f;
//            backgroundPanel.BorderColor = Color.Black;
//            Append(backgroundPanel);

//            // Initialize mod list
//            modList = new UIList();
//            modList.Width.Set(-20f, 1f); // Leave space for scrollbar
//            modList.Height.Set(-20f, 1f); // Leave space for scrollbar
//            modList.Top.Set(10f, 0f);
//            modList.Left.Set(10f, 0f);
//            backgroundPanel.Append(modList);

//            // Initialize scrollbar
//            scrollbar = new UIScrollbar();
//            scrollbar.SetView(25f, 100f);
//            scrollbar.Height.Set(-20f, 1f);
//            scrollbar.Top.Set(10f, 0f);
//            scrollbar.Left.Set(-25f, 1f);
//            modList.SetScrollbar(scrollbar);
//            backgroundPanel.Append(scrollbar);

//            // Populate mod list with checkboxes and labels
//            foreach (var mod in mods)
//            {
//                var modCheckbox = new UIModCheckbox(mod, this);
//                modList.Add(modCheckbox);
//                SelectedMods[mod] = false; // Default to unselected
//            }
//        }

//        // Method to retrieve selected mods
//        public List<string> GetSelectedMods()
//        {
//            return SelectedMods.Where(kv => kv.Value).Select(kv => kv.Key).ToList();
//        }

//        // Optional: Override DrawSelf for custom rendering
//        protected override void DrawSelf(SpriteBatch spriteBatch)
//        {
//            base.DrawSelf(spriteBatch);
//            // Additional drawing code if necessary
//        }
//    }
//}
