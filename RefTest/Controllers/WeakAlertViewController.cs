using System;
using UIKit;

namespace RefTest
{
    public class WeakAlertViewController: BaseViewController
    {
        public WeakAlertViewController()
        {
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (animated == true)
            {
                var weak = new WeakReference(this);
                var alertController = UIAlertController.Create("Weak Alert", "Simple Alert, that shows how WeakReference work.", UIAlertControllerStyle.Alert);
                alertController.AddAction(UIAlertAction.Create("Done", UIAlertActionStyle.Cancel, delegate(UIAlertAction alert)
                        {
                            var self = weak.Target as WeakAlertViewController;
                            if (self != null)
                                Console.WriteLine("Reference To this: " + self);
                        }));
                PresentViewController(alertController, true, null);
        
            }
        }
    }
}

