using Frontend_CreatEvent.ApplicationData;
using Newtonsoft.Json;

namespace Frontend_CreatEvent.Views.Content;

public partial class RequestsPage : ContentPage
{
	public RequestsPage()
	{
		InitializeComponent();
	}

    private async void SwipeItem_Invoked(object sender, EventArgs e)
    {
        activInd.IsRunning = true;
        SwipeItem swipeItem = (SwipeItem)sender;
        var itemId = int.Parse(swipeItem.AutomationId);

        HttpClient client = new HttpClient();
        var response = await client.DeleteAsync($"{App.conString}requestsview/delete/{itemId}");
        if (response.IsSuccessStatusCode)
        {
            HttpClient getclient = new HttpClient();
            HttpResponseMessage getresponse = await getclient.GetAsync($"{App.conString}requestsview/get/{App.enteredUser.UserId}");
            if (getresponse.IsSuccessStatusCode)
            {
                string getcontent = await getresponse.Content.ReadAsStringAsync();
                var getdata = JsonConvert.DeserializeObject<RequestsView[]>(getcontent);
                requestsCv.ItemsSource = getdata.ToList();
            }
        }
        else
        {
            await DisplayAlert("Ошибка!", "Ошибка при удалении запроса!", "Закрыть");
        }
        activInd.IsRunning = false;
    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
		try
		{
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"{App.conString}requestsview/get/{App.enteredUser.UserId}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<RequestsView[]>(content);
                requestsCv.ItemsSource = data.ToList();
            }
        }
		catch (Exception)
		{

		}
    }

    private async void refRequest_Refreshing(object sender, EventArgs e)
    {
        refRequest.IsRefreshing = true;
        try
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"{App.conString}requestsview/get/{App.enteredUser.UserId}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<RequestsView[]>(content);
                requestsCv.ItemsSource = data.ToList();
            }
        }
        catch (Exception)
        {

        }
        refRequest.IsRefreshing = false;
    }
}