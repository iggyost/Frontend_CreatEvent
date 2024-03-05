using Frontend_CreatEvent.ApplicationData;
using Newtonsoft.Json;

namespace Frontend_CreatEvent.Views.Content;

public partial class PasswordEnterPage : ContentPage
{
	public PasswordEnterPage()
	{
		InitializeComponent();
	}

    private async void passwordEntry_Completed(object sender, EventArgs e)
    {
        HttpClient client = new HttpClient();

        HttpResponseMessage response = await client.GetAsync($"{App.conString}users/login/{App.enteredPhone}/{passwordEntry.Text}");

        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<User>(content);
            App.enteredUser = data;
            Application.Current.MainPage = new AppShell();
        }
        else
        {
            await DisplayAlert("Ошибка!", "Неправильный пароль!", "Закрыть");
        }
    }
}