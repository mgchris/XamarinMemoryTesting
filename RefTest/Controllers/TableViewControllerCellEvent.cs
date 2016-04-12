using System;

using UIKit;
using CoreGraphics;

namespace RefTest.Controllers
{
    public partial class TableViewControllerCellEvent : BaseViewController
    {
        EventHandlerTableViewSource _tableSource;
        UITableView _tableView;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            _tableSource = new EventHandlerTableViewSource();

            _tableView = new UITableView(new CGRect(0, 0, View.Bounds.Width, View.Bounds.Height));
            _tableView.Source = _tableSource;
           
            View.AddSubview(_tableView);

            View.SetNeedsLayout();
        }

        public override void ViewDidDisappear(bool animated) 
        {
            base.ViewDidDisappear(animated);

            _tableSource.LeavingFocus();
        }
    }
}


