using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SHDocVw;

namespace GraphAnimator
{
	public class HelpBrowser : Form
	{

		public static bool run (string sURL)
		{
			InternetExplorer oIE = (InternetExplorer)COMCreateObject("InternetExplorer.Application");

			if (oIE != null)
			{	
				object oEmpty = String.Empty;
				object oURL= sURL;
				oIE.Visible = true;
				oIE.Navigate2 (ref oURL, ref oEmpty, ref oEmpty, ref oEmpty, ref oEmpty);
			}
			return true;			
		}

		public static object COMCreateObject (string sProgID)
		{
			// We get the type using just the ProgID
			Type oType = Type.GetTypeFromProgID (sProgID);

			if (oType != null)
			{					
				return Activator.CreateInstance(oType);
			}
			

			return null;

		}
	}
}