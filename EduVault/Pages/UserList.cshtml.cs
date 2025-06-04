using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EduVault.Models;
using EduVault.Data;

namespace EduVault.Pages
{
	//[Authorize]
	public class UserListModel : PageModel
	{
        public List<User> Users;
        public UserListModel()
        {

        }
	}
}
