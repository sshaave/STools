using CrossSectionPlugin;
using Genius3D.Commands;
using Genius3D.Framework;
using Genius3D.Interfaces;
using Genius3D.Model;
using Genius3D.UtilitiesBasic;
using Genius3D.UtilitiesDependent;
using KUtilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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

			//    >>>>>        READ TEXTFILE FROM RL-ALGORITHM             <<<<<

			int counter = 0;
			string readPath = @"C:\testfolder\utskrift.txt";
			List<string[]> readInfo = new List<string[]>();

			foreach (string line in File.ReadLines(readPath))
			{
				//Console.WriteLine(line);
				string[] par = line.Split(' ');
				counter++;
				readInfo.Add(par);
			}

			string[][] pI = readInfo.ToArray();						// profileInfo
			List<CrossSection> pL = new List<CrossSection>();       // profileList

			NumberFormatInfo provider = new NumberFormatInfo();
			provider.NumberDecimalSeparator = ".";
			for (int i = 0; i < counter; i++)
			{

				// Check if I-profile
				if (Int32.Parse(pI[i][0]) == 0)
				{
					// is I-profile
					IMCrossSectionI tempI = new CrossSectionIBeam();
					tempI.H = Convert.ToDouble(pI[i][2], provider) / 10e2; tempI.B = Convert.ToDouble(pI[i][3], provider) / 10e2;
					tempI.S = Convert.ToDouble(pI[i][4], provider) / 10e2; tempI.T = Convert.ToDouble(pI[i][5], provider) / 10e2;
					tempI.R = Convert.ToDouble(pI[i][6], provider) / 10e2;

					var temp = tempI as CrossSection;
					temp.CrossSectTypeID = 100; temp.ComputeAutomaticCrsParams = false;
					temp.Name = pI[i][1]; temp.Area = Convert.ToDouble(pI[i][7], provider) / 10e5;
					temp.Cw = Convert.ToDouble(pI[i][9], provider) / 10e17;	temp.Ix = Convert.ToDouble(pI[i][8], provider) / 10e11;
					temp.Riy = Convert.ToDouble(pI[i][10], provider) / 10e2; temp.Iy = Convert.ToDouble(pI[i][11], provider) / 10e11;
					temp.Riz = Convert.ToDouble(pI[i][12], provider) / 10e2; temp.Iz = Convert.ToDouble(pI[i][13], provider) / 10e11;
					temp.Sy = Convert.ToDouble(pI[i][14], provider) / 10e8; temp.Wy = Convert.ToDouble(pI[i][15], provider) / 10e8;
					temp.Wz = Convert.ToDouble(pI[i][16], provider) / 10e8;
					pL.Add(temp);
				}
				else
                {
					// is HUP
					CrossSectionKFHUP tempI = new CrossSectionKFHUP(); tempI.Name = pI[i][1];
					tempI.H = Convert.ToDouble(pI[i][2], provider) / 10e2; tempI.B = Convert.ToDouble(pI[i][3], provider) / 10e2;
					tempI.T = Convert.ToDouble(pI[i][4], provider) / 10e2; tempI.Area = Convert.ToDouble(pI[i][5], provider) / 10e5;
					tempI.Ix = Convert.ToDouble(pI[i][6], provider) / 10e11; tempI.Riy = Convert.ToDouble(pI[i][7], provider) / 10e2;
					tempI.Iy = Convert.ToDouble(pI[i][8], provider) / 10e11; tempI.Riz = Convert.ToDouble(pI[i][9], provider) / 10e2;
					tempI.Iz = Convert.ToDouble(pI[i][10], provider) / 10e11; tempI.Wpy = Convert.ToDouble(pI[i][11], provider) / 10e8;
					tempI.Wpz = Convert.ToDouble(pI[i][12], provider) / 10e8; tempI.Wy = Convert.ToDouble(pI[i][13], provider) / 10e8;
					tempI.Wz = Convert.ToDouble(pI[i][14], provider) / 10e8;
					var temp = tempI as CrossSection;
					pL.Add(temp);
				}
			}

			//        >>>>>     Change the cross sections       <<<<<

			var cmdModelListGet = new MCmdObjectsAllGet("Segments");
			controller.Do(cmdModelListGet);
			IUList segments = cmdModelListGet.ModelList;

			//MessageBox.Show("Antal segmenter: " + segments.Count.ToString());    // ------> "Antall segmenter: 10"

			List<IMCrossSection> Xsections = new List<IMCrossSection>();
			foreach (IMSegment temp in segments)
            {
				Xsections.Add(temp.CrossSect1);
			}

			// Lager 3 grupper. Gruppe 1 er overgurt, 2 er undergurt, og 3 er stavene
			int j = 0;
			foreach (IMSegment temp in segments)
            {
				if (j < 2)
				{
					var cmdSegmentUpdate = new MCmdSegmentCrossSectSet(temp.ID, pL[j], false);
					controller.Do(cmdSegmentUpdate);
				}
				else
                {
					var cmdSegmentUpdate = new MCmdSegmentCrossSectSet(temp.ID, pL[2], false);
					controller.Do(cmdSegmentUpdate);
				}
				j++;
            }

			//          >>>>>     Run the analysis           <<<<<

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
				

				string writePath = @"C:\output\Example.txt";

				if (!File.Exists(writePath))
				{
					File.Create(writePath).Dispose();

					using (TextWriter tw = new StreamWriter(writePath, false))
					{
						tw.WriteLine("The first line");
					}

				}
				else if (File.Exists(writePath))
				{

					//    >>>> Fetch utilization from each segment   <<<<
					List<string> util = new List<string>();
					var results = activeAnalysis.ResultsModel as IAResultsModel;
					var resultsSegments = results.ResultsSegment as Dictionary<UID, IAResultsSegment>;
					var en = new CultureInfo("en-US");
					
					foreach(KeyValuePair<UID, IAResultsSegment> entry in resultsSegments)
                    {
						entry.Value.ComputeDesignResults();
						IDDesignResultSegment tempEntry = entry.Value.DesignCheckResults;
						IDDesignResult tempKap = tempEntry.WorstResult.WorstResult; // <---------- Denne virker ikke
						util.Add(tempKap.Value.ToString("0.00", en));
					} 
					

					// >>>> Write output to text file   <<<<

					double egenvekt = activeAnalysis.ResultsModel.GetMinResultants()[2]; // enhet: N 

					using (TextWriter tw = new StreamWriter(writePath, false))
					{
						var jk = activeAnalysis.ResultsModel.ResultsSegment;
						// Egenvekt fagverk N -  maks deformasjon mm - util pr seg
						tw.Write(egenvekt.ToString("0.00", en) + " " + (1000 * max_deformation).ToString("0.00", en));
						foreach (string strUtil in util)
                        {
							tw.Write(" " + strUtil);
                        }
						tw.Write("\n");
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
			// MessageBox.Show("Lytter");
			var fileSystemWatcher = new FileSystemWatcher(@"C:\testfolder\")
            {
				Filter = "*.*",
				NotifyFilter = NotifyFilters.LastWrite,
				EnableRaisingEvents = true,
			};

			fileSystemWatcher.Changed += OnChanged;

        }
	}
}
