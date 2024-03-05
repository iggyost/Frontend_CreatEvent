using System.Text.RegularExpressions;

namespace Frontend_CreatEvent.Views.Content;

public partial class PhoneEnterPage : ContentPage
{
	public PhoneEnterPage()
	{
		InitializeComponent();
	}

    private async void phoneEntry_Completed(object sender, EventArgs e)
    {
        var regex = new Regex("^((\\+7|7|8)+([0-9]){10})$");
        if (regex.IsMatch(phoneEntry.Text))
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"{App.conString}users/phone/{phoneEntry.Text}");

            if (response.IsSuccessStatusCode)
            {
                App.enteredPhone = phoneEntry.Text;
                Application.Current.MainPage = new PasswordEnterPage();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                App.enteredPhone = phoneEntry.Text;
                Application.Current.MainPage = new PasswordEnterPage();
            }

        }
        else
        {
            await DisplayAlert("Ошибка!","Неправильный формат номера!","Закрыть");
        }

    }
}