using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace TerraLens.Project.UI
{
    public class ExtendedUIPanel : UIPanel
    {
        // Reference to the titlePanel
        public UIPanel TitlePanel { get; set; }

        public override bool ContainsPoint(Vector2 point)
        {
            // Check if the point is within the mainPanel's bounds
            bool withinMainPanel = base.ContainsPoint(point);

            // Additionally, check if the point is within the titlePanel's bounds
            if (TitlePanel != null)
            {
                CalculatedStyle titleDimensions = TitlePanel.GetOuterDimensions();
                if (titleDimensions.X < point.X && point.X < titleDimensions.X + titleDimensions.Width &&
                    titleDimensions.Y < point.Y && point.Y < titleDimensions.Y + titleDimensions.Height)
                {
                    withinMainPanel = true;
                }
            }

            return withinMainPanel;
        }
    }
}
