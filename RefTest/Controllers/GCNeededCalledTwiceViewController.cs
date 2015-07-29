
using System;
using UIKit;
using CoreGraphics;

namespace RefTest
{
    public partial class GCNeededCalledTwiceViewController : BaseViewController
    {
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (animated == true)
            {
                var weak = new WeakReference(this);

                var center = View.Center;
                var size = new CGSize(300, 30);
                var button = new UIButton(new CGRect(center.X - size.Width / 2, center.Y, size.Width, size.Height));
                button.SetTitle("Have to call GC twice, after clicking me", UIControlState.Normal);
                button.TouchDown += (s, e) =>
                    {
                        var self = weak.Target as GCNeededCalledTwiceViewController;
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

            var alertController = UIAlertController.Create("Alert", "This Will now need GC called twice", UIAlertControllerStyle.Alert);

            // NOTE: Even thought this is creating a strong retain, somewhere it is getting cleared up, hit Collect Garabage twice and this object will be disposed
            alertController.AddAction(UIAlertAction.Create("Done", UIAlertActionStyle.Cancel, (UIAlertAction alert) => Console.WriteLine("Reference To this: " + this)));

            PresentViewController(alertController, true, null);
        }
    }
}

