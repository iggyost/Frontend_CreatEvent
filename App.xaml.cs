using Frontend_CreatEvent.ApplicationData;
using Frontend_CreatEvent.Views.Content;

namespace Frontend_CreatEvent;

public partial class App : Application
{
	public static string enteredPhone;
	public static User enteredUser;
    public static string conString = "http://192.168.0.10:45455/api/";
	public static Event selectedEvent;
    public App()
	{
		InitializeComponent();

		MainPage = new WelcomePage();
	}
}
