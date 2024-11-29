using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TerraLens.Project.UI
{
    public class FilterButton : UIElement
    {
        private UIPanel buttonPanel;
        private UIText buttonText;
        private Action onClick;

        public FilterButton(Action onClickAction)
        {
            onClick = onClickAction;

            // Set the size of the FilterButton itself
            Width.Set(80f, 0f); // Set width
            Height.Set(30f, 0f); // Set height

            // Initialize the button panel
            buttonPanel = new UIPanel();
            buttonPanel.Width.Set(0f, 1f); // Fill the parent width
            buttonPanel.Height.Set(0f, 1f); // Fill the parent height
            buttonPanel.BackgroundColor = new Color(73, 94, 171) * 0.7f;
            buttonPanel.BorderColor = Color.Black;
            Append(buttonPanel);

            // Add text to the button
            buttonText = new UIText("Filter", 0.6f, true);
            buttonText.HAlign = 0.5f;
            buttonText.VAlign = 0.5f;
            buttonPanel.Append(buttonText);

            // Assign click event
            buttonPanel.OnLeftClick += FilterButtonClicked;
        }

        private void FilterButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            onClick?.Invoke();
        }
    }
}
