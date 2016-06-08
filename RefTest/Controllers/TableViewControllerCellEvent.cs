using System;

using UIKit;
using CoreGraphics;

namespace RefTest.Controllers
{
    public partial class TableViewControllerCellEvent : BaseViewController
    {
        EventHandlerTableViewSource _tableSource;
        CustomTableView _tableView;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            _tableSource = new EventHandlerTableViewSource();

            _tableView = new CustomTableView(new CGRect(0, 0, View.Bounds.Width, View.Bounds.Height));
            _tableView.Source = _tableSource;

            View.AddSubview(_tableView);

            View.SetNeedsLayout();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            _tableSource.CleanUp();
        }
    }

    public class CustomTableView: UITableView 
    {
        private string _uniqueId;
        public string UniqueId
        {
            get
            {
                if (_uniqueId == null)
                {
                    _uniqueId = Log.UniqueToken();
                }
                return _uniqueId;
            }
        }

        public CustomTableView(CGRect rect) : base(rect)
        {
            Log.State(this, UniqueId);
        }

        ~CustomTableView()
        {
            Log.State(this, UniqueId);
        }

        protected override void Dispose(bool disposing)
        {
            Log.State(this, UniqueId);
            base.Dispose(disposing);
        }
    }
}


