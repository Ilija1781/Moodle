using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Telekomunikacije.Models
{
    public class IndexViewModel
    {
        
        public string FirstName { get; set; }
        public string Id { get; set; }      
        public string LastName { get; set; }       
        public string IndexNumber { get; set; }
        public DateTime? BirthDay { get; set; }
        public byte[] UserPhoto { get; set; }
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }
  
    public class ChangeProfileViewModel
    {
        [Display(Name ="Ime")]
        [Required(ErrorMessage ="Polje Predvidjeno Za Ime Ne Moze Biti Prazno")]
        public string FirstName { get; set; }
        public string Id { get; set; }

        [Display(Name ="Prezime")]
        [Required(ErrorMessage = "Polje Predvidjeno Za Prezime Ne Moze Biti Prazno")]
        public string LastName { get; set; }

        [Display(Name ="Broj Indeksa")]
        public string IndexNumber { get; set; }

        [Display(Name ="Datum Rodjenja")]
        public DateTime? BirthDay { get; set; }

        [Display(Name ="Profilna Slika")]
        public byte[] UserPhoto { get; set; }

        [Display(Name ="Broj Telefona")]
        public string PhoneNumber { get; set; }
    }
    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Trenutna Lozinka")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        [Display(Name = "Nova Lozinka")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potvrditi Lozinku")]
        [Compare("NewPassword", ErrorMessage = "Lozinke Se Ne Poklapaju")]
        public string ConfirmPassword { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }
}