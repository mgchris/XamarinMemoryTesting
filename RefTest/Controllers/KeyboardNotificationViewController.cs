using System;
using UIKit;
using Foundation;

namespace RefTest
{
    public class KeyboardNotificationViewController : BaseViewController
    {
        UITextView textView;
        NSObject obs1, obs2;
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            textView = new UITextView (View.Frame){
                TextColor = UIColor.Black,
                Font = UIFont.FromName ("Arial", 18f),
                BackgroundColor = UIColor.White,
                Text = "This is a textView",
                ReturnKeyType = UIReturnKeyType.Default,
                KeyboardType = UIKeyboardType.Default,
                ScrollEnabled = true,
                AutoresizingMask = UIViewAutoresizing.FlexibleHeight,
            };
            View.AddSubview(textView);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            if (obs1 == null)
            {
                // ObjC code is creating the retain of this object, we just need to make sure we remove the Actions 
                obs1 = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, n =>
                    {
                        Console.WriteLine("Keyboard Will show. This ref: " + this);

                        NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Done, 
                            (s, e) => textView.ResignFirstResponder()); 
                    });

                obs2 = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, 
                            n => Console.WriteLine("Keyboard Will Hide. This ref: " + this));
            }
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            if (obs1 != null)
            {
                // We are being good and removing them
                NSNotificationCenter.DefaultCenter.RemoveObserver (obs1);
                NSNotificationCenter.DefaultCenter.RemoveObserver (obs2);
                NavigationItem.RightBarButtonItem = null;  // NOTE: Got to get rid of the reference
            }
        }
    }
}


