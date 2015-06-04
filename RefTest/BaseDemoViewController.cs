using System;
using UIKit;

namespace RefTest
{
    

    public class BaseDemoViewController : UIViewController
    {
        public BaseDemoViewController() : base(null, null)
        {
            string.LOG(this);
        }

        public virtual ~BaseDemoViewController()
        {
            Console.WriteLine("~MemoryViewController: " + this.GetHashCode ());
        }
    }


}

