namespace SyncoStronbo.Pages;

public partial class Main : ContentPage
{
	public Main()
	{
		InitializeComponent();
	}

    private async void btnCreateClicked(object sender, EventArgs e){
        await Shell.Current.GoToAsync("Create");
    }


	private void btnEnterClicked(object sender, EventArgs e){

	}
}