using System;
using UIKit;
using Foundation;

namespace RefTest.Controllers
{
    public class EventHandlerTableViewSource: UITableViewSource
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

        string[] _displayCells;
        const int _sourceSize = 1024;

        event EventHandler<string> SelectedEvent;

        public EventHandlerTableViewSource()
        {
            Log.State(this, UniqueId);

            _displayCells = new string[_sourceSize];

            for (int i = 0; i < _sourceSize; i++)
            {
                _displayCells[i] = "Cell: " + i;
            }
        }
         
        ~EventHandlerTableViewSource()
        {
            Log.State(this, UniqueId);
        }
            
        protected override void Dispose (bool disposing)
        {
            Log.State(this, UniqueId);
            base.Dispose (disposing);
        }

        // --------------------
        public void LeavingFocus(UITableView tableView)
        {
//            return;


            for (int i = 0; i < _sourceSize; i++)
            {
                var cell = tableView.CellAt(NSIndexPath.Create(0, i));
                if (cell != null)
                {
                    var eventCell = cell as EventTableViewCell;
                    if (eventCell != null)
                    {
                        eventCell.SelectedEvent -= HandleClicked;
                        eventCell.Dispose();
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        public void EnterFocus()
        {

        }


        // --------------------
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _sourceSize;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = null;

            const bool overrideButtonEvent = false;

            if (overrideButtonEvent)
            {
                var buttonCell = tableView.DequeueReusableCell(ButtonTableViewCell.Key) as ButtonTableViewCell ?? new ButtonTableViewCell();

                buttonCell.Button.TouchDown -= HandleClickedOverride;
                buttonCell.Button.TouchDown += HandleClickedOverride;

                cell = buttonCell;
            }
            else
            {
                var eventCell = tableView.DequeueReusableCell(EventTableViewCell.Key) as EventTableViewCell ?? new EventTableViewCell();

                eventCell.SelectedEvent -= HandleClicked;
                eventCell.SelectedEvent += HandleClicked;

                cell = eventCell;
            }

            cell.TextLabel.Text = _displayCells[indexPath.Row];
           
            return cell;
        }

        public void HandleClickedOverride(object sender, EventArgs text)  
        {
            Console.WriteLine("Did override TouchDown: " + sender + ": With args: " + text);
        }

        public void HandleClicked(object sender, string text)  
        {
            Console.WriteLine("Did use SelectedEvent: " + sender + ": With args: " + text);
        }
    }

    class BaseTableViewCell: UITableViewCell 
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

        public BaseTableViewCell(UITableViewCellStyle style, string key) : base(style, key)
        {
        }

        ~BaseTableViewCell()
        {
            Log.State(this, UniqueId);
        }

        protected override void Dispose (bool disposing)
        {
            Log.State(this, UniqueId);
            base.Dispose (disposing);
        }
    }

    class ButtonTableViewCell: BaseTableViewCell 
    {
        public static readonly NSString Key = new NSString("buttonCellIdent");

        public UIButton Button;

        public ButtonTableViewCell() : base(UITableViewCellStyle.Subtitle, Key)
        {
            Log.State(this, UniqueId);

            Button = new UIButton(UIButtonType.InfoDark);
            AddSubview(Button);
        }
    }


    class EventTableViewCell: BaseTableViewCell 
    {
        public static readonly NSString Key = new NSString("EventCellIdent");

        public event EventHandler<String> SelectedEvent;

        public UIButton Button;

        public EventTableViewCell() : base(UITableViewCellStyle.Subtitle, Key)
        {
            Log.State(this, UniqueId);

            Button = new UIButton(UIButtonType.InfoDark);
            Button.TouchDown += HandleTap;
            AddSubview(Button);
        }

        public void HandleTap(object sender, EventArgs e)
        {
            SelectedEvent(this, TextLabel.Text);
        }

        protected override void Dispose (bool disposing)
        {
            Button.TouchDown -= HandleTap; // Note we have to clean up the events we created
            base.Dispose (disposing);
        }
    }
}





