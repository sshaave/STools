using Genius3D.Commands;
using Genius3D.Interfaces;
using Genius3D.UtilitiesBasic;
using Genius3D.UtilitiesDependent;
using KUtilities;
using System.Windows;

namespace STools.PageSTools.Buttons
{
	public class BarItemBtnRunAnalysis : BarItemBtn, IPlugin, ICWithBarPos
	{
		public BarItemBtnRunAnalysis()
		{
			m_strCaption = "Run analysis";
			m_strTooltip = "Runs the selected analysis one time";
			m_Parents.Add(new UBarItemPositionData("Default|STools.PageSTools.RibbonPageSTools|STools.PageSTools.Groups.RibbonGroupOther", 2, false));
			m_ItemStyle = ITEMSTYLE.Large;
		}

		public override void OnDoCommand(object sender, UEventArgs args)
		{
			var cmdRun = new MCmdAnalysisPerform();
			cmdRun.Analysis = UAppData.Application.ActiveController.ActiveAnalysis;
			var success = UAppData.Application.ActiveController.Do(cmdRun);
			if(success)
				MessageBox.Show("It twirks! The results can be seen in the result view now");


			/* I denne funksjonen vil jeg lage 10000 testtilfeller til det "ordinære" nevrale nettverket.
			 * Brukes for å finne det fasit-svar på hvilket profil som er det beste.
			 * Finnes det et eksempel på hvordan å definere ett segment med 2 opplagerbetingelser, påført egenvekt, nyttelast, knekklengder osv?
			 *  
			 */



			/* I en annen funksjon har jeg lyst til å bruke focus til å evaluere ett segment
			 * eller en hel fagverkskonstruksjon. Dette skal bli brukt i en RL-løkke (reinforcement learning) da det ikke er mulig å
			 * finne et fasit-svar på hva som er best.
			 * 
			 * Input til Focus:
			 * - liste med knutepunkters koordinater
			 * - liste med navn på profiler
			 * - liste med materialkvaliteter (pga HUP og I-profiler leveres nesten alltid som hhv S420 og S355) 
			 * 
			 * Output: 
			 * - Kapasitetsutnyttelse (enten globalt eller pr objekt)
			 */

		}
	}
}
