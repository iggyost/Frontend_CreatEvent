using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Frontend_CreatEvent.Views.Content;

public partial class ProfilePage : ContentPage
{
	public ProfilePage()
	{
		InitializeComponent();
		userNameLbl.Text = App.enteredUser.Name;
		userPhoneLbl.Text = App.enteredUser.Phone;
		userEmailLbl.Text = App.enteredUser.Email;
	}

    private async void editNameBtn_Clicked(object sender, EventArgs e)
    {
		string result = await DisplayPromptAsync("Смена имени", "Введите имя пользователя");
		if (result != null)
		{
			HttpClient client = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(result));
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            var response = await client.PutAsync($"{App.conString}users/name/{App.enteredUser.UserId}/{result}",content);
			if (response.IsSuccessStatusCode)
			{
				userNameLbl.Text = result;
			}
		}
    }

    private void leaveBtn_Clicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new WelcomePage();
    }
}