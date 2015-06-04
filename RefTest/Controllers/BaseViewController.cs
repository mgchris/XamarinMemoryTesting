using System;
using UIKit;

namespace RefTest
{
    public class BaseViewController: UIViewController
    {
        private string _uniqueId;
        public string UniqueId {
            get {
                if(_uniqueId == null) {
                    _uniqueId = Log.UniqueToken();
                }
                return _uniqueId;
            }
        }

        public BaseViewController() : base(null, null)
        {
            Log.State(this, UniqueId);
        }

        ~BaseViewController()
        {
            Log.State(this, UniqueId);
        }

        protected override void Dispose (bool disposing)
        {
            Log.State(this, UniqueId);
            base.Dispose (disposing);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            View.BackgroundColor = UIColor.Green;
        }
    }
}

