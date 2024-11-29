//using Terraria.UI;
//using Terraria.GameContent.UI.Elements;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace TerraLens.Project.UI
//{
//    public class UIModCheckbox : UIElement
//    {
//        public string ModName { get; private set; }
//        private UICheckbox checkbox;
//        private UIText label;
//        private DropdownMenu parentMenu;

//        public bool IsChecked
//        {
//            get => checkbox.IsChecked;
//            set => checkbox.IsChecked = value;
//        }

//        public UIModCheckbox(string modName, DropdownMenu parent)
//        {
//            ModName = modName;
//            parentMenu = parent;

//            Width.Set(0f, 1f); // Fill the parent width
//            Height.Set(25f, 0f); // Fixed height

//            // Initialize checkbox
//            checkbox = new UICheckbox();
//            checkbox.Left.Set(0f, 0f);
//            checkbox.VAlign = 0f; // Top aligned
//            Append(checkbox);

//            // Initialize label
//            label = new UIText(modName, 0.6f, true);
//            label.Left.Set(25f, 0f); // Offset to the right of the checkbox
//            label.VAlign = 0f; // Top aligned
//            Append(label);

//            // Assign event
//            checkbox.OnChange += Checkbox_OnChange;
//        }

//        private void Checkbox_OnChange(bool isChecked, UICheckbox checkbox)
//        {
//            //parentMenu.UpdateSelectedMods(ModName, isChecked);
//        }

//        protected override void DrawSelf(SpriteBatch spriteBatch)
//        {
//            base.DrawSelf(spriteBatch);
//            // Additional drawing code if necessary
//        }
//    }
//}
