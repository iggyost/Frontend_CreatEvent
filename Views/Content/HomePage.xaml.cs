using Frontend_CreatEvent.ApplicationData;
using Newtonsoft.Json;

namespace Frontend_CreatEvent.Views.Content;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();

    }
    public static Event[] eventData = new Event[] {};

    private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        Border border = sender as Border;
        var idBorder = Convert.ToInt32(border.AutomationId);
        if (idBorder != 0)
        {
            App.selectedEvent = eventData.Where(x => x.EventId == idBorder).FirstOrDefault();
            await Navigation.PushModalAsync(new EventOrderPage());
        }
        else
        {
            await DisplayAlert($"Ошибка", "Мероприятие не найдено!", "Закрыть");
        }
    }

    private void ContentPage_Loaded(object sender, EventArgs e)
    {

    }

    private async void eventsCv_Loaded(object sender, EventArgs e)
    {
        HttpClient client = new HttpClient();

        HttpResponseMessage response = await client.GetAsync($"{App.conString}events/get");

        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            eventData = JsonConvert.DeserializeObject<Event[]>(content);
            eventsCv.ItemsSource = eventData.ToList();
        }
    }

    private async void refreshPage_Refreshing(object sender, EventArgs e)
    {
        refreshPage.IsRefreshing = true;
        HttpClient client = new HttpClient();

        HttpResponseMessage response = await client.GetAsync($"{App.conString}events/get");

        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<Event[]>(content);
            eventsCv.ItemsSource = data.ToList();
            refreshPage.IsRefreshing = false;
        }
    }
}