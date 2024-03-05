using Frontend_CreatEvent.Views.Content;

namespace Frontend_CreatEvent;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        Routing.RegisterRoute("Home", typeof(HomePage));
    }
}
