using Frontend_CreatEvent.ApplicationData;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace Frontend_CreatEvent.Views.Content;

public partial class EventOrderPage : ContentPage
{
    public static int categorySelected;
    public static CartsView[] cartsViewsData = new CartsView[] { };
    public EventOrderPage()
    {
        InitializeComponent();
    }

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//Home");
    }

    private async void OnDeleteSwipeItemInvoked(object sender, EventArgs e)
    {
        SwipeItem swipeItem = (SwipeItem)sender;
        var itemId = int.Parse(swipeItem.AutomationId);

        HttpClient client = new HttpClient();
        var response = await client.DeleteAsync($"{App.conString}cartsdish/delete/{itemId}");
        if (response.IsSuccessStatusCode)
        {
            HttpClient refrClient = new HttpClient();
            HttpResponseMessage refrResponse = await refrClient.GetAsync($"{App.conString}cartsview/get/{App.enteredUser.UserId}/{App.selectedEvent.EventId}");
            if (refrResponse.IsSuccessStatusCode)
            {
                string content = await refrResponse.Content.ReadAsStringAsync();
                cartsViewsData = JsonConvert.DeserializeObject<CartsView[]>(content);
                cartDishesCv.ItemsSource = cartsViewsData.ToList();
                decimal sum = 0;
                foreach (var item in cartsViewsData)
                {
                    sum = sum + item.TotalCost;
                }
                allCostLbl.Text = sum.ToString();
            }
        }
        else
        {
            await DisplayAlert("Ошибка!", "Ошибка при удалении блюда из корзины!", "Закрыть");
        }
    }

    private async void doOrderBtn_Clicked(object sender, EventArgs e)
    {
        HttpClient client = new HttpClient();
        HttpResponseMessage response = new HttpResponseMessage();

        foreach (var item in cartsViewsData)
        {
            var content = new StringContent(JsonConvert.SerializeObject(item.Id));
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            response = await client.PutAsync($"{App.conString}CartsDish/put/status/{item.Id}", content);
        }
        if (response.IsSuccessStatusCode)
        {
            HttpClient refrClient = new HttpClient();
            HttpResponseMessage refrResponse = await refrClient.GetAsync($"{App.conString}cartsview/get/{App.enteredUser.UserId}/{App.selectedEvent.EventId}");
            if (refrResponse.IsSuccessStatusCode)
            {
                string cartcontent = await refrResponse.Content.ReadAsStringAsync();
                cartsViewsData = JsonConvert.DeserializeObject<CartsView[]>(cartcontent);
                cartDishesCv.ItemsSource = cartsViewsData.ToList();
                decimal sum = 0;
                foreach (var item in cartsViewsData)
                {
                    sum = sum + item.TotalCost;
                }
                allCostLbl.Text = sum.ToString();
            }
            await DisplayAlert("Спасибо!", "Ваша заявка отправлена на рассмотрение менеджером, скоро с вами свяжутся для уточнения деталей", "Закрыть");
        }
    }

    private async void addDishBtn_Clicked(object sender, EventArgs e)
    {
        activityInd.IsRunning = true;
        Button btn = (Button)sender;
        var dishId = int.Parse(btn.AutomationId);

        HttpClient client = new HttpClient();
        var responseCheck = await client.GetAsync($"{App.conString}cartsdish/check/{App.enteredUser.CartId}/{App.selectedEvent.EventId}/{dishId}");
        if (responseCheck.IsSuccessStatusCode)
        {
            CartsDish cartsDish = new CartsDish();
            HttpClient postClient = new HttpClient();
            var postCartsDish = await postClient.PostAsJsonAsync($"{App.conString}cartsdish/add/{App.enteredUser.CartId}/{App.selectedEvent.EventId}/{dishId}/{1}", cartsDish);
            if (postCartsDish.IsSuccessStatusCode)
            {
                HttpClient viewClient = new HttpClient();
                HttpResponseMessage viewResponse = await viewClient.GetAsync($"{App.conString}cartsview/get/{App.enteredUser.UserId}/{App.selectedEvent.EventId}");
                if (viewResponse.IsSuccessStatusCode)
                {
                    string viewContent = await viewResponse.Content.ReadAsStringAsync();
                    cartsViewsData = JsonConvert.DeserializeObject<CartsView[]>(viewContent);
                    cartDishesCv.ItemsSource = cartsViewsData.ToList();
                    decimal sum = 0;
                    foreach (var item in cartsViewsData)
                    {
                        sum = sum + item.TotalCost;
                    }
                    allCostLbl.Text = sum.ToString();
                }
            }
            else
            {
                await DisplayAlert("Ошибка!", "Ошибка при добавлении блюда в корзину!", "Закрыть");
            }
        }
        else
        {
            if (responseCheck.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                await DisplayAlert("Ошибка!", "Этот товар уже есть в корзине!", "Закрыть");
            }
            else
            {
                await DisplayAlert("Ошибка!", "Ошибка сервера. Попробуйте снова", "Закрыть");
            }
        }
        activityInd.IsRunning = false;
    }
    private void categoriesCv_Loaded(object sender, EventArgs e)
    {
        //try
        //{
        //    HttpClient client = new HttpClient();
        //    HttpResponseMessage response = await client.GetAsync($"{App.conString}categories");

        //    if (response.IsSuccessStatusCode)
        //    {
        //        string content = await response.Content.ReadAsStringAsync();
        //        var data = JsonConvert.DeserializeObject<Category[]>(content);
        //        categoriesCv.ItemsSource = data.ToList();

        //    }
        //}
        //catch (Exception)
        //{

        //}
    }

    private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        activityInd.IsRunning = true;
        RadioButton radioButton = sender as RadioButton;
        var btnId = int.Parse(radioButton.AutomationId);
        if (btnId != 0)
        {
            menuCv.ItemsSource = dishesList.Where(x => x.CategoryId == btnId).ToList();
            switch (btnId)
            {
                case 1:
                    categoryNameLbl.Text = "Горячее";
                    break;
                case 2:
                    categoryNameLbl.Text = "Фуршет";
                    break;
                case 3:
                    categoryNameLbl.Text = "Салаты";
                    break;
                case 4:
                    categoryNameLbl.Text = "Напитки";
                    break;
            }
        }
        activityInd.IsRunning = false;
    }

    private void cartDishesCv_Loaded(object sender, EventArgs e)
    {

    }

    public static Dish[] dishesList = new Dish[] { };

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        activityInd.IsRunning = true;
        eventImage.Source = App.selectedEvent.CoverPhoto;
        eventNameLbl.Text = App.selectedEvent.Name;
        try
        {
            HttpClient dishClient = new HttpClient();
            HttpResponseMessage dishResponse = await dishClient.GetAsync($"{App.conString}dishes/get");
            if (dishResponse.IsSuccessStatusCode)
            {
                string dishContent = await dishResponse.Content.ReadAsStringAsync();
                dishesList = JsonConvert.DeserializeObject<Dish[]>(dishContent);
            }

        }
        catch (Exception)
        {

        }
        try
        {
            HttpClient cartsClient = new HttpClient();
            HttpResponseMessage cartsResponse = await cartsClient.GetAsync($"{App.conString}cartsview/get/{App.enteredUser.UserId}/{App.selectedEvent.EventId}");
            if (cartsResponse.IsSuccessStatusCode)
            {
                string cartsContent = await cartsResponse.Content.ReadAsStringAsync();
                cartsViewsData = JsonConvert.DeserializeObject<CartsView[]>(cartsContent);
                cartDishesCv.ItemsSource = cartsViewsData.ToList();
                decimal sum = 0;
                foreach (var item in cartsViewsData)
                {
                    sum = sum + item.TotalCost;
                }
                allCostLbl.Text = sum.ToString();
            }
        }
        catch (Exception)
        {

        }
        try
        {
            HttpClient categoryClient = new HttpClient();
            HttpResponseMessage categoryResponse = await categoryClient.GetAsync($"{App.conString}categories/get");

            if (categoryResponse.IsSuccessStatusCode)
            {
                string categoryContent = await categoryResponse.Content.ReadAsStringAsync();
                var categoryData = JsonConvert.DeserializeObject<Category[]>(categoryContent);
                categoriesCv.ItemsSource = categoryData.ToList();

            }
        }
        catch (Exception)
        {

        }
        activityInd.IsRunning = false;
    }

    private async void minusBtn_Clicked(object sender, EventArgs e)
    {
        ImageButton btn = sender as ImageButton;
        var cartsDishId = int.Parse(btn.AutomationId);
        if (cartsViewsData.Where(x => x.Id == cartsDishId).Select(s => s.Count > 1).FirstOrDefault())
        {
            var selectedCartsDishCount = cartsViewsData.Where(x => x.Id == cartsDishId).Select(s => s.Count).FirstOrDefault();
            HttpClient countClient = new HttpClient();
            HttpResponseMessage countResponse = await countClient.PutAsync($"{App.conString}cartsdish/putcount/{cartsDishId}/{selectedCartsDishCount - 1}", null);
            if (countResponse.IsSuccessStatusCode)
            {
                HttpClient refrClient = new HttpClient();
                HttpResponseMessage refrResponse = await refrClient.GetAsync($"{App.conString}cartsview/get/{App.enteredUser.UserId}/{App.selectedEvent.EventId}");
                if (refrResponse.IsSuccessStatusCode)
                {
                    string content = await refrResponse.Content.ReadAsStringAsync();
                    cartsViewsData = JsonConvert.DeserializeObject<CartsView[]>(content);
                    cartDishesCv.ItemsSource = cartsViewsData.ToList();
                    decimal sum = 0;
                    foreach (var item in cartsViewsData)
                    {
                        sum = sum + item.TotalCost;
                    }
                    allCostLbl.Text = sum.ToString();
                }
            }
        }
    }

    private async void plusBtn_Clicked(object sender, EventArgs e)
    {
        ImageButton btn = sender as ImageButton;
        var cartsDishId = int.Parse(btn.AutomationId);
        if (cartsViewsData.Where(x => x.Id == cartsDishId).Select(s => s.Count < 100).FirstOrDefault())
        {
            var selectedCartsDishCount = cartsViewsData.Where(x => x.Id == cartsDishId).Select(s => s.Count).FirstOrDefault();
            HttpClient countClient = new HttpClient();
            HttpResponseMessage countResponse = await countClient.PutAsync($"{App.conString}cartsdish/putcount/{cartsDishId}/{selectedCartsDishCount + 1}", null);
            if (countResponse.IsSuccessStatusCode)
            {
                HttpClient refrClient = new HttpClient();
                HttpResponseMessage refrResponse = await refrClient.GetAsync($"{App.conString}cartsview/get/{App.enteredUser.UserId}/{App.selectedEvent.EventId}");
                if (refrResponse.IsSuccessStatusCode)
                {
                    string content = await refrResponse.Content.ReadAsStringAsync();
                    cartsViewsData = JsonConvert.DeserializeObject<CartsView[]>(content);
                    cartDishesCv.ItemsSource = cartsViewsData.ToList();
                    decimal sum = 0;
                    foreach (var item in cartsViewsData)
                    {
                        sum = sum + item.TotalCost;
                    }
                    allCostLbl.Text = sum.ToString();
                }
            }
        }
    }
}