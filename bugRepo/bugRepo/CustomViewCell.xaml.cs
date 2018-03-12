using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace bugRepo
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CustomViewCell : ViewCell
	{
		public CustomViewCell ()
		{
			InitializeComponent ();

            var menuItem1 = new MenuItem{Text = "Redfish", IsDestructive = true};
            var menuItem2 = new MenuItem {Text = "Bluefish"};

		    menuItem1.Clicked += (sender, args) =>
		    {
		        ContextActions.Remove(menuItem1);
                ContextActions.Add(menuItem2);
		    };

		    menuItem2.Clicked += (sender, args) =>
		    {
                ContextActions.Remove(menuItem2);
                ContextActions.Add(menuItem1);
		    };

            ContextActions.Add(menuItem2);
		}
	}
}