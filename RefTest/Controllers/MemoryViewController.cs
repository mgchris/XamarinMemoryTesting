using System;
using UIKit;
using System.Collections.Generic;
using System.Linq;

namespace RefTest
{
    public class MemoryFillUpViewController: BaseViewController
    {
        List<string> fillUp;
       
        public MemoryFillUpViewController() 
        {
            fillUp = new List<string>();

            for (int index = 0; index < 100; index++)
            {
                fillUp.Add(CreateRandomString(10));
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            var label = new UILabel();
            label.Text = "Done loading, you can go back now";
            label.Center = View.Center;
            View.AddSubview(label);
        }

        static string CreateRandomString(int size)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                             Enumerable.Repeat(chars, size)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());
            return result;
        }

    }
}

