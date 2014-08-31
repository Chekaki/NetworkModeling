using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace NM.Domain.Entities {
    public class Node {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите имя узла")]
        [DisplayName("Название")]
        public string Name { get; set; }
    }
}