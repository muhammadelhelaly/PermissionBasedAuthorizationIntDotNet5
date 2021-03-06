using System.ComponentModel.DataAnnotations;

namespace PermissionBasedAuthorizationIntDotNet5.ViewModel
{
    public class RoleFormViewModel
    {
        [Required, StringLength(256)]
        public string Name { get; set; }
    }
}