using Genius3D.UtilitiesBasic;
using KUtilities;

namespace STools.PageSTools.Groups
{
	public class RibbonGroupOther : RibbonGroup
	{
		public RibbonGroupOther()
		{
			m_strCaption = "Other buttons";
			m_Parents.Add(new UBarItemPositionData("Default|STools.PageSTools.RibbonPageSTools", 2, false));
		}

	}
}
