using System;
using UIKit;
using CoreGraphics;

namespace RefTest
{
    public class EventHandleViewController : BaseViewController
    {
        UIButton trickyButton;
        EventHandler trickyEvent;

        UIButton carefulButton;
        EventHandler carefulEvent;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var center = View.Center;
            var size = new CGSize(300, 30);
            const float padding = 20;
            var button = new UIButton(new CGRect(center.X - size.Width / 2, center.Y, size.Width, size.Height));
            button.SetTitle("Nothing to Worry about", UIControlState.Normal);

            // Does not create a Strong retain to 'this' directly or indirectly
            button.TouchDown += ((sender, e) => new UIAlertView("Alert", "All Good!", null, "Cancel", null).Show()); 

            View.AddSubview(button);


            trickyButton = new UIButton(new CGRect(center.X - size.Width / 2, button.Center.Y + padding, size.Width, size.Height));
            trickyButton.SetTitle("I am tricky", UIControlState.Normal);
            View.AddSubview(trickyButton);


            carefulButton = new UIButton(new CGRect(center.X - size.Width / 2, trickyButton.Center.Y + padding, size.Width, size.Height));
            carefulButton.SetTitle("Be careful", UIControlState.Normal);
            View.AddSubview(carefulButton);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (trickyEvent == null)
            {
                // This event is creating a strong retain of this
                trickyEvent = (sender, e) => {
                    new UIAlertView("Alert", "Tricky Tricky!", null, "Cancel", null).Show();
                    Console.WriteLine("Reference To this: " + this);
                };
                trickyButton.TouchDown += trickyEvent;

                // This is indirectly creating a strong retain to this.  Basicaly this.HandleTap
                carefulEvent = HandleTap;
                carefulButton.TouchDown += carefulEvent;
            }
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            // We remove the events so this object can get released
            if (trickyEvent != null)
            {
                trickyButton.TouchDown -= trickyEvent;
                trickyEvent = null;

                carefulButton.TouchDown -= carefulEvent;
                carefulEvent = null;
            }
        }

        public void HandleTap(object sender, EventArgs e) 
        {
            new UIAlertView("Alert", "Why so careful?", null, "Cancel", null).Show();
        }
    }
}

