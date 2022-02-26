using Genius3D.Interfaces;
using Genius3D.UtilitiesBasic;
using KUtilities;

namespace STools.PageSTools
{
	public class RibbonPageSTools : RibbonPage
	{
		public RibbonPageSTools()
		{
			m_strCaption = "Simen tools";
			m_Parents.Add(new UBarItemPositionData("Default", 70, false));
		}

		//protected override void OnIsInDrawingModeChanged(object sender, System.EventArgs args)
		//! Event handler for the <IsInDrawingModeChanged> event of the application
		/**
		 * Event handler for the <IsInDrawingModeChanged> event of the application.
		 *
		 * Input:
		 *  - <sender> : The application that fired the event
		 *  - <args>   : Event arguments. Contains information about the old and the new active controller
		 */
		//{
		//}

        protected override void OnViewModeChanged(object sender, IUObjectChangedEventArgs args)
        {
            throw new System.NotImplementedException();
        }
    }
}
