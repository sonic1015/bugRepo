using System.Collections.Generic;
using Xamarin.Forms;

namespace bugRepo
{
    public class FlashingTriggerAction : TriggerAction<VisualElement>
    {
        public uint Duration { get; set; } = 1000;

        protected override void Invoke(VisualElement sender)
        {
            new Animation {
                { 0.0, 0.5, new Animation(v => sender.Opacity = v)},
                {0.5, 1.0, new Animation(v => sender.Opacity = 1.0 - v) },
            }.Commit(sender, "FlashingAnimation", 16U, Duration, Easing.Linear, null, () => true);
        }
    }
}