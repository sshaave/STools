using Genius3D.UtilitiesBasic;
using KUtilities;

namespace STools.PageSTools.Groups
{
	public class RibbonGroupAI : RibbonGroup
	{
		public RibbonGroupAI()
		{
			m_strCaption = "AI group";
			m_Parents.Add(new UBarItemPositionData("Default|STools.PageSTools.RibbonPageSTools", 1, false));
		}
	}
}
