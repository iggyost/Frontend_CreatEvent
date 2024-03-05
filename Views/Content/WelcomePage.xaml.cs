namespace Frontend_CreatEvent.Views.Content;

public partial class WelcomePage : ContentPage
{
	public WelcomePage()
	{
		InitializeComponent();
	}

    private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
		Application.Current.MainPage = new PhoneEnterPage();
    }
}