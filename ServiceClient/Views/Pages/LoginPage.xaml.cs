using Android.Widget;

namespace ServiceClient.Views.Pages;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

    public async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        string username = txtUsername.Text;
        string password = txtContraseña.Text;

        //if (string.IsNullOrEmpty(validacionLogin(username, password)))
        //{
        //    try
        //    {
        //        // Realizar la solicitud de inicio de sesión a la API
        //        var response = await RN_Users.RN_Login(username, password);
        //
        //        // Verificar si la solicitud fue exitosa
        //        if (response.RerrorCode == GlobalEnum.ErrorCode.SUCCESS.GetHashCode().ToString())//Significa que no hubo error en la peticion
        //        {
        //            // Obtener los datos del usuario de la respuesta
        //
        //
        //            var userData = response.Rbody.FirstOrDefault();
        //            if (userData != null)
        //            {
        //                AccionesSession.LLenarValoresSession(userData);
        //                // Guardar la sesión en un archivo XML
        //                AccionesSession.SaveSession();
        //
        //                // Mostrar un mensaje de éxito
        //                var toastMessage = Toast.Make(response.Rmessage, CommunityToolkit.Maui.Core.ToastDuration.Short, 13);
        //                await toastMessage.Show();
        //
        //                // Redirigir a la página principal de la aplicación
        //                App.Current.MainPage = new AppShell();
        //            }
        //            else
        //            {
        //                await DisplayAlert("Error", response.Rmessage, "OK");
        //            }
        //
        //
        //        }
        //
        //    }
        //    catch (Exception ex)
        //    {
        //        // Mostrar un mensaje de error si ocurre una excepción durante el inicio de sesión
        //        await DisplayAlert("Error", "Ha ocurrido un error en el proceso del login\nDetalle: " + ex.Message, "Ok");
        //    }
        //}
        //else
        //{
        //    // Mostrar un mensaje si los datos de inicio de sesión no son válidos
        //    await DisplayAlert("Alerta", validacionLogin(username, password), "OK");
        //}
    }
    public string validacionLogin(string username, string password)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            return "Los campos no pueden ser vacíos";

        return null;

    }
}