using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using System.Collections.Generic;
using System.Linq;
using RefTest.Controllers;

namespace RefTest
{
    partial class TestTableViewControllerController : UITableViewController
    {
        public TestTableViewControllerController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            TableView.Source = new TestTableViewControllerSource(NavigationController);
            //TableView.RowHeight = UITableView.AutomaticDimension; // Not really working, for some reason
            TableView.RowHeight = 60; 
            TableView.ReloadData();
        }
    }

    public class TestItem
    {
        readonly public string Title;
        readonly public string DetailText;
        readonly public Action<UINavigationController> Action;

        public TestItem(string title, string detailText, Action<UINavigationController> action)
        {
            Title = title;
            DetailText = detailText;
            Action = action;
        }

        public TestItem(string title, Action<UINavigationController> action)
        {
            Title = title;
            DetailText = null;
            Action = action;
        }
    }

    public class TestTableViewControllerSource : UITableViewSource
    {
        UINavigationController _navigationController;
        List<TestItem> dataItems;

        public TestTableViewControllerSource(UINavigationController navigationController)
        {
            _navigationController = navigationController;

            dataItems = new List<TestItem>();
            dataItems.Add(new TestItem("Collect Garabage", "This will cause the garbage collect to run", nc 
                => {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }));

            dataItems.Add(new TestItem("Filler Memory", "Create a bunch of strings that should trigger the Garabage Collector", nc 
                => {

                const int tripGarabageCollector = 10000; // semi random count, that should cause the garabage collector to get called
                for (int index = 0; index < tripGarabageCollector; index++) 
                {
                    CreateRandomString(10);
                }

                new UIAlertView("Finished", "Done Filling Memory", null, "OK", null).Show();
            }));

            dataItems.Add(new TestItem("Bad Event Handle", "This view controller will never get release when created because of event delegates", nc 
                => nc.PushViewController(new BadEventHandleViewController(), true)));

            dataItems.Add(new TestItem("Good Event Handle", "Shows one way of dealing with event delegate", nc 
                => nc.PushViewController(new EventHandleViewController(), true)));

            dataItems.Add(new TestItem("Keyboard Notification", "Shows one way of dealing with notifications", nc 
                => nc.PushViewController(new KeyboardNotificationViewController(), true)));

            dataItems.Add(new TestItem("Weak Alert", "Example of how we can use WeakReference to simplify events", nc 
                => nc.PushViewController(new WeakAlertViewController(), true)));

            dataItems.Add(new TestItem("Weak Reference Bad", "Example of WeakReference usage in code, that results in memory issues", nc 
                => nc.PushViewController(new WeakReferenceBadViewController(), true)));

            dataItems.Add(new TestItem("Confusion", "Example requires GC to be called twice", nc 
                => nc.PushViewController(new GCNeededCalledTwiceViewController(), true)));

            dataItems.Add(new TestItem("Confusion", "Having events on a UITableViewCell", nc 
                => nc.PushViewController(new TableViewControllerCellEvent(), true)));
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return dataItems.Count();
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(TestTableViewControllerCell.Key) as TestTableViewControllerCell;
            if (cell == null)
                cell = new TestTableViewControllerCell();

            var item = dataItems[indexPath.Row];
            cell.TextLabel.Text = item.Title;
            cell.DetailTextLabel.Text = item.DetailText;

            return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var item = dataItems[indexPath.Row];
            item.Action.Invoke(_navigationController);

            tableView.DeselectRow(indexPath, true);
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

    public class TestTableViewControllerCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("TestTableViewControllerCell");

        public TestTableViewControllerCell()
            : base(UITableViewCellStyle.Subtitle, Key)
        {
            TextLabel.Text = "TextLabel";
            DetailTextLabel.Lines = 0;
        }
    }
}
