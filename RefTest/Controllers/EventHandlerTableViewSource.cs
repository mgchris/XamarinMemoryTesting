using System;
using UIKit;

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
        public void LeavingFocus()
        {

        }

        public void EnterFocus()
        {

        }


        // --------------------
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _sourceSize;
        }

        public override void CellDisplayingEnded(UITableView tableView, UITableViewCell cell, Foundation.NSIndexPath indexPath)
        {
            var eventCell = cell as EventTableViewCell;
            if (eventCell != null)
            {
                eventCell.SelectedEvent = null;
            }
        }

        public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(EventTableViewCell.Identifier) as EventTableViewCell ?? new EventTableViewCell();


            var overrideButtonEvent = true;

            if (overrideButtonEvent)
            {
                cell.Button.TouchDown -= HandleClickedOverride;
                cell.Button.TouchDown += HandleClickedOverride;
            }
            else
            {
                if (cell.SelectedEvent == null)
                {
                    cell.useButtonHandle(true);
                    cell.SelectedEvent = HandleClicked;
                }
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


    class EventTableViewCell: UITableViewCell 
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

        static public string Identifier = "CellIdentifier";
        public EventHandler<string> SelectedEvent;

        public UIButton Button;

        public EventTableViewCell()
        {
            Log.State(this, UniqueId);

            Button = new UIButton(UIButtonType.InfoDark);
            AddSubview(Button);
        }

        // The idea here is I can play with using the button event or not.
        // If you pass in true then you need to make sure you have SelectedEvent set.
        // Or you can set the button event manually
        public void useButtonHandle(bool use)
        {
            if (use)
            {
                Button.TouchDown += HandleTap;
            }
            else
            {
                Button.TouchDown -= HandleTap;
            }
        }

        public void HandleTap(object sender, EventArgs e)
        {
            SelectedEvent(this, TextLabel.Text);
        }

        ~EventTableViewCell()
        {
            Log.State(this, UniqueId);
        }

        protected override void Dispose (bool disposing)
        {
            Log.State(this, UniqueId);
            base.Dispose (disposing);
        }
    }
}





