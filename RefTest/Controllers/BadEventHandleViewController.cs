using System;
using UIKit;
using CoreGraphics;

namespace RefTest
{
    public class BadEventHandleViewController : BaseViewController
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.Red;

            var center = View.Center;
            var size = new CGSize(300, 30);
            var button = new UIButton(new CGRect(center.X - size.Width / 2, center.Y, size.Width, size.Height));
            button.SetTitle("I am bad", UIControlState.Normal);
            button.TouchDown += HandleTap;  // This is creating a Strong retain, that never will get collected
            View.AddSubview(button);
        }

        public void HandleTap(object sender, EventArgs e)
        {
            new UIAlertView("Alert", "Already did my bad thing", null, "Cancel", null).Show();
        }
    }
}


