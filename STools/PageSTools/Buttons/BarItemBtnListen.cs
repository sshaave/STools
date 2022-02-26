using Genius3D.Commands;
using Genius3D.Framework;
using Genius3D.Interfaces;
using Genius3D.UtilitiesBasic;
using Genius3D.UtilitiesDependent;
using KUtilities;
using System.Collections;
using System.IO;
using System.Windows;

namespace STools.PageSTools.Buttons
{
	public class BarItemBtnListen : BarItemBtn, IPlugin, ICWithBarPos
	{
	

		public BarItemBtnListen()
		{
			m_strCaption = "Lytt";
			m_strTooltip = "Lytt for endring";
			m_Parents.Add(new UBarItemPositionData("Default|STools.PageSTools.RibbonPageSTools|STools.PageSTools.Groups.RibbonGroupAI", 1, false));
			m_ItemStyle = ITEMSTYLE.Large;
		}

		private void OnChanged(object source, FileSystemEventArgs e)
		{

			var controller = UAppData.Application.ActiveController;
			if (controller == null)
			{
				MessageBox.Show("Controller = null");
				return;
			}
			
			var cmdModelListGet = new MCmdObjectsAllGet("Segments");
			controller.Do(cmdModelListGet);
			IUList segments = cmdModelListGet.ModelList;

			MessageBox.Show("Antal segmenter: " + segments.Count.ToString());    // ------> "Antall segmenter: 10"
			/*
			foreach (IMSegment temp in segments)
            {
				var cmdSegmentUpdate = new MCmdSegmentCrossSectSet(temp.ID, null, false);
				controller.Do(cmdSegmentUpdate);
				controller.ForceModelUpdate();
			}
			*/
			
			MessageBox.Show("Før IUHashTable");
			IUHashTable hashtableExtdata = controller.ExtensionData;
			
			foreach (DictionaryEntry kvp in hashtableExtdata)
				MessageBox.Show("Key: " + kvp.Key.ToString() + "Value: " + kvp.Value.ToString());

			FSectionViewInfo cmdTverrsnitt = (FSectionViewInfo)controller.ExtensionData["SectionViewInfos"];
			MessageBox.Show("Etter cast");

			MessageBox.Show(cmdTverrsnitt.ToString());      // ----------> "System.Collections.Generic.List'1[Genius3D.Framework.FSectionViewInfo]"
	
			
			
			// IUList Xsec = controller.Model.AvailableCrossSects;
			var cmdProfilesListGet = new MCmdObjectsAllGet("AvailableCrossSects");
			controller.Do(cmdProfilesListGet);
			IUList profiles = cmdProfilesListGet.ModelList;
			//MessageBox.Show(profiles.FirstItem.ToString());

			MessageBox.Show(profiles.Count.ToString());        // ---------->  0 med MCmcObjects... "CrossSections" og "AvailableCrossSects"

			var activeAnalysis = controller.ActiveAnalysis;
			if (!activeAnalysis.CanCompute)
			{
				MessageBox.Show("Cannot compute");
				return;
			}

			var cmdRun = new MCmdAnalysisPerform();

			cmdRun.Analysis = UAppData.Application.ActiveController.ActiveAnalysis;
			var success = UAppData.Application.ActiveController.Do(cmdRun);

			if (success)
            {
				MessageBox.Show("Success");
				var max_deformation = activeAnalysis.ResultsModel.Summary.Displacement.Translation.GetLength();
				MessageBox.Show(max_deformation.ToString());

				string path = @"C:\output\Example.txt";

				if (!File.Exists(path))
				{
					File.Create(path).Dispose();

					using (TextWriter tw = new StreamWriter(path, false))
					{
						tw.WriteLine("The first line");
					}

				}
				else if (File.Exists(path))
				{


					/*     
					var firstSegment = segments.FirstItem as IMSegment; <------- Skal erstattes via geometrimodell eller at python vet l
					IMRefLine refLine = firstSegment.RefLine;
					double lengde = refLine.GetLength();
					



					var results = activeAnalysis.ResultsModel as IAResultsModel;
					var resultsSegments = results.ResultsSegment as Dictionary<UID, IAResultsSegment>;
					
					foreach(KeyValuePair<UID, IAResultsSegment> entry in resultsSegments)
                    {
						IDDesignResultSegment tempEntry = entry.Value.DesignCheckResults;
						
						IDDesignResultSection tempKap = tempEntry.WorstResult; // <---------- Denne virker ikke
						MessageBox.Show("Før tempWorst");
						IDDesignResult tempWorst = tempKap.WorstResult;
						MessageBox.Show("Før twfm");
						double tempWorstForMember = tempWorst.Value;
						MessageBox.Show(tempWorstForMember.ToString());

						
						IDDesignResultSection[] tempKap = tempEntry.Results;
						foreach(IDDesignResult i in tempKap)    <------------ Virker heller ikke i vektor-format
                        {
							MessageBox.Show("Før double j");
							double j = i.Value;
							MessageBox.Show(j.ToString());

                        }
						
					} 
					*/

					// >>>> eksempel på hvordan å hente ut kapasitetsutnyttelse fra et segment

					double egenvekt = activeAnalysis.ResultsModel.GetMinResultants()[2]; // enhet: N 

					using (TextWriter tw = new StreamWriter(path, true))
					{

						tw.Write("Egenvekt fagverk: " + egenvekt.ToString().Substring(0,8) + "N, maks deformasjon: " + (1000*max_deformation).ToString() +"mm\n");
						// -----> printer ut egenvekt i N og maks deformasjon i mm + linjeskift
						//tw.WriteLine(max_deformation + " " + lengde + " " + kapasitetVektor.toString() + " " + egenvekt);
					}
				}
			}
			else if (!success)
			{
				MessageBox.Show("Not success");
            }
		}


		public override void OnDoCommand(object sender, UEventArgs args)
		{
			MessageBox.Show("Lytter");
			var fileSystemWatcher = new FileSystemWatcher(@"C:\testfolder\")
            {
				Filter = "*.*",
				NotifyFilter = NotifyFilters.LastWrite,
				EnableRaisingEvents = true,
			};

			fileSystemWatcher.Changed += OnChanged;

        }

        public void AILayers(double length, double permLast, double varLast)
        {
			// Her må jeg laste lage klasse for neuroner og aktiveringsfunksjoner som leser verdier fra .csv eller at jeg taster inne noe manuelt.
			// Eventuelt dra inn et python-skript, men jeg er usikker på hele prosessen med kompilering osv. 
			return;
        }
	}
}
