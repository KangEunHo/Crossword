namespace HealingJam.Popups
{
    public delegate void PopupClosedDelegate(string message);

    public class CallbackPopup : Popup
    {
        private PopupClosedDelegate popupClosedDelegate = null;

        public override void Enter(params object[] args)
        {
            base.Enter(args);

            if (args != null && args.Length > 0)
                popupClosedDelegate = args[0] as PopupClosedDelegate;
            else
                popupClosedDelegate = null;
        }

        public override void Exit(params object[] args)
        {
            base.Exit(args);

            string message = null;
            if (args != null && args.Length > 0)
                message = args[0] as string;

            popupClosedDelegate?.Invoke(message);
        }
    }
}