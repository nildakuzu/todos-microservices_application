namespace UserManagementService.Api.Response.Models
{
    public class LoginResponseModel
    {
        public LoginResponseModel(string loginStatus)
        {
            LoginStatus = loginStatus ?? "Login is succeed;";
        }
        public LoginResponseModel()
        {

        }

        public string UserName { get; set; }

        public string Token { get; set; }

        public string LoginStatus { get; set; }
    }
}
