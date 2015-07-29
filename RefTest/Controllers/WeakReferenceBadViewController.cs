using System;
using UIKit;
using CoreGraphics;

namespace RefTest
{
    public partial class WeakReferenceBadViewController : BaseViewController
    {
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (animated == true)
            {
                var center = View.Center;
                var size = new CGSize(300, 30);
                var button = new UIButton(new CGRect(center.X - size.Width / 2, center.Y, size.Width, size.Height));
                button.SetTitle("Display Weak Alert", UIControlState.Normal);

                // This will retain
                var self = new WeakReference(this).Target as WeakReferenceBadViewController;
                button.TouchDown += (s, e) =>
                    {
                        if (self != null)
                            self.HandleTap(s, e);
                        else
                            System.Diagnostics.Debug.WriteLine("WeakAlertViewController doesn't exist");
                    };
                View.AddSubview(button);
            }
        }

        public void HandleTap(object sender, EventArgs e)
        {
            var alertController = UIAlertController.Create("Weak Alert", "Simple Alert, that shows how WeakReference work.", UIAlertControllerStyle.Alert);
            alertController.AddAction(UIAlertAction.Create("Done", UIAlertActionStyle.Cancel, (UIAlertAction alert) => Console.WriteLine("Reference To WeakReferenceConfusionViewController")));
            PresentViewController(alertController, true, null);
        }
    }
}

