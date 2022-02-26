using Genius3D.Commands;
using Genius3D.Interfaces;
using Genius3D.UtilitiesBasic;
using Genius3D.UtilitiesDependent;
using KUtilities;
using System.Windows;

namespace STools.PageSTools.Buttons
{
	public class BarItemBtnMoveJoint : BarItemBtn, IPlugin, ICWithBarPos
	{
		public BarItemBtnMoveJoint()
		{
			m_strCaption = "Move the first joint";
			m_strTooltip = "This button moves the first joint in the model, if any";
			m_Parents.Add(new UBarItemPositionData("Default|STools.PageSTools.RibbonPageSTools|STools.PageSTools.Groups.RibbonGroupOther", 2, false));
			m_ItemStyle = ITEMSTYLE.Large;
		}

		public override void OnDoCommand(object sender, UEventArgs args)
		{
			var controller = UAppData.Application.ActiveController;
			if(controller == null)
				return;

			var cmdModelListGet = new MCmdModelListGet("Joints"); // Can also be: Segments, Edges, Shells, Loads, Masses, BoundaryConditions etc.
			controller.Do(cmdModelListGet);
			var joints = cmdModelListGet.ModelList as IUList;
			var firstjoint = joints.FirstItem as IMJoint;

				if(firstjoint == null)
					return;
			var cmdCommand = new MCmdObjectsAllGet("Joints");
			controller.Do(cmdCommand);
			IUList segments = cmdCommand.ModelList;
			MessageBox.Show("Joints metode 2: " + segments.Count.ToString());  // -------> "2"

			var cmdMoveJoint = new MCmdJointMove(firstjoint.ID, new UVector3D(0, 0, 10));
			controller.Do(cmdMoveJoint);
			controller.ForceModelUpdate();
		}
	}
}
