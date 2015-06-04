using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using System.Collections.Generic;
using System.Linq;

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
        }
    }

    public class TestItem
    {
        readonly public string Title;
        readonly public Action<UINavigationController> Action;

        public TestItem(string title, Action<UINavigationController> action)
        {
            Title = title;
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
            dataItems.Add(new TestItem("Collect Garabage", nc 
                => GC.Collect()));

            dataItems.Add(new TestItem("Fill Up Memory", nc 
                => nc.PushViewController(new MemoryFillUpViewController(), true)));

            dataItems.Add(new TestItem("Bad Event Handle", nc 
                => nc.PushViewController(new BadEventHandleViewController(), true)));

            dataItems.Add(new TestItem("Good Event Handle", nc 
                => nc.PushViewController(new EventHandleViewController(), true)));

            dataItems.Add(new TestItem("Keyboard Notification", nc 
                => nc.PushViewController(new KeyboardNotificationViewController(), true)));
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

            return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var item = dataItems[indexPath.Row];
            item.Action.Invoke(_navigationController);

            tableView.DeselectRow(indexPath, true);
        }
    }

    public class TestTableViewControllerCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("TestTableViewControllerCell");

        public TestTableViewControllerCell()
            : base(UITableViewCellStyle.Value1, Key)
        {
            // TODO: add subviews to the ContentView, set various colors, etc.
            TextLabel.Text = "TextLabel";
        }
    }
}
